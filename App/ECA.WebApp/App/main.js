requirejs.config({
    paths: {
        'pdtracker': '../Common',
        'text': '../Scripts/text',
        'durandal': '../Scripts/durandal',
        'plugins': '../Scripts/durandal/plugins',
        'transitions': '../Scripts/durandal/transitions'
        //, 'kendo': '../Scripts/kendo'
    }
});

define('jquery', function () { return jQuery; });
define('knockout', ko);

define(['durandal/system', 'durandal/app', 'durandal/viewLocator', 'durandal/binder', 'pdtracker/auth','transitions/entrance'],  function (system, app, viewLocator, binder, auth, entrance) {
    //>>excludeStart("build", true);
    system.debug(true);
    //>>excludeEnd("build");

    app.title = 'ECA PD Tracker';

    app.configurePlugins({
        router: true,
        dialog: true,
        widget: true,
        observable: true
    });

    //kendo.ns = "kendo-";

    //binder.binding = function (obj, view) {
    //    kendo.bind(view, obj.viewModel || obj);
    //};


    auth.signIn().then(function () {
        app.start().then(function () {
            app.setRoot(
                'shell/shell',      // root module
                'entrance',               // transition
                'applicationHost'); // application host id
        });
    }, function (url) {
        window.location = url;      // sign in or home page url
    });
});