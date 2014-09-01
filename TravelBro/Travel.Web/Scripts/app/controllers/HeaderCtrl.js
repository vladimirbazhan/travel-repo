define(['./module'], function (controllers) {
    'use strict';

    controllers.controller('HeaderCtrl', ['$scope', '$route', '$location', 'Auth', function ($scope, $route, $location, Auth) {
        $scope.signedIn = Auth.token.isSet();
        $scope.userName = Auth.getUserName();
        $scope.headerFilter = '';

        $scope.logOut = function () {
            Auth.logOut();
            $route.reload();
        };

        $scope.isActive = function(viewLoc) {
            return viewLoc == $location.path();
        };
    }]);
});