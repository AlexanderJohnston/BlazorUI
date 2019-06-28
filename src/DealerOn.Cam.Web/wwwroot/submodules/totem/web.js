import { HubConnectionBuilder } from "@aspnet/signalr";
import { appendEvent } from "./flows";
import Http from "./http";

let types = new Set();

export function declareWebRequest(loadedEventType, options) {
  let type = new WebRequestType(loadedEventType, options);

  types.add(type);

  return {
    bind: (args, notify) => type.bind(args, notify)
  };
}

//
// A type of request for data from the web
//

class WebRequestType {
  constructor(loadedEventType, options) {
    this.loadedEventType = loadedEventType;
    this.options = options;
  }

  bind(args, notify) {
    return new WebRequestBinding(this, args, notify);
  }

  resolveOptions(args) {
    return new WebRequestOptions(this.options, args);
  }

  appendLoaded(data) {
    appendEvent(null, this.loadedEventType, data);
  }
}

//
// Fetches and possibly observes changes in a web resource
//

class WebRequestBinding {
  constructor(type, args, notify) {
    this.type = type;
    this.args = args;
    this.notify = notify;

    this.options = this.resolveOptions();
    this.client = new WebClient(this.options);
  }

  get data() {
    return this.client.data;
  }

  resolveOptions() {
    return this.type.resolveOptions(this.args);
  }

  subscribe() {
    this.client.subscribe(this);
  }

  resubscribeIfArgsChanged() {
    let current = this.options;
    let next = this.resolveOptions();

    if(next.shouldResubscribe(current)) {
      this.options = next;

      this.client.resubscribe(next);

      this.notify();
    }
  }

  unsubscribe() {
    this.client.unsubscribe(this);
  }
}

//
// Options declared for GET requests to a web resource
//

class WebRequestOptions {
  constructor(typeOptions, args) {
    if(typeof typeOptions === "function") {
      if(typeof args === "function") {
        args = args();
      }

      typeOptions = typeOptions(args);
    }

    switch(typeof typeOptions) {
      case "string":
        this.isUrlOnly = true;
        this.url = typeOptions;
        this.object = { url: typeOptions };
        break;
      case "object":
        this.isUrlOnly = false;
        this.url = typeOptions ? typeOptions.url : null;
        this.object = typeOptions;
        break;
      default:
        this.url = null;
        break;
    }

    if(!this.url) {
      throw new Error("Options must include a URL at minimum");
    }
  }

  shouldResubscribe(current) {
    return !current || (this.isUrlOnly && current.isUrlOnly && this.url !== current.url);
  }
}

//
// Client lifecycle
//

let clientsByKey = new Map();

function addClientByKey(client) {
  let clients = clientsByKey.get(client.key);

  if(!clients) {
    clients = new Set();

    clientsByKey.set(client.key, clients);
  }

  clients.add(client);
}

function deleteClient(client) {
  if(!client.key) {
    return;
  }

  let clients = clientsByKey.get(client.key);

  if(clients) {
    clients.delete(client);

    if(clients.size === 0) {
      clientsByKey.delete(client.key);
    }
  }
}

function loadClients(key) {
  let clients = clientsByKey.get(key);

  if(clients) {
    for(let client of clients) {
      client.loadAsync();
    }
  }
}

//
// Fetches a web resource and notifies bindings of changes
//

class WebClient {
  bindings = new Set();

  constructor(options) {
    this.options = options;

    this.state = new WebClientState(this);

    this.data = this.state.toData();
  }

  get key() {
    return this.state.etag ? this.state.etag.key : null;
  }

  notify() {
    this.data = this.state.toData();

    for(let binding of this.bindings) {
      binding.notify();
    }
  }

  subscribe(binding) {
    this.bindings.add(binding);

    if(this.bindings.size === 1) {
      this.loadAsync();
    }
  }

  resubscribe(options) {
    this.options = options;

    this.loadAsync();
  }

  unsubscribe(binding) {
    this.bindings.delete(binding);

    if(this.bindings.size === 0) {
      this.unsubscribeAsync();

      deleteClient(this);
    }
  }

  async loadAsync() {
    if(this.state.loading) {
      return;
    }

    this.state.setLoading();

    try {
      let { request, data } = await Http.getJson(this.options.url, this.options.object);

      this.state.setLoaded(request, data);

      for(let binding of this.bindings) {
        binding.type.appendLoaded(data);
      }

      this.trySubscribeAsync();
    }
    catch({ request, status, error }) {
      this.state.setLoadError(request, error);

      if(status === 404) {
        this.trySubscribeAsync();
      }
    }
  }

  async trySubscribeAsync() {
    let { subscribing, subscribed, etag } = this.state;

    if(subscribing || subscribed || !etag) {
      return;
    }

    addClientByKey(this);

    this.state.setSubscribing();

    try {
      await subscribeToChanged(etag);

      this.state.setSubscribed();
    }
    catch(error) {
      this.state.setSubscribeError(error);
    }
  }

  async unsubscribeAsync() {
    if(this.state.etag) {
      try {
        await unsubscribeFromChanged(this.state.etag);
      }
      catch(error) {
        // Nowhere to go
      }
    }
  }
}

//
// The observable state of a WebClient
//

class WebClientState {
  loading = false;
  loaded = false;
  loadError = null;
  subscribing = false;
  subscribed = false;
  subscribeError = null;
  etag = null;
  data = {};

  constructor(client) {
    this.client = client;
  }

  toData() {
    let data = Object.assign({}, this);

    delete data.client;

    return data;
  }

  setLoading() {
    this.loading = true;

    this.client.notify();
  }

  setLoaded(request, data) {
    this.loading = false;
    this.loaded = true;
    this.loadError = null;
    this.etag = QueryETag.tryFromResponse(request);
    this.data = data;

    this.client.notify();
  }

  setLoadError(request, error) {
    this.loading = false;
    this.loaded = false;
    this.loadError = error;
    this.etag = QueryETag.tryFromResponse(request);
    this.data = {};

    this.client.notify();
  }

  setSubscribing() {
    this.subscribing = true;

    this.client.notify();
  }

  setSubscribed() {
    this.subscribing = false;
    this.subscribed = true;
    this.subscribeError = null;

    this.client.notify();
  }

  setSubscribeError(error) {
    this.subscribing = false;
    this.subscribed = false;
    this.subscribeError = error;

    this.client.notify();
  }
}

//
// Hub connection
//

let connection = null;
let connectionStart = null;

async function subscribeToChanged(etag) {
  await ensureConnected();

  await connection.invoke("SubscribeToChanged", etag.toString());
}

async function unsubscribeFromChanged(etag) {
  await ensureConnected();

  await connection.invoke("UnsubscribeFromChanged", etag.key);

  await stopIfNoClients();
}

async function ensureConnected() {
  if(connection) {
    return connectionStart;
  }

  connection = new HubConnectionBuilder().withUrl("/hubs/query").build();

  connection.on("onChanged", etag => {
    let parsedETag = QueryETag.tryFrom(etag);

    if(parsedETag) {
      loadClients(parsedETag.key);
    }
  });

  connectionStart = connection.start();

  return connectionStart;
}

async function stopIfNoClients() {
  if(clientsByUrl.size === 0) {
    try {
      await connection.stop();
    }
    finally {
      connection = null;
      connectionStart = null;
    }
  }
}

//
// The key and position of a server query
//

class QueryETag {
  constructor(key, checkpoint) {
    this.key = key;
    this.checkpoint = checkpoint;
  }

  toString() {
    return this.checkpoint === null ? this.key : `${this.key}@${this.checkpoint}`;
  }

  static tryFrom(value) {
    if(value) {
      let separatorIndex = value.indexOf("@");

      if(separatorIndex == -1) {
        return new QueryETag(value, null);
      }

      if(separatorIndex > 0 && separatorIndex < value.length - 1) {
        let key = value.substring(0, separatorIndex);
        let checkpoint = parseInt(value.substring(separatorIndex + 1));

        if(Number.isInteger(checkpoint)) {
          return new QueryETag(key, checkpoint);
        }
      }
    }

    return null;
  }

  static tryFromResponse(request) {
    return QueryETag.tryFrom(request.getResponseHeader("ETag"));
  }
}