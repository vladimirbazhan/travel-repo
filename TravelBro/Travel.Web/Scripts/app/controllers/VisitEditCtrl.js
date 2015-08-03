define(['./module'], function (controllers) {
    'use strict';

    controllers.controller('VisitEditCtrl', ['$scope', '$routeParams', '$location', 'Auth', 'Backend', 'Entity', 'Alerts', 'GMapsUtils',
        function ($scope, $routeParams, $location, Auth, Backend, Entity, Alerts, GMapsUtils) {
            $scope.editMode = $routeParams.visitId == 'new' ? false : true;
            $scope.signedIn = Auth.token.isSet();
            $scope.legend = $scope.editMode ? "Edit visit" : "Create visit";
            
            $scope.trip = Backend.trips.get({ tripId: $routeParams.tripId }, function () {
                if (!$scope.editMode) {
                    var mapInfo = JSON.parse($scope.trip.MapInfo);
                    $scope.map.setCenter(new google.maps.LatLng(mapInfo.mapCenter.G, mapInfo.mapCenter.K));
                    $scope.map.setZoom(mapInfo.mapZoom);
                }
            }, function (err) {
                Alerts.add('danger', JSON.stringify(err));
            });

            $scope.map = null;
            $scope.marker = null;
            $scope.mapOptions = {
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

            $scope.savePhoto = function(photo, callbacks) {
                Backend.visits.savePhoto({ tripId: $scope.trip.Id, visitId: $scope.visit.Id }, photo, callbacks);
            }
            $scope.onAllPhotosSaved = function() {
                Backend.visits.get({ visitId: $routeParams.visitId }, function (res) {
                    $scope.visit.Photos = res.Photos;
                });
            }
            $scope.stickPositionToggle = stickPositionToggle;

            $scope.save = function () {
                $scope.visit.TripId = $scope.trip.Id;
                $scope.visit.Cost = parseFloat($scope.visit.Cost) || 0;
                if (!$scope.isMapStick) {
                    var mapInfo = {};
                    mapInfo.mapCenter = $scope.map.getCenter();
                    mapInfo.mapZoom = $scope.map.getZoom();
                    $scope.visit.MapInfo = JSON.stringify(mapInfo);
                }
                if (!$scope.editMode) {
                    $scope.visit.Order = (typeof $routeParams.order != 'undefined') ? ($routeParams.order + 1) : -1;
                }

                if ($scope.editMode) {
                    Backend.visits.update({ visitId: $scope.visit.Id }, $scope.visit, function () {
                        Alerts.add('info', 'Changes saved');
                    }, function (err) {
                        Alerts.add('danger', 'Error ' + err.status + ': ' + err.statusText);
                    });
                } else {
                    Backend.visits.save($scope.visit, function () {
                        Alerts.add('info', 'Changes saved');
                        $location.$$search = {};
                        $location.path('/trips/edit/' + $routeParams.tripId);
                    }, function (err) {
                        Alerts.add('danger', 'Error ' + err.status + ': ' + err.statusText);
                    });
                }
            };
            $scope.delete = function() {
                Backend.visits.delete({ visitId: $scope.visit.Id, }, function () {
                    Alerts.add('info', 'Visit deleted');
                    $location.path('/trips/edit/' + $scope.trip.Id);
                }, function (err) {
                    Alerts.add('danger', 'Error ' + err.status + ': ' + err.statusText);
                });
            };

            $scope.selectPlace = function (placeId) {
                var service = new google.maps.places.PlacesService($scope.map);
                service.getDetails({
                    placeId: placeId
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
                $scope.isMapStick = $scope.editMode;
                if ($scope.editMode) {
                    $scope.visit = Backend.visits.get({ visitId: $routeParams.visitId }, function (res) {
                        $scope.selectPlace(res.GPlaceId);
                        var mapInfo = JSON.parse(res.MapInfo);
                        $scope.map.setCenter(new google.maps.LatLng(mapInfo.mapCenter.G, mapInfo.mapCenter.K));
                        $scope.map.setZoom(mapInfo.mapZoom);
                    }, function (err) {
                        Alerts.add('danger', JSON.stringify(err));
                    });
                } else {
                    $scope.visit = Entity.visit.Default();
                }

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
            
            function stickPositionToggle() {
                if ($scope.isMapStick) {
                    var mapInfo = {};
                    mapInfo.mapCenter = $scope.map.getCenter();
                    mapInfo.mapZoom = $scope.map.getZoom();
                    $scope.visit.MapInfo = JSON.stringify(mapInfo);
                }
            };
        }]);
});