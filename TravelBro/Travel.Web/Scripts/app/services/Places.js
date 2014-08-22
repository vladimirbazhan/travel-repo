define(['./module'], function (services) {
    'use strict';
    services.factory('Places', ['$resource', function ($resource) {
        return $resource("/api/trips", {},
        {
            'query': { url: '/api/places', method: 'GET', isArray: true }
        });
    }]);
});