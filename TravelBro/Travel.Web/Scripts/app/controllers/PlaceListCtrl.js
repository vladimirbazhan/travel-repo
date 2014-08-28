define(['./module'], function (controllers) {
    'use strict';

    controllers.controller('PlaceListCtrl', ['$scope', 'Backend', function ($scope, Backend) {
        $scope.places = Backend.places.query();
    }]);
});