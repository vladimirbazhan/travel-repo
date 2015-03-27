define(['./module'], function(controllers) {
    'use strict';

    controllers.controller('RouteEditCtrl', [
        '$scope', '$routeParams', '$location', 'Auth', 'Backend', 'Entity', 'Alerts', 'GMapsUtils', function($scope, $routeParams, $location, Auth, Backend, Entity, Alerts, GMapsUtils) {
            $scope.editMode = false;
            $scope.signedIn = Auth.token.isSet();
            $scope.legend = $scope.editMode ? "Edit route" : "Create route";
            $scope.trip = Backend.trips.get({ tripId: $routeParams.tripId });
            $scope.transTypes = Backend.transTypes.query();

            $scope.startPlace = {};
            $scope.finishPlace = {};

            $scope.dateOptions = {
                showOn: "button",
                changeYear: true,
                autoSize: true
            };

            $scope.map = null;
            $scope.mapControl = {};
            $scope.mapOptions = {
                center: new google.maps.LatLng(-34.397, 150.644),
                zoom: 5,
                minZoom: 1,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            $scope.nearbyPlaces = [];

            setTimeout(function() {
                $scope.mapControl.setContextMenu(geContextMenuItems);
            }, 0);

            if ($scope.editMode) {
                $scope.route = {};
            } else {
                $scope.route = Entity.route.Default();
            }

            $scope.transTypeSelected = function (transType) {
                $scope.route.TransType = transType;
            }

            $scope.save = function() {
                $scope.route.TripId = $scope.trip.Id;
                $scope.route.Cost = parseFloat($scope.route.Cost) || 0;
                Backend.routes.save($scope.route, function() {
                    Alerts.add('info', 'Changes saved');
                    $location.path('/trips/' + $routeParams.tripId);
                }, function(err) {
                    Alerts.add('danger', 'Error ' + err.status + ': ' + err.statusText);
                });
            };

            $scope.selectPlace = function(place) {
                var service = new google.maps.places.PlacesService($scope.map);
                service.getDetails({
                    placeId: place.place_id
                }, function(place, status) {
                    if (status == google.maps.places.PlacesServiceStatus.OK) {
                        if (nearbyShowsDirectionsFrom) {
                            $scope.$apply(function() {
                                $scope.startPlace = place;
                                $scope.route.StartGPlaceId = place.place_id;
                            });

                        } else {
                            $scope.$apply(function() {
                                $scope.finishPlace = place;
                                $scope.route.FinishGPlaceId = place.place_id;
                            });
                        }
                    }
                });
            };

            // context menu stuff
            var menuItems = {
                from: { text: 'Directions from here', handler: onDirectionsFrom },
                to: { text: 'Directions to here', handler: onDirectionsTo },
                clear: { text: 'Clear', handler: onDirectionsClear },
                divider: { divider: 0 }
            };
            var markerFrom = null;
            var markerTo = null;
            var nearbyShowsDirectionsFrom = true;

            function geContextMenuItems() {
                var items = [];
                if (!markerFrom) {
                    items.push(menuItems.from);
                }
                if (!markerTo) {
                    items.push(menuItems.to);
                }
                if (markerFrom || markerTo) {
                    if (items.length) {
                        items.push(menuItems.divider);
                    }
                    items.push(menuItems.clear);
                }

                return items;
            }

            function onDirectionsFrom(e) {
                nearbyShowsDirectionsFrom = true;
                markerFrom = new google.maps.Marker({
                    position: e,
                    map: $scope.map
                });

                GMapsUtils.performNearbySearch($scope.map, e, function(results) {
                    $scope.nearbyPlaces = results;
                    $scope.$apply();
                });
            }

            function onDirectionsTo(e) {
                nearbyShowsDirectionsFrom = false;
                markerTo = new google.maps.Marker({
                    position: e,
                    map: $scope.map
                });
                GMapsUtils.performNearbySearch($scope.map, e, function(results) {
                    $scope.nearbyPlaces = results;
                    $scope.$apply();
                });
            }

            function onDirectionsClear(e) {
                if (markerFrom) {
                    markerFrom.setMap(null);
                    markerFrom = null;
                }
                if (markerTo) {
                    markerTo.setMap(null);
                    markerTo = null;
                }

                $scope.$apply(function() {
                    $scope.nearbyPlaces = [];
                });
            }
        }
    ]);
});