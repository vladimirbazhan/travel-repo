define(['./module'], function (controllers) {
    'use strict';
    controllers.controller('SettingsCtrl', SettingsCtrl);

    SettingsCtrl.$inject = ['Backend', 'Alerts'];

    function SettingsCtrl(Backend, Alerts) {
        var vm = this;
        var userInfoString = null;
        var emptyPwdFields = {
            OldPassword: "",
            NewPassword: "",
            ConfirmPassword: ""
        };

        init();

        vm.saveName = function (params, callbacks) {
            Backend.userInfo.update({}, vm.userInfo, function (userInfo) {
                init();
                Alerts.add('info', 'Changes saved');
                callbacks.onsuccess();
            }, function (err) {
                Alerts.add('danger', JSON.stringify(err));
                callbacks.onerror();
            });
        }

        vm.changePassword = function(data, callbacks) {
            Backend.account.changePassword(vm.pwd, function () {
                vm.pwd = JSON.parse(JSON.stringify(emptyPwdFields));
                Alerts.add('info', 'Password successfully changed');
                callbacks.onsuccess();
            }, function (err) {
                Alerts.add('danger', JSON.stringify(err));
                callbacks.onerror();
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
            vm.pwd = JSON.parse(JSON.stringify(emptyPwdFields));
        }
    };
});