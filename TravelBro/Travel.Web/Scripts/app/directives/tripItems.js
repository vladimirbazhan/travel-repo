define(['./module'], function(directives) {
    'use strict';
    directives.directive('tripItems', [
        '$location', '$route', 'Backend', 'Alerts', function ($location, $route, Backend, Alerts) {
            return {
                scope: {
                    items: '=',
                },
                templateUrl: '/Scripts/app/partials/trip-items.html',
                restrict: 'E',
                link: function (scope, element, attrs) {
                    scope.dropdownOrders = [0];
                    scope.dropdownItems = [getDropdownItems(scope, 0)];

                    scope.changeOrder = function (dragItem, newOrder) {
                        dragItem.data.Order = newOrder;

                        var onsuccess = function() {
                            Alerts.add('info', 'Changes saved');
                            $route.reload();
                        };
                        var onfailure = function(res) {
                            Alerts.add('danger', res.error_description);
                        };

                        switch (dragItem.type) {
                            case 'visit':
                                Backend.visits.update({ visitId: dragItem.data.Id }, dragItem.data, onsuccess, onfailure);
                                break;
                            case 'route':
                                Backend.routes.update({ routeId: dragItem.data.Id }, dragItem.data, onsuccess, onfailure);
                                break;
                        }
                    }

                    scope.$watch('items', function(newVal) {
                        if (newVal) {
                            scope.items.forEach(function (x, i) {
                                scope.dropdownOrders.push(x.data.Order + 1);
                                scope.dropdownItems.push(getDropdownItems(scope, x.data.Order + 1));
                            });
                        }
                    });
                }
            };

            function getDropdownItems(scope, order) {
                return [
                    {
                        text: 'Add visit',
                        onclick: addVisit,
                        clickData: order
                    },
                    {
                        text: 'Add route',
                        onclick: addRoute,
                        clickData: order
                    }
                ];
            }

            function addVisit(e, order) {
                $location.path($location.url() + '/visit/new').search({ order: order });
            };

            function addRoute(e, order) {
                $location.path($location.url() + '/route/new').search({ order: order });
            };
        }
    ]);
});