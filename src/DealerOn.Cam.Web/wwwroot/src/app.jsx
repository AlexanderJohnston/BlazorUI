import "./css/reset.css";
import "./css/app.css";
import React from "react";
import ReactDOM from "react-dom";
import Cam from "./cam.jsx";

$(document).ready(() => {
  let app = document.createElement("div");

  document.body.appendChild(app);

  app.id = "app";

  ReactDOM.render(<Cam />, app);
});