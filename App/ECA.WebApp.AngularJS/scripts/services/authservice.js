'use strict';

/**
 * @ngdoc service
 * @name staticApp.authService
 * @description
 * # authService
 * Factory for handling authorization.
 */
angular.module('staticApp')
  .factory('authService', function($http, $q, $window) {
  var userInfo;

  function getUserInfo() {
    return userInfo;
  }
 
  function login(userName, password) {
    var deferred = $q.defer();
 
    $http.post('/auth', {
      userName: userName,
      password: password
    }).then(function(result) {
      console.log(result);
      userInfo = {
        accessToken: result.data.accessToken,
        userName: result.data.userName
      };
      $window.sessionStorage.userInfo = JSON.stringify(userInfo);
      deferred.resolve(userInfo);
    }, function(error) {
      deferred.reject(error);
    });
 
    return deferred.promise;
  }

  // TODO - make this do something server side
  function logout(){
    $window.sessionStorage.userInfo = null;
  }
 
  return {
    login: login,
    getUserInfo: getUserInfo,
    logout: logout
  };
});
