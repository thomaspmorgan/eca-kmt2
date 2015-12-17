
















angular.module('staticApp').factory('ConstantsService', function() {
	return {
		dataPointCategory: {
			office: {id: 1, value: "Office"},
			program: {id: 2, value: "Program"},
			project: {id: 3, value: "Project"},
			person: {id: 4, value: "Person"}
		},
		dataPointProperty: {
			themes: {id: 1, value: "Themes"},
			goals: {id: 2, value: "Goals"},
			regions: {id: 3, value: "Regions"},
			categories: {id: 4, value: "Categories"},
			objectives: {id: 5, value: "Objectives"},
			locations: {id: 6, value: "Locations"},
			addresses: {id: 7, value: "Addresses"},
			emails: {id: 8, value: "Emails"},
			languageProficiencies: {id: 9, value: "Language Proficiencies"},
			phoneNumbers: {id: 10, value: "Phone Numbers"}
		},
		emailAddressType: {
			home: {id: 1, value: "Home"},
			homeEmergency: {id: 2, value: "Home Emergency"},
			host: {id: 3, value: "Host"},
			hostEmergency: {id: 4, value: "Host Emergency"},
			organization: {id: 5, value: "Organization"},
			personal: {id: 6, value: "Personal"},
			other: {id: 7, value: "Other"},
			business: {id: 8, value: "Business"},
			undetermined: {id: 9, value: "Undetermined"}
		},
		locationType: {
			region: {id: 2, value: "Region"},
			country: {id: 3, value: "Country"},
			division: {id: 4, value: "Division"},
			city: {id: 5, value: "City"},
			building: {id: 6, value: "Building"},
			post: {id: 7, value: "Post"},
			place: {id: 8, value: "Place"},
			address: {id: 9, value: "Address"}
		},
		moneyFlowSourceRecipientType: {
			organization: {id: 1, value: "Organization"},
			program: {id: 2, value: "Program"},
			project: {id: 3, value: "Project"},
			participant: {id: 4, value: "Participant"},
			travelStop: {id: 5, value: "Travel Stop"},
			accomodation: {id: 6, value: "Accomodation"},
			transportation: {id: 7, value: "Transportation"},
			expense: {id: 8, value: "Expense"},
			post: {id: 9, value: "Post"},
			office: {id: 10, value: "Office"}
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
			uSFederalGovernment: {id: 11, value: "U.S. Federal Government"},
			uSStateOrLocalGovernment: {id: 12, value: "U.S. State or Local Government"},
			internationalOrganization: {id: 13, value: "International Organization"},
			uSCompany: {id: 14, value: "U.S. Company"},
			foreignCompany: {id: 15, value: "Foreign Company"},
			bilateralCommission: {id: 16, value: "Bilateral Commission"},
			uSEmbassyAmericanSpace: {id: 17, value: "U.S. Embassy/American Space"}
		},
		participantType: {
			organizationalParticipant: {id: 1, value: "Organizational Participant"},
			individual: {id: 6, value: "Individual"},
			otherOrganization: {id: 8, value: "Other Organization"},
			other: {id: 9, value: "Other"},
			foreignNonTravelingParticipant: {id: 10, value: "Foreign Non Traveling Participant"},
			uSNonTravelingParticipant: {id: 11, value: "U.S. Non Traveling Participant"},
			foreignTravelingParticipant: {id: 12, value: "Foreign Traveling Participant"},
			uSTravelingParticipant: {id: 13, value: "U.S. Traveling Participant"}
		},
		permission: {
			viewOffice: {id: 1, value: "View Office"},
			viewProgram: {id: 2, value: "View Program"},
			viewProject: {id: 3, value: "View Project"},
			editOffice: {id: 4, value: "Edit Office"},
			editProgram: {id: 5, value: "Edit Program"},
			editProject: {id: 7, value: "Edit Project"},
			projectOwner: {id: 8, value: "Project Owner"},
			programOwner: {id: 9, value: "Program Owner"},
			officeOwner: {id: 10, value: "Office Owner"},
			administrator: {id: 11, value: "Administrator"},
			search: {id: 12, value: "Search"}
		},
		phoneNumberType: {
			home: {id: 1, value: "Home"},
			work: {id: 2, value: "Work"},
			cell: {id: 3, value: "Cell"}
		},
		programStatus: {
			active: {id: 1, value: "Active"},
			pending: {id: 2, value: "Pending"},
			completed: {id: 3, value: "Completed"},
			draft: {id: 4, value: "Draft"},
			canceled: {id: 5, value: "Canceled"},
			other: {id: 6, value: "Other"}
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
			rejected: {id: 11, value: "Rejected"},
			nominated: {id: 12, value: "Nominated"},
			transferred: {id: 13, value: "Transferred"},
			waitlisted: {id: 14, value: "Waitlisted"}
		},
		resourceType: {
			application: {id: 1, value: "Application"},
			office: {id: 2, value: "Office"},
			program: {id: 3, value: "Program"},
			project: {id: 4, value: "Project"}
		},
		socialMediaType: {
			facebook: {id: 1, value: "Facebook"},
			linkedin: {id: 2, value: "LinkedIn"},
			twitter: {id: 3, value: "Twitter"},
			weibo: {id: 4, value: "Weibo"},
			instagram: {id: 5, value: "Instagram"},
			pinterest: {id: 6, value: "Pinterest"},
			google: {id: 7, value: "Google+"},
			other: {id: 8, value: "Other"}
		},
		visitorType: {
			notApplicable: {id: 1, value: "Not Applicable"},
			exchangeVisitor: {id: 2, value: "Exchange Visitor"},
			studentVisitor: {id: 3, value: "Student Visitor"}
		},
		adalLoginSuccessEventName: 'adal:loginSuccess',
		cancelProgramChangesEventName: 'cancelProgramEdit',
		cancelProjectEventName: 'cancelProjectEdit',
		containsAnyComparisonType: 'containsany',
		emptyGuid: '00000000-0000-0000-0000-000000000000',
		equalComparisonType: 'eq',
		greaterThanComparisonType: 'gt',
		inComparisonType: 'in',
		isNotNullComparisonType: 'notnull',
		isNullComparisonType: 'null',
		kmtApplicationResourceId: '1',
		lessThanComparisonType: 'lt',
		likeComparisonType: 'like',
		logoutEventName: 'logout',
		maxNumericValue: '2147483647',
		notEqualComparisonType: 'not_eq',
		notInComparisonType: 'notin',
		officeCategorySettingName: 'Category',
		officeFocusSettingName: 'Focus',
		officeJustificationSettingName: 'Justification',
		officeObjectiveSettingName: 'Objective',
		primaryAddressChangedEventName: 'primaryAddressChanged',
		primaryLanguageProficiencyChangedEventName: 'primaryLanguageProficiencyChanged',
		registeringUserEventName: 'registeringUser',
		registerUserFailureEventName: 'registerUserFailure',
		registerUserSuccessEventName: 'registerUserSuccess',
		removeNewAddressEventName: 'removeNewAddress',
		removeNewEducationEventName: 'removeNewEducation',
		removeNewEmailAddressEventName: 'removeNewEmailAddress',
		removeNewEmploymentEventName: 'removeNewEmployment',
		removeNewEvaluationEventName: 'removeNewEvaluation',
		removeNewLanguageProficiencyEventName: 'removeNewLanguageProficiency',
		removeNewMembershipsEventName: 'removeNewMembership',
		removeNewSocialMediaEventName: 'removeNewSocialMedia',
		removewNewPhoneNumberEventName: 'removeNewPhoneNumber',
		saveProgramEventName: 'saveProgram',
		saveProjectEventName: 'saveProject',
		searchDelimiter: ' '
	};
});


