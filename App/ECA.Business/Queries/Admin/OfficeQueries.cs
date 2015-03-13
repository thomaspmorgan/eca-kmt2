using ECA.Business.Queries.Models.Admin;
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

                        where office.OrganizationTypeId == OrganizationType.Office.Id && office.OrganizationId == officeId
                        select new OfficeDTO
                        {
                            Contacts = contacts.Select(x => new SimpleLookupDTO { Id = x.ContactId, Value = x.FullName }),
                            Description = office.Description,
                            Foci = foci.Select(x => new SimpleLookupDTO {Id = x.FocusId, Value = x.FocusName}),
                            Goals = goals.Select(x => new SimpleLookupDTO { Id = x.GoalId, Value = x.GoalName }),
                            Id = office.OrganizationId,
                            Name = office.Name,
                            Themes = themes.Select(x => new SimpleLookupDTO { Id = x.ThemeId, Value = x.ThemeName })
                        };
            return query;
        }
    }
}
