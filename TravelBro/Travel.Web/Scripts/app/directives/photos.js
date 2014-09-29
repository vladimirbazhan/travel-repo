define(['./module'], function (directives) {
    'use strict';
    directives.directive('photos', ['$location', function ($location) {
        return {
            restrict: 'E',
            replace: true,
            template: '<div class="photos"></div>',
            link: function (scope, element, attrs) {
                scope.$watch('trip.Photos', function (newVal, oldVal) {
                    if (newVal) {
                        var colCount = 4;
                        var imgIndex = 0;
                        var rowCount = Math.ceil(scope.trip.Photos.length / colCount);
                        for (var iRow = 0; iRow < rowCount; ++iRow) {
                            var row = $('<div class="row"></div>');
                            for (var jCol = 0; jCol < colCount; jCol++) {
                                imgIndex = iRow * colCount + jCol;
                                if (imgIndex >= scope.trip.Photos.length) {
                                    break;
                                }
                                var imgPath = $location.protocol() + '://' + $location.host() + ':' + $location.port() + '/' + 'api/trips/photos/';
                                var img = '<img src="' + imgPath + scope.trip.Photos[imgIndex] + '/' + '" class="img-responsive img-rounded center-block" />';
                                var col = $('<div class="col-md-3">' + img + '</div>');
                                row.append(col);
                            }
                            element.append(row);
                            if (imgIndex >= scope.trip.Photos.length) {
                                break;
                            }
                        }
                    }
                });
            }
        };
    }]);
});