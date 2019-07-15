var path = require('path');
var alias = require('../config/aliasReact');
var pathConfig = require('../config/pathReact');
var pluginGenerator = require('../config/pluginReact');
const UglifyJsPlugin = require("uglifyjs-webpack-plugin");

function generateConfig(environment) {
    var jsPath = `${pathConfig.scriptPaths.build}/${environment}/reactpages`;
    var cssPath = `${pathConfig.cssPaths.build}/${environment}/reactpages`;
    var commonConfigs = {
        resolve: {
            alias: alias,
            extensions: ['.js', '.jsx']
        },
        entry: {
            /*js*/
            
            [`${jsPath}/lib/langHelper`]: pathConfig.scriptPaths.languageResource,
            [`${jsPath}/lib/lang-zh-CN`]: pathConfig.scriptPaths.langCN,
            [`${jsPath}/lib/lang-zh-Hans`]: pathConfig.scriptPaths.langHans,
            [`${jsPath}/lib/lang-zh-TW`]: pathConfig.scriptPaths.langTW,

            [`${jsPath}/bettingticketscenter/multicustomer`]: pathConfig.scriptPaths.bettingticketscenter_multicustomer,
            [`${jsPath}/bettingticketscenter/singlecustomer`]: pathConfig.scriptPaths.bettingticketscenter_singlecustomer,
            [`${jsPath}/bettingticketscenter/refno`]: pathConfig.scriptPaths.bettingticketscenter_refno,
            [`${jsPath}/bettingticketscenter/running`]: pathConfig.scriptPaths.bettingticketscenter_running,

            /*css*/
            [`${cssPath}/vendor`]: pathConfig.cssPaths.layout,
            [`${cssPath}/bettingticketscenter/index`]: pathConfig.cssPaths.bettingticketscenter,
            [`${cssPath}/betlist/index`]: pathConfig.cssPaths.betlist,
        },
        output: {
            path: pathConfig.projectRoot,
            filename: "[name].chunk.js",
            chunkFilename: jsPath + '/lib/[name].chunk.js',
        },
        module: {
            rules: [{
                test: /\.(js|jsx)$/,
                loader: ["babel-loader"],
                exclude: /node_modules/
            }]
        },
        plugins: pluginGenerator(environment),
        stats: {
            assets: false,
            colors: true,
            version: false,
            hash: false,
            timings: true,
            chunks: false,
            chunkModules: false,
            children: true
        },
        node: {
            fs: 'empty'
        },
        externals: [
            { './jszip': 'jszip' },
            { './ods': 'ods' },
            { './cptable': 'var cptable' }
        ],
        optimization: {
            minimizer: [
             new UglifyJsPlugin({
                 sourceMap: true
             })
            ],
            splitChunks: {
                chunks: 'all',
                minSize: 30000,
                maxSize: 0,
                minChunks: 2,
                maxAsyncRequests: 5,
                maxInitialRequests: 3,
                name: false,
                cacheGroups: {
                    // disable default cache group
                    default: false,
                    vendors: false,
                    common: {
                        name: 'common',
                        test: /(node_modules)/,
                        minChunks: 2,
                        priority: -10
                    }
                }
            }
        }
    };

    return commonConfigs;
}

module.exports = generateConfig;