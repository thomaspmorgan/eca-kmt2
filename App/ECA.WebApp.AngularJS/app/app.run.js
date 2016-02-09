'use strict';

angular.module('staticApp')
    .run(function ($rootScope, $location, $state, $modal, $anchorScroll, LogoutEventService, ConstantsService, RegisterUserEventService, NotificationService, Idle, AuthService, AppSettingsService) {

          console.assert(NotificationService, "The NotificationService is needed so that we can display notifications for user registration.");
          console.assert(RegisterUserEventService, "The RegisterUserEventService is needed so that we can register on rootscope the handler to automatically register the user.");
          $rootScope.rootStates = [
            { name: 'Home', state: 'home.shortcuts' },
            { name: 'Offices', state: 'alloffices' },
            { name: 'Programs', state: 'allprograms' },
            { name: 'People', state: 'allpeople' },
            { name: 'Organizations', state: 'allorganizations' },
            { name: 'Activities', state: 'activities' },
            { name: 'Reports', state: 'reports.archive' },
            { name: 'Partners', state: 'home.notifications' }
          ];

          var leftOpen = false;
          $rootScope.pushMenu = function ($event) {
              var self = $event.target;
              self.classList.toggle('active');
              document.body.classList.toggle('cbp-spmenu-push-toright');
              document.getElementById('cbp-spmenu-s1').classList.toggle('cbp-spmenu-open');
              leftOpen = !leftOpen;
              toggleToolbar();
          };

          var rightOpen = false;
          $rootScope.pushMenu2 = function ($event) {
              var self = $event.target;
              self.classList.toggle('active');
              document.body.classList.toggle('cbp-spmenu-push-toleft');
              document.getElementById('cbp-spmenu-s2').classList.toggle('cbp-spmenu-open');
              rightOpen = !rightOpen;
              toggleToolbar();
          };

          function toggleToolbar() {
              var toolbar = document.getElementsByClassName('toolbar')[0];
              if (toolbar && toolbar.style.position === "fixed") {
                  if (leftOpen && toolbar.style.left === "0px") {
                      toolbar.style.left = "240px";
                  } else if (rightOpen && toolbar.style.left === "0px") {
                      toolbar.style.left = "-240px";
                  } else {
                      toolbar.style.left = "0px";
                  }
              }
          };

          $rootScope.constants = ConstantsService;

          $rootScope.currentUser = {};
          $rootScope.currentUser.userMenuToggled = function (open) { };
          $rootScope.currentUser.logout = function () {
              $rootScope.currentUser.isBusy = true;
              $rootScope.$broadcast(ConstantsService.logoutEventName);
          };
          $rootScope.$on(ConstantsService.registeringUserEventName, function () {
              $rootScope.currentUser.isBusy = true;
          });
          $rootScope.$on(ConstantsService.registerUserFailureEventName, function () {
              $rootScope.currentUser.isBusy = false;
          });
          $rootScope.$on(ConstantsService.registerUserSuccessEventName, function () {
              $rootScope.currentUser.isBusy = false;
          });

          $rootScope.$on('$routeChangeSuccess', function () {
              $location.hash('top');
              $anchorScroll();
          });

          $rootScope.$on('$routeChangeError', function (event, current, previous, eventObj) {
          });

          $rootScope.$on('$stateChangeSuccess', function () {
              $location.hash('top');
              closeMenus();
              // $anchorScroll();
          });

          function closeMenus() {
              if (leftOpen) {
                  document.body.classList.toggle('cbp-spmenu-push-toright');
                  document.getElementById('cbp-spmenu-s1').classList.toggle('cbp-spmenu-open');
                  leftOpen = !leftOpen;

              }
              if (rightOpen) {
                  document.body.classList.toggle('cbp-spmenu-push-toleft');
                  document.getElementById('cbp-spmenu-s2').classList.toggle('cbp-spmenu-open');
                  rightOpen = !rightOpen;
              }
          }

          $rootScope.$on('$stateChangeError', function (event, toState, toParams, fromState, fromParams, error) {
              // Prevent the transition from happening
              event.preventDefault();
          });

          AppSettingsService.get()
         .then(function (response) {
             Idle.setIdle(parseInt(response.data.idleDuration));
             Idle.setTimeout(parseInt(response.data.idleTimeout));
         }, function () {
             console.log('Unable to load app settings.');
         });

          Idle.watch();

          $rootScope.$on('IdleStart', function () { /* Display modal warning */
              var modalInstance = $modal.open({
                  controller: 'LogoutCtrl',
                  templateUrl: '/app/auth/logout-warning.html',
                  windowClass: 'full-screen-modal',
                  backdrop: 'static'
              });
          });
          $rootScope.$on('IdleTimeout', function () { /* Logout user */
              AuthService.logOut();
          });
      });