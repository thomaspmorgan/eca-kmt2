﻿using ECA.Core.Settings;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Search
{
    /// <summary>
    /// An OfficeDocumentSaveAction is a save action that handles organizations that are office, branches, or divisions.
    /// </summary>
    public class OfficeDocumentSaveAction : DocumentSaveAction<Organization>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="settings">The app settings.</param>
        public OfficeDocumentSaveAction(AppSettings settings) : base(settings)
        {
            Contract.Requires(settings != null, "The settings must not be null.");
        }

        public override DocumentKey GetDocumentKey(Organization entity, DbEntityEntry<Organization> entityEntry)
        {
            return new DocumentKey(OfficeDTODocumentConfiguration.OFFICE_DTO_DOCUMENT_TYPE_ID, entity.OrganizationId);
        }

        /// <summary>
        /// Returns true if the given organization is not an office.
        /// </summary>
        /// <param name="createdEntity">The organization.</param>
        /// <returns>True if the given organization is not an office.</returns>
        public override bool IsCreatedEntityExcluded(Organization createdEntity)
        {
            return !IsOffice(createdEntity);
        }

        /// <summary>
        /// Returns true if the given organization is not an office.
        /// </summary>
        /// <param name="deletedEntity">The organization.</param>
        /// <returns>True if the given organization is not an office.</returns>
        public override bool IsDeletedEntityExcluded(Organization deletedEntity)
        {
            return !IsOffice(deletedEntity);
        }

        /// <summary>
        /// Returns true if the given organization is not an office.
        /// </summary>
        /// <param name="modifiedEntity">The organization.</param>
        /// <returns>True if the given organization is not an office.</returns>
        public override bool IsModifiedEntityExcluded(Organization modifiedEntity)
        {
            return !IsOffice(modifiedEntity);
        }

        /// <summary>
        /// Returns true if the given organization is an office.
        /// </summary>
        /// <param name="organization">The organization.</param>
        /// <returns>True if the given organization is an office, branch or division.</returns>
        public bool IsOffice(Organization organization)
        {
            return Organization.OFFICE_ORGANIZATION_TYPE_IDS.Contains(organization.OrganizationTypeId);
        }
    }
}
