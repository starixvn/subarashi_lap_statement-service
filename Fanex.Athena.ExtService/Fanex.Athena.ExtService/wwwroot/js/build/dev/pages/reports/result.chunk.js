/******/ (function(modules) { // webpackBootstrap
/******/ 	// install a JSONP callback for chunk loading
/******/ 	function webpackJsonpCallback(data) {
/******/ 		var chunkIds = data[0];
/******/ 		var moreModules = data[1];
/******/ 		var executeModules = data[2];
/******/
/******/ 		// add "moreModules" to the modules object,
/******/ 		// then flag all "chunkIds" as loaded and fire callback
/******/ 		var moduleId, chunkId, i = 0, resolves = [];
/******/ 		for(;i < chunkIds.length; i++) {
/******/ 			chunkId = chunkIds[i];
/******/ 			if(installedChunks[chunkId]) {
/******/ 				resolves.push(installedChunks[chunkId][0]);
/******/ 			}
/******/ 			installedChunks[chunkId] = 0;
/******/ 		}
/******/ 		for(moduleId in moreModules) {
/******/ 			if(Object.prototype.hasOwnProperty.call(moreModules, moduleId)) {
/******/ 				modules[moduleId] = moreModules[moduleId];
/******/ 			}
/******/ 		}
/******/ 		if(parentJsonpFunction) parentJsonpFunction(data);
/******/
/******/ 		while(resolves.length) {
/******/ 			resolves.shift()();
/******/ 		}
/******/
/******/ 		// add entry modules from loaded chunk to deferred list
/******/ 		deferredModules.push.apply(deferredModules, executeModules || []);
/******/
/******/ 		// run deferred modules when all chunks ready
/******/ 		return checkDeferredModules();
/******/ 	};
/******/ 	function checkDeferredModules() {
/******/ 		var result;
/******/ 		for(var i = 0; i < deferredModules.length; i++) {
/******/ 			var deferredModule = deferredModules[i];
/******/ 			var fulfilled = true;
/******/ 			for(var j = 1; j < deferredModule.length; j++) {
/******/ 				var depId = deferredModule[j];
/******/ 				if(installedChunks[depId] !== 0) fulfilled = false;
/******/ 			}
/******/ 			if(fulfilled) {
/******/ 				deferredModules.splice(i--, 1);
/******/ 				result = __webpack_require__(__webpack_require__.s = deferredModule[0]);
/******/ 			}
/******/ 		}
/******/
/******/ 		return result;
/******/ 	}
/******/
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// object to store loaded and loading chunks
/******/ 	// undefined = chunk not loaded, null = chunk preloaded/prefetched
/******/ 	// Promise = chunk loading, 0 = chunk loaded
/******/ 	var installedChunks = {
/******/ 		"wwwroot/js/build/dev/pages/reports/result": 0
/******/ 	};
/******/
/******/ 	var deferredModules = [];
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId]) {
/******/ 			return installedModules[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			i: moduleId,
/******/ 			l: false,
/******/ 			exports: {}
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.l = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// define getter function for harmony exports
/******/ 	__webpack_require__.d = function(exports, name, getter) {
/******/ 		if(!__webpack_require__.o(exports, name)) {
/******/ 			Object.defineProperty(exports, name, { enumerable: true, get: getter });
/******/ 		}
/******/ 	};
/******/
/******/ 	// define __esModule on exports
/******/ 	__webpack_require__.r = function(exports) {
/******/ 		if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 			Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 		}
/******/ 		Object.defineProperty(exports, '__esModule', { value: true });
/******/ 	};
/******/
/******/ 	// create a fake namespace object
/******/ 	// mode & 1: value is a module id, require it
/******/ 	// mode & 2: merge all properties of value into the ns
/******/ 	// mode & 4: return value when already ns object
/******/ 	// mode & 8|1: behave like require
/******/ 	__webpack_require__.t = function(value, mode) {
/******/ 		if(mode & 1) value = __webpack_require__(value);
/******/ 		if(mode & 8) return value;
/******/ 		if((mode & 4) && typeof value === 'object' && value && value.__esModule) return value;
/******/ 		var ns = Object.create(null);
/******/ 		__webpack_require__.r(ns);
/******/ 		Object.defineProperty(ns, 'default', { enumerable: true, value: value });
/******/ 		if(mode & 2 && typeof value != 'string') for(var key in value) __webpack_require__.d(ns, key, function(key) { return value[key]; }.bind(null, key));
/******/ 		return ns;
/******/ 	};
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function(module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
/******/ 	};
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function(object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "";
/******/
/******/ 	var jsonpArray = window["webpackJsonp"] = window["webpackJsonp"] || [];
/******/ 	var oldJsonpFunction = jsonpArray.push.bind(jsonpArray);
/******/ 	jsonpArray.push = webpackJsonpCallback;
/******/ 	jsonpArray = jsonpArray.slice();
/******/ 	for(var i = 0; i < jsonpArray.length; i++) webpackJsonpCallback(jsonpArray[i]);
/******/ 	var parentJsonpFunction = oldJsonpFunction;
/******/
/******/
/******/ 	// add entry module to deferred list
/******/ 	deferredModules.push([4,"wwwroot/js/build/dev/pages/lib/common"]);
/******/ 	// run deferred modules when ready
/******/ 	return checkDeferredModules();
/******/ })
/************************************************************************/
/******/ ({

/***/ "./node_modules/bootstrap-daterangepicker/daterangepicker.js":
/*!*******************************************************************!*\
  !*** ./node_modules/bootstrap-daterangepicker/daterangepicker.js ***!
  \*******************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/**
* @version: 2.1.30
* @author: Dan Grossman http://www.dangrossman.info/
* @copyright: Copyright (c) 2012-2017 Dan Grossman. All rights reserved.
* @license: Licensed under the MIT license. See http://www.opensource.org/licenses/mit-license.php
* @website: http://www.daterangepicker.com/
*/
// Follow the UMD template https://github.com/umdjs/umd/blob/master/templates/returnExportsGlobal.js
(function (root, factory) {
    if (true) {
        // AMD. Make globaly available as well
        !(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(/*! moment */ "./node_modules/moment/moment.js"), __webpack_require__(/*! jquery */ "./node_modules/jquery/dist/jquery.js-exposed")], __WEBPACK_AMD_DEFINE_RESULT__ = (function (moment, jquery) {
            if (!jquery.fn) jquery.fn = {}; // webpack server rendering
            return factory(moment, jquery);
        }).apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
    } else { var moment, jQuery; }
}(this, function(moment, $) {
    var DateRangePicker = function(element, options, cb) {

        //default settings for options
        this.parentEl = 'body';
        this.element = $(element);
        this.startDate = moment().startOf('day');
        this.endDate = moment().endOf('day');
        this.minDate = false;
        this.maxDate = false;
        this.dateLimit = false;
        this.autoApply = false;
        this.singleDatePicker = false;
        this.showDropdowns = false;
        this.showWeekNumbers = false;
        this.showISOWeekNumbers = false;
        this.showCustomRangeLabel = true;
        this.timePicker = false;
        this.timePicker24Hour = false;
        this.timePickerIncrement = 1;
        this.timePickerSeconds = false;
        this.linkedCalendars = true;
        this.autoUpdateInput = true;
        this.alwaysShowCalendars = false;
        this.ranges = {};

        this.opens = 'right';
        if (this.element.hasClass('pull-right'))
            this.opens = 'left';

        this.drops = 'down';
        if (this.element.hasClass('dropup'))
            this.drops = 'up';

        this.buttonClasses = 'btn btn-sm';
        this.applyClass = 'btn-success';
        this.cancelClass = 'btn-default';

        this.locale = {
            direction: 'ltr',
            format: moment.localeData().longDateFormat('L'),
            separator: ' - ',
            applyLabel: 'Apply',
            cancelLabel: 'Cancel',
            weekLabel: 'W',
            customRangeLabel: 'Custom Range',
            daysOfWeek: moment.weekdaysMin(),
            monthNames: moment.monthsShort(),
            firstDay: moment.localeData().firstDayOfWeek()
        };

        this.callback = function() { };

        //some state information
        this.isShowing = false;
        this.leftCalendar = {};
        this.rightCalendar = {};

        //custom options from user
        if (typeof options !== 'object' || options === null)
            options = {};

        //allow setting options with data attributes
        //data-api options will be overwritten with custom javascript options
        options = $.extend(this.element.data(), options);

        //html template for the picker UI
        if (typeof options.template !== 'string' && !(options.template instanceof $))
            options.template = '<div class="daterangepicker dropdown-menu">' +
                '<div class="calendar left">' +
                    '<div class="daterangepicker_input">' +
                      '<input class="input-mini form-control" type="text" name="daterangepicker_start" value="" />' +
                      '<i class="fa fa-calendar glyphicon glyphicon-calendar"></i>' +
                      '<div class="calendar-time">' +
                        '<div></div>' +
                        '<i class="fa fa-clock-o glyphicon glyphicon-time"></i>' +
                      '</div>' +
                    '</div>' +
                    '<div class="calendar-table"></div>' +
                '</div>' +
                '<div class="calendar right">' +
                    '<div class="daterangepicker_input">' +
                      '<input class="input-mini form-control" type="text" name="daterangepicker_end" value="" />' +
                      '<i class="fa fa-calendar glyphicon glyphicon-calendar"></i>' +
                      '<div class="calendar-time">' +
                        '<div></div>' +
                        '<i class="fa fa-clock-o glyphicon glyphicon-time"></i>' +
                      '</div>' +
                    '</div>' +
                    '<div class="calendar-table"></div>' +
                '</div>' +
                '<div class="ranges">' +
                    '<div class="range_inputs">' +
                        '<button class="applyBtn" disabled="disabled" type="button"></button> ' +
                        '<button class="cancelBtn" type="button"></button>' +
                    '</div>' +
                '</div>' +
            '</div>';

        this.parentEl = (options.parentEl && $(options.parentEl).length) ? $(options.parentEl) : $(this.parentEl);
        this.container = $(options.template).appendTo(this.parentEl);

        //
        // handle all the possible options overriding defaults
        //

        if (typeof options.locale === 'object') {

            if (typeof options.locale.direction === 'string')
                this.locale.direction = options.locale.direction;

            if (typeof options.locale.format === 'string')
                this.locale.format = options.locale.format;

            if (typeof options.locale.separator === 'string')
                this.locale.separator = options.locale.separator;

            if (typeof options.locale.daysOfWeek === 'object')
                this.locale.daysOfWeek = options.locale.daysOfWeek.slice();

            if (typeof options.locale.monthNames === 'object')
              this.locale.monthNames = options.locale.monthNames.slice();

            if (typeof options.locale.firstDay === 'number')
              this.locale.firstDay = options.locale.firstDay;

            if (typeof options.locale.applyLabel === 'string')
              this.locale.applyLabel = options.locale.applyLabel;

            if (typeof options.locale.cancelLabel === 'string')
              this.locale.cancelLabel = options.locale.cancelLabel;

            if (typeof options.locale.weekLabel === 'string')
              this.locale.weekLabel = options.locale.weekLabel;

            if (typeof options.locale.customRangeLabel === 'string'){
                //Support unicode chars in the custom range name.
                var elem = document.createElement('textarea');
                elem.innerHTML = options.locale.customRangeLabel;
                var rangeHtml = elem.value;
                this.locale.customRangeLabel = rangeHtml;
            }
        }
        this.container.addClass(this.locale.direction);

        if (typeof options.startDate === 'string')
            this.startDate = moment(options.startDate, this.locale.format);

        if (typeof options.endDate === 'string')
            this.endDate = moment(options.endDate, this.locale.format);

        if (typeof options.minDate === 'string')
            this.minDate = moment(options.minDate, this.locale.format);

        if (typeof options.maxDate === 'string')
            this.maxDate = moment(options.maxDate, this.locale.format);

        if (typeof options.startDate === 'object')
            this.startDate = moment(options.startDate);

        if (typeof options.endDate === 'object')
            this.endDate = moment(options.endDate);

        if (typeof options.minDate === 'object')
            this.minDate = moment(options.minDate);

        if (typeof options.maxDate === 'object')
            this.maxDate = moment(options.maxDate);

        // sanity check for bad options
        if (this.minDate && this.startDate.isBefore(this.minDate))
            this.startDate = this.minDate.clone();

        // sanity check for bad options
        if (this.maxDate && this.endDate.isAfter(this.maxDate))
            this.endDate = this.maxDate.clone();

        if (typeof options.applyClass === 'string')
            this.applyClass = options.applyClass;

        if (typeof options.cancelClass === 'string')
            this.cancelClass = options.cancelClass;

        if (typeof options.dateLimit === 'object')
            this.dateLimit = options.dateLimit;

        if (typeof options.opens === 'string')
            this.opens = options.opens;

        if (typeof options.drops === 'string')
            this.drops = options.drops;

        if (typeof options.showWeekNumbers === 'boolean')
            this.showWeekNumbers = options.showWeekNumbers;

        if (typeof options.showISOWeekNumbers === 'boolean')
            this.showISOWeekNumbers = options.showISOWeekNumbers;

        if (typeof options.buttonClasses === 'string')
            this.buttonClasses = options.buttonClasses;

        if (typeof options.buttonClasses === 'object')
            this.buttonClasses = options.buttonClasses.join(' ');

        if (typeof options.showDropdowns === 'boolean')
            this.showDropdowns = options.showDropdowns;

        if (typeof options.showCustomRangeLabel === 'boolean')
            this.showCustomRangeLabel = options.showCustomRangeLabel;

        if (typeof options.singleDatePicker === 'boolean') {
            this.singleDatePicker = options.singleDatePicker;
            if (this.singleDatePicker)
                this.endDate = this.startDate.clone();
        }

        if (typeof options.timePicker === 'boolean')
            this.timePicker = options.timePicker;

        if (typeof options.timePickerSeconds === 'boolean')
            this.timePickerSeconds = options.timePickerSeconds;

        if (typeof options.timePickerIncrement === 'number')
            this.timePickerIncrement = options.timePickerIncrement;

        if (typeof options.timePicker24Hour === 'boolean')
            this.timePicker24Hour = options.timePicker24Hour;

        if (typeof options.autoApply === 'boolean')
            this.autoApply = options.autoApply;

        if (typeof options.autoUpdateInput === 'boolean')
            this.autoUpdateInput = options.autoUpdateInput;

        if (typeof options.linkedCalendars === 'boolean')
            this.linkedCalendars = options.linkedCalendars;

        if (typeof options.isInvalidDate === 'function')
            this.isInvalidDate = options.isInvalidDate;

        if (typeof options.isCustomDate === 'function')
            this.isCustomDate = options.isCustomDate;

        if (typeof options.alwaysShowCalendars === 'boolean')
            this.alwaysShowCalendars = options.alwaysShowCalendars;

        // update day names order to firstDay
        if (this.locale.firstDay != 0) {
            var iterator = this.locale.firstDay;
            while (iterator > 0) {
                this.locale.daysOfWeek.push(this.locale.daysOfWeek.shift());
                iterator--;
            }
        }

        var start, end, range;

        //if no start/end dates set, check if an input element contains initial values
        if (typeof options.startDate === 'undefined' && typeof options.endDate === 'undefined') {
            if ($(this.element).is('input[type=text]')) {
                var val = $(this.element).val(),
                    split = val.split(this.locale.separator);

                start = end = null;

                if (split.length == 2) {
                    start = moment(split[0], this.locale.format);
                    end = moment(split[1], this.locale.format);
                } else if (this.singleDatePicker && val !== "") {
                    start = moment(val, this.locale.format);
                    end = moment(val, this.locale.format);
                }
                if (start !== null && end !== null) {
                    this.setStartDate(start);
                    this.setEndDate(end);
                }
            }
        }

        if (typeof options.ranges === 'object') {
            for (range in options.ranges) {

                if (typeof options.ranges[range][0] === 'string')
                    start = moment(options.ranges[range][0], this.locale.format);
                else
                    start = moment(options.ranges[range][0]);

                if (typeof options.ranges[range][1] === 'string')
                    end = moment(options.ranges[range][1], this.locale.format);
                else
                    end = moment(options.ranges[range][1]);

                // If the start or end date exceed those allowed by the minDate or dateLimit
                // options, shorten the range to the allowable period.
                if (this.minDate && start.isBefore(this.minDate))
                    start = this.minDate.clone();

                var maxDate = this.maxDate;
                if (this.dateLimit && maxDate && start.clone().add(this.dateLimit).isAfter(maxDate))
                    maxDate = start.clone().add(this.dateLimit);
                if (maxDate && end.isAfter(maxDate))
                    end = maxDate.clone();

                // If the end of the range is before the minimum or the start of the range is
                // after the maximum, don't display this range option at all.
                if ((this.minDate && end.isBefore(this.minDate, this.timepicker ? 'minute' : 'day')) 
                  || (maxDate && start.isAfter(maxDate, this.timepicker ? 'minute' : 'day')))
                    continue;

                //Support unicode chars in the range names.
                var elem = document.createElement('textarea');
                elem.innerHTML = range;
                var rangeHtml = elem.value;

                this.ranges[rangeHtml] = [start, end];
            }

            var list = '<ul>';
            for (range in this.ranges) {
                list += '<li data-range-key="' + range + '">' + range + '</li>';
            }
            if (this.showCustomRangeLabel) {
                list += '<li data-range-key="' + this.locale.customRangeLabel + '">' + this.locale.customRangeLabel + '</li>';
            }
            list += '</ul>';
            this.container.find('.ranges').prepend(list);
        }

        if (typeof cb === 'function') {
            this.callback = cb;
        }

        if (!this.timePicker) {
            this.startDate = this.startDate.startOf('day');
            this.endDate = this.endDate.endOf('day');
            this.container.find('.calendar-time').hide();
        }

        //can't be used together for now
        if (this.timePicker && this.autoApply)
            this.autoApply = false;

        if (this.autoApply && typeof options.ranges !== 'object') {
            this.container.find('.ranges').hide();
        } else if (this.autoApply) {
            this.container.find('.applyBtn, .cancelBtn').addClass('hide');
        }

        if (this.singleDatePicker) {
            this.container.addClass('single');
            this.container.find('.calendar.left').addClass('single');
            this.container.find('.calendar.left').show();
            this.container.find('.calendar.right').hide();
            this.container.find('.daterangepicker_input input, .daterangepicker_input > i').hide();
            if (this.timePicker) {
                this.container.find('.ranges ul').hide();
            } else {
                this.container.find('.ranges').hide();
            }
        }

        if ((typeof options.ranges === 'undefined' && !this.singleDatePicker) || this.alwaysShowCalendars) {
            this.container.addClass('show-calendar');
        }

        this.container.addClass('opens' + this.opens);

        //swap the position of the predefined ranges if opens right
        if (typeof options.ranges !== 'undefined' && this.opens == 'right') {
            this.container.find('.ranges').prependTo( this.container.find('.calendar.left').parent() );
        }

        //apply CSS classes and labels to buttons
        this.container.find('.applyBtn, .cancelBtn').addClass(this.buttonClasses);
        if (this.applyClass.length)
            this.container.find('.applyBtn').addClass(this.applyClass);
        if (this.cancelClass.length)
            this.container.find('.cancelBtn').addClass(this.cancelClass);
        this.container.find('.applyBtn').html(this.locale.applyLabel);
        this.container.find('.cancelBtn').html(this.locale.cancelLabel);

        //
        // event listeners
        //

        this.container.find('.calendar')
            .on('click.daterangepicker', '.prev', $.proxy(this.clickPrev, this))
            .on('click.daterangepicker', '.next', $.proxy(this.clickNext, this))
            .on('mousedown.daterangepicker', 'td.available', $.proxy(this.clickDate, this))
            .on('mouseenter.daterangepicker', 'td.available', $.proxy(this.hoverDate, this))
            .on('mouseleave.daterangepicker', 'td.available', $.proxy(this.updateFormInputs, this))
            .on('change.daterangepicker', 'select.yearselect', $.proxy(this.monthOrYearChanged, this))
            .on('change.daterangepicker', 'select.monthselect', $.proxy(this.monthOrYearChanged, this))
            .on('change.daterangepicker', 'select.hourselect,select.minuteselect,select.secondselect,select.ampmselect', $.proxy(this.timeChanged, this))
            .on('click.daterangepicker', '.daterangepicker_input input', $.proxy(this.showCalendars, this))
            .on('focus.daterangepicker', '.daterangepicker_input input', $.proxy(this.formInputsFocused, this))
            .on('blur.daterangepicker', '.daterangepicker_input input', $.proxy(this.formInputsBlurred, this))
            .on('change.daterangepicker', '.daterangepicker_input input', $.proxy(this.formInputsChanged, this))
            .on('keydown.daterangepicker', '.daterangepicker_input input', $.proxy(this.formInputsKeydown, this));

        this.container.find('.ranges')
            .on('click.daterangepicker', 'button.applyBtn', $.proxy(this.clickApply, this))
            .on('click.daterangepicker', 'button.cancelBtn', $.proxy(this.clickCancel, this))
            .on('click.daterangepicker', 'li', $.proxy(this.clickRange, this))
            .on('mouseenter.daterangepicker', 'li', $.proxy(this.hoverRange, this))
            .on('mouseleave.daterangepicker', 'li', $.proxy(this.updateFormInputs, this));

        if (this.element.is('input') || this.element.is('button')) {
            this.element.on({
                'click.daterangepicker': $.proxy(this.show, this),
                'focus.daterangepicker': $.proxy(this.show, this),
                'keyup.daterangepicker': $.proxy(this.elementChanged, this),
                'keydown.daterangepicker': $.proxy(this.keydown, this) //IE 11 compatibility
            });
        } else {
            this.element.on('click.daterangepicker', $.proxy(this.toggle, this));
            this.element.on('keydown.daterangepicker', $.proxy(this.toggle, this));
        }

        //
        // if attached to a text input, set the initial value
        //

        if (this.element.is('input') && !this.singleDatePicker && this.autoUpdateInput) {
            this.element.val(this.startDate.format(this.locale.format) + this.locale.separator + this.endDate.format(this.locale.format));
            this.element.trigger('change');
        } else if (this.element.is('input') && this.autoUpdateInput) {
            this.element.val(this.startDate.format(this.locale.format));
            this.element.trigger('change');
        }

    };

    DateRangePicker.prototype = {

        constructor: DateRangePicker,

        setStartDate: function(startDate) {
            if (typeof startDate === 'string')
                this.startDate = moment(startDate, this.locale.format);

            if (typeof startDate === 'object')
                this.startDate = moment(startDate);

            if (!this.timePicker)
                this.startDate = this.startDate.startOf('day');

            if (this.timePicker && this.timePickerIncrement)
                this.startDate.minute(Math.round(this.startDate.minute() / this.timePickerIncrement) * this.timePickerIncrement);

            if (this.minDate && this.startDate.isBefore(this.minDate)) {
                this.startDate = this.minDate.clone();
                if (this.timePicker && this.timePickerIncrement)
                    this.startDate.minute(Math.round(this.startDate.minute() / this.timePickerIncrement) * this.timePickerIncrement);
            }

            if (this.maxDate && this.startDate.isAfter(this.maxDate)) {
                this.startDate = this.maxDate.clone();
                if (this.timePicker && this.timePickerIncrement)
                    this.startDate.minute(Math.floor(this.startDate.minute() / this.timePickerIncrement) * this.timePickerIncrement);
            }

            if (!this.isShowing)
                this.updateElement();

            this.updateMonthsInView();
        },

        setEndDate: function(endDate) {
            if (typeof endDate === 'string')
                this.endDate = moment(endDate, this.locale.format);

            if (typeof endDate === 'object')
                this.endDate = moment(endDate);

            if (!this.timePicker)
                this.endDate = this.endDate.add(1,'d').startOf('day').subtract(1,'second');

            if (this.timePicker && this.timePickerIncrement)
                this.endDate.minute(Math.round(this.endDate.minute() / this.timePickerIncrement) * this.timePickerIncrement);

            if (this.endDate.isBefore(this.startDate))
                this.endDate = this.startDate.clone();

            if (this.maxDate && this.endDate.isAfter(this.maxDate))
                this.endDate = this.maxDate.clone();

            if (this.dateLimit && this.startDate.clone().add(this.dateLimit).isBefore(this.endDate))
                this.endDate = this.startDate.clone().add(this.dateLimit);

            this.previousRightTime = this.endDate.clone();

            if (!this.isShowing)
                this.updateElement();

            this.updateMonthsInView();
        },

        isInvalidDate: function() {
            return false;
        },

        isCustomDate: function() {
            return false;
        },

        updateView: function() {
            if (this.timePicker) {
                this.renderTimePicker('left');
                this.renderTimePicker('right');
                if (!this.endDate) {
                    this.container.find('.right .calendar-time select').attr('disabled', 'disabled').addClass('disabled');
                } else {
                    this.container.find('.right .calendar-time select').removeAttr('disabled').removeClass('disabled');
                }
            }
            if (this.endDate) {
                this.container.find('input[name="daterangepicker_end"]').removeClass('active');
                this.container.find('input[name="daterangepicker_start"]').addClass('active');
            } else {
                this.container.find('input[name="daterangepicker_end"]').addClass('active');
                this.container.find('input[name="daterangepicker_start"]').removeClass('active');
            }
            this.updateMonthsInView();
            this.updateCalendars();
            this.updateFormInputs();
        },

        updateMonthsInView: function() {
            if (this.endDate) {

                //if both dates are visible already, do nothing
                if (!this.singleDatePicker && this.leftCalendar.month && this.rightCalendar.month &&
                    (this.startDate.format('YYYY-MM') == this.leftCalendar.month.format('YYYY-MM') || this.startDate.format('YYYY-MM') == this.rightCalendar.month.format('YYYY-MM'))
                    &&
                    (this.endDate.format('YYYY-MM') == this.leftCalendar.month.format('YYYY-MM') || this.endDate.format('YYYY-MM') == this.rightCalendar.month.format('YYYY-MM'))
                    ) {
                    return;
                }

                this.leftCalendar.month = this.startDate.clone().date(2);
                if (!this.linkedCalendars && (this.endDate.month() != this.startDate.month() || this.endDate.year() != this.startDate.year())) {
                    this.rightCalendar.month = this.endDate.clone().date(2);
                } else {
                    this.rightCalendar.month = this.startDate.clone().date(2).add(1, 'month');
                }

            } else {
                if (this.leftCalendar.month.format('YYYY-MM') != this.startDate.format('YYYY-MM') && this.rightCalendar.month.format('YYYY-MM') != this.startDate.format('YYYY-MM')) {
                    this.leftCalendar.month = this.startDate.clone().date(2);
                    this.rightCalendar.month = this.startDate.clone().date(2).add(1, 'month');
                }
            }
            if (this.maxDate && this.linkedCalendars && !this.singleDatePicker && this.rightCalendar.month > this.maxDate) {
              this.rightCalendar.month = this.maxDate.clone().date(2);
              this.leftCalendar.month = this.maxDate.clone().date(2).subtract(1, 'month');
            }
        },

        updateCalendars: function() {

            if (this.timePicker) {
                var hour, minute, second;
                if (this.endDate) {
                    hour = parseInt(this.container.find('.left .hourselect').val(), 10);
                    minute = parseInt(this.container.find('.left .minuteselect').val(), 10);
                    second = this.timePickerSeconds ? parseInt(this.container.find('.left .secondselect').val(), 10) : 0;
                    if (!this.timePicker24Hour) {
                        var ampm = this.container.find('.left .ampmselect').val();
                        if (ampm === 'PM' && hour < 12)
                            hour += 12;
                        if (ampm === 'AM' && hour === 12)
                            hour = 0;
                    }
                } else {
                    hour = parseInt(this.container.find('.right .hourselect').val(), 10);
                    minute = parseInt(this.container.find('.right .minuteselect').val(), 10);
                    second = this.timePickerSeconds ? parseInt(this.container.find('.right .secondselect').val(), 10) : 0;
                    if (!this.timePicker24Hour) {
                        var ampm = this.container.find('.right .ampmselect').val();
                        if (ampm === 'PM' && hour < 12)
                            hour += 12;
                        if (ampm === 'AM' && hour === 12)
                            hour = 0;
                    }
                }
                this.leftCalendar.month.hour(hour).minute(minute).second(second);
                this.rightCalendar.month.hour(hour).minute(minute).second(second);
            }

            this.renderCalendar('left');
            this.renderCalendar('right');

            //highlight any predefined range matching the current start and end dates
            this.container.find('.ranges li').removeClass('active');
            if (this.endDate == null) return;

            this.calculateChosenLabel();
        },

        renderCalendar: function(side) {

            //
            // Build the matrix of dates that will populate the calendar
            //

            var calendar = side == 'left' ? this.leftCalendar : this.rightCalendar;
            var month = calendar.month.month();
            var year = calendar.month.year();
            var hour = calendar.month.hour();
            var minute = calendar.month.minute();
            var second = calendar.month.second();
            var daysInMonth = moment([year, month]).daysInMonth();
            var firstDay = moment([year, month, 1]);
            var lastDay = moment([year, month, daysInMonth]);
            var lastMonth = moment(firstDay).subtract(1, 'month').month();
            var lastYear = moment(firstDay).subtract(1, 'month').year();
            var daysInLastMonth = moment([lastYear, lastMonth]).daysInMonth();
            var dayOfWeek = firstDay.day();

            //initialize a 6 rows x 7 columns array for the calendar
            var calendar = [];
            calendar.firstDay = firstDay;
            calendar.lastDay = lastDay;

            for (var i = 0; i < 6; i++) {
                calendar[i] = [];
            }

            //populate the calendar with date objects
            var startDay = daysInLastMonth - dayOfWeek + this.locale.firstDay + 1;
            if (startDay > daysInLastMonth)
                startDay -= 7;

            if (dayOfWeek == this.locale.firstDay)
                startDay = daysInLastMonth - 6;

            var curDate = moment([lastYear, lastMonth, startDay, 12, minute, second]);

            var col, row;
            for (var i = 0, col = 0, row = 0; i < 42; i++, col++, curDate = moment(curDate).add(24, 'hour')) {
                if (i > 0 && col % 7 === 0) {
                    col = 0;
                    row++;
                }
                calendar[row][col] = curDate.clone().hour(hour).minute(minute).second(second);
                curDate.hour(12);

                if (this.minDate && calendar[row][col].format('YYYY-MM-DD') == this.minDate.format('YYYY-MM-DD') && calendar[row][col].isBefore(this.minDate) && side == 'left') {
                    calendar[row][col] = this.minDate.clone();
                }

                if (this.maxDate && calendar[row][col].format('YYYY-MM-DD') == this.maxDate.format('YYYY-MM-DD') && calendar[row][col].isAfter(this.maxDate) && side == 'right') {
                    calendar[row][col] = this.maxDate.clone();
                }

            }

            //make the calendar object available to hoverDate/clickDate
            if (side == 'left') {
                this.leftCalendar.calendar = calendar;
            } else {
                this.rightCalendar.calendar = calendar;
            }

            //
            // Display the calendar
            //

            var minDate = side == 'left' ? this.minDate : this.startDate;
            var maxDate = this.maxDate;
            var selected = side == 'left' ? this.startDate : this.endDate;
            var arrow = this.locale.direction == 'ltr' ? {left: 'chevron-left', right: 'chevron-right'} : {left: 'chevron-right', right: 'chevron-left'};

            var html = '<table class="table-condensed">';
            html += '<thead>';
            html += '<tr>';

            // add empty cell for week number
            if (this.showWeekNumbers || this.showISOWeekNumbers)
                html += '<th></th>';

            if ((!minDate || minDate.isBefore(calendar.firstDay)) && (!this.linkedCalendars || side == 'left')) {
                html += '<th class="prev available"><i class="fa fa-' + arrow.left + ' glyphicon glyphicon-' + arrow.left + '"></i></th>';
            } else {
                html += '<th></th>';
            }

            var dateHtml = this.locale.monthNames[calendar[1][1].month()] + calendar[1][1].format(" YYYY");

            if (this.showDropdowns) {
                var currentMonth = calendar[1][1].month();
                var currentYear = calendar[1][1].year();
                var maxYear = (maxDate && maxDate.year()) || (currentYear + 5);
                var minYear = (minDate && minDate.year()) || (currentYear - 50);
                var inMinYear = currentYear == minYear;
                var inMaxYear = currentYear == maxYear;

                var monthHtml = '<select class="monthselect">';
                for (var m = 0; m < 12; m++) {
                    if ((!inMinYear || m >= minDate.month()) && (!inMaxYear || m <= maxDate.month())) {
                        monthHtml += "<option value='" + m + "'" +
                            (m === currentMonth ? " selected='selected'" : "") +
                            ">" + this.locale.monthNames[m] + "</option>";
                    } else {
                        monthHtml += "<option value='" + m + "'" +
                            (m === currentMonth ? " selected='selected'" : "") +
                            " disabled='disabled'>" + this.locale.monthNames[m] + "</option>";
                    }
                }
                monthHtml += "</select>";

                var yearHtml = '<select class="yearselect">';
                for (var y = minYear; y <= maxYear; y++) {
                    yearHtml += '<option value="' + y + '"' +
                        (y === currentYear ? ' selected="selected"' : '') +
                        '>' + y + '</option>';
                }
                yearHtml += '</select>';

                dateHtml = monthHtml + yearHtml;
            }

            html += '<th colspan="5" class="month">' + dateHtml + '</th>';
            if ((!maxDate || maxDate.isAfter(calendar.lastDay)) && (!this.linkedCalendars || side == 'right' || this.singleDatePicker)) {
                html += '<th class="next available"><i class="fa fa-' + arrow.right + ' glyphicon glyphicon-' + arrow.right + '"></i></th>';
            } else {
                html += '<th></th>';
            }

            html += '</tr>';
            html += '<tr>';

            // add week number label
            if (this.showWeekNumbers || this.showISOWeekNumbers)
                html += '<th class="week">' + this.locale.weekLabel + '</th>';

            $.each(this.locale.daysOfWeek, function(index, dayOfWeek) {
                html += '<th>' + dayOfWeek + '</th>';
            });

            html += '</tr>';
            html += '</thead>';
            html += '<tbody>';

            //adjust maxDate to reflect the dateLimit setting in order to
            //grey out end dates beyond the dateLimit
            if (this.endDate == null && this.dateLimit) {
                var maxLimit = this.startDate.clone().add(this.dateLimit).endOf('day');
                if (!maxDate || maxLimit.isBefore(maxDate)) {
                    maxDate = maxLimit;
                }
            }

            for (var row = 0; row < 6; row++) {
                html += '<tr>';

                // add week number
                if (this.showWeekNumbers)
                    html += '<td class="week">' + calendar[row][0].week() + '</td>';
                else if (this.showISOWeekNumbers)
                    html += '<td class="week">' + calendar[row][0].isoWeek() + '</td>';

                for (var col = 0; col < 7; col++) {

                    var classes = [];

                    //highlight today's date
                    if (calendar[row][col].isSame(new Date(), "day"))
                        classes.push('today');

                    //highlight weekends
                    if (calendar[row][col].isoWeekday() > 5)
                        classes.push('weekend');

                    //grey out the dates in other months displayed at beginning and end of this calendar
                    if (calendar[row][col].month() != calendar[1][1].month())
                        classes.push('off');

                    //don't allow selection of dates before the minimum date
                    if (this.minDate && calendar[row][col].isBefore(this.minDate, 'day'))
                        classes.push('off', 'disabled');

                    //don't allow selection of dates after the maximum date
                    if (maxDate && calendar[row][col].isAfter(maxDate, 'day'))
                        classes.push('off', 'disabled');

                    //don't allow selection of date if a custom function decides it's invalid
                    if (this.isInvalidDate(calendar[row][col]))
                        classes.push('off', 'disabled');

                    //highlight the currently selected start date
                    if (calendar[row][col].format('YYYY-MM-DD') == this.startDate.format('YYYY-MM-DD'))
                        classes.push('active', 'start-date');

                    //highlight the currently selected end date
                    if (this.endDate != null && calendar[row][col].format('YYYY-MM-DD') == this.endDate.format('YYYY-MM-DD'))
                        classes.push('active', 'end-date');

                    //highlight dates in-between the selected dates
                    if (this.endDate != null && calendar[row][col] > this.startDate && calendar[row][col] < this.endDate)
                        classes.push('in-range');

                    //apply custom classes for this date
                    var isCustom = this.isCustomDate(calendar[row][col]);
                    if (isCustom !== false) {
                        if (typeof isCustom === 'string')
                            classes.push(isCustom);
                        else
                            Array.prototype.push.apply(classes, isCustom);
                    }

                    var cname = '', disabled = false;
                    for (var i = 0; i < classes.length; i++) {
                        cname += classes[i] + ' ';
                        if (classes[i] == 'disabled')
                            disabled = true;
                    }
                    if (!disabled)
                        cname += 'available';

                    html += '<td class="' + cname.replace(/^\s+|\s+$/g, '') + '" data-title="' + 'r' + row + 'c' + col + '">' + calendar[row][col].date() + '</td>';

                }
                html += '</tr>';
            }

            html += '</tbody>';
            html += '</table>';

            this.container.find('.calendar.' + side + ' .calendar-table').html(html);

        },

        renderTimePicker: function(side) {

            // Don't bother updating the time picker if it's currently disabled
            // because an end date hasn't been clicked yet
            if (side == 'right' && !this.endDate) return;

            var html, selected, minDate, maxDate = this.maxDate;

            if (this.dateLimit && (!this.maxDate || this.startDate.clone().add(this.dateLimit).isAfter(this.maxDate)))
                maxDate = this.startDate.clone().add(this.dateLimit);

            if (side == 'left') {
                selected = this.startDate.clone();
                minDate = this.minDate;
            } else if (side == 'right') {
                selected = this.endDate.clone();
                minDate = this.startDate;

                //Preserve the time already selected
                var timeSelector = this.container.find('.calendar.right .calendar-time div');
                if (timeSelector.html() != '') {

                    selected.hour(timeSelector.find('.hourselect option:selected').val() || selected.hour());
                    selected.minute(timeSelector.find('.minuteselect option:selected').val() || selected.minute());
                    selected.second(timeSelector.find('.secondselect option:selected').val() || selected.second());

                    if (!this.timePicker24Hour) {
                        var ampm = timeSelector.find('.ampmselect option:selected').val();
                        if (ampm === 'PM' && selected.hour() < 12)
                            selected.hour(selected.hour() + 12);
                        if (ampm === 'AM' && selected.hour() === 12)
                            selected.hour(0);
                    }

                }

                if (selected.isBefore(this.startDate))
                    selected = this.startDate.clone();

                if (maxDate && selected.isAfter(maxDate))
                    selected = maxDate.clone();

            }

            //
            // hours
            //

            html = '<select class="hourselect">';

            var start = this.timePicker24Hour ? 0 : 1;
            var end = this.timePicker24Hour ? 23 : 12;

            for (var i = start; i <= end; i++) {
                var i_in_24 = i;
                if (!this.timePicker24Hour)
                    i_in_24 = selected.hour() >= 12 ? (i == 12 ? 12 : i + 12) : (i == 12 ? 0 : i);

                var time = selected.clone().hour(i_in_24);
                var disabled = false;
                if (minDate && time.minute(59).isBefore(minDate))
                    disabled = true;
                if (maxDate && time.minute(0).isAfter(maxDate))
                    disabled = true;

                if (i_in_24 == selected.hour() && !disabled) {
                    html += '<option value="' + i + '" selected="selected">' + i + '</option>';
                } else if (disabled) {
                    html += '<option value="' + i + '" disabled="disabled" class="disabled">' + i + '</option>';
                } else {
                    html += '<option value="' + i + '">' + i + '</option>';
                }
            }

            html += '</select> ';

            //
            // minutes
            //

            html += ': <select class="minuteselect">';

            for (var i = 0; i < 60; i += this.timePickerIncrement) {
                var padded = i < 10 ? '0' + i : i;
                var time = selected.clone().minute(i);

                var disabled = false;
                if (minDate && time.second(59).isBefore(minDate))
                    disabled = true;
                if (maxDate && time.second(0).isAfter(maxDate))
                    disabled = true;

                if (selected.minute() == i && !disabled) {
                    html += '<option value="' + i + '" selected="selected">' + padded + '</option>';
                } else if (disabled) {
                    html += '<option value="' + i + '" disabled="disabled" class="disabled">' + padded + '</option>';
                } else {
                    html += '<option value="' + i + '">' + padded + '</option>';
                }
            }

            html += '</select> ';

            //
            // seconds
            //

            if (this.timePickerSeconds) {
                html += ': <select class="secondselect">';

                for (var i = 0; i < 60; i++) {
                    var padded = i < 10 ? '0' + i : i;
                    var time = selected.clone().second(i);

                    var disabled = false;
                    if (minDate && time.isBefore(minDate))
                        disabled = true;
                    if (maxDate && time.isAfter(maxDate))
                        disabled = true;

                    if (selected.second() == i && !disabled) {
                        html += '<option value="' + i + '" selected="selected">' + padded + '</option>';
                    } else if (disabled) {
                        html += '<option value="' + i + '" disabled="disabled" class="disabled">' + padded + '</option>';
                    } else {
                        html += '<option value="' + i + '">' + padded + '</option>';
                    }
                }

                html += '</select> ';
            }

            //
            // AM/PM
            //

            if (!this.timePicker24Hour) {
                html += '<select class="ampmselect">';

                var am_html = '';
                var pm_html = '';

                if (minDate && selected.clone().hour(12).minute(0).second(0).isBefore(minDate))
                    am_html = ' disabled="disabled" class="disabled"';

                if (maxDate && selected.clone().hour(0).minute(0).second(0).isAfter(maxDate))
                    pm_html = ' disabled="disabled" class="disabled"';

                if (selected.hour() >= 12) {
                    html += '<option value="AM"' + am_html + '>AM</option><option value="PM" selected="selected"' + pm_html + '>PM</option>';
                } else {
                    html += '<option value="AM" selected="selected"' + am_html + '>AM</option><option value="PM"' + pm_html + '>PM</option>';
                }

                html += '</select>';
            }

            this.container.find('.calendar.' + side + ' .calendar-time div').html(html);

        },

        updateFormInputs: function() {

            //ignore mouse movements while an above-calendar text input has focus
            if (this.container.find('input[name=daterangepicker_start]').is(":focus") || this.container.find('input[name=daterangepicker_end]').is(":focus"))
                return;

            this.container.find('input[name=daterangepicker_start]').val(this.startDate.format(this.locale.format));
            if (this.endDate)
                this.container.find('input[name=daterangepicker_end]').val(this.endDate.format(this.locale.format));

            if (this.singleDatePicker || (this.endDate && (this.startDate.isBefore(this.endDate) || this.startDate.isSame(this.endDate)))) {
                this.container.find('button.applyBtn').removeAttr('disabled');
            } else {
                this.container.find('button.applyBtn').attr('disabled', 'disabled');
            }

        },

        move: function() {
            var parentOffset = { top: 0, left: 0 },
                containerTop;
            var parentRightEdge = $(window).width();
            if (!this.parentEl.is('body')) {
                parentOffset = {
                    top: this.parentEl.offset().top - this.parentEl.scrollTop(),
                    left: this.parentEl.offset().left - this.parentEl.scrollLeft()
                };
                parentRightEdge = this.parentEl[0].clientWidth + this.parentEl.offset().left;
            }

            if (this.drops == 'up')
                containerTop = this.element.offset().top - this.container.outerHeight() - parentOffset.top;
            else
                containerTop = this.element.offset().top + this.element.outerHeight() - parentOffset.top;
            this.container[this.drops == 'up' ? 'addClass' : 'removeClass']('dropup');

            if (this.opens == 'left') {
                this.container.css({
                    top: containerTop,
                    right: parentRightEdge - this.element.offset().left - this.element.outerWidth(),
                    left: 'auto'
                });
                if (this.container.offset().left < 0) {
                    this.container.css({
                        right: 'auto',
                        left: 9
                    });
                }
            } else if (this.opens == 'center') {
                this.container.css({
                    top: containerTop,
                    left: this.element.offset().left - parentOffset.left + this.element.outerWidth() / 2
                            - this.container.outerWidth() / 2,
                    right: 'auto'
                });
                if (this.container.offset().left < 0) {
                    this.container.css({
                        right: 'auto',
                        left: 9
                    });
                }
            } else {
                this.container.css({
                    top: containerTop,
                    left: this.element.offset().left - parentOffset.left,
                    right: 'auto'
                });
                if (this.container.offset().left + this.container.outerWidth() > $(window).width()) {
                    this.container.css({
                        left: 'auto',
                        right: 0
                    });
                }
            }
        },

        show: function(e) {
            if (this.isShowing) return;

            // Create a click proxy that is private to this instance of datepicker, for unbinding
            this._outsideClickProxy = $.proxy(function(e) { this.outsideClick(e); }, this);

            // Bind global datepicker mousedown for hiding and
            $(document)
              .on('mousedown.daterangepicker', this._outsideClickProxy)
              // also support mobile devices
              .on('touchend.daterangepicker', this._outsideClickProxy)
              // also explicitly play nice with Bootstrap dropdowns, which stopPropagation when clicking them
              .on('click.daterangepicker', '[data-toggle=dropdown]', this._outsideClickProxy)
              // and also close when focus changes to outside the picker (eg. tabbing between controls)
              .on('focusin.daterangepicker', this._outsideClickProxy);

            // Reposition the picker if the window is resized while it's open
            $(window).on('resize.daterangepicker', $.proxy(function(e) { this.move(e); }, this));

            this.oldStartDate = this.startDate.clone();
            this.oldEndDate = this.endDate.clone();
            this.previousRightTime = this.endDate.clone();

            this.updateView();
            this.container.show();
            this.move();
            this.element.trigger('show.daterangepicker', this);
            this.isShowing = true;
        },

        hide: function(e) {
            if (!this.isShowing) return;

            //incomplete date selection, revert to last values
            if (!this.endDate) {
                this.startDate = this.oldStartDate.clone();
                this.endDate = this.oldEndDate.clone();
            }

            //if a new date range was selected, invoke the user callback function
            if (!this.startDate.isSame(this.oldStartDate) || !this.endDate.isSame(this.oldEndDate))
                this.callback(this.startDate, this.endDate, this.chosenLabel);

            //if picker is attached to a text input, update it
            this.updateElement();

            $(document).off('.daterangepicker');
            $(window).off('.daterangepicker');
            this.container.hide();
            this.element.trigger('hide.daterangepicker', this);
            this.isShowing = false;
        },

        toggle: function(e) {
            if (this.isShowing) {
                this.hide();
            } else {
                this.show();
            }
        },

        outsideClick: function(e) {
            var target = $(e.target);
            // if the page is clicked anywhere except within the daterangerpicker/button
            // itself then call this.hide()
            if (
                // ie modal dialog fix
                e.type == "focusin" ||
                target.closest(this.element).length ||
                target.closest(this.container).length ||
                target.closest('.calendar-table').length
                ) return;
            this.hide();
            this.element.trigger('outsideClick.daterangepicker', this);
        },

        showCalendars: function() {
            this.container.addClass('show-calendar');
            this.move();
            this.element.trigger('showCalendar.daterangepicker', this);
        },

        hideCalendars: function() {
            this.container.removeClass('show-calendar');
            this.element.trigger('hideCalendar.daterangepicker', this);
        },

        hoverRange: function(e) {

            //ignore mouse movements while an above-calendar text input has focus
            if (this.container.find('input[name=daterangepicker_start]').is(":focus") || this.container.find('input[name=daterangepicker_end]').is(":focus"))
                return;

            var label = e.target.getAttribute('data-range-key');

            if (label == this.locale.customRangeLabel) {
                this.updateView();
            } else {
                var dates = this.ranges[label];
                this.container.find('input[name=daterangepicker_start]').val(dates[0].format(this.locale.format));
                this.container.find('input[name=daterangepicker_end]').val(dates[1].format(this.locale.format));
            }

        },

        clickRange: function(e) {
            var label = e.target.getAttribute('data-range-key');
            this.chosenLabel = label;
            if (label == this.locale.customRangeLabel) {
                this.showCalendars();
            } else {
                var dates = this.ranges[label];
                this.startDate = dates[0];
                this.endDate = dates[1];

                if (!this.timePicker) {
                    this.startDate.startOf('day');
                    this.endDate.endOf('day');
                }

                if (!this.alwaysShowCalendars)
                    this.hideCalendars();
                this.clickApply();
            }
        },

        clickPrev: function(e) {
            var cal = $(e.target).parents('.calendar');
            if (cal.hasClass('left')) {
                this.leftCalendar.month.subtract(1, 'month');
                if (this.linkedCalendars)
                    this.rightCalendar.month.subtract(1, 'month');
            } else {
                this.rightCalendar.month.subtract(1, 'month');
            }
            this.updateCalendars();
        },

        clickNext: function(e) {
            var cal = $(e.target).parents('.calendar');
            if (cal.hasClass('left')) {
                this.leftCalendar.month.add(1, 'month');
            } else {
                this.rightCalendar.month.add(1, 'month');
                if (this.linkedCalendars)
                    this.leftCalendar.month.add(1, 'month');
            }
            this.updateCalendars();
        },

        hoverDate: function(e) {

            //ignore mouse movements while an above-calendar text input has focus
            //if (this.container.find('input[name=daterangepicker_start]').is(":focus") || this.container.find('input[name=daterangepicker_end]').is(":focus"))
            //    return;

            //ignore dates that can't be selected
            if (!$(e.target).hasClass('available')) return;

            //have the text inputs above calendars reflect the date being hovered over
            var title = $(e.target).attr('data-title');
            var row = title.substr(1, 1);
            var col = title.substr(3, 1);
            var cal = $(e.target).parents('.calendar');
            var date = cal.hasClass('left') ? this.leftCalendar.calendar[row][col] : this.rightCalendar.calendar[row][col];

            if (this.endDate && !this.container.find('input[name=daterangepicker_start]').is(":focus")) {
                this.container.find('input[name=daterangepicker_start]').val(date.format(this.locale.format));
            } else if (!this.endDate && !this.container.find('input[name=daterangepicker_end]').is(":focus")) {
                this.container.find('input[name=daterangepicker_end]').val(date.format(this.locale.format));
            }

            //highlight the dates between the start date and the date being hovered as a potential end date
            var leftCalendar = this.leftCalendar;
            var rightCalendar = this.rightCalendar;
            var startDate = this.startDate;
            if (!this.endDate) {
                this.container.find('.calendar tbody td').each(function(index, el) {

                    //skip week numbers, only look at dates
                    if ($(el).hasClass('week')) return;

                    var title = $(el).attr('data-title');
                    var row = title.substr(1, 1);
                    var col = title.substr(3, 1);
                    var cal = $(el).parents('.calendar');
                    var dt = cal.hasClass('left') ? leftCalendar.calendar[row][col] : rightCalendar.calendar[row][col];

                    if ((dt.isAfter(startDate) && dt.isBefore(date)) || dt.isSame(date, 'day')) {
                        $(el).addClass('in-range');
                    } else {
                        $(el).removeClass('in-range');
                    }

                });
            }

        },

        clickDate: function(e) {

            if (!$(e.target).hasClass('available')) return;

            var title = $(e.target).attr('data-title');
            var row = title.substr(1, 1);
            var col = title.substr(3, 1);
            var cal = $(e.target).parents('.calendar');
            var date = cal.hasClass('left') ? this.leftCalendar.calendar[row][col] : this.rightCalendar.calendar[row][col];

            //
            // this function needs to do a few things:
            // * alternate between selecting a start and end date for the range,
            // * if the time picker is enabled, apply the hour/minute/second from the select boxes to the clicked date
            // * if autoapply is enabled, and an end date was chosen, apply the selection
            // * if single date picker mode, and time picker isn't enabled, apply the selection immediately
            // * if one of the inputs above the calendars was focused, cancel that manual input
            //

            if (this.endDate || date.isBefore(this.startDate, 'day')) { //picking start
                if (this.timePicker) {
                    var hour = parseInt(this.container.find('.left .hourselect').val(), 10);
                    if (!this.timePicker24Hour) {
                        var ampm = this.container.find('.left .ampmselect').val();
                        if (ampm === 'PM' && hour < 12)
                            hour += 12;
                        if (ampm === 'AM' && hour === 12)
                            hour = 0;
                    }
                    var minute = parseInt(this.container.find('.left .minuteselect').val(), 10);
                    var second = this.timePickerSeconds ? parseInt(this.container.find('.left .secondselect').val(), 10) : 0;
                    date = date.clone().hour(hour).minute(minute).second(second);
                }
                this.endDate = null;
                this.setStartDate(date.clone());
            } else if (!this.endDate && date.isBefore(this.startDate)) {
                //special case: clicking the same date for start/end,
                //but the time of the end date is before the start date
                this.setEndDate(this.startDate.clone());
            } else { // picking end
                if (this.timePicker) {
                    var hour = parseInt(this.container.find('.right .hourselect').val(), 10);
                    if (!this.timePicker24Hour) {
                        var ampm = this.container.find('.right .ampmselect').val();
                        if (ampm === 'PM' && hour < 12)
                            hour += 12;
                        if (ampm === 'AM' && hour === 12)
                            hour = 0;
                    }
                    var minute = parseInt(this.container.find('.right .minuteselect').val(), 10);
                    var second = this.timePickerSeconds ? parseInt(this.container.find('.right .secondselect').val(), 10) : 0;
                    date = date.clone().hour(hour).minute(minute).second(second);
                }
                this.setEndDate(date.clone());
                if (this.autoApply) {
                  this.calculateChosenLabel();
                  this.clickApply();
                }
            }

            if (this.singleDatePicker) {
                this.setEndDate(this.startDate);
                if (!this.timePicker)
                    this.clickApply();
            }

            this.updateView();

            //This is to cancel the blur event handler if the mouse was in one of the inputs
            e.stopPropagation();

        },

        calculateChosenLabel: function () {
            var customRange = true;
            var i = 0;
            for (var range in this.ranges) {
              if (this.timePicker) {
                    var format = this.timePickerSeconds ? "YYYY-MM-DD hh:mm:ss" : "YYYY-MM-DD hh:mm";
                    //ignore times when comparing dates if time picker seconds is not enabled
                    if (this.startDate.format(format) == this.ranges[range][0].format(format) && this.endDate.format(format) == this.ranges[range][1].format(format)) {
                        customRange = false;
                        this.chosenLabel = this.container.find('.ranges li:eq(' + i + ')').addClass('active').html();
                        break;
                    }
                } else {
                    //ignore times when comparing dates if time picker is not enabled
                    if (this.startDate.format('YYYY-MM-DD') == this.ranges[range][0].format('YYYY-MM-DD') && this.endDate.format('YYYY-MM-DD') == this.ranges[range][1].format('YYYY-MM-DD')) {
                        customRange = false;
                        this.chosenLabel = this.container.find('.ranges li:eq(' + i + ')').addClass('active').html();
                        break;
                    }
                }
                i++;
            }
            if (customRange) {
                if (this.showCustomRangeLabel) {
                    this.chosenLabel = this.container.find('.ranges li:last').addClass('active').html();
                } else {
                    this.chosenLabel = null;
                }
                this.showCalendars();
            }
        },

        clickApply: function(e) {
            this.hide();
            this.element.trigger('apply.daterangepicker', this);
        },

        clickCancel: function(e) {
            this.startDate = this.oldStartDate;
            this.endDate = this.oldEndDate;
            this.hide();
            this.element.trigger('cancel.daterangepicker', this);
        },

        monthOrYearChanged: function(e) {
            var isLeft = $(e.target).closest('.calendar').hasClass('left'),
                leftOrRight = isLeft ? 'left' : 'right',
                cal = this.container.find('.calendar.'+leftOrRight);

            // Month must be Number for new moment versions
            var month = parseInt(cal.find('.monthselect').val(), 10);
            var year = cal.find('.yearselect').val();

            if (!isLeft) {
                if (year < this.startDate.year() || (year == this.startDate.year() && month < this.startDate.month())) {
                    month = this.startDate.month();
                    year = this.startDate.year();
                }
            }

            if (this.minDate) {
                if (year < this.minDate.year() || (year == this.minDate.year() && month < this.minDate.month())) {
                    month = this.minDate.month();
                    year = this.minDate.year();
                }
            }

            if (this.maxDate) {
                if (year > this.maxDate.year() || (year == this.maxDate.year() && month > this.maxDate.month())) {
                    month = this.maxDate.month();
                    year = this.maxDate.year();
                }
            }

            if (isLeft) {
                this.leftCalendar.month.month(month).year(year);
                if (this.linkedCalendars)
                    this.rightCalendar.month = this.leftCalendar.month.clone().add(1, 'month');
            } else {
                this.rightCalendar.month.month(month).year(year);
                if (this.linkedCalendars)
                    this.leftCalendar.month = this.rightCalendar.month.clone().subtract(1, 'month');
            }
            this.updateCalendars();
        },

        timeChanged: function(e) {

            var cal = $(e.target).closest('.calendar'),
                isLeft = cal.hasClass('left');

            var hour = parseInt(cal.find('.hourselect').val(), 10);
            var minute = parseInt(cal.find('.minuteselect').val(), 10);
            var second = this.timePickerSeconds ? parseInt(cal.find('.secondselect').val(), 10) : 0;

            if (!this.timePicker24Hour) {
                var ampm = cal.find('.ampmselect').val();
                if (ampm === 'PM' && hour < 12)
                    hour += 12;
                if (ampm === 'AM' && hour === 12)
                    hour = 0;
            }

            if (isLeft) {
                var start = this.startDate.clone();
                start.hour(hour);
                start.minute(minute);
                start.second(second);
                this.setStartDate(start);
                if (this.singleDatePicker) {
                    this.endDate = this.startDate.clone();
                } else if (this.endDate && this.endDate.format('YYYY-MM-DD') == start.format('YYYY-MM-DD') && this.endDate.isBefore(start)) {
                    this.setEndDate(start.clone());
                }
            } else if (this.endDate) {
                var end = this.endDate.clone();
                end.hour(hour);
                end.minute(minute);
                end.second(second);
                this.setEndDate(end);
            }

            //update the calendars so all clickable dates reflect the new time component
            this.updateCalendars();

            //update the form inputs above the calendars with the new time
            this.updateFormInputs();

            //re-render the time pickers because changing one selection can affect what's enabled in another
            this.renderTimePicker('left');
            this.renderTimePicker('right');

        },

        formInputsChanged: function(e) {
            var isRight = $(e.target).closest('.calendar').hasClass('right');
            var start = moment(this.container.find('input[name="daterangepicker_start"]').val(), this.locale.format);
            var end = moment(this.container.find('input[name="daterangepicker_end"]').val(), this.locale.format);

            if (start.isValid() && end.isValid()) {

                if (isRight && end.isBefore(start))
                    start = end.clone();

                this.setStartDate(start);
                this.setEndDate(end);

                if (isRight) {
                    this.container.find('input[name="daterangepicker_start"]').val(this.startDate.format(this.locale.format));
                } else {
                    this.container.find('input[name="daterangepicker_end"]').val(this.endDate.format(this.locale.format));
                }

            }

            this.updateView();
        },

        formInputsFocused: function(e) {

            // Highlight the focused input
            this.container.find('input[name="daterangepicker_start"], input[name="daterangepicker_end"]').removeClass('active');
            $(e.target).addClass('active');

            // Set the state such that if the user goes back to using a mouse, 
            // the calendars are aware we're selecting the end of the range, not
            // the start. This allows someone to edit the end of a date range without
            // re-selecting the beginning, by clicking on the end date input then
            // using the calendar.
            var isRight = $(e.target).closest('.calendar').hasClass('right');
            if (isRight) {
                this.endDate = null;
                this.setStartDate(this.startDate.clone());
                this.updateView();
            }

        },

        formInputsBlurred: function(e) {

            // this function has one purpose right now: if you tab from the first
            // text input to the second in the UI, the endDate is nulled so that
            // you can click another, but if you tab out without clicking anything
            // or changing the input value, the old endDate should be retained

            if (!this.endDate) {
                var val = this.container.find('input[name="daterangepicker_end"]').val();
                var end = moment(val, this.locale.format);
                if (end.isValid()) {
                    this.setEndDate(end);
                    this.updateView();
                }
            }

        },

        formInputsKeydown: function(e) {
            // This function ensures that if the 'enter' key was pressed in the input, then the calendars
            // are updated with the startDate and endDate.
            // This behaviour is automatic in Chrome/Firefox/Edge but not in IE 11 hence why this exists.
            // Other browsers and versions of IE are untested and the behaviour is unknown.
            if (e.keyCode === 13) {
                // Prevent the calendar from being updated twice on Chrome/Firefox/Edge
                e.preventDefault(); 
                this.formInputsChanged(e);
            }
        },


        elementChanged: function() {
            if (!this.element.is('input')) return;
            if (!this.element.val().length) return;

            var dateString = this.element.val().split(this.locale.separator),
                start = null,
                end = null;

            if (dateString.length === 2) {
                start = moment(dateString[0], this.locale.format);
                end = moment(dateString[1], this.locale.format);
            }

            if (this.singleDatePicker || start === null || end === null) {
                start = moment(this.element.val(), this.locale.format);
                end = start;
            }

            if (!start.isValid() || !end.isValid()) return;

            this.setStartDate(start);
            this.setEndDate(end);
            this.updateView();
        },

        keydown: function(e) {
            //hide on tab or enter
            if ((e.keyCode === 9) || (e.keyCode === 13)) {
                this.hide();
            }

            //hide on esc and prevent propagation
            if (e.keyCode === 27) {
                e.preventDefault();
                e.stopPropagation();

                this.hide();
            }
        },

        updateElement: function() {
            if (this.element.is('input') && !this.singleDatePicker && this.autoUpdateInput) {
                this.element.val(this.startDate.format(this.locale.format) + this.locale.separator + this.endDate.format(this.locale.format));
                this.element.trigger('change');
            } else if (this.element.is('input') && this.autoUpdateInput) {
                this.element.val(this.startDate.format(this.locale.format));
                this.element.trigger('change');
            }
        },

        remove: function() {
            this.container.remove();
            this.element.off('.daterangepicker');
            this.element.removeData();
        }

    };

    $.fn.daterangepicker = function(options, callback) {
        var implementOptions = $.extend(true, {}, $.fn.daterangepicker.defaultOptions, options);
        this.each(function() {
            var el = $(this);
            if (el.data('daterangepicker'))
                el.data('daterangepicker').remove();
            el.data('daterangepicker', new DateRangePicker(el, implementOptions, callback));
        });
        return this;
    };

    return DateRangePicker;

}));


/***/ }),

/***/ "./www/js/source/components/jquery.flatpopup.js":
/*!******************************************************!*\
  !*** ./www/js/source/components/jquery.flatpopup.js ***!
  \******************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;﻿
!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(/*! jquery */ "./node_modules/jquery/dist/jquery.js-exposed")], __WEBPACK_AMD_DEFINE_RESULT__ = (function ($) {
    let $blockBody;
    
    const _methods = {
        init: function (inputOptions) {
            const defaultOptions = {
                width: 640,
                height: 480,
                padding: 0,
                id: 'popup-content',
                className: '',
                title: '&nbsp;',
                container: 'body',
                modal: true,
                onOpen: null,
                onOpened: null,
                onClose: null
            };

            $blockBody = $('.popup-block-body');
            const options = $.extend(defaultOptions, inputOptions || {});
            return this.each(function () {
                let $self = $(this);
                const $container = $(options.container);

                if (options.modal && $blockBody.length === 0) {
                    $blockBody = $('<div class="popup-block-body" style="display: none"></div>');
                    $container.append($blockBody);
                }
                
                const $popupElement = $('#' + options.id);
                if ($popupElement.length === 0 || $popupElement.hasClass('flat-popup') === false) {
                    $self.attr('id', options.id);
                    $self.addClass('flat-popup ' + options.className);
                    $self.children().wrapAll('<div class="wrapContent" style="padding:' + options.padding + 'px"></div>');
                    $self.hide()
                         .prepend('<div class="title-bar popup-title"><div class="txt-title">'
                             + options.title
                             + '</div><a class="popup-close" href="javascript:;"><i class="icon-close"></i></a></div>');

                    $self.find('a.popup-close').on('click', function () {
                        if (options.onClose) {
                            options.onClose.apply($self);
                        }

                        if ($self.find('iframe').length > 0) {
                            $self.find('.wrapContent').empty();
                        }
                        _methods.hide.apply($self);
                    });

                    $container.append($self);
                }
                else {
                    $self = $('#' + options.id);
                    $self.find('.txt-title').text(options.title);
                    const $wrapContent = $self.find('.wrapContent');
                    $wrapContent.append($self.children().not('.popup-title'));
                }

                $self.width(options.width);
                $self.height(options.height);

                if (options.onOpen) {
                    options.onOpen.apply($self);
                }

                _methods.show.call($self, options.modal);

                if (options.onOpened) {
                    setTimeout(options.onOpened.apply($self), 350);
                }
            });
        },

        show: function (modal) {
            const $self = this;
            //Set the center alignment padding + border see css style
            const left = ($(window).width() - $self.width()) / 2;
            let top = ($(window).height() - $self.height()) / 2;

            if (top > 300) {
                top -= 100;
            }

            $self.css({
                'top': (top < 0 ? 0 : top) + 'px',
                'left': (left < 0 ? 0 : left) + 'px'
            });

            if (modal === true || modal === undefined) {
                $blockBody.show();
            }

            $self.fadeIn(300);
        },

        hide: function () {
            $(this).fadeOut(300);
            $blockBody.hide();
            return false;
        }
    };

    $.fn.flatPopup = function (methodOrOptions) {
        if (_methods[methodOrOptions]) {
            return _methods[methodOrOptions].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof methodOrOptions === 'object' || !methodOrOptions) {
            // Default to 'init'
            return _methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + methodOrOptions + ' does not exist on jQuery.flatPopup');
        }
    };
}).apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ }),

/***/ "./www/js/source/components/pluginhelper/daterangepicker.js":
/*!******************************************************************!*\
  !*** ./www/js/source/components/pluginhelper/daterangepicker.js ***!
  \******************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;﻿!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(/*! jquery */ "./node_modules/jquery/dist/jquery.js-exposed"), __webpack_require__(/*! common/site */ "./www/js/source/common/site.js"), __webpack_require__(/*! moment */ "./node_modules/moment/moment.js"), __webpack_require__(/*! bootstrap-daterangepicker */ "./node_modules/bootstrap-daterangepicker/daterangepicker.js")], __WEBPACK_AMD_DEFINE_RESULT__ = (function ($, Site, moment) {
        'use strict';

        let _daysOfWeek, _monthNames;

        function initCommonTextResource() {
            if (!_daysOfWeek || !_monthNames) {
                _daysOfWeek = [
                    Site.getSingleResource('Su'), Site.getSingleResource('Mo'), Site.getSingleResource('Tu'), Site.getSingleResource('We'),
                    Site.getSingleResource('Th'), Site.getSingleResource('Fr'), Site.getSingleResource('Sa')
                ];

                _monthNames = [
                    Site.getSingleResource('Jan'), Site.getSingleResource('Feb'), Site.getSingleResource('Mar'), Site.getSingleResource('Apr'),
                    Site.getSingleResource('May'), Site.getSingleResource('Jun'), Site.getSingleResource('Jul'), Site.getSingleResource('Aug'),
                    Site.getSingleResource('Sep'), Site.getSingleResource('Oct'), Site.getSingleResource('Nov'), Site.getSingleResource('Dec')
                ];
            }
        }

        function initDateRangePicker(firstDayOfWeekIsMonday, rangeOption) {
            const $dateRange = $('#DateRange');

            const $from = $('#FromDate');
            const $to = $('#ToDate');
            const start = moment($from.attr('data-original-from'), 'MM/DD/YYYY');
            const end = moment($to.attr('data-original-to'), 'MM/DD/YYYY');

            function setRangeDate(start, end) {
                $from.val(start.format('MM/DD/YYYY'));
                $to.val(end.format('MM/DD/YYYY'));
                $dateRange.val(start.format('MM/DD/YYYY') + ' - ' + end.format('MM/DD/YYYY'));
            }

            const now = moment($dateRange.attr('data-today'), 'MM/DD/YYYY');
            const isoDayOfWeek = firstDayOfWeekIsMonday ? 1 : 7; // 1.Monday  7.Sunday
            const startDayOfWeek = now.clone().startOf('week').isoWeekday(isoDayOfWeek);
            const startDayOfLastWeek = now.clone().subtract(1, 'week').startOf('week').isoWeekday(isoDayOfWeek);

            const ranges = {};
            ranges[Site.getSingleResource('Today')] = [now, now];
            ranges[Site.getSingleResource('Yesterday')] = [now.clone().subtract(1, 'days'), now.clone().subtract(1, 'days')];
            ranges[Site.getSingleResource('Currentweek')] = [startDayOfWeek, now];
            ranges[Site.getSingleResource('Lastweek')] = [startDayOfLastWeek, startDayOfLastWeek.clone().add(6, 'days')];
            ranges[Site.getSingleResource('Currentmonth')] = [now.clone().startOf('month'), now];
            ranges[Site.getSingleResource('Lastmonth')] = [now.clone().subtract(1, 'month').startOf('month'), now.clone().subtract(1, 'month').endOf('month')];
            ranges[Site.getSingleResource('Sincelastmonth')] = [now.clone().subtract(1, 'month').startOf('month'), now];

            const customRanges = {};
            const hasRangeOption = rangeOption && rangeOption.length > 0;
            if (hasRangeOption) {
                for (let i = 0; i < rangeOption.length; i++) {
                    const rangeLabel = Site.getSingleResource(rangeOption[i]);
                    customRanges[rangeLabel] = ranges[rangeLabel];
                }
            }

            initCommonTextResource();
            const locale = {
                format: 'MM/DD/YYYY',
                applyLabel: Site.getSingleResource('OK'),
                cancelLabel: Site.getSingleResource('Cancel'),
                daysOfWeek: _daysOfWeek,
                monthNames: _monthNames,
                firstDay: firstDayOfWeekIsMonday ? 1 : 0
            };

            $dateRange.daterangepicker({
                startDate: start,
                endDate: end,
                minDate: moment($dateRange.attr('data-mindate'), 'MM/DD/YYYY'),
                maxDate: moment($dateRange.attr('data-maxdate'), 'MM/DD/YYYY'),
                ranges: hasRangeOption ? customRanges : ranges,
                locale: locale,
                showDropdowns: true,
                linkedCalendars: false,
                alwaysShowCalendars: true,
                showCustomRangeLabel: false
            }, setRangeDate);

            $dateRange.on('apply.daterangepicker', function (ev, picker) {
                setRangeDate(picker.startDate, picker.endDate);
            });

            $dateRange.on('show.daterangepicker', function (ev, picker) {
                const toDate = picker.endDate || new Date();
                const toMoment = moment(toDate);

                picker.rightCalendar.month = toMoment;
                picker.renderCalendar('right');
            });

            $('.date-range').on('click', '.icon-calendar', function () {
                $dateRange.data('daterangepicker').toggle();
            });

            setRangeDate(start, end);
        }

        function initSingleDatePicker(idElement, parentElement) {
            const $picker = $('#' + idElement);
            initCommonTextResource();
            const locale = {
                format: 'MM/DD/YYYY',
                daysOfWeek: _daysOfWeek,
                monthNames: _monthNames
            };

            $picker.daterangepicker({
                startDate: moment($picker.attr('data-original'), 'MM/DD/YYYY'),
                endDate: moment($picker.attr('data-original'), 'MM/DD/YYYY'),
                minDate: moment($picker.attr('data-mindate'), 'MM/DD/YYYY'),
                maxDate: moment($picker.attr('data-maxdate'), 'MM/DD/YYYY'),
                locale: locale,
                singleDatePicker: true,
                showDropdowns: true,
                parentEl: parentElement ? parentElement : '.single-date'
            });

            $('.single-date').on('click', '.icon-calendar', function () {
                $picker.data('daterangepicker').toggle();
            });
        }

        function setValueForDateRange(startDate, endDate) {
            if (startDate) {
                $('#DateRange').data('daterangepicker').setStartDate(startDate);
                $('#FromDate').val(startDate);
            }

            if (endDate) {
                $('#DateRange').data('daterangepicker').setEndDate(endDate);
                $('#ToDate').val(startDate);
            }
        }

        function setValueForSingleDate($element, dateValue) {
            $element.val(dateValue);
        }

        return {
            initDateRangePicker: initDateRangePicker,
            initSingleDatePicker: initSingleDatePicker,
            setValueForDateRange: setValueForDateRange,
            setValueForSingleDate: setValueForSingleDate
        };
    }).apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ }),

/***/ "./www/js/source/components/scrollablegrid.js":
/*!****************************************************!*\
  !*** ./www/js/source/components/scrollablegrid.js ***!
  \****************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;﻿
!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(/*! jquery */ "./node_modules/jquery/dist/jquery.js-exposed")], __WEBPACK_AMD_DEFINE_RESULT__ = (function ($) {
    const _me = {
        resizeGrid: function ($scrollableContent) {
            const $grid = $scrollableContent || $('.body-content');
            if ($grid.length === 0) {
                return;
            }

            _me.updateHeight($grid);
            _me.updateWidth($grid);
        },

        updateHeight: function ($grid) {
            const gridTop = $grid.offset().top;
            const footerSite = 18;
            const footerHeight = $('.footer-content').length > 0 ? $('.footer-content').height() : 0;
            const pagingHeight = $('.pagingContainer').length > 0 ? $('.pagingContainer').height() : 0;

            let gridHeight = window.innerHeight - gridTop - footerHeight - pagingHeight - footerSite - 10;
            gridHeight = Math.max(150, gridHeight);
            $grid.css('max-height', gridHeight);
        },

        updateWidth: function ($grid) {
            if (!$grid.hasVerticalScrollBar()) {
                $grid.addClass('noScroll').find('table').css('width', '');
            }
            else {
                $grid.removeClass('noScroll').find('table').css('width', $grid.width() - 17);
            }
        }
    };

    return _me;
}).apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ }),

/***/ "./www/js/source/components/selectlist.js":
/*!************************************************!*\
  !*** ./www/js/source/components/selectlist.js ***!
  \************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;﻿!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(/*! jquery */ "./node_modules/jquery/dist/jquery.js-exposed")], __WEBPACK_AMD_DEFINE_RESULT__ = (function ($) {
    'use strict';

    const CONTAINER_SELECTOR = '.select-list-container';
    const DISABLED_CLASS = 'disabled';
    const HIGHLIGHT_CLASS = 'highlighted';

    function getLabelHtml(option) {
        const targetDropdownId = option.selectionId + '-result-container';
        const className = option.isDisabled ? 'selection disabled' : 'selection';
        return '<div class="select-list-container">' +
                '<div class="' + className + '" id="' + option.selectionId + '" aria-targetDropdown="' + targetDropdownId + '">' +
                    '<div class="selection-label">' + option.labelText + '</div>' +
                    '<div class="selection-icon"><span class="selection-icon-arrow"></span></div>' +
                '</div>' +
            '</div>';
    }

    function getSearchBoxHtml(isShow) {
        return isShow ? '<div class="select-list-dropdown-search"><input class="select-list-search-results" autocomplete="off" /></div>' : '';
    }

    function getOptionItemHtml(dataItem) {
        return '<li class="option-item ' + dataItem.className + '" data-key="' + dataItem.key + '">' + dataItem.value + '</li>';
    }

    function getOptionListHtml(dataList) {
        const htmlArray = ['<div class="select-list-dropdown-results"><ul class="option-list">'];

        for (let i = 0; i < dataList.length; i++) {
            const dataItem = dataList[i];
            htmlArray.push(getOptionItemHtml(dataItem));
        }

        htmlArray.push('</ul></div>');

        return htmlArray.join('');
    }

    function scrollToHighLightItem($container, $item, index) {
        const itemTop = $item.offset().top;
        const itemHeight = $item.outerHeight();
        const containerTop = $container.offset().top;
        let scrollUp = null;
        if (itemTop <= containerTop) {
            scrollUp = true;
        }
        else if (itemTop >= containerTop + $container.height()) {
            scrollUp = false;
        }
        if (scrollUp === null) {
            return;
        }
        if (scrollUp) {
            $container.scrollTop(itemHeight * (index - 1) - $container.height());
        }
        else {
            $container.scrollTop(itemHeight * index);
        }
    }

    function keyUp($element) {
        const $selectList = $element.closest('.select-list-dropdown').find('.select-list-dropdown-results');
        const showOptionList = $selectList.find('.option-item:visible');
        const $highlightItem = $selectList.find('.option-item.' + HIGHLIGHT_CLASS + ':visible').first();
        const index = showOptionList.index($highlightItem);

        $highlightItem.removeClass(HIGHLIGHT_CLASS);
        if ($highlightItem.length > 0 && index !== 0) {
            const $preItem = showOptionList.eq(index - 1);
            $preItem.addClass(HIGHLIGHT_CLASS);
            scrollToHighLightItem($selectList, $preItem, index - 1);
        }
        else {
            showOptionList.last().addClass(HIGHLIGHT_CLASS);
            $selectList.scrollTop(showOptionList.length * $highlightItem.outerHeight());
        }
    }

    function keyDown($element) {
        const $selectList = $element.closest('.select-list-dropdown').find('.select-list-dropdown-results');
        const showOptionList = $selectList.find('.option-item:visible');
        const $highlightItem = $selectList.find('.option-item.' + HIGHLIGHT_CLASS + ':visible').first();
        const index = showOptionList.index($highlightItem);

        $highlightItem.removeClass(HIGHLIGHT_CLASS);
        if ($highlightItem.length > 0 && index !== (showOptionList.length - 1)) {
            const $nextItem = showOptionList.eq(index + 1);
            $nextItem.addClass(HIGHLIGHT_CLASS);
            scrollToHighLightItem($selectList, $nextItem, index + 1);
        }
        else {
            showOptionList.first().addClass(HIGHLIGHT_CLASS);
            $selectList.scrollTop(0);
        }
    }

    function keySearch($element, canEnter) {
        setTimeout(function () {
            const query = $element.val().toUpperCase();
            const $selectList = $element.closest('.select-list-dropdown').find('.select-list-dropdown-results');
            const optionList = $selectList.find('.option-item');
            let isFirstItem = canEnter;
            if (canEnter) {
                optionList.removeClass(HIGHLIGHT_CLASS);
            }
            optionList.each(function (index, item) {
                const $item = $(item);
                const currentText = $item.text().toUpperCase();
                if (currentText.indexOf(query) === -1) {
                    $item.hide();
                }
                else {
                    if (isFirstItem) {
                        $item.addClass(HIGHLIGHT_CLASS);
                        isFirstItem = false;
                        $selectList.scrollTop(0);
                    }
                    $item.show();
                }
            });
        }, 100);
    }

    function enter($element) {
        const $highlightItem = $element.closest('.select-list-dropdown').find('.option-item.' + HIGHLIGHT_CLASS + ':visible');
        $highlightItem.click();
    }

    function getDropdownList(resultContainerId, canSearch, canEnter) {
        /*jshint validthis: true*/
        const $originalSelectElement = $(this);
        const dataList = $originalSelectElement.find('option').map(function (index, item) {
            const key = item.value;
            const text = item.text;

            return { key: key, value: text, className: item.className };
        });
        const selectedValue = $originalSelectElement.val();

        const dropdownListHtml = '<div id="' + resultContainerId + '" class="select-list-result-container"><div class="select-list-dropdown">' +
                                getSearchBoxHtml(canSearch) + getOptionListHtml(dataList) +
                            '</div></div>';
        const $dropdown = $(dropdownListHtml);
        $dropdown.find('.option-item[data-key="' + selectedValue + '"]').addClass(HIGHLIGHT_CLASS);

        $dropdown.on('click', '.option-item', function () {
            $('.option-item').removeClass(HIGHLIGHT_CLASS);
            $(this).addClass(HIGHLIGHT_CLASS);
            const selectedKey = $(this).data('key');
            $originalSelectElement.val(selectedKey);

            $originalSelectElement.trigger('change');
        });

        const $searchBox = $dropdown.find('.select-list-search-results');

        $searchBox.on('click', function (e) {
            e.stopPropagation();
        });

        if (canEnter) {
            $searchBox.on('keyup', function (e) {
                const $this = $(this);
                if (e.keyCode === 40) {
                    keyDown($this);
                }
                else if (e.keyCode === 38) {
                    keyUp($this);
                }
                else if (e.keyCode === 13) {
                    enter($this);
                }
                else {
                    keySearch($this, true);
                }
            });
        }
        else {
            $searchBox.on('keyup', function () {
                const $this = $(this);
                keySearch($this, false);
            });
        }

        return $dropdown;
    }

    function generateId($element) {
        let id = '';

        if ($element.attr('id') !== null) {
            id = $element.attr('id');
        } else if ($element.attr('name') !== null) {
            id = $element.attr('name') + '-' + generateChars(2);
        } else {
            id = generateChars(4);
        }

        id = id.replace(/(:|\.|\[|\]|,)/g, '');
        id = 'select-list-' + id;

        return id;
    }

    function generateChars(length) {
        let chars = '';

        for (let i = 0; i < length; i++) {
            const randomChar = Math.floor(Math.random() * 36);
            chars += randomChar.toString(36);
        }

        return chars;
    }

    function closeAll() {
        $('.select-list-result-container').remove();
    }

    const defaultOption = {
        canSearch: false,
        canEnter: false,
        isDisabled: false,
        beforeOpen: function () { },
        afterOpen: function () { }
    };

    const _methods = {
        init: function (options) {
            const selectListOption = $.extend({}, defaultOption, options || {});

            return this.each(function () {
                const self = this;
                const $element = $(self);
                if (!selectListOption.canSearch) {
                    selectListOption.canSearch = $element.attr('data-can-search') === '1';
                }
                if (!selectListOption.canEnter) {
                    selectListOption.canEnter = $element.attr('data-can-enter') === '1';
                }

                _methods.buildLabel.call(self);

                const $container = $element.parent();
                $container.off('click', '.selection');
                $container.on('click', '.selection', function () {
                    const $selection = $(this);
                    if (!$selection.hasClass(DISABLED_CLASS)) {
                        _methods.toggle.call(self, selectListOption);
                    }
                });

                $element.on('change', function () {
                    _methods.update.call(self);
                });

                $('html').on('click', function (e) {
                    const $target = $(e.target);

                    if (!$target.closest(CONTAINER_SELECTOR).length) {
                        closeAll();
                    }
                });
            });
        },

        buildLabel: function (option) {
            const newOption = option || {};
            const $self = $(this);
            $self.siblings(CONTAINER_SELECTOR).remove();
            const optionSelected = $self.find('option:selected');
            const labelText = optionSelected.length ? optionSelected.text() : '';
            const selectionId = generateId($self);

            newOption.labelText = labelText;
            newOption.selectionId = selectionId;
            const $selectionLabel = $(getLabelHtml(newOption));

            $self.hide().parent().append($selectionLabel);
        },

        open: function (resultContainerId, canSearch, canEnter) {
            closeAll();

            const $self = $(this);
            const $dropdown = getDropdownList.call(this, resultContainerId, canSearch, canEnter);

            const $selection = $self.siblings(CONTAINER_SELECTOR).find('.selection');
            const offset = $selection.offset();
            $dropdown.css({ top: (offset.top + 26) + 'px', left: offset.left + 'px', width: $selection.width() + 'px' });

            $('body').append($dropdown);
            $dropdown.find('.select-list-search-results').focus();
        },

        close: function () {
            const $this = $(this);
            const $selection = $this.siblings(CONTAINER_SELECTOR).find('.selection');
            const resultContainerId = $selection.attr('aria-targetDropdown');
            $('#' + resultContainerId).remove();
        },

        toggle: function (option) {
            const $this = $(this);
            const $selection = $this.siblings(CONTAINER_SELECTOR).find('.selection');
            const resultContainerId = $selection.attr('aria-targetDropdown');

            if (resultContainerId && resultContainerId.length > 0) {
                const $currentDropdown = $('#' + resultContainerId);
                const isShowing = $currentDropdown.length > 0;

                if (isShowing) {
                    $currentDropdown.remove();
                }
                else {
                    if (typeof option.beforeOpen === 'function') {
                        option.beforeOpen.call(this);
                    }

                    _methods.open.call(this, resultContainerId, option.canSearch, option.canEnter);

                    if (typeof option.afterOpen === 'function') {
                        option.afterOpen.call(this);
                    }
                }
            }
        },

        update: function (option) {
            _methods.buildLabel.call(this, option);
        },

        disabled: function (isDisabled) {
            const $selection = $(this).siblings(CONTAINER_SELECTOR).find('.selection');
            $selection.toggleClass(DISABLED_CLASS, !!isDisabled);
        }
    };

    $.fn.selectList = function (methodOrOptions) {
        if (_methods[methodOrOptions]) {
            return _methods[methodOrOptions].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof methodOrOptions === 'object' || !methodOrOptions) {
            return _methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + methodOrOptions + ' does not exist on selectList');
        }
    };
}).apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ }),

/***/ "./www/js/source/extensions/jquery.extension.js":
/*!******************************************************!*\
  !*** ./www/js/source/extensions/jquery.extension.js ***!
  \******************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(jQuery) {﻿///

(function ($) {
    $.fn.checkboxgroup = function () {
        var $parent = $(this).parent();
        var allChk = this.first();
        var elseChk = this.slice(1);

        allChk.prop('checked', elseChk.filter(":checked").length === elseChk.length);

        allChk.change(function () {
            var thisIsChecked = $(this).is(':checked');
            elseChk.prop('checked', thisIsChecked);
            $parent.toggleClass("invalid", !thisIsChecked);
        });

        elseChk.change(function () {
            if ($(this).is(':checked')) {
                allChk.prop('checked', elseChk.filter(":checked").length === elseChk.length);
            } else {
                allChk.prop('checked', false);
            }

            $parent.toggleClass("invalid", elseChk.filter(":checked").length === 0);
        });

        return this;
    };

    $.fn.showHide = function (isShow) {
        if (isShow) {
            this.show();
        } else {
            this.hide();
        }

        return this;
    };

    jQuery.fn.serializeObject = function () {
        var o = {};
        var a = this.serializeArray();
        $.each(a, function () {
            if (o[this.name] !== undefined) {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                }
                o[this.name].push(this.value || '');
            } else {
                o[this.name] = this.value || '';
            }
        });
        return o;
    };

    jQuery.fn.tableRowSpan = function (colIndexs, effectToIndex) {
        /// <summary>
        /// Created by Alex@20130228
        /// set table rowspan
        /// </summary>
        /// <param name="colIndexs">column indexs</param>
        /// <param name="effectToIndex">set rowspan for this columns base on lasted column in [colIndexs]</param>
        if (effectToIndex !== undefined) {
            if ($.inArray(effectToIndex, colIndexs) > -1) {
                return;
            }
        }

        function process(table, colIdx) {
            var that;
            var ethat;
            var ethis;
            var rowspan = 1;
            var len = table.rows.length - 1;
            for(var index = 0; index <= len; index++)
            {
                var currentRow = table.rows[index];                
                var td = currentRow.cells[colIdx];
                if(td) {
                    ethis = currentRow.cells[effectToIndex];
                    var found = that != null && $(td).html() === $(that).html();
                    if (found && index < len) {
                        rowspan++;
                        $(td).hide();
                        if (effectToIndex !== undefined) {
                            $(ethis).hide();
                        }
                        //.remove// do your action for the old cell here
                    }
                    else if (found && index === len) {
                        //do your action for lasted row
                        $(td).hide();
                        rowspan++;
                        $(that).attr("rowSpan", rowspan);
                        that = td;
                        if (effectToIndex !== undefined) {
                            $(ethis).hide();
                            $(ethat).attr("rowSpan", rowspan);
                            ethat = ethis;
                        }
                        rowspan = 1;
                        // do your action for the colSpan cell here
                    }
                    else {
                        if (effectToIndex !== undefined) {
                            $(ethat).attr("rowSpan", rowspan);
                            ethat = ethis;
                        }
                        $(that).attr("rowSpan", rowspan);
                        // do your action for the colSpan cell here
                        that = td;
                        rowspan = 1;
                    }
                    that = (that == null) ? td : that;
                    if (effectToIndex !== undefined) {
                        ethat = (ethat == null) ? ethis : ethat;
                    }
                    // set the that if not already set
                }
            }
        }

        return this.each(function () {
            for (var i = 0; i < colIndexs.length; i++) {
                process(this, colIndexs[i]);
            }
        });
    };

    jQuery.fn.numbericTextBox = function (min, max) {
        var thousandFormater = function (x) {
            var value = x.toString();
            var pattern = /(-?\d+)(\d{3})/;
            while (pattern.test(value)) {
                value = value.replace(pattern, "$1,$2");
            }
            return value;
        };

        var maxLength = max.toString().length;

        this.each(function (i, e) {
            $(e).keydown(function (event) {
                // Allow: backspace, delete, tab, escape, and enter
                if (event.keyCode === 46 || event.keyCode === 8 || event.keyCode === 9 || event.keyCode === 27 || event.keyCode === 13 ||
                // Allow: Ctrl+A
                    (event.keyCode === 65 && event.ctrlKey === true) ||
                // Allow: home, end, left, right
                    (event.keyCode >= 35 && event.keyCode <= 39)) {
                    // let it happen, don't do anything
                    return;
                }
                else {
                    // Ensure that it is a number and stop the keypress
                    if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                        event.preventDefault();
                    }
                }
            }).keyup(function (event) {
                var textBox = event.target;
                var value = textBox.value.replace(/,/g, '');

                if (value.length > maxLength) {
                    value = value.slice(0, maxLength);
                }

                if (value < min) {
                    value = min;
                }
                else if (value > max) {
                    value = max;
                }

                textBox.value = thousandFormater(value);
            });
        });

        return this;
    };

    $.fn.hasVerticalScrollBar = function () {
        return this.length > 0 ? this.get(0).scrollHeight > this.height() : false;
    };
    $.fn.hasHorizontalScrollBar = function () {
        return this.length > 0 ? this.get(0).scrollWidth > this.width() : false;
    };
})(jQuery);

jQuery.extend({
    format: function () {
        /// <summary>
        /// simply format method
        /// </summary>
        /// <returns type="string">formated string</returns>
        var formatted = arguments[0];
        var passValue = jQuery.grep(arguments, function (n, i) { return i > 0; });
        for (var arg in passValue) {
            if (passValue.hasOwnProperty(arg)) {
                formatted = formatted.replace("{" + arg + "}", passValue[arg]);
            }
        }
        return formatted;
    }
});
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(/*! jquery */ "./node_modules/jquery/dist/jquery.js-exposed")))

/***/ }),

/***/ "./www/js/source/extensions/prototype.js":
/*!***********************************************!*\
  !*** ./www/js/source/extensions/prototype.js ***!
  \***********************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
﻿///
///
/*
* @Alex-1/11/2013
* The below function will work adequatly for the six simple types I mentioned,
* as long as data in the objects and arrays form a tree structure.That is, there isn't
* more than one reference to the same data in the object.
*
*/

//#region String Prototype

/* jshint ignore:start */
String.prototype.format = function () {
    /// <summary>
    /// simply format method
    /// </summary>
    /// <returns type="string">formated string</returns>
    var formatted = this;
    for (var i = 0; i < arguments.length; i++) {
        formatted = formatted.replace("{" + i + "}", arguments[i]);
    }
    return formatted;
};
String.prototype.trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, '');
};

String.prototype.ltrim = function () {
    return this.replace(/^\s*/g, '');
};

String.prototype.rtrim = function () {
    return this.replace(/\s*$/g, '');
};
String.prototype.replaceAll = function (token, newToken, ignoreCase) {
    var _token;
    var str = this + "";
    var i = -1;

    if (typeof token === "string") {
        if (ignoreCase) {
            _token = token.toLowerCase();

            while ((
                i = str.toLowerCase().indexOf(
                    token, i >= 0 ? i + newToken.length : 0
                )) !== -1
            ) {
                str = str.substring(0, i) +
                    newToken +
                    str.substring(i + token.length);
            }
        } else {
            return this.split(token).join(newToken);
        }
    }
    return str;
};

//#endregion

//#region Array prototype

Array.prototype.remove = function (value, fieldName) {
    /// <summary>
    ///
    /// Remove specific element in array
    /// </summary>
    /// <param name="value"> element or value of field in object</param>
    /// <param name="fieldName">Name of a field in object</param>
    for (var i = 0; i < this.length; ++i) {
        if (typeof this[i] == 'object' && fieldName !== undefined) {
            if (this[i][fieldName] !== undefined && this[i][fieldName] == value) {
                this.splice(i, 1);
                return;
            }
        } else {
            if (this[i] == value) {
                this.splice(i, 1);
                return;
            }
        }
    }
};
Array.prototype.insertAfter = function (value, after) {
    /// <summary>
    ///
    /// </summary>
    /// <param name="value">element</param>
    /// <param name="after">position</param>
    this.splice(after + 1, 0, value);
};
Array.prototype.insertBefore = function (value, before) {
    /// <summary>
    ///
    /// </summary>
    /// <param name="value">element</param>
    /// <param name="before">position</param>
    this.splice(before, 0, value);
};

//#endregion

/* jshint ignore:end */

/***/ }),

/***/ "./www/js/source/pages/report/result/main-result.js":
/*!**********************************************************!*\
  !*** ./www/js/source/pages/report/result/main-result.js ***!
  \**********************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function($) {var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;﻿
!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(/*! ./result */ "./www/js/source/pages/report/result/result.js")], __WEBPACK_AMD_DEFINE_RESULT__ = (function (Result) {
    $(Result.initialize);
}).apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(/*! jquery */ "./node_modules/jquery/dist/jquery.js-exposed")))

/***/ }),

/***/ "./www/js/source/pages/report/result/result.js":
/*!*****************************************************!*\
  !*** ./www/js/source/pages/report/result/result.js ***!
  \*****************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;﻿
!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(/*! jquery */ "./node_modules/jquery/dist/jquery.js-exposed"), __webpack_require__(/*! common/site */ "./www/js/source/common/site.js"), __webpack_require__(/*! components/pluginhelper/daterangepicker */ "./www/js/source/components/pluginhelper/daterangepicker.js"), __webpack_require__(/*! common/popup */ "./www/js/source/common/popup.js"), __webpack_require__(/*! components/scrollablegrid */ "./www/js/source/components/scrollablegrid.js"), __webpack_require__(/*! extensions/jquery.extension */ "./www/js/source/extensions/jquery.extension.js")], __WEBPACK_AMD_DEFINE_RESULT__ = (function ($, Site, DatePicker, Popup, GridHelper) {
        const DATA_API = 'ResultReport/Index';
        const EXCEL_API = 'ResultReport/ResultExcel?';
        const SPORT_API = 'ResultReport/GetSports';
        const KENO_RESULT_API = 'ResultKenoReport/Index';
        const KENO_RESULT_SESSION_API = 'ResultKenoReport/KeepSession';
        let ajaxRequest = null;
        let ajaxFilterRequest = null;
        let kenoAjaxRequest = null;
        let kenoIntervalId = 0;
        let lotteryAjaxRequest = null;
        let lotteryIntervalId = 0;
        const param = {
            FromDate: '',
            ToDate: '',
            IsOutRight: false,
            IsExportExcel: false,
            SportId: 0,
            LeagueId: 0
        };

        const ACTIVE_CLASS = 'active';

        const _me = {
            initialize: function () {
                Site.initialize();
                DatePicker.initSingleDatePicker('Date');
                DatePicker.initDateRangePicker();
                _me.initEvent();
            },

            initEvent: function () {
                const $submitOutright = $('#submitOutright');

                $('#resultContent').on('click', '.detail-link', function () {
                    const url = Site.resolveClientUrl('ResultReport/ResultDetail')
                     + '?matchid=' + $(this).attr('data-match-id') + '&SportType=' + param.SportId;

                    const popupOption = { id: 'result-popup', width: 775, height: 520, title: Site.getSingleResource('Result') };
                    Popup.openPopupWithIframe(url, popupOption);
                });

                $submitOutright.click(function () {
                    if (_me.canSubmit(this)) {
                        _me.showOutRight(true);
                        const startDate = $('#Date').val();
                        DatePicker.setValueForDateRange(startDate);
                        _me.submit();
                    }
                });

                $('#submitNormal').click(function () {
                    if (_me.canSubmit(this)) {
                        _me.showOutRight(false);
                        DatePicker.setValueForSingleDate($('#Date'), $('#FromDate').val());
                        _me.submit();
                    }
                });

                $('#SubmitButton').click(function () {
                    _me.submit();
                });

                $('#submitToday').click(function () {
                    _me.clearExcelExport();
                    _me.clearResult();
                    const date = $(this).attr('data-date');
                    const isShowOutright = $submitOutright.attr('data-isoutright') === '1';
                    param.FromDate = date;
                    param.ToDate = date;
                    param.IsExportExcel = false;
                    param.IsOutRight = isShowOutright;
                    param.SportId = 0;
                    param.LeagueId = 0;

                    _me.updateDateInput(isShowOutright);
                    _me.getData();
                });

                $('#submitYesterday').click(function () {
                    _me.clearExcelExport();
                    _me.clearResult();
                    const date = $(this).attr('data-date');
                    const isShowOutright = $submitOutright.attr('data-isoutright') === '1';
                    param.FromDate = date;
                    param.ToDate = date;
                    param.IsExportExcel = false;
                    param.IsOutRight = isShowOutright;
                    param.SportId = 0;
                    param.LeagueId = 0;

                    _me.updateDateInput(isShowOutright);
                    _me.getData();
                });

                $('#ExportToExcel').click(function (e) {
                    e.preventDefault();
                    const isShowOutright = $submitOutright.attr('data-isoutright') === '1';
                    _me.setDateParam(isShowOutright);

                    param.IsExportExcel = true;
                    param.IsOutRight = isShowOutright;
                    param.SportId = $('#ddlSport option').length > 0 ? $('#ddlSport').val() : 0;
                    param.LeagueId = $('#ddlLeague').val();
                    _me.getData();
                });

                $('#ddlSport').change(function () {
                    _me.clearExcelExport();
                    _me.clearLeagueFilter();

                    param.SportId = $(this).val();
                    param.LeagueId = 0;
                    _me.getData();
                });

                $('#ddlLeague').change(function () {
                    _me.clearExcelExport();
                    param.LeagueId = $(this).val();
                    _me.getReportData();
                });

                $(window).on('resize', function () {
                    GridHelper.resizeGrid($('.fn-rs-content-scroll'));
                });

                $(window).on('unload', function () {
                    $(window).off('resize');
                });

                if ($('#submitKeno').length) {
                    $('#submitKeno').click(function () {
                        _me.showKeno($(this).data('vendorproductid'));
                    });
                }

                if ($('#submitLottery').length) {
                    $('#submitLottery').click(function () {
                        _me.showLottery($(this).data('vendorproductid'));
                    });
                }
            },

            submit: function () {
                _me.clearExcelExport();
                _me.clearResult();
                const isShowOutright = $('#submitOutright').attr('data-isoutright') === '1';
                _me.setDateParam(isShowOutright);

                param.IsExportExcel = false;
                param.IsOutRight = isShowOutright;
                param.SportId = 0;
                param.LeagueId = 0;
                _me.getData();
            },

            showKeno: function (vendorProductId) {
                const option = {
                    type: 'GET',
                    cache: false,
                    data: { vendorProductId: vendorProductId },
                    url: Site.resolveClientUrl(KENO_RESULT_API),
                    success: function (response) {
                        window.open(response.Url, '_blank', 'left=20,top=20,width=950,height=650,toolbar=1,resizable=1,scrollbars,status=1');
                        clearInterval(kenoIntervalId);
                        _me.extendKenoSession(vendorProductId, response.UserId);
                    },
                    beforeSend: function () {
                        Site.blockUI();
                    },
                    complete: function () {
                        Site.unBlockUI();
                    }
                };
                _me.stopAjaxRequest(kenoAjaxRequest);
                kenoAjaxRequest = $.ajax(option);
            },

            showLottery: function (vendorProductId) {
                const option = {
                    type: 'GET',
                    cache: false,
                    data: { vendorProductId: vendorProductId },
                    url: Site.resolveClientUrl(KENO_RESULT_API),
                    success: function (response) {
                        window.open(response.Url, '_blank', 'left=20,top=20,width=950,height=650,toolbar=1,resizable=1,scrollbars,status=1');
                        clearInterval(lotteryIntervalId);
                        _me.extendLotterySession(vendorProductId, response.UserId);
                    },
                    beforeSend: function () {
                        Site.blockUI();
                    },
                    complete: function () {
                        Site.unBlockUI();
                    }
                };
                _me.stopAjaxRequest(lotteryAjaxRequest);
                lotteryAjaxRequest = $.ajax(option);
            },

            extendKenoSession: function (vendorProductId, userId) {
                if (!userId)
                {
                    return;
                }

                kenoIntervalId = setInterval(function () {
                    const option = {
                        type: 'POST',
                        cache: false,
                        data: {
                            userId: userId,
                            vendorProductId: vendorProductId
                        },
                        url: Site.resolveClientUrl(KENO_RESULT_SESSION_API)
                    };
                    _me.stopAjaxRequest(kenoAjaxRequest);
                    kenoAjaxRequest = $.ajax(option);
                }, 6000);
            },

            extendLotterySession: function (vendorProductId, userId) {
                if (!userId) {
                    return;
                }

                lotteryIntervalId = setInterval(function () {
                    const option = {
                        type: 'POST',
                        cache: false,
                        data: {
                            userId: userId,
                            vendorProductId: vendorProductId
                        },
                        url: Site.resolveClientUrl(KENO_RESULT_SESSION_API)
                    };
                    _me.stopAjaxRequest(lotteryAjaxRequest);
                    lotteryAjaxRequest = $.ajax(option);
                }, 6000);
            },

            getData: function () {
                if (param.IsExportExcel) {
                    _me.exportExcel();
                } else {
                    _me.getFilter();
                }
            },

            exportExcel: function () {
                let url = Site.resolveClientUrl(EXCEL_API);
                url += 'FromDate=' + param.FromDate;
                url += '&ToDate=' + param.ToDate;
                url += '&IsOutRight=' + param.IsOutRight;
                url += '&SportId=' + param.SportId;
                url += '&LeagueId=' + param.LeagueId;
                url += '&IsExportExcel=' + param.IsExportExcel;

                $('#iframeForExportExcel').attr('src', url);
            },

            getFilter: function () {
                const url = Site.resolveClientUrl(SPORT_API);
                const option = {
                    type: 'POST',
                    dataType: 'json',
                    data: param,
                    cache: false,
                    url: url,
                    success: function (response) {
                        _me.renderFilter(response);
                        if (response !== null && response.toString().length > 1) {
                            _me.getReportData();
                        } else {
                            $('#noInfoRow').show();
                            Site.unBlockUI();
                        }
                    },
                    beforeSend: function () {
                        Site.blockUI();
                    }
                };
                _me.stopAjaxRequest(ajaxFilterRequest);
                _me.stopAjaxRequest(ajaxRequest);
                ajaxFilterRequest = $.ajax(option);
            },

            getReportData: function () {
                const url = Site.resolveClientUrl(DATA_API);
                const option = {
                    type: 'POST',
                    cache: false,
                    data: param,
                    url: url,
                    success: function (response) {
                        if (response !== null && response.length > 0) {
                            $('#noInfoRow').hide();
                            $('#resultContent').html(response);

                            if ($('.fn-rs-content-scroll').length > 0) {
                                GridHelper.resizeGrid($('.fn-rs-content-scroll'));
                            }
                        } else {
                            $('#noInfoRow').show();
                        }
                    },
                    beforeSend: function () {
                        Site.blockUI();
                    },
                    complete: function () {
                        Site.unBlockUI();
                    }
                };
                _me.stopAjaxRequest(ajaxRequest);
                ajaxRequest = $.ajax(option);
            },

            renderFilter: function (data) {
                if (data) {
                    _me.renderSportFilter(data.Sports);
                    _me.renderLeagueFilter(data.Leagues);
                    Site.initSelectElements();

                    $('#sportAndLeagueFilter').css('visibility', 'visible');
                    $('#noInfoRow').hide();
                }
                else {
                    $('#sportAndLeagueFilter').css('visibility', 'hidden');
                }
            },

            renderSportFilter: function (sportList) {
                if (param.SportId === 0 && sportList.length > 0) {
                    const firstSport = sportList[0].SportId;
                    param.SportId = firstSport;

                    const $sport = $('#ddlSport');
                    $.each(sportList, function (index, value) {
                        $sport.append('<option value="' + value.SportId + '">' + value.SportName + '</option>');
                    });
                }
            },

            renderLeagueFilter: function (leagueList) {
                if (leagueList !== null && leagueList.length > 0) {
                    const $league = $('#ddlLeague');
                    $.each(leagueList, function (index, value) {
                        $league.append('<option value="' + value.LeagueId + '">' + value.LeagueName + '</option>');
                    });
                    $league.val(param.LeagueId);
                }
            },

            clearResult: function () {
                _me.clearLeagueFilter();
                $('#ddlSport option').remove();
                $('#resultContent').html('');
                $('#sportAndLeagueFilter').css('visibility', 'hidden');
            },

            clearLeagueFilter: function () {
                const defaultLeague = $('#ddlLeague option[value="0"]').clone();
                $('#ddlLeague option').remove();
                $('#ddlLeague').append(defaultLeague);
            },

            clearExcelExport: function () {
                param.IsExportExcel = false;
                $('#iframeForExportExcel').attr('src', '');
            },

            showOutRight: function (isShow) {
                if (isShow) {
                    $('#singleDate').hide();
                    $('#dateRange').show();
                    $('#submitOutright').addClass(ACTIVE_CLASS);
                    $('#submitNormal').removeClass(ACTIVE_CLASS);
                } else {
                    $('#dateRange').hide();
                    $('#singleDate').show();
                    $('#submitOutright').removeClass(ACTIVE_CLASS);
                    $('#submitNormal').addClass(ACTIVE_CLASS);
                }
                $('#submitOutright').attr('data-isOutright', isShow ? '1' : '0');
            },

            setDateParam: function (isShowOutright) {
                if (isShowOutright) {
                    param.FromDate = $('#FromDate').val();
                    param.ToDate = $('#ToDate').val();
                }
                else {
                    param.FromDate = $('#Date').val();
                    param.ToDate = $('#Date').val();
                }
            },

            updateDateInput: function (isShowOutright) {
                if (isShowOutright) {
                    DatePicker.setValueForDateRange(param.FromDate, param.ToDate);
                }
                else {
                    DatePicker.setValueForSingleDate($('#Date'), param.FromDate);
                }
            },

            canSubmit: function (element) {
                return !$(element).hasClass(ACTIVE_CLASS);
            },

            stopAjaxRequest: function (request) {
                if (request !== null) {
                    request.abort();
                }
            }
        };

        return _me;
    }).apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ }),

/***/ 4:
/*!****************************************************************!*\
  !*** multi ./www/js/source/pages/report/result/main-result.js ***!
  \****************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

module.exports = __webpack_require__(/*! ./www/js/source/pages/report/result/main-result.js */"./www/js/source/pages/report/result/main-result.js");


/***/ })

/******/ });
//# sourceMappingURL=result.chunk.js.map