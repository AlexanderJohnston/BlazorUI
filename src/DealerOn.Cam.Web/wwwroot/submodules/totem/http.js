function sendAjax(options) {
  return new Promise((resolve, reject) =>
    $.ajax(options).then(
      (data, statusText, request) =>
        resolve({ request, statusText, status: request.status, data }),
      (request, statusText, error) =>
        reject({ request, statusText, status: request.status, error })));
}

function send(method, url, options) {
  options = options || {};
  options.headers = options.headers || {};

  options.method = method;
  options.url = url;

  return sendAjax(options);
}

function sendJson(method, url, options) {
  options = options || {};
  options.headers = options.headers || {};

  options.method = method;
  options.url = url;

  if(options.content) {
    options.data = JSON.stringify(options.content, null, 2);
  }

  const jsonType = "application/json; charset=utf-8";

  if(options.data) {
    options.headers["Content-Type"] = jsonType;
  }

  options.headers["Accept"] = jsonType;

  return sendAjax(options).then(({ request, status, data }) => {
    return { request, status, data: !data ? data : JSON.parse(data) };
  });
}

export default {
  send,
  sendJson,

  get: (url, options) => send("GET", url, options),
  put: (url, options) => send("PUT", url, options),
  post: (url, options) => send("POST", url, options),
  delete: (url, options) => send("DELETE", url, options),

  getJson: (url, options) => sendJson("GET", url, options),
  putJson: (url, options) => sendJson("PUT", url, options),
  postJson: (url, options) => sendJson("POST", url, options),
  deleteJson: (url, options) => sendJson("DELETE", url, options)
}