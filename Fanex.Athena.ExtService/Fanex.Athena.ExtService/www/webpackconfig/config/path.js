var path = require('path');

var projectRoot = path.resolve(__dirname, '../../../');

var cssPaths = {
    build: "wwwroot/css/build",

    home: ["./www/css/source/pages/home/home.scss"],
    main: ["./www/css/source/main.scss"],
    error: ["./www/css/source/pages/utility/error.scss"],
    um: ["./www/css/source/pages/utility/um.scss"],

    reportjqgridlayout: ["./www/css/source/pages/common/reportjqgridlayout.main.scss"],
    betlistLayout: ["./www/css/source/pages/betlist/betlistcommon.main.scss"],
    betlistSimpleLayout: ["./www/css/source/pages/betlist/betlistcommon.main.scss", "./www/css/source/extensions/popup-ex.scss"],

    colossusbetdetail: ["./www/css/source/pages/betlist/colossusbetdetail.scss"],
    systemparlaydetail: ["./www/css/source/pages/betlist/systemparlaydetail.scss"],

    betlistResult: ["./www/css/source/pages/betlist/result/betlist.result.scss"],

    mixparlay: ["./www/css/source/pages/totalbet/mixparlay.scss"],
    result: ["./www/css/source/pages/report/result.scss"],
    cashouthistory: ["./www/css/source/pages/betlist/cashouthistory.main.scss", "./www/css/source/extensions/popup-ex.scss"],
    
};

var scriptPaths = {
    build: "wwwroot/js/build",

    language: "common/languages/ViewResources.js",
    languageCN: "common/languages/ViewResources.zh-CN.js", 
    languageTW: "common/languages/ViewResources.zh-TW.js", 
    languageHans: "common/languages/ViewResources.zh-Hans.js",

    betlist: "./www/js/source/pages/betlist/betlist.js",
    betlistsimple: "./www/js/source/pages/betlist/main-simple.js",
    betlistrunning: "./www/js/source/pages/betlist/main-betlistrunning.js",
    betlistrunningmultiplebettypes: "./www/js/source/pages/betlist/main-betlistrunning.multiplebettypes.js",

    result: "./www/js/source/pages/report/result/main-result.js",

};

module.exports = {
    projectRoot: projectRoot,
    cssPaths: cssPaths,
    scriptPaths: scriptPaths
};