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
                    scope.$watch('items', function(newVal, oldVal) {
                        if (newVal) {
                            scope.groups = 'orderTripItems';
                            element.append(createDropdown(scope, 0));
                            for (var i = 0; i < newVal.length; i++) {
                                scope.item = newVal[i].data;
                                var itemElem = $('<div ' + newVal[i].type + ' item="item" draggable-item draggable-item-groups="{{groups}}" draggable-item-data="' + escape(JSON.stringify(scope.item)) + '"></div>');
                                $compile(itemElem)(scope);
                                element.append(itemElem);
                                element.append(createDropdown(scope, scope.item.Order + 1));
                            }
                        }
                    });
                }
            };

            function createDropdown(scope, order) {
                scope.dropdownItems = getDropdownItems(scope, order);
                var aBtn = '<div droppable-item droppable-item-groups="{{groups}}" droppable-item-data="' + escape(JSON.stringify({ order: order })) + '" width="100%"><dropdown text="add item" items="dropdownItems" style="border: 1px;" /></div>';
                var addBtn = $(aBtn);
                $compile(addBtn)(scope);
                return addBtn;
            }

            function getDropdownItems(scope, order) {
                return [
                    {
                        text: 'Add visit',
                        onclick: scope.handlers.addVisit,
                        clickData: order
                    },
                    {
                        text: 'Add route',
                        onclick: scope.handlers.addRoute,
                        clickData: order
                    }
                ];
            }
        }
    ]);
});