define(['./module'], function(directives) {
    'use strict';
    directives.directive('onEvents', [
        function() {
            return {
                scope: {
                    onEvents: '='
                },
                restrict: 'A',
                link: function(scope, element, attrs) {
                    if (!!scope.onEvents) {
                        if (Array.isArray(scope.onEvents)) {
                            scope.onEvents.forEach(function(item) {
                                element.bind(item.event, item.data, item.handler);
                            });
                        } else if (typeof scope.onEvents === "object") {
                            var item = scope.onEvents;
                            element.bind(item.event, item.data, item.handler);
                        }
                    }
                }
            };
        }
    ]);
});