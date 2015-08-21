﻿'use strict';

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
              reset: function () {
                  this.start = 0;
                  this.limit = 0;
                  this.sort = [];
                  this.filter = [];
              },

              skip: function (start) {
                  this.start = start;
                  return this;
              },

              take: function (limit) {
                  this.limit = limit;
                  return this;
              },

              sortBy: function (propertyName) {
                  return this._sortBy(propertyName, 'asc');
              },

              sortByDescending: function (propertyName) {
                  return this._sortBy(propertyName, 'desc');
              },

              _sortBy: function (propertyName, direction) {
                  this.sort = [
                    {
                        property: propertyName,
                        direction: direction
                    }
                  ];
                  return this;
              },

              thenBy: function (propertyName) {
                  return this._thenBy(propertyName, 'asc');
              },

              thenByDescending: function (propertyName) {
                  return this._thenBy(propertyName, 'desc');
              },

              _thenBy: function (propertyName, direction) {
                  this.sort.push({
                      property: propertyName,
                      direction: direction
                  });
                  return this;
              },

              equal: function (propertyName, value) {
                  return this._addFilter(ConstantsService.equalComparisonType, propertyName, value);
              },

              notEqual: function (propertyName, value) {
                  return this._addFilter(ConstantsService.notEqualComparisonType, propertyName, value);
              },

              in: function (propertyName, value) {
                  return this._addFilter(ConstantsService.inComparisonType, propertyName, value);
              },

              notIn: function (propertyName, value) {
                  return this._addFilter(ConstantsService.notInComparisonType, propertyName, value);
              },

              like: function (propertyName, value) {
                  return this._addFilter(ConstantsService.likeComparisonType, propertyName, value);
              },

              isNotNull: function (propertyName) {
                  return this._addFilter(ConstantsService.isNotNullComparisonType, propertyName);
              },
              isNull: function (propertyName) {
                  return this._addFilter(ConstantsService.isNullComparisonType, propertyName);
              },



              toParams: function () {
                  return {
                      start: this.start,
                      limit: this.limit,
                      filter: this.filter,
                      sort: this.sort
                  }
              },

              _addFilter: function (comparison, propertyName, val) {
                  var f = {
                      comparison: comparison,
                      property: propertyName,
                  };
                  if (val) {
                      f.value = val;
                  }
                  this.filter.push(f);
                  return this;
              }
          };
          service.parameters[key] = clientFilter;
          return clientFilter;
      }

      service.remove = function (key) {
          return service.parameters[key];
      }

      service.get = function (key) {
          return service.parameters[key];
      }
      return service;
  });