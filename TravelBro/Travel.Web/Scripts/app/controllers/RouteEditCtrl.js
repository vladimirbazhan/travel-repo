define(['./module'], function(controllers) {
    'use strict';

    controllers.controller('RouteEditCtrl', [
        '$scope', '$routeParams', '$location', 'Auth', 'Backend', 'Entity', 'Alerts', 'GMapsUtils', function ($scope, $routeParams, $location, Auth, Backend, Entity, Alerts, GMapsUtils) {

            // init scope

            $scope.editMode = $routeParams.routeId == 'new' ? false : true;
            $scope.signedIn = Auth.token.isSet();
            $scope.legend = $scope.editMode ? "Edit route" : "Create route";
            $scope.trip = Backend.trips.get({ tripId: $routeParams.tripId }, function () {
                    if(!$scope.editMode){
                        var mapInfo = JSON.parse($scope.trip.MapInfo);
                        $scope.map.setCenter(new google.maps.LatLng(mapInfo.mapCenter.G, mapInfo.mapCenter.K));
                        $scope.map.setZoom(mapInfo.mapZoom);
                    }
                }, function(err) {
                    Alerts.add('danger', JSON.stringify(err));
                });
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
                minZoom: 1,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            $scope.nearbyPlaces = [];

            $scope.transTypeSelected = function (transType) {
                $scope.route.TransType = transType;
            }

            $scope.save = save;
            $scope.delete = deleteRoute;
            $scope.savePhoto = savePhoto;
            $scope.onAllPhotosSaved = onAllPhotosSaved;
            $scope.stickPositionToggle = stickPositionToggle;

            $scope.selectPlace = function (placeId) {
                selectPlaceImpl(placeId, nearbyShowsDirectionsFrom);
            };

            var markerFrom = null;
            var markerTo = null;
            var nearbyShowsDirectionsFrom = true;

            init();

            function init() {
                $scope.isMapStick = $scope.editMode;
                setTimeout(function () {
                    $scope.mapControl.setContextMenu(geContextMenuItems);
                }, 0);

                if ($scope.editMode) {
                    $scope.route = Backend.routes.get({ routeId: $routeParams.routeId }, function (res) {
                        selectPlaceImpl(res.StartGPlaceId, true);
                        selectPlaceImpl(res.FinishGPlaceId, false);
                        var mapInfo = JSON.parse(res.MapInfo);
                        $scope.map.setCenter(new google.maps.LatLng(mapInfo.mapCenter.G, mapInfo.mapCenter.K));
                        $scope.map.setZoom(mapInfo.mapZoom);
                    }, function (err) {
                        Alerts.add('danger', JSON.stringify(err));
                    });
                } else {
                    $scope.route = Entity.route.Default();
                }
            }

            function save() {
                $scope.route.TripId = $scope.trip.Id;
                $scope.route.Cost = parseFloat($scope.route.Cost) || 0;
                if (!$scope.isMapStick) {
                    var mapInfo = {};
                    mapInfo.mapCenter = $scope.map.getCenter();
                    mapInfo.mapZoom = $scope.map.getZoom();
                    $scope.route.MapInfo = JSON.stringify(mapInfo);
                }
                if (!$scope.editMode) {
                    $scope.route.Order = $routeParams.order !== 'undefined' ? $routeParams.order + 1 : -1;
                }

                if ($scope.editMode) {
                    Backend.routes.update({ routeId: $scope.route.Id }, $scope.route, function () {
                        Alerts.add('info', 'Changes saved');
                    }, function (err) {
                        Alerts.add('danger', 'Error ' + err.status + ': ' + err.statusText);
                    });
                } else {
                    Backend.routes.save($scope.route, function () {
                        Alerts.add('info', 'Changes saved');
                        $location.$$search = {};
                        $location.path('/trips/edit/' + $routeParams.tripId);
                    }, function (err) {
                        Alerts.add('danger', 'Error ' + err.status + ': ' + err.statusText);
                    });
                }
            }

            function deleteRoute () {
                Backend.routes.delete({ routeId: $scope.route.Id, }, function () {
                    Alerts.add('info', 'Route deleted');
                    $location.path('/trips/edit/' + $scope.trip.Id);
                }, function (err) {
                    Alerts.add('danger', 'Error ' + err.status + ': ' + err.statusText);
                });
            };

            function savePhoto (photo, callbacks) {
                Backend.routes.savePhoto({ tripId: $scope.trip.Id, routeId: $scope.route.Id }, photo, callbacks);
            }

            function onAllPhotosSaved () {
                Backend.routes.get({ routeId: $routeParams.routeId }, function (res) {
                    $scope.route.Photos = res.Photos;
                });
            }

            function selectPlaceImpl (placeId, isFrom) {
                var service = new google.maps.places.PlacesService($scope.map);
                service.getDetails({
                    placeId: placeId
                }, function (place, status) {
                    if (status == google.maps.places.PlacesServiceStatus.OK) {
                        if (isFrom) {
                            $scope.$apply(function () {
                                $scope.startPlace = place;
                                $scope.route.StartGPlaceId = place.place_id;
                            });

                        } else {
                            $scope.$apply(function () {
                                $scope.finishPlace = place;
                                $scope.route.FinishGPlaceId = place.place_id;
                            });
                        }
                    }
                });
            }

            function geContextMenuItems() {
                var menuItems = {
                    from: { text: 'Directions from here', handler: onDirectionsFrom },
                    to: { text: 'Directions to here', handler: onDirectionsTo },
                    clear: { text: 'Clear', handler: onDirectionsClear },
                    divider: { divider: 0 }
                };

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
            
            function stickPositionToggle() {
                if ($scope.isMapStick) {
                    var mapInfo = {};
                    mapInfo.mapCenter = $scope.map.getCenter();
                    mapInfo.mapZoom = $scope.map.getZoom();
                    $scope.route.MapInfo = JSON.stringify(mapInfo);
                }
            }
        }
    ]);
});