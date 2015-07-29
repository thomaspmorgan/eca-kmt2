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
    public static class ParticipantPersonQueries
    {
        /// <summary>
        /// Query to get a list of participant people 
        /// </summary>
        /// <param name="context">The context to query</param>
        /// <returns>List of participant people</returns>
        public static IQueryable<SimpleParticipantPersonDTO> CreateGetSimpleParticipantPersonsDTOQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = (from p in context.ParticipantPersons
                         select new SimpleParticipantPersonDTO
                         {
                             ParticipantId = p.ParticipantId,
                             SevisId = p.SevisId,
                             ContactAgreement = p.ContactAgreement,
                             ProjectId = p.Participant.ProjectId,
                             StudyProject = p.StudyProject,
                             FieldOfStudy = p.FieldOfStudy != null ? p.FieldOfStudy.Description : null,
                             ProgramSubject = p.ProgramSubject != null ? p.ProgramSubject.Description : null,
                             Position = p.Position != null ? p.Position.Description : null,
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
        /// Creates a query to return all participantPersons in the context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The filtered and sorted query to retrieve participantPersons.</returns>
        public static IQueryable<SimpleParticipantPersonDTO> CreateGetSimpleParticipantPersonsDTOQuery(EcaContext context, QueryableOperator<SimpleParticipantPersonDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            var query = CreateGetSimpleParticipantPersonsDTOQuery(context);
            query = query.Apply(queryOperator);
            return query;
        }

        /// <summary>
        /// Creates a query to return all participantPersons for the project with the given id in the context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The project id.</param>
        /// <returns>The filtered and sorted query to retrieve participantPersons.</returns>
        public static IQueryable<SimpleParticipantPersonDTO> CreateGetSimpleParticipantPersonsDTOByProjectIdQuery(EcaContext context, int projectId, QueryableOperator<SimpleParticipantPersonDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            var query = CreateGetSimpleParticipantPersonsDTOQuery(context).Where(x => x.ProjectId == projectId);
            query = query.Apply(queryOperator);
            return query;
        }

        /// <summary>
        /// Returns the participantPerson by participant id 
        /// </summary>
        /// <param name="context">The context to query</param>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>The participantPerson</returns>
        public static IQueryable<SimpleParticipantPersonDTO> CreateGetParticipantPersonDTOByIdQuery(EcaContext context, int participantId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = CreateGetSimpleParticipantPersonsDTOQuery(context).
                Where(p => p.ParticipantId == participantId);
            return query;
        }
    }
}
