import "./css/reset.css";
import "./css/app.css";
import React from "react";
import ReactDOM from "react-dom";
import QueryHub from "totem-timeline-signalr";
import Cam from "./cam.jsx";

$(document).ready(() => {
  QueryHub.enable();

  let app = document.createElement("div");

  document.body.appendChild(app);

  app.id = "app";

  ReactDOM.render(<Cam />, app);
});