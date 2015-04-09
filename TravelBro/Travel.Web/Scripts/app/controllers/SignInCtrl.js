define(['./module'], function(controllers) {
    'use strict';
    controllers.controller('SignInCtrl', SignInCtrl);

    SignInCtrl.$inject = ['$location', 'Auth', 'Alerts'];

    function SignInCtrl($location, Auth, Alerts) {
        var vm = {
            signInData: {
                mail: "a@a.a",
                password: "password",
            },
            onOk: onOk
        };
        $.extend(this, vm);

        function onOk() {
            Auth.signIn(vm.signInData.mail, vm.signInData.password, function() {
                $location.path('/trips');
            }, function(res) {
                Alerts.add('danger', res.error_description);
            });
        }
    };
});