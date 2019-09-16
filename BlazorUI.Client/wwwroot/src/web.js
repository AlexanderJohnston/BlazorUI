import Timeline from "totem-timeline";

export default {
  status: Timeline.webRequest("statusLoaded", "/api"),
  regions: Timeline.webRequest("regionsLoaded", "/api/regions"),
  dealerDetails: Timeline.webRequest("dealerDetailsLoaded", args => args.link),
  assets: Timeline.webRequest("assetsLoaded", "/api/assets")
};