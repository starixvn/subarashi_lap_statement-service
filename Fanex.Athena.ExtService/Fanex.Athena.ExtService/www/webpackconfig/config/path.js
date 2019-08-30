var path = require('path');

var projectRoot = path.resolve(__dirname, '../../../');

var cssPaths = {
    build: "wwwroot/css/build",

    main: ["./www/css/source/main.scss"],
    error: ["./www/css/source/pages/utility/error.scss"],
    um: ["./www/css/source/pages/utility/um.scss"],
    betlistLayout: ["./www/css/source/pages/betlist/betlistcommon.main.scss"],
    betlistRefno: ["./www/css/source/pages/betlist/refno.scss"]

};

var scriptPaths = {
    build: "wwwroot/js/build",
    language: "common/languages/ViewResources.js",
    languageCN: "common/languages/ViewResources.zh-CN.js",
    languageTW: "common/languages/ViewResources.zh-TW.js",
    languageHans: "common/languages/ViewResources.zh-Hans.js",
    betlist: "./www/js/source/pages/betlist/betlist.js"

};

module.exports = {
    projectRoot: projectRoot,
    cssPaths: cssPaths,
    scriptPaths: scriptPaths
};