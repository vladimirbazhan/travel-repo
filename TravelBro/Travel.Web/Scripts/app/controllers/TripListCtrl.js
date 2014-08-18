define(['./module'], function (controllers) {
    'use strict';
    controllers.controller('TripListCtrl', ['$scope', '$location', '$route', '$window', 'Trips', 'Auth', function ($scope, $location, $route, $window, Trips, Auth) {
        $scope.trips = Trips.trips.query();
        $scope.tripsOrder = 'Name';
        $scope.tripFilter = '';
        $scope.signedIn = Auth.token.isSet();
        $scope.userName = Auth.getUserName();

        $scope.create = function() {
          $location.path('/trips/new');
        };

        $scope.signIn = function () {
          $scope.modalShown = !$scope.modalShown;
        };

        $scope.logOut = function () {
          Auth.logOut();
          $route.reload();
        };
  }]);
});