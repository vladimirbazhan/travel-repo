define(['./module'], function(directives) {
    'use strict';
    directives.directive('route', ['$timeout', '$location',
        function ($timeout, $location) {
            return {
                scope: {
                    item: '='
                },
                restrict: 'A',
                replace: false,
                templateUrl: '/Scripts/app/partials/route-small.html',
                link: function (scope, element, attrs) {
                    var route = scope.item;
                    scope.edit = function () {
                        $location.path('/trips/' + route.TripId + '/route/' + route.Id);
                    }

                    if (!(route.StartGPlaceId && route.FinishGPlaceId)) {
                        return;
                    }

                    var mapDiv = $('<div>');
                    var map = new google.maps.Map(mapDiv.get()[0], {});
                    var service = new google.maps.places.PlacesService(map);

                    getStartPlace(scope, service);
                    getFinishPlace(scope, service);
                }
            };

            function getStartPlace(scope, service) {
                service.getDetails({
                    placeId: scope.item.StartGPlaceId
                }, function (place, status) {
                    if (status == google.maps.places.PlacesServiceStatus.OK) {
                        onSuccess(scope, true, place);
                    } else {
                        $timeout(getStartPlace, 100);
                    }
                });
            };

            function getFinishPlace(scope, service) {
                service.getDetails({
                    placeId: scope.item.FinishGPlaceId
                }, function (place, status) {
                    if (status == google.maps.places.PlacesServiceStatus.OK) {
                        onSuccess(scope, false, place);
                    } else {
                        $timeout(getFinishPlace, 100);
                    }
                });
            };

            function onSuccess(scope, isStartPlace, place) {
                scope.$apply(function () {
                    if (isStartPlace) 
                        scope.item.startPlace = place;
                    else {
                        scope.item.finishPlace = place;
                    }
                    
                    if (scope.item.startPlace && scope.item.finishPlace) {
                        scope.item.loaded = true;
                    }
                });
            }

        }
    ]);
});