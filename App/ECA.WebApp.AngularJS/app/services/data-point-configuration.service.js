'use strict';

/**
 * @ngdoc service
 * @name staticApp.DataPointConfigurationService
 * @description
 * # DataPointConfigurationService
 * Factory in the staticApp.
 */
angular.module('staticApp')
  .factory('DataPointConfigurationService', function (DragonBreath) {
      return {
          deleteDataPointConfiguration: function (dataPointConfiguration) {
              return DragonBreath.delete(dataPointConfiguration, 'dataPointConfigurations/' + dataPointConfiguration.dataPointConfigurationId);
          }
      };
  });