define(['./module'], function (controllers) {
    'use strict';
    controllers.controller('SignUpCtrl', ['$scope', '$location', 'Auth', 'Alerts', function ($scope, $location, Auth, Alerts) {
        $scope.mail = '';
        $scope.password = '';
        $scope.confirmPassword = '';

        $scope.register = function () {
          Auth.signUp($scope.mail, $scope.password, $scope.confirmPassword, function() {
              Auth.signIn($scope.mail, $scope.password, function () {
                  $location.path('/trips');
              }, function (res) {
                  Alerts.add('danger', res.error_description);
              });
          }, function (err) {
              var str = '';
              if (err.ModelState) {
                  var errs = err.ModelState['model.Email'];
                  if (errs) {
                      str += 'Email:';
                      errs.forEach(function (curr) {
                          str += ' ' + curr;
                      });
                      str += '\n';
                  }
                  var errs = err.ModelState['model.Password'];
                  if (errs) {
                      str += 'Password:';
                      errs.forEach(function (curr) {
                          str += ' ' + curr;
                      });
                      str += '\n';
                  }
                  var errs = err.ModelState['model.ConfirmPassword'];
                  if (errs) {
                      str += 'ConfirmPassword:';
                      errs.forEach(function (curr) {
                          str += ' ' + curr;
                      });
                      str += '\n';
                  }
                  var errs = err.ModelState[''];
                  var isFirst = true;
                  if (errs) {
                      errs.forEach(function (curr) {
                          str += (isFirst ? '' : ' ') + curr;
                          isFirst = false;
                      });
                  }
              } else {
                  str = err.Message;
              }
              Alerts.add('danger', str);
          });
        };
    }]);
});