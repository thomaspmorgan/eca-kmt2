define(function (require) {

    var ko = require('knockout');
    var ajax = require('pdtracker/ajax');
    var globals = require('pdtracker/globals');

    return {
        user: globals.user,
        version: '0.0.1',
        activate: null
    };
        //activate: function () {
        //    var self = this;
        //    return ajax.get('sysinfo').done(function (info) {
        ////        self.version(info.version);
        //    })
    ////}
    //};
});
