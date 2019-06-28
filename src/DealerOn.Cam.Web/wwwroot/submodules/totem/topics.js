import { appendEvent, FlowType, FlowScope } from "./flows";

export function declareTopic(declaration) {
  FlowType.declare(new TopicType(declaration));
}

class TopicType extends FlowType {
  constructor(declaration) {
    super(declaration);
  }

  openScope(id) {
    return new TopicScope(this, id);
  }
}

class TopicScope extends FlowScope {
  constructor(type, id) {
    super(type, id);
  }

  async observe(e, observation) {
    try {
      let { flow } = this;
      let newEvents = [];

      let then = (type, data) =>
        newEvents.push({ type, data });

      then.schedule = (whenOccurs, type, data) =>
        newEvents.push({ whenOccurs, type, data });

      then.done = (type, data) => {
        if(type) {
          then(type, data);
        }

        this.done = true;
      };

      await Promise.resolve(observation.method.call(flow, e, then));

      for(let { whenOccurs, type, data } of newEvents) {
        if(whenOccurs) {
          scheduleEvent(whenOccurs, e.$position, type, data);
        }
        else {
          appendEvent(e.$position, type, data);
        }
      }

      this.onObserved();
    }
    catch(error) {
      this.onObserveFailed(e, observation, error);
    }
  }
}