define(['./module'], function (services) {
    'use strict';
    services.factory('Alerts', ['$rootScope', '$interval', function ($rootScope, $interval) {
        $rootScope.alerts = [];
        var counter = 0;

        var removeAlert = function (id) {
            for (var i = 0; i < $rootScope.alerts.length; i++) {
                if (id == $rootScope.alerts[i].id) {
                    $rootScope.alerts.splice(i, 1);
                    break;
                }
            }
        }
        return {
            add: function (type, msg, opts) {
                opts = opts || {};
                opts.type = type;
                opts.msg = msg;
                var alert = angular.extend({
                    dismissable: true,
                    timeout: 3000
                }, opts);
                alert.id = ++counter;
                $rootScope.alerts.push(alert);
                if (alert.timeout >= 0) {
                    $interval(function () {
                        removeAlert(alert.id);
                    }, alert.timeout, 1);
                }
            },
            remove: removeAlert
    }
    }]);
});