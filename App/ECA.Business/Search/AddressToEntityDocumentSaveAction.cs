using ECA.Core.DynamicLinq;
using ECA.Core.Settings;
using ECA.Data;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace ECA.Business.Search
{
    /// <summary>
    /// An AddressToEntityDocumentSaveAction handles retrieving a related entity's primary key and converting
    /// it to a document key to force and update to an entity index.  For example, if an address is updated to an
    /// organization the organization's index should be updated.
    /// </summary>
    public class AddressToEntityDocumentSaveAction : RelatedEntityDocumentSaveAction<Address>
    {

        private static string ORGANIZATION_ID_PROPERTY_NAME = PropertyHelper.GetPropertyName<Address>(x => x.OrganizationId);

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public AddressToEntityDocumentSaveAction(AppSettings settings) : base(settings)
        {
            Contract.Requires(settings != null, "The app settings must not be null.");
        }

        /// <summary>
        /// Returns the address entity document keys.
        /// </summary>
        /// <param name="addedEntity">The added address.</param>
        /// <returns>The document keys.</returns>
        public override List<DocumentKey> GetRelatedEntityDocumentKeysOfAddedEntity(Address addedEntity)
        {
            return GetOrganizationDocumentKeys(addedEntity);
        }

        /// <summary>
        /// Returns the address entity document keys.
        /// </summary>
        /// <param name="addedEntity">The added address.</param>
        /// <returns>The document keys.</returns>
        public override Task<List<DocumentKey>> GetRelatedEntityDocumentKeysOfAddedEntityAsync(Address addedEntity)
        {
            return Task.FromResult<List<DocumentKey>>(GetRelatedEntityDocumentKeysOfAddedEntity(addedEntity));
        }

        /// <summary>
        /// Returns the organization document keys from the added address.
        /// </summary>
        /// <param name="addedEntity">The address.</param>
        /// <returns>The organization document keys.</returns>
        public List<DocumentKey> GetOrganizationDocumentKeys(Address addedEntity)
        {
            Contract.Requires(addedEntity != null, "The addedEntity must not be null.");
            var documentTypeId = OrganizationDTODocumentConfiguration.ORGANIZATION_DTO_DOCUMENT_TYPE_ID;
            var list = new List<DocumentKey>();
            if (addedEntity.OrganizationId.HasValue)
            {
                list.Add(GetOrganizationDocumentKey(addedEntity.OrganizationId.Value));
            }
            else if (addedEntity.Organization != null)
            {
                list.Add(GetOrganizationDocumentKey(addedEntity.Organization.OrganizationId));
            }
            return list;
        }

        /// <summary>
        /// Returns the organization document keys from the added address.
        /// </summary>
        /// <param name="originalValues">The address original values.</param>
        /// <returns>The organization document keys.</returns>
        public List<DocumentKey> GetOrganizationDocumentKeys(DbPropertyValues originalValues)
        {
            Contract.Requires(originalValues != null, "The original values must not be null.");
            var organizationId = originalValues.GetValue<int?>(ORGANIZATION_ID_PROPERTY_NAME);
            var list = new List<DocumentKey>();
            if (organizationId.HasValue)
            {
                list.Add(GetOrganizationDocumentKey(organizationId.Value));
            }
            return list;
        }

        private DocumentKey GetOrganizationDocumentKey(int organizationId)
        {
            var documentTypeId = OrganizationDTODocumentConfiguration.ORGANIZATION_DTO_DOCUMENT_TYPE_ID;
            return new DocumentKey(documentTypeId, organizationId);
        }

        /// <summary>
        /// Returns the entity document keys for the given address.
        /// </summary>
        /// <param name="updatedOrDeletedEntity">The updated or deleted address.</param>
        /// <param name="originalValues">The address original values.</param>
        /// <returns>The document keys.</returns>
        public override List<DocumentKey> GetRelatedEntityDocumentKeysOfModifiedEntity(Address updatedOrDeletedEntity, DbPropertyValues originalValues)
        {
            return GetOrganizationDocumentKeys(originalValues);
        }

        /// <summary>
        /// Returns the entity document keys for the given address.
        /// </summary>
        /// <param name="updatedOrDeletedEntity">The updated or deleted address.</param>
        /// <param name="originalValues">The address original values.</param>
        /// <returns>The document keys.</returns>
        public override Task<List<DocumentKey>> GetRelatedEntityDocumentKeysOfModifiedEntityAsync(Address updatedOrDeletedEntity, DbPropertyValues originalValues)
        {
            return Task.FromResult<List<DocumentKey>>(GetRelatedEntityDocumentKeysOfModifiedEntity(updatedOrDeletedEntity, originalValues));
        }

        /// <summary>
        /// Returns false, an address entity will never be excluded.
        /// </summary>
        /// <param name="createdEntity">The entity.</param>
        /// <returns>Talse, an address entity will never be excluded.</returns>
        public override bool IsCreatedEntityExcluded(Address createdEntity)
        {
            return false;
        }

        /// <summary>
        /// Returns false, an address entity will never be excluded.
        /// </summary>
        /// <param name="deletedEntity">The entity.</param>
        /// <returns>Talse, an address entity will never be excluded.</returns>
        public override bool IsDeletedEntityExcluded(Address deletedEntity)
        {
            return false;
        }

        /// <summary>
        /// Returns false, an address entity will never be excluded.
        /// </summary>
        /// <param name="modifiedEntity">The entity.</param>
        /// <returns>Talse, an address entity will never be excluded.</returns>
        public override bool IsModifiedEntityExcluded(Address modifiedEntity)
        {
            return false;
        }
    }
}
