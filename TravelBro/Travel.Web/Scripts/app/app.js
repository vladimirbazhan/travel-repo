'use strict';

/* App Module */

define([
    'angular', 
    'angular-route',
    'angular-resource',
    'controllers/index',
    'services/index'
], function (ng) {
    'use strict';
    
    var app = ng.module('app', [
        'app.controllers', 
        'app.services', 
        'ngRoute',
        'ngResource'
    ]);
    
    app.run(['$location', '$window', 'Auth', function ($location, $window, Auth) {
        var tokenData = $window.localStorage.getItem('tokenData');
        var isTokenDataValid = tokenData && tokenData.length && tokenData !== 'null';
        if (isTokenDataValid) {
            Auth.token.set(tokenData);
        }
    }]);
    
    return app;
});