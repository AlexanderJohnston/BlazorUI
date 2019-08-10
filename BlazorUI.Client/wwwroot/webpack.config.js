var path = require("path");
var webpack = require("webpack");
var ExtractTextPlugin = require("extract-text-webpack-plugin");
var CopyWebpackPlugin = require("copy-webpack-plugin");

module.exports = {
  context: path.resolve(__dirname, "src"),
  entry: {
    "app": "./app.jsx"
  },
  resolve: {
    extensions: ["*", ".js", ".jsx"]
  },
  output: {
		path: path.join(__dirname, "dist"),
		filename: "js/[name].js"
  },
  devtool: "source-map",
	plugins: [
		new ExtractTextPlugin("css/[name].css"),
    new webpack.ProvidePlugin({
      $: "jquery",
      jQuery: "jquery",
      "window.jQuery": "jquery"
    })
	],
  module: {
    rules: [
      {
        test: /\.jsx?$/,
        loader: "babel-loader",
        exclude: /node_modules/,
        query: {
          plugins: ["transform-class-properties", "transform-runtime"],
          presets: ["env", "react"]
        }
      },
			{
				test: /\.css$/,
        loader: ExtractTextPlugin.extract({
          fallback: "style-loader",
          use: ["css-loader"]
        })
			},
			{
				test: /\.html$/,
				loader: "file-loader?name=[name].[ext]"
			},
			{
				test: /\.(woff|woff2|ttf|eot|svg)/,
				loader: "file-loader?name=/fonts/[name].[ext]"
			},
			{
				test: /\.json$/,
				loader: "json-loader"
			}
    ]
  }
};