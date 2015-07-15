define(['./module'], function(controllers) {
    'use strict';
    controllers.controller('TripEditCtrl', TripEditCtrl);

    TripEditCtrl.$inject = ['$scope', '$routeParams', '$location', 'Backend', 'Auth', 'Entity', 'Alerts', '$http', '$route'];

    function TripEditCtrl($scope, $routeParams, $location, Backend, Auth, Entity, Alerts, $http, $route) {
        var vm = this;
        vm.editMode = $routeParams.tripId == 'new' ? false : true;
        vm.signedIn = Auth.token.isSet();
        vm.userName = Auth.getUserName();
        vm.legend = vm.editMode ? "Edit trip" : "Create trip";
        vm.map = null;
        vm.mapOptions = {
            center: new google.maps.LatLng(31.203405, 7.382813),
            zoom: 2,
            minZoom: 1,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };
        vm.dateOptions = {
            showOn: "button",
            changeYear: true,
            autoSize: true
        };

        // methods
        vm.save = save;
        vm.delete = deleteTrip;
        vm.savePhoto = savePhoto;
        vm.onAllPhotosSaved = onAllPhotosSaved;

        init();

        // private stuff

        function init() {
            if (vm.editMode) {
                vm.trip = Backend.trips.get({ tripId: $routeParams.tripId }, function () {
                    mapNavigateByTitle();
                }, function(err) {
                    Alerts.add('danger', JSON.stringify(err));
                });
            } else {
                vm.trip = Entity.trip.Default();
            }
        }

        function save() {
            new TripChangesSaver(vm).save();
        };

        function deleteTrip() {
            Backend.trips.delete({ tripId: vm.trip.Id }, function() {
                Alerts.add('info', 'Trip deleted');
                $location.path('/trips');
            }, function(err) {
                Alerts.add('danger', 'Error ' + err.status + ': ' + err.statusText);
            });
        };

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
            geocoder.geocode({ 'address': nameWords[0] }, function(results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    vm.map.fitBounds(results[0].geometry.viewport || results[0].geometry.bounds);
                }
            });
        };

        function savePhoto(photo, callbacks) {
            Backend.trips.savePhoto({ tripId: vm.trip.Id }, photo, callbacks);
        }

        function onAllPhotosSaved() {
            Backend.trips.get({ tripId: $routeParams.tripId }, function (res) {
                vm.trip.Photos = res.Photos;
            });
        }

        // performs saving changes to the current trip
        function TripChangesSaver(vm) {
            this.save = function () {
                if (vm.editMode) {
                    Backend.trips.update({ tripId: vm.trip.Id }, vm.trip, function () {
                        Alerts.add('info', 'Changes saved');
                        mapNavigateByTitle();
                        oncomplete();
                    }, function (err) {
                        Alerts.add('danger', 'Error ' + err.status + ': ' + err.statusText);
                        oncomplete();
                    });
                } else {
                    Backend.trips.save(vm.trip, function (trip) {
                        Alerts.add('info', 'Trip created');
                        vm.trip = trip;
                        oncomplete();
                    }, function (err) {
                        Alerts.add('danger', 'Error ' + err.status + ': ' + err.statusText);
                        oncomplete();
                    });
                }
            }

            function oncomplete() {
                if (!vm.editMode) {
                    $location.path('/trips');
                }
            }
        }
    };
});