define(['./module'], function (controllers) {
    'use strict';
    controllers.controller('TripViewCtrl', TripViewCtrl);

    TripViewCtrl.$inject = ['$scope', '$routeParams', '$location', 'Backend', 'Auth', 'Entity', 'Alerts'];

    function TripViewCtrl($scope, $routeParams, $location, Backend, Auth, Entity, Alerts) {
        var vm = this;
        vm.isOwner = null;
        vm.userName = Auth.getUserName();
        vm.map = null;
        vm.mapOptions = {
            center: new google.maps.LatLng(31.203405, 7.382813),
            zoom: 2,
            minZoom: 1,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };

        init();

        // private stuff

        function init() {
            vm.trip = Backend.trips.get({ tripId: $routeParams.tripId }, function (trip) {
                vm.isOwner = (Auth.token.isSet() && Auth.getUserName() == trip.AuthorEmail) ? {} : null;
                var mapInfo = JSON.parse(vm.trip.MapInfo);
                vm.map.setCenter(new google.maps.LatLng(mapInfo.mapCenter.G, mapInfo.mapCenter.K));
                vm.map.setZoom(mapInfo.mapZoom);
            }, function (err) {
                Alerts.add('danger', JSON.stringify(err));
            });
        }

        function mapNavigateByTitle() {
            var nameWords = vm.trip.Name.split(" ");
            var curr = 0;
            while (curr < nameWords.length) {
                if (nameWords[curr][0] != nameWords[curr][0].toUpperCase()) {
                    nameWords.splice(curr, 1);
                } else {
                    curr++;
                }
            }

            if (nameWords.length == 0)
                return;

            var geocoder = new google.maps.Geocoder();
            geocoder.geocode({ 'address': nameWords[0] }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    vm.map.fitBounds(results[0].geometry.viewport || results[0].geometry.bounds);
                }
            });
        };
    };
});