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
            var $q = $injector.get('$q');
            var notificationService = $injector.get('NotificationService');
            if (response.status === 403) {               
                notificationService.showErrorMessage('You are not authorized to view this resource.');
            }
            else if (response.status === 500) {
                notificationService.showErrorMessage('A system error has occurred, we apologize for the inconvience.');
            }
            return $q.reject(response);
        };
        return service;
    })
