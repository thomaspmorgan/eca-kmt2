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
          },
          createDataPointConfiguration: function (dataPointConfiguration) {
              return DragonBreath.create(dataPointConfiguration, 'dataPointConfigurations');
          },
          getDataPointConfigurations: function (params) {
              return DragonBreath.get(params, 'dataPointConfigurations');
          }
      };
  });