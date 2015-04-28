﻿'use strict';

//http://onehungrymind.com/winning-http-interceptors-angularjs/
//http://stackoverflow.com/questions/20230691/injecting-state-ui-router-into-http-interceptor-causes-circular-dependency
// register the interceptor as a service
angular.module('staticApp')
    .factory('ErrorInterceptor', function ($injector) {
        var service = this;
        service.request = function (config) {
            return config;
        };
        service.responseError = function (response) {
            var $q = $injector.get('$q');
            var $state = $injector.get('$state');
            var AuthService = $injector.get('AuthService');
            var notificationService = $injector.get('NotificationService');
            if (response.status === 401) {
                AuthService.login();
            }
            else if (response.status === 403) {               
                //$state.go('forbidden');
            }
            
            else if (response.status === 500) {
                $state.go('error');
            }
            return $q.reject(response);
        };
        return service;
    })
