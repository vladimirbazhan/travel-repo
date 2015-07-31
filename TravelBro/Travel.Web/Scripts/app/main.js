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
        'jquery-ui': 'bower_components/jquery-ui/ui/jquery-ui',
        'jquery.ui.datepicker': 'bower_components/jquery-ui/ui/jquery.ui.datepicker',
        'angular-ui-date': 'bower_components/angular-ui-date/src/date',
        'angular-bootstrap': 'bower_components/angular-bootstrap/ui-bootstrap-tpls',
        'lightbox-js': 'bower_components/lightbox2-master/dist/js/lightbox'
    },

    // angular does not support AMD out of the box, put it in a shim
    shim: {
        'angular': {
            exports: 'angular',
            deps: ['jquery']
        },
        'angular-route': {
            deps: ['angular']
        },
        'angular-resource': {
            deps: ['angular']
        },
        'jquery.ui.datepicker': {
            deps: ['jquery-ui']
        },
        'angular-ui-date': {
            deps: ['angular', 'jquery.ui.datepicker']
        },
        'angular-bootstrap': {
            deps: ['angular']
        },
        'bootstrap': {
            deps: ['jquery']
        },
        'jquery-ui': {
            deps: ['jquery']
        },
        'lightbox-js': {
            deps: ['jquery']
        }
    },

    // kick start application, load common utils
    deps: ['./boot', './utils/standartTypeExtensions']
});