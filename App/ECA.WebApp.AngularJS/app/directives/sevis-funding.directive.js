'use strict';

angular.module('staticApp')
  .directive('sevisFunding', function ($log, FilterService, LookupService, NotificationService, ParticipantExchangeVisitorService, ParticipantPersonsService) {
      var directive = {
          restrict: 'E',
          templateUrl: 'app/directives/sevis-funding.directive.html',
          scope: {
              personid: '@',
              exchangevisitorinfo: '=',
          },
          controller: function ($scope) {
              $scope.editMode = false;
              $scope.edit = {};
              $scope.editLocked = true;

              $scope.$watch("personid", function (personId) {
                  ParticipantPersonsService.getIsParticipantPersonLocked(personId)
                  .then(function (response) {
                      $scope.editLocked = response.data;
                  });
                  $scope.editMode = false;
              });

              $scope.editSevisFunding = function () {
                  if (!$scope.editLocked) {
                      $scope.editMode = true;
                  }
              }

              $scope.cancelEditFunding = function () {
                  loadExchangeVisitorInfo();
                  $scope.editMode = false;
              }

              $scope.saveEditFunding = function () {
                  saveExchangeVisitorInfo();
                  $scope.editMode = false;
              }

              $scope.onGovtAgency1Select = function (item) {
                  if (item.description != null)
                      if (item.description == "OTHER")
                          $scope.govtAgency1Other = true;
                      else {
                          $scope.govtAgency1Other = false;
                          $scope.exchangevisitorinfo.govtAgency1OtherName = '';
                      }
              };

              $scope.onGovtAgency2Select = function (item) {
                  if (item.description != null)
                      if (item.description == "OTHER")
                          $scope.govtAgency2Other = true;
                      else {
                          $scope.govtAgency2Other = false;
                          $scope.exchangevisitorinfo.govtAgency2OtherName = '';
                      }
              };

              $scope.onIntlOrg1Select = function (item) {
                  if (item.description != null)
                      if (item.description == "OTHER")
                          $scope.intlOrg1Other = true;
                      else {
                          $scope.intlOrg1Other = false;
                          $scope.exchangevisitorinfo.intlOrg1OtherName = '';
                      }
              };

              $scope.onIntlOrg2Select = function (item) {
                  if (item.description != null)
                      if (item.description == "OTHER")
                          $scope.intlOrg2Other = true;
                      else {
                          $scope.intlOrg2Other = false;
                          $scope.exchangevisitorinfo.intlOrg2OtherName = '';
                      }
              };

              var limit = 300;
              function loadUSGovernmentAgencies() {
                  var usGovernmentAgenciesFilter = FilterService.add('project-participant-editSevis-usGovernmentAgencies');
                  usGovernmentAgenciesFilter = usGovernmentAgenciesFilter.skip(0).take(limit);
                  return LookupService.getSevisUSGovernmentAgencies(usGovernmentAgenciesFilter.toParams())
                  .then(function (response) {
                      if (response.data.total > limit) {
                          var message = "The number of USGovernmentAgencies loaded is less than the total number.  Some USGovernmentAgencies may not be shown."
                          NotificationService.showErrorMessage(message);
                          $log.error(message);
                      }
                      $scope.edit.usGovernmentAgencies = response.data.results;
                      return $scope.edit.usGovernmentAgencies;
                  })
                  .catch(function (response) {
                      var message = "Unable to load USGovernmentAgencies.";
                      $log.error(message);
                      NotificationService.showErrorMessage(message);
                  });
              }

              function loadInternationalOrganizations() {
                  var internationalOrganizationsFilter = FilterService.add('project-participant-editSevis-internationalOrganizations');
                  internationalOrganizationsFilter = internationalOrganizationsFilter.skip(0).take(limit);
                  return LookupService.getSevisInternationalOrganizations(internationalOrganizationsFilter.toParams())
                  .then(function (response) {
                      if (response.data.total > limit) {
                          var message = "The number of InternationalOrganizations loaded is less than the total number.  Some InternationalOrganizations may not be shown."
                          NotificationService.showErrorMessage(message);
                          $log.error(message);
                      }
                      $scope.edit.internationalOrganizations = response.data.results;
                      return $scope.edit.internationalOrganizations;
                  })
                  .catch(function (response) {
                      var message = "Unable to load InternationalOrganizations.";
                      $log.error(message);
                      NotificationService.showErrorMessage(message);
                  });
              }

              function loadExchangeVisitorInfo() {
                  return ParticipantExchangeVisitorService.getParticipantExchangeVisitorById($scope.exchangevisitorinfo.projectId, $scope.exchangevisitorinfo.participantId)
                  .then(function (data) {
                      $scope.exchangevisitorinfo = data.data;
                  })
                  .catch(function (error) {
                      if (error.status === 404) {
                          $scope.exchangevisitorinfo = {};
                      } else {
                          $log.error('Unable to load participant exchange visitor info for ' + $scope.exchangevisitorinfo.participantId + '.');
                          NotificationService.showErrorMessage('Unable to load participant exchange visitor info for ' + $scope.exchangevisitorinfo.participantId + '.');
                      }
                  });
              }

              function saveExchangeVisitorInfo() {
                  return ParticipantExchangeVisitorService.updateParticipantExchangeVisitor($scope.exchangevisitorinfo.projectId, $scope.exchangevisitorinfo)
                  .then(function (data) {
                      NotificationService.showSuccessMessage('Participant exchange visitor info saved successfully.');
                      $scope.exchangevisitorinfo = data.data;
                  })
                  .catch(function (error) {
                      $log.error('Unable to save participant exchange visitor info for participantId: ' + $scope.exchangevisitorinfo.participantId);
                      NotificationService.showErrorMessage('Unable to save participant exchange visitor info for participant: ' + $scope.exchangevisitorinfo.participantId + '.');
                  });
              };

              loadUSGovernmentAgencies();
              loadInternationalOrganizations();
          }
      };
      return directive;
  });
