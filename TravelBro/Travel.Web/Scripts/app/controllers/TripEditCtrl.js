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
            addVisit: function() {
                $location.path($location.url() + '/visit-new');
            },
            addRoute: function() {
                $location.path($location.url() + '/route-new');
            }
        };

        vm.uploadFiles = uploadFiles;
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
            if (vm.editMode) {
                Backend.trips.update({ tripId: vm.trip.Id }, vm.trip, function() {
                    Alerts.add('info', 'Changes saved');
                    mapNavigateByTitle();
                }, function(err) {
                    Alerts.add('danger', 'Error ' + err.status + ': ' + err.statusText);
                });
            } else {
                Backend.trips.save(vm.trip, function() {
                    Alerts.add('info', 'Trip created');
                    $location.path('/trips');
                }, function(err) {
                    Alerts.add('danger', 'Error ' + err.status + ': ' + err.statusText);
                });
            }
        };

        function uploadFiles() {
            vm.photos.forEach(function(currPhoto) {
                var form = new FormData();
                form.append('photo[]', currPhoto);

                // TODO: implement via service
                $.ajax({
                    url: '/api/trips/' + vm.trip.Id + '/photos',
                    data: form,
                    cache: false,
                    contentType: false,
                    processData: false,
                    type: 'POST',
                    xhr: function() {
                        var myXhr = $.ajaxSettings.xhr();
                        if (myXhr.upload) {
                            myXhr.upload.addEventListener('progress', function(e) {
                                $scope.$apply(function() {
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
                            }, false);
                        }
                        return myXhr;
                    }
                }).done(function(response) {
                    $scope.$apply(function() {
                        currPhoto.uploadStatus = 'Done';
                        var totalDone = 0;
                        vm.photos.forEach(function(photo) {
                            if (photo.uploadStatus == 'Done') {
                                totalDone++;
                            }
                        });
                        if (totalDone == vm.photos.length) {
                            Alerts.add('info', 'Photos Saved');
                            $route.reload();
                        }
                    });
                }).fail(function(err) {
                    currPhoto.uploadStatus = 'Failed';
                    Alerts.add('danger', err.Message);
                });
            });
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
    };
});