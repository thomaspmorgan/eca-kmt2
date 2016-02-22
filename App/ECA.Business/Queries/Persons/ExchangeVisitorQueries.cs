using ECA.Business.Queries.Admin;
using ECA.Business.Queries.Models.Persons;
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
            var query = from participant in context.Participants
                        let participantPerson = participant.ParticipantPerson
                        let person = participant.Person

                        let gender = person.Gender

                        //right now assuming one country of citizenship, this should be checked by business layer
                        let countryOfCitizenship = person.CountriesOfCitizenship.FirstOrDefault()

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

                        let hostInsitutionAddressCountry = participantPerson.HostInstitutionAddress != null
                            && participantPerson.HostInstitutionAddress.Location != null
                            && participantPerson.HostInstitutionAddress.Location.Country != null
                            ? participantPerson.HostInstitutionAddress.Location.Country
                            : null

                        where participant.PersonId.HasValue

                        select new BiographicalDTO
                        {
                            ParticipantId = participant.ParticipantId,
                            ProjectId = participant.ProjectId,
                            PersonId = person.PersonId,
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
                            CitizenshipCountryCode = countryOfCitizenship != null ? countryOfCitizenship.LocationIso2 : null,
                            BirthCountryReason = null,
                            EmailAddress = emailAddress != null ? emailAddress.Address : null,
                            PermanentResidenceCountryCode = hostInsitutionAddressCountry != null
                                ? hostInsitutionAddressCountry.LocationIso2
                                : null,
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

        #endregion
    }
}
