var webpack = require("webpack");
var config = require("./webpack.config");

config.plugins.push(new webpack.DefinePlugin({
  ENVIRONMENT: JSON.stringify("QA")
}));

module.exports = config;