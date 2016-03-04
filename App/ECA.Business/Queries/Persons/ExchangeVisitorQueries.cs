using ECA.Business.Queries.Admin;
using ECA.Business.Queries.Models.Persons;
using ECA.Business.Service.Admin;
using ECA.Business.Validation.Model;
using ECA.Business.Validation.Model.CreateEV;
using ECA.Business.Validation.Model.Shared;
using ECA.Data;
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
            var cityMaxLength = BiographicalValidator.CITY_MAX_LENGTH;
            var unitedStatesCountryName = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
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
                            .OrderByDescending(x => x.IsPrimary)
                            .FirstOrDefault()

                        let phoneNumber = phoneNumberQuery
                            .Where(x => x.PersonId.HasValue && x.PersonId == person.PersonId)
                            .OrderByDescending(x => x.IsPrimary)
                            .FirstOrDefault()

                        let residenceAddress = addressQuery
                            .Where(x => x.PersonId.HasValue && x.PersonId == person.PersonId)
                            .Where(x => x.Country != unitedStatesCountryName)
                            .OrderByDescending(x => x.IsPrimary)
                            .FirstOrDefault()
                        let residenceCountry = residenceAddress != null ? context.Locations.Where(x => x.LocationId == residenceAddress.CountryId).FirstOrDefault() : null
                        let residenceSevisCountry = residenceCountry != null ? residenceCountry.BirthCountry : null
                        let residenceSevisCountryCode = residenceSevisCountry != null ? residenceSevisCountry.CountryCode : null

                        select new BiographicalDTO
                        {
                            NumberOfCitizenships = numberOfCitizenships,
                            PersonId = person.PersonId,
                            EmailAddressId = emailAddress != null ? emailAddress.Id : default(int?),
                            AddressId = residenceAddress != null ? residenceAddress.AddressId : default(int?),
                            PhoneNumberId = phoneNumber != null ? phoneNumber.Id : default(int?),
                            GenderId = gender.GenderId,
                            FullName = new FullNameDTO
                            {
                                FirstName = person.FirstName,
                                LastName = person.LastName,
                                Suffix = person.NameSuffix,
                                PreferredName = person.Alias,
                            },
                            BirthDate = person.DateOfBirth.HasValue
                                && (!person.IsDateOfBirthEstimated.HasValue || !person.IsDateOfBirthEstimated.Value)
                                ? person.DateOfBirth : null,
                            Gender = gender != null && sevisGender != null ? sevisGender : null,
                            BirthCity = birthCity,
                            BirthCountryCode = hasCountryOfBirth ? sevisCountryOfBirth.CountryCode : null,
                            CitizenshipCountryCode = numberOfCitizenships == 1 ? sevisCountryOfCitizenshipCode : null,
                            BirthCountryReason = null,
                            EmailAddress = emailAddress != null ? emailAddress.Address : null,
                            PermanentResidenceCountryCode = residenceSevisCountryCode,
                            PhoneNumber = phoneNumber != null ? phoneNumber.Number : null
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
            var biographyQuery = CreateGetBiographicalDataQuery(context);
            var familyMemberIds = context.Participants
                .Where(x => x.ParticipantId == participantId && x.PersonId.HasValue)
                .SelectMany(x => x.Person.Family.Select(f => f.PersonId));

            return biographyQuery.Where(x => familyMemberIds.Contains(x.PersonId))
                .Select(b => new DependentBiographicalDTO
                {
                    AddressId = b.AddressId,
                    BirthCity = b.BirthCity,
                    BirthCountryCode = b.BirthCountryCode,
                    BirthCountryReason = b.BirthCountryReason,
                    BirthDate = b.BirthDate,
                    CitizenshipCountryCode = b.CitizenshipCountryCode,
                    EmailAddress = b.EmailAddress,
                    EmailAddressId = b.EmailAddressId,
                    FullName = b.FullName,
                    Gender = b.Gender,
                    GenderId = b.GenderId,
                    NumberOfCitizenships = b.NumberOfCitizenships,
                    PermanentResidenceCountryCode = b.PermanentResidenceCountryCode,
                    PersonId = b.PersonId,
                    PhoneNumber = b.PhoneNumber,
                    PhoneNumberId = b.PhoneNumberId,
                    PositionCode = b.PositionCode,
                    Relationship = null //this will have to change
                });

        }

        /// <summary>
        /// Returns a query to get the subject field of a participant as it relates to sevis.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="participantId">The participant id.</param>
        /// <returns>The subject field query for sevis by participant id.</returns>
        public static IQueryable<SubjectField> CreateGetSubjectFieldByParticipantIdQuery(EcaContext context, int participantId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = from participant in context.Participants

                        join exchangeVisitor in context.ParticipantExchangeVisitors
                        on participant.ParticipantId equals exchangeVisitor.ParticipantId

                        join fieldOfStudy in context.FieldOfStudies
                        on exchangeVisitor.FieldOfStudyId equals fieldOfStudy.FieldOfStudyId

                        where participant.ParticipantId == participantId
                        select new SubjectField
                        {
                            SubjectFieldCode = fieldOfStudy.FieldOfStudyCode,
                            Remarks = fieldOfStudy.Description
                        };
            return query;
        }

        /// <summary>
        /// Returns a query to get a USAddress from an address in the context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="addressId">The address id.</param>
        /// <returns>The query to get the US address from an address with the given id.</returns>
        public static IQueryable<USAddress> CreateGetUsAddressByAddressIdQuery(EcaContext context, int addressId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var addressQuery = AddressQueries.CreateGetAddressDTOQuery(context);
            var query = from address in addressQuery
                        where address.AddressId == addressId
                        select new USAddress
                        {
                            Address1 = address.Street1,
                            Address2 = address.Street2,
                            City = address.City,
                            State = address.Division,
                            PostalCode = address.PostalCode
                        };
            return query;
        }

        /// <summary>
        /// Returns a query to get the international funding of a participant for sevis.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="participantId">The participant id.</param>
        /// <returns>The query to get international funding as it relates to sevis.</returns>
        public static IQueryable<International> CreateGetInternationalFundingQuery(EcaContext context, int participantId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = from visitor in context.ParticipantExchangeVisitors

                        let firstInternationalFundingOrg = visitor.IntlOrg1
                        let secondIternationalFundingOrg = visitor.IntlOrg2

                        let otherName1 = firstInternationalFundingOrg == null
                            || (firstInternationalFundingOrg != null && firstInternationalFundingOrg.OrganizationCode == InternationalValidator.OTHER_ORG_CODE)
                            ? visitor.IntlOrg1OtherName : null

                        let otherName2 = secondIternationalFundingOrg == null
                            || (secondIternationalFundingOrg != null && secondIternationalFundingOrg.OrganizationCode == InternationalValidator.OTHER_ORG_CODE)
                            ? visitor.IntlOrg2OtherName : null

                        where visitor.ParticipantId == participantId

                        select new International
                        {
                            Amount1 = firstInternationalFundingOrg != null && visitor.FundingIntlOrg1.HasValue ? ((int)visitor.FundingIntlOrg1).ToString() : null,
                            Org1 = firstInternationalFundingOrg != null ? firstInternationalFundingOrg.OrganizationCode : null,
                            OtherName1 = otherName1,

                            Amount2 = secondIternationalFundingOrg != null && visitor.FundingIntlOrg2.HasValue ? ((int)visitor.FundingIntlOrg2).ToString() : null,
                            Org2 = secondIternationalFundingOrg != null ? secondIternationalFundingOrg.OrganizationCode : null,
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
        public static IQueryable<USGovt> CreateGetUSFundingQuery(EcaContext context, int participantId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = from visitor in context.ParticipantExchangeVisitors

                        let firstUsGovFundingAgency = visitor.GovtAgency1
                        let secondUsGovFundingAgency = visitor.GovtAgency2

                        let otherName1 = firstUsGovFundingAgency == null
                            || (firstUsGovFundingAgency != null && firstUsGovFundingAgency.AgencyCode == USGovtValidator.OTHER_ORG_CODE)
                            ? visitor.GovtAgency1OtherName : null

                        let otherName2 = secondUsGovFundingAgency == null
                            || (secondUsGovFundingAgency != null && secondUsGovFundingAgency.AgencyCode == USGovtValidator.OTHER_ORG_CODE)
                            ? visitor.GovtAgency2OtherName : null

                        where visitor.ParticipantId == participantId

                        select new USGovt
                        {
                            Amount1 = firstUsGovFundingAgency != null && visitor.FundingGovtAgency1.HasValue ? ((int)visitor.FundingGovtAgency1).ToString() : null,
                            Org1 = firstUsGovFundingAgency != null ? firstUsGovFundingAgency.AgencyCode : null,
                            OtherName1 = otherName1,

                            Amount2 = secondUsGovFundingAgency != null && visitor.FundingGovtAgency2.HasValue ? ((int)visitor.FundingGovtAgency2).ToString() : null,
                            Org2 = secondUsGovFundingAgency != null ? secondUsGovFundingAgency.AgencyCode : null,
                            OtherName2 = otherName2
                        };

            return query;
        }
    }
}
