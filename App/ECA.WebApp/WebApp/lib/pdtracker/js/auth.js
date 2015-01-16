define(function (require) {

    var globals = require('pdtracker/globals'),
        accessToken = null,
        expiresOn = null,
        refreshToken = null,
        tenantId = 'statedept.us',
        clientId = 'e0356e55-e124-452c-837d-aeb7504185ff',
        resource = 'https://ecaserver.state.gov'

    //------------------------------------------------------------------------
    // Public Functions
    //------------------------------------------------------------------------

    // Returns a promise that is resolved on success and rejected with a
    // redirect URL on failure.
    function signIn() {
        var promise = $.Deferred(),
            query = getQuery();
        if (query.error) {
            alert(query.error_description);
            promise.reject(getSignInUri());
        } else if (query.code) {
            // This is a redirect from federated sign in. We reject the
            // promise because we will redirect the user to the home page
            // of the and do not want the rest of the application to load.
            getTokenFromServer(query.code).always(function () {
                promise.reject(getRedirectUri());
            });
        } else {
            getCurrentUser().done(function (user) {
                globals.user = user;
                if (user.isAnonymous) {
                    globals.ticket = 'anonymous-user';
                    promise.resolve();
                } else {
                    $.ajax({
                        type: 'GET',
                        url: globals.apiHost + 'api/tickets',
                        headers: {
                            Authorization: 'Bearer ' + accessToken
                        }
                    }).done(function (ticket) {
                        globals.ticket = ticket.value;
                        promise.resolve();
                    }).fail(function () {
                        promise.reject(getSignInUri());
                    });
                }
            }).fail(function () {
                promise.reject(getSignInUri());
            });
        }
        return promise;
    }

    function signOut() {
        getAccessToken().then(function (accessToken) {
            return $.ajax({
                type: 'DELETE',
                url: globals.apiHost + 'api/tickets',
                headers: {
                    Authorization: 'Bearer ' + accessToken
                }
            });
        }).always(function () {
            accessToken = null;
            expiresOn = null;
            refreshToken = null;
            Cookies.expire('access_token');
            Cookies.expire('expires_on');
            Cookies.expire('refresh_token');
            window.location = getSignInUri();
        });
    }

    // Returns a promise that is resolved with the access token.
    function getAccessToken() {
        var promise = $.Deferred();
        if (isSignedIn()) {
            promise.resolve(accessToken);
        } else if (typeof refreshToken === 'string') {
            getNewTokenFromServer().done(function () {
                promise.resolve(accessToken);
            }).fail(function () {
                window.location = getSignInUri();
                promise.reject();
            });
        } else {
            window.location = getSignInUri();
            promise.reject();
        }
        return promise;
    }

    //------------------------------------------------------------------------
    // Private Functions
    //------------------------------------------------------------------------

    // Returns an object having the query string parameters. This is required
    // because the Durandal router will only parse query parameters if a hash
    // is part of the URL. With the Azure Active Directory redirect, the hash
    // is not included (and is ignored if present in the redirect_uri).
    function getQuery() {
        var params = window.location.search.substr(1).split('&'),
            query = {};
        for (var i = 0; n = params.length, i < n; i++) {
            var q = params[i].split('=', 2);
            if (q.length == 2) {
                query[q[0]] = decodeURIComponent(q[1].replace(/\+/g, ' '));
            }
        }
        return query;
    }

    // Returns a promise that is resolved with the current user.
    function getCurrentUser() {
        var promise = $.Deferred(),
            url = globals.apiHost + 'api/auth/user';
        $.getJSON(url).done(function (user) {
            if (user.isAnonymous) {
                promise.resolve(user);
            } else {
                accessToken = Cookies.get('access_token');
                expiresOn = Cookies.get('expires_on');
                refreshToken = Cookies.get('refresh_token');
                if (expiresOn) {
                    expiresOn = parseInt(expiresOn);
                }
                getAccessToken().done(function (accessToken) {
                    $.ajax({
                        type: 'GET',
                        url: url,
                        headers: {
                            Authorization: 'Bearer ' + accessToken
                        }
                    }).done(function (user) {
                        promise.resolve(user);
                    }).fail(function () {
                        promise.reject();
                    });
                }).fail(function () {
                    promise.reject();
                });
            }
        }).fail(function () {
            promise.reject();
        });
        return promise;
    }

    function getRedirectUri() {
        var loc = window.location;
        //loc.port = '5555'; //change port to web app vs webapi app
        return loc.protocol + '//' + loc.hostname + (loc.port ? ':' + loc.port : '') + '/webapp';
    }

    function getSignInUri() {
        var query = {
            client_id: clientId,
            resource: resource,
            redirect_uri: getRedirectUri(),
            login_hint: 'OpenNetId@' + tenantId,
            prompt: 'login',
            response_type: 'code'
        };
        var params = [];
        params.push('client_id=' + clientId);
        params.push('resource=' + resource);
        params.push('redirect_uri=' + getRedirectUri());
        params.push('login_hint=' + tenantId);
        params.push('prompt=login');
        params.push('response_type=code');
        return 'https://login.windows.net/{0}/oauth2/authorize?{1}'.format(tenantId, params.join('&'));
    }

    // Returns true if the access token is not expired. Even if there is no
    // valid access token, a new one can be retrieved using the refresh token.
    function isSignedIn() {
        var now = (typeof Date.now === 'function' ? Date.now() : new Date().getTime()) / 1000;
        return typeof accessToken === 'string' && typeof expiresOn === 'number' && expiresOn > now;
    }

    // Returns a promise that is resolved with the access token.
    function getTokenFromServer(code) {
        var promise = $.Deferred();
        $.ajax({
            type: 'GET',
            url: globals.apiHost + 'api/auth/token',
            data: {
                redirect_uri: getRedirectUri(),
                code: code
            }
        }).then(handleSuccess, handleFailure).done(function () {
            promise.resolve(accessToken);
        }).fail(function () {
            promise.reject();
        });
        return promise;
    }

    // Returns a promise that is resolved with the access token.
    function getNewTokenFromServer() {
        var promise = $.Deferred();
        $.ajax({
            type: 'GET',
            url: globals.apiHost + 'api/auth/refresh',
            data: {
                refresh_token: refreshToken
            }
        }).then(handleSuccess, handleFailure).done(function () {
            promise.resolve(accessToken);
        }).fail(function () {
            promise.reject();
        });
        return promise;
    }

    function handleSuccess(response) {
        var fiveMinutes = 5 * 60;
        var milliseconds = 1000;
        accessToken = response.access_token;
        expiresOn = parseInt(response.expires_on) - fiveMinutes;
        refreshToken = response.refresh_token;
        var expiresDate = new Date(expiresOn * milliseconds);
        Cookies.set('access_token', response.access_token, {
            expires: expiresDate
        });
        Cookies.set('expires_on', expiresOn, {
            expires: expiresDate
        });
        Cookies.set('refresh_token', response.refresh_token, {
            expires: 14 * 86400
        });
    }

    function handleFailure(response) {
        var json = response.responseJSON;
        if (json && json.error_description) {
            alert(json.error_description);
        } else {
            alert('Authentication failed.');
        }
    }

    return {
        signIn: signIn,
        signOut: signOut,
        getAccessToken: getAccessToken
    };
});
