define(['./module'], function (directives) {
    'use strict';
    directives.directive('placeAutocomplete', [function () {
        return {
            scope: {
                details: '=',
                placeId: '=',
                options: '=',
                placeObject: '=',
                map: '='
            },
            restrict: 'A',
            link: function (scope, element, attrs) {
                var opts = {};

                if (scope.map) {
                    $.extend(opts, { bounds: scope.map.getBounds() });
                    scope.map.bounds_changed = function () {
                        scope.placeObject.setBounds(scope.map.getBounds());
                    }
                }

                scope.placeObject = new google.maps.places.Autocomplete(element[0], opts);
                google.maps.event.addListener(scope.placeObject, 'place_changed', function () {
                    scope.$apply(function () {
                        scope.details = scope.placeObject.getPlace();
                        scope.placeId = scope.details.place_id;
                    });
                });

                google.maps.event.addDomListener(element[0], 'keydown', function(e) {
                    if (e.keyCode == 13)
                    {
                        e.preventDefault(); 
                    } 
                }); 
            }
        };
    }]);
});