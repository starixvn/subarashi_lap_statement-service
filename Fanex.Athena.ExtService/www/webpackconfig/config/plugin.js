var webpack = require('webpack');
var pathConfig = require('../config/path');
const { CleanWebpackPlugin } = require('clean-webpack-plugin');
var WebpackBuildNotifierPlugin = require('webpack-build-notifier');

var cssRootPath = pathConfig.cssPaths.build;
var jsRootPath = pathConfig.scriptPaths.build;

function generatePlugins(environment) {
    var commonPlugins = [
        new CleanWebpackPlugin({
            cleanOnceBeforeBuildPatterns: [`${cssRootPath}/**/*`],
            verbose: true,
            dry: false,
            dangerouslyAllowCleanPatternsOutsideProject: false,
        }),
        new CleanWebpackPlugin({
            cleanOnceBeforeBuildPatterns: [`${jsRootPath}/**/*`],
            verbose: true,
            dry: false,
            dangerouslyAllowCleanPatternsOutsideProject: false,
        }),
        new webpack.IgnorePlugin({ resourceRegExp: /^\.\/locale$/, contextRegExp: /moment$/ }),
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
        })
    ];

    return commonPlugins;
}

module.exports = generatePlugins;