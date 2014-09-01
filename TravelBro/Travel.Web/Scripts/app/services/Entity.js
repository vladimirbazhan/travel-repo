define(['./module'], function (services) {
    'use strict';
    services.factory('Entity', [function () {
        var service = {
            trip: {
                Default: {
                    IsPrivate: false,
                    DateFrom: new Date(),
                    DateTo: new Date()
                }
            },
            visit: {
                Default: {
                    Start: new Date(),
                    Finish: new Date(),
                    ActivityOrder: 0,
                    Cost: 0
                }
            },
            route: {
                Default: {
                    Start: new Date(),
                    Finish: new Date(),
                    ActivityOrder: 0,
                    Cost: 0
                }
            }
        };

        return service;
    }]);
});