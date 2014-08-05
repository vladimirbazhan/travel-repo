'use strict';

/* Controllers */

var travelbroControllers = angular.module('travelbroControllers', []);

travelbroControllers.controller('TripListCtrl', ['$scope', '$location', '$route', '$window', 'Trips', 'Auth',
  function ($scope, $location, $route, $window, Trips, Auth) {
      $scope.trips = Trips.trips.query();
      $scope.tripsOrder = 'Name';
      $scope.tripFilter = '';
      $scope.signedIn = Auth.token.isSet();
      $scope.userName = Auth.getUserName();

      $scope.create = function() {
          $location.path('/trips/new');
      };

      $scope.signIn = function () {
          $scope.modalShown = !$scope.modalShown;
      };

      $scope.logOut = function () {
          Auth.logOut();
          $route.reload();
      };
  }]);

travelbroControllers.controller('TripEditCtrl', ['$scope', '$routeParams', '$location', 'Trips', 'Auth',
  function ($scope, $routeParams, $location, Trips, Auth) {

      $scope.editMode = $routeParams.tripId == 'new' ? false : true;
      $scope.signedIn = Auth.token.isSet();
      $scope.legend = $scope.editMode ? "Edit trip" : "Create trip";

      if ($scope.editMode) {
          $scope.trip = Trips.trips.get({ tripId: $routeParams.tripId }, function (trip) {
              // trip fetched
          }, function(err) {
              alert(JSON.stringify(err));
          });
      } else {
          $scope.trip = { IsPrivate: false };
      }

      $scope.save = function () {
          if ($scope.editMode) {
              Trips.trips.update({ tripId: $scope.trip.Id }, $scope.trip, function () {
                  alert($scope.editMode ? "Changes saved" : "Trip created");
              });
          } else {
              Trips.trips.save($scope.trip, function() {
                  alert("Trip created");
                  $location.path('/trips');
              });
          }
      };
      $scope.delete = function() {
          Trips.trips.delete({ tripId: $scope.trip.Id }, function() {
              alert("Trip deleted");
              $location.path('/trips');
          });
      };
  }]);

travelbroControllers.controller('SignInCtrl', ['$scope', '$location', 'Auth',
  function ($scope, $location, Auth) {
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

travelbroControllers.controller('SignUpCtrl', ['$scope', '$location', 'Auth',
  function ($scope, $location, Auth) {
      $scope.mail = '';
      $scope.password = '';
      $scope.confirmPassword = '';

      $scope.register = function () {
          Auth.signUp($scope.mail, $scope.password, $scope.confirmPassword, function() {
              Auth.signIn($scope.mail, $scope.password, function () {
                  $location.path('/trips');
              }, function (res) {
                  alert(res.error_description);
              });
          }, function (err) {
              debugger;
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
              alert(str);
          });
      };
  }]);