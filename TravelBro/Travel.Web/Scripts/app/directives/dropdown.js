define(['./module'], function(directives) {
    'use strict';
    directives.directive('dropdown', [
        '$compile', function($compile) {
            return {
                scope: {
                    text: '@',
                    items: '='
                },
                restrict: 'E',
                replace: true,
                template: '<span class="dropdown"></span>',
                link: function(scope, element) {
                    scope.items = scope.items || [];
                    element.append('<a href="" class="dropdown-toggle">' + scope.text + '</a>');
                    var ul = $('<ul class="dropdown-menu"></ul>');
                    scope.items.forEach(function(item) {
                        var li = $('<li><a href ng-click="addVisit()">' + item.text + '</a></li>').click(function() {
                            scope.$apply(function(e) {
                                item.onclick(e);
                            });
                        });
                        ul.append(li);
                    });
                    element.append(ul);

                    // trick for closing dropdown
                    element.find('li a').click(function() {
                        element.click();
                    });

                    $compile(element.contents())(scope);
                }
            };
        }
    ]);
});