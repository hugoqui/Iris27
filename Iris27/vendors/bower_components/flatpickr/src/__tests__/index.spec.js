"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var index_1 = require("index");
var ru_1 = require("l10n/ru");
index_1.default.defaultConfig.animate = false;
index_1.default.defaultConfig.closeOnSelect = true;
jest.useFakeTimers();
var elem, fp;
var UA = navigator.userAgent;
var mockAgent;
navigator.__defineGetter__("userAgent", function () {
    return mockAgent || UA;
});
function createInstance(config, el) {
    fp = index_1.default(el || elem || document.createElement("input"), config || {});
    return fp;
}
function beforeEachTest() {
    mockAgent = undefined;
    jest.runAllTimers();
    document.activeElement.blur();
    fp && fp.destroy && fp.destroy();
    if (elem === undefined) {
        elem = document.createElement("input");
        document.body.appendChild(elem);
    }
}
function incrementTime(type, by) {
    var e = fp[type];
    var times = Math.abs(by);
    var childNodeNum = by >= 0 ? 1 : 2;
    if (e !== undefined && e.parentNode)
        for (var i = times; i--;)
            simulate("mousedown", e.parentNode.childNodes[childNodeNum], { which: 1 }, MouseEvent);
}
function simulate(eventType, onElement, options, type) {
    var eventOptions = Object.assign(options || {}, { bubbles: true });
    var evt = new (type || CustomEvent)(eventType, eventOptions);
    try {
        Object.assign(evt, eventOptions);
    }
    catch (e) { }
    onElement.dispatchEvent(evt);
}
describe("flatpickr", function () {
    beforeEach(beforeEachTest);
    describe("init", function () {
        it("should parse defaultDate", function () {
            createInstance({
                defaultDate: "2016-12-27T16:16:22.585Z",
                enableTime: true,
            });
            var date = new Date("2016-12-27T16:16:22.585Z");
            expect(fp.currentYear).toEqual(date.getFullYear());
            expect(fp.currentMonth).toEqual(date.getMonth());
            var selected = fp.days.querySelector(".selected");
            expect(selected).toBeTruthy();
            if (selected)
                expect(selected.textContent).toEqual(date.getDate() + "");
        });
        it("shouldn't parse out-of-bounds defaultDate", function () {
            createInstance({
                minDate: "2016-12-28T16:16:22.585Z",
                defaultDate: "2016-12-27T16:16:22.585Z",
            });
            expect(fp.days.querySelector(".selected")).toEqual(null);
            createInstance({
                defaultDate: "2016-12-27T16:16:22.585Z",
                enableTime: true,
            });
            fp.set("maxDate", "2016-12-25");
            fp.set("minDate", "2016-12-24");
            expect(fp.currentMonth).toEqual(11);
            expect(fp.days.querySelector(".selected")).toEqual(null);
            var enabledDays = fp.days.querySelectorAll(".flatpickr-day:not(.disabled)");
            expect(enabledDays.length).toEqual(2);
            expect(enabledDays[0].textContent).toEqual("24");
            expect(enabledDays[1].textContent).toEqual("25");
            createInstance({
                defaultDate: "2016-12-27T16:16:22.585Z",
                minDate: "2016-12-27T16:26:22.585Z",
                enableTime: true,
            });
            expect(fp.selectedDates.length).toBe(0);
            expect(fp.days.querySelector(".selected")).toEqual(null);
        });
        it("doesn't throw with undefined properties", function () {
            createInstance({
                onChange: undefined,
            });
            fp.set("minDate", "2016-10-20");
            expect(fp.config.minDate).toBeDefined();
        });
    });
    describe("datetimestring parser", function () {
        describe("date string parser", function () {
            it("should parse timestamp", function () {
                createInstance({
                    defaultDate: 1477111633771,
                });
                expect(fp.selectedDates[0]).toBeDefined();
                expect(fp.selectedDates[0].getFullYear()).toEqual(2016);
                expect(fp.selectedDates[0].getMonth()).toEqual(9);
                expect(fp.selectedDates[0].getDate()).toEqual(22);
            });
            it("should parse unix time", function () {
                createInstance({
                    defaultDate: "1477111633.771",
                    dateFormat: "U",
                });
                var parsedDate = fp.selectedDates[0];
                expect(parsedDate).toBeDefined();
                expect(parsedDate.getFullYear()).toEqual(2016);
                expect(parsedDate.getMonth()).toEqual(9);
                expect(parsedDate.getDate()).toEqual(22);
            });
            it('should parse "2016-10"', function () {
                createInstance({
                    defaultDate: "2016-10",
                });
                expect(fp.selectedDates[0]).toBeDefined();
                expect(fp.selectedDates[0].getFullYear()).toEqual(2016);
                expect(fp.selectedDates[0].getMonth()).toEqual(9);
            });
            it('should parse "2016-10-20 3:30"', function () {
                createInstance({
                    defaultDate: "2016-10-20 3:30",
                    enableTime: true,
                });
                expect(fp.selectedDates[0]).toBeDefined();
                expect(fp.selectedDates[0].getFullYear()).toEqual(2016);
                expect(fp.selectedDates[0].getMonth()).toEqual(9);
                expect(fp.selectedDates[0].getDate()).toEqual(20);
                expect(fp.selectedDates[0].getHours()).toEqual(3);
                expect(fp.selectedDates[0].getMinutes()).toEqual(30);
            });
            it("should parse ISO8601", function () {
                createInstance({
                    defaultDate: "2007-03-04T21:08:12",
                    enableTime: true,
                    enableSeconds: true,
                });
                expect(fp.selectedDates[0]).toBeDefined();
                expect(fp.selectedDates[0].getFullYear()).toEqual(2007);
                expect(fp.selectedDates[0].getMonth()).toEqual(2);
                expect(fp.selectedDates[0].getDate()).toEqual(4);
                expect(fp.selectedDates[0].getHours()).toEqual(21);
                expect(fp.selectedDates[0].getMinutes()).toEqual(8);
                expect(fp.selectedDates[0].getSeconds()).toEqual(12);
            });
            it('should parse "today"', function () {
                createInstance({});
                var today = fp.parseDate("today", undefined, true);
                expect(today).toBeDefined();
                today && expect(today.getHours()).toBe(0);
            });
            it("should parse AM/PM", function () {
                createInstance({
                    dateFormat: "m/d/Y h:i K",
                    enableTime: true,
                    defaultDate: "8/3/2017 12:00 AM",
                });
                expect(fp.selectedDates[0]).toBeDefined();
                expect(fp.selectedDates[0].getFullYear()).toEqual(2017);
                expect(fp.selectedDates[0].getMonth()).toEqual(7);
                expect(fp.selectedDates[0].getDate()).toEqual(3);
                expect(fp.selectedDates[0].getHours()).toEqual(0);
                expect(fp.selectedDates[0].getMinutes()).toEqual(0);
            });
            it("should parse JSON datestrings", function () {
                createInstance({});
                var date = fp.parseDate("2016-12-27T16:16:22.585Z", undefined);
                expect(date).toBeDefined();
                if (!date)
                    return;
                expect(date.getTime()).toBeDefined();
                expect(date.getTime()).toEqual(Date.parse("2016-12-27T16:16:22.585Z"));
            });
        });
        describe("time string parser", function () {
            it('should parse "21:11"', function () {
                createInstance({
                    defaultDate: "21:11",
                    allowInput: true,
                    enableTime: true,
                    noCalendar: true,
                });
                expect(fp.selectedDates[0]).toBeDefined();
                expect(fp.selectedDates[0].getHours()).toEqual(21);
                expect(fp.selectedDates[0].getMinutes()).toEqual(11);
            });
            it('should parse "21:11:12"', function () {
                createInstance({
                    allowInput: true,
                    enableTime: true,
                    enableSeconds: true,
                    noCalendar: true,
                    defaultDate: "21:11:12",
                });
                expect(fp.selectedDates[0]).toBeDefined();
                expect(fp.selectedDates[0].getHours()).toEqual(21);
                expect(fp.selectedDates[0].getMinutes()).toEqual(11);
                expect(fp.selectedDates[0].getSeconds()).toEqual(12);
            });
            it('should parse "11:59 PM"', function () {
                createInstance({
                    allowInput: true,
                    enableTime: true,
                    noCalendar: true,
                    dateFormat: "h:i K",
                    defaultDate: "11:59 PM",
                });
                expect(fp.selectedDates[0]).toBeDefined();
                expect(fp.selectedDates[0].getHours()).toBe(23);
                expect(fp.selectedDates[0].getMinutes()).toBe(59);
                expect(fp.selectedDates[0].getSeconds()).toBe(0);
                expect(fp.amPM).toBeDefined();
                fp.amPM && expect(fp.amPM.innerHTML).toBe("PM");
            });
            it('should parse "3:05:03 PM"', function () {
                createInstance({
                    allowInput: true,
                    enableTime: true,
                    enableSeconds: true,
                    noCalendar: true,
                    dateFormat: "h:i:S K",
                    defaultDate: "3:05:03 PM",
                });
                expect(fp.selectedDates[0]).toBeDefined();
                expect(fp.selectedDates[0].getHours()).toBe(15);
                expect(fp.selectedDates[0].getMinutes()).toBe(5);
                expect(fp.selectedDates[0].getSeconds()).toBe(3);
                expect(fp.amPM).toBeDefined();
                fp.amPM && expect(fp.amPM.innerHTML).toBe("PM");
            });
            it("should parse defaultHour", function () {
                createInstance({
                    enableTime: true,
                    noCalendar: true,
                    defaultHour: 0,
                });
                expect(fp.hourElement.value).toEqual("12");
                createInstance({
                    enableTime: true,
                    noCalendar: true,
                    defaultHour: 12,
                });
                expect(fp.hourElement.value).toEqual("12");
                createInstance({
                    enableTime: true,
                    noCalendar: true,
                    defaultHour: 23,
                    time_24hr: true,
                });
                expect(fp.hourElement.value).toEqual("23");
            });
        });
    });
    describe("date formatting", function () {
        describe("default formatter", function () {
            var DEFAULT_FORMAT_1 = "d.m.y H:i:S", DEFAULT_FORMAT_2 = "D j F, 'y";
            it("should format the date with the pattern \"" + DEFAULT_FORMAT_1 + "\"", function () {
                var RESULT = "20.10.16 09:19:59";
                createInstance({
                    dateFormat: DEFAULT_FORMAT_1,
                });
                fp.setDate("20.10.16 09:19:59");
                expect(fp.input.value).toEqual(RESULT);
                fp.setDate("2015.11.21 19:29:49");
                expect(fp.input.value).not.toEqual(RESULT);
            });
            it("should format the date with the pattern \"" + DEFAULT_FORMAT_2 + "\"", function () {
                var RESULT = "Thu 20 October, '16";
                createInstance({
                    dateFormat: DEFAULT_FORMAT_2,
                });
                fp.setDate("Thu 20 October, '16");
                expect(fp.input.value).toEqual(RESULT);
                fp.setDate("2015-11-21 19:29:49");
                expect(fp.input.value).not.toEqual(RESULT);
            });
        });
        describe("custom formatter", function () {
            it("should format the date using the custom formatter", function () {
                var RESULT = "MAAAGIC.*^*.2016.*^*.20.*^*.10";
                createInstance({
                    dateFormat: "YEAR-DAYOFMONTH-MONTH",
                    formatDate: function (date, formatStr) {
                        var segs = formatStr.split("-");
                        return ("MAAAGIC.*^*." +
                            segs
                                .map(function (seg) {
                                var mapped = null;
                                switch (seg) {
                                    case "DAYOFMONTH":
                                        mapped = date.getDate();
                                        break;
                                    case "MONTH":
                                        mapped = date.getMonth() + 1;
                                        break;
                                    case "YEAR":
                                        mapped = date.getFullYear();
                                        break;
                                }
                                return "" + mapped;
                            })
                                .join(".*^*."));
                    },
                });
                fp.setDate(new Date(2016, 9, 20));
                expect(fp.input.value).toEqual(RESULT);
                fp.setDate(new Date(2016, 10, 20));
                expect(fp.input.value).not.toEqual(RESULT);
            });
        });
    });
    describe("API", function () {
        it("changeMonth()", function () {
            createInstance({
                defaultDate: "2016-12-20",
            });
            fp.changeMonth(1);
            expect(fp.currentYear).toEqual(2017);
            fp.changeMonth(-1);
            expect(fp.currentYear).toEqual(2016);
            fp.changeMonth(2);
            expect(fp.currentMonth).toEqual(1);
            expect(fp.currentYear).toEqual(2017);
            fp.changeMonth(14);
            expect(fp.currentYear).toEqual(2018);
            expect(fp.currentMonth).toEqual(3);
        });
        it("monthScroll", function () {
            createInstance();
            fp.changeMonth(1, false);
            fp.open();
            simulate("wheel", fp.currentMonthElement, {
                wheelDelta: 1,
            });
            jest.runAllTimers();
            expect(fp.currentMonth).toEqual(2);
        });
        it("monthScroll: 0 < abs(delta) < 1", function () {
            createInstance();
            fp.changeMonth(1, false);
            fp.open();
            simulate("wheel", fp.currentMonthElement, {
                deltaY: -0.3,
            });
            jest.runAllTimers();
            expect(fp.currentMonth).toEqual(2);
        });
        it("yearScroll", function () {
            createInstance();
            var now = new Date();
            fp.setDate(now);
            fp.open();
            simulate("wheel", fp.currentYearElement, {
                wheelDelta: 1,
            }, MouseEvent);
            jest.runAllTimers();
            expect(fp.currentYear).toEqual(now.getFullYear() + 1);
        });
        it("destroy()", function () {
            var fired = false;
            var input = fp.input;
            createInstance({
                altInput: true,
                onKeyDown: [
                    function () {
                        fired = true;
                    },
                ],
            });
            expect(input.type).toEqual("hidden");
            fp.open();
            fp.altInput &&
                simulate("keydown", fp.altInput, { key: "ArrowLeft", bubbles: true });
            expect(fired).toEqual(true);
            fp.destroy();
            expect(input.type).toEqual("text");
            expect(fp.altInput).toBeUndefined();
            expect(fp.config).toBeUndefined();
            fired = false;
            simulate("keydown", input, { key: "ArrowLeft", bubbles: true });
            simulate("keydown", document.body, { key: "ArrowLeft", bubbles: true });
            expect(fired).toEqual(false);
        });
        it("set (option, value)", function () {
            createInstance();
            fp.set("minDate", "2016-10-20");
            expect(fp.currentYearElement.min).toEqual("2016");
            expect(fp.config.minDate).toBeDefined();
            fp.set("minDate", null);
            expect(fp.currentYearElement.hasAttribute("min")).toEqual(false);
            fp.set("maxDate", "2016-10-20");
            expect(fp.config.maxDate).toBeDefined();
            expect(fp.currentYearElement.max).toEqual("2016");
            fp.set("maxDate", null);
            expect(fp.currentYearElement.hasAttribute("max")).toEqual(false);
            fp.set("mode", "range");
            expect(fp.config.mode).toEqual("range");
        });
        it("setDate (date)", function () {
            createInstance({
                enableTime: true,
            });
            fp.setDate("2016-10-20 03:00");
            expect(fp.selectedDates[0]).toBeDefined();
            expect(fp.selectedDates[0].getFullYear()).toEqual(2016);
            expect(fp.selectedDates[0].getMonth()).toEqual(9);
            expect(fp.selectedDates[0].getDate()).toEqual(20);
            expect(fp.selectedDates[0].getHours()).toEqual(3);
            expect(fp.currentYear).toEqual(2016);
            expect(fp.currentMonth).toEqual(9);
            if (fp.hourElement && fp.minuteElement && fp.amPM) {
                expect(fp.hourElement.value).toEqual("03");
                expect(fp.minuteElement.value).toEqual("00");
                expect(fp.amPM.textContent).toEqual("AM");
            }
            fp.setDate(0);
            expect(fp.selectedDates[0]).toBeDefined();
            expect(fp.selectedDates[0].getFullYear()).toBeLessThan(1971);
            fp.setDate("");
            expect(fp.selectedDates[0]).not.toBeDefined();
        });
        it("has valid latestSelectedDateObj", function () {
            createInstance({
                defaultDate: "2016-10-01 3:30",
                enableTime: true,
            });
            expect(fp.latestSelectedDateObj).toBeDefined();
            if (fp.latestSelectedDateObj) {
                expect(fp.latestSelectedDateObj.getFullYear()).toEqual(2016);
                expect(fp.latestSelectedDateObj.getMonth()).toEqual(9);
                expect(fp.latestSelectedDateObj.getDate()).toEqual(1);
            }
            if (fp.hourElement && fp.minuteElement && fp.amPM) {
                expect(fp.hourElement.value).toEqual("03");
                expect(fp.minuteElement.value).toEqual("30");
                expect(fp.amPM.textContent).toEqual("AM");
            }
            fp.setDate("2016-11-03 16:49");
            expect(fp.latestSelectedDateObj).toBeDefined();
            if (fp.latestSelectedDateObj) {
                expect(fp.latestSelectedDateObj.getFullYear()).toEqual(2016);
                expect(fp.latestSelectedDateObj.getMonth()).toEqual(10);
                expect(fp.latestSelectedDateObj.getDate()).toEqual(3);
            }
            if (fp.hourElement && fp.minuteElement && fp.amPM) {
                expect(fp.hourElement.value).toEqual("04");
                expect(fp.minuteElement.value).toEqual("49");
                expect(fp.amPM.textContent).toEqual("PM");
            }
            fp.setDate("");
            expect(fp.latestSelectedDateObj).toEqual(undefined);
        });
        it("parses dates in enable[] and disable[]", function () {
            createInstance({
                disable: [
                    { from: "2016-11-20", to: "2016-12-20" },
                    "2016-12-21",
                    null,
                ],
                enable: [
                    { from: "2016-11-20", to: "2016-12-20" },
                    "2016-12-21",
                    null,
                ],
            });
            expect(fp.config.disable[0].from instanceof Date).toBe(true);
            expect(fp.config.disable[0].to instanceof Date).toBe(true);
            expect(fp.config.disable[1] instanceof Date).toBe(true);
            expect(fp.config.disable.indexOf(null)).toBe(-1);
            expect(fp.config.enable[0].from instanceof Date).toBe(true);
            expect(fp.config.enable[0].to instanceof Date).toBe(true);
            expect(fp.config.enable[1] instanceof Date).toBe(true);
            expect(fp.config.enable.indexOf(null)).toBe(-1);
        });
        it("documentClick", function () {
            createInstance({
                mode: "range",
            });
            simulate("focus", fp._input, { which: 1, bubbles: true }, CustomEvent);
            fp._input.focus();
            expect(fp.isOpen).toBe(true);
            simulate("mousedown", window.document.body, { which: 1 }, CustomEvent);
            fp._input.blur();
            expect(fp.isOpen).toBe(false);
            expect(fp.calendarContainer.classList.contains("open")).toBe(false);
            expect(fp.selectedDates.length).toBe(0);
            simulate("focus", fp._input);
            simulate("mousedown", fp.days.childNodes[15], { which: 1, bubbles: true }, CustomEvent);
            expect(fp.selectedDates.length).toBe(1);
            fp.isOpen = true;
            simulate("mousedown", window.document.body, { which: 1 }, CustomEvent);
            expect(fp.isOpen).toBe(false);
            expect(fp.selectedDates.length).toBe(0);
            expect(fp._input.value).toBe("");
        });
        it("onKeyDown", function () {
            createInstance({
                enableTime: true,
                altInput: true,
            });
            fp.jumpToDate("2016-2-1");
            fp.open();
            fp.days.childNodes[15].focus();
            simulate("keydown", fp.days.childNodes[15], {
                key: "Enter",
            }, KeyboardEvent);
            expect(fp.selectedDates.length).toBe(1);
            simulate("keydown", fp.calendarContainer, {
                key: "Escape",
            }, KeyboardEvent);
            expect(fp.isOpen).toEqual(false);
        });
        it("onKeyDown: arrow nav", function () {
            jest.runAllTimers();
            createInstance({
                defaultDate: "2017-01-01",
            });
            fp.open();
            fp.input.focus();
            simulate("keydown", window.document.body, { key: "ArrowLeft", bubbles: true }, KeyboardEvent);
            var dayElem = document.activeElement;
            expect(fp.currentMonth).toBe(0);
            expect(dayElem.dateObj.getDate()).toEqual(1);
            simulate("keydown", document.activeElement, { key: "ArrowLeft" });
            expect(fp.currentMonth).toBe(11);
            expect(fp.currentYear).toBe(2016);
            dayElem = document.activeElement;
            expect(dayElem.dateObj.getDate()).toEqual(7);
            simulate("keydown", document.activeElement, { key: "ArrowRight" });
            dayElem = document.activeElement;
            expect(dayElem.dateObj.getDate()).toEqual(1);
            expect(fp.currentMonth).toBe(0);
            expect(fp.currentYear).toBe(2017);
            simulate("keydown", document.activeElement, { key: "ArrowUp" });
            simulate("keydown", document.activeElement, { key: "ArrowUp" });
            expect(fp.currentMonth).toBe(11);
            expect(fp.currentYear).toBe(2016);
            dayElem = document.activeElement;
            expect(dayElem.dateObj.getDate()).toEqual(25);
            simulate("keydown", document.activeElement, { key: "ArrowDown" });
            simulate("keydown", document.activeElement, { key: "ArrowDown" });
            expect(fp.currentMonth).toBe(0);
            expect(fp.currentYear).toBe(2017);
            dayElem = document.activeElement;
            expect(dayElem.dateObj.getDate()).toEqual(1);
            simulate("keydown", document.activeElement, {
                key: "ArrowRight",
                ctrlKey: true,
            });
            expect(fp.currentMonth).toBe(1);
            expect(fp.currentYear).toBe(2017);
            simulate("keydown", document.activeElement, {
                key: "ArrowLeft",
                ctrlKey: true,
            });
            simulate("keydown", document.activeElement, {
                key: "ArrowLeft",
                ctrlKey: true,
            });
            expect(fp.currentMonth).toBe(11);
            expect(fp.currentYear).toBe(2016);
        });
        it("enabling dates by function", function () {
            createInstance({
                enable: [function (d) { return d.getDate() === 6; }, new Date()],
                disable: [{ from: "2016-10-20", to: "2016-10-25" }],
            });
            expect(fp.isEnabled("2016-10-06")).toBe(true);
            expect(fp.isEnabled(new Date())).toBe(true);
            expect(fp.isEnabled("2016-10-20")).toBe(false);
            expect(fp.isEnabled("2016-10-22")).toBe(false);
            expect(fp.isEnabled("2016-10-25")).toBe(false);
        });
    });
    describe("UI", function () {
        it("mode: multiple", function () {
            createInstance({
                mode: "multiple",
            });
            fp.jumpToDate("2017-1-1");
            fp.open();
            simulate("keydown", fp.days.childNodes[0], { key: "Enter" });
            expect(fp.selectedDates.length).toBe(1);
            simulate("keydown", fp.days.childNodes[0], { key: "Enter" });
            expect(fp.selectedDates.length).toBe(0);
        });
        it("switch month to selectedDate", function () {
            createInstance();
            fp.jumpToDate("2017-1-1");
            expect(fp.currentMonth).toBe(0);
            simulate("mousedown", fp.days.childNodes[41], { which: 1 }, MouseEvent);
            expect(fp.selectedDates.length).toBe(1);
            expect(fp.currentMonth).toBe(1);
        });
        it("static calendar", function () {
            createInstance({
                static: true,
            });
            expect(fp.calendarContainer.classList.contains("static")).toBe(true);
            if (!fp.element.parentNode)
                return;
            expect(fp.element.parentNode.classList.contains("flatpickr-wrapper")).toBe(true);
            expect(fp.element.parentNode.childNodes[0]).toEqual(fp.element);
            expect(fp.element.parentNode.childNodes[1]).toEqual(fp.calendarContainer);
        });
        it("mobile calendar", function () {
            mockAgent = "Android";
            createInstance({
                enableTime: true,
            });
            expect(fp.isMobile).toBe(true);
            var mobileInput = fp.mobileInput;
            mobileInput.value = "2016-10-20T02:30";
            simulate("change", mobileInput);
            expect(fp.selectedDates.length).toBe(1);
            expect(fp.latestSelectedDateObj).toBeDefined();
            if (!fp.latestSelectedDateObj)
                return;
            expect(fp.latestSelectedDateObj.getFullYear()).toBe(2016);
            expect(fp.latestSelectedDateObj.getMonth()).toBe(9);
            expect(fp.latestSelectedDateObj.getDate()).toBe(20);
            expect(fp.latestSelectedDateObj.getHours()).toBe(2);
            expect(fp.latestSelectedDateObj.getMinutes()).toBe(30);
        });
        it("selectDate() + onChange() through GUI", function () {
            function verifySelected(date) {
                expect(date).toBeDefined();
                if (!date)
                    return;
                expect(date.getFullYear()).toEqual(2016);
                expect(date.getMonth()).toEqual(9);
                expect(date.getDate()).toEqual(10);
                expect(fp.hourElement.value).toEqual("03");
                expect(fp.minuteElement.value).toEqual("30");
                expect(fp.amPM.textContent).toEqual("AM");
            }
            createInstance({
                enableTime: true,
                defaultDate: "2016-10-01 3:30",
                onChange: function (dates) {
                    if (dates.length)
                        verifySelected(dates[0]);
                },
            });
            fp.open();
            simulate("mousedown", fp.days.childNodes[15], { which: 1 }, MouseEvent);
            verifySelected(fp.selectedDates[0]);
        });
        it("year input", function () {
            createInstance();
            fp.currentYearElement.value = "2000";
            simulate("keyup", fp.currentYearElement);
            expect(fp.currentYear).toEqual(2000);
            incrementTime("currentYearElement", 1);
            expect(fp.currentYear).toEqual(2001);
            expect(fp.currentYearElement.value).toEqual("2001");
            expect(fp.days.childNodes[10].dateObj.getFullYear()).toEqual(2001);
        });
        it("time input and increments", function () {
            createInstance({
                enableTime: true,
                defaultDate: "2017-1-1 10:00",
            });
            expect(fp.hourElement).toBeDefined();
            expect(fp.minuteElement).toBeDefined();
            expect(fp.amPM).toBeDefined();
            if (!fp.hourElement || !fp.minuteElement || !fp.amPM)
                return;
            expect(fp.hourElement.value).toEqual("10");
            expect(fp.minuteElement.value).toEqual("00");
            expect(fp.amPM.textContent).toEqual("AM");
            incrementTime("hourElement", 1);
            expect(fp.hourElement.value).toEqual("11");
            incrementTime("minuteElement", 1);
            expect(fp.minuteElement.value).toEqual("05");
            simulate("mousedown", fp.amPM, { which: 1 }, MouseEvent);
            expect(fp.amPM.textContent).toEqual("PM");
            simulate("wheel", fp.hourElement, {
                wheelDelta: 1,
            }, MouseEvent);
            expect(fp.hourElement.value).toEqual("12");
            fp.hourElement.value = "9";
            simulate("input", fp.hourElement);
            expect(fp.hourElement.value).toEqual("09");
        });
        it("time input respects minDate", function () {
            createInstance({
                enableTime: true,
                dateFormat: "Y-m-d H:i",
                defaultDate: "2017-1-1 4:00",
                minDate: "2017-1-01 3:35",
            });
            expect(!!fp.minDateHasTime).toBe(true);
            var _a = [
                fp.hourElement,
                fp.minuteElement,
            ], hourElem = _a[0], minuteElem = _a[1];
            incrementTime("hourElement", -1);
            expect(hourElem.value).toEqual("03");
            expect(minuteElem.value).toEqual("35");
            incrementTime("hourElement", -1);
            expect(hourElem.value).toEqual("03");
            incrementTime("minuteElement", -1);
            expect(minuteElem.value).toEqual("35");
            incrementTime("minuteElement", 1);
            expect(minuteElem.value).toEqual("40");
            hourElem.value = "2";
            simulate("input", hourElem);
            jest.runAllTimers();
            expect(hourElem.value).toEqual("03");
            minuteElem.value = "00";
            simulate("input", minuteElem);
            expect(minuteElem.value).toEqual("35");
        });
        it("time input respects maxDate", function () {
            createInstance({
                enableTime: true,
                defaultDate: "2017-1-1 3:00",
                maxDate: "2017-1-01 3:35",
            });
            var _a = [
                fp.hourElement,
                fp.minuteElement,
            ], hourElem = _a[0], minuteElem = _a[1];
            incrementTime("hourElement", -1);
            expect(hourElem.value).toEqual("02");
            incrementTime("hourElement", 3);
            expect(hourElem.value).toEqual("03");
            incrementTime("minuteElement", 8);
            expect(minuteElem.value).toEqual("35");
        });
        it("time input respects same-day minDate/maxDate", function () {
            createInstance({
                enableTime: true,
                minDate: "2017-1-01 2:00 PM",
                maxDate: "2017-1-01 3:35 PM",
            });
            var _a = [
                fp.hourElement,
                fp.minuteElement,
            ], hourElem = _a[0], minuteElem = _a[1];
            fp.setDate("2017-1-1 2:30 PM");
            incrementTime("hourElement", -1);
            simulate("wheel", hourElem, {
                wheelDelta: -1,
            }, MouseEvent);
            expect(hourElem.value).toEqual("02");
            incrementTime("hourElement", 4);
            expect(hourElem.value).toEqual("03");
            incrementTime("minuteElement", 8);
            simulate("wheel", minuteElem, {
                wheelDelta: 1,
            }, MouseEvent);
            expect(minuteElem.value).toEqual("35");
        });
        it("time picker: implicit selectedDate", function () {
            createInstance({
                enableTime: true,
                noCalendar: true,
            });
            expect(fp.selectedDates.length).toEqual(0);
            incrementTime("minuteElement", 1);
            expect(fp.selectedDates.length).toEqual(1);
            expect(fp.selectedDates[0].getDate()).toEqual(new Date().getDate());
        });
        it("time picker: minDate/maxDate + preloading", function () {
            createInstance({
                enableTime: true,
                noCalendar: true,
                minDate: "02:30",
                defaultDate: "3:30",
            });
            var _a = [
                fp.hourElement,
                fp.minuteElement,
                fp.amPM,
            ], hours = _a[0], minutes = _a[1], amPM = _a[2];
            expect(hours.value).toBe("03");
            expect(minutes.value).toBe("30");
            expect(amPM.textContent).toBe("AM");
            incrementTime("hourElement", -1);
            expect(hours.value).toBe("02");
            fp.set("maxDate", "04:30");
            incrementTime("hourElement", 3);
            expect(hours.value).toBe("04");
            simulate("mousedown", amPM, { which: 1 }, MouseEvent);
            expect(amPM.textContent).toBe("AM");
            fp.clear();
            fp.setDate("03:30");
            expect(hours.value).toBe("03");
            fp.setDate("05:30");
            expect(hours.value).toBe("03");
            fp.setDate("00:30");
            expect(hours.value).toBe("03");
        });
        it("should delay time input validation on keydown", function () {
            createInstance({
                enableTime: true,
                defaultDate: new Date().setHours(17, 30, 0, 0),
                minDate: new Date().setHours(16, 30, 0, 0),
                time_24hr: true,
            });
            var hours = fp.hourElement;
            hours.value = "16";
            simulate("input", hours, {}, KeyboardEvent);
            jest.runAllTimers();
            expect(hours.value).toEqual("16");
            hours.value = "1";
            simulate("input", hours);
            expect(hours.value).toEqual("1");
            jest.runAllTimers();
            jest.runAllTimers();
            expect(hours.value).toEqual("16");
        });
        it("should have working strap mode", function () {
            var wrapper = document.createElement("div");
            var input = document.createElement("input");
            input.setAttribute("data-input", "");
            wrapper.appendChild(input);
            ["open", "close", "toggle", "clear"].forEach(function (type) {
                var e = document.createElement("button");
                e.setAttribute("data-" + type, "");
                wrapper.appendChild(e);
            });
            var instance = createInstance({
                wrap: true,
            }, wrapper);
            expect(instance.input).toEqual(input);
            simulate("click", wrapper.childNodes[1], { which: 1 }, MouseEvent);
            expect(instance.isOpen).toEqual(true);
            simulate("click", wrapper.childNodes[2], { which: 1 }, MouseEvent);
            expect(instance.isOpen).toEqual(false);
            simulate("click", wrapper.childNodes[3], { which: 1 }, MouseEvent);
            expect(instance.isOpen).toEqual(true);
            simulate("click", wrapper.childNodes[3], { which: 1 }, MouseEvent);
            expect(instance.isOpen).toEqual(false);
            instance.setDate(new Date());
            expect(instance.selectedDates.length).toEqual(1);
            expect(instance.selectedDateElem).toBeDefined();
            instance.selectedDateElem &&
                expect(parseInt(instance.selectedDateElem.textContent)).toEqual(new Date().getDate());
            simulate("click", wrapper.childNodes[4], { which: 1 }, MouseEvent);
            expect(instance.selectedDates.length).toEqual(0);
            expect(instance.input.value).toEqual("");
            instance.destroy();
            wrapper.parentNode && wrapper.parentNode.removeChild(wrapper);
        });
        it("valid mouseover behavior in range mode", function () {
            createInstance({
                mode: "range",
            });
            var day = function (i) { return fp.days.childNodes[i]; };
            simulate("mouseover", fp.days.childNodes[15]);
            expect(fp.selectedDates.length).toEqual(0);
            fp.setDate("2016-1-17");
            expect(fp.selectedDates.length).toEqual(1);
            simulate("mouseover", fp.days.childNodes[32]);
            expect(day(21).classList.contains("startRange")).toEqual(true);
            expect(day(32).classList.contains("endRange")).toEqual(true);
            for (var i = 22; i < 32; i++)
                expect(day(i).classList.contains("inRange")).toEqual(true);
            fp.clear();
            fp.set("disable", ["2016-1-12", "2016-1-20"]);
            fp.setDate("2016-1-17");
            simulate("mouseover", day(32));
            expect(day(32).classList.contains("endRange")).toEqual(false);
            expect(day(24).classList.contains("disabled")).toEqual(true);
            expect(day(25).classList.contains("notAllowed")).toEqual(true);
            for (var i = 25; i < 32; i++)
                expect(day(i).classList.contains("inRange")).toEqual(false);
            for (var i = 17; i < 22; i++) {
                expect(day(i).classList.contains("notAllowed")).toEqual(false);
                expect(day(i).classList.contains("disabled")).toEqual(false);
            }
            simulate("mousedown", fp.days.childNodes[17], { which: 1 }, MouseEvent);
            expect(fp.selectedDates.length).toEqual(2);
            expect(fp.input.value).toEqual("2016-01-13 to 2016-01-17");
        });
        it("show and hide prev/next month arrows", function () {
            var isArrowVisible = function (which) {
                return fp[which].style.display !== "none";
            };
            createInstance({
                minDate: "2099-1-1",
                maxDate: "2099-3-4",
                mode: "range",
            });
            expect(fp.currentMonth).toBe(0);
            expect(isArrowVisible("prevMonthNav")).toBe(false);
            expect(isArrowVisible("nextMonthNav")).toBe(true);
            simulate("mousedown", fp.days.childNodes[10], { which: 1 }, MouseEvent);
            jest.runOnlyPendingTimers();
            simulate("mousedown", fp.nextMonthNav, { which: 1 }, MouseEvent);
            expect(isArrowVisible("prevMonthNav")).toBe(true);
            expect(isArrowVisible("nextMonthNav")).toBe(true);
            simulate("mousedown", fp.nextMonthNav, { which: 1 }, MouseEvent);
            expect(isArrowVisible("prevMonthNav")).toBe(true);
            expect(isArrowVisible("nextMonthNav")).toBe(false);
        });
    });
    describe("Localization", function () {
        it("By locale config option", function () {
            createInstance({
                locale: ru_1.Russian,
            });
            expect(fp.l10n.months.longhand[0]).toEqual("Январь");
            createInstance();
            expect(fp.l10n.months.longhand[0]).toEqual("January");
        });
        it("By overriding default locale", function () {
            index_1.default.localize(ru_1.Russian);
            expect(index_1.default.l10ns.default.months.longhand[0]).toEqual("Январь");
            createInstance();
            expect(fp.l10n.months.longhand[0]).toEqual("Январь");
        });
        it("correctly formats altInput", function () {
            createInstance({
                locale: ru_1.Russian,
                altInput: true,
                altFormat: "F",
                defaultDate: "2016-12-27T16:16:22.585Z",
            });
            var altInput = fp.altInput;
            expect(altInput.value).toEqual("Декабрь");
            fp.destroy();
            createInstance({
                locale: "en",
                altInput: true,
                altFormat: "F",
                defaultDate: "2016-12-27T16:16:22.585Z",
            });
            expect(fp.altInput.value).toEqual("December");
        });
    });
});
