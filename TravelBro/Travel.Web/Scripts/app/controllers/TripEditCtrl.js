define(['./module'], function (controllers) {
    'use strict';
    
    controllers.controller('TripEditCtrl', ['$scope', '$routeParams', '$location', 'Backend', 'Auth', 'Entity', 'Alerts', '$http', '$route', function ($scope, $routeParams, $location, Backend, Auth, Entity, Alerts, $http, $route) {
        $scope.editMode = $routeParams.tripId == 'new' ? false : true;
        $scope.signedIn = Auth.token.isSet();
        $scope.legend = $scope.editMode ? "Edit trip" : "Create trip";
        $scope.photos = [];
        $scope.dateOptions = {
          showOn: "button",
          changeYear: true,
          autoSize: true
        };

        if ($scope.editMode) {
            $scope.trip = Backend.trips.get({ tripId: $routeParams.tripId }, null, function (err) {
                Alerts.add('danger', JSON.stringify(err));
          });
        } else {
          $scope.trip = Entity.trip.Default();
        }

        $scope.save = function () {
          if ($scope.editMode) {
              Backend.trips.update({ tripId: $scope.trip.Id }, $scope.trip, function () {
                  Alerts.add('info', 'Changes saved');
              }, function (err) {
                  Alerts.add('danger', 'Error ' + err.status + ': ' + err.statusText);
              });
          } else {
              Backend.trips.save($scope.trip, function () {
                  Alerts.add('info', 'Trip created');
                  $location.path('/trips');
              });
          }
        };

        $scope.savePhotos = function () {
            var fd = new FormData();
            for (var i = 0; i < $scope.photos.length; ++i) {
                fd.append('photo[]', $scope.photos[i]);
            }
            fd.append('fileType', 'tripphotos');
            // TODO: implement via service
            $http.post('/api/trips/' + $scope.trip.Id + '/photos', fd, {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).success(function () {
                Alerts.add('info', 'Photos Saved');
                $route.reload();
            }).error(function (err) {
                Alerts.add('danger', err.Message);
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
    }]);
});