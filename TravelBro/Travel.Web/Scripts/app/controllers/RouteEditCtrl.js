define(['./module'], function (controllers) {
    'use strict';

    controllers.controller('RouteEditCtrl', ['$scope', '$routeParams', '$location', 'Auth', 'Backend', function ($scope, $routeParams, $location, Auth, Backend) {
        $scope.editMode = false;
        $scope.signedIn = Auth.token.isSet();
        $scope.legend = $scope.editMode ? "Edit route" : "Create route";
        $scope.places = Backend.places.query();
        $scope.trip = Backend.trips.get({ tripId: $routeParams.tripId });

        $scope.dateOptions = {
            showOn: "button",
            changeYear: true,
            autoSize: true
        };

        if ($scope.editMode) {
            $scope.route = {};
        } else {
            $scope.route = {
                Start: new Date(),
                Finish: new Date(),
                ActivityOrder: 0,
                Cost: 0
            };
        }
        $scope.route.TripId = parseInt($routeParams.tripId);

        $scope.save = function () {
            debugger;
            $scope.route.Cost = parseFloat($scope.route.Cost) || 0;
            //$scope.visit.PlaceId = parseInt($scope.visit.PlaceId);
            Backend.routes.save($scope.route, function () {
                $scope.trip.Routes = $scope.trip.Routes || [];
                $scope.trip.Routes.push($scope.route.Id);
                Backend.trips.update({ tripId: $scope.trip.Id }, $scope.trip, function () {
                    alert("Changes saved");
                    $location.path('/trips/' + $routeParams.tripId);
                });
            });
        };
    }]);
});