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
        /// Query to get list of organizations
        /// </summary>
        /// <param name="context">The context to query</param>
        /// <returns>List of organizations</returns>
        public static IQueryable<SimpleOrganizationDTO> CreateGetSimpleOrganizationsDTOQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = from organization in context.Organizations
                        let organizationType = organization.OrganizationType
                        let address = organization.Addresses.OrderByDescending(x => x.IsPrimary).FirstOrDefault()

                        let addressHasCity = address != null
                                            && address.Location != null
                                            && address.Location.City != null
                                            && address.Location.City.LocationName != null

                        let addressHasCountry = address != null
                                            && address.Location != null
                                            && address.Location.Country != null
                                            && address.Location.Country.LocationName != null

                        where !Organization.OFFICE_ORGANIZATION_TYPE_IDS.Contains(organization.OrganizationTypeId)
                        select new SimpleOrganizationDTO
                        {
                            OrganizationId = organization.OrganizationId,
                            Name = organization.Name,
                            OrganizationType = organizationType.OrganizationTypeName,
                            Status = organization.Status,
                            Location = (addressHasCity ? address.Location.City.LocationName : String.Empty)
                                        + (addressHasCity && addressHasCountry ? ", " : String.Empty)
                                        + (addressHasCountry ? address.Location.Country.LocationName : String.Empty)
                        };
            return query;
        }
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

            var query = CreateGetSimpleOrganizationsDTOQuery(context);
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
                        let socialMedias = org.SocialMedias
                        let contacts = org.Contacts
                        let addresses = org.Addresses
                        let parentOrg = org.ParentOrganization
                        let orgRoles = org.OrganizationRoles
                        where org.OrganizationId == organizationId
                        select new OrganizationDTO
                        {
                            Contacts = contacts.Select(x => new SimpleLookupDTO { Id = x.ContactId, Value = x.FullName }),
                            Description = org.Description,
                            Addresses = (from address in addresses
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
                            Name = org.Name,
                            OrganizationId = org.OrganizationId,
                            OrganizationType = orgType.OrganizationTypeName,
                            OrganizationTypeId = org.OrganizationTypeId,
                            ParentOrganizationId = parentOrg == null ? default(int?) : parentOrg.OrganizationId,
                            ParentOrganizationName = parentOrg == null ? null : parentOrg.Name,
                            RevisedOn = org.History.RevisedOn,
                            SocialMedias = socialMedias.Select(x =>
                                new SocialMediaDTO
                                {
                                    Id = x.SocialMediaId,
                                    SocialMediaType = x.SocialMediaType.SocialMediaTypeName,
                                    SocialMediaTypeId = x.SocialMediaTypeId,
                                    Value = x.SocialMediaValue
                                }).OrderBy(s => s.SocialMediaType),
                            Website = org.Website,
                            Status = org.Status,
                            OrganizationRoles = orgRoles.Select(x =>
                                new SimpleLookupDTO
                                {
                                    Id = x.OrganizationRoleId,
                                    Value = x.OrganizationRoleName
                                })
                        };
            return query;
        }

        /// <summary>
        /// Creates a query that returns a list of organization roles
        /// </summary>
        /// <param name="context">The context to query</param>
        /// <param name="queryOperator">The query operator to apply</param>
        /// <returns>The query</returns>
        public static IQueryable<SimpleLookupDTO> CreateGetOrganizationRolesQuery(EcaContext context, QueryableOperator<SimpleLookupDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");

            var query = context.OrganizationRoles.Select(x => new SimpleLookupDTO { Id = x.OrganizationRoleId, Value = x.OrganizationRoleName });
            query = query.Apply(queryOperator);
            return query;
        }
    }
}
