define(['./module'], function (controllers) {
    'use strict';

    controllers.controller('VisitEditCtrl', ['$scope', '$routeParams', '$location', 'Auth', 'Backend', 'Entity', 'Alerts',
        function ($scope, $routeParams, $location, Auth, Backend, Entity, Alerts) {
        $scope.editMode = false;
        $scope.signedIn = Auth.token.isSet();
        $scope.legend = $scope.editMode ? "Edit visit" : "Create visit";
        $scope.trip = Backend.trips.get({ tripId: $routeParams.tripId });

        $scope.place = {};

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
                Alerts.add('info', 'Changes saved');
                $location.path('/trips/' + $routeParams.tripId);
            }, function (err) {
                Alerts.add('danger', 'Error ' + err.status + ': ' + err.statusText);
            });
        };
    }]);
});