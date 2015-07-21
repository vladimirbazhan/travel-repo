define(['app'], function (app) {
    'use strict';

    return app.config(['$routeProvider', function ($routeProvider) {
        $routeProvider.
            when('/sign-in', {
                templateUrl: '/Scripts/app/partials/sign-in.html',
                controller: 'SignInCtrl',
                controllerAs: 'signInVm'
            }).
            when('/sign-up', {
                templateUrl: '/Scripts/app/partials/sign-up.html',
                controller: 'SignUpCtrl',
                controllerAs: 'signUpVm'
            }).
            when('/trips', {
                templateUrl: '/Scripts/app/partials/trip-list.html',
                controller: 'TripListCtrl',
                controllerAs: 'tripListVm'
            }).
            when('/trips/:tripId', {
                templateUrl: '/Scripts/app/partials/trip-view.html',
                controller: 'TripViewCtrl',
                controllerAs: 'vm'
            }).
            when('/trips/edit/:tripId', {
                templateUrl: '/Scripts/app/partials/trip-edit.html',
                controller: 'TripEditCtrl',
                controllerAs: 'vm'
            }).
            when('/trips/:tripId/visit/:visitId', {
                templateUrl: '/Scripts/app/partials/visit-edit.html',
                controller: 'VisitEditCtrl'
            }).
            when('/trips/:tripId/route/:routeId', {
                templateUrl: '/Scripts/app/partials/route-edit.html',
                controller: 'RouteEditCtrl'
            }).
            otherwise({
                redirectTo: '/trips'
            });
    }]);
});