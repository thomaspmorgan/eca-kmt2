using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ECA.Business.Service.Lookup;

namespace ECA.Business.Queries.Admin
{
    /// <summary>
    /// Queries for organization
    /// </summary>
    public static class OrganizationQueries
    {
        /// <summary>
        /// Query to get a list of organizations
        /// </summary>
        /// <param name="context">The context to query</param>
        /// <param name="queryOperator">The query operator to apply</param>
        /// <returns>List of organizations</returns>
        public static IQueryable<SimpleOrganizationDTO> CreateGetSimpleOrganizationsDTOQuery(EcaContext context, QueryableOperator<SimpleOrganizationDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");

            var query = context.Organizations
                .Include(x => x.Addresses)
                .Where(x => !Organization.OFFICE_ORGANIZATION_TYPE_IDS.Contains(x.OrganizationTypeId))
                .Select(x => new SimpleOrganizationDTO
                {
                    Name = x.Name,
                    OrganizationType = x.OrganizationType.OrganizationTypeName,
                    OrganizationId = x.OrganizationId,
                    Status = x.Status,
                    Location = x.Addresses.FirstOrDefault().Location.Country.LocationName
                });
            query = query.Apply(queryOperator);
            return query;
        }

        /// <summary>
        /// Creates a query that returns and organization dto from the organization with the given id.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="organizationId">The organization id.</param>
        /// <returns>The query.</returns>
        public static IQueryable<OrganizationDTO> CreateGetOrganizationDTOByOrganizationIdQuery(EcaContext context, int organizationId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = from org in context.Organizations
                        let orgType = org.OrganizationType
                        let socialMedias = org.SocialMediaPresence
                        let contacts = org.Contacts
                        let addresses = org.Addresses
                        let parentOrg = org.ParentOrganization
                        where org.OrganizationId == organizationId
                        select new OrganizationDTO
                        {
                            Contacts = contacts.Select(x => new SimpleLookupDTO { Id = x.ContactId, Value = x.FullName}),
                            Description = org.Description,
                            Locations = addresses.Select(x => new ECA.Business.Queries.Models.Persons.LocationDTO
                            {
                                City = x.Location.City,
                                CityId = -1,
                                Country = x.Location.Country.LocationName,
                                CountryId = x.Location.Country.LocationId,
                                Id = x.AddressId,
                                PostalCode = x.Location.PostalCode,
                                Street1 = x.Location.Street1,
                                Street2 = x.Location.Street2,
                                Street3 = x.Location.Street3
                            }),
                            Name = org.Name,
                            OrganizationId = org.OrganizationId,
                            OrganizationType = orgType.OrganizationTypeName,
                            OrganizationTypeId = org.OrganizationTypeId,
                            ParentOrganizationId = parentOrg == null ? default(int?) : parentOrg.OrganizationId,
                            ParentOrganizationName = parentOrg == null ? null : parentOrg.Name,
                            RevisedOn = org.History.RevisedOn,
                            SocialMedias = socialMedias.Select(x => new SimpleTypeLookupDTO { Id = x.SocialMediaId, Type = x.SocialMediaType.SocialMediaTypeName, Value = x.SocialMediaValue}),
                            Website = org.Website
                        };
            return query;
        }
    }
}
