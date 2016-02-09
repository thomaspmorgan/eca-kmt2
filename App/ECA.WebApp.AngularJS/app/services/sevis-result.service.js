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
                      return obj.validateUpdateSevis(participant.data.participantId);
                  } else {
                      if (participant.data.participantId) {
                          defer.resolve(obj.validateCreateSevis(participant.data.participantId));
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
                  defer.resolve(obj.validateUpdateSevis(participant.participantId));
              } else {
                  if (participant.participantId) {
                      defer.resolve(obj.validateCreateSevis(participant.participantId));
                  }
              }

              return defer.promise;
          }          
      }

      // pre-sevis create validation
      obj.validateCreateSevis = function (participantid) {
          var defer = $q.defer();
          ParticipantPersonsSevisService.validateParticipantPersonsCreateSevis(participantid)
          .then(function (response) {
              $log.info('Validated participant create SEVIS information');
              // log participant sevis validation attempt
              ParticipantPersonsSevisService.createParticipantSevisCommStatus(participantid, response.data);
              // update participant sevis validation results
              defer.resolve(obj.updateSevisInfo(participantid, response.data));
          })
          .catch(function () {
              $log.error("Unable to validate participant create SEVIS information.");
          });

          return defer.promise;
      }

      // pre-sevis update validation
      obj.validateUpdateSevis = function (participantid) {
          var defer = $q.defer();
          ParticipantPersonsSevisService.validateParticipantPersonsUpdateSevis(participantid)
            .then(function (response) {
                $log.info('Validated participant update SEVIS information');
                // log participant sevis validation attempt
                ParticipantPersonsSevisService.createParticipantSevisCommStatus(participantid, response.data);
                // update participant sevis validation results
                defer.resolve(obj.updateSevisInfo(participantid, response.data));
            })
            .catch(function () {
                $log.error("Unable to validate participant update SEVIS information.");
            });

          return defer.promise;
      }

      // get participant record and attach validation results
      obj.updateSevisInfo = function (participantId, validationResults) {
          var defer = $q.defer();
          ParticipantPersonsSevisService.getParticipantPersonsSevisById(participantId)
          .then(function (data) {
              var sevisInfo = data.data;
              if (sevisInfo) {
                  sevisInfo.sevisValidationResult = JSON.stringify(validationResults);
                  defer.resolve(obj.saveSevisInfo(participantId, sevisInfo));
              }
          })
          .catch(function () {
              $log.error('Unable to load participant SEVIS information.');
          });

          return defer.promise;
      }

      // update participant sevis results
      obj.saveSevisInfo = function (participantId, updatedSevisInfo) {
          var defer = $q.defer();
          ParticipantPersonsSevisService.updateParticipantPersonsSevis(updatedSevisInfo)
          .then(function (data) {
              $log.info('Participant SEVIS verification results saved successfully.');
              defer.resolve(updatedSevisInfo);
          })
          .catch(function () {
              $log.error('Unable to save participant SEVIS verification results');
          });

          return defer.promise;
      }
      
      return obj;
  });
