var path = require("path");
var webpack = require("webpack");
var ExtractTextPlugin = require("extract-text-webpack-plugin");
var CopyWebpackPlugin = require("copy-webpack-plugin");
const { VueLoaderPlugin } = require('vue-loader');
const VuetifyLoaderPlugin = require('vuetify-loader/lib/plugin');

module.exports = {
  mode: 'development',
  context: path.resolve(__dirname, "src"),
  entry: {
    //"app": "./app.jsx",
    //"camVue": "./camVue/main.js"
    "app": "./main.js"
  },
  resolve: {
    extensions: ["*", ".js", ".jsx", ".vue", ".json"],
    alias: {
      'vue$': 'vue/dist/vue.esm.js',
      totem: path.resolve(__dirname, "submodules/totem"),
      "totem-react": path.resolve(__dirname, "submodules/totem-react")
    }
  },
  output: {
		path: path.join(__dirname, "dist"),
		filename: "js/[name].js"
  },
  devtool: "source-map",
	plugins: [
    new VueLoaderPlugin(),
		new ExtractTextPlugin("css/[name].css"),
    new CopyWebpackPlugin([{ from: "images", to: "images" }]),
    new webpack.ProvidePlugin({
      $: "jquery",
      jQuery: "jquery",
      "window.jQuery": "jquery"
    }),
    new VuetifyLoaderPlugin()
	],
  module: {
    rules: [
      {
        test: /\.vue$/,
        loader: "vue-loader",
        options: {
          loaders: {
            // Since sass-loader (weirdly) has SCSS as its default parse mode, we map
            // the "scss" and "sass" values for the lang attribute to the right configs here.
            // other preprocessors should work out of the box, no loader config like this necessary.
            styl: "vue-style-loader!css-loader!stylus-loader",
            stylus: "vue-style-loader!css-loader!stylus-loader"
          }
          // other vue-loader options go here
        }
      },
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
        test: /\.styl$/,
        use: [
          {
            loader: "style-loader" // creates style nodes from JS strings
          },
          {
            loader: "css-loader" // translates CSS into CommonJS
          },
          {
            loader: "stylus-loader" // compiles Stylus to CSS
          }
        ]
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