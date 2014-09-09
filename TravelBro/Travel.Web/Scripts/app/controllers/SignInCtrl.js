define(['./module'], function (controllers) {
    'use strict';
    controllers.controller('SignInCtrl', ['$scope', '$location', 'Auth', 'Alerts', function ($scope, $location, Auth, Alerts) {
        $scope.mail = "";
        $scope.password = "";

        $scope.ok = function () {
          Auth.signIn($scope.mail, $scope.password, function() {
              $location.path('/trips');
          }, function (res) {
              Alerts.add('danger', res.error_description);
          });
        }
    }]);
});