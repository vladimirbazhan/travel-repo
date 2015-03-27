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
        this.GPlaceId = '';
    }

    function route() {
        this.Start = new Date();
        this.Finish = new Date();
        this.ActivityOrder = 0;
        this.TransType = new transType();
        this.Cost = 0;
    }

    function comment() {
    }

    function transType() {
        this.Id = 0;
        this.Name = 'NotSpecified';
    }

    services.factory('Entity', [function () {
        var service = {
            trip: {
                Default: function () {
                    return new trip();
                }
            },
            visit: {
                Default: function () {
                    return new visit();
                }
            },
            route: {
                Default: function () {
                    return new route();
                }
            },
            comment: {
                Default: function() {
                    return new comment();
                }
            },
            transType: {
                Default: function() {
                    return new transType();
                }
            }
        };

        return service;
    }]);
});