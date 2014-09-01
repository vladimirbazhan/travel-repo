define(['./module'], function (services) {
    'use strict';
    services.factory('Backend', ['$resource', 'Auth', function ($resource, Auth) {
        var authHeaders = {
            transformRequest: function (data, headersGetter) {
                if (Auth.token.isSet()) {
                    var headers = headersGetter();
                    headers['Authorization'] = Auth.token.get();
                    return JSON.stringify(data);
                }
            }
        };

        var dateTransform = {
            transformResponse: function(data, headersGetter) {
                var dataObj = JSON.parse(data);
                var parseDate = function(str) {
                    return new Date(Date.parse(str));
                }
                var fTransformObj = function(obj) {
                    if (obj.hasOwnProperty('DateFrom')) {
                        obj.DateFrom = parseDate(obj.DateFrom);
                    }
                    if (obj.hasOwnProperty('DateTo')) {
                        obj.DateTo = parseDate(obj.DateTo);
                    }
                    return obj;
                };
                if (Object.prototype.toString.call(dataObj) === '[object Array]') {
                    dataObj.forEach(fTransformObj);
                } else if (Object.prototype.toString.call(dataObj) === '[object Object]') {
                    dataObj = fTransformObj(dataObj);
                }
                return dataObj;
            }
        };
        
        var service = {
            trips: $resource("/api/trips", {},
            {
                'query': angular.extend({ url: '/api/trips', method: 'GET', isArray: true }, authHeaders, dateTransform),
                'save': angular.extend({ url: '/api/trips', method: 'POST' }, authHeaders),
                'delete': angular.extend({ url: '/api/trips/:tripId', method: 'DELETE' }, authHeaders),
                'get': angular.extend({ url: '/api/trips/:tripId', method: 'GET' }, authHeaders, dateTransform),
                'update': angular.extend({ url: '/api/trips/:tripId', method: 'PUT' }, authHeaders)
            }),
            places: $resource("/api/places", {},
            {
                'query': { url: '/api/places', method: 'GET', isArray: true }
            }),
            visits: $resource('/api/visits', {},
            {
                'save': { url: '/api/visits', method: 'POST' }
            }),
            routes: $resource('/api/routes', {},
            {
                'save': { url: '/api/routes', method: 'POST' }
            })
        };

        return service;
    }]);
});