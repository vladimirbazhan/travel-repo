define(['app'], function (app) {
    'use strict';

    return app.config(['$routeProvider', function ($routeProvider) {
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
            when('/places', {
                templateUrl: '/Scripts/app/partials/place-list.html',
                controller: 'PlaceListCtrl'
            }).
            when('/trips/:tripId/visit-new', {
                templateUrl: '/Scripts/app/partials/visit-edit.html',
                controller: 'VisitEditCtrl'
            }).
            otherwise({
                redirectTo: '/trips'
            });
    }]);
});