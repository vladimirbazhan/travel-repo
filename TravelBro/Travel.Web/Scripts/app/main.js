require.config({
    baseUrl: "Scripts/app",

	// alias libraries paths
    paths: {
        'domReady': 'bower_components/requirejs-domready/domReady',
        'angular': 'bower_components/angular/angular',
        'angular-route': 'bower_components/angular-route/angular-route',
        'angular-resource': 'bower_components/angular-resource/angular-resource',
        'bootstrap': 'bower_components/bootstrap/dist/js/bootstrap',
        'jquery': 'bower_components/jquery/jquery',
        'jquery-ui': 'bower_components/jquery-ui/ui/jquery-ui'
    },

    // angular does not support AMD out of the box, put it in a shim
    shim: {
        'angular': {
            exports: 'angular'
        },
        'bootstrap': {
            deps: ['jquery']
        },
        'jquery-ui': {
            deps: ['jquery']
        }
    },

    // kick start application
    deps: ['./boot']
});