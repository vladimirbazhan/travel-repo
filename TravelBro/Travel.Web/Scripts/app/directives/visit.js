define(['./module'], function(directives) {
    'use strict';
    directives.directive('visit', [
        function() {
            return {
                scope: {
                    item: '=',
                },
                restrict: 'A',
                templateUrl: '/Scripts/app/partials/visit-small.html',
                link: function (scope, element) {
                    var visit = scope.item;
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