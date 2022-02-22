define(
    ['jquery', 'common/site', 'common/querystring', 'common/popup'],
    function ($, Site, QueryString, Popup) {
        'use strict';
        const BETTYPE_CASHOUT = 911;
        const BETTYPE_MIXPARLAY = 9, BETTYPE_SYSTEMPARLAY = 29, BETTYPE_MIXPARLAY_BETRADAR = 2799;
        const MIN_BETTYPE_FASTMARKET = 221;
        const MAX_BETTYPE_FASTMARKET = 227;
        const SPORT_RNG_GAME = 208;
        const SPORT_RNG_MINIGAME = 209;
        const SPORT_RNG_MOBILE = 210;
        const SPORT_ALLBET = 211;
        const SPORT_VOIDBRIDGE = 212;
        const SPORT_PP = 248;
        const SPORT_GLOBALGAMING = 219;
        const SPORT_CARDCLUB = 165;
        const SPORT_SG = 247;
        const SPORT_VRG = 245;
        const SPORT_SXG = 243;
        const SPORT_SBC = 200;
        const SPORT_SBC_FreeBets = 150;
        const SPORT_SAG = 244;
        const SPORT_PGS = 232;
        const SPORT_JK = 234;
        const SPORT_HAPPY5LINKEDGAMES = 168;
        const SPORT_GDG = 236;
        const SPORT_SPORTSLOTTERY = 169;
        const SPORT_HB = 238;
        const SPORT_WM = 233;
        const SPORT_BGG = 239;
        const SPORT_VGA = 171;
        const SPORT_VGA_FreeBets = 173;
        const SPORT_CG = 167;
        const SPORT_AP = 172;
        const SPORT_APM = 174;

        let _sortColumnName, _sortOrder;

        function showDetailPopUp(url, option) {
            option.id = 'betlist-popup-detail';
            Popup.openPopupWithIframe(url, option);
        }

        function showResultPopUp(url, option) {
            option.id = 'betlist-popup-result';
            Popup.openPopupWithIframe(url, option);
        }

        function isFastMarket(bettype) {
            return bettype >= MIN_BETTYPE_FASTMARKET && bettype <= MAX_BETTYPE_FASTMARKET;
        }
        function getBaseCurrency() {
            return $('#baseCurrency').val();
        }

        function calculatePopupWidth() {
            return Math.min(890, $(window).width());
        }

        function resize3rdPartyResultFrame(minHeight) {
            var $frame = $("#popupFrame");

            if ($frame.length) {
                var height = $(window).height() - 10;
                height = Math.max(minHeight, height);

                $frame.height(height);
            }
        }

        let DetailsWidth = calculatePopupWidth();

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

            getMixParlayResultUrl: function (refno, winlostdate, bettype) {
                let url = Site.resolveClientUrl('ResultReport/ResultPopup');
                url = QueryString.setParam(url, 'refno', refno);
                url = QueryString.setParam(url, 'winlossdate', winlostdate);
                url = QueryString.setParam(url, 'bettype', bettype);
                url = QueryString.setParam(url, 'matchid', 1);
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());

                return url;
            },

            getSportBookResultUrl: function (matchid, bettype, raceNo, sporttype, league, isOutright, extension1, extension2) {
                let url = Site.resolveClientUrl('ResultReport/ResultPopup');
                url = QueryString.setParam(url, 'matchid', matchid);
                url = QueryString.setParam(url, 'bettype', bettype);
                url = QueryString.setParam(url, 'raceno', raceNo);
                url = QueryString.setParam(url, 'sporttype', sporttype);
                url = QueryString.setParam(url, 'isoutright', isOutright);
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());

                if (isFastMarket(bettype)) {
                    url = QueryString.setParam(url, 'handicap1', extension1);
                    url = QueryString.setParam(url, 'handicap2', extension2);
                }
                else {
                    url = QueryString.setParam(url, 'betid', extension1);
                }

                return url;
            },

            viewRngCasinoResult: function (title, refno) {
                let url = Site.resolveClientUrl('ResultReport/ShowRngCasinoSingleWalletResult');

                const params = {
                    'refNo': refno,
                    'lang': $("#lang").val()
                };

                $.ajax({
                    type: 'POST',
                    contentType: 'application/json',
                    dataType: 'json',
                    data: JSON.stringify(params),
                    cache: false,
                    url: url,
                    success: function (response) {
                        const result = response.Result;
                        if (response.Success) {
                            if (result === '') {
                                return;
                            }

                            showResultPopUp(result, { width: DetailsWidth, height: 380, title: title });
                        }
                        else {
                            alert(Site.getSingleResource('ErrorProcessingMsg'));
                        }
                    },
                    beforeSend: function () {
                        Site.blockUI();
                    },
                    complete: function () {
                        Site.unBlockUI();
                    }
                });
            },

            viewVoidbridgeResult: function (custId, gamePlayId, title) {
                let url = Site.resolveClientUrl('ResultReport/VoidbridgeResult') + '?custid=' + custId + '&gamePlayId=' + gamePlayId;
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());

                Site.blockUI();
                $.get(url, function (result) {
                    if (result !== '') {
                        showResultPopUp(result, { width: DetailsWidth, height: 1200, title: title });
                        Site.unBlockUI();
                    }
                });
            },

            viewPPResult: function (custId, gamePlayId, title) {
                let url = Site.resolveClientUrl('ResultReport/PPResultPopup') + '?custid=' + custId + '&gamePlayId=' + gamePlayId;
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());

                Site.blockUI();
                $.get(url, function (result) {
                    if (result !== '') {
                        var height = 450;
                        showResultPopUp(result, { width: DetailsWidth, height: height, title: title });
                        resize3rdPartyResultFrame(height);
                        Site.unBlockUI();
                    }
                });
            },

            viewSGResult: function (custId, gamePlayId, title) {
                let url = Site.resolveClientUrl('ResultReport/SGResultPopup') + '?custid=' + custId + '&gamePlayId=' + gamePlayId;
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());

                Site.blockUI();
                $.get(url, function (result) {
                    if (result !== '') {
                        showResultPopUp(result, { width: DetailsWidth, height: 380, title: title });
                        Site.unBlockUI();
                    }
                });
            },

            viewVRGResult: function (gameId, platform, title) {
                let url = Site.resolveClientUrl('ResultReport/VirtualGamesResultPopup') + '?gameId=' + gameId + '&platform=' + platform;
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());

                Site.blockUI();
                $.get(url, function (result) {
                    if (result !== '') {
                        showResultPopUp(result, { width: DetailsWidth, height: 380, title: title });
                        Site.unBlockUI();
                    }
                });
            },

            viewCardClubResult: function (custId, gameTypeId, transId3rd, title) {
                let url = Site.resolveClientUrl('ResultReport/CCResultPopup') + '?custId=' + custId + '&gameTypeId=' + gameTypeId + '&transId3rd=' + transId3rd;
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());

                Site.blockUI();
                $.get(url, function (result) {
                    if (result !== '') {
                        showResultPopUp(result, { width: DetailsWidth, height: 380, title: title });
                        Site.unBlockUI();
                    }
                });
            },

            viewSabaClubResult: function (custId, gameId, siteId, gameRoundId, title, sporttype) {
                let url = Site.resolveClientUrl('ResultReport/SBCResultPopup') + '?custId=' + custId + '&gameId=' + gameId + '&siteId=' + siteId + '&gameRoundId=' + gameRoundId + '&sportType=' + sporttype;
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());

                Site.blockUI();
                $.get(url, function (result) {
                    if (result !== '') {
                        showResultPopUp(result, { width: DetailsWidth, height: 380, title: title });
                        Site.unBlockUI();
                    }
                });
            },

            viewAllBetResult: function (trasdesc, betcheck, bettype, title) {
                let url = Site.resolveClientUrl('ResultReport/AllBetResult?transdesc=' +
                    encodeURIComponent(trasdesc) + '&betcheck=' + betcheck + '&bettype=' + bettype);
                url += "&lang=" + $("#lang").val();
                url += "&access_token=" + $("#access_token").val();

                Site.blockUI();
                showResultPopUp(url, { width: DetailsWidth, height: 380, title: title });
                Site.unBlockUI();
            },

            viewSexyGamingResult: function (trasdesc, matchCode, bettype, title) {
                let url = Site.resolveClientUrl('ResultReport/SexyGamingResult?transdesc=' +
                    encodeURIComponent(trasdesc) + '&matchCode=' + matchCode + '&bettype=' + bettype);
                url += "&lang=" + $("#lang").val();
                url += "&access_token=" + $("#access_token").val();

                Site.blockUI();
                showResultPopUp(url, { width: DetailsWidth, height: 380, title: title });
                Site.unBlockUI();
            },

            viewSAGamingResult: function (trasdesc, matchCode, bettype, title) {
                let url = Site.resolveClientUrl('ResultReport/SAGamingResult?transdesc=' +
                    encodeURIComponent(trasdesc) + '&matchCode=' + matchCode + '&bettype=' + bettype);
                url += "&lang=" + $("#lang").val();
                url += "&access_token=" + $("#access_token").val();

                Site.blockUI();
                showResultPopUp(url, { width: DetailsWidth, height: 380, title: title });
                Site.unBlockUI();
            },

            ViewJokerResult: function (custId, gamePlayId, gameCode, title) {
                let url = Site.resolveClientUrl('ResultReport/JokerResult?custId=' + custId + '&gamePlayId=' + gamePlayId + '&gameCode=' + gameCode);
                url += "&lang=" + $("#lang").val();
                url += "&access_token=" + $("#access_token").val();

                Site.blockUI();
                $.get(url, function (result) {
                    if (result !== '') {
                        showResultPopUp(result, { width: DetailsWidth, height: 380, title: title });
                        Site.unBlockUI();
                    }
                });
            },

            viewGlobalGamingResult: function (custId, gameId, title) {
                let url = Site.resolveClientUrl('ResultReport/GlobalGamingResult') + '?custid=' + custId + '&gameId=' + gameId;
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());

                Site.blockUI();
                $.get(url, function (result) {
                    if (result !== '') {
                        showResultPopUp(result, { width: DetailsWidth, height: 380, title: title });
                        Site.unBlockUI();
                    }
                });
            },

            viewPGSResult: function (custId, gamePlayId, gameId3rd, title) {
                let url = Site.resolveClientUrl('ResultReport/PGSoftResultPopup') +
                    '?custid=' + custId +
                    '&gamePlayId=' + gamePlayId +
                    '&gameId3rd=' + gameId3rd;
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());

                Site.blockUI();
                $.get(url, function (result) {
                    if (result !== '') {
                        showResultPopUp(result, { width: DetailsWidth, height: 450, title: title });
                        Site.unBlockUI();
                    }
                });
            },

            viewBGGResult: function (custId, transId3rd, currencyId, siteId, title) {
                let url = Site.resolveClientUrl('ResultReport/BGGResultPopup') +
                    '?custid=' + custId +
                    '&transId3rd=' + transId3rd +
                    '&currencyId=' + currencyId +
                    '&siteId=' + siteId;
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());

                Site.blockUI();
                $.get(url, function (result) {
                    if (result !== '') {
                        showResultPopUp(result, { width: DetailsWidth, height: 450, title: title });
                        Site.unBlockUI();
                    }
                });
            },

            ViewHappy5LinkedGamesResult: function (custId, transId3rd, platform, title) {
                let url = Site.resolveClientUrl('ResultReport/Happy5LinkedGamesResultPopup?custId=' + custId +
                    '&transId3rd=' + transId3rd + '&platform=' + platform);
                url += "&lang=" + $("#lang").val();
                url += "&access_token=" + $("#access_token").val();

                Site.blockUI();
                $.get(url, function (result) {
                    if (result !== '') {
                        showResultPopUp(result, { width: DetailsWidth, height: 380, title: title });
                        Site.unBlockUI();
                    }
                });
            },

            viewSportsLotteryResult: function (custId, transId3rd, platform, title) {
                let url = Site.resolveClientUrl('ResultReport/SportsLotteryResult?custId=' + custId +
                    '&transId3rd=' + transId3rd + '&platform=' + platform);
                url += "&lang=" + $("#lang").val();
                url += "&access_token=" + $("#access_token").val();

                Site.blockUI();
                $.get(url, function (result) {
                    if (result !== '') {
                        showResultPopUp(result, { width: DetailsWidth, height: 380, title: title });
                        Site.unBlockUI();
                    }
                });
            },

            viewGDGResult: function (custId, transId3rd, gameId3rd, siteId, title) {
                let url = Site.resolveClientUrl('ResultReport/GDGResult?custId=' + custId + '&transId3rd=' + transId3rd + '&gameId3rd=' + gameId3rd + '&siteId=' + siteId);
                url += "&lang=" + $("#lang").val();
                url += "&access_token=" + $("#access_token").val();

                Site.blockUI();
                $.get(url, function (result) {
                    if (result !== '') {
                        showResultPopUp(result, { width: DetailsWidth, height: 380, title: title });
                        Site.unBlockUI();
                    }
                });
            },

            viewHabaneroResult: function (custId, transId3rd, gameId3rd, siteId, title) {
                let url = Site.resolveClientUrl('ResultReport/HabaneroResult?custId=' + custId + '&transId3rd=' + transId3rd + '&gameId3rd=' + gameId3rd + '&siteId=' + siteId);
                url += "&lang=" + $("#lang").val();
                url += "&access_token=" + $("#access_token").val();

                Site.blockUI();
                $.get(url, function (result) {
                    if (result !== '') {
                        showResultPopUp(result, { width: DetailsWidth, height: 380, title: title });
                        Site.unBlockUI();
                    }
                });
            },

            viewWMResult: function (transDesc, gameId3rd, betType, title) {
                let url = Site.resolveClientUrl('ResultReport/WMResult?transDesc=' +
                    encodeURIComponent(transDesc) + '&gameId3rd=' + gameId3rd + '&betType=' + betType);
                url += "&lang=" + $("#lang").val();
                url += "&access_token=" + $("#access_token").val();

                Site.blockUI();
                showResultPopUp(url, { width: DetailsWidth, height: 380, title: title });
                Site.unBlockUI();
            },

            viewVGamingResult: function (custId, gameId, siteId, gameRoundId, title, sporttype, currencyName) {
                let url = Site.resolveClientUrl('ResultReport/VGamingResultPopup') + '?custId=' + custId + '&gameId=' + gameId + '&siteId=' + siteId + '&gameRoundId=' + gameRoundId + '&sportType=' + sporttype + '&currencyName=' + currencyName;
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());

                Site.blockUI();
                $.get(url, function (result) {
                    if (result !== '') {
                        showResultPopUp(result, { width: DetailsWidth, height: 380, title: title });
                        Site.unBlockUI();
                    }
                });
            },

            viewCGResult: function (custId, transId3rd, gameId3rd, siteId, title) {
                let url = Site.resolveClientUrl('ResultReport/CGResult?custId=' + custId + '&transId3rd=' + transId3rd + '&gameId3rd=' + gameId3rd + '&siteId=' + siteId);
                url += "&lang=" + $("#lang").val();
                url += "&access_token=" + $("#access_token").val();

                Site.blockUI();
                $.get(url, function (result) {
                    if (result !== '') {
                        showResultPopUp(result, { width: DetailsWidth, height: 380, title: title });
                        Site.unBlockUI();
                    }
                });
            },

            viewAdvantPlayResult: function (custId, transId3rd, gameId3rd, siteId, title) {
                let url = Site.resolveClientUrl('ResultReport/AdvantPlayResult?custId=' + custId + '&transId3rd=' + transId3rd + '&gameId3rd=' + gameId3rd + '&siteId=' + siteId);
                url += "&lang=" + $("#lang").val();
                url += "&access_token=" + $("#access_token").val();

                Site.blockUI();
                $.get(url, function (result) {
                    if (result !== '') {
                        showResultPopUp(result, { width: DetailsWidth, height: 380, title: title });
                        Site.unBlockUI();
                    }
                });
            },

            viewAdvantPlayMiniResult: function (custId, transId3rd, gameId3rd, siteId, title) {
                let url = Site.resolveClientUrl('ResultReport/AdvantPlayMiniResult?custId=' + custId + '&transId3rd=' + transId3rd + '&gameId3rd=' + gameId3rd + '&siteId=' + siteId);
                url += "&lang=" + $("#lang").val();
                url += "&access_token=" + $("#access_token").val();

                Site.blockUI();
                $.get(url, function (result) {
                    if (result !== '') {
                        showResultPopUp(result, { width: DetailsWidth, height: 380, title: title });
                        Site.unBlockUI();
                    }
                });
            },

            viewThirdPartyProductResult: function (custId, transId3rd, gameId3rd, currencyId, siteId, betTypeId, productPath, title) {
                const param = 'custId=' + custId +
                    '&transId3rd=' + transId3rd +
                    '&gameId3rd=' + gameId3rd +
                    '&siteId=' + siteId +
                    '&currencyId=' + currencyId +
                    '&betTypeId=' + betTypeId +
                    '&productPath=' + productPath;
                const url = Site.resolveClientUrl('ResultReport/ThirdPartyProduct?' + param);
                Site.blockUI();

                $.get(url, function (result) {
                    if (result !== '') {
                        showResultPopUp(result, { width: 800, height: 380, title: title });
                        Site.unBlockUI();
                    }
                });
            },

            viewResult: function (matchid, raceNo, bettype, sporttype, refno,
                currency, winlostdate, refnoMixParlay, league, isOutright, extension1, extension2, custid, transdecs, siteId) {
                const title = Site.getSingleResource('Result');
                const defaultPlatform = 'd';  // desktop

                if (sporttype === SPORT_RNG_GAME || sporttype === SPORT_RNG_MINIGAME || sporttype === SPORT_RNG_MOBILE) {
                    _me.viewThirdPartyProductResult(custid, transdecs, extension2, extension1, siteId, bettype, 'RNGCasino', title);
                }
                else if (sporttype === SPORT_VOIDBRIDGE) {
                    _me.viewThirdPartyProductResult(custid, transdecs, extension2, extension1, siteId, bettype, 'Voidbridge', title);
                }
                else if (sporttype === SPORT_PP) {
                    _me.viewThirdPartyProductResult(custid, transdecs, extension2, extension1, siteId, bettype, 'PP', title);
                }
                else if (sporttype === SPORT_CARDCLUB) {
                    _me.viewThirdPartyProductResult(custid, transdecs, extension2, extension1, siteId, bettype, 'CardClub', title);
                }
                else if (sporttype === SPORT_SBC || sporttype === SPORT_SBC_FreeBets) {
                    _me.viewSabaClubResult(custid, bettype, extension2, transdecs, title, sporttype);
                }
                else if (sporttype === SPORT_SG) {
                    _me.viewThirdPartyProductResult(custid, transdecs, extension2, extension1, siteId, bettype, 'SG', title);
                }
                else if (sporttype === SPORT_VRG) {
                    _me.viewVRGResult(refno, defaultPlatform, title);
                }
                else if (sporttype === SPORT_ALLBET) {
                    _me.viewAllBetResult(transdecs, extension2, bettype, title);
                }
                else if (sporttype === SPORT_SXG) {
                    _me.viewSexyGamingResult(transdecs, extension2, bettype, title);
                }
                else if (sporttype === SPORT_SAG) {
                    _me.viewSAGamingResult(transdecs, extension2, bettype, title);
                }
                else if (sporttype === SPORT_JK) {
                    _me.viewThirdPartyProductResult(custid, transdecs, extension2, extension1, siteId, bettype, 'Joker', title);
                }
                else if (sporttype === SPORT_GLOBALGAMING) {
                    _me.viewGlobalGamingResult(custid, extension2, title);
                }
                else if (sporttype === SPORT_PGS) {
                    _me.viewThirdPartyProductResult(custid, transdecs, extension2, extension1, siteId, bettype, 'PGSoft', title);
                }
                else if (sporttype === SPORT_HAPPY5LINKEDGAMES) {
                    _me.viewThirdPartyProductResult(custid, refno, extension2, extension1, siteId, bettype, 'Happy5LinkedGames', title);
                }
                else if (sporttype === SPORT_GDG) {
                    _me.viewThirdPartyProductResult(custid, transdecs, extension2, extension1, siteId, bettype, 'GDG', title);
                }
                else if (sporttype === SPORT_SPORTSLOTTERY) {
                    _me.viewThirdPartyProductResult(custid, transdecs, extension2, extension1, siteId, bettype, 'SportsLottery', title);
                }
                else if (sporttype === SPORT_HB) {
                    _me.viewThirdPartyProductResult(custid, transdecs, extension2, extension1, siteId, bettype, 'Habanero', title);
                }
                else if (sporttype === SPORT_WM) {
                    _me.viewWMResult(transdecs, extension2, bettype, title);
                }
                else if (sporttype === SPORT_BGG) {
                    _me.viewThirdPartyProductResult(custid, transdecs, extension2, extension1, siteId, bettype, 'BGG', title);
                }
                else if (sporttype === SPORT_VGA || sporttype === SPORT_VGA_FreeBets) {
                    _me.viewVGamingResult(custid, bettype, extension2, transdecs, title, sporttype, currency);
                }
                else if (sporttype === SPORT_CG) {
                    _me.viewThirdPartyProductResult(custid, transdecs, extension2, extension1, siteId, bettype, 'CG', title);
                }
                else if (sporttype === SPORT_AP) {
                    _me.viewThirdPartyProductResult(custid, transdecs, extension2, extension1, siteId, bettype, 'AdvantPlay', title);
                }
                else if (sporttype === SPORT_APM) {
                    _me.viewThirdPartyProductResult(custid, transdecs, extension2, extension1, siteId, bettype, 'AdvantPlayMini', title);
                }
                else {
                    const numberBettype = Number(bettype);
                    const isParlay = numberBettype === BETTYPE_MIXPARLAY || numberBettype === BETTYPE_SYSTEMPARLAY || numberBettype === BETTYPE_MIXPARLAY_BETRADAR;
                    const url = isParlay
                        ? _me.getMixParlayResultUrl(refnoMixParlay, winlostdate, numberBettype)
                        : _me.getSportBookResultUrl(matchid, numberBettype, raceNo, sporttype, league, isOutright, extension1, extension2, custid);

                    const height = 450;
                    showResultPopUp(url, { width: DetailsWidth, height: height + 70, title: title });
                }
            },

            showMixParlayDetail: function (transId, divEvent, custid) {
                let url = Site.resolveClientUrl('api/MixParlayBetList/Detail?transId=' + transId + '&custId=' + custid);
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());

                if (divEvent.css('display') === 'none') {
                    $.ajax({
                        type: 'GET',
                        contentType: 'application/json',
                        dataType: 'json',
                        cache: false,
                        url: url,
                        success: function (response) {
                            let choice = '';
                            const len = response.length;
                            $.each(response, function (index, item) {
                                choice += item;
                                if (index < len - 1) {
                                    choice += '<div class="line"></div>';
                                }
                            });

                            divEvent.html(choice);
                            divEvent.css('display', '');
                        },
                        beforeSend: function () {
                            Site.blockUI();
                        },
                        complete: function () {
                            Site.unBlockUI();
                        }
                    });
                }
                else {
                    divEvent.css('display', 'none');
                    divEvent.html('');
                }
            },

            showNewParlayDetail: function (transId, transIndex, refNo, handicap1, handicap2, betcheck, betteam, custId) {
                const $divEvent = $('#divEvent_' + transId + '' + transIndex);
                const $nameDiv = $('#combinationOfferName_' + transId + transIndex);
                $divEvent.addClass('mx-parlay'); //to make line-through for void-parlay-ticket

                if ($divEvent.is(':hidden')) {
                    let url = Site.resolveClientUrl('api/MixParlayBetList/NewParlayDetail');
                    url = QueryString.setParam(url, 'transId', transId);
                    url = QueryString.setParam(url, 'refNo', refNo);
                    url = QueryString.setParam(url, 'handicap1', handicap1);
                    url = QueryString.setParam(url, 'handicap2', handicap2);
                    url = QueryString.setParam(url, 'betcheck', betcheck);
                    url = QueryString.setParam(url, 'betteam', betteam);
                    url = QueryString.setParam(url, 'custId', custId);
                    url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                    url = QueryString.setParam(url, 'lang', $("#lang").val());
                    $.ajax({
                        type: 'GET',
                        contentType: 'application/json',
                        dataType: 'json',
                        cache: false,
                        url: url,
                        success: function (response) {
                            let length = response.data === null ? 0 : response.data.length;
                            if (length > 0) {
                                let choice = '';
                                $.each(response.texts, function (index, item) {
                                    choice += item;
                                });

                                choice += '<div class="line"></div>';

                                $.each(response.data, function (index, item) {
                                    choice += item;
                                    if (index < length - 1) {
                                        choice += '<div class="line"></div>';
                                    }
                                });

                                $divEvent.html(choice).show();
                                $nameDiv.hide();
                            }
                        },
                        beforeSend: Site.blockUI,
                        complete: Site.unBlockUI
                    });
                }
                else {
                    $divEvent.hide().html('');
                    $nameDiv.show();
                    $('.ui-jqgrid-bdiv').css('height', 'auto');
                }
            },

            showSingleMatchParlayDetail: function (transId, custId, status, transIndex) {
                const $divEvent = $('#divEvent_' + transId + '' + transIndex);

                if ($divEvent.is(':hidden')) {
                    let url = Site.resolveClientUrl('api/SingleMatchParlayBetList/Detail');
                    url = QueryString.setParam(url, 'transId', transId);
                    url = QueryString.setParam(url, 'custId', custId);
                    url = QueryString.setParam(url, 'status', status);
                    url = QueryString.setParam(url, 'access_token', $('#access_token').val());
                    url = QueryString.setParam(url, 'lang', $('#lang').val());

                    $.ajax({
                        type: 'GET',
                        contentType: 'application/json',
                        dataType: 'json',
                        cache: false,
                        url: url,
                        success: function (response) {
                            const length = response === null ? 0 : response.length;
                            if (length > 0) {
                                let choice = '<div class="single-match-parlay-detail">';
                                $.each(response, function (index, item) {
                                    choice += item;
                                });

                                choice += '</div>';
                                $divEvent.html(choice).show();
                            }
                        },
                        beforeSend: Site.blockUI,
                        complete: Site.unBlockUI
                    });
                }
                else {
                    $divEvent.hide().html('');
                    $('.ui-jqgrid-bdiv').css('height', 'auto');
                }
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
            showColossusBetsDetails: function (refNo, winlostDate, transId, custId, betCheck) {
                let url = Site.resolveClientUrl('ColossusBetBetList/Detail');
                url = QueryString.setParam(url, 'refNo', refNo);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'transid', transId);
                url = QueryString.setParam(url, 'custid', custId);
                url = QueryString.setParam(url, 'betCheck', betCheck);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());

                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('TicketDetailOf').replace('{0}', refNo);

                showDetailPopUp(url, { width: DetailsWidth, height: popH + 50, title: title });
            },
            showGlobalGamingDetails: function (transId, winlostDate, custId, transType) {
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
                url = QueryString.setParam(url, 'haslicWl', 1);
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");

                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductFW') + ' - ' +
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
            showPPJackpotWonDetails: function (transId, custId, betTypeId) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'betType', betTypeId);
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
            showSabaClubJackpotWonDetails: function (transId, custId, betTypeId) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'betType', betTypeId);
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
            showSGJackpotWonDetails: function (transId, custId, betTypeId) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'betType', betTypeId);
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
            showGDGBetDetails: function (transId, winlostDate, custId, transType) {
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
                const title = Site.getSingleResource('ProductGDG') + ' - ' +
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
            showVGamingJackpotWonDetails: function (transId, custId, betTypeId) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'betType', betTypeId);
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
            registerBetListGlobalFunctions: function () {
                window.ViewResult = _me.viewResult;
                // showMP and showCombMP are only for old tickets
                window.showMP = function (transId, refNo, custId) {
                    const divEvent = $('#divEvent_' + transId);
                    divEvent.addClass('mx-parlay'); //to make line-through for void-parlay-ticket
                    _me.showMixParlayDetail(transId, divEvent, custId);
                };

                window.showCombMP = function (transId, index, refNo, custId) {
                    const divEvent = $('#divEvent_' + transId + '' + index);
                    divEvent.addClass('mx-parlay'); //to make line-through for void-parlay-ticket
                    _me.showMixParlayDetail(transId, divEvent, custId);
                };

                window.showNewCombMP = _me.showNewParlayDetail;
                window.showCombMPDetail = _me.showSystemParlayDetail;
                window.ViewCashOutHistory = _me.viewCashOutHistory;
                window.showColossusBetsDetails = _me.showColossusBetsDetails;
                window.showAGCasinoDetails = _me.showAGCasinoDetails;
                window.showRnGCasinoDetails = _me.showRnGCasinoDetails;
                window.showVoidBridgeDetails = _me.showVoidBridgeDetails;
                window.showVoidbridgeResult = _me.viewVoidbridgeResult;
                window.showTodayRunningBetsDetails = _me.showAllBetDetails;
                window.showGlobalGaming = _me.showGlobalGamingDetails;
                window.ShowBonusDetails = _me.showBonusDetails;
                window.showPPDetails = _me.showPPDetails;
                window.showPPJackpotWonDetails = _me.showPPJackpotWonDetails;
                window.showSabaClubJackpotWonDetails = _me.showSabaClubJackpotWonDetails;
                window.showSGJackpotWonDetails = _me.showSGJackpotWonDetails;
                window.showVideoPokerGameDetails = _me.showVideoPokerGameDetails;
                window.showSingleMatchParlayDetail = _me.showSingleMatchParlayDetail;
                window.showSGDetails = _me.showSGDetails;
                window.showSexyGamingBetDetails = _me.showSexyGamingBetDetails;
                window.showSAGamingDetails = _me.showSAGamingDetails;
                window.showJokerBetDetails = _me.showJokerBetDetails;
                window.showSabaClubDetails = _me.showSabaClubDetails;
                window.showPGSoftDetails = _me.showPGSoftDetails;
                window.showGDGBetDetails = _me.showGDGBetDetails;
                window.showHabaneroBetDetails = _me.showHabaneroBetDetails;
                window.showWMBetDetails = _me.showWMBetDetails;
                window.showBGBetDetails = _me.showBGGDetails;
                window.showVGamingDetails = _me.showVGamingDetails;
                window.showVGamingJackpotWonDetails = _me.showVGamingJackpotWonDetails;
                window.showCGBetDetails = _me.showCGBetDetails;
                window.showAdvantPlayBetDetails = _me.showAdvantPlayBetDetails;
                window.showAdvantPlayMiniBetDetails = _me.showAdvantPlayMiniBetDetails;

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