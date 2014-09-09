define(['./module'], function (controllers) {
    'use strict';

    controllers.controller('RouteEditCtrl', ['$scope', '$routeParams', '$location', 'Auth', 'Backend', 'Entity', 'Alerts', function ($scope, $routeParams, $location, Auth, Backend, Entity, Alerts) {
        $scope.editMode = false;
        $scope.signedIn = Auth.token.isSet();
        $scope.legend = $scope.editMode ? "Edit route" : "Create route";
        $scope.trip = Backend.trips.get({ tripId: $routeParams.tripId });

        $scope.startPlace = {};
        $scope.finishPlace = {};

        $scope.dateOptions = {
            showOn: "button",
            changeYear: true,
            autoSize: true
        };

        if ($scope.editMode) {
            $scope.route = {};
        } else {
            $scope.route = Entity.route.Default();
        }

        $scope.save = function () {
            $scope.route.TripId = $scope.trip.Id;
            $scope.route.Cost = parseFloat($scope.route.Cost) || 0;
            Backend.routes.save($scope.route, function () {
                Alerts.add('info', 'Changes saved');
                $location.path('/trips/' + $routeParams.tripId);
            });
        };
    }]);
});