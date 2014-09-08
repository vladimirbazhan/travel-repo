define(['./module'], function (directives) {
    'use strict';
    directives.directive('placeAutocomplete', [function () {
        return {
            scope: {
                details: '=',
                placeId: '=',
                options: '=',
                placeObject: '='
            },
            restrict: 'A',
            link: function (scope, element, attrs) {
                var opts = {};
                scope.placeObject = new google.maps.places.Autocomplete(element[0], opts);
                google.maps.event.addListener(scope.placeObject, 'place_changed', function () {
                    scope.$apply(function () {
                        scope.details = scope.placeObject.getPlace();
                        scope.placeId = scope.details.place_id;
                    });
                });
            }
        };
    }]);
});