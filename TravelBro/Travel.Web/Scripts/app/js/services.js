'use strict';

/* Services */

var travelbroServices = angular.module('travelbroServices', ['ngResource']);

travelbroServices.factory('Trips', ['$resource',
  function ($resource) {
      var service = {
          trips: $resource("/api/trips", {},
          {
              'query': { url: '/api/trips', method: 'GET', isArray: true },
              'save': { url: '/api/trips', method: 'POST' },
              'delete': { url: '/api/trips/:tripId', method: 'DELETE' },
              'get': { url: '/api/trips/:tripId', method: 'GET' },
              'update': { url: '/api/trips/:tripId', method: 'PUT' }
          })
      };

      return service;
  }]);
