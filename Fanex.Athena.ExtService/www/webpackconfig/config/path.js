var path = require('path');

var projectRoot = path.resolve(__dirname, '../../../');

var cssPaths = {
    build: "wwwroot/css",

    main: ["./www/css/source/main.scss"],
    error: ["./www/css/source/pages/utility/error.scss"],
    um: ["./www/css/source/pages/utility/um.scss"],
    betlistLayout: ["./www/css/source/pages/betlist/betlistcommon.main.scss"],
    betlistRefno: ["./www/css/source/pages/betlist/refno.scss"],
    systemparlaydetail: ["./www/css/source/pages/betlist/systemparlaydetail.scss"],
    cashouthistory: ["./www/css/source/pages/betlist/cashouthistory.main.scss", "./www/css/source/extensions/popup-ex.scss"]
};

var scriptPaths = {
    build: "wwwroot/js",
    language: "common/languages/ViewResources.js",
    languageCN: "common/languages/ViewResources.zh-CN.js",
    languageTW: "common/languages/ViewResources.zh-TW.js",
    languageHans: "common/languages/ViewResources.zh-Hans.js",
    languageVN: "common/languages/ViewResources.vi-VN.js",
    betlist_refno: "./www/js/source/pages/betlist/refno.js"
};

module.exports = {
    projectRoot: projectRoot,
    cssPaths: cssPaths,
    scriptPaths: scriptPaths
};