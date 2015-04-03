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
        vm.photos = [];
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
        vm.handlers = {
            addVisit: function(e, params) {
                $location.path($location.url() + '/visit-new').search({ order: params.order });
            },
            addRoute: function (e, params) {
                $location.path($location.url() + '/route-new').search({ order: params.order });
            }
        };

        vm.onSelectedPhotosChanged = onSelectedPhotosChanged;
        vm.removeSelectedPhoto = removeSelectedPhoto;

        init();

        // private stuff

        function init() {
            if (vm.editMode) {
                vm.trip = Backend.trips.get({ tripId: $routeParams.tripId }, function() {
                    mapNavigateByTitle();
                }, function(err) {
                    Alerts.add('danger', JSON.stringify(err));
                });
            } else {
                vm.trip = Entity.trip.Default();
            }
        }

        function save() {
            var saver = new TripChangesSaver(vm);
            saver.save();
        };

        function onSelectedPhotosChanged(e) {
            $scope.$apply(function() {
                var files = e.target.files;
                vm.photos = [];
                for (var i = 0; i < files.length; ++i) {
                    vm.photos.push(files[i]);
                }
            });
        };

        function removeSelectedPhoto($index) {
            vm.photos.splice($index, 1);
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

        // performs saving changes to the current trip
        function TripChangesSaver(vm) {
            var operationLeft = vm.photos.length + 1; // every photo + fields saving

            this.save = function () {
                if (vm.editMode) {
                    Backend.trips.update({ tripId: vm.trip.Id }, vm.trip, function () {
                        Alerts.add('info', 'Changes saved');
                        mapNavigateByTitle();
                        if (!--operationLeft)
                            oncomplete();
                    }, function (err) {
                        Alerts.add('danger', 'Error ' + err.status + ': ' + err.statusText);
                        if (!--operationLeft)
                            oncomplete();
                    });
                    savePhotos();
                } else {
                    Backend.trips.save(vm.trip, function (trip) {
                        Alerts.add('info', 'Trip created');
                        vm.trip = trip;
                        savePhotos();
                        if (!--operationLeft)
                            oncomplete();
                    }, function (err) {
                        Alerts.add('danger', 'Error ' + err.status + ': ' + err.statusText);
                        if (!--operationLeft)
                            oncomplete();
                    });
                }
            }

            function savePhotos() {
                vm.photos.forEach(function (currPhoto) {
                    Backend.trips.savePhoto({ tripId: vm.trip.Id }, currPhoto, {
                        onprogress: function (e) {
                            $scope.$apply(function () {
                                var percentCompleted;
                                if (e.lengthComputable) {
                                    percentCompleted = Math.round(e.loaded / e.total * 100);
                                    if (percentCompleted < 1) {
                                        currPhoto.uploadStatus = 'Uploading...';
                                    } else if (percentCompleted == 100) {
                                        currPhoto.uploadStatus = 'Saving...';
                                    } else {
                                        currPhoto.uploadStatus = percentCompleted + '%';
                                    }
                                } else {
                                    currPhoto.uploadStatus = "Length is not computable";
                                }
                            });
                        },
                        ondone: function (response) {
                            $scope.$apply(function () {
                                currPhoto.uploadStatus = 'Done';
                                if (!--operationLeft) {
                                    Alerts.add('info', 'Photos Saved');
                                    oncomplete();
                                }
                            });
                        },
                        onfail: function (err) {
                            currPhoto.uploadStatus = 'Failed';
                            Alerts.add('danger', err.Message);
                            if (!--operationLeft) {
                                oncomplete();
                            }
                        }
                    });
                });
            };

            function oncomplete() {
                if (!vm.editMode) {
                    $location.path('/trips');
                } else {
                    if (vm.photos.length) {
                        $route.reload();
                    }
                }
            }
        }
    };
});