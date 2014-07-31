'use strict';

/* Controllers */

var travelbroControllers = angular.module('travelbroControllers', []);

travelbroControllers.controller('TripListCtrl', ['$scope', '$location', 'Trips',
  function ($scope, $location, Trips) {
      $scope.trips = Trips.trips.query();
      $scope.tripsOrder = 'Name';

      $scope.create = function() {
          $location.path('/trips/new');
      };
  }]);

travelbroControllers.controller('TripEditCtrl', ['$scope', '$routeParams', '$location', 'Trips',
  function ($scope, $routeParams, $location, Trips) {

      $scope.editMode = $routeParams.tripId == 'new' ? false : true;

      $scope.legend = $scope.editMode ? "Edit trip" : "Create trip";

      if ($scope.editMode) {
          $scope.trip = Trips.trips.get({ tripId: $routeParams.tripId }, function (trip) {
              // trip fetched
          });
      } else {
          $scope.trip = {};
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