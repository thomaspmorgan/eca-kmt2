﻿//(function () {

//    // Enter Global Config Values & Instantiate ADAL AuthenticationContext
//    window.config = {
//        instance: 'https://login.microsoftonline.com/',
//        tenant: '[Enter your tenant here, e.g. contoso.onmicrosoft.com]',
//        clientId: '[Enter your client_id here, e.g. g075edef-0efa-453b-997b-de1337c29185]',
//        postLogoutRedirectUri: window.location.origin,
//        cacheLocation: 'localStorage', // enable this for IE, as sessionStorage does not work for localhost.
//    };
//    var authContext = new AuthenticationContext(config);

//    // Get UI jQuery Objects
//    var $panel = $(".panel-body");
//    var $userDisplay = $(".app-user");
//    var $signInButton = $(".app-login");
//    var $signOutButton = $(".app-logout");
//    var $errorMessage = $(".app-error");

//    // Check For & Handle Redirect From AAD After Login
//    var isCallback = authContext.isCallback(window.location.hash);
//    authContext.handleWindowCallback();
//    $errorMessage.html(authContext.getLoginError());

//    if (isCallback && !authContext.getLoginError()) {
//        window.location = authContext._getItem(authContext.CONSTANTS.STORAGE.LOGIN_REQUEST);
//    }

//    // Check Login Status, Update UI
//    var user = authContext.getCachedUser();
//    if (user) {
//        $userDisplay.html(user.userName);
//        $userDisplay.show();
//        $signInButton.hide();
//        $signOutButton.show();
//    } else {
//        $userDisplay.empty();
//        $userDisplay.hide();
//        $signInButton.show();
//        $signOutButton.hide();
//    }

//    // Handle Navigation Directly to View
//    window.onhashchange = function () {
//        loadView(stripHash(window.location.hash));
//    };
//    window.onload = function () {
//        $(window).trigger("hashchange");
//    };

//    // Register NavBar Click Handlers
//    $signOutButton.click(function () {
//        authContext.logOut();
//    });
//    $signInButton.click(function () {
//        authContext.login();
//    });

//    // Route View Requests To Appropriate Controller
//    function loadCtrl(view) {
//        switch (view.toLowerCase()) {
//            case 'home':
//                return homeCtrl;
//            case 'todolist':
//                return todoListCtrl;
//            case 'userdata':
//                return userDataCtrl;
//        }
//    }

//    // Show a View
//    function loadView(view) {

//        $errorMessage.empty();
//        var ctrl = loadCtrl(view);

//        if (!ctrl)
//            return;

//        // Check if View Requires Authentication
//        if (ctrl.requireADLogin && !authContext.getCachedUser()) {
//            authContext.config.redirectUri = window.location.href;
//            authContext.login();
//            return;
//        }

//        // Load View HTML
//        $.ajax({
//            type: "GET",
//            url: "App/Views/" + view + '.html',
//            dataType: "html",
//        }).done(function (html) {

//            // Show HTML Skeleton (Without Data)
//            var $html = $(html);
//            $html.find(".data-container").empty();
//            $panel.html($html.html());
//            ctrl.postProcess(html);

//        }).fail(function () {
//            $errorMessage.html('Error loading page.');
//        }).always(function () {

//        });
//    };

//    function stripHash(view) {
//        return view.substr(view.indexOf('#') + 1);
//    }

//}());


//(function () {
//    console.log('here');
//    $(function () {
//        var basicAuthUI =
//            '<div class="input"><input placeholder="username" id="input_username" name="username" type="text" size="10"></div>' +
//            '<div class="input"><input placeholder="password" id="input_password" name="password" type="password" size="10"></div>';
//        $(basicAuthUI).insertBefore('#api_selector div.input:last-child');
//        $("#input_apiKey").hide();

//        $('#input_username').change(addAuthorization);
//        $('#input_password').change(addAuthorization);
//    });

//    function addAuthorization() {
//        var username = $('#input_username').val();
//        var password = $('#input_password').val();
//        if (username && username.trim() != "" && password && password.trim() != "") {
//            //var basicAuth = new SwaggerClient.PasswordAuthorization('bearer', username, password);
//            window.swaggerUi.api.clientAuthorizations.add("bearer", 'hey');
//            console.log("authorization added: username = " + username + ", password = " + password);
//        }
//    }
//})();

(function () {
    $(function () {       
        $('#input_apiKey').off();
        $('#input_apiKey').attr('placeholder', 'Azure AD Bearer Token Value');
        $('#input_apiKey').on('change', function () {
            var key = this.value;
            if (key && key.trim() !== '') {
                swaggerUi.api.clientAuthorizations.add("key", new SwaggerClient.ApiKeyAuthorization("Authorization", "Bearer " + key, "header"));
                var element = $('.info_title');
                var authElement = $('#auth');
                if (authElement.length > 0) {
                    authElement[0].remove();
                }
                $.ajax({
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader("Authorization", "Bearer " + key);
                    },
                    url: '../../api/auth/user',
                    success: function (result, status, xhr) {
                        var authElement = '<div id="auth"><p><small>Authorized as ' + result.userName + ' with principal id ' + result.ecaUserId + '</small></p></div>';
                        $(authElement).insertAfter(element);
                    },
                    error: function () {
                        var errorElement = '<div id="auth"><p style="color:red;"><small>Unable to authenticate with api and azure ad token.</small></p></div>';
                        $(errorElement).insertAfter(element);
                    }
                });
            }
        });
    });
})();

//http://stevemichelotti.com/customize-authentication-header-in-swaggerui-using-swashbuckle/