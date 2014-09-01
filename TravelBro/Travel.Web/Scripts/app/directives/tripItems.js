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
                                    var visitElem = $('<visit></visit>');
                                    visitElem.data('visit', newVal[i]);
                                    element.append(visitElem);
                                    break;
                                case "route":
                                    var routeElem = $('<route></route>');
                                    routeElem.data('route', newVal[i]);
                                    element.append(routeElem);
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