define(['./module'], function (directives) {
    'use strict';
    directives.directive('visit', [function () {
        return {
            restrict: 'E',
            template: '<p>my visit info</p>',
            link: function(scope, element, attrs) {
            }
        };
    }]);
});