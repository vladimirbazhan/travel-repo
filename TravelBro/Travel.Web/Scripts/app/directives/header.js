define(['./module'], function (directives) {
    'use strict';
    directives.directive('header', [function () {
        return {
            restrict: 'E',
            templateUrl: '/Scripts/app/partials/header.html',
            replace: true,
            transclude: true
        };
    }]);
});