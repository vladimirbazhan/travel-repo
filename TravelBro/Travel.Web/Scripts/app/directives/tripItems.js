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
                            var addBtn = $(aBtn).data('params', { order: 0 });
                            element.append(addBtn);
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
                                addBtn = $(aBtn).data('params', { order: newVal[i].data.Order + 1 });
                                element.append(addBtn);
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
                        onclick: scope.handlers.addVisit,
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