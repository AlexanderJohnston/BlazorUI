import Timeline from "../submodules/totem/timeline";

export default {
  status: Timeline.webRequest("statusLoaded", "/api"),
  regions: Timeline.webRequest("regionsLoaded", "/api/regions"),
  dealerDetails: Timeline.webRequest("dealerDetailsLoaded", args => args.link)
};