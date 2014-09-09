define(['./module'], function (controllers) {
    'use strict';
    controllers.controller('AlertsCtrl', ['$scope', 'Alerts', function ($scope, Alerts) {
        $scope.close = function(id) {
            Alerts.remove(id);
        }
    }]);
});