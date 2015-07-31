define(['./module'], function (controllers) {
    'use strict';
    controllers.controller('SettingsCtrl', SettingsCtrl);

    SettingsCtrl.$inject = ['Backend', 'Alerts'];

    function SettingsCtrl(Backend, Alerts) {
        var vm = this;
        var userInfoString = null;

        init();

        vm.saveName = function () {
            Backend.userInfo.update({}, vm.userInfo, function (userInfo) {
                init();
                Alerts.add('info', 'Changes saved');
            }, function (err) {
                Alerts.add('danger', JSON.stringify(err));
            });
        }

        vm.cancel = function() {
            vm.userInfo = JSON.parse(userInfoString);
        }

        function init() {
            vm.userInfo = Backend.userInfo.get({}, function (userInfo) {
                userInfoString = JSON.stringify(userInfo);
                vm.fullName =
                    (vm.userInfo.Name ? (vm.userInfo.Name + ' ') : '') +
                    (vm.userInfo.Patronymic ? (vm.userInfo.Patronymic + ' ') : '') +
                    (vm.userInfo.Surname || '');
            }, function (err) {
                Alerts.add('danger', JSON.stringify(err));
            });
        }
    };
});