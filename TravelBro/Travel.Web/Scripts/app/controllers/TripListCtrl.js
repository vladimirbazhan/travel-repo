define(['./module'], function (controllers) {
    'use strict';
    controllers.controller('TripListCtrl', ['$scope', '$location', '$route', '$window', 'Backend', 'Auth', function ($scope, $location, $route, $window, Backend, Auth) {
        $scope.trips = Backend.trips.query();
        $scope.tripsOrder = 'Name';

        $scope.create = function() {
          $location.path('/trips/new');
        };
  }]);
});