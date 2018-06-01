"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
function compareDates(date1, date2, timeless) {
    if (timeless !== false) {
        return (new Date(date1.getTime()).setHours(0, 0, 0, 0) -
            new Date(date2.getTime()).setHours(0, 0, 0, 0));
    }
    return date1.getTime() - date2.getTime();
}
exports.compareDates = compareDates;
exports.monthToStr = function (monthNumber, shorthand, locale) { return locale.months[shorthand ? "shorthand" : "longhand"][monthNumber]; };
exports.getWeek = function (givenDate) {
    var onejan = new Date(givenDate.getFullYear(), 0, 1);
    return Math.ceil(((givenDate.getTime() - onejan.getTime()) / 86400000 +
        onejan.getDay() +
        1) /
        7);
};
exports.duration = {
    DAY: 86400000,
};
