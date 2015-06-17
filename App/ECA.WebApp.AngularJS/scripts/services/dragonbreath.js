'use strict';

/**
 * @ngdoc service
 * @name staticApp.DragonBreath
 * @description
 * # DragonBreath
 * Factory in the staticApp.
 */
angular.module('staticApp')
	.constant('API_PREFIX', 'api/')
	.constant('API_ENDPOINT_CLOUDAPP_DEV', 'https://ecaapi-kmt-dev.azurewebsites.net/')
	.constant('API_ENDPOINT_CLOUDAPP_QA', 'https://ecaapi-kmt-qa.azurewebsites.net/')
    .constant('API_ENDPOINT_CLOUDAPP_UAT', 'http://ecaapi.usgovcloudapp.net/')
	.constant('API_ENDPOINT_LOCALHOST', 'http://localhost:5555/')
  .factory('DragonBreath', function ($http, API_ENDPOINT_CLOUDAPP_QA, API_ENDPOINT_CLOUDAPP_DEV, API_ENDPOINT_LOCALHOST, API_ENDPOINT_CLOUDAPP_UAT, API_PREFIX) {

      function DragonPath(args, slicePos) {
          if (location.hostname == 'localhost') {
              this.path = API_ENDPOINT_LOCALHOST + API_PREFIX + Array.prototype.slice.call(args, slicePos).join('/');
          }
          else if (location.hostname.indexOf('qa') != -1) {
              this.path = API_ENDPOINT_CLOUDAPP_QA + API_PREFIX + Array.prototype.slice.call(args, slicePos).join('/');
          }
          else if (location.hostname.indexOf('dev') != -1) {
              this.path = API_ENDPOINT_CLOUDAPP_DEV+ API_PREFIX + Array.prototype.slice.call(args, slicePos).join('/');
          }
          else {
              this.path = API_ENDPOINT_CLOUDAPP_UAT + API_PREFIX + Array.prototype.slice.call(args, slicePos).join('/');
          }

      }

      return {
          get: function (filter) {
              var dPath;
              if (typeof filter === 'string') {
                  dPath = new DragonPath(arguments);
                  return $http.get(dPath.path);
              }
              dPath = new DragonPath(arguments, 1);
              return $http.get(dPath.path, { params: filter });
          },
          save: function (object) {
              var dPath = new DragonPath(arguments, 1);
              return $http.put(dPath.path, object);
          },
          create: function (object) {
              var dPath = new DragonPath(arguments, 1);
              return $http.post(dPath.path, object);
          },
          copy: function (object) {
              var dPath = new DragonPath(arguments, 1);
              return $http.copy(dPath.path, object);
          },
          delete: function (object) {
              var dPath = new DragonPath(arguments, 1);
              return $http.delete(dPath.path, object);
          },
          getUrl: function () {
              return new DragonPath(arguments).path;
          }
      };
  });
