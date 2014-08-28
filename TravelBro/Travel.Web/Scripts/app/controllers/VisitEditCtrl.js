define(['./module'], function (controllers) {
    'use strict';

    controllers.controller('VisitEditCtrl', ['$scope', '$routeParams', '$location', 'Auth', 'Backend', function ($scope, $routeParams, $location, Auth, Backend) {
        $scope.editMode = false;
        $scope.signedIn = Auth.token.isSet();
        $scope.legend = $scope.editMode ? "Edit visit" : "Create visit";
        $scope.places = Backend.places.query();
        $scope.trip = Backend.trips.get({ tripId: $routeParams.tripId });

        $scope.dateOptions = {
            showOn: "button",
            changeYear: true,
            autoSize: true
        };

        if ($scope.editMode) {
            $scope.visit = {};
        } else {
            $scope.visit = {
                Start: new Date(),
                Finish: new Date(),
                ActivityOrder: 0,
                Cost: 0
            };
        }
        $scope.visit.TripId = parseInt($routeParams.tripId);

        $scope.save = function() {
            $scope.visit.Cost = parseFloat($scope.visit.Cost) || 0;
            $scope.visit.PlaceId = parseInt($scope.visit.PlaceId);
            Backend.visits.save($scope.visit, function () {
                $scope.trip.Visits = $scope.trip.Visits || [];
                $scope.trip.Visits.push($scope.visit.Id);
                Backend.trips.update({ tripId: $scope.trip.Id }, $scope.trip, function () {
                    alert("Changes saved");
                    $location.path('/trips/' + $routeParams.tripId);
                });
            });
        };
    }]);
});