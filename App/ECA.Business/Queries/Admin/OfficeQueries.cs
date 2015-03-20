using ECA.Business.Queries.Models.Admin;
using ECA.Business.Queries.Models.Office;
using ECA.Business.Service.Lookup;
using ECA.Core.DynamicLinq;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Admin
{
    /// <summary>
    /// OfficeQueries are queries for offices and related entities in an EcaContext.
    /// </summary>
    public static class OfficeQueries
    {
        /// <summary>
        /// Returns a query to locate the office with the given id.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="officeId">The office id.</param>
        /// <returns>The query to locate the office with the given id.</returns>
        public static IQueryable<OfficeDTO> CreateGetOfficeByIdQuery(EcaContext context, int officeId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = from office in context.Organizations
                        let contacts = office.Contacts
                        let programs = office.OwnerPrograms
                        let goals = programs.SelectMany(x => x.Goals).Distinct()
                        let themes = programs.SelectMany(x => x.Themes).Distinct()
                        let foci = programs.Select(x => x.Focus).Distinct()

                        where Organization.OFFICE_ORGANIZATION_TYPE_IDS.Contains(office.OrganizationTypeId) && office.OrganizationId == officeId
                        select new OfficeDTO
                        {
                            Contacts = contacts.OrderBy(x => x.FullName).Select(x => new SimpleLookupDTO { Id = x.ContactId, Value = x.FullName }),
                            Description = office.Description,
                            Foci = foci.OrderBy(x => x.FocusName).Select(x => new SimpleLookupDTO { Id = x.FocusId, Value = x.FocusName }),
                            Goals = goals.OrderBy(x => x.GoalName).Select(x => new SimpleLookupDTO { Id = x.GoalId, Value = x.GoalName }),
                            Id = office.OrganizationId,
                            Name = office.Name,
                            RevisedOn = office.History.RevisedOn,
                            Themes = themes.OrderBy(x => x.ThemeName).Select(x => new SimpleLookupDTO { Id = x.ThemeId, Value = x.ThemeName }),
                            OfficeSymbol = office.OfficeSymbol
                        };
            return query;
        }

        private static IQueryable<SimpleOfficeDTO> CreateGetSimpleOfficeDTO(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = from office in context.Organizations
                        where Organization.OFFICE_ORGANIZATION_TYPE_IDS.Contains(office.OrganizationTypeId)
                        select new SimpleOfficeDTO
                        {
                            Description = office.Description,
                            Name = office.Name,
                            OfficeLevel = 1,
                            OfficeSymbol = office.OfficeSymbol,
                            OrganizationId = office.OrganizationId,
                            OrganizationTypeId = office.OrganizationTypeId,
                            OrganizationType = office.OrganizationType.OrganizationTypeName,
                            ParentOrganization_OrganizationId = office.ParentOrganization.OrganizationId
                        };
            return query;
        }

        /// <summary>
        /// Returns a query that retrieves first level child offices, branches, and divisions of the office with the given id.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="officeId">The office id.</param>
        /// <returns>The child offices.</returns>
        public static IQueryable<SimpleOfficeDTO> CreateGetChildOfficesByOfficeIdQuery(EcaContext context, int officeId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return CreateGetSimpleOfficeDTO(context).Where(x => x.ParentOrganization_OrganizationId == officeId);
        }

        public static IQueryable<SimpleOfficeDTO> CreateGetOfficesQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = context.Database.SqlQuery<SimpleOfficeDTO>("GetOffices");
            return query.AsQueryable<SimpleOfficeDTO>();
        }
    }
}
