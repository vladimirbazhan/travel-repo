'use strict';

/* App Module */

define([
    'angular', 
    'angular-route',
    'angular-resource',
    'bootstrap',
    'jquery',
    'jquery-ui',
    'jquery.ui.datepicker',
    'angular-ui-date',
    'angular-bootstrap',
    'controllers/index',
    'services/index',
    'directives/index',
    'lightbox-js'
], function (ng) {
    'use strict';
    
    var app = ng.module('app', [
        'app.controllers', 
        'app.services',
        'app.directives',
        'ngRoute',
        'ngResource',
        'ui.date',
        'ui.bootstrap'
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