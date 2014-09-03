define(['./module'], function (services) {
    'use strict';

    var resourceWrapper = function (resource, action, handlerSuccess, handlerError) {
        // copy original action
        resource['_' + action] = resource[action];
        // create new action wrapping the original and handling result
        resource[action] = function (data, success, error) {
            return resource['_' + action](data, function (obj) {
                if (handlerSuccess) {
                    handlerSuccess(obj);
                }
                if (success) {
                    success(obj);
                }
            }, function (err) {
                if (handlerError) {
                    handlerError(err);
                }
                if (error) {
                    error(err);
                }
            });
        };
    };

    var wrapActions = function (resource, actions, handlers) {
        // copy original resource
        var wrappedResource = resource;
        for (var i = 0; i < actions.length; i++) {
            resourceWrapper(wrappedResource, actions[i], handlers[i]);
        };
        // return modified copy of resource
        return wrappedResource;
    };

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
                'update': angular.extend({ url: '/api/trips/:tripId', method: 'PUT' }, authHeaders),
                'saveComment': angular.extend({ url: '/api/trips/:tripId/comments', method: 'POST' }, authHeaders)
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

        var handleTrip = function(trip) {
            trip.Id = parseInt(trip.Id);
            trip.Comments = trip.Comments || [];

            var items = [];

            trip.Visits = trip.Visits || [];
            trip.Visits.forEach(function(curr) {
                items.push({ type: "visit", data: curr });
            });

            trip.Routes = trip.Routes || [];
            trip.Routes.forEach(function(curr) {
                items.push({ type: "route", data: curr });
            });

            trip.tripItems = items;
        };

        var successHandlers = {
            trips: {
                query: function (trips) {
                    trips.forEach(handleTrip);
                },
                get: handleTrip
            }
        }
        
        service.trips = wrapActions(service.trips,
            ['query', 'get'],
            [successHandlers.trips.query, successHandlers.trips.get]);

        return service;
    }]);
});