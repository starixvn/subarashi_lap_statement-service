define(
    ['jquery', 'common/site', 'common/querystring', 'common/popup'],
    function ($, Site, QueryString, Popup) {
        'use strict';
        const BETTYPE_CASHOUT = 911;

        let _sortColumnName, _sortOrder;

        function _beforeShow() {
            Site.blockUI();
        }

        function _afterShow() {
            Site.unBlockUI();
        }

        function _openPopup(url, title, sporttype) {
            showResultPopUp(url, { width: ResultWidth, height: 450, title: title });
        }

        function _getAccessToken() {
            return $("#access_token").val();
        }

        function _getLanguage() {
            return $("#lang").val();
        }

        function showDetailPopUp(url, option) {
            option.id = 'betlist-popup-detail';
            Popup.openPopupWithIframe(url, option);
        }

        function showResultPopUp(url, option) {
            option.id = 'betlist-popup-result';
            Popup.openPopupWithIframe(url, option);
        }

        function getBaseCurrency() {
            return $('#baseCurrency').val();
        }

        function calculatePopupWidth() {
            return Math.min(910, $(window).width());
        }

        let DetailsWidth = calculatePopupWidth();
        let ResultWidth = Math.min(850, $(window).width());

        const _me = {
            getSortParams: function () {
                if (!_sortColumnName) {
                    return null;
                }

                return {
                    SortBy: _sortColumnName,
                    SortOrder: _sortOrder
                };
            },
            submiting: true,
            initialize: function () {
                Site.initialize();

                _sortColumnName = _sortColumnName || $('#SortBy').val();
                _sortOrder = _sortOrder || $('#SortOrder').val();

                _me.registerBetListGlobalFunctions();

                $(".icon-show-more").click(function () {
                    var e = $(this);
                    _me.showHideMoreInfo(e);
                });
            },

            showSystemParlayDetail: function (transId, refNo, betId, custId, betTeam, baseCurrency) {
                if (Number(transId) === 0) { return; }

                let url = Site.resolveClientUrl('SystemParlayBetList/Detail');
                url = QueryString.setParam(url, 'refno', refNo);
                url = QueryString.setParam(url, 'betteam', betTeam);
                url = QueryString.setParam(url, 'basecurrency', baseCurrency);
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");

                DetailsWidth = calculatePopupWidth();
                showDetailPopUp(url, { width: DetailsWidth, height: 500, title: betTeam });
            },

            showAGCasinoDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transid', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custid', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_LC_Detail');
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");

                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('AGCasino') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },

            showRnGCasinoDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_RNG_Detail');
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'hastotal', 1);
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");

                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductCS') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },

            showVoidBridgeDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_RNG_Detail');
                url = QueryString.setParam(url, 'hastotal', 1);
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");

                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductVB') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },
            showAllBetDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transid', transId);
                url = QueryString.setParam(url, 'custid', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_LC_Detail');
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");

                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductAB') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },
            showPPDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_RNG_Detail');
                url = QueryString.setParam(url, 'hastotal', 1);
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");

                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductPP') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },
            showPPJackpotWonDetails: function (transId, custId, betTypeId, winlostDate) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'betType', betTypeId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_Jackpot_Detail');
                url = QueryString.setParam(url, 'hastotal', 1);
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");

                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductPP') + ' - ' +
                    Site.getSingleResource('JackpotWonDetails');

                showDetailPopUp(url, { width: 890, height: popH + 50, title: title });
            },
            showSabaClubJackpotWonDetails: function (transId, custId, betTypeId, winlostDate) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'betType', betTypeId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_Jackpot_Detail');
                url = QueryString.setParam(url, 'hastotal', 1);
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");

                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductSBC') + ' - ' +
                    Site.getSingleResource('JackpotWonDetails');

                showDetailPopUp(url, { width: 890, height: popH + 50, title: title });
            },
            showSGJackpotWonDetails: function (transId, custId, betTypeId, winlostDate) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'betType', betTypeId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_Jackpot_Detail');
                url = QueryString.setParam(url, 'hastotal', 1);
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");

                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductSG') + ' - ' +
                    Site.getSingleResource('JackpotWonDetails');

                showDetailPopUp(url, { width: 890, height: popH + 50, title: title });
            },
            showSGDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_RNG_Detail');
                url = QueryString.setParam(url, 'hastotal', 1);
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");

                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductSG') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },
            showSexyGamingBetDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_LC_Detail');
                url = QueryString.setParam(url, 'hastotal', 1);
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");
                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductSXG') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },
            showJokerBetDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_RNG_Detail');
                url = QueryString.setParam(url, 'hastotal', 1);
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");
                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductJK') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },
            showSabaClubDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_RNG_Detail');
                url = QueryString.setParam(url, 'hastotal', 1);
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");
                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductSBC') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },
            showSAGamingDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_LC_Detail');
                url = QueryString.setParam(url, 'hastotal', 1);
                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductSAG') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },
            showPGSoftDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_RNG_Detail');
                url = QueryString.setParam(url, 'hastotal', 1);
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");
                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductPGS') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },
            showHabaneroBetDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_RNG_Detail');
                url = QueryString.setParam(url, 'hastotal', 1);
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");
                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductHB') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },
            showWMBetDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_LC_Detail');
                url = QueryString.setParam(url, 'hastotal', 1);
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");
                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductWM') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },
            showBGGDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_LC_Detail');
                url = QueryString.setParam(url, 'hastotal', 1);
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");
                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductBGG') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },
            showVGamingJackpotWonDetails: function (transId, custId, betTypeId, winlostDate) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'betType', betTypeId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_Jackpot_Detail');
                url = QueryString.setParam(url, 'hastotal', 1);
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");

                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductVGA') + ' - ' +
                    Site.getSingleResource('JackpotWonDetails');

                showDetailPopUp(url, { width: 890, height: popH + 50, title: title });
            },
            showRNGJackpotWonDetails: function (transId, custId, betTypeId, winlostDate, sporttypeName) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'betType', betTypeId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_Jackpot_Detail');
                url = QueryString.setParam(url, 'hastotal', 1);
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");

                const popH = Math.min($(window).height() - 100, 550);
                const title = sporttypeName + ' - ' + Site.getSingleResource('JackpotWonDetails');

                showDetailPopUp(url, { width: 890, height: popH + 50, title: title });
            },
            showVGamingDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_RNG_Detail');
                url = QueryString.setParam(url, 'hastotal', 1);
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");
                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductVGA') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },
            showCGBetDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_RNG_Detail');
                url = QueryString.setParam(url, 'hastotal', 1);
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");
                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductCG') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },
            showAdvantPlayBetDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_RNG_Detail');
                url = QueryString.setParam(url, 'hastotal', 1);
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");
                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductAP') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },
            showAdvantPlayMiniBetDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_RNG_Detail');
                url = QueryString.setParam(url, 'hastotal', 1);
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");
                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductAPM') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },
            showFGGBetDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_LC_Detail');
                url = QueryString.setParam(url, 'hastotal', 1);
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");
                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductFGGL') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },
            showMaxGameBetDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_RNG_Detail');
                url = QueryString.setParam(url, 'hastotal', 1);
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");
                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductMXG') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },
            showJiliBetDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_RNG_Detail');
                url = QueryString.setParam(url, 'hastotal', 1);
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");
                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductJIG') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },
            showYeeBetBetDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_LC_Detail');
                url = QueryString.setParam(url, 'hastotal', 1);
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");
                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductYB') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },
            showMGRNGCasinoBetDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_RNG_Detail');
                url = QueryString.setParam(url, 'hastotal', 1);
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");
                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductMGRNG') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },
            showSealEntertainmentBetDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_RNG_Detail');
                url = QueryString.setParam(url, 'hastotal', 1);
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");
                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductSEA') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },
            showWELiveCasinoBetDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_LC_Detail');
                url = QueryString.setParam(url, 'hastotal', 1);
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");
                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductWE') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },
            showLive22BetDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_RNG_Detail');
                url = QueryString.setParam(url, 'hastotal', 1);
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");
                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductL22') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },
            showUUSlotsBetDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_RNG_Detail');
                url = QueryString.setParam(url, 'hastotal', 1);
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");
                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductUUSL') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },
            ViewCasinoHandHistory: function (refNo) {
                let url = Site.resolveClientUrl('HandHistory/RNGCasino');
                const urlParam = $('#casino-hand-history-' + refNo).attr('url-param');

                url += urlParam;
                Site.redirect(url);
            },

            viewCashOutHistory: function (transId, refNo, betType, betdaqId) {
                const baseCurrency = $('#baseCurrency').val();

                const isCashOut = betdaqId < 1 || betType === BETTYPE_CASHOUT;
                let url = Site.resolveClientUrl('CashOutBetList/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'refNo', refNo);
                url = QueryString.setParam(url, 'betType', betType);
                url = QueryString.setParam(url, 'betdaqId', betdaqId);
                url = QueryString.setParam(url, 'baseCurrency', baseCurrency);
                url = QueryString.setParam(url, 'isCashout', isCashOut);
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");

                const popH = Math.min($(window).height() - 100, 550);
                const title = isCashOut ? Site.getSingleResource('CashOutDetails') : Site.getSingleResource('BoughtDetails');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },

            ViewNewCashOutHistory: function (transId, custId, winlostDate, currency) {
                const baseCurrency = $('#baseCurrency').val();

                let url = Site.resolveClientUrl('CashOutBetList/NewDetail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'currency', currency);
                url = QueryString.setParam(url, 'baseCurrency', baseCurrency);
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");

                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('CashOutHistory');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },

            showBonusDetails: function (refCode, transId) {
                const baseCurrency = $('#baseCurrency').val();

                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'RefCode', refCode);
                url = QueryString.setParam(url, 'TransId', transId);
                url = QueryString.setParam(url, 'BaseCurrency', baseCurrency);
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_LicenseeBonus_Detail');
                url = QueryString.setParam(url, 'haslicWl', 1);
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");

                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('TransactionDetail');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },

            showVideoPokerGameDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_RNG_Detail');
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");

                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductCC') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },

            showFastSpinBetDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_RNG_Detail');
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");

                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductFS') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },

            showFACHAIBetDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_RNG_Detail');
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");

                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductFAC') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },

            showNextspinBetDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_RNG_Detail');
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");

                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductNS') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },

            showGPIBetDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_RNG_Detail');
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");

                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductGPIRNG') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },

            showONLiveCasinoBetDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_LC_Detail');
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");

                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductON') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },

            showPlaytechBetDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_RNG_Detail');
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");

                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductPTRNG') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },

            showGPILCBetDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_LC_Detail');
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");

                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductGPILC') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },

            showPlayStarBetDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_RNG_Detail');
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");

                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductPS') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },

            showASKMESLOTBetDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_RNG_Detail');
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");

                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductASK') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },

            showFunkyGamesBetDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_RNG_Detail');
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");

                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductFG') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },

            showHotDogBetDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_RNG_Detail');
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");

                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductHOT') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },

            showMicroGamingLCBetDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_LC_Detail');
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");

                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductMGLC') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },

            showOdinGamingBetDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_RNG_Detail');
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");

                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductODG') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },

            showLiveCasinoBetDetails: function (transId, winlostDate, custId, transType, sporttypeId, sporttypeName) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_LC_Detail');
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");

                const popH = Math.min($(window).height() - 100, 550);
                const title = sporttypeName + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },

            showRNGBetDetails: function (transId, winlostDate, custId, transType, sporttypeId, sporttypeName) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_RNG_Detail');
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");

                const popH = Math.min($(window).height() - 100, 550);
                const title = sporttypeName + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },

            showDetail: function (id) {
                var e = document.getElementById(id);
                if (e.style.display == 'block') {
                    e.style.display = 'none';
                }
                else {
                    e.style.display = 'block';
                }
            },

            registerBetListGlobalFunctions: function () {
                window.openPopup = _openPopup;
                window.getResource = null;
                window.formatDateString = null;
                window.beforeShow = _beforeShow;
                window.afterShow = _afterShow;
                window.getBaseCurrency = null;
                window.getAccessToken = _getAccessToken;
                window.getLanguage = _getLanguage;

                window.showCombMPDetail = _me.showSystemParlayDetail;
                window.ViewCashOutHistory = _me.viewCashOutHistory;
                window.showAGCasinoDetails = _me.showAGCasinoDetails;
                window.showRnGCasinoDetails = _me.showRnGCasinoDetails;
                window.showVoidBridgeDetails = _me.showVoidBridgeDetails;
                window.showTodayRunningBetsDetails = _me.showAllBetDetails;
                window.ShowBonusDetails = _me.showBonusDetails;
                window.showPPDetails = _me.showPPDetails;
                window.showPPJackpotWonDetails = _me.showPPJackpotWonDetails;
                window.showSabaClubJackpotWonDetails = _me.showSabaClubJackpotWonDetails;
                window.showSGJackpotWonDetails = _me.showSGJackpotWonDetails;
                window.showVideoPokerGameDetails = _me.showVideoPokerGameDetails;
                window.showSGDetails = _me.showSGDetails;
                window.showSexyGamingBetDetails = _me.showSexyGamingBetDetails;
                window.showSAGamingDetails = _me.showSAGamingDetails;
                window.showJokerBetDetails = _me.showJokerBetDetails;
                window.showSabaClubDetails = _me.showSabaClubDetails;
                window.showPGSoftDetails = _me.showPGSoftDetails;
                window.showHabaneroBetDetails = _me.showHabaneroBetDetails;
                window.showWMBetDetails = _me.showWMBetDetails;
                window.showBGBetDetails = _me.showBGGDetails;
                window.showVGamingDetails = _me.showVGamingDetails;
                window.showVGamingJackpotWonDetails = _me.showVGamingJackpotWonDetails;
                window.showCGBetDetails = _me.showCGBetDetails;
                window.showAdvantPlayBetDetails = _me.showAdvantPlayBetDetails;
                window.showAdvantPlayMiniBetDetails = _me.showAdvantPlayMiniBetDetails;
                window.ViewNewCashOutHistory = _me.ViewNewCashOutHistory;
                window.showFGGBetDetails = _me.showFGGBetDetails;
                window.showMaxGameBetDetails = _me.showMaxGameBetDetails;
                window.showJiliBetDetails = _me.showJiliBetDetails;
                window.showYeeBetBetDetails = _me.showYeeBetBetDetails;
                window.showMGRNGCasinoBetDetails = _me.showMGRNGCasinoBetDetails;
                window.showSealEntertainmentBetDetails = _me.showSealEntertainmentBetDetails;
                window.showWELiveCasinoBetDetails = _me.showWELiveCasinoBetDetails;
                window.showLive22BetDetails = _me.showLive22BetDetails;
                window.showUUSlotsBetDetails = _me.showUUSlotsBetDetails;
                window.showFastSpinBetDetails = _me.showFastSpinBetDetails;
                window.showFACHAIBetDetails = _me.showFACHAIBetDetails;
                window.showNextspinBetDetails = _me.showNextspinBetDetails;
                window.showONLiveCasinoBetDetails = _me.showONLiveCasinoBetDetails;
                window.showPlaytechBetDetails = _me.showPlaytechBetDetails;
                window.showGPIBetDetails = _me.showGPIBetDetails;
                window.showGPILCBetDetails = _me.showGPILCBetDetails;
                window.showPlayStarBetDetails = _me.showPlayStarBetDetails;
                window.showASKMESLOTBetDetails = _me.showASKMESLOTBetDetails;
                window.showFunkyGamesBetDetails = _me.showFunkyGamesBetDetails;
                window.showHotDogBetDetails = _me.showHotDogBetDetails;
                window.showMicroGamingLCBetDetails = _me.showMicroGamingLCBetDetails;
                window.showOdinGamingBetDetails = _me.showOdinGamingBetDetails;
                window.showLiveCasinoBetDetails = _me.showLiveCasinoBetDetails;
                window.showRNGBetDetails = _me.showRNGBetDetails;
                window.showRNGJackpotWonDetails = _me.showRNGJackpotWonDetails;

                const betList = {
                    ViewCasinoHandHistory: _me.ViewCasinoHandHistory
                };
                window.BetList = betList;
            },
            showHideMoreInfo: function (e) {
                var isCollapsed = e.hasClass("collapsed");
                var refNo = e.attr("data-refno");
                var moreInfo = $(".more-info[data-refno='" + refNo + "']");

                if (isCollapsed) {
                    moreInfo.removeClass("hide");
                    e.removeClass("collapsed");
                } else {
                    moreInfo.addClass("hide");
                    e.addClass("collapsed");
                }
            }
        };

        return _me;
    });