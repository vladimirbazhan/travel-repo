define(['./module'], function(controllers) {
    'use strict';
    controllers.controller('SignUpCtrl', SignUpCtrl);

    SignUpCtrl.$inject = ['$location', 'Auth', 'Alerts'];

    function SignUpCtrl($location, Auth, Alerts) {
        var vm = {
            formData: {
                mail: '',
                password: '',
                confirmPassword: ''
            },
            register: register
        };
        $.extend(this, vm);

        function register() {
            Auth.signUp(vm.formData.mail, vm.formData.password, vm.formData.confirmPassword, function() {
                Auth.signIn(vm.formData.mail, vm.formData.password, function() {
                    $location.path('/trips');
                }, function(res) {
                    Alerts.add('danger', res.error_description);
                });
            }, showError);
        };

        function showError(err) {
            var str = '';
            if (err.ModelState) {
                var errs = err.ModelState['model.Email'];
                if (errs) {
                    str += 'Email:';
                    errs.forEach(function(curr) {
                        str += ' ' + curr;
                    });
                    str += '\n';
                }
                var errs = err.ModelState['model.Password'];
                if (errs) {
                    str += 'Password:';
                    errs.forEach(function(curr) {
                        str += ' ' + curr;
                    });
                    str += '\n';
                }
                var errs = err.ModelState['model.ConfirmPassword'];
                if (errs) {
                    str += 'ConfirmPassword:';
                    errs.forEach(function(curr) {
                        str += ' ' + curr;
                    });
                    str += '\n';
                }
                var errs = err.ModelState[''];
                var isFirst = true;
                if (errs) {
                    errs.forEach(function(curr) {
                        str += (isFirst ? '' : ' ') + curr;
                        isFirst = false;
                    });
                }
            } else {
                str = err.Message;
            }
            Alerts.add('danger', str);
        }
    };
});