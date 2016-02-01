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
          var defer = $q.defer();
          PersonService.getParticipantByPersonId(personid)
          .then(function (participant) {
              if (participant.data.sevisId) {
                  defer.resolve(obj.validateUpdateSevis(participant.data.participantId));
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
      
      obj.updateSevisVerificationResultsByParticipant = function (participant) {
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

      // pre-sevis update validation
      obj.validateUpdateSevis = function (participantid) {
          ParticipantPersonsSevisService.validateParticipantPersonsUpdateSevis(participantid)
            .then(function (response) {
                $log.info('Validated participant update SEVIS information');
                var valErrors = [];
                for (var i = 0; i < response.data.errors.length; i++) {
                    valErrors.push({ msg: response.data.errors[i].errorMessage, path: response.data.errors[i].customState });
                }
                // log participant sevis validation attempt
                ParticipantPersonsSevisService.createParticipantSevisCommStatus(participantid, response.data);
                // update participant sevis validation results
                obj.updateSevisInfo(participantid, response.data);

                return valErrors;
            })
            .catch(function () {
                $log.error("Unable to validate participant create SEVIS information.");
            });
      }
      
      // pre-sevis create validation
      obj.validateCreateSevis = function (participantid) {
          ParticipantPersonsSevisService.validateParticipantPersonsCreateSevis(participantid)
          .then(function (response) {
              $log.info('Validated participant create SEVIS information');
              var valErrors = [];
              for (var i = 0; i < response.data.errors.length; i++) {
                  valErrors.push({ msg: response.data.errors[i].errorMessage, path: response.data.errors[i].customState });
              }
              // log participant sevis validation attempt
              ParticipantPersonsSevisService.createParticipantSevisCommStatus(participantid, response.data);
              // update participant sevis validation results
              obj.updateSevisInfo(participantid, response.data);

              return valErrors;
          })
          .catch(function () {
              $log.error("Unable to validate participant create SEVIS information.");
          });
      }

      // get participant record and attach validation results
      obj.updateSevisInfo = function (participantId, validationResults) {
          ParticipantPersonsSevisService.getParticipantPersonsSevisById(participantId)
          .then(function (data) {
              var sevisInfo = data.data;
              if (sevisInfo) {
                  sevisInfo.sevisValidationResult = JSON.stringify(validationResults);
                  obj.saveSevisInfo(participantId, sevisInfo);
              }              
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
