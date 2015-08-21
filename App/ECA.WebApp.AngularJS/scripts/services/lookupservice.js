'use strict';

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
          getSocialMediaTypes: function (params) {
              return DragonBreath.getCached(params, 'socialmedias/types');
          },
          getLocationTypes: function (params) {
              return DragonBreath.getCached(params, 'locations/types');
          },
          getParticipantTypes: function (params) {
              return DragonBreath.getCached(params, 'participanttypes');
          },
          getOrganizationTypes: function (params) {
              return DragonBreath.getCached(params, 'organizations/types');
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
          getAllLanguages: function (params) {
              return DragonBreath.getCached(params, 'languages');
          }

      };
  });
