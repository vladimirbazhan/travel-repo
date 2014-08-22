define(['./module'], function (controllers) {
    'use strict';
    
    controllers.controller('TripEditCtrl', ['$scope', '$routeParams', '$location', 'Trips', 'Auth', function ($scope, $routeParams, $location, Trips, Auth) {
        $scope.editMode = $routeParams.tripId == 'new' ? false : true;
        $scope.signedIn = Auth.token.isSet();
        $scope.legend = $scope.editMode ? "Edit trip" : "Create trip";
        $scope.dateOptions = {
          showOn: "button",
          changeYear: true,
          autoSize: true
        };
        $scope.dateFrom = new Date();
        $scope.dateTo = new Date();

        if ($scope.editMode) {
          $scope.trip = Trips.trips.get({ tripId: $routeParams.tripId }, function (trip) {
              // trip fetched
              // TODO: fetch real items instead of fake ones
              var items = [];
              items.push({ type: "visit" });
              items.push({ type: "route" });
              items.push({ type: "visit" });
              items.push({ type: "route" });
              $scope.tripItems = items;
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

        $scope.addVisit = function () {
            console.log('addVisit Not implemented');
        }
        $scope.addRoute = function () {
            console.log('addRoute Not implemented');
        }
    }]);
});