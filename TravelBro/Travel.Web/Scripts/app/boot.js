define([
    'require',
    'angular',
    'app',
    'routes',
    'bootstrap'
], function(require, ng) {
    'use strict';
   
    require(['domReady'], function() {
        ng.bootstrap(document, ['app']);
    });
});