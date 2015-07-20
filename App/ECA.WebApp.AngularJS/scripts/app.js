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
    'isteven-multi-select',
    'xeditable',
    'sticky',
    'AdalAngular',
    'smart-table',
    'ui.select',
    'ui.date',
    'toaster',
    'ngAnimate',
    'smoothScroll'
  ])
  .config(function ($stateProvider, $httpProvider, $urlRouterProvider, adalAuthenticationServiceProvider) {

      adalAuthenticationServiceProvider.init({
          base: '',
          tenant: 'statedept.us',
          clientId: 'e0356e55-e124-452c-837d-aeb7504185ff',
          resource: 'https://ecaserver.state.gov'
      }, $httpProvider
      );

      $urlRouterProvider.otherwise('/');

      $stateProvider
        .state('home', {
            templateUrl: 'views/home.html',
            controller: 'HomeCtrl',
            requireADLogin: true
        })
        .state('home.shortcuts', {
            url: '/',
            templateUrl: 'views/home/shortcuts.html',
            requireADLogin: true
        })
        .state('home.notifications', {
            url: '/',
            templateUrl: 'views/home/notifications.html',
            requireADLogin: true
        })
        .state('home.news', {
            url: '/',
            templateUrl: 'views/home/news.html',
            requireADLogin: true
        })

        .state('activities', {
            url: '/activities',
            templateUrl: 'views/activities/activityList.html',
            requireADLogin: true
        })
        .state('activitiesCreate', {
            url: '/activities/create',
            templateUrl: 'views/activities/activityCreate.html',
            requireADLogin: true
        })
        .state('activitiesTag', {
            url: '/activities/tag',
            templateUrl: 'views/activities/activitiyTag.html',
            requireADLogin: true
        })
        .state('activitiesOverview', {
            url: '/activities/overview',
            templateUrl: 'views/activities/activityOverview.html',
            requireADLogin: true
        })
        .state('forbidden', {
            url: '/forbidden',
            templateUrl: 'views/forbidden.html',
            requireADLogin: false
        })
        .state('error', {
            url: '/error',
            templateUrl: 'views/error.html',
            requireADLogin: false
        })

        .state('about', {
            url: '/about',
            templateUrl: 'views/about.html',
            controller: 'AboutCtrl',
            requireADLogin: true
        })
        // .state('list', {
        //   url: 
        // })

        .state('secureTest', {
            url: '/secureTest',
            templateUrl: 'views/securetest.html',
            controller: 'SecuretestCtrl',
            requireADLogin: true
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
            requireADLogin: true
        })

        .state('reports', {
            url: '/report',
            templateUrl: 'views/reports.html',
            controller: 'ReportsCtrl',
            requireADLogin: true
        })

        .state('reportsCreate', {
            url: '/report/create',
            templateUrl: 'views/reports/create.html',
            requireADLogin: true
        })
        .state('reportsCreated', {
            url: '/report/created',
            templateUrl: 'views/reports/created.html',
            requireADLogin: true
        })
        .state('reportsAddData', {
            url: '/report/addData',
            templateUrl: 'views/reports/addData.html',
            requireADLogin: true
        })
        .state('reportsSearchResults', {
            url: '/report/searchResult',
            templateUrl: 'views/reports/searchResults.html',
            requireADLogin: true
        })
        .state('reportsRefineSearch', {
            url: '/report/refineSearch',
            templateUrl: 'views/reports/refineSearch.html',
            requireADLogin: true
        })
        .state('reportsRefineSearchDeselect', {
            url: '/report/refineSearchDeselect',
            templateUrl: 'views/reports/refineSearchDeselect.html',
            requireADLogin: true
        })
        .state('reportsSearchResultsClearance', {
            url: '/report/searchResultClearance',
            templateUrl: 'views/reports/searchResultsClearance.html',
            requireADLogin: true
        })
        .state('reportsContent', {
            url: '/report/content',
            templateUrl: 'views/reports/content.html',
            requireADLogin: true
        })
        .state('reportsCrunchingData', {
            url: '/report/crunchingData',
            templateUrl: 'views/reports/crunchingData.html',
            requireADLogin: true
        })
        .state('reportsGenerated', {
            url: '/report/reportGenerated',
            templateUrl: 'views/reports/reportGenerated.html',
            requireADLogin: true
        })
        .state('reportsOverviewOpen', {
            url: '/report/overviewOpen',
            templateUrl: 'views/reports/overviewOpen.html',
            requireADLogin: true
        })
        .state('reportsPreview', {
            url: '/report/preview',
            templateUrl: 'views/reports/preview.html',
            requireADLogin: true
        })
        .state('reportsAddCollab', {
            url: '/report/addCollab',
            templateUrl: 'views/reports/addCollab.html',
            requireADLogin: true
        })
        .state('reportsSubmittingClearance', {
            url: '/report/submittingClearance',
            templateUrl: 'views/reports/submittingClearance.html',
            requireADLogin: true
        })
        .state('reportsSubmitClearance', {
            url: '/report/submitClearance',
            templateUrl: 'views/reports/submitClearance.html',
            requireADLogin: true
        })
        .state('reportsClearing', {
            url: '/report/clearing',
            templateUrl: 'views/reports/clearing.html',
            requireADLogin: true
        })
        .state('reportsPublish', {
            url: '/report/publish',
            templateUrl: 'views/reports/publish.html',
            requireADLogin: true
        })


        .state('reports.archive', {
            url: '/archive',
            templateUrl: 'views/reports/archive.html',
            controller: 'ReportsArchiveCtrl',
            requireADLogin: true
        })
        .state('reports.custom', {
            url: '/custom',
            templateUrl: 'views/reports/custom.html',
            controller: 'ReportsCustomCtrl',
            requireADLogin: true
        })


        .state('settings', {
            url: '/settings',
            templateUrl: 'views/settings.html',
            controller: 'SettingsCtrl',
            requireADLogin: true
        })
        .state('participants', {
            url: '/participants/:personId',
            templateUrl: 'views/participants.html',
            controller: 'ParticipantCtrl',
            requireADLogin: true
        })
        .state('participants.overview', {
            url: '/overview',
            templateUrl: 'views/participants/overview.html',
            controller: 'ParticipantCtrl',
            requireADLogin: true
        })
        .state('participants.timeline', {
            url: '/timeline',
            templateUrl: 'views/participants/timeline.html',
            controller: 'ParticipantTimelineCtrl'
        })
        .state('participants.personalinformation', {
            url: '/personalinformation',
            templateUrl: 'views/participants/personalinformation.html',
            controller: 'ParticipantCtrl'
        })
        .state('participants.relatedreports', {
            url: '/relatedreports',
            templateUrl: 'views/participants/relatedreports.html',
            controller: 'ParticipantCtrl'
        })
        .state('participants.impact', {
            url: '/impact',
            templateUrl: 'views/participants/impact.html',
            controller: 'ParticipantCtrl'
        })

        .state('allprograms', {
            url: '/allprograms',
            templateUrl: 'views/program/allprograms.html',
            controller: 'AllProgramsCtrl',
            requireADLogin: true
        })

        .state('programs', {
            url: '/programs/:programId',
            templateUrl: 'views/programs.html',
            controller: 'ProgramsCtrl',
            requireADLogin: true
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

        .state('programs.collaborators', {
            url: '/collaborators',
            templateUrl: 'views/program/collaborators.html',
            controller: 'ProgramCollaboratorsCtrl'
        })

        .state('programs.moneyflows', {
            url: '/moneyflows',
            templateUrl: 'views/program/moneyflows.html',
            controller: 'ProgramMoneyFlowsCtrl'
        })

        .state('projects', {
            url: '/offices/:officeId/programs/:programId/project/:projectId',
            templateUrl: 'views/project.html',
            controller: 'ProjectCtrl',
            requireADLogin: true
        })
        .state('projects.overview', {
            url: '/overview',
            templateUrl: 'views/project/overview.html',
            controller: 'ProjectOverviewCtrl',
            requireADLogin: true
        })
        .state('projects.edit', {
            url: '/edit',
            templateUrl: 'views/project/edit.html',
            controller: 'ProjectEditCtrl',
            requireADLogin: true
        })
        .state('projects.participants', {
            url: '/participant',
            templateUrl: 'views/project/participant.html',
            controller: 'ProjectParticipantCtrl',
            requireADLogin: true
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
            templateUrl: 'views/project/moneyflows.html',
            controller: 'ProjectMoneyFlowsCtrl'
        })

        .state('projects.impact', {
            url: '/impact',
            templateUrl: 'views/project/impact.html'
        })

        .state('offices', {
            url: '/offices/:officeId',
            templateUrl: 'views/offices.html',
            controller: 'OfficeCtrl',
            requireADLogin: true
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

        .state('alloffices', {
            url: '/alloffices',
            templateUrl: 'views/office/alloffices.html',
            controller: 'AllOfficesCtrl',
            requireADLogin: true
        })
        .state('allparticipants', {
            url: '/allparticipants',
            templateUrl: 'views/participants/allparticipants.html',
            controller: 'AllParticipantsCtrl',
            requireADLogin: true
        })
        .state('allpersons', {
            url: '/allpersons',
            templateUrl: 'views/participants/allpersons.html',
            controller: 'AllPersonsCtrl',
            requireADLogin: true
        })
        .state('allorganizations', {
            url: '/allorganizations',
            templateUrl: 'views/organizations/allorganizations.html',
            controller: 'AllOrganizationsCtrl',
            requireADLogin: true
        })
        .state('organizations', {
            url: '/organizations/:organizationId',
            templateUrl: 'views/organization.html',
            controller: 'OrganizationCtrl',
            requireADLogin: true
        })
        .state('organizations.overview', {
            url: '/overview',
            templateUrl: 'views/organizations/overview.html',
            controller: 'OrganizationOverviewCtrl',
            requireADLogin: true
        })        
        .state('organizations.artifacts', {
            url: '/artifacts',
            templateUrl: 'views/organizations/artifacts.html',
            controller: 'OrganizationArtifactsCtrl',
            requireADLogin: true
        })
        .state('organizations.impact', {
            url: '/impact',
            templateUrl: 'views/organizations/impact.html',
            controller: 'OrganizationImpactCtrl',
            requireADLogin: true
        })
        .state('organizations.activities', {
            url: '/activities',
            templateUrl: 'views/organizations/activities.html',
            controller: 'OrganizationActivitiesCtrl',
            requireADLogin: true
        })
        .state('partner', {
            url: '/partner',
            templateUrl: 'views/partner.html',
            requireADLogin: true
        })

        .state('offshoot', {
            url: '/clearance',
            templateUrl: 'views/offshoot.html',
            requireADLogin: true
        })

        .state('lastupdated', {
            url: '/lastupdated',
            templateUrl: 'views/lastupdated.html',
            requireADLogin: true
        });
      $httpProvider.interceptors.push('ErrorInterceptor');
  })
  .run(['$rootScope', '$location', '$state', 'editableOptions', '$anchorScroll', 'LogoutEventService', 'ConstantsService', 'RegisterUserEventService', 'NotificationService',
    function ($rootScope, $location, $state, editableOptions, $anchorScroll, LogoutEventService, ConstantsService, RegisterUserEventService, NotificationService) {

        console.assert(RegisterUserEventService, "The RegisterUserEventService is needed so that we can register on rootscope the handler to automatically register the user.");
        editableOptions.theme = 'bs3';
        $rootScope.rootStates = [
          { name: 'Home', state: 'home.shortcuts' },
          { name: 'Offices', state: 'alloffices' },
          { name: 'Programs', state: 'allprograms' },
          { name: 'Persons', state: 'allpersons' },
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
            NotificationService.showErrorMessage('There was an error registering your user account in this application.');
        });
        $rootScope.$on(ConstantsService.registerUserSuccessEventName, function () {
            $rootScope.currentUser.isBusy = false;
            NotificationService.showSuccessMessage('This is your first visit to the application!  You have been successfully registered.');
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

        $rootScope.spotlightModal = false;
    }]);