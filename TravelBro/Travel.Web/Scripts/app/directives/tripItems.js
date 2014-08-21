define(['./module'], function (directives) {
    'use strict';
    directives.directive('tripItems', ['$compile', function ($compile) {
        return {
            restrict: 'E',
            link: function (scope, element, attrs) {
                scope.$watch('tripItems', function(newVal, oldVal) {
                    if (newVal) {
                        var aBtn = '<add-trip-item/>';
                        element.append(aBtn);
                        for (var i = 0; i < newVal.length; i++) {
                            switch (newVal[i].type) {
                                case "visit":
                                    element.append('<visit></visit>');
                                    break;
                                case "route":
                                    element.append('<route></route>');
                                    break;
                            }
                            element.append(aBtn);
                        }
                        $compile(element.contents())(scope);
                    }
                });
            }
        };
    }]);
});