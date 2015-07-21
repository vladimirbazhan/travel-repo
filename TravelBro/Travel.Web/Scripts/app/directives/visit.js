define(['./module'], function(directives) {
    'use strict';
    directives.directive('visit', ['$location',
        function ($location) {
            return {
                scope: {
                    item: '=',
                },
                restrict: 'A',
                templateUrl: '/Scripts/app/partials/visit-small.html',
                link: function (scope, element, attrs) {
                    scope.readonly = attrs.hasOwnProperty('readonly');
                    var visit = scope.item;

                    scope.edit = function() {
                        $location.path('/trips/' + visit.TripId + '/visit/' + visit.Id);
                    }

                    scope.showEditButton = false;
                    element.hover(function () {
                        scope.$apply(function () { scope.showEditButton = true; })
                    }, function () {
                        scope.$apply(function () { scope.showEditButton = false; })
                    });
                    
                    var mapDiv = $('<div>');
                    var map = new google.maps.Map(mapDiv.get()[0], {});
                    var service = new google.maps.places.PlacesService(map);

                    var exec = function() {
                        service.getDetails({
                            placeId: visit.GPlaceId
                        }, function(place, status) {
                            if (status == google.maps.places.PlacesServiceStatus.OK) {
                                scope.$apply(function() {
                                    visit.Place = place;
                                    visit.loaded = true;
                                });
                            } else {
                                setTimeout(exec, 100);
                            }
                        });
                    };
                    exec();
                }
            };
        }
    ]);
});