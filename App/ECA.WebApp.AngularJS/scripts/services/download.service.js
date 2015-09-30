'use strict';

/**
 * @ngdoc service
 * @name staticApp.LocationService
 * @description
 * # LocationService
 * Factory in the staticApp.
 */
angular.module('staticApp')
  .factory('DownloadService', function (DragonBreath, $q, $http, $log, NotificationService) {
      var errorMessage = 'The browser does not support Blob storage. File downloads will not be supported.';
      try {
          var isFileSaverSupported = !!new Blob;
          if (!isFileSaverSupported) {
              NotificationService.showErrorMessage(errorMessage);
              $log.error(errorMessage);
          }
      } catch (e) {
          NotificationService.showErrorMessage(errorMessage);
          $log.error(errorMessage);
      }

      return {
          //Given the path of the download (NOT including the origin i.e. localhost:5555), the content type
          //of the download and the desired fileName, this method will perform a get on the web api
          //with the authorization header.  If you wish to use the server generated fileName,
          //do not pass a value for fileName.  If the server does not send a file name, and one is not
          //provided a default file name will be used.  If you do not know the content type do not pass a value
          //and the default will be used.
          get: function (path, contentType, fileName) {
              contentType = contentType || 'application/octet-stream';
              var url = DragonBreath.getUrl(path);
              return $http.get(url, {
                  method: 'GET',
                  responseType: 'arraybuffer'
              })
              .success(function (data, status, headers, config) {
                  if (!fileName) {
                      var disposition = headers('Content-Disposition');
                      if (disposition) {
                          disposition = disposition.toLowerCase();
                          var index = disposition.indexOf('=');
                          if (index >= 0) {
                              fileName = disposition.substring(index + 1);
                          }
                          else {
                              fileName = 'download.file';
                          }
                      }
                      else {
                          $log.warn('Unable to determine a filename for the file download.  Using a default name.');
                          fileName = "download.file";
                      }
                  }
                  var blob = new Blob([data], { type: contentType });
                  saveAs(blob, fileName);
              })
              .error(function (data, status, headers, config) {
                  $log.error('Unable to download file requested at path ' + path + ' with content type ' + contentType);
              });

          }
      };
  });
