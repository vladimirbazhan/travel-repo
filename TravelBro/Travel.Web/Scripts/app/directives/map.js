define(['./module'], function (directives) {
    'use strict';
    directives.directive('map', [function () {
        return {
            scope: {
                options: '=',
                map: '='
            },
            restrict: 'E',
            template: '<div></div>',
            replace: true,
            link: function (scope, element) {
                scope.options = scope.options || {};
                scope.map = new google.maps.Map(element.get(0), scope.options);
            }
        };
    }]);
});