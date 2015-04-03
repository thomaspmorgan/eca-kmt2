angular.module('staticApp').factory('ConstantsService', function() {
	return {
		comparisonType: {
		},
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
			other: 8
		},
		projectStatus: {
			active: 1,
			pending: 2,
			completed: 4,
			draft: 5,
			canceled: 6,
			other: 7
		},
		searchDelimiter: ' ',
		emptyGuid: '00000000-0000-0000-0000-000000000000',
		editProjectEventName: 'EditProject',
		inComparisonType: 'in',
		likeComparisonType: 'like',
		lessThanComparisonType: 'lt',
		greaterComparisonType: 'gt',
		equalComparisonType: 'eq',
		isNullComparisonType: 'null',
		isNotNullComparisonType: 'notnull'
	};
});

