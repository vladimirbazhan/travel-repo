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
                init();
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

                // find when password was changed
                vm.passwordChangedContent = createPasswordChangedContentString(new Date(Date.parse(userInfo.PasswordChangedUtc)));
            }, function (err) {
                Alerts.add('danger', JSON.stringify(err));
            });
            vm.pwd = JSON.parse(JSON.stringify(emptyPwdFields));
        }

        function createPasswordChangedContentString(passChangedUtc) {
            var compileString = function(unitName, unitCount) {
                return 'Changed ' + unitCount + ' ' + unitName + (unitCount == 1 ? '' : 's') + ' ago';
            }

            var diff = new Date().getTime() - passChangedUtc.getTime();

            var years = Math.floor(diff / (1000 * 60 * 60 * 24 * 365));
            if (years > 0) {
                return compileString('year', years);
            }
            
            var days = Math.floor(diff / (1000 * 60 * 60 * 24));
            if (days > 0) {
                return compileString('day', days);
            }

            var hours = Math.floor(diff / (1000 * 60 * 60));
            if (hours > 0) {
                return compileString('hour', hours);
            }

            var mins = Math.floor(diff / (1000 * 60));
            return compileString('minute', mins);
        }
    };
});