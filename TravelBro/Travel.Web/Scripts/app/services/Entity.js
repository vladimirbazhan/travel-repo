define(['./module'], function (services) {
    'use strict';

    function trip() {
        this.IsPrivate = false;
        this.DateFrom = new Date();
        this.DateTo = new Date();
    }

    function visit() {
        this.Start = new Date();
        this.Finish = new Date();
        this.ActivityOrder = 0;
        this.Cost = 0;
    }

    function route() {
        this.Start = new Date();
        this.Finish = new Date();
        this.ActivityOrder = 0;
        this.Cost = 0;
    }

    function comment() {
    }

    services.factory('Entity', [function () {
        var service = {
            trip: {
                Default: function () {
                    return Object.create(trip.prototype);
                }
            },
            visit: {
                Default: function () {
                    return Object.create(visit.prototype);
                }
            },
            route: {
                Default: function () {
                    return Object.create(route.prototype);
                }
            },
            comment: {
                Default: function() {
                    return Object.create(comment.prototype);
                }
            }
        };

        return service;
    }]);
});