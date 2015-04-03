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

        // filters array, returns removed items
        Array.prototype.filter = function (predic) {
            if (!predic) {
                return this;
            }
            var removed = [];
            var curr = 0;
            while (curr < this.length) {
                if (predic(this[curr])) {
                    removed.push(this.splice(curr, 1)[0]);
                } else {
                    curr++;
                }
            }
            return removed;
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