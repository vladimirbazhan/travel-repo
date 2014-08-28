define(['./module'], function (directives) {
    'use strict';
    directives.directive('visit', [function () {
        return {
            restrict: 'E',
            template: '<div></div>',
            link: function (scope, element, attrs) {
                var visit = element.data('visit').data;
                var templ =
                    '<a class="list-group-item">' +
                        '<bold><h4 class="list-group-item-heading">Visited: ' + visit.Place.Name + '</h4></bold>' +
                        '<p class="list-group-item-text">' + visit.Description + '</p>' +
                        '<p class="list-group-item-text">Spent money: '+ visit.Cost +'</p>' +
                    '</a>';
                element.append(templ);
            }
        };
    }]);
});