﻿'use strict';

/**
 * @ngdoc service
 * @name staticApp.project
 * @description
 * # project
 * Factory in the staticApp.
 */
angular.module('staticApp')
  .factory('LookupService', function (DragonBreath, $q) {

      return {
          getAllThemes: function (params) {
              var defer = $q.defer();
              DragonBreath.getCached(params, 'themes')
                .success(function (data) {
                    defer.resolve(data);
                });
              return defer.promise;
          },

          getAllGoals: function (params) {
              var defer = $q.defer();
              DragonBreath.getCached(params, 'goals')
                .success(function (data) {
                    defer.resolve(data);
                });
              return defer.promise;
          },
          getAllRegions: function (params) {
              var defer = $q.defer();
              DragonBreath.getCached(params, 'locations')
                .success(function (data) {
                    defer.resolve(data);
                });
              return defer.promise;
          },         
          getAllFocusAreas: function (params) {
              var defer = $q.defer();
              DragonBreath.get(params, 'focus')
                .success(function (data) {
                    defer.resolve(data);
                });
              return defer.promise;
          },
          getAllContacts: function (params) {
              var defer = $q.defer();
              DragonBreath.get(params, 'contacts')
                .success(function (data) {
                    defer.resolve(data);
                });
              return defer.promise;
          },
          getAllProjectStati: function (params) {
              return DragonBreath.getCached(params, 'projectstatuses');
          },
          getAllProgramStati: function (params) {
              return DragonBreath.getCached(params, 'programstatuses');
          },
          getSocialMediaTypes: function (params) {
              return DragonBreath.getCached(params, 'socialmedias/types');
          },
          getEmailAddressTypes:  function (params) {
              return DragonBreath.getCached(params, 'emailaddresses/types');
          },
          getPhoneNumberTypes: function (params) {
              return DragonBreath.getCached(params, 'phonenumbers/types');
          },
          getLocationTypes: function (params) {
              return DragonBreath.getCached(params, 'locations/types');
          },
          getDependentTypes: function (params) {
              return DragonBreath.getCached(params, 'dependent/types');
          },
          getBirthCountryReasons: function (params) {
              return DragonBreath.getCached(params, 'birthcountryreasons');
          },
          getParticipantTypes: function (params) {
              return DragonBreath.getCached(params, 'participanttypes');
          },
          getParticipantStatii: function (params) {
              return DragonBreath.getCached(params, 'participantstatuses');
          },
          getOrganizationTypes: function (params) {
              return DragonBreath.getCached(params, 'organizations/types');
          },
          getOrganizationRoles: function (params) {
              return DragonBreath.getCached(params, 'organizations/roles');
          },
          getAddressTypes: function (params) {
              return DragonBreath.getCached(params, 'addresses/types');
          },
          getAllMoneyFlowStati: function (params) {
              return DragonBreath.getCached(params, 'moneyflowstatuses');
          },
          getAllMoneyFlowTypes: function (params) {
              return DragonBreath.getCached(params, 'moneyflowtypes');
          },
          getAllMoneyFlowSourceRecipientTypes: function (params) {
              return DragonBreath.getCached(params, 'moneyflowsourcerecipienttypes');
          },
          getAllowedRecipientMoneyFlowSourceRecipientTypesBySourceTypeId: function (id) {
              return DragonBreath.getCached({}, 'moneyflowsourcerecipienttypes/' + id + '/recipienttypes');
          },
          getAllowedSourceMoneyFlowSourceRecipientTypesByRecipientTypeId: function (id) {
              return DragonBreath.getCached({}, 'moneyflowsourcerecipienttypes/' + id + '/sourcetypes');
          },
          getAllGenders: function (params) {
              var defer = $q.defer();
              DragonBreath.getCached(params, 'genders')
                .success(function (data) {
                    defer.resolve(data);
                });
              return defer.promise;
          },
          getAllMaritalStatuses: function (params) {
              var defer = $q.defer();
              DragonBreath.getCached(params, 'maritalStatuses')
                .success(function (data) {
                    defer.resolve(data);
                });
              return defer.promise;
          },
          getGeneric: function (params, strType) {
              var defer = $q.defer();
              DragonBreath.get(params, type)
                .success(function (data) {
                    defer.resolve(data);
                });
              return defer.promise;
          },
          getAllProminentCategories: function (params) {
              return DragonBreath.getCached(params, 'prominentCategories');
          },
          getLanguages: function (params) {
              return DragonBreath.getCached(params, 'languages');
          },
          getSevisCommStatuses: function(params) {
              return DragonBreath.getCached(params,'seviscommstatuses');
          },
          getSevisPositions: function (params) {
              return DragonBreath.getCached(params, 'positions');
          },
          getSevisProgramCategories: function (params) {
              return DragonBreath.getCached(params, 'programcategories');
          },
          getSevisFieldOfStudies: function (params) {
              return DragonBreath.getCached(params, 'fieldofstudies');
          },
          getSevisUSGovernmentAgencies: function (params) {
          return DragonBreath.getCached(params, 'usgovernmentagencies');
          },
          getSevisInternationalOrganizations: function (params) {
              return DragonBreath.getCached(params, 'internationalorganizations');
          },
          getSevisStudentCreations: function (params) {
              return DragonBreath.getCached(params, 'studentcreations');
          },
          getSevisEducationLevels: function (params) {
              return DragonBreath.getCached(params, 'educationlevels');
          },
          getVisitorTypes: function(params) {
              return DragonBreath.getCached(params,'visitorTypes');
          }
      };
  });
