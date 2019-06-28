import { appendEvent, scheduleEvent } from "./flows";
import { declareTopic } from "./topics";
import { declareQuery } from "./queries";
import { declareWebRequest } from "./web";

export default {
  append: appendEvent,
  schedule: scheduleEvent,
  topic: declareTopic,
  query: declareQuery,
  webRequest: declareWebRequest
};