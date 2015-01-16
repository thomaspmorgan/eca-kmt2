define(function (require) {

    var ko = require('knockout');
    var ajax = require('pdtracker/ajax');
    var globals = require('pdtracker/globals');

    return {
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
