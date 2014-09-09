define(['./module'], function (directives) {
    'use strict';
    directives.directive('nextOnEnter', [function () {
        return {
            restrict: 'A',
            link: function ($scope, elem, attrs) {
                elem.bind('keydown', function (e) {
                    var code = e.keyCode || e.which;
                    if (code === 13) {
                        e.preventDefault();

                        var selectables = $(':tabbable');
                        var current = $(':focus');
                        var nextIndex = 0;
                        if (current.length === 1) {
                            var currentIndex = selectables.index(current);
                            if (currentIndex + 1 < selectables.length) {
                                nextIndex = currentIndex + 1;
                            }
                        }
                        selectables.eq(nextIndex).focus();
                    }
                });
            }
        };
    }]);
});