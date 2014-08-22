define(['./module'], function (controllers) {
    'use strict';

    controllers.controller('PlaceListCtrl', ['$scope', 'Places', function ($scope, Places) {
        $scope.places = Places.query();
    }]);
});