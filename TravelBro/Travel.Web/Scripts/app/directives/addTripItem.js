define(['./module'], function (directives) {
    'use strict';
    directives.directive('addTripItem', [function () {
        var isOpen = true;
        var templ =
            '<span class="dropdown">' +
                '<a href class="dropdown-toggle">add item</a>' +
                '<ul class="dropdown-menu">' +
                '<li><a href ng-click="addVisit()">Add visit</a></li>' +
                '<li><a href ng-click="addRoute()">Add route</a></li>' +
                '</ul></span>';
        return {
            restrict: 'E',
            replace: true,
            template: templ,
            link: function (scope, element, attrs) {
                element.find('li a').click(function () {
                    //TODO: implement better solution for closing dropdown
                    element.click();
                });
            }
        };
    }]);
});