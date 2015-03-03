define(['./module'], function (controllers) {
    'use strict';
    controllers.controller('AlertsCtrl', AlertsCtrl);

    AlertsCtrl.$inject = ['Alerts'];

    function AlertsCtrl(Alerts) {
        var vm = {
            close: function(id) {
                 Alerts.remove(id);
            }
        }
        $.extend(this, vm);
    }
});