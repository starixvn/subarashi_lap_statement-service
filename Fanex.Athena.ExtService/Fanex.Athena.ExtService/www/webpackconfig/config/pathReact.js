var path = require('path');

var projectRoot = path.resolve(__dirname, '../../../');

var cssPaths = {
    build: "wwwroot/css/build",
    layout: "./www/css/source/reactpages/main.scss",
   
    bettingticketscenter: "./www/css/source/reactpages/pages/bettingticketscenter/index.scss",
    betlist: "./www/css/source/reactpages/pages/betlist/index.scss",
};

var scriptPaths = {
    build: "wwwroot/js/build",

    languageResource: ["./www/js/source/reactpages/lib/langHelper.js",
        "./www/js/source/common/languages/ViewResources.js",
    ],
    langCN: "./www/js/source/common/languages/ViewResources.zh-CN.js",
    langHans: "./www/js/source/common/languages/ViewResources.zh-Hans.js",
    langTW: "./www/js/source/common/languages/ViewResources.zh-TW.js",
    /*React page*/
   
    bettingticketscenter_multicustomer: "./www/js/source/reactpages/pages/bettingticketscenter/multicustomer/components/index.jsx",
    bettingticketscenter_singlecustomer: "./www/js/source/reactpages/pages/bettingticketscenter/singlecustomer/components/index.jsx",
    bettingticketscenter_refno: "./www/js/source/reactpages/pages/bettingticketscenter/refno/components/index.jsx",
    bettingticketscenter_running: "./www/js/source/reactpages/pages/bettingticketscenter/running/components/index.jsx",
};
module.exports = {
    projectRoot: projectRoot,
    cssPaths: cssPaths,
    scriptPaths: scriptPaths
};