var webpack = require('webpack');
var pathConfig = require('../config/path');
var CleanWebpackPlugin = require('clean-webpack-plugin');
var WebpackBuildNotifierPlugin = require('webpack-build-notifier');
var Visualizer = require('webpack-visualizer-plugin');

var cssRootPath = pathConfig.cssPaths.build;
var jsRootPath = pathConfig.scriptPaths.build;

function generatePlugins(environment) {
    var commonPlugins = [
        new CleanWebpackPlugin([cssRootPath], {
            root: process.cwd(),
            exclude: ['fonts']
        }),
        new CleanWebpackPlugin([jsRootPath], {
            root: process.cwd()
        }),
        new webpack.IgnorePlugin(/^\.\/locale$/, /moment$/),
        new webpack.ProvidePlugin({
            $: "jquery",
            jQuery: "jquery",
            jquery: 'jquery',
            'window.$': 'jquery',
            "window.jQuery": 'jquery',
            "root.jQuery": 'jquery'
        }),
         new WebpackBuildNotifierPlugin({
             title: "New UI Webpack Build",
             suppressSuccess: true
        }),
        new Visualizer({
            filename: './wwwroot/webpack-stats-newui.html'
        })
    ];

    return commonPlugins;
}

module.exports = generatePlugins;