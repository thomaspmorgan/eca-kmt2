'use strict';

//http://onehungrymind.com/winning-http-interceptors-angularjs/
//http://stackoverflow.com/questions/20230691/injecting-state-ui-router-into-http-interceptor-causes-circular-dependency
// register the interceptor as a service
angular.module('staticApp')
    .factory('UnauthorizedInterceptor', function ($rootScope, $injector) {
        var service = this;

        service.request = function (config) {
            return config;
        };

        service.responseError = function (response) {
            if (response.status === 401) {
                var AuthService = $injector.get('AuthService');
                var $state = $injector.get('$state');
                var $q = $injector.get('$q');
                $q.when(AuthService.getUserInfo())
                .then(function (response) {
                    var data = response.data;
                    if (!data.isRegistered) {
                        $state.go('register');
                        //$q.when(AuthService.register()).
                        //then(function () {
                        //    debugger;
                            
                        //}, function () {
                        //    console.log('Unable to register user.');
                        //});
                    }
                    else {
                        console.log('user is registered, user does not have permission.');
                    }
                }, function (errorResponse) {
                    console.log('unable to get user info.');
                });
            }
            return response;
        };
        return service;
    })
