define(['knockout', 'durandal/app', 'pdtracker/auth', 'pdtracker/globals'], function (ko, app, auth, globals) {

    function apiUrl(resource) {
        if (globals.user && globals.user.isAnonymous) {
            return globals.apiHost + 'proxy/api/' + resource;
        } else {
            return globals.apiHost +'api/' + resource;
        }
    }

    function ajax(options) {
        if (globals.user.isAnonymous) {
            return ajaxHelper(options);
        } else {
            return auth.getAccessToken().then(function (accessToken) {
                if (!options.headers) {
                    options.headers = {};
                }
                options.headers.Authorization = 'Bearer ' + accessToken;
                return ajaxHelper(options);
            })
        }

    }

    function ajaxHelper(options) {
        $('#wrap').mask('Processing request...');
        return $.ajax(options).then(null, ajaxErrorHandler).always(function () {
            $('#wrap').unmask();
        });;
    }

    function ajaxErrorHandler(xhr) {
        var error
        if (xhr.status == 0) {
            error = 'There appears to be a problem with your network connection.\n\nPlease check your network connection, save any data, and then refresh this page.';
        } else {
            var msg = [];
            msg.push(xhr.status);
            msg.push(' (');
            msg.push(xhr.statusText);
            msg.push(')');
            if (xhr.responseJSON && xhr.responseJSON.message) {
                msg.push('\n\n');
                msg.push(xhr.responseJSON.message);
            }
            error = msg.join('');
        }
        alert(error);
    }

    return {

        apiUrl: apiUrl,

        get: function (resource, query) {
            return ajax({
                type: 'GET',
                url: apiUrl(resource),
                data: query,
                dataType: 'json'
            });
        },

        getText: function (resource, query) {
            return ajax({
                type: 'GET',
                url: apiUrl(resource),
                data: query,
                dataType: 'text'
            });
        },

        getHtml: function (resource, query) {
            return ajax({
                type: 'GET',
                url: apiUrl(resource),
                data: query,
                dataType: 'html'
            });
        },

        post: function (resource, data, accept) {
            accept = accept || 'json';
            return ajax({
                url: apiUrl(resource),
                data: ko.toJSON(data),
                type: 'POST',
                contentType: 'application/json',
                dataType: accept
            });
        },

        put: function (resource, data) {
            if (typeof (data) !== 'string') {
                data = ko.toJSON(data);
            }
            return ajax({
                url: apiUrl(resource),
                data: data,
                type: 'PUT',
                contentType: 'application/json',
                dataType: 'json'
            });
        },

        del: function (resource) {
            return ajax({
                url: apiUrl(resource),
                type: 'DELETE',
                dataType: 'json'
            });
        }
    };
});
