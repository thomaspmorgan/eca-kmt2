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
              DragonBreath.get(params, 'themes')
                .success(function (data) {
                    defer.resolve(data);
                });
              return defer.promise;
          },

          getAllGoals: function (params) {
              var defer = $q.defer();
              DragonBreath.get(params, 'goals')
                .success(function (data) {
                    defer.resolve(data);
                });
              return defer.promise;
          },
          getAllRegions: function (params) {
              var defer = $q.defer();
              DragonBreath.get(params, 'locations')
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
              return DragonBreath.get(params, 'projectstatuses');
          },
          getSocialMediaTypes: function (params) {
              return DragonBreath.get(params, 'socialmedias/types');

          },
          getParticipantTypes: function (params) {
              return DragonBreath.get(params, 'participanttypes');
          },
          getOrganizationTypes: function (params) {
              return DragonBreath.get(params, 'organizations/types');
          },
          getAddressTypes: function (params) {
              return DragonBreath.get(params, 'addresses/types');
          },
          getAllMoneyFlowStati: function (params) {
              return DragonBreath.get(params, 'moneyflowstatuses');
          },
          getAllMoneyFlowTypes: function (params) {
              return DragonBreath.get(params, 'moneyflowtypes');
          },
          getAllMoneyFlowSourceRecipientTypes: function (params) {
              return DragonBreath.get(params, 'moneyflowsourcerecipienttypes');
          },
          getAllGenders: function (params) {
              var defer = $q.defer();
              DragonBreath.get(params, 'genders')
                .success(function (data) {
                    defer.resolve(data);
                });
              return defer.promise;
          },
          getAllMaritalStatuses: function (params) {
              var defer = $q.defer();
              DragonBreath.get(params, 'maritalStatuses')
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
          }
  };
});
