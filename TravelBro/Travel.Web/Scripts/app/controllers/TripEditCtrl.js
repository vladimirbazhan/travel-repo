define(['./module'], function (controllers) {
    'use strict';
    
    controllers.controller('TripEditCtrl', ['$scope', '$routeParams', '$location', 'Backend', 'Auth', 'Entity', function ($scope, $routeParams, $location, Backend, Auth, Entity) {
        $scope.editMode = $routeParams.tripId == 'new' ? false : true;
        $scope.signedIn = Auth.token.isSet();
        $scope.legend = $scope.editMode ? "Edit trip" : "Create trip";
        $scope.dateOptions = {
          showOn: "button",
          changeYear: true,
          autoSize: true
        };

        if ($scope.editMode) {
            $scope.trip = Backend.trips.get({ tripId: $routeParams.tripId }, null, function(err) {
              alert(JSON.stringify(err));
          });
        } else {
          $scope.trip = Entity.trip.Default;
        }

        $scope.save = function () {
          if ($scope.editMode) {
              Backend.trips.update({ tripId: $scope.trip.Id }, $scope.trip, function () {
                  alert($scope.editMode ? "Changes saved" : "Trip created");
              });
          } else {
              Backend.trips.save($scope.trip, function () {
                  alert("Trip created");
                  $location.path('/trips');
              });
          }
        };
        $scope.delete = function() {
            Backend.trips.delete({ tripId: $scope.trip.Id }, function () {
              alert("Trip deleted");
              $location.path('/trips');
          });
        };

        $scope.addVisit = function () {
            $location.path($location.url() + '/visit-new');
        }
        $scope.addRoute = function () {
            $location.path($location.url() + '/route-new');
        }
    }]);
});