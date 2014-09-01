define(['./module'], function (controllers) {
    'use strict';

    controllers.controller('RouteEditCtrl', ['$scope', '$routeParams', '$location', 'Auth', 'Backend', 'Entity', function ($scope, $routeParams, $location, Auth, Backend, Entity) {
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
            $scope.route = Entity.route.Default;
        }

        $scope.save = function () {
            $scope.route.TripId = $scope.trip.Id;
            $scope.route.Cost = parseFloat($scope.route.Cost) || 0;
            Backend.routes.save($scope.route, function () {
                alert("Changes saved");
                $location.path('/trips/' + $routeParams.tripId);
            });
        };
    }]);
});