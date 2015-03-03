define(['./module'], function (controllers) {
    'use strict';
    controllers.controller('HeaderCtrl', HeaderCtrl);

    HeaderCtrl.$inject = ['$route', '$location', 'Auth'];

    function HeaderCtrl($route, $location, Auth) {
        var vm = {
            signedIn: Auth.token.isSet(),
            userName: Auth.getUserName(),
            headerFilter: '',
            logOut: logOut,
            isActive: function(viewLoc) {
                 return viewLoc == $location.path();
            }
        }
        $.extend(this, vm);

        // private methods
        function logOut() {
            Auth.logOut();
            $route.reload();
        };
    };
});