using ECA.Core.Settings;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Search
{
    public class AddressDocumentSaveAction : DocumentSaveAction<Address>
    {
        /// <summary>
        /// An AddressDocumentSaveAction is used to craft proper index batch messages saying
        /// the address' related entity (organization, person, etc.) must be updated.
        /// </summary>
        /// <param name="settings">The app settings.</param>
        public AddressDocumentSaveAction(AppSettings settings) : base(settings)
        {
            Contract.Requires(settings != null, "The settings must not be null.");
        }

        /// <summary>
        /// Creates a new batch message that only contains modified documents because the create, delete, or update
        /// of an address implies another entity had a modified, created, or deleted address related to it.  Therefore,
        /// the organization, person, etc must be re-indexed.
        /// </summary>
        /// <returns>The batch message detailing the person or organization indexed document must be updated.</returns>
        public override IndexDocumentBatchMessage GetBatchMessage()
        {
            var createdDocuments = this.CreatedEntities;
            var modifiedDocuments = this.ModifiedEntities;
            var deletedDocuments = this.DeletedEntities;
            var allDocuments = createdDocuments
                .Union(modifiedDocuments)
                .Union(deletedDocuments);

            var baseMessage = new IndexDocumentBatchMessage();
            baseMessage.ModifiedDocuments = allDocuments.Select(x => GetDocumentKey(x).ToString()).ToList();
            return baseMessage;
        }

        /// <summary>
        /// Returns a document key for the organization or person depending on the given entities property values.
        /// </summary>
        /// <param name="entity">The address entity.</param>
        /// <returns>A document key for the address's organization or person depending on the related entities.</returns>
        public override DocumentKey GetDocumentKey(Address entity)
        {
            var organizationDocumentTypeId = OrganizationDTODocumentConfiguration.ORGANIZATION_DTO_DOCUMENT_TYPE_ID;
            if (entity.OrganizationId.HasValue)
            {
                return new DocumentKey(organizationDocumentTypeId, entity.OrganizationId.Value);
            }
            if (entity.Organization != null)
            {
                return new DocumentKey(organizationDocumentTypeId, entity.Organization.OrganizationId);
            }
            else
            {
                throw new NotSupportedException("Currently people are not indexed for searching; therefore, addresses related to people should not be indexed.  These address should be excluded.");
            }
        }

        /// <summary>
        /// Returns false, an address will never be ignored.
        /// </summary>
        /// <param name="createdEntity">The created address.</param>
        /// <returns>False</returns>
        public override bool IsCreatedEntityExcluded(Address createdEntity)
        {
            return false;
        }

        /// <summary>
        /// Returns false, an address will never be ignored.
        /// </summary>
        /// <param name="deletedEntity">The deleted address.</param>
        /// <returns>False</returns>
        public override bool IsDeletedEntityExcluded(Address deletedEntity)
        {
            return false;
        }

        /// <summary>
        /// Returns false, an address will never be ignored.
        /// </summary>
        /// <param name="modifiedEntity">The modified address.</param>
        /// <returns>False</returns>
        public override bool IsModifiedEntityExcluded(Address modifiedEntity)
        {
            return false;
        }
    }
}
