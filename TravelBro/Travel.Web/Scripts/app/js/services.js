'use strict';

/* Services */

var travelbroServices = angular.module('travelbroServices', ['ngResource']);

travelbroServices.factory('Trips', ['$resource', 'Auth',
  function ($resource, Auth) {

      var authHeaders = {
          transformRequest: function (data, headersGetter) {
              if (Auth.token.isSet()) {
                  var headers = headersGetter();
                  headers['Authorization'] = Auth.token.get();
                  return JSON.stringify(data);
              }
          }
      };

      var service = {
          trips: $resource("/api/trips", {},
          {
              'query': angular.extend({ url: '/api/trips', method: 'GET', isArray: true }, authHeaders),
              'values': angular.extend({ url: '/api/values', method: 'GET', isArray: true }, authHeaders),
              'save': angular.extend({ url: '/api/trips', method: 'POST' }, authHeaders),
              'delete': angular.extend({ url: '/api/trips/:tripId', method: 'DELETE' }, authHeaders),
              'get': angular.extend({ url: '/api/trips/:tripId', method: 'GET' }, authHeaders),
              'update': angular.extend({ url: '/api/trips/:tripId', method: 'PUT' }, authHeaders)
          })
      };

      return service;
  }]);

travelbroServices.factory('Auth', ['$http', '$window', function ($http, $window) {
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
        userName: function () {
            var tokenData = $window.localStorage.getItem('tokenData');
            var isTokenDataValid = tokenData && tokenData.length && tokenData !== 'null';
            if (isTokenDataValid) {
                return JSON.parse(tokenData).userName;
            }
        }()
    };
    return authService;
}]);