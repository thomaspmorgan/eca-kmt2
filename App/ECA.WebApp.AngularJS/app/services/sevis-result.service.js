'use strict';

/**
 * @ngdoc service
 * @name staticApp.SevisResult
 * @description
 * # SevisResult
 * Factory for handling social media.
 */
angular.module('staticApp')
  .factory('SevisResultService', function ($q, $log, ParticipantPersonsSevisService, PersonService) {

      var obj = {};

      // update the sevis verification result for a participant
      obj.updateSevisVerificationResults = function (personid) {
          var defer = $q.defer();
          PersonService.getParticipantByPersonId(personid)
          .then(function (participant) {

            // initiate pre-sevis validation
            if (participant.data.sevisId) {
                // pre-sevis update validation
                ParticipantPersonsSevisService.validateParticipantPersonsUpdateSevis(participant.data.participantId)
                  .then(function (response) {
                      $log.info('Validated participant update SEVIS information');
                      var valErrors = [];
                      for (var i = 0; i < response.data.errors.length; i++) {
                          valErrors.push({ msg: response.data.errors[i].errorMessage, path: response.data.errors[i].customState });
                      }
                      // log participant sevis validation attempt
                      ParticipantPersonsSevisService.createParticipantSevisCommStatus(participant.data.participantId, response.data);
                      // update participant sevis validation results
                      obj.updateSevisInfo(participant.data.participantId, response.data);

                      defer.resolve(valErrors);
                  })
                  .catch(function () {
                      $log.error("Unable to validate participant create SEVIS information.");
                  });
            } else {
                if (participant.data.participantId) {
                    // pre-sevis create validation
                    ParticipantPersonsSevisService.validateParticipantPersonsCreateSevis(participant.data.participantId)
                    .then(function (response) {
                        $log.info('Validated participant create SEVIS information');
                        var valErrors = [];
                        for (var i = 0; i < response.data.errors.length; i++) {
                            valErrors.push({ msg: response.data.errors[i].errorMessage, path: response.data.errors[i].customState });
                        }
                        // log participant sevis validation attempt
                        ParticipantPersonsSevisService.createParticipantSevisCommStatus(participant.data.participantId, response.data);
                        // update participant sevis validation results
                        obj.updateSevisInfo(participant.data.participantId, response.data);

                        defer.resolve(valErrors);
                    })
                    .catch(function () {
                        $log.error("Unable to validate participant create SEVIS information.");
                    });
                }
            }
          })
          .catch(function () {
              $log.error("Unable to retrieve participant by person id.");
          });

          return defer.promise;
      }

      // get participant record and attach validation results
      obj.updateSevisInfo = function (participantId, validationResults) {
          ParticipantPersonsSevisService.getParticipantPersonsSevisById(participantId)
          .then(function (data) {
              var sevisInfo = data.data;
              sevisInfo.sevisValidationResult = JSON.stringify(validationResults);
              obj.saveSevisInfo(participantId, sevisInfo);
          })
          .catch(function () {
              $log.error('Unable to load participant SEVIS information.');
          });
      }

      // update participant sevis results
      obj.saveSevisInfo = function (participantId, updatedSevisInfo) {
          ParticipantPersonsSevisService.updateParticipantPersonsSevis(updatedSevisInfo)
          .then(function (data) {
              $log.info('Participant SEVIS verification results saved successfully.');
          })
          .catch(function () {
              $log.error('Unable to save participant SEVIS verification results');
          });
      }

      return obj;
  });
