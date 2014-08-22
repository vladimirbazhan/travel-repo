define(['./module'], function (directives) {
    'use strict';
    directives.directive('route', [function () {
        return {
            restrict: 'E',
            template: '<p>my route info</p>',
            link: function (scope, element, attrs) {
            }
        };
    }]);
});