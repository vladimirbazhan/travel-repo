define(['./module'], function(directives) {
    'use strict';
    directives.directive('visit', [
        function() {

            var buildHtml = function(visit) {
                return '<a class="list-group-item">' +
                    '<bold><h4 class="list-group-item-heading">Visited: ' + (visit.Place ? visit.Place.name + ' (' + visit.Place.formatted_address + ')' : '') + '</h4></bold>' +
                    (visit.Description ? '<p class="list-group-item-text">' + visit.Description + '</p>' : '') +
                    '<p class="list-group-item-text">Spent money: ' + visit.Cost + '</p>' +
                    '<p class="list-group-item-text">Order: ' + visit.Order + '</p>' +
                    '<p class="list-group-item-text">Start: ' + visit.Start + ' Finish: ' + visit.Finish + '</p>' +
                    '</a>';
            };
            return {
                scope: {
                    item: '=',
                },
                restrict: 'A',
                link: function (scope, element) {
                    var visit = scope.item;

                    var map = new google.maps.Map(element.get()[0], {});
                    var service = new google.maps.places.PlacesService(map);
                    var exec = function() {
                        service.getDetails({
                            placeId: visit.GPlaceId
                        }, function(place, status) {
                            if (status == google.maps.places.PlacesServiceStatus.OK) {
                                visit.Place = place;
                                element.append(buildHtml(visit));
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