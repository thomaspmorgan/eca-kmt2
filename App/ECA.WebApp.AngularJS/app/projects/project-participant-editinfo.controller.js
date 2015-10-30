'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ProjectOverviewCtrl
 * @description The overview controller is used on the overview tab of a project.
 * # ProjectOverviewCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ProjectParticipantEditInfoCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        $modal,
        $state,
		FilterService,
		LookupService,
        OrganizationService,
        PersonService,
        ConstantsService,
        AuthService,
        ProjectService,
        NotificationService,
        ParticipantPersonsService
        ) {

  	$scope.view = {};
  	$scope.view.participantTypes = [];
  	
  	var projectId = $stateParams.projectId;
  	var limit = 300;

  	var participantTypesFilter = FilterService.add('project-participant-editinfo-partipanttypes');
  	participantTypesFilter = participantTypesFilter.isTrue('isPerson').skip(0).take(limit);
  	function loadParticipantTypes() {
  		return LookupService.getParticipantTypes(participantTypesFilter.toParams())
		.then(function (response) {
			if (response.data.total > limit) {
				var message = "The number of participant types loaded is less than the total number.  Some participant types may not be shown."
				NotificationService.showErrorMessage(message);
				$log.error(message);
			}
			$scope.view.participantTypes = response.data.results;
			return $scope.view.participantTypes;
		})
		.catch(function (response) {
			var message = "Unable to load participant types.";
			$log.error(message);
			NotificationService.showErrorMessage(message);
		});
  	}

  	$scope.view.isLoading = true;
  	$q.all([loadParticipantTypes()])
  	.then(function (results) {

  	}, function (errorResponse) {
  		$log.error('Failed initial loading of participant info view.');
  	})
  	.then(function () {
  		$scope.view.isLoading = false;
  	});
  });
