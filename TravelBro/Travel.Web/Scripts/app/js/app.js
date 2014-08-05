'use strict';

/* App Module */

var travelbroApp = angular.module('travelbroApp', [
  'ngRoute',
  'travelbroControllers',
  'travelbroFilters',
  'travelbroServices'
]);

travelbroApp.config(['$routeProvider',
    function ($routeProvider) {
        $routeProvider.
            when('/sign-in', {
                templateUrl: '/Scripts/app/partials/sign-in.html',
                controller: 'SignInCtrl'
            }).
            when('/sign-up', {
                templateUrl: '/Scripts/app/partials/sign-up.html',
                controller: 'SignUpCtrl'
            }).
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


travelbroApp.run(['$location', '$window', 'Auth', function ($location, $window, Auth) {
    var tokenData = $window.localStorage.getItem('tokenData');
    var isTokenDataValid = tokenData && tokenData.length && tokenData !== 'null';
    if (isTokenDataValid) {
        Auth.token.set(tokenData);
    }
}]);