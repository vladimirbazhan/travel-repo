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

    function getVisitCustomActions(Auth) {
        return {
            savePhoto: function (params, photo, handlers) {
                var url = '/api/trips/' + params.tripId + '/visit/' + params.visitId + '/photos';
                return ajaxSavePhoto(url, Auth.token.get(), photo, handlers);
            }
        }
    }

    function getRouteCustomActions(Auth) {
        return {
            savePhoto: function (params, photo, handlers) {
                var url = '/api/trips/' + params.tripId + '/route/' + params.routeId + '/photos';
                return ajaxSavePhoto(url, Auth.token.get(), photo, handlers);
            }
        }
    }

    function getTripCustomActions(Auth) {
        return {
            savePhoto: function (params, photo, handlers) {
                var url = '/api/trips/' + params.tripId + '/photos';
                return ajaxSavePhoto(url, Auth.token.get(), photo, handlers);
            }
        };
    }

    function ajaxSavePhoto(url, authToken, photo, handlers) {
        var form = new FormData();
        form.append('photo[]', photo);
        $.ajax({
            url: url,
            data: form,
            cache: false,
            contentType: false,
            processData: false,
            type: 'POST',
            headers: { 'Authorization': authToken },
            xhr: function () {
                var myXhr = $.ajaxSettings.xhr();
                if (myXhr.upload) {
                    myXhr.upload.addEventListener('progress', handlers.onprogress, false);
                }
                return myXhr;
            }
        }).done(handlers.ondone).fail(handlers.onfail);
    }

    services.factory('Backend', ['$resource', 'Auth', 'Entity', function ($resource, Auth, Entity) {
        var authHeaders = {
            transformRequest: function (data, headersGetter) {
                if (Auth.token.isSet()) {
                    var headers = headersGetter();
                    headers['Authorization'] = Auth.token.get();
                    return JSON.stringify(data);
                }
            }
        };

        var service = {
            trips: $resource("/api/trips", {},
            {
                'query': angular.extend({ url: '/api/trips', method: 'GET', isArray: true }, authHeaders),
                'save': angular.extend({ url: '/api/trips', method: 'POST' }, authHeaders),
                'delete': angular.extend({ url: '/api/trips/:tripId', method: 'DELETE' }, authHeaders),
                'get': angular.extend({ url: '/api/trips/:tripId', method: 'GET' }, authHeaders),
                'update': angular.extend({ url: '/api/trips/:tripId', method: 'PUT' }, authHeaders),
                'saveComment': angular.extend({ url: '/api/trips/:tripId/comments', method: 'POST' }, authHeaders)
            }),
            visits: $resource('/api/visits', {},
            {
                'get': angular.extend({ url: '/api/visits/:visitId', method: 'GET' }, authHeaders),
                'save': angular.extend({ url: '/api/visits', method: 'POST' }, authHeaders),
                'update': angular.extend({ url: '/api/visits/:visitId', method: 'PUT' }, authHeaders),
                'delete': angular.extend({ url: '/api/visits/:visitId', method: 'DELETE' }, authHeaders),
            }),
            routes: $resource('/api/routes', {},
            {
                'get': angular.extend({ url: '/api/routes/:routeId', method: 'GET' }, authHeaders),
                'save': angular.extend({ url: '/api/routes', method: 'POST' }, authHeaders),
                'update': angular.extend({ url: '/api/routes/:routeId', method: 'PUT' }, authHeaders),
                'delete': angular.extend({ url: '/api/routes/:routeId', method: 'DELETE' }, authHeaders),
            }),
            transTypes: $resource('/api/transtypes', {},
            {
                'query': angular.extend({ url: '/api/transtypes', method: 'GET', isArray: true }, authHeaders),
            }),
        };

        function parseDate(str) {
            return new Date(Date.parse(str));
        };

        function handleDate(obj, datePropName) {
            if (obj.hasOwnProperty(datePropName) && !!obj[datePropName]) {
                obj[datePropName] = parseDate(obj[datePropName]);
            }
        }

        // sorts visits and routes taking into account Start-Finish date order and relative items order.
        function sortTripItems(items) {
            var itemsWithDate = items.filter(function (item) {
                return item.data.Start && item.data.Finish;
            });
            itemsWithDate.sort(function (a, b) {
                return a.data.Finish - b.data.Start;
            });

            var sortedItems = [];
            itemsWithDate.forEach(function (x) {
                var tempSet = items.filter(function (item) {
                    return item.data.Order <= x.data.Order;
                });
                tempSet.sort(function (a, b) {
                    return a.data.Order > b.data.Order ? 1 : -1;
                });
                sortedItems = sortedItems.concat(tempSet);
                sortedItems.push(x);
            });
            items.sort(function (a, b) {
                return a.data.Order > b.data.Order ? 1 : -1;
            });
            sortedItems = sortedItems.concat(items);

            return sortedItems;
        }
        
        var handleTrip = function(trip) {
            trip.Id = parseInt(trip.Id);
            trip.Comments = trip.Comments || [];

            handleDate(trip, 'DateFrom');
            handleDate(trip, 'DateTo');

            var items = [];

            trip.Visits = trip.Visits || [];
            trip.Visits.forEach(function (curr) {
                handleVisit(curr);
                items.push({ type: "visit", data: curr });
            });

            trip.Routes = trip.Routes || [];
            trip.Routes.forEach(function (curr) {
                handleRoute(curr);
                items.push({ type: "route", data: curr });
            });
            
            trip.tripItems = sortTripItems(items);
            trip.Photos = trip.Photos || [];
        };

        function handleRoute(route) {
            route.TransType = route.TransType || Entity.transType.Default();
            handleTransType(route.TransType);

            handleDate(route, 'Start');
            handleDate(route, 'Finish');
            route.loaded = false;
        }

        function handleVisit(visit) {
            handleDate(visit, 'Start');
            handleDate(visit, 'Finish');
            visit.loaded = false;
        }

        function handleTransType(transType) {
            transType.Name = transType.Name || "";
            transType.Name = transType.Name.replace(/([a-z])([A-Z])/g, '$1 $2');
        }

        var successHandlers = {
            trips: {
                query: function (trips) {
                    trips.forEach(handleTrip);
                },
                get: handleTrip
            },
            routes: {
                get: handleRoute
            },
            visits: {
                get: handleVisit
            },
            transTypes: {
                query: function(transTypes) {
                    transTypes.forEach(handleTransType);
                }
            }
        };
        service.trips = wrapActions(service.trips,
            ['query', 'get'],
            [successHandlers.trips.query, successHandlers.trips.get]);
        service.routes = wrapActions(service.routes,
            ['get'],
            [successHandlers.routes.get]);
        service.visits = wrapActions(service.visits,
            ['get'],
            [successHandlers.visits.get]);

        $.extend(service.trips, getTripCustomActions(Auth));
        $.extend(service.routes, getRouteCustomActions(Auth));
        $.extend(service.visits, getVisitCustomActions(Auth));

        service.transTypes = wrapActions(service.transTypes,
            ['query'],
            [successHandlers.transTypes.query]);

        return service;
    }]);
});