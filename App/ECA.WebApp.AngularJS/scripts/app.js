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
    'ui.map',
    'ui.validate',
    'ui.event',
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
    'smoothScroll',
    'internationalPhoneNumber'
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
        .state('people', {
            url: '/people/:personId',
            templateUrl: 'views/person.html',
            controller: 'PeopleCtrl',
            requireADLogin: true
        })
        .state('people.timeline', {
            url: '/timeline',
            templateUrl: 'views/person/timeline.html',
            controller: 'PersonTimelineCtrl',
            requireADLogin: true
        })
        .state('people.personalinformation', {
            url: '/personalinformation',
            templateUrl: 'views/person/personalinformation.html',
            controller: 'PersonInformationCtrl',
            requireADLogin: true
        })
        .state('people.relatedreports', {
            url: '/relatedreports',
            templateUrl: 'views/person/relatedreports.html',
            requireADLogin: true
        })
        .state('people.impact', {
            url: '/impact',
            templateUrl: 'views/person/impact.html',
            requireADLogin: true
        })
        .state('people.moneyflows', {
            url: '/moneyflows',
            templateUrl: 'views/person/moneyflows.html',
            requireADLogin: true
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
            controller: 'ProgramCtrl',
            requireADLogin: true
        })
        .state('programs.overview', {
            url: '/overview',
            controller: 'ProgramOverviewCtrl',
            templateUrl: 'views/program/overview.html',
            requireADLogin: true
        })
        .state('programs.edit', {
            url: '/edit',
            controller: 'ProgramEditCtrl',
            templateUrl: 'views/program/edit.html',
            requireADLogin: true
        })
        .state('programs.projects', {
            url: '/projects',
            templateUrl: 'views/program/projects.html',
            controller: 'SubProgramsAndProjectsCtrl',
            requireADLogin: true
        })

        .state('programs.activity', {
            url: '/activity',
            templateUrl: 'views/program/activity.html',
            requireADLogin: true
        })
        .state('programs.artifacts', {
            url: '/artifacts',
            templateUrl: 'views/program/artifacts.html',
            requireADLogin: true
        })

        .state('programs.impact', {
            url: '/impact',
            templateUrl: 'views/program/impact.html',
            requireADLogin: true
        })
        .state('programs.moneyflows', {
            url: '/moneyflows',
            templateUrl: 'views/program/moneyflows.html',
            requireADLogin: true
        })

        .state('projects', {
            url: '/projects/:projectId',
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
            templateUrl: 'views/project/artifact.html',
            requireADLogin: true
        })
        .state('projects.activity', {
            url: '/activity',
            templateUrl: 'views/project/activity.html',
            requireADLogin: true
        })
        .state('projects.itineraries', {
            url: '/itinerary',
            templateUrl: 'views/project/itinerary.html',
            requireADLogin: true
        })
        .state('projects.implementers', {
            url: '/implementers',
            templateUrl: 'views/project/implementer.html',
            requireADLogin: true
        })
        .state('projects.partners', {
            url: '/partners',
            templateUrl: 'views/project/partners.html',
            requireADLogin: true
        })

        .state('projects.moneyflows', {
            url: '/moneyflows',
            templateUrl: 'views/project/moneyflows.html',
            requireADLogin: true
        })

        .state('projects.impact', {
            url: '/impact',
            templateUrl: 'views/project/impact.html',
            requireADLogin: true
        })

        .state('offices', {
            url: '/offices/:officeId',
            templateUrl: 'views/offices.html',
            controller: 'OfficeCtrl',
            requireADLogin: true
        })
        .state('offices.overview', {
            url: '/overview',
            templateUrl: 'views/office/overview.html',
            controller: 'OfficeOverviewCtrl',
            requireADLogin: true
        })
        .state('offices.branches', {
            url: '/branches',
            templateUrl: 'views/office/branches.html',
            controller: 'BranchesAndProgramsCtrl',
            requireADLogin: true
        })
        .state('offices.activity', {
            url: '/activity',
            templateUrl: 'views/office/activity.html',
            requireADLogin: true
        })
        .state('offices.artifacts', {
            url: '/artifacts',
            templateUrl: 'views/office/artifacts.html',
            requireADLogin: true
        })
        .state('offices.moneyflows', {
            url: '/moneyflows',
            templateUrl: 'views/office/moneyflows.html',
            requireADLogin: true
        })

        .state('alloffices', {
            url: '/alloffices',
            templateUrl: 'views/office/alloffices.html',
            controller: 'AllOfficesCtrl',
            requireADLogin: true
        })
        .state('allpeople', {
            url: '/allpeople',
            templateUrl: 'views/person/allpeople.html',
            controller: 'AllPeopleCtrl',
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
        .state('organizations.moneyflows', {
            url: '/moneyflows',
            templateUrl: 'views/organizations/moneyflows.html',
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
  .run(['$rootScope', '$location', '$state', '$modal', 'editableOptions', '$anchorScroll', 'LogoutEventService', 'ConstantsService', 'RegisterUserEventService', 'NotificationService',
    function ($rootScope, $location, $state, $modal, editableOptions, $anchorScroll, LogoutEventService, ConstantsService, RegisterUserEventService, NotificationService) {

        console.assert(RegisterUserEventService, "The RegisterUserEventService is needed so that we can register on rootscope the handler to automatically register the user.");
        editableOptions.theme = 'bs3';
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
        
        $rootScope.onSpotlightSearchClick = function () {
            var spotlightModalInstance = $modal.open({
                animation: true,
                templateUrl: 'views/partials/searchbar.html',
                controller: 'searchbarCtrl',
                size: '90',
                resolve: {}
            });
        };


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

    }]);