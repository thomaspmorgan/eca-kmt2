'use strict';

/**
 * @ngdoc overview
 * @name staticApp
 * @description
 * # staticApp
 *
 * Main module of the application.
 */
angular
  .module('staticApp', [
    'ngCookies',
    'ngResource',
    'ngSanitize',
    'ui.router',
    'ui.bootstrap',
    'ngModal',
    'multi-select',
    'xeditable',
    'sticky'
  ])
  .config(function ($stateProvider, $urlRouterProvider) {
    var authorizer = ['$q', 'authService', function($q, authService) {
            /** var userInfo = authService.getUserInfo();
            if (userInfo) {
              return $q.when(userInfo);
            } else {
              return $q.reject({ authenticated: false });
            } **/
            return true;
          }];
    $urlRouterProvider.otherwise('/');

    $stateProvider
      .state('home', {
        templateUrl: 'views/home.html',
        controller: 'HomeCtrl',
        resolve: {
           auth: authorizer
        }
     })

      .state('home.shortcuts', {
        url: '/',
        templateUrl: 'views/home/shortcuts.html',
        resolve: {
          auth: authorizer
        }
      })
      .state('home.notifications', {
        url: '/',
        templateUrl: 'views/home/notifications.html',
        resolve: {
          auth: authorizer
        }
      })
      .state('home.news', {
        url: '/',
        templateUrl: 'views/home/news.html',
        resolve: {
          auth: authorizer
        }
      })

      .state('events', {
        url: '/events',
        templateUrl: 'views/events/eventList.html',
        resolve: {
           auth: authorizer
        }
      })
      .state('eventsCreate', {
        url: '/events/create',
        templateUrl: 'views/events/eventCreate.html',
        resolve: {
           auth: authorizer
        }
      })
      .state('eventsTag', {
        url: '/events/tag',
        templateUrl: 'views/events/eventTag.html',
        resolve: {
           auth: authorizer
        }
      })
      .state('eventsOverview', {
        url: '/events/overview',
        templateUrl: 'views/events/eventOverview.html',
        resolve: {
           auth: authorizer
        }
      })

      .state('about', {
        url: '/about',
        templateUrl: 'views/about.html',
        controller: 'AboutCtrl',
        resolve: {
           auth: authorizer
        }
      })

      

      // .state('list', {
      //   url: 
      // })

      .state('secureTest', {
        url: '/secureTest',
        templateUrl: 'views/securetest.html',
        controller: 'SecuretestCtrl',
        resolve: {
           auth: authorizer
         }
      })
      .state('login', {
        url: '/login',
        templateUrl: 'views/login.html',
        controller: 'LoginCtrl'
      })
      .state('logout', {
        url: '/logout',
        templateUrl: 'views/logout.html',
        controller: 'LogoutCtrl'
      })
      .state('formValidation', {
        url: '/formValidation',
        templateUrl: 'views/formvalidation.html',
        controller: 'FormvalidationCtrl',
        resolve: {
           auth: authorizer
        }
      })

      .state('reports', {
        url: '/report',
        templateUrl: 'views/reports/archiveList.html',
        controller: 'ReportCtrl',
        resolve: {
           auth: authorizer
        }
      })

      .state('reportsCreate', {
        url: '/report/create',
        templateUrl: 'views/reports/create.html',
        resolve: {
           auth: authorizer
        }
      })
      .state('reportsCreated', {
        url: '/report/created',
        templateUrl: 'views/reports/created.html',
        resolve: {
           auth: authorizer
        }
      })
      .state('reportsAddData', {
        url: '/report/addData',
        templateUrl: 'views/reports/addData.html',
        resolve: {
           auth: authorizer
        }
      })
      .state('reportsSearchResults', {
        url: '/report/searchResult',
        templateUrl: 'views/reports/searchResults.html',
        resolve: {
           auth: authorizer
        }
      })
      .state('reportsRefineSearch', {
        url: '/report/refineSearch',
        templateUrl: 'views/reports/refineSearch.html',
        resolve: {
           auth: authorizer
        }
      })
      .state('reportsRefineSearchDeselect', {
        url: '/report/refineSearchDeselect',
        templateUrl: 'views/reports/refineSearchDeselect.html',
        resolve: {
           auth: authorizer
        }
      })
      .state('reportsSearchResultsClearance', {
        url: '/report/searchResultClearance',
        templateUrl: 'views/reports/searchResultsClearance.html',
        resolve: {
           auth: authorizer
        }
      })
      .state('reportsContent', {
        url: '/report/content',
        templateUrl: 'views/reports/content.html',
        resolve: {
           auth: authorizer
        }
      })
      .state('reportsCrunchingData', {
        url: '/report/crunchingData',
        templateUrl: 'views/reports/crunchingData.html',
        resolve: {
           auth: authorizer
        }
      })
      .state('reportsGenerated', {
        url: '/report/reportGenerated',
        templateUrl: 'views/reports/reportGenerated.html',
        resolve: {
           auth: authorizer
        }
      })
      .state('reportsOverviewOpen', {
        url: '/report/overviewOpen',
        templateUrl: 'views/reports/overviewOpen.html',
        resolve: {
           auth: authorizer
        }
      })
      .state('reportsPreview', {
        url: '/report/preview',
        templateUrl: 'views/reports/preview.html',
        resolve: {
           auth: authorizer
        }
      })
      .state('reportsAddCollab', {
        url: '/report/addCollab',
        templateUrl: 'views/reports/addCollab.html',
        resolve: {
           auth: authorizer
        }
      })
      .state('reportsSubmittingClearance', {
        url: '/report/submittingClearance',
        templateUrl: 'views/reports/submittingClearance.html',
        resolve: {
           auth: authorizer
        }
      })
      .state('reportsSubmitClearance', {
        url: '/report/submitClearance',
        templateUrl: 'views/reports/submitClearance.html',
        resolve: {
           auth: authorizer
        }
      })
      .state('reportsClearing', {
        url: '/report/clearing',
        templateUrl: 'views/reports/clearing.html',
        resolve: {
           auth: authorizer
        }
      })
      .state('reportsPublish', {
        url: '/report/publish',
        templateUrl: 'views/reports/publish.html',
        resolve: {
           auth: authorizer
        }
      })


      .state('reports.archive', {
        url: '/reports/archive',
        templateUrl: 'views/reports/archive.html',
      })


      .state('settings', {
        url: '/settings',
        templateUrl: 'views/settings.html',
        controller: 'SettingsCtrl',
        resolve: {
           auth: authorizer
        }
      })
      .state('people', {
        url: '/people/:personId',
        templateUrl: 'views/people.html',
        controller: 'PeopleCtrl',
        resolve: {
           auth: authorizer
        }
      })
      .state('people.overview', {
        url: '/overview',
        templateUrl: 'views/people/overview.html',
        controller: 'PeopleCtrl',
        resolve: {
          auth: authorizer
        }
      })
      .state('people.activity', {
        url: '/activity',
        templateUrl: 'views/people/activity.html',
        controller: 'PeopleCtrl'
      })
      .state('people.personalinformation', {
        url: '/personalinformation',
        templateUrl: 'views/people/personalinformation.html',
        controller: 'PeopleCtrl'
      })
      .state('people.relatedreports', {
        url: '/relatedreports',
        templateUrl: 'views/people/relatedreports.html',
        controller: 'PeopleCtrl'
      })
      .state('people.impact', {
        url: '/impact',
        templateUrl: 'views/people/impact.html',
        controller: 'PeopleCtrl'
      })
      
      .state('allprograms',{
        url: '/allprograms/:page',
        templateUrl: 'views/program/allprograms.html',
        controller: 'AllProgramsCtrl',
        resolve: {
          auth: authorizer
        }
      })

      .state('programs', {
        url: '/offices/:officeId/programs/:programId',
        templateUrl: 'views/programs.html',
        controller: 'ProgramsCtrl',
        resolve: {
           auth: authorizer
        }
      })
      .state('programs.overview', {
        url: '/overview',
        templateUrl: 'views/program/overview.html'
      })
      .state('programs.projects', {
        url: '/projects',
        templateUrl: 'views/program/projects.html'
      })
      .state('programs.activity', {
        url: '/activity',
        templateUrl: 'views/program/activity.html'
      })
      .state('programs.artifacts', {
        url: '/artifacts',
        templateUrl: 'views/program/artifacts.html'
      })
      .state('programs.impact', {
        url: '/impact',
        templateUrl: 'views/program/impact.html'
      })

      .state('projects', {
        url: '/offices/:officeId/programs/:programId/project/:projectId',
        templateUrl: 'views/project.html',
        controller: 'ProjectCtrl',
        resolve: {
           auth: authorizer
        }
      })
      .state('projects.overview', {
        url: '/overview',
        templateUrl: 'views/project/overview.html'
      })
      .state('projects.participants', {
        url: '/participant',
        templateUrl: 'views/project/participant.html'
      })
      .state('projects.artifacts', {
        url: '/artifact',
        templateUrl: 'views/project/artifact.html'
      })
      .state('projects.activity', {
        url: '/activity',
        templateUrl: 'views/project/activity.html'
      })
      .state('projects.itineraries', {
        url: '/itinerary',
        templateUrl: 'views/project/itinerary.html'
      })
      .state('projects.implementers', {
        url: '/implementers',
        templateUrl: 'views/project/implementer.html'
      })
      .state('projects.partners', {
        url: '/partners',
        templateUrl: 'views/project/partners.html'
      })
      .state('projects.moneyflows', {
        url: '/moneyflows',
        templateUrl: 'views/project/moneyflows.html'
      })
      .state('projects.impact', {
        url: '/impact',
        templateUrl: 'views/project/impact.html'
      })

      .state('offices',{
        url: '/offices/:officeId',
        templateUrl: 'views/offices.html',
        controller: 'OfficeCtrl',
        resolve: {
           auth: authorizer
        }
      })
      .state('offices.overview', {
        url: '/overview',
        templateUrl: 'views/office/overview.html'
      })
      .state('offices.branches', {
        url: '/branches',
        templateUrl: 'views/office/branches.html'
      })
      .state('offices.activity', {
        url: '/activity',
        templateUrl: 'views/office/activity.html'
      })
      .state('offices.artifacts', {
        url: '/artifacts',
        templateUrl: 'views/office/artifacts.html'
      })
      .state('offices.moneyflows', {
        url: '/moneyflows',
        templateUrl: 'views/office/moneyflows.html'
      })

      .state('alloffices',{
        url: '/alloffices/:page',
        templateUrl: 'views/office/alloffices.html',
        controller: 'AllOfficesCtrl',
        resolve: {
           auth: authorizer
        }
      })
      .state('allparticipants', {
        url: '/allparticipants/:page',
        templateUrl: 'views/people/allpeople.html',
        controller: 'AllPeopleCtrl',
        resolve: {
          auth: authorizer
        }
      })

      .state('partner', {
        url: '/partner',
        templateUrl: 'views/partner.html',
        resolve: {
           auth: authorizer
        }
      })

      .state('offshoot', {
        url: '/clearance',
        templateUrl: 'views/offshoot.html',
        resolve: {
           auth: authorizer
        }
      })

      .state('lastupdated', {
        url: '/lastupdated',
        templateUrl: 'views/lastupdated.html',
        resolve: {
           auth: authorizer
        }
      });
  })
  .run(['$rootScope', '$location', '$state', 'editableOptions', function($rootScope, $location, $state, editableOptions, $anchorScroll) {

    editableOptions.theme = 'bs3';
    $rootScope.rootStates = [
      {name: 'Home', state: 'home.shortcuts'},
      {name: 'Programs', state: 'allprograms'},
      {name: 'Participants', state: 'allparticipants({page: 1})'},
      {name: 'Events', state: 'events'},
      {name: 'Reports', state: 'reports'},
      {name: 'Partners', state: 'home.notifications'}
    ];
    
    var leftOpen = false;
    $rootScope.pushMenu = function($event) {
      var self = $event.target;
      self.classList.toggle('active');
      document.body.classList.toggle('cbp-spmenu-push-toright');
      document.getElementById('cbp-spmenu-s1').classList.toggle('cbp-spmenu-open');
      leftOpen = true;
    };

    $rootScope.closeMenu = function() {
      document.body.classList.toggle('cbp-spmenu-push-toright');
      document.getElementById('cbp-spmenu-s1').classList.toggle('cbp-spmenu-open');
      leftOpen = false;
    };

    var rightOpen = false;
    $rootScope.pushMenu2 = function($event) {
      rightOpen = true;
      var self = $event.target;
      self.classList.toggle('active');
      document.body.classList.toggle('cbp-spmenu-push-toleft');
      document.getElementById('cbp-spmenu-s2').classList.toggle('cbp-spmenu-open');
    };

    $rootScope.closeMenu2 = function() {
      document.body.classList.toggle('cbp-spmenu-push-toleft');
      document.getElementById('cbp-spmenu-s2').classList.toggle('cbp-spmenu-open');
    };

    $rootScope.closeMenus = function() {
      if (leftOpen === true) {
        $rootScope.closeMenu();
      }
    };


    $rootScope.$on('$routeChangeSuccess', function() {
      $location.hash('top');
      $anchorScroll();

    });
   
    $rootScope.$on('$routeChangeError', function(event, current, previous, eventObj) {
      if (eventObj.authenticated === false) {
        $location.path('/login');
      }
    });

    $rootScope.$on('$stateChangeSuccess', function() {
      $location.hash('top');
      // $anchorScroll();
    });

    $rootScope.$on('$stateChangeError', function(event, toState, toParams, fromState, fromParams, error) {
      if (error.authenticated === false) {
        $state.go('login');
      }
    });

    $rootScope.spotlightModal = false;

  }]);
