define(['./module'], function (directives) {
    'use strict';
    directives.directive('tripItems', ['$compile', function ($compile) {
        return {
            restrict: 'E',
            link: function (scope, element, attrs) {
                scope.$watch('tripItems', function(newVal, oldVal) {
                    if (newVal) {
                        var aBtn = '<a href="" class="btn btn-link" ng-click="addTripItem();">Add trip item</a>';
                        for (var i = 0; i < newVal.length; i++) {
                            if (i == 0) {
                                element.append(aBtn);
                            }
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