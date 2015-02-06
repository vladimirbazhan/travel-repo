define(['./module'], function (controllers) {
    'use strict';
    
    controllers.controller('TripEditCtrl', ['$scope', '$routeParams', '$location', 'Backend', 'Auth', 'Entity', 'Alerts', '$http', '$route', function ($scope, $routeParams, $location, Backend, Auth, Entity, Alerts, $http, $route) {
        $scope.editMode = $routeParams.tripId == 'new' ? false : true;
        $scope.signedIn = Auth.token.isSet();
        $scope.userName = Auth.getUserName();
        $scope.legend = $scope.editMode ? "Edit trip" : "Create trip";
        $scope.photos = [];

        $scope.map = null;
        $scope.mapOptions = {
            center: new google.maps.LatLng(31.203405, 7.382813),
            zoom: 2,
            minZoom: 1,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };

        $scope.dateOptions = {
          showOn: "button",
          changeYear: true,
          autoSize: true
        };

        if ($scope.editMode) {
            $scope.trip = Backend.trips.get({ tripId: $routeParams.tripId }, function() {
                mapNavigateByTitle();
            }, function (err) {
                Alerts.add('danger', JSON.stringify(err));
          });
        } else {
          $scope.trip = Entity.trip.Default();
        }

        $scope.save = function () {
          if ($scope.editMode) {
              Backend.trips.update({ tripId: $scope.trip.Id }, $scope.trip, function () {
                  Alerts.add('info', 'Changes saved');
                  mapNavigateByTitle();
              }, function (err) {
                  Alerts.add('danger', 'Error ' + err.status + ': ' + err.statusText);
              });
          } else {
              Backend.trips.save($scope.trip, function () {
                  Alerts.add('info', 'Trip created');
                  $location.path('/trips');
              }, function (err) {
                  Alerts.add('danger', 'Error ' + err.status + ': ' + err.statusText);
              });
          }
        };
        
        $scope.uploadFiles = function () {
            $scope.photos.forEach(function (currPhoto) {
                var form = new FormData();
                form.append('photo[]', currPhoto);

                // TODO: implement via service
                $.ajax({
                    url: '/api/trips/' + $scope.trip.Id + '/photos',
                    data: form,
                    cache: false,
                    contentType: false,
                    processData: false,
                    type: 'POST',
                    xhr: function() {
                        var myXhr = $.ajaxSettings.xhr();
                        if (myXhr.upload) {
                            myXhr.upload.addEventListener('progress',function(e) {
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
                            }, false);
                        }
                        return myXhr;
                    }
                }).done(function (response) {
                    $scope.$apply(function () {
                        currPhoto.uploadStatus = 'Done';
                        var totalDone = 0;
                        $scope.photos.forEach(function (photo) {
                            if (photo.uploadStatus == 'Done') {
                                totalDone++;
                            }
                        });
                        if (totalDone == $scope.photos.length) {
                            Alerts.add('info', 'Photos Saved');
                            $route.reload();
                        }
                    });
                }).fail(function (err) {
                    currPhoto.uploadStatus = 'Failed';
                    Alerts.add('danger', err.Message);
                });
            });
        }

        $scope.onSelectedPhotosChanged = function (files) {
            $scope.photos = [];
            for (var i = 0; i < files.length; ++i) {
                $scope.photos.push(files[i]);
            }
            $scope.$apply();
        }

        $scope.removeSelectedPhoto = function ($index) {
            $scope.photos.splice($index, 1);
        }

        $scope.delete = function() {
            Backend.trips.delete({ tripId: $scope.trip.Id }, function () {
                Alerts.add('info', 'Trip deleted');
                $location.path('/trips');
            }, function (err) {
                Alerts.add('danger', 'Error ' + err.status + ': ' + err.statusText);
            });
        };

        $scope.addVisit = function () {
            $location.path($location.url() + '/visit-new');
        }
        $scope.addRoute = function () {
            $location.path($location.url() + '/route-new');
        }

        // private methods
        function mapNavigateByTitle() {
            var nameWords = $scope.trip.Name.split(" ");
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
                    $scope.map.fitBounds(results[0].geometry.viewport || results[0].geometry.bounds);
                }
            });
        };
    }]);
});