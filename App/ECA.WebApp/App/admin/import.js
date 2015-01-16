define(function (require) {

    var $ = require('jquery');
    var ko = require('knockout');
    var app = require('durandal/app');
    var ajax = require('pdip/ajax');
    var globals = require('pdip/globals');

    function ViewModel() {
        var self = this;

        self.activate = function () {
            self.view = 'import';
            self.user = globals.user;
            return true;
        };

        self.submitOptions = {
            method: 'POST',
            dataType: 'text',
            beforeSend: function () {
                self.uploadSuccess('');
                self.uploadError('');
            },
            complete: function (response) {
                $('#wrap').unmask();
                var text = response.responseText;
                if (text.indexOf('ERROR: ') == 0) {
                    self.uploadError(text);
                } else {
                    self.uploadSuccess(text);
                }
            },
            error: function () {
                $('#wrap').unmask();
                self.uploadError("Error uploading file.");
            }
        };

        self.uploadSuccess = ko.observable();
        self.uploadError = ko.observable();

        self.doImport = function () {
            $('#wrap').mask('Importing Plans...');
            self.submitOptions.url = '../api/import?dryRun=false';
            $("#pdip-import-form").ajaxSubmit(self.submitOptions);
        };

        self.doValidate = function () {
            $('#wrap').mask('Validating Plans...');
            self.submitOptions.url = '../api/import?dryRun=true';
            $("#pdip-import-form").ajaxSubmit(self.submitOptions);
        };
    }

    return new ViewModel();
});
