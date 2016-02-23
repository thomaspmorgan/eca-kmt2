using ECA.Business.Queries.Admin;
using ECA.Business.Queries.Models.Persons;
using ECA.Business.Queries.Models.Persons.ExchangeVisitor;
using ECA.Business.Validation.Model;
using ECA.Business.Validation.Model.CreateEV;
using ECA.Business.Validation.Model.Shared;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Persons
{
    /// <summary>
    /// The ExchangeVisitorQueries are provided to convert ECA KMT data in sevis compatible data.
    /// </summary>
    public static class ExchangeVisitorQueries
    {
        #region Create Exchange Visitor
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

            var query = from participant in context.Participants
                        let participantPerson = participant.ParticipantPerson
                        let person = participant.Person

                        let gender = person.Gender

                        //right now assuming one country of citizenship, this should be checked by business layer
                        let countryOfCitizenship = person.CountriesOfCitizenship.FirstOrDefault()
                        let sevisCountryOfCitizenship = countryOfCitizenship != null ? countryOfCitizenship.BirthCountry : null
                        let sevisCountryOfCitizenshipCode = sevisCountryOfCitizenship != null ? sevisCountryOfCitizenship.CountryCode : null

                        let hasPlaceOfBirth = person.PlaceOfBirthId.HasValue
                        let placeOfBirth = hasPlaceOfBirth ? person.PlaceOfBirth : null

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
                            .OrderByDescending(x => x.IsPrimary)
                            .FirstOrDefault()
                        let residenceCountry = residenceAddress != null ? context.Locations.Where(x => x.LocationId == residenceAddress.CountryId).FirstOrDefault() : null
                        let residenceSevisCountry = residenceCountry != null ? residenceCountry.BirthCountry : null
                        let residenceSevisCounryCode = residenceSevisCountry != null ? residenceSevisCountry.CountryCode : null

                        where participant.PersonId.HasValue

                        select new BiographicalDTO
                        {
                            ParticipantId = participant.ParticipantId,
                            ProjectId = participant.ProjectId,
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
                            BirthDate = person.DateOfBirth,
                            Gender = gender != null ? gender.SevisGenderCode : null,
                            BirthCity = hasPlaceOfBirth ? placeOfBirth.LocationName : null,
                            BirthCountryCode = hasCountryOfBirth ? sevisCountryOfBirth.CountryCode : null,
                            CitizenshipCountryCode = sevisCountryOfCitizenshipCode,
                            BirthCountryReason = null,
                            EmailAddress = emailAddress != null ? emailAddress.Address : null,
                            PermanentResidenceCountryCode = residenceSevisCounryCode,
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
            return CreateGetBiographicalDataQuery(context).Where(x => x.ParticipantId == participantId);
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

                        where visitor.ParticipantId == participantId

                        select new International
                        {
                            Amount1 = firstInternationalFundingOrg != null && visitor.FundingIntlOrg1.HasValue ? ((int)visitor.FundingIntlOrg1).ToString() : null,
                            Org1 = firstInternationalFundingOrg != null ? firstInternationalFundingOrg.OrganizationCode : null,
                            OtherName1 = firstInternationalFundingOrg != null && firstInternationalFundingOrg.OrganizationCode == null ? firstInternationalFundingOrg.Description : null,

                            Amount2 = secondIternationalFundingOrg != null && visitor.FundingIntlOrg2.HasValue ? ((int)visitor.FundingIntlOrg2).ToString() : null,
                            Org2 = secondIternationalFundingOrg != null ? secondIternationalFundingOrg.OrganizationCode : null,
                            OtherName2 = secondIternationalFundingOrg != null && secondIternationalFundingOrg.OrganizationCode == null ? secondIternationalFundingOrg.Description : null,
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

                        where visitor.ParticipantId == participantId

                        select new USGovt
                        {
                            Amount1 = firstUsGovFundingAgency != null && visitor.FundingGovtAgency1.HasValue ? ((int)visitor.FundingGovtAgency1).ToString() : null,
                            Org1 = firstUsGovFundingAgency != null ? firstUsGovFundingAgency.AgencyCode : null,
                            OtherName1 = firstUsGovFundingAgency != null && firstUsGovFundingAgency.AgencyCode == null ? firstUsGovFundingAgency.Description : null,

                            Amount2 = secondUsGovFundingAgency != null && visitor.FundingGovtAgency2.HasValue ? ((int)visitor.FundingGovtAgency2).ToString() : null,
                            Org2 = secondUsGovFundingAgency != null ? secondUsGovFundingAgency.AgencyCode : null,
                            OtherName2 = secondUsGovFundingAgency != null && secondUsGovFundingAgency.AgencyCode == null ? secondUsGovFundingAgency.Description : null,
                        };

            return query;
        }

        /// <summary>
        /// Returns a query to retrieve participants that can be sent through the sevis validation.  These person participants should have
        /// a valid sevis gender, a birthdate, and a city of birth, and a single country of citizenship.  They should belong to a project that is an exchange visitor project and they should be
        /// a traveling foreign participant.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IQueryable<ValidatableExchangeVisitorParticipantDTO> CreateGetValidatableParticipantsQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var exchangeVisitorTypeId = VisitorType.ExchangeVisitor.Id;
            var maleGenderCode = Gender.SEVIS_MALE_GENDER_CODE_VALUE;
            var femaleGenderCode = Gender.SEVIS_FEMALE_GENDER_CODE_VALUE;
            var foreignTravelingParticipantTypeId = ParticipantType.ForeignTravelingParticipant.Id;
            var cityLocationTypeId = LocationType.City.Id;

            var query = from participant in context.Participants
                        let project = participant.Project
                        let person = participant.Person
                        let gender = person.Gender
                        let countriesOfCitizenship = person.CountriesOfCitizenship
                        let placeOfBirth = person.PlaceOfBirth != null ? person.PlaceOfBirth : null
                        let placeOfBirthTypeId = placeOfBirth != null ? placeOfBirth.LocationTypeId : default(int?)

                        where project.VisitorTypeId == exchangeVisitorTypeId
                        && (gender.SevisGenderCode == maleGenderCode || gender.SevisGenderCode == femaleGenderCode)
                        && participant.PersonId.HasValue
                        && participant.ParticipantTypeId == foreignTravelingParticipantTypeId
                        && person.DateOfBirth.HasValue
                        && person.PlaceOfBirthId.HasValue
                        && countriesOfCitizenship.Count() == 1
                        && (placeOfBirthTypeId.HasValue && placeOfBirthTypeId.Value == cityLocationTypeId)
                        select new ValidatableExchangeVisitorParticipantDTO
                        {
                            ParticipantId = participant.ParticipantId,
                            ProjectId = project.ProjectId,
                            PersonId = person.PersonId
                        };
            return query;
        }

        /// <summary>
        /// Returns a query to retrieve participants that can be sent through the sevis validation.  These person participants should have
        /// a valid sevis gender, a birthdate, and a city of birth, and a single country of citizenship.  They should belong to a project that is an exchange visitor project and they should be
        /// a traveling foreign participant.
        /// </summary>
        /// <param name="participantIds">The participant ids to get validatable participants of.</param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IQueryable<ValidatableExchangeVisitorParticipantDTO> CreateGetValidatableParticipantsByParticipantIdsQuery(EcaContext context, IEnumerable<int> participantIds)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return CreateGetValidatableParticipantsQuery(context).Where(x => participantIds.Distinct().Contains(x.ParticipantId));
        }


        #endregion
    }
}
