define(['./module'], function (directives) {
    'use strict';
    directives.directive('route', [function () {
        return {
            restrict: 'E',
            template: '<div></div>',
            link: function (scope, element, attrs) {
                var route = element.data('route').data;
                debugger;
                var templ =
                    '<a class="list-group-item">' +
                        '<bold><h4 class="list-group-item-heading">Start place: ' + route.StartPlace.Name + '</h4></bold>' +
                        '<bold><h4 class="list-group-item-heading">Finish place: ' + route.FinishPlace.Name + '</h4></bold>' +
                        '<p class="list-group-item-text">' + (route.Comment || '') + '</p>' +
                        '<p class="list-group-item-text">Spent money: ' + route.Cost + '</p>' +
                    '</a>';
                element.append(templ);
            }
        };
    }]);
});