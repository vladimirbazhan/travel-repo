define(['./module'], function (controllers) {
    'use strict';
    controllers.controller('SignInCtrl', ['$scope', '$location', 'Auth', function ($scope, $location, Auth) {
        $scope.mail = "";
        $scope.password = "";

        $scope.ok = function () {
          Auth.signIn($scope.mail, $scope.password, function() {
              $location.path('/trips');
          }, function(res) {
              alert(res.error_description);
          });
        }
    }]);
});