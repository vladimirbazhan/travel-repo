define(['./module'], function (controllers) {
    'use strict';

    controllers.controller('VisitEditCtrl', ['$scope', '$routeParams', '$location', 'Auth', 'Backend', 'Entity', 'Alerts', 'GMapsUtils',
        function ($scope, $routeParams, $location, Auth, Backend, Entity, Alerts, GMapsUtils) {
            $scope.editMode = false;
            $scope.signedIn = Auth.token.isSet();
            $scope.legend = $scope.editMode ? "Edit visit" : "Create visit";
            $scope.trip = Backend.trips.get({ tripId: $routeParams.tripId });

            $scope.map = null;
            $scope.marker = null;
            $scope.mapOptions = {
                center: new google.maps.LatLng(-34.397, 150.644),
                zoom: 5,
                minZoom: 1,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };

            $scope.place = {};
            $scope.placeObject = {};
            $scope.nearbyPlaces = [];

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

            $scope.selectPlace = function (place) {
                var service = new google.maps.places.PlacesService($scope.map);
                service.getDetails({
                    placeId: place.place_id
                }, function (place, status) {
                    if (status == google.maps.places.PlacesServiceStatus.OK) {
                        $scope.place = place;
                        $scope.visit.GPlaceId = place.place_id;
                        $scope.$apply();
                    }
                });
            };

            setTimeout(init, 0); // wait until directives are loaded

            function init() {
                if (navigator.geolocation)
                    navigator.geolocation.getCurrentPosition(function (pos) {
                        var me = new google.maps.LatLng(pos.coords.latitude, pos.coords.longitude);
                        $scope.map.setCenter(me);
                    });

                google.maps.event.addListener($scope.map, 'click', function (e) {
                    if ($scope.marker)
                        $scope.marker.setMap(null);

                    $scope.marker = new google.maps.Marker({
                        position: e.latLng,
                        map: $scope.map
                    });

                    GMapsUtils.performNearbySearch($scope.map, e.latLng, function(results) {
                        $scope.nearbyPlaces = results;
                        $scope.$apply();
                    });
                });
            };
        }]);
});