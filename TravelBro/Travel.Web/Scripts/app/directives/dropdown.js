define(['./module'], function(directives) {
    'use strict';
    directives.directive('dropdown', [
        function() {
            return {
                scope: {
                    text: '@',
                    items: '=',
                },
                restrict: 'E',
                templateUrl: '/Scripts/app/partials/dropdown.html',
            };
        }
    ]);
});