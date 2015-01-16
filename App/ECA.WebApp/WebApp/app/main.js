requirejs.config({
    paths: {
        'pdtracker': '../lib/pdtracker/js',
        'text': '../lib/require/js/text',
        'durandal': '../lib/durandal/js',
        'plugins': '../lib/durandal/js/plugins',
        'transitions': '../lib/durandal/js/transitions'
    }
});

define('jquery', function () { return jQuery; });
define('knockout', ko);

define(['durandal/system', 'durandal/app', 'durandal/viewLocator', 'pdtracker/auth'], function (system, app, viewLocator, auth) {
    //>>excludeStart("build", true);
    system.debug(true);
    //>>excludeEnd("build");

    app.title = 'ECA PD Tracker';

    app.configurePlugins({
        router: true,
        dialog: true,
        widget: true
    });

    system.debug(true);

    auth.signIn().then(function () {
        app.start().then(function () {
            app.setRoot(
                'shell/shell',      // root module
                null,               // transition
                'applicationHost'); // application host id
        });
    }, function (url) {
        window.location = url;      // sign in or home page url
    });
});
