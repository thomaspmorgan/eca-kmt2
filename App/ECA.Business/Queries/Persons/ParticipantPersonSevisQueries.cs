using ECA.Business.Queries.Models.Persons;
using ECA.Core.DynamicLinq;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECA.Business.Queries.Models.Admin;

namespace ECA.Business.Queries.Persons
{
    /// <summary>
    /// The ParticipantQueries are used to query a DbContext for Participant information.
    /// </summary>
    public static class ParticipantPersonSevisQueries
    {
        /// <summary>
        /// Query to get a list of participant people with SEVIS information
        /// </summary>
        /// <param name="context">The context to query</param>
        /// <returns>List of participant people with SEVIS</returns>
        public static IQueryable<ParticipantPersonSevisDTO> CreateGetParticipantPersonSevisesDTOQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = (from p in context.ParticipantPersons
                         select new ParticipantPersonSevisDTO
                         {
                             ParticipantId = p.ParticipantId,
                             SevisId = p.SevisId,
                             ProjectId = p.Participant.ProjectId,
                             StudyProject = p.StudyProject,
                             FieldOfStudy = p.FieldOfStudy != null ? p.FieldOfStudy.Description : null,
                             ProgramCategory = p.ProgramCategory != null ? p.ProgramCategory.Description : null,
                             Position = p.Position != null ? p.Position.Description : null,
                             ParticipantType = p.Participant.ParticipantType != null ? p.Participant.ParticipantType.Name : null,
                             ParticipantStatus = p.Participant.Status != null ? p.Participant.Status.Status : null,
                             IsCancelled = p.IsCancelled,
                             IsDS2019Printed = p.IsDS2019Printed,
                             IsDS2019SentToTraveler = p.IsDS2019SentToTraveler,
                             IsNeedsUpdate = p.IsNeedsUpdate,
                             IsSentToSevisViaRTI = p.IsSentToSevisViaRTI,
                             IsValidatedViaRTI = p.IsValidatedViaRTI,
                             StartDate = p.StartDate,
                             EndDate = p.EndDate,
                             FundingGovtAgency1 = p.FundingGovtAgency1,
                             FundingGovtAgency2 = p.FundingGovtAgency2,
                             FundingIntlOrg1 = p.FundingIntlOrg1,
                             FundingIntlOrg2 = p.FundingIntlOrg2,
                             FundingOther = p.FundingOther,
                             FundingPersonal = p.FundingPersonal,
                             FundingSponsor = p.FundingSponsor,
                             FundingTotal = p.FundingTotal,
                             FundingVisBNC = p.FundingVisBNC,
                             FundingVisGovt = p.FundingVisGovt,
                             HomeInstitution = p.HomeInstitution != null ? new InstitutionDTO
                             {
                                 Name = p.HomeInstitution.Name,
                                 Addresses = (from address in p.HomeInstitution.Addresses
                                              let addressType = address.AddressType

                                              let location = address.Location

                                              let hasCity = location.City != null
                                              let city = location.City

                                              let hasCountry = location.Country != null
                                              let country = location.Country

                                              let hasDivision = location.Division != null
                                              let division = location.Division

                                              select new AddressDTO
                                              {
                                                  AddressId = address.AddressId,
                                                  AddressType = addressType.AddressName,
                                                  AddressTypeId = addressType.AddressTypeId,
                                                  City = hasCity ? city.LocationName : null,
                                                  CityId = location.CityId,
                                                  Country = hasCountry ? country.LocationName : null,
                                                  CountryId = location.CountryId,
                                                  Division = hasDivision ? division.LocationName : null,
                                                  DivisionId = location.DivisionId,
                                                  IsPrimary = address.IsPrimary,
                                                  LocationId = location.LocationId,
                                                  LocationName = location.LocationName,
                                                  OrganizationId = address.OrganizationId,
                                                  PostalCode = location.PostalCode,
                                                  PersonId = address.PersonId,
                                                  Street1 = location.Street1,
                                                  Street2 = location.Street2,
                                                  Street3 = location.Street3,
                                              }).OrderByDescending(a => a.IsPrimary).ThenBy(a => a.AddressType),
                             } : null,
                             HostInstitution = p.HostInstitution != null ? new InstitutionDTO
                             {
                                 Name = p.HostInstitution.Name,
                                 Addresses = (from address in p.HostInstitution.Addresses
                                              let addressType = address.AddressType

                                              let location = address.Location

                                              let hasCity = location.City != null
                                              let city = location.City

                                              let hasCountry = location.Country != null
                                              let country = location.Country

                                              let hasDivision = location.Division != null
                                              let division = location.Division

                                              select new AddressDTO
                                              {
                                                  AddressId = address.AddressId,
                                                  AddressType = addressType.AddressName,
                                                  AddressTypeId = addressType.AddressTypeId,
                                                  City = hasCity ? city.LocationName : null,
                                                  CityId = location.CityId,
                                                  Country = hasCountry ? country.LocationName : null,
                                                  CountryId = location.CountryId,
                                                  Division = hasDivision ? division.LocationName : null,
                                                  DivisionId = location.DivisionId,
                                                  IsPrimary = address.IsPrimary,
                                                  LocationId = location.LocationId,
                                                  LocationName = location.LocationName,
                                                  OrganizationId = address.OrganizationId,
                                                  PostalCode = location.PostalCode,
                                                  PersonId = address.PersonId,
                                                  Street1 = location.Street1,
                                                  Street2 = location.Street2,
                                                  Street3 = location.Street3,
                                              }).OrderByDescending(a => a.IsPrimary).ThenBy(a => a.AddressType),
                             } : null
                         });
            return query;
        }

        /// <summary>
        /// Creates a query to return all participantPersonSevises in the context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The filtered and sorted query to retrieve participantPersonSevises.</returns>
        public static IQueryable<ParticipantPersonSevisDTO> CreateGetParticipantPersonSevisesDTOQuery(EcaContext context, QueryableOperator<ParticipantPersonSevisDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            var query = CreateGetParticipantPersonSevisesDTOQuery(context);
            query = query.Apply(queryOperator);
            return query;
        }

        /// <summary>
        /// Creates a query to return all participantPersonSevises for the project with the given id in the context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The project id.</param>
        /// <returns>The filtered and sorted query to retrieve participantPersonSevises.</returns>
        public static IQueryable<ParticipantPersonSevisDTO> CreateGetParticipantPersonSevisesDTOByProjectIdQuery(EcaContext context, int projectId, QueryableOperator<ParticipantPersonSevisDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            var query = CreateGetParticipantPersonSevisesDTOQuery(context).Where(x => x.ProjectId == projectId);
            query = query.Apply(queryOperator);
            return query;
        }

        /// <summary>
        /// Returns the participantPersonSevis by participant id 
        /// </summary>
        /// <param name="context">The context to query</param>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>The participantPersonSevis</returns>
        public static IQueryable<ParticipantPersonSevisDTO> CreateGetParticipantPersonSevisDTOByIdQuery(EcaContext context, int participantId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = CreateGetParticipantPersonSevisesDTOQuery(context).
                Where(p => p.ParticipantId == participantId);
            return query;
        }
    }
}
