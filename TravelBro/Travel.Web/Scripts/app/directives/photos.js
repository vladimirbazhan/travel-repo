define(['./module'], function (directives) {
    'use strict';
    directives.directive('photos', ['$location', 'Alerts', function ($location, Alerts) {
        return {
            scope: {
                photos: '=',
                columns: '=?',
                savePhoto: '&',
                onAllSaved: '&',
            },
            restrict: 'E',
            replace: true,
            templateUrl: '/Scripts/app/partials/photos.html',
            link: {
                pre: function (scope, element, attrs) {
                    scope.rows = [];
                    scope.readonly = attrs.hasOwnProperty('readonly');
                    if (!scope.readonly) {
                        scope.selectedPhotos = [];
                        scope.removeSelectedPhoto = function (index) {
                            scope.selectedPhotos.splice(index, 1);
                        }
                        scope.onSelectedPhotosChanged = function (e) {
                            scope.$apply(function () {
                                var files = e.target.files;
                                scope.selectedPhotos = [];
                                for (var i = 0; i < files.length; ++i) {
                                    scope.selectedPhotos.push(files[i]);
                                }
                            });
                        }
                        scope.fileChangedHandler = { event: 'change', handler: scope.onSelectedPhotosChanged };
                    }
                },
                post: function (scope, element, attrs) {
                    scope.columns = angular.isDefined(scope.columns) ? parseInt(scope.columns) : 4;
                    if (!scope.readonly) {
                        scope.save = function () {
                            scope.operationLeft = scope.selectedPhotos.length;
                            scope.selectedPhotos.forEach(function (currPhoto) {
                                scope.savePhoto({ photo: currPhoto, callbacks: savePhotoCallbacks(scope, currPhoto) });
                            });
                        }
                    }

                    scope.$watch('photos', function (newVal, oldVal) {
                        if (newVal) {
                            organizePhotos(scope, scope.columns);
                        }
                    });
                }
            }
        };

        function organizePhotos(scope, columns) {
            var imgServerPath = $location.protocol() + '://' + $location.host() + ':' + $location.port() + '/' + 'api/trips/photos/';

            var rowCount = Math.ceil(scope.photos.length / columns);
            var rows = [];
            for (var iRow = 0; iRow < rowCount; ++iRow) {
                var cols = [];
                for (var jCol = 0; jCol < columns; jCol++) {
                    var imgIndex = iRow * columns + jCol;
                    if (imgIndex >= scope.photos.length) {
                        break;
                    }
                    var position = null;
                    if (jCol == 0) {
                        position = 'leftmost';
                    } else if (jCol == columns - 1) {
                        position = 'rightmost';
                    }

                    cols.push({
                        Path: imgServerPath + scope.photos[imgIndex].ImagePath,
                        IsMain: scope.photos[imgIndex].IsMain,
                        Position: position
                    });
                }
                rows.push(cols);
            }
            scope.rows = rows;
        }

        function savePhotoCallbacks(scope, currPhoto) {
            return {
                onprogress: function (e) {
                    scope.$apply(function () {
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
                    scope.$apply(function () {
                        currPhoto.uploadStatus = 'Done';
                        if (!--scope.operationLeft) {
                            Alerts.add('info', 'Photos Saved');
                            scope.onAllSaved();
                            scope.selectedPhotos = [];
                        }
                    });
                },
                onfail: function (err) {
                    currPhoto.uploadStatus = 'Failed';
                    Alerts.add('danger', err.Message);
                    if (!--scope.operationLeft) {
                        scope.onAllSaved();
                        scope.selectedPhotos = [];
                    }
                }
            }
        }
    }]);
});