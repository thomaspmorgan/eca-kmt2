angular.module('staticApp').factory('ConstantsService', function() {
	return {
		locationType: {
			region: 2,
			country: 3,
			state: 4,
			city: 5,
			building: 6,
			post: 7,
			place: 8,
			address: 9
		},
		organizationType: {
			office: 1,
			branch: 2,
			division: 3,
			foreignEducationalInstitution: 4,
			foreignGovernment: 5,
			foreignNgoPvo: 6,
			other: 7,
			publicInternationalOrganizationPio: 8,
			uSEducationalInstitution: 9,
			uSNonProfitOrganization501C3: 10,
			individual: 11
		},
		participantType: {
			foreignEducationalInstitution: 1,
			foreignGovernment: 2,
			uSEducationalInstitution: 3,
			publicInternationalOrganizationPio: 4,
			uSNonProfitOrganization501C3: 5,
			individual: 6,
			foreignNgoPvo: 7,
			other: 8,
			foreignNonTravelingParticipant: 9,
			uSNonTravelingParticipant: 10,
			foreignTravelingParticipant: 11,
			uSTravelingParticipant: 12
		},
		permission: {
			viewoffice: 1,
			viewprogram: 2,
			viewproject: 3,
			editoffice: 4,
			editprogram: 5,
			editproject: 7
		},
		projectStatus: {
			active: 1,
			pending: 2,
			completed: 4,
			draft: 5,
			canceled: 6,
			other: 7,
			activeUseActuals: 8,
			projectPostponed: 9,
			proposed: 10,
			rejected: 11
		},
		resourceType: {
			application: 1,
			office: 2,
			program: 3,
			project: 4
		},
		adalLoginSuccessEventName: 'adal:loginSuccess',
		editProjectEventName: 'EditProject',
		emptyGuid: '00000000-0000-0000-0000-000000000000',
		equalComparisonType: 'eq',
		greaterComparisonType: 'gt',
		inComparisonType: 'in',
		isNotNullComparisonType: 'notnull',
		isNullComparisonType: 'null',
		lessThanComparisonType: 'lt',
		likeComparisonType: 'like',
		logoutEventName: 'logout',
		notInComparisonType: 'notin',
		registeringUserEventName: 'registeringUser',
		registerUserFailureEventName: 'registerUserFailure',
		registerUserSuccessEventName: 'registerUserSuccess',
		searchDelimiter: ' '
	};
});

