define(['./module'], function (directives) {
    'use strict';
    directives.directive('notificationArea', [function () {
        return {
            restrict: 'E',
            templateUrl: '/Scripts/app/partials/notificationArea.html',
            replace: true,
            transclude: true
        };
    }]);
});