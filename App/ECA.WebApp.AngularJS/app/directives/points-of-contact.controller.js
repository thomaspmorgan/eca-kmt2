'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:PointsOfContactCtrl
 * @description The controller is used to control the list of points of contact of a project.
 * # PointsOfContactCtrl
 * Controller of the project points of contact directive
 */
angular.module('staticApp')
  .controller('PointsOfContactCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        filterFilter,
        ContactsService,
        LookupService,
        NotificationService,
        ConstantsService,
        FilterService) {
            
      $scope.view = {};
      $scope.view.pointOfContactFilter = '';
      $scope.view.isLoadingRequiredData = false;
      $scope.view.isSavingPointOfContact = false;
      $scope.view.isLoadingPointsOfContactByFullName = false;
      var tempId = 0;

      $scope.view.maxNameLength = 100;
      $scope.view.searchLimit = 30;
      $scope.view.maxEmailAddresses = 10;
      $scope.view.maxPhoneNumbers = 10;
      $scope.view.collapsePocs = true;
      $scope.view.collapsePoc = true;
      $scope.view.showEditPoc = false;
      $scope.view.pointsOfContact = [];
      $scope.view.selectedPointsOfContact = [];
      $scope.view.newPointOfContact = createNewPointOfContact();
      $scope.view.likePointsOfContactByFullNameTotal = 0;
      $scope.view.showConfirmDelete = false;
      
      var maxLimit = 300;
      var pocFilter = FilterService.add('projectedit_pocfilter');
      function loadPointsOfContact(search) {
          pocFilter.reset();
          pocFilter = pocFilter.skip(0).take(maxLimit);
          if (search) {
              pocFilter = pocFilter.like('fullName', search);
          }
          return LookupService.getAllContacts(pocFilter.toParams())
              .then(function (response) {
                  if (response.total > maxLimit) {
                      $log.error('There are more contacts in the system then are currently loaded, an issue could occur in the UI not showing all possible values.');
                  }
                  var position = "";
                  for (var i = 0; i < response.results.length; i++) {
                        position = "";
                        if (response.results[i].position) {
                            position = " (" + response.results[i].position + ")";
                        }
                        response.results[i].value = response.results[i].fullName + position;
                  }
                  $scope.view.pointsOfContact = response.results;
              });
      }

      $scope.view.onAddPointOfContactClick = function (pointsOfContact) {
          console.assert(pointsOfContact, 'The entity points of contact is not defined.');
          console.assert(pointsOfContact instanceof Array, 'The entity points of contact is defined but must be an array.');
          var newPointOfContact = {
              id: --tempId,
              isNew: true,
              emailAddresses: [],
              phoneNumbers: []
          };
          pointsOfContact.splice(0, 0, newPointOfContact);
          $scope.view.collapsePoc = false;
      }
      
      var likePointsOfContactByFullNameFilter = FilterService.add('points-of-contact-model-search-by-full-name-filter');
      function loadPointsOfContactByFullName(fullName) {
          if (fullName && fullName.length > 0) {
              likePointsOfContactByFullNameFilter.reset();
              likePointsOfContactByFullNameFilter = likePointsOfContactByFullNameFilter
                  .skip(0)
                  .take(1)
                  .equal('fullName', fullName);
              $scope.view.isLoadingPointsOfContactByFullName = true;
              var params = likePointsOfContactByFullNameFilter.toParams();
              return ContactsService.get(params)
              .then(function (response) {
                  $scope.view.likePointsOfContactByFullName = response.data.results;
                  $scope.view.isLoadingPointsOfContactByFullName = false;
                  $scope.view.likePointsOfContactByFullNameTotal = response.data.total;
                  return $scope.view.likePointsOfContactByFullName;
              })
              .catch(function (response) {
                  $scope.view.isLoadingPointsOfContactByFullName = false;
                  var mesage = "Unable to load like points of contact.";
                  NotificationService.showErrorMessage(message);
                  $log.error(message);
              });
          }
      }
      
      function createNewPointOfContact() {
          var newPointOfContact = {
              emailAddresses: [],
              phoneNumbers: []
          };
          return newPointOfContact;
      }

      $q.all([loadPointsOfContact(null)]);

  });