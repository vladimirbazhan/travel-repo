define(['./module'], function (controllers) {
    'use strict';

    controllers.controller('VisitEditCtrl', ['$scope', '$routeParams', '$location', 'Auth', 'Backend', 'Entity', function ($scope, $routeParams, $location, Auth, Backend, Entity) {
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
            $scope.visit = Entity.visit.Default();
        }

        $scope.save = function () {
            $scope.visit.TripId = $scope.trip.Id;
            $scope.visit.Cost = parseFloat($scope.visit.Cost) || 0;
            Backend.visits.save($scope.visit, function () {
                alert("Changes saved");
                $location.path('/trips/' + $routeParams.tripId);
            });
        };
    }]);
});