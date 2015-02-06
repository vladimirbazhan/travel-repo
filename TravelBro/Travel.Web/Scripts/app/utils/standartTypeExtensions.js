define([], function () {
    'use strict';

    // extensions for Array
    (function() {
        Array.prototype.unique = function (predic) {
            var newArr = this.concat();

            if (!predic) {
                predic = function (a, b) {
                    return a === b;
                }
            }

            for (var i = 0; i < newArr.length; i++) {
                for (var j = i + 1; j < newArr.length; j++) {
                    if (predic(newArr[i], newArr[j])) {
                        newArr.splice(j--, 1);
                    }
                }
            }

            return newArr;
        };

        Array.prototype.filter = function (predic) {
            if (!predic) {
                return this;
            }
            var newArr = this.concat();
            var curr = 0;
            while (curr < newArr.length) {
                if (predic(newArr[curr])) {
                    newArr.splice(curr, 1);
                } else {
                    curr++;
                }
            }
            return newArr;
        };

        Array.prototype.find = function (callback) {
            for (var i = 0; i < this.length; i++) {
                if (callback(this[i]), i) {
                    return this[i];
                }
            }
            return null;
        };
    })();
});