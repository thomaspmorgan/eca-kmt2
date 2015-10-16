using ECA.Core.DynamicLinq;
using ECA.Core.Settings;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq.Expressions;

namespace ECA.Business.Search
{
    public class AddressToOrganizationDocumentSaveAction : RelatedEntityDocumentSaveAction<Address>
    {

        private static string ORGANIZATION_ID_PROPERTY_NAME = PropertyHelper.GetPropertyName<Address>(x => x.OrganizationId);

        public AddressToOrganizationDocumentSaveAction(AppSettings settings) : base(settings)
        {
            Contract.Requires(settings != null, "The app settings must not be null.");
        }

        public override List<DocumentKey> GetRelatedEntityDocumentKeysOfAddedEntity(Address addedEntity)
        {
            return GetOrganizationDocumentKeys(addedEntity);
        }

        public override Task<List<DocumentKey>> GetRelatedEntityDocumentKeysOfAddedEntityAsync(Address addedEntity)
        {
            return Task.FromResult<List<DocumentKey>>(GetRelatedEntityDocumentKeysOfAddedEntity(addedEntity));
        }

        public List<DocumentKey> GetOrganizationDocumentKeys(Address addedEntity)
        {
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
            else
            {
                throw new NotSupportedException("Unable to determine organization document key from the given address.");
            }
            return list;
        }

        public List<DocumentKey> GetOrganizationDocumentKeys(DbPropertyValues originalValues)
        {
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

        public override List<DocumentKey> GetRelatedEntityDocumentKeysOfModifiedEntity(Address updatedOrDeletedEntity, DbPropertyValues originalValues)
        {
            return GetOrganizationDocumentKeys(originalValues);
        }

        public override Task<List<DocumentKey>> GetRelatedEntityDocumentKeysOfModifiedEntityAsync(Address updatedOrDeletedEntity, DbPropertyValues originalValues)
        {
            return Task.FromResult<List<DocumentKey>>(GetRelatedEntityDocumentKeysOfModifiedEntity(updatedOrDeletedEntity, originalValues));
        }

        public override bool IsCreatedEntityExcluded(Address createdEntity)
        {
            return false;
        }

        public override bool IsDeletedEntityExcluded(Address deletedEntity)
        {
            return false;
        }

        public override bool IsModifiedEntityExcluded(Address modifiedEntity)
        {
            return false;
        }
    }

    //public class AddressDocumentSaveAction : DocumentSaveAction<Address>
    //{
    //    private static string ORGANIZATION_ID_PROPERTY_NAME = PropertyHelper.GetPropertyName<Address>(x => x.OrganizationId);
    //    private static string PERSON_ID_PROPERTY_NAME = PropertyHelper.GetPropertyName<Address>(x => x.PersonId);

    //    private Dictionary<Address, int> addressToOrganizationIdMapping;
    //    private Dictionary<Address, int> addressToPersonIdMapping;

    //    /// <summary>
    //    /// An AddressDocumentSaveAction is used to craft proper index batch messages saying
    //    /// the address' related entity (organization, person, etc.) must be updated.
    //    /// </summary>
    //    /// <param name="settings">The app settings.</param>
    //    public AddressDocumentSaveAction(AppSettings settings) : base(settings)
    //    {
    //        Contract.Requires(settings != null, "The settings must not be null.");
    //        this.addressToOrganizationIdMapping = new Dictionary<Address, int>();
    //        this.addressToPersonIdMapping = new Dictionary<Address, int>();
    //    }

    //    public override void BeforeSaveChanges(DbContext context)
    //    {
    //        base.BeforeSaveChanges(context);
    //        var addressEntries = GetAllModifiedAddressDbEntityEntries().ToList();
    //        foreach (var addressEntry in addressEntries)
    //        {
    //            if (addressEntry.State == EntityState.Added)
    //            {
    //                HandleAddedAddress((Address)addressEntry.Entity);
    //            }
    //            else
    //            {
    //                var propertyValues = addressEntry.GetDatabaseValues();
    //                HandleDatabaseValues((Address)addressEntry.Entity, propertyValues);
    //            }
    //        }
    //    }

    //    public override async Task BeforeSaveChangesAsync(DbContext context)
    //    {
    //        await base.BeforeSaveChangesAsync(context);
    //        var addressEntries = GetAllModifiedAddressDbEntityEntries().ToList();
    //        foreach (var addressEntry in addressEntries)
    //        {
    //            if (addressEntry.State == EntityState.Added)
    //            {
    //                HandleAddedAddress((Address)addressEntry.Entity);
    //            }
    //            else
    //            {
    //                var propertyValues = await addressEntry.GetDatabaseValuesAsync();
    //                HandleDatabaseValues((Address)addressEntry.Entity, propertyValues);
    //            }
    //        }
    //    }

    //    private void HandleAddedAddress(Address address)
    //    {
    //        if (address.OrganizationId.HasValue)
    //        {
    //            this.addressToOrganizationIdMapping[address] = address.OrganizationId.Value;
    //        }
    //        else if (address.Organization != null)
    //        {
    //            this.addressToOrganizationIdMapping[address] = address.Organization.OrganizationId;
    //        }
    //        else if (address.PersonId.HasValue)
    //        {
    //            this.addressToPersonIdMapping[address] = address.PersonId.Value;
    //        }
    //        else if (address.Person != null)
    //        {
    //            this.addressToPersonIdMapping[address] = address.Person.PersonId;
    //        }
    //        else
    //        {
    //            throw new NotSupportedException(String.Format("Unable to ascertain affected entity of address."));
    //        }
    //    }

    //    private IEnumerable<DbEntityEntry> GetAllModifiedAddressDbEntityEntries()
    //    {
    //        var addresses = GetAllModifiedAddresses();
    //        foreach (var address in addresses)
    //        {
    //            var entry = GetEntityEntry(address);
    //            yield return entry;
    //        }
    //    }

    //    private List<Address> GetAllModifiedAddresses()
    //    {
    //        return this.CreatedEntities.Union(this.ModifiedEntities).Union(this.DeletedEntities).Distinct().ToList();
    //    }

    //    private void HandleDatabaseValues(Address address, DbPropertyValues values)
    //    {
    //        var organizationId = values.GetValue<int?>(ORGANIZATION_ID_PROPERTY_NAME);
    //        var personId = values.GetValue<int?>(PERSON_ID_PROPERTY_NAME);
    //        if (organizationId.HasValue)
    //        {
    //            this.addressToOrganizationIdMapping.Add(address, organizationId.Value);
    //        }
    //        if (personId.HasValue)
    //        {
    //            this.addressToPersonIdMapping.Add(address, personId.Value);
    //        }
    //    }

    //    public int? GetOrganizationId(Address address)
    //    {
    //        Contract.Requires(address != null, "The address must not be null.");
    //        Contract.Assert(this.addressToPersonIdMapping != null, "The mapping must not be null.");
    //        Contract.Assert(this.addressToOrganizationIdMapping != null, "The mapping must not be null.");
    //        if (this.addressToOrganizationIdMapping.ContainsKey(address))
    //        {
    //            return this.addressToOrganizationIdMapping[address];
    //        }
    //        else
    //        {
    //            return null;
    //        }
    //    }

    //    public int? GetPersonId(Address address)
    //    {
    //        Contract.Requires(address != null, "The address must not be null.");
    //        Contract.Assert(this.addressToPersonIdMapping != null, "The mapping must not be null.");
    //        Contract.Assert(this.addressToOrganizationIdMapping != null, "The mapping must not be null.");
    //        if (this.addressToPersonIdMapping.ContainsKey(address))
    //        {
    //            return this.addressToPersonIdMapping[address];
    //        }
    //        else
    //        {
    //            return null;
    //        }
    //    }

    //    /// <summary>
    //    /// Creates a new batch message that only contains modified documents because the create, delete, or update
    //    /// of an address implies another entity had a modified, created, or deleted address related to it.  Therefore,
    //    /// the organization, person, etc must be re-indexed.
    //    /// </summary>
    //    /// <returns>The batch message detailing the person or organization indexed document must be updated.</returns>
    //    public override IndexDocumentBatchMessage GetBatchMessage()
    //    {
    //        Contract.Assert(addressToPersonIdMapping.Count == 0, "There should not be any people to update as people are not yet indexed.");
    //        var modifiedOrganizationKeys = this.addressToOrganizationIdMapping.Select(x => GetDocumentKey(x.Key, null).ToString()).ToList();
    //        var modifiedPeopleKeys = this.addressToPersonIdMapping.Select(x => GetDocumentKey(x.Key, null).ToString()).ToList();

    //        var baseMessage = new IndexDocumentBatchMessage();
    //        baseMessage.ModifiedDocuments = modifiedOrganizationKeys.Union(modifiedPeopleKeys).ToList();
    //        return baseMessage;
    //    }

    //    /// <summary>
    //    /// Returns a document key for the organization or person depending on the given entities property values.
    //    /// </summary>
    //    /// <param name="entity">The address entity.</param>
    //    /// <returns>A document key for the address's organization or person depending on the related entities.</returns>
    //    public override DocumentKey GetDocumentKey(Address entity, DbEntityEntry<Address> addressEntry)
    //    {
    //        //var organizationDocumentTypeId = OrganizationDTODocumentConfiguration.ORGANIZATION_DTO_DOCUMENT_TYPE_ID;
    //        //var originalOrganizationId = addressEntry.OriginalValues.GetValue<int?>(ORGANIZATION_ID_PROPERTY_NAME);
    //        //var originalPersonId = addressEntry.OriginalValues.GetValue<int?>(PERSON_ID_PROPERTY_NAME);
    //        //if (originalOrganizationId.HasValue)
    //        //{
    //        //    return new DocumentKey(organizationDocumentTypeId, originalOrganizationId.Value);
    //        //}
    //        //else
    //        //{
    //        //    throw new NotSupportedException("Currently people are not indexed for searching; therefore, addresses related to people should not be indexed.  These address should be excluded.");
    //        //}
    //        if (this.addressToOrganizationIdMapping.ContainsKey(entity))
    //        {
    //            var orgId = this.addressToOrganizationIdMapping[entity];
    //            return new DocumentKey(OrganizationDTODocumentConfiguration.ORGANIZATION_DTO_DOCUMENT_TYPE_ID, orgId);
    //        }
    //        else
    //        {
    //            throw new NotSupportedException("Currently people are not indexed for searching; therefore, addresses related to people should not be indexed.  These address should be excluded.");
    //        }
    //    }

    //    /// <summary>
    //    /// Returns false, an address will never be ignored.
    //    /// </summary>
    //    /// <param name="createdEntity">The created address.</param>
    //    /// <returns>False</returns>
    //    public override bool IsCreatedEntityExcluded(Address createdEntity)
    //    {
    //        return false;
    //    }

    //    /// <summary>
    //    /// Returns false, an address will never be ignored.
    //    /// </summary>
    //    /// <param name="deletedEntity">The deleted address.</param>
    //    /// <returns>False</returns>
    //    public override bool IsDeletedEntityExcluded(Address deletedEntity)
    //    {
    //        return false;
    //    }

    //    /// <summary>
    //    /// Returns false, an address will never be ignored.
    //    /// </summary>
    //    /// <param name="modifiedEntity">The modified address.</param>
    //    /// <returns>False</returns>
    //    public override bool IsModifiedEntityExcluded(Address modifiedEntity)
    //    {
    //        return false;
    //    }
    //}
}
