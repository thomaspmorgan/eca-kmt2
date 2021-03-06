﻿using ECA.Business.Queries.Models.Admin;
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
            return CreateGetOfficesQuery(context).Where(x => x.Id == officeId);
        }

        public static IQueryable<OfficeDTO> CreateGetOfficesQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = from office in context.Organizations
                        let contacts = office.Contacts
                        let programs = office.OwnerPrograms
                        let goals = programs.SelectMany(x => x.Goals).Distinct()
                        let themes = programs.SelectMany(x => x.Themes).Distinct()
                        let parentOffice = office.ParentOrganization

                        where Organization.OFFICE_ORGANIZATION_TYPE_IDS.Contains(office.OrganizationTypeId)
                        select new OfficeDTO
                        {
                            Contacts = contacts.OrderBy(x => x.FullName).Select(x => new SimpleLookupDTO { Id = x.ContactId, Value = x.Position == null ? x.FullName : x.FullName + " (" + x.Position + ")" }),
                            Description = office.Description,
                            Goals = goals.OrderBy(x => x.GoalName).Select(x => new SimpleLookupDTO { Id = x.GoalId, Value = x.GoalName }),
                            Id = office.OrganizationId,
                            Name = office.Name,
                            RevisedOn = office.History.RevisedOn,
                            Themes = themes.OrderBy(x => x.ThemeName).Select(x => new SimpleLookupDTO { Id = x.ThemeId, Value = x.ThemeName }),
                            OfficeSymbol = office.OfficeSymbol,
                            ParentOfficeId = parentOffice == null ? default(int?) : parentOffice.OrganizationId,
                            ParentOfficeName = parentOffice == null ? null : parentOffice.Name
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

        /// <summary>
        /// Returns a query to select office setting dtos from the given context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to retrieve office settings.</returns>
        public static IQueryable<OfficeSettingDTO> CreateGetOfficeSettingDTOQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return context.OfficeSettings.Select(x => new OfficeSettingDTO
            {
                Id = x.OfficeSettingId,
                Name = x.Name,
                OfficeId = x.OfficeId,
                Value = x.Value
            });
        }

        /// <summary>
        /// Returns a query to select office setting dtos from the given context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="officeId">The office id.</param>
        /// <returns>The query to retrieve office settings.</returns>
        public static IQueryable<OfficeSettingDTO> CreateGetOfficeSettingDTOByOfficeIdQuery(EcaContext context, int officeId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return CreateGetOfficeSettingDTOQuery(context).Where(x => x.OfficeId == officeId);
        }
    }
}
