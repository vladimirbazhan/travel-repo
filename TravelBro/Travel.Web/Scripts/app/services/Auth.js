define(['./module'], function (services) {
    'use strict';
    services.factory('Auth', ['$http', '$window', function ($http, $window) {
        var tokenVal = null;
        var token = {
            set: function (tokenData) {
                if (tokenData) {
                    tokenVal = tokenData.token_type + ' ' + tokenData.access_token;
                } else {
                    tokenVal = null;
                }
            },
            isSet: function () {
                return !!tokenVal;
            },
            get: function () {
                return tokenVal;
            }
        }

        var authService = {
            token: token,
            signIn: function (mail, password, onsuccess, onerror) {
                $http.post('/Token', "grant_type=password&username=" + mail + "&password=" + password).success(function (res) {
                    token.set(res);
                    $window.localStorage.setItem('tokenData', JSON.stringify(res));
                    if (onsuccess) {
                        onsuccess();
                    }
                }).error(onerror);
            },
            signUp: function (mail, password, confirm, onsuccess, onerror) {
                var data = {
                    Email: mail,
                    Password: password,
                    ConfirmPassword: confirm
                };
                $http.post('/api/Account/Register', data).success(onsuccess).error(onerror);
            },
            logOut: function() {
                $window.localStorage.removeItem('tokenData');
                token.set(null);
            },
            getUserName: function () {
                var tokenData = $window.localStorage.getItem('tokenData');
                var isTokenDataValid = tokenData && tokenData.length && tokenData !== 'null';
                if (isTokenDataValid) {
                    return JSON.parse(tokenData).userName;
                }
            }
        };
        return authService;
    }]);
});