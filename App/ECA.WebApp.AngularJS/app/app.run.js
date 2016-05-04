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
            { name: 'Funding Sources', state: 'allfunding'},
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

          var loginSuccess = false;

          $rootScope.$on('$locationChangeStart', function (event) {
              if ($location.path() === "") {
                  event.preventDefault();
                  $location.path('/');
              } else if (loginSuccess && $location.path() === '/') {
                  var savedPath = localStorage.getItem('savedPath');
                  localStorage.removeItem('savedPath');
                  loginSuccess = false;
                  event.preventDefault();
                  $location.path(savedPath || '/');
              }
          });

          $rootScope.$on('$stateChangeStart', function(event, toState, toParams){ 
              var authenticated = AuthService.isAuthenticated();
              if(!authenticated && toState.name !== 'consent') {
                  // Need to remove the hash
                  var savedPath = $state.href(toState.name, toParams).slice(1);
                  localStorage.setItem('savedPath', savedPath);
                  event.preventDefault();
                  $state.go('consent');
              }
          });

          $rootScope.$on("adal:loginSuccess", function () {
              loginSuccess = true;
              AppSettingsService.get()
                 .then(function (response) {
                     Idle.setIdle(parseInt(response.data.idleDurationInSeconds));
                     Idle.setTimeout(parseInt(response.data.idleTimeoutInSeconds));
                     Idle.watch();
                 }, function (error) {
                     console.log(error);
                 });
          });

          $rootScope.$on('$stateChangeSuccess', function () {
              closeMenus();
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

          var modalInstance;

          $rootScope.$on('IdleStart', function () {
              var modalInstance = $modal.open({
                  controller: 'LogoutCtrl',
                  templateUrl: '/app/auth/logout-warning.html',
                  backdrop: 'static',
                  size: 'lg'
              });
          });
          $rootScope.$on('IdleTimeout', function () {
              AuthService.logOut();
          });
      });