'use strict';

/* App Module */

var travelbroApp = angular.module('travelbroApp', [
  'ngRoute',
  'travelbroControllers',
  'travelbroFilters',
  'travelbroServices'
]);

travelbroApp.config(['$routeProvider',
  function($routeProvider) {
    $routeProvider.
      when('/trips', {
        templateUrl: '/Scripts/app/partials/trip-list.html',
        controller: 'TripListCtrl'
      }).
      when('/trips/:tripId', {
        templateUrl: '/Scripts/app/partials/trip-edit.html',
        controller: 'TripEditCtrl'
      }).
      when('/trips/new', {
          templateUrl: '/Scripts/app/partials/trip-edit.html',
          controller: 'TripEditCtrl'
      }).
      otherwise({
        redirectTo: '/trips'
      });
  }]);
