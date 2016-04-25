using ECA.Business.Queries.Admin;
using ECA.Business.Queries.Models.Persons;
using ECA.Business.Queries.Models.Persons.ExchangeVisitor;
using ECA.Business.Service.Admin;
using ECA.Business.Validation.Sevis.Bio;
using ECA.Business.Validation.Sevis.Finance;
using ECA.Data;
using System;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ECA.Business.Queries.Persons
{
    /// <summary>
    /// The ExchangeVisitorQueries are provided to convert ECA KMT data in sevis compatible data.
    /// </summary>
    public static class ExchangeVisitorQueries
    {
        /// <summary>
        /// Returns a query to get biographical information about a participant as it relates to sevis.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to get biographical information about a participant.</returns>
        public static IQueryable<BiographicalDTO> CreateGetBiographicalDataQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");

            var emailAddressQuery = EmailAddressQueries.CreateGetEmailAddressDTOQuery(context);
            var phoneNumberQuery = PhoneNumberQueries.CreateGetPhoneNumberDTOQuery(context);
            var addressQuery = AddressQueries.CreateGetAddressDTOQuery(context);
            var cityLocationTypeId = LocationType.City.Id;
            var maleGenderCode = Gender.SEVIS_MALE_GENDER_CODE_VALUE;
            var femaleGenderCode = Gender.SEVIS_FEMALE_GENDER_CODE_VALUE;
            var cityMaxLength = PersonValidator.CITY_MAX_LENGTH;
            var unitedStatesCountryName = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            var hostAddressTypeId = AddressType.Host.Id;
            var homeAddressTypeId = AddressType.Home.Id;
            var visitingPhoneNumberTypeId = Data.PhoneNumberType.Visiting.Id;
            var personalEmailTypeId = EmailAddressType.Personal.Id;
            var query = from person in context.People

                        let gender = person.Gender
                        let sevisGender = (gender.SevisGenderCode == maleGenderCode || gender.SevisGenderCode == femaleGenderCode) ? gender.SevisGenderCode : null

                        let numberOfCitizenships = person.CountriesOfCitizenship.Count()
                        let countryOfCitizenship = person.CountriesOfCitizenship.FirstOrDefault()
                        let sevisCountryOfCitizenship = countryOfCitizenship != null ? countryOfCitizenship.BirthCountry : null
                        let sevisCountryOfCitizenshipCode = sevisCountryOfCitizenship != null ? sevisCountryOfCitizenship.CountryCode : null

                        let hasPlaceOfBirth = person.PlaceOfBirthId.HasValue && person.PlaceOfBirth.LocationTypeId == cityLocationTypeId
                        let placeOfBirth = hasPlaceOfBirth ? person.PlaceOfBirth : null
                        let birthCity = hasPlaceOfBirth && placeOfBirth.LocationName != null
                            ? placeOfBirth.LocationName.Length > cityMaxLength
                                ? placeOfBirth.LocationName.Substring(0, cityMaxLength)
                                : placeOfBirth.LocationName
                            : null

                        let hasCountryOfBirth = hasPlaceOfBirth && placeOfBirth.CountryId.HasValue
                        let countryOfBirth = hasCountryOfBirth ? placeOfBirth.Country : null

                        let sevisCountryOfBirth = (hasCountryOfBirth && countryOfBirth.BirthCountryId.HasValue) ? countryOfBirth.BirthCountry : null

                        let emailAddress = emailAddressQuery
                            .Where(x => x.PersonId.HasValue && x.PersonId == person.PersonId)
                            .Where(x => x.EmailAddressTypeId == personalEmailTypeId)
                            .OrderByDescending(x => x.IsPrimary)
                            .FirstOrDefault()

                        let phoneNumber = phoneNumberQuery
                            .Where(x => x.PersonId.HasValue && x.PersonId == person.PersonId)
                            .Where(x => x.PhoneNumberTypeId == visitingPhoneNumberTypeId)
                            .OrderByDescending(x => x.IsPrimary)
                            .FirstOrDefault()

                        let residenceAddressQuery = addressQuery
                            .Where(x => x.PersonId.HasValue && x.PersonId == person.PersonId)
                            .Where(x => x.Country != unitedStatesCountryName)
                            .Where(x => x.AddressTypeId == homeAddressTypeId)

                        let residenceAddressesCount = addressQuery
                            .Where(x => x.PersonId.HasValue && x.PersonId == person.PersonId)
                            .Where(x => x.AddressTypeId == homeAddressTypeId)
                            .Count()
                        let residenceAddress = residenceAddressesCount == 1 ? residenceAddressQuery.FirstOrDefault() : null

                        let residenceCountry = residenceAddress != null ? context.Locations.Where(x => x.LocationId == residenceAddress.CountryId).FirstOrDefault() : null
                        let residenceSevisCountry = residenceCountry != null ? residenceCountry.BirthCountry : null
                        let residenceSevisCountryCode = residenceSevisCountry != null ? residenceSevisCountry.CountryCode : null

                        let mailAddress = addressQuery
                            .Where(x => x.PersonId.HasValue && x.PersonId == person.PersonId)
                            .Where(x => x.Country == unitedStatesCountryName)
                            .Where(x => x.AddressTypeId == hostAddressTypeId)
                            .OrderByDescending(x => x.IsPrimary)
                            .FirstOrDefault()

                        select new BiographicalDTO
                        {
                            NumberOfCitizenships = numberOfCitizenships,
                            PersonId = person.PersonId,
                            EmailAddressId = emailAddress != null ? emailAddress.Id : default(int?),
                            PermanentResidenceAddressId = residenceAddress != null ? residenceAddress.AddressId : default(int?),
                            PhoneNumberId = phoneNumber != null ? phoneNumber.Id : default(int?),
                            GenderId = gender.GenderId,
                            FullName = new FullNameDTO
                            {
                                FirstName = person.FirstName != null && person.FirstName.Trim().Length > 0 ? person.FirstName.Trim() : null,
                                LastName = person.LastName != null && person.LastName.Trim().Length > 0 ? person.LastName.Trim() : null,
                                Suffix = person.NameSuffix != null && person.NameSuffix.Trim().Length > 0 ? person.NameSuffix.Trim() : null,
                                MiddleName = person.MiddleName != null && person.MiddleName.Trim().Length > 0 ? person.MiddleName.Trim() : null,
                                PreferredName = person.Alias != null && person.Alias.Trim().Length > 0 ? person.Alias.Trim() : null,
                            },
                            BirthDate = person.DateOfBirth.HasValue
                                && (!person.IsDateOfBirthEstimated.HasValue || !person.IsDateOfBirthEstimated.Value)
                                ? person.DateOfBirth : null,
                            Gender = gender != null && sevisGender != null ? sevisGender : null,
                            BirthCity = birthCity,
                            BirthCountryCode = hasCountryOfBirth ? sevisCountryOfBirth.CountryCode : null,
                            CitizenshipCountryCode = numberOfCitizenships == 1 ? sevisCountryOfCitizenshipCode : null,
                            BirthCountryReasonId = null,
                            BirthCountryReasonCode = null,
                            EmailAddress = emailAddress != null ? emailAddress.Address : null,
                            PermanentResidenceCountryCode = residenceSevisCountryCode,
                            PhoneNumber = phoneNumber != null
                                ? phoneNumber.Number
                                    .Replace(" ", string.Empty)
                                    .Replace("-", string.Empty)
                                    .Replace("+", string.Empty)
                                    .Replace("(", string.Empty)
                                    .Replace(")", string.Empty)
                                : null,
                            MailAddress = mailAddress,
                        };
            return query;
        }

        /// <summary>
        /// Returns a query to get biographical information about a participant as it relates to sevis.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="participantId">the participant by id.</param>
        /// <returns>The query to get biographical information about a participant.</returns>
        public static IQueryable<BiographicalDTO> CreateGetBiographicalDataByParticipantIdQuery(EcaContext context, int participantId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = from biography in CreateGetBiographicalDataQuery(context)
                        join participant in context.Participants
                        on biography.PersonId equals participant.PersonId
                        where participant.ParticipantId == participantId
                        && participant.PersonId.HasValue
                        select biography;
            return query;
        }

        /// <summary>
        /// Returns a query to get biographical data for the dependents of the participant with the given id.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="participantId">The participant id.</param>
        /// <returns>The query to get biographical data of a participant's dependents.</returns>
        public static IQueryable<DependentBiographicalDTO> CreateGetParticipantDependentsBiographicalQuery(EcaContext context, int participantId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var maleGenderCode = Gender.SEVIS_MALE_GENDER_CODE_VALUE;
            var femaleGenderCode = Gender.SEVIS_FEMALE_GENDER_CODE_VALUE;

            var familyMemberIds = context.Participants
                .Where(x => x.ParticipantId == participantId && x.PersonId.HasValue)
                .SelectMany(x => x.Person.Family.Select(f => f.DependentId));

            var locationsQuery = LocationQueries.CreateGetLocationsQuery(context);
            var emailAddressQuery = EmailAddressQueries.CreateGetEmailAddressDTOQuery(context);

            var query = from dependent in context.PersonDependents

                        let gender = context.Genders.Where(x => x.GenderId == dependent.GenderId).FirstOrDefault()
                        let sevisGender = gender != null && (gender.SevisGenderCode == maleGenderCode || gender.SevisGenderCode == femaleGenderCode) ? gender.SevisGenderCode : null

                        let residenceCountry = context.Locations.Where(x => x.LocationId == dependent.PlaceOfResidenceId).FirstOrDefault()
                        let residenceSevisCountry = residenceCountry != null ? residenceCountry.BirthCountry : null
                        let residenceSevisCountryCode = residenceSevisCountry != null ? residenceSevisCountry.CountryCode : null

                        let birthCity = context.Locations.Where(x => x.LocationId == dependent.PlaceOfBirthId).FirstOrDefault()
                        let birthCountry = birthCity != null ? birthCity.Country : null
                        let sevisBirthCountry = birthCountry != null ? birthCountry.BirthCountry : null
                        let sevisBirthCountryCode = sevisBirthCountry != null ? sevisBirthCountry.CountryCode : null

                        let birthDate = dependent.DateOfBirth

                        let numberOfCitizenships = dependent.CountriesOfCitizenship.Count()
                        let countryOfCitizenship = dependent.CountriesOfCitizenship.OrderByDescending(x => x.IsPrimary).FirstOrDefault()
                        let sevisCountryOfCitizenship = countryOfCitizenship != null ? countryOfCitizenship.Location : null
                        let sevisCountryOfCitizenshipCode = sevisCountryOfCitizenship != null ? sevisCountryOfCitizenship.LocationIso : null

                        let relationship = context.DependentTypes.Where(x => x.DependentTypeId == dependent.DependentTypeId).FirstOrDefault()
                        let relationshipCode = relationship != null ? relationship.SevisDependentTypeCode : null

                        let emailAddress = dependent.EmailAddresses
                            .OrderByDescending(x => x.IsPrimary)
                            .FirstOrDefault()

                        let birthCountryReason = context.BirthCountryReasons.Where(x => x.BirthCountryReasonId == dependent.BirthCountryReasonId).FirstOrDefault()

                        where familyMemberIds.Contains(dependent.DependentId) && !dependent.IsSevisDeleted
                        select new DependentBiographicalDTO
                        {
                            PermanentResidenceAddressId = null,
                            BirthCity = birthCity.LocationName,
                            BirthCountryCode = sevisBirthCountryCode,
                            BirthCountryReasonId = dependent.BirthCountryReasonId,
                            BirthCountryReasonCode = birthCountryReason != null ? birthCountryReason.BirthReasonCode : null,
                            BirthDate = birthDate,
                            CitizenshipCountryCode = numberOfCitizenships == 1 ? sevisCountryOfCitizenshipCode : null,
                            EmailAddress = emailAddress != null ? emailAddress.Address : null,
                            EmailAddressId = emailAddress != null ? emailAddress.EmailAddressId : default(int?),
                            IsTravelingWithParticipant = dependent.IsTravellingWithParticipant,
                            IsDeleted = dependent.IsDeleted,
                            FullName = new FullNameDTO
                            {
                                FirstName = dependent.FirstName != null && dependent.FirstName.Trim().Length > 0 ? dependent.FirstName.Trim() : null,
                                LastName = dependent.LastName != null && dependent.LastName.Trim().Length > 0 ? dependent.LastName.Trim() : null,
                                Suffix = dependent.NameSuffix != null && dependent.NameSuffix.Trim().Length > 0 ? dependent.NameSuffix.Trim() : null,
                                PassportName = dependent.PassportName != null && dependent.PassportName.Trim().Length > 0 ? dependent.PassportName.Trim() : null,
                                PreferredName = dependent.PreferredName != null && dependent.PreferredName.Trim().Length > 0 ? dependent.PreferredName.Trim() : null,
                            },
                            Gender = gender != null && sevisGender != null ? sevisGender : null,
                            GenderId = gender.GenderId,
                            NumberOfCitizenships = numberOfCitizenships,
                            PermanentResidenceCountryCode = residenceSevisCountryCode,
                            PersonId = dependent.PersonId,
                            ParticipantId = participantId,
                            Relationship = relationshipCode,
                            DependentTypeId = relationship != null ? relationship.DependentTypeId : -1,
                            SevisId = dependent.SevisId
                        };
            return query;

        }

        /// <summary>
        /// Returns a query to get the subject field of a participant as it relates to sevis.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="participantId">The participant id.</param>
        /// <returns>The subject field query for sevis by participant id.</returns>
        public static IQueryable<SubjectFieldDTO> CreateGetSubjectFieldByParticipantIdQuery(EcaContext context, int participantId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = from participant in context.Participants

                        join exchangeVisitor in context.ParticipantExchangeVisitors
                        on participant.ParticipantId equals exchangeVisitor.ParticipantId

                        join fieldOfStudy in context.FieldOfStudies
                        on exchangeVisitor.FieldOfStudyId equals fieldOfStudy.FieldOfStudyId

                        where participant.ParticipantId == participantId
                        select new SubjectFieldDTO
                        {
                            SubjectFieldCode = fieldOfStudy.FieldOfStudyCode,
                            Remarks = fieldOfStudy.Description
                        };
            return query;
        }

        /// <summary>
        /// Returns a query to get the international funding of a participant for sevis.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="participantId">The participant id.</param>
        /// <returns>The query to get international funding as it relates to sevis.</returns>
        public static IQueryable<ExchangeVisitorFundingDTO> CreateGetInternationalFundingQuery(EcaContext context, int participantId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = from visitor in context.ParticipantExchangeVisitors

                        let firstInternationalFundingOrg = visitor.IntlOrg1
                        let secondIternationalFundingOrg = visitor.IntlOrg2

                        let hasFirstInternationalFunding = visitor.FundingIntlOrg1.HasValue && visitor.FundingIntlOrg1.Value > 0
                        let hasSecondInternationalFunding = visitor.FundingIntlOrg2.HasValue && visitor.FundingIntlOrg2.Value > 0

                        let otherName1 = firstInternationalFundingOrg == null
                            || (firstInternationalFundingOrg != null && firstInternationalFundingOrg.OrganizationCode == InternationalFundingValidator.OTHER_ORG_CODE)
                            ? visitor.IntlOrg1OtherName : null

                        let otherName2 = secondIternationalFundingOrg == null
                            || (secondIternationalFundingOrg != null && secondIternationalFundingOrg.OrganizationCode == InternationalFundingValidator.OTHER_ORG_CODE)
                            ? visitor.IntlOrg2OtherName : null

                        where visitor.ParticipantId == participantId

                        select new ExchangeVisitorFundingDTO
                        {
                            Amount1 = hasFirstInternationalFunding ? visitor.FundingIntlOrg1 : null,
                            Org1 = firstInternationalFundingOrg != null && hasFirstInternationalFunding ? firstInternationalFundingOrg.OrganizationCode : null,
                            OtherName1 = otherName1,

                            Amount2 = hasSecondInternationalFunding && hasSecondInternationalFunding ? visitor.FundingIntlOrg2 : null,
                            Org2 = secondIternationalFundingOrg != null && hasSecondInternationalFunding ? secondIternationalFundingOrg.OrganizationCode : null,
                            OtherName2 = otherName2
                        };

            return query;
        }

        /// <summary>
        /// Returns a query to get the us government funding of a participant for sevis.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="participantId">The participant id.</param>
        /// <returns>The query to get us government funding as it relates to sevis.</returns>
        public static IQueryable<ExchangeVisitorFundingDTO> CreateGetUSFundingQuery(EcaContext context, int participantId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = from visitor in context.ParticipantExchangeVisitors

                        let firstUsGovFundingAgency = visitor.GovtAgency1
                        let secondUsGovFundingAgency = visitor.GovtAgency2

                        let hasFirstUsGovFunding = visitor.FundingGovtAgency1.HasValue && visitor.FundingGovtAgency1.Value > 0
                        let hasSecondUsGovFunding = visitor.FundingGovtAgency2.HasValue && visitor.FundingGovtAgency2.Value > 0

                        let otherName1 = firstUsGovFundingAgency == null
                            || (firstUsGovFundingAgency != null && firstUsGovFundingAgency.AgencyCode == USGovernmentFundingValidator.OTHER_ORG_CODE)
                            ? visitor.GovtAgency1OtherName : null

                        let otherName2 = secondUsGovFundingAgency == null
                            || (secondUsGovFundingAgency != null && secondUsGovFundingAgency.AgencyCode == USGovernmentFundingValidator.OTHER_ORG_CODE)
                            ? visitor.GovtAgency2OtherName : null

                        where visitor.ParticipantId == participantId

                        select new ExchangeVisitorFundingDTO
                        {
                            Amount1 = hasFirstUsGovFunding ? visitor.FundingGovtAgency1 : null,
                            Org1 = firstUsGovFundingAgency != null && hasFirstUsGovFunding ? firstUsGovFundingAgency.AgencyCode : null,
                            OtherName1 = otherName1,

                            Amount2 = hasSecondUsGovFunding ? visitor.FundingGovtAgency2 : null,
                            Org2 = secondUsGovFundingAgency != null && hasSecondUsGovFunding ? secondUsGovFundingAgency.AgencyCode : null,
                            OtherName2 = otherName2
                        };

            return query;
        }

        /// <summary>
        /// Returns a query that retrieves dtos detailing participants that have a sevis id, whos start date is before the given start date and have not yet been validated by batch.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="startDate">The minimum start date for a participant.</param>
        /// <returns>The query of dtos who are ready to be validated.</returns>
        public static IQueryable<ReadyToValidateParticipantDTO> CreateGetReadyToValidateParticipantDTOsQuery(EcaContext context, DateTimeOffset startDate)
        {

            var query = from participantPerson in context.ParticipantPersons
                        let participant = participantPerson.Participant
                        let hasValidatedByBatchStatus = participantPerson.ParticipantPersonSevisCommStatuses
                            .Where(x => x.SevisCommStatusId == SevisCommStatus.ReadyToValidate.Id || x.SevisCommStatusId == SevisCommStatus.NeedsValidationInfo.Id)
                            .Count() > 0

                        where participantPerson.SevisId != null
                        && participantPerson.SevisId.Length > 0
                        && participantPerson.StartDate.HasValue
                        && participantPerson.StartDate.Value < startDate
                        && !hasValidatedByBatchStatus
                        select new ReadyToValidateParticipantDTO
                        {
                            ParticipantId = participant.ParticipantId,
                            ProjectId = participant.ProjectId,
                            SevisId = participantPerson.SevisId
                        };
            return query;
        }
    }
}
