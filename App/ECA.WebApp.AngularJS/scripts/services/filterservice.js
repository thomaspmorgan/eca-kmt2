'use strict';

/**
 * @ngdoc service
 * @name staticApp.filterService
 * @description
 * # filterService
 * Factory for handling server side filtering.
 */
angular.module('staticApp')
  .factory('FilterService', function (
      $rootScope,
      $log,
      $http,
      $q,
      $window,
      DragonBreath,
      ConstantsService) {

      var service = {};
      service.parameters = {};

      service.add = function (key) {
          var clientFilter = {
              start: 0,
              limit: 0,
              sort: [],
              filter: [],
              key: key,
              reset: function() {
                  this.start = 0;
                  this.limit = 0;
                  this.sort = [];
                  this.filter = [];
              },

              skip: function(start){
                  this.start = start;
                  return this;
              },

              take: function(limit){
                  this.limit = limit;
                  return this;
              },

              sortBy: function(propertyName, direction) {
                  this.sort = [
                      {
                          property: propertyName,
                          direction: direction
                      }
                  ];
              },

              thenBy: function(propertyName, direction){
                  this.sort.push({
                      property: propertyName,
                      direction: direction
                  });
              },

              equal: function(propertyName, value){
                  return this._addFilter(ConstantsService.equalComparisonType, propertyName, value);
              },

              notEqual: function(propertyName, value){
                  return this._addFilter(ConstantsService.notEqualComparisonType, propertyName, value);
              },

              in: function(propertyName, value){
                  return this._addFilter(ConstantsService.inComparisonType, propertyName, value);
              },

              toParams: function() {
                  return {
                      start: this.start,
                      limit: this.limit,
                      filter: this.filter,
                      sort: this.sort
                  }
              },

              _addFilter: function(comparison, propertyName, value){
                  this.filter.push({
                      comparison: comparison,
                      property: propertyName,
                      value: value
                  });
                  return this;
              }
          };
          service.parameters[key] = clientFilter;
          return clientFilter;
      }

      service.remove = function (key) {
          return service.parameters[key];
      }

      service.get = function(key){
          return service.parameters[key];
      }
      return service;
  });