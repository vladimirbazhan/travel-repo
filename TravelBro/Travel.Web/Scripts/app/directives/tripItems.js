define(['./module'], function(directives) {
    'use strict';
    directives.directive('tripItems', [
        '$compile', function($compile) {
            return {
                scope: {
                    items: '=',
                    handlers: '='
                },
                restrict: 'E',
                link: function(scope, element, attrs) {

                    scope.dropdownItems = getDropdownItems(scope);
                    var aBtn = '<dropdown text="add item" items="dropdownItems" />';

                    scope.$watch('items', function(newVal, oldVal) {
                        if (newVal) {
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

            function getDropdownItems(scope) {
                return [
                    {
                        text: 'Add visit',
                        onclick: scope.handlers.addVisit
                    },
                    {
                        text: 'Add route',
                        onclick: scope.handlers.addRoute
                    }
                ];
            }
        }
    ]);
});