var webpack = require("webpack");
var config = require("./webpack.config");

config.plugins.push(new webpack.DefinePlugin({
  ENVIRONMENT: JSON.stringify("Production")
}));

// React will show show an error if you just minify it without setting the environment to production
// https://stackoverflow.com/questions/30030031/passing-environment-dependent-variables-in-webpack
config.plugins.push(new webpack.DefinePlugin({
  "process.env": {
    "NODE_ENV": JSON.stringify("production")
  }
}));

config.output = {
  path: path.join(__dirname, "build"),
  filename: `js/[name].${buildId}.js`
}

config.module.loaders.push({
  test: /\.html$/,
  loader: "string-replace",
  query: {
    search: ".js",
    replace: `.${buildId}.js`
  }
});

module.exports = config;