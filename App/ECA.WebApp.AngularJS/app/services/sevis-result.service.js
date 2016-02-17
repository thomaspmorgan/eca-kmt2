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
      obj.updateSevisVerificationResultsByPersonId = function (personid) {
          if (personid) {
              var defer = $q.defer();
              PersonService.getParticipantByPersonId(personid)
              .then(function (participant) {
                  if (participant.data.sevisId) {
                      return obj.validateUpdateSevis(participant.data.projectId, participant.data.participantId);
                  } else {
                      if (participant.data.participantId) {
                          defer.resolve(obj.validateCreateSevis(participant.data.projectId, participant.data.participantId));
                      }
                  }
              })
              .catch(function () {
                  $log.error("Unable to retrieve participant by person id.");
              });

              return defer.promise;
          }
      }

      obj.updateSevisVerificationResultsByParticipant = function (participant) {
          if (participant) {
              var defer = $q.defer();
              if (participant.sevisId) {
                  defer.resolve(obj.validateUpdateSevis(participant.projectId, participant.participantId));
              } else {
                  if (participant.participantId) {
                      defer.resolve(obj.validateCreateSevis(participant.projectId, participant.participantId));
                  }
              }

              return defer.promise;
          }
      }

      // pre-sevis create validation
      obj.validateCreateSevis = function (projectId, participantid) {
          return ParticipantPersonsSevisService.validateParticipantPersonsCreateSevis(projectId, participantid)
          .then(function (response) {
              $log.info('Validated participant create SEVIS information');
              var verifyResult = response.data;
              // log and update participant sevis validation results
              return ParticipantPersonsSevisService.createParticipantSevisCommStatus(projectId, participantid, verifyResult)
                .then(function (response) {
                    return obj.updateSevisInfo(projectId, participantid, verifyResult)
                })
          })
          .catch(function () {
              $log.error("Unable to validate participant create SEVIS information.");
          });
      }

      // pre-sevis update validation
      obj.validateUpdateSevis = function (projectId, participantid) {
          return ParticipantPersonsSevisService.validateParticipantPersonsUpdateSevis(projectId, participantid)
            .then(function (response) {
                $log.info('Validated participant update SEVIS information');
                var verifyResult = response.data;
                // log and update participant sevis validation results
                return ParticipantPersonsSevisService.createParticipantSevisCommStatus(projectId, participantid, verifyResult)
                .then(function (response) {
                    return obj.updateSevisInfo(projectId, participantid, verifyResult)
                })
            })
            .catch(function () {
                $log.error("Unable to validate participant update SEVIS information.");
            });
      }

      // get participant record and attach validation results
      obj.updateSevisInfo = function (projectId, participantId, validationResults) {
          return ParticipantPersonsSevisService.getParticipantPersonsSevisById(projectId, participantId)
          .then(function (data) {
              var sevisInfo = data.data;
              if (sevisInfo) {
                  sevisInfo.sevisValidationResult = JSON.stringify(validationResults);
                  return obj.saveSevisInfo(projectId, participantId, sevisInfo);
              }
          })
          .catch(function () {
              $log.error('Unable to load participant SEVIS information.');
          });
      }

      // update participant sevis results
      obj.saveSevisInfo = function (projectId, participantId, updatedSevisInfo) {
          return ParticipantPersonsSevisService.updateParticipantPersonsSevis(projectId, updatedSevisInfo)
          .then(function (data) {
              $log.info('Participant SEVIS verification results saved successfully.');
              return updatedSevisInfo;
          })
          .catch(function () {
              $log.error('Unable to save participant SEVIS verification results');
          });
      }

      return obj;
  });
