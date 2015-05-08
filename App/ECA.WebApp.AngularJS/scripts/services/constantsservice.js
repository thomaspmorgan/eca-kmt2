angular.module('staticApp').factory('ConstantsService', function() {
	return {
		locationType: {
			region: {id: 2, value: "Region"},
			country: {id: 3, value: "Country"},
			state: {id: 4, value: "State"},
			city: {id: 5, value: "City"},
			building: {id: 6, value: "Building"},
			post: {id: 7, value: "Post"},
			place: {id: 8, value: "Place"},
			address: {id: 9, value: "Address"}
		},
		organizationType: {
			office: {id: 1, value: "Office"},
			branch: {id: 2, value: "Branch"},
			division: {id: 3, value: "Division"},
			foreignEducationalInstitution: {id: 4, value: "Foreign Educational Institution"},
			foreignGovernment: {id: 5, value: "Foreign Government"},
			foreignNgoPvo: {id: 6, value: "Foreign NGO/PVO"},
			other: {id: 7, value: "Other"},
			publicInternationalOrganizationPio: {id: 8, value: "Public International Organization (PIO)"},
			uSEducationalInstitution: {id: 9, value: "U.S. Educational Institution"},
			uSNonProfitOrganization501C3: {id: 10, value: "U.S. Non-Profit Organization (501(c)(3))"},
			individual: {id: 11, value: "Individual"}
		},
		participantType: {
			foreignEducationalInstitution: {id: 1, value: "Foreign Educational Institution"},
			foreignGovernment: {id: 2, value: "Foreign Government"},
			uSEducationalInstitution: {id: 3, value: "U.S. Educational Institution"},
			publicInternationalOrganizationPio: {id: 4, value: "Public International Organization (PIO)"},
			uSNonProfitOrganization501C3: {id: 5, value: "U.S. Non-Profit Organization (501(c)(3))"},
			individual: {id: 6, value: "Individual"},
			foreignNgoPvo: {id: 7, value: "Foreign NGO/PVO"},
			other: {id: 8, value: "Other"},
			foreignNonTravelingParticipant: {id: 9, value: "Foreign Non Traveling Participant"},
			uSNonTravelingParticipant: {id: 10, value: "U.S. Non Traveling Participant"},
			foreignTravelingParticipant: {id: 11, value: "Foreign Traveling Participant"},
			uSTravelingParticipant: {id: 12, value: "U.S. Traveling Participant"}
		},
		permission: {
			viewoffice: {id: 1, value: "ViewOffice"},
			viewprogram: {id: 2, value: "ViewProgram"},
			viewproject: {id: 3, value: "ViewProject"},
			editoffice: {id: 4, value: "EditOffice"},
			editprogram: {id: 5, value: "EditProgram"},
			editproject: {id: 7, value: "EditProject"}
		},
		projectStatus: {
			active: {id: 1, value: "Active"},
			pending: {id: 2, value: "Pending"},
			completed: {id: 4, value: "Completed"},
			draft: {id: 5, value: "Draft"},
			canceled: {id: 6, value: "Canceled"},
			other: {id: 7, value: "Other"},
			activeUseActuals: {id: 8, value: "Active - Use Actuals"},
			projectPostponed: {id: 9, value: "Project Postponed"},
			proposed: {id: 10, value: "Proposed"},
			rejected: {id: 11, value: "Rejected"}
		},
		resourceType: {
			application: {id: 1, value: "Application"},
			office: {id: 2, value: "Office"},
			program: {id: 3, value: "Program"},
			project: {id: 4, value: "Project"}
		},
		adalLoginSuccessEventName: 'adal:loginSuccess',
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
		officeCategorySettingName: 'Category',
		officeFocusSettingName: 'Focus',
		officeJustificationSettingName: 'Justification',
		officeObjectiveSettingName: 'Objective',
		registeringUserEventName: 'registeringUser',
		registerUserFailureEventName: 'registerUserFailure',
		registerUserSuccessEventName: 'registerUserSuccess',
		searchDelimiter: ' '
	};
});

