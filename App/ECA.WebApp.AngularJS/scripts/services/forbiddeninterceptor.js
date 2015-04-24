'use strict';

//http://onehungrymind.com/winning-http-interceptors-angularjs/
//http://stackoverflow.com/questions/20230691/injecting-state-ui-router-into-http-interceptor-causes-circular-dependency
// register the interceptor as a service
angular.module('staticApp')
    .factory('ForbiddenInterceptor', function ($injector) {
        var service = this;

        service.request = function (config) {
            return config;
        };

        service.responseError = function (response) {
            var AuthService = $injector.get('AuthService');
            var $q = $injector.get('$q');
            var rScope = $injector.get('$rootScope');
            
            if (response.status === 403) {
                debugger;
                var $state = $injector.get('$state');
                var adalUserInfo = rScope.userInfo;
                console.assert(adalUserInfo, "The adal user info must be defined.");
                if (adalUserInfo.isAuthenticated) {
                    $q.when(AuthService.getUserInfo())
                    .then(function (userInfoResponse) {
                        var data = userInfoResponse.data;
                        if (!data.isRegistered) {
                            return $q.resolve($state.go('register'));
                        }
                        else {
                            console.log('user is registered, user does not have permission.');
                            return $q.reject(response);
                        }
                    }, function (infoErrorResponse) {
                        return $q.reject('Unable to return user info.');
                    });
                }
                else {
                    $q.reject(response);
                }
            }
            return $q.reject(response);
        };
        return service;
    })
