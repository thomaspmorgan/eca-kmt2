'use strict';

/**
 * @ngdoc service
 * @name staticApp.SevisResult
 * @description
 * # SevisResult
 * Factory for handling social media.
 */
angular.module('staticApp')
  .factory('SevisResultService', function ($log, ParticipantPersonsSevisService, PersonService) {

      var obj = {};

      // update the sevis verification result for a participant
      obj.updateSevisVerificationResults = function (personid) {
          return PersonService.getParticipantByPersonId(personid)
          .then(function (participant) {
              return obj.validateSevisInfo(participant.data);
          })
          .catch(function () {
              $log.error("Unable to retrieve participant by person id.");
          });
      }

      // initiate pre-sevis validation
      obj.validateSevisInfo = function (participant) {
          if (participant.sevisId && participant.participantId) {
              return obj.validateUpdateSevisInfo(participant.participantId);
          } else {
              if (participant.participantId) {
                  return obj.validateCreateSevisInfo(participant.participantId);
              }
          }
      }

      // pre-sevis create validation
      obj.validateCreateSevisInfo = function (participantid) {
          return ParticipantPersonsSevisService.validateParticipantPersonsCreateSevis(participantid)
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

      // pre-sevis update validation
      obj.validateUpdateSevisInfo = function (participantid) {
          return ParticipantPersonsSevisService.validateParticipantPersonsUpdateSevis(participantid)
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

      // get participant record and attach validation results
      obj.updateSevisInfo = function (participantId, validationResults) {
          return ParticipantPersonsSevisService.getParticipantPersonsSevisById(participantId)
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
          return ParticipantPersonsSevisService.updateParticipantPersonsSevis(updatedSevisInfo)
          .then(function (data) {
              $log.info('Participant SEVIS verification results saved successfully.');
          })
          .catch(function () {
              $log.error('Unable to save participant SEVIS verification results');
          });
      }

      return obj;
  });
