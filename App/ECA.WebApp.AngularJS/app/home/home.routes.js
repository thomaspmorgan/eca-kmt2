'use strict';

angular.module('staticApp')
  .config(function ($stateProvider) {
      $stateProvider
        .state('home', {
            templateUrl: 'app/home/home.html',
            controller: 'HomeCtrl',
            requireADLogin: false
        })
        .state('home.shortcuts', {
            url: '/',
            templateUrl: 'app/home/shortcuts.html',
            requireADLogin: false
        })
        .state('home.notifications', {
            url: '/',
            templateUrl: 'app/home/notifications.html',
            requireADLogin: true
        })
        .state('home.news', {
            url: '/',
            templateUrl: 'app/home/news.html',
            requireADLogin: true
        });
  });