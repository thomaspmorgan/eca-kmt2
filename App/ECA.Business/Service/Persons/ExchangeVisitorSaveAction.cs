using ECA.Business.Queries.Models.Persons;
using ECA.Business.Queries.Persons;
using ECA.Core.DynamicLinq;
using ECA.Core.Service;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// The ExchangeVisitorSaveAction is responsible for detecting changes to participants and participant related entities
    /// so that a sevis validation may be executed on that participant.
    /// </summary>
    public class ExchangeVisitorSaveAction : ISaveAction
    {
        private IExchangeVisitorValidationService validationService;
        /// <summary>
        /// Creates a new instance and initializes it with the given dependencies.
        /// </summary>
        /// <param name="validationService">The exchange visitor validation service.</param>
        /// <param name="userProvider">The user provider delegate.</param>
        public ExchangeVisitorSaveAction(IExchangeVisitorValidationService validationService)
        {
            Contract.Requires(validationService != null, "The validation service must not be null.");
            this.validationService = validationService;
            this.CreatedObjects = new List<object>();
            this.ModifiedObjects = new List<object>();
            this.DeletedObjects = new List<object>();
            this.ParticipantIds = new HashSet<int>();
        }

        /// <summary>
        /// Gets the added entities.
        /// </summary>
        public List<object> CreatedObjects { get; private set; }

        /// <summary>
        /// Gets the modified entities.
        /// </summary>
        public List<object> ModifiedObjects { get; private set; }

        /// <summary>
        /// Gets the deleted entities.
        /// </summary>
        public List<object> DeletedObjects { get; private set; }

        /// <summary>
        /// Gets the set of participant ids.
        /// </summary>
        public HashSet<int> ParticipantIds { get; private set; }

        /// <summary>
        /// Gets the EcaContext instance.
        /// </summary>
        public EcaContext Context { get; set; }

        /// <summary>
        /// Returns the list of created entities from the given context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The created entities.</returns>
        public IList<object> GetCreatedEntities(DbContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var createdParticipants = GetParticipantEntities(context, GetCreatedAndModifiedEntityTypes(), EntityState.Added).ToList();
            return createdParticipants;
        }

        /// <summary>
        /// Returns the list of modified entities from the given context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The modified entities.</returns>
        public IList<object> GetModifiedEntities(DbContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var modifiedParticipants = GetParticipantEntities(context, GetCreatedAndModifiedEntityTypes(), EntityState.Modified).ToList();
            return modifiedParticipants;
        }

        /// <summary>
        /// Returns the list of modified entities from the given context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The modified entities.</returns>
        public IList<object> GetDeletedEntities(DbContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var deletedEntities = GetParticipantEntities(context, GetDeletedEntityTypes(), EntityState.Deleted).ToList();
            return deletedEntities;
        }

        /// <summary>
        /// Returns the list of objects whose entity type should be watched for potential participant changes.
        /// </summary>
        /// <param name="context">The context to get entities from.</param>
        /// <param name="state">The entity state.</param>
        /// <param name="participantEntityTypes">The participant entity types that are to be found in the give context.</param>
        /// <returns>The list of objects that relate to a participant.</returns>
        public IList<object> GetParticipantEntities(DbContext context, List<Type> participantEntityTypes, EntityState state)
        {
            Contract.Requires(context != null, "The context must not be null.");

            var changedParticipantEntities = from changedEntity in context.ChangeTracker.Entries().Where(x => x.State == state)
                                             let type = changedEntity.Entity.GetType()
                                             let baseType = type.BaseType
                                             let isParticipantPersonType = type == typeof(ParticipantPerson) || (baseType != typeof(Object) && baseType == typeof(ParticipantPerson))
                                             let participantEntityTypesContainsType = participantEntityTypes.Contains(type) || (baseType != typeof(Object) && participantEntityTypes.Contains(baseType))

                                             let participantHasImportantPropertiesChanged = state != EntityState.Modified
                                             || (isParticipantPersonType ?
                                                (
                                                    from participantPersonEntity in context.ChangeTracker.Entries<ParticipantPerson>()
                                                    let ds2019PropertyName = nameof(ParticipantPerson.IsDS2019Printed)
                                                    let ignoredPropertiesByName = new string[] { ds2019PropertyName }

                                                    let originalValues = participantPersonEntity.OriginalValues
                                                    let currentValues = participantPersonEntity.CurrentValues

                                                    let hasChanges = originalValues.PropertyNames
                                                        .Where(x => !ignoredPropertiesByName.Contains(x))
                                                        .Any(p => !originalValues[p].Equals(currentValues[p]))

                                                    where Object.ReferenceEquals(participantPersonEntity.Entity, changedEntity.Entity)
                                                    select hasChanges
                                                    ).FirstOrDefault()
                                                : false)


                                             where (participantEntityTypesContainsType && !isParticipantPersonType)
                                             || (isParticipantPersonType && participantHasImportantPropertiesChanged)
                                             select changedEntity.Entity;

            return changedParticipantEntities.ToList();
        }

        /// <summary>
        /// Returns the list of types that should be located when they are added or changed to the context.
        /// </summary>
        /// <returns>The list of types that should be located when they are added or changed to the context.</returns>
        public List<Type> GetCreatedAndModifiedEntityTypes()
        {
            var participantEntityTypes = new List<Type>();
            participantEntityTypes.Add(typeof(Participant));
            participantEntityTypes.Add(typeof(ParticipantPerson));
            participantEntityTypes.Add(typeof(ParticipantExchangeVisitor));
            participantEntityTypes.Add(typeof(Person));
            participantEntityTypes.Add(typeof(Address));
            participantEntityTypes.Add(typeof(EmailAddress));
            participantEntityTypes.Add(typeof(PhoneNumber));
            participantEntityTypes.Add(typeof(Location));
            participantEntityTypes.Add(typeof(PersonDependent));
            participantEntityTypes.Add(typeof(PersonDependentCitizenCountry));
            return participantEntityTypes;
        }

        /// <summary>
        /// Returns the list of types that should be located when they are deleted from the context.
        /// </summary>
        /// <returns>The list of types that should be located when they are deleted from the context.</returns>
        public List<Type> GetDeletedEntityTypes()
        {
            var participantEntityTypes = new List<Type>();
            participantEntityTypes.Add(typeof(Address));
            participantEntityTypes.Add(typeof(EmailAddress));
            participantEntityTypes.Add(typeof(PhoneNumber));
            participantEntityTypes.Add(typeof(PersonDependent));
            participantEntityTypes.Add(typeof(PersonDependentCitizenCountry));
            return participantEntityTypes;
        }

        #region SaveAction

        private void OnBeforeSaveChanges(DbContext context)
        {
            Contract.Requires(context is EcaContext, "The given context must be an EcaContext instance.");
            this.Context = (EcaContext)context;
            this.CreatedObjects = GetCreatedEntities(context).ToList();
            this.ModifiedObjects = GetModifiedEntities(context).ToList();
            this.DeletedObjects = GetDeletedEntities(context).ToList();
        }

        /// <summary>
        /// Locates participant related entities that have been created or modified in the context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The task.</returns>
        public void BeforeSaveChanges(DbContext context)
        {
            OnBeforeSaveChanges(context);
            foreach (var deletedObject in this.DeletedObjects)
            {
                var id = GetPersonIdByObject(this.Context, deletedObject);
                if (id.HasValue)
                {
                    var person = this.Context.People.Find(id.Value);
                    var participantId = GetParticipantId(person);
                    if (participantId.HasValue)
                    {
                        this.ParticipantIds.Add(participantId.Value);
                    }
                }
            }
        }

        /// <summary>
        /// Locates participant related entities that have been created or modified in the context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The task.</returns>
        public async Task BeforeSaveChangesAsync(DbContext context)
        {
            OnBeforeSaveChanges(context);
            foreach (var deletedObject in this.DeletedObjects)
            {
                var id = await GetPersonIdByObjectAsync(this.Context, deletedObject);
                if (id.HasValue)
                {
                    var person = await this.Context.People.FindAsync(id.Value);
                    var participantId = await GetParticipantIdAsync(person);
                    if (participantId.HasValue)
                    {
                        this.ParticipantIds.Add(participantId.Value);
                    }
                }
            }
        }

        /// <summary>
        /// Performs sevis validation on participant related entities if any were found.
        /// </summary>
        /// <param name="context">The context that has created or modified participant related entities.</param>
        public void AfterSaveChanges(DbContext context)
        {
            var allParticipantObjects = GetCreatedAndModifiedEntities();
            var ids = GetParticipantIds(allParticipantObjects);
            ids.ForEach(x => this.ParticipantIds.Add(x));
            var callSaveChanges = false;
            if (this.ParticipantIds.Count > 0)
            {
                foreach (var id in this.ParticipantIds)
                {
                    var participant = this.Context.Participants.Find(id);
                    Contract.Assert(participant != null, "The participant should be found.");
                    var participantPerson = this.Context.ParticipantPersons.Find(id);
                    if (participantPerson != null)
                    {
                        validationService.RunParticipantSevisValidation(participant.ProjectId, participant.ParticipantId);
                        callSaveChanges = true;
                    }
                }
                if (callSaveChanges)
                {
                    Context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Performs sevis validation on participant related entities if any were found.
        /// </summary>
        /// <param name="context">The context that has created or modified participant related entities.</param>
        public async Task AfterSaveChangesAsync(DbContext context)
        {
            var allParticipantObjects = GetCreatedAndModifiedEntities();
            var ids = await GetParticipantIdsAsync(allParticipantObjects);
            ids.ForEach(x => this.ParticipantIds.Add(x));
            var callSaveChanges = false;
            if (this.ParticipantIds.Count > 0)
            {
                foreach (var id in this.ParticipantIds)
                {
                    var participant = await this.Context.Participants.FindAsync(id);
                    Contract.Assert(participant != null, "The participant should be found.");
                    var participantPerson = await this.Context.ParticipantPersons.FindAsync(id);
                    if (participantPerson != null)
                    {
                        await validationService.RunParticipantSevisValidationAsync(participant.ProjectId, participant.ParticipantId);
                        callSaveChanges = true;
                    }
                }
                if (callSaveChanges)
                {
                    await Context.SaveChangesAsync();
                }
            }
        }

        #endregion

        /// <summary>
        /// Returns the union of the created, modified, and deleted objects.
        /// </summary>
        /// <returns>The list of created, modified and deleted objects.</returns>
        public List<object> GetCreatedAndModifiedEntities()
        {
            return this.ModifiedObjects.Union(this.CreatedObjects).Union(this.DeletedObjects).ToList();
        }

        #region GetPersonIdByEntity

        /// <summary>
        /// Returns the person id from the given object.  Use this method when an object has been deleted from the context
        /// and the person id must be recovered from the database.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="obj">The deleted object.</param>
        /// <returns>The person id, or null if it does not exist.</returns>
        public int? GetPersonIdByObject(EcaContext context, Object obj)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(obj != null, "The obj must not be null.");
            var type = obj.GetType();
            if (typeof(PhoneNumber).IsAssignableFrom(type))
            {
                var phoneNumber = (PhoneNumber)obj;
                return GetPersonIdByPhoneNumber(context, phoneNumber);
            }
            else if (typeof(EmailAddress).IsAssignableFrom(type))
            {
                var email = (EmailAddress)obj;
                return GetPersonIdByEmailAddress(context, email);
            }
            else if (typeof(Address).IsAssignableFrom(type))
            {
                var address = (Address)obj;
                return GetPersonIdByAddress(context, address);
            }
            else if (typeof(PersonDependent).IsAssignableFrom(type))
            {
                var personDependent = (PersonDependent)obj;
                var personId = CreateGetPersonIdByDependentIdQuery(context, personDependent.DependentId).FirstOrDefault();
                return personId == default(int) ? default(int?) : personId;
            }
            else if (typeof(PersonDependentCitizenCountry).IsAssignableFrom(type))
            {
                var personDependentCitizenCountry = (PersonDependentCitizenCountry)obj;
                var dependentId = personDependentCitizenCountry.DependentId;
                var personId = CreateGetPersonIdByDependentIdQuery(context, dependentId).FirstOrDefault();
                return personId == default(int) ? default(int?) : personId;
            }
            else
            {
                throw new NotSupportedException(String.Format("The object type [{0}] is not supported.", type.Name));
            }
        }

        /// <summary>
        /// Returns the person id from the given object.  Use this method when an object has been deleted from the context
        /// and the person id must be recovered from the database.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="obj">The deleted object.</param>
        /// <returns>The person id, or null if it does not exist.</returns>
        public async Task<int?> GetPersonIdByObjectAsync(EcaContext context, Object obj)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(obj != null, "The obj must not be null.");
            var type = obj.GetType();
            if (typeof(PhoneNumber).IsAssignableFrom(type))
            {
                var phoneNumber = (PhoneNumber)obj;
                return await GetPersonIdByPhoneNumberAsync(context, phoneNumber);
            }
            else if (typeof(EmailAddress).IsAssignableFrom(type))
            {
                var email = (EmailAddress)obj;
                return await GetPersonIdByEmailAddressAsync(context, email);
            }
            else if (typeof(Address).IsAssignableFrom(type))
            {
                var address = (Address)obj;
                return await GetPersonIdByAddressAsync(context, address);
            }
            else if (typeof(PersonDependent).IsAssignableFrom(type))
            {
                var personDependent = (PersonDependent)obj;
                var personId = await CreateGetPersonIdByDependentIdQuery(context, personDependent.DependentId).FirstOrDefaultAsync();
                return personId == default(int) ? default(int?) : personId;
            }
            else if (typeof(PersonDependentCitizenCountry).IsAssignableFrom(type))
            {
                var personDependentCitizenCountry = (PersonDependentCitizenCountry)obj;
                var dependentId = personDependentCitizenCountry.DependentId;
                var personId = await CreateGetPersonIdByDependentIdQuery(context, dependentId).FirstOrDefaultAsync();
                return personId == default(int) ? default(int?) : personId;
            }
            else
            {
                throw new NotSupportedException(String.Format("The object type [{0}] is not supported.", type.Name));
            }
        }

        /// <summary>
        /// Returns the person id from the given address.  Use this method with an address hat has been 
        /// deleted to recover the original person id.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="address">The deleted address.</param>
        /// <returns>The person id or null if it doesn't exist.</returns>
        public int? GetPersonIdByAddress(EcaContext context, Address address)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(address != null, "The address must not be null.");
            return GetPersonIdByEntity<Address>(context, address, x => x.PersonId);
        }

        /// <summary>
        /// Returns the person id from the given address.  Use this method with an address hat has been 
        /// deleted to recover the original person id.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="address">The deleted address.</param>
        /// <returns>The person id or null if it doesn't exist.</returns>
        public Task<int?> GetPersonIdByAddressAsync(EcaContext context, Address address)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(address != null, "The address must not be null.");
            return GetPersonIdByEntityAsync<Address>(context, address, x => x.PersonId);
        }

        /// <summary>
        /// Returns the person id from the given phone number.  Use this method with a phone number that has been 
        /// deleted to recover the original person id.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="phoneNumber">The deleted phone number.</param>
        /// <returns>The person id or null if it doesn't exist.</returns>
        public int? GetPersonIdByPhoneNumber(EcaContext context, PhoneNumber phoneNumber)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(phoneNumber != null, "The phone number must not be null.");
            return GetPersonIdByEntity<PhoneNumber>(context, phoneNumber, x => x.PersonId);
        }


        /// <summary>
        /// Returns the person id from the given phone number.  Use this method with a phone number that has been 
        /// deleted to recover the original person id.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="phoneNumber">The deleted phone number.</param>
        /// <returns>The person id or null if it doesn't exist.</returns>
        public Task<int?> GetPersonIdByPhoneNumberAsync(EcaContext context, PhoneNumber phoneNumber)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(phoneNumber != null, "The phone number must not be null.");
            return GetPersonIdByEntityAsync<PhoneNumber>(context, phoneNumber, x => x.PersonId);
        }

        /// <summary>
        /// Returns the person id from the given email address.  Use this method with an email address has been 
        /// deleted to recover the original person id.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="emailAddress">The deleted email address.</param>
        /// <returns>The person id or null if it doesn't exist.</returns>
        public int? GetPersonIdByEmailAddress(EcaContext context, EmailAddress emailAddress)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(emailAddress != null, "The email address must not be null.");
            if (emailAddress.PersonId.HasValue)
            {
                return GetPersonIdByEntity<EmailAddress>(context, emailAddress, x => x.PersonId);
            }
            else if (emailAddress.DependentId.HasValue)
            {
                var personId = CreateGetPersonIdByDependentIdQuery(context, emailAddress.DependentId.Value).FirstOrDefault();
                return personId == default(int) ? default(int?) : personId;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the person id from the given email address.  Use this method with an email address has been 
        /// deleted to recover the original person id.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="emailAddress">The deleted email address.</param>
        /// <returns>The person id or null if it doesn't exist.</returns>
        public async Task<int?> GetPersonIdByEmailAddressAsync(EcaContext context, EmailAddress emailAddress)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(emailAddress != null, "The email address must not be null.");
            if (emailAddress.PersonId.HasValue)
            {
                return await GetPersonIdByEntityAsync<EmailAddress>(context, emailAddress, x => x.PersonId);
            }
            else if (emailAddress.DependentId.HasValue)
            {
                var personId = await CreateGetPersonIdByDependentIdQuery(context, emailAddress.DependentId.Value).FirstOrDefaultAsync();
                return personId == default(int) ? default(int?) : personId;
            }
            else
            {
                return null;
            }
        }

        private IQueryable<int> CreateGetPersonIdByDependentIdQuery(EcaContext context, int dependentId)
        {
            return context.PersonDependents.Where(x => x.DependentId == dependentId).Select(x => x.PersonId);
        }


        /// <summary>
        /// Returns the person id from an entity that is related to a person.  This is useful when an entity
        /// has been deleted via the context, but the related person must be retrieved.
        /// </summary>
        /// <typeparam name="T">The entity type that is related to a person.</typeparam>
        /// <param name="context">The context to query.</param>
        /// <param name="entity">The entity that has been deleted.</param>
        /// <param name="propertySelector"></param>
        /// <returns>The id of the person or null if it does not exist.</returns>
        public int? GetPersonIdByEntity<T>(EcaContext context, T entity, Expression<Func<T, object>> propertySelector) where T : class
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(entity != null, "The entity must not be null.");
            Contract.Requires(propertySelector != null, "The property selector must not be null.");
            var databaseValues = context.GetEntry<T>(entity).GetDatabaseValues();
            var personId = databaseValues.GetValue<int?>(PropertyHelper.GetPropertyName<T>(propertySelector));
            return personId;
        }


        /// <summary>
        /// Returns the person id from an entity that is related to a person.  This is useful when an entity
        /// has been deleted via the context, but the related person must be retrieved.
        /// </summary>
        /// <typeparam name="T">The entity type that is related to a person.</typeparam>
        /// <param name="context">The context to query.</param>
        /// <param name="entity">The entity that has been deleted.</param>
        /// <param name="propertySelector"></param>
        /// <returns>The id of the person or null if it does not exist.</returns>
        public async Task<int?> GetPersonIdByEntityAsync<T>(EcaContext context, T entity, Expression<Func<T, object>> propertySelector) where T : class
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(entity != null, "The entity must not be null.");
            Contract.Requires(propertySelector != null, "The property selector must not be null.");
            var databaseValues = await context.GetEntry<T>(entity).GetDatabaseValuesAsync();
            var personId = databaseValues.GetValue<int?>(PropertyHelper.GetPropertyName<T>(propertySelector));
            return personId;
        }

        #endregion

        #region GetParticipantId
        /// <summary>
        /// Returns the participant ids of the created or modified entities that should have their validation rechecked.
        /// </summary>
        /// <param name="objects">The objects to retrieve the participant ids from.</param>
        /// <returns>The list of participant ids.</returns>
        public async Task<List<int>> GetParticipantIdsAsync(List<object> objects)
        {
            Contract.Requires(objects != null, "The objects must not be null.");
            var ids = new List<int>();
            Action<int?> addIfNotNull = (nullableId) =>
            {
                if (nullableId.HasValue)
                {
                    ids.Add(nullableId.Value);
                }
            };
            foreach (var obj in objects)
            {
                var type = obj.GetType();
                if (typeof(Person).IsAssignableFrom(type))
                {
                    var participantId = await GetParticipantIdAsync((Person)obj);
                    addIfNotNull(participantId);
                }
                else if (typeof(PersonDependentCitizenCountry).IsAssignableFrom(type))
                {
                    var participantId = await GetParticipantIdAsync((PersonDependentCitizenCountry)obj);
                    addIfNotNull(participantId);
                }
                else if (typeof(PersonDependent).IsAssignableFrom(type))
                {
                    var participantId = await GetParticipantIdAsync((PersonDependent)obj);
                    addIfNotNull(participantId);
                }
                else if (typeof(PhoneNumber).IsAssignableFrom(type))
                {
                    var phoneNumber = (PhoneNumber)obj;
                    var participantId = await GetParticipantIdAsync(phoneNumber);
                    addIfNotNull(participantId);
                }
                else if (typeof(EmailAddress).IsAssignableFrom(type))
                {
                    var email = (EmailAddress)obj;
                    var participantId = await GetParticipantIdAsync(email);
                    addIfNotNull(participantId);
                }
                else if (typeof(Address).IsAssignableFrom(type))
                {
                    var address = (Address)obj;
                    var participantId = await GetParticipantIdAsync(address);
                    addIfNotNull(participantId);
                }
                else if (typeof(Location).IsAssignableFrom(type))
                {
                    var location = (Location)obj;
                    var participantId = await GetParticipantIdAsync(location);
                    addIfNotNull(participantId);
                }
                else
                {
                    ids.Add(GetParticipantId(obj, type));
                }
            }
            return ids.Distinct().ToList();
        }

        /// <summary>
        /// Returns the participant ids of the created or modified entities that should have their validation rechecked.
        /// </summary>
        /// <param name="objects">The objects to retrieve the participant ids from.</param>
        /// <returns>The list of participant ids.</returns>
        public List<int> GetParticipantIds(List<object> objects)
        {
            Contract.Requires(objects != null, "The objects must not be null.");
            var ids = new List<int>();
            Action<int?> addIfNotNull = (nullableId) =>
            {
                if (nullableId.HasValue)
                {
                    ids.Add(nullableId.Value);
                }
            };
            foreach (var obj in objects)
            {
                var type = obj.GetType();
                if (typeof(Person).IsAssignableFrom(type))
                {
                    var participantId = GetParticipantId((Person)obj);
                    addIfNotNull(participantId);
                }
                else if (typeof(PersonDependentCitizenCountry).IsAssignableFrom(type))
                {
                    var participantId = GetParticipantId((PersonDependentCitizenCountry)obj);
                    addIfNotNull(participantId);
                }
                else if (typeof(PersonDependent).IsAssignableFrom(type))
                {
                    var participantId = GetParticipantId((PersonDependent)obj);
                    addIfNotNull(participantId);
                }
                else if (typeof(PhoneNumber).IsAssignableFrom(type))
                {
                    var phoneNumber = (PhoneNumber)obj;
                    var participantId = GetParticipantId(phoneNumber);
                    addIfNotNull(participantId);
                }
                else if (typeof(EmailAddress).IsAssignableFrom(type))
                {
                    var email = (EmailAddress)obj;
                    var participantId = GetParticipantId(email);
                    addIfNotNull(participantId);
                }
                else if (typeof(Address).IsAssignableFrom(type))
                {
                    var address = (Address)obj;
                    var participantId = GetParticipantId(address);
                    addIfNotNull(participantId);
                }
                else if (typeof(Location).IsAssignableFrom(type))
                {
                    var location = (Location)obj;
                    var participantId = GetParticipantId(location);
                    addIfNotNull(participantId);
                }
                else
                {
                    ids.Add(GetParticipantId(obj, type));
                }
            }
            return ids.Distinct().ToList();
        }

        private IQueryable<Address> CreateGetAddressByLocationIdQuery(int locationId)
        {
            return Context.Addresses.Where(x => x.LocationId == locationId);
        }

        private IQueryable<Person> CreateGetPersonByPersonDependentCitizenCountryDependentIdQuery(PersonDependentCitizenCountry personDependentCitizenCountry)
        {
            return Context.PersonDependentCitizenCountries
                .Where(x => x.Dependent != null)
                .Where(x => x.Dependent.Person != null)
                .Where(x => x.DependentId == personDependentCitizenCountry.DependentId)
                .Select(x => x.Dependent.Person);
        }

        private async Task<int?> GetParticipantIdAsync(PersonDependentCitizenCountry personDependentCitizenCountry)
        {
            int? participantId = null;
            var person = await CreateGetPersonByPersonDependentCitizenCountryDependentIdQuery(personDependentCitizenCountry).FirstOrDefaultAsync();
            if (person != null)
            {
                participantId = GetParticipantId(person);
            }
            return participantId;
        }

        private int? GetParticipantId(PersonDependentCitizenCountry personDependentCitizenCountry)
        {
            int? participantId = null;
            var person = CreateGetPersonByPersonDependentCitizenCountryDependentIdQuery(personDependentCitizenCountry).FirstOrDefault();
            if (person != null)
            {
                participantId = GetParticipantId(person);
            }
            return participantId;
        }

        private async Task<int?> GetParticipantIdAsync(Location location)
        {
            int? participantId = null;
            var address = await CreateGetAddressByLocationIdQuery(location.LocationId).FirstOrDefaultAsync();
            if (address != null)
            {
                participantId = await GetParticipantIdAsync(address);
            }
            return participantId;
        }

        private int? GetParticipantId(Location location)
        {
            int? participantId = null;
            var address = CreateGetAddressByLocationIdQuery(location.LocationId).FirstOrDefault();
            if (address != null)
            {
                participantId = GetParticipantId(address);
            }
            return participantId;
        }

        private async Task<int?> GetParticipantIdAsync(PhoneNumber phoneNumber)
        {
            int? participantId = null;
            var personId = phoneNumber.PersonId;
            if (personId.HasValue)
            {
                var person = await Context.People.FindAsync(personId.Value);
                if (person != null)
                {
                    participantId = await GetParticipantIdAsync(person);
                }
            }
            return participantId;
        }

        private int? GetParticipantId(PhoneNumber phoneNumber)
        {
            int? participantId = null;
            var personId = phoneNumber.PersonId;
            if (personId.HasValue)
            {
                var person = Context.People.Find(personId.Value);
                if (person != null)
                {
                    participantId = GetParticipantId(person);
                }
            }
            return participantId;
        }

        private int? GetParticipantId(PersonDependent personDependent)
        {
            int? participantId = null;
            var personId = personDependent.PersonId;
            var person = Context.People.Find(personId);
            if (person != null)
            {
                participantId = GetParticipantId(person);
            }
            return participantId;
        }

        private async Task<int?> GetParticipantIdAsync(PersonDependent personDependent)
        {
            int? participantId = null;
            var personId = personDependent.PersonId;
            var person = await Context.People.FindAsync(personId);
            if (person != null)
            {
                participantId = await GetParticipantIdAsync(person);
            }
            return participantId;
        }

        private async Task<int?> GetParticipantIdAsync(Address address)
        {
            int? participantId = null;
            var personId = address.PersonId;
            if (personId.HasValue)
            {
                var person = await Context.People.FindAsync(personId.Value);
                if (person != null)
                {
                    participantId = await GetParticipantIdAsync(person);
                }
            }
            return participantId;
        }

        private int? GetParticipantId(Address address)
        {
            int? participantId = null;
            var personId = address.PersonId;
            if (personId.HasValue)
            {
                var person = Context.People.Find(personId.Value);
                if (person != null)
                {
                    participantId = GetParticipantId(person);
                }
            }
            return participantId;
        }

        private async Task<int?> GetParticipantIdAsync(EmailAddress emailAddress)
        {
            int? participantId = null;
            var personId = emailAddress.PersonId;
            var dependentId = emailAddress.DependentId;
            if (personId.HasValue)
            {
                var person = await Context.People.FindAsync(personId.Value);
                if (person != null)
                {
                    participantId = await GetParticipantIdAsync(person);
                }
            }
            if (dependentId.HasValue)
            {
                var person = await Context.PersonDependents.FindAsync(dependentId.Value);
                if (person != null)
                {
                    participantId = await GetParticipantIdAsync(person);
                }
            }
            return participantId;
        }

        private int? GetParticipantId(EmailAddress emailAddress)
        {
            int? participantId = null;
            var personId = emailAddress.PersonId;
            var dependentId = emailAddress.DependentId;
            if (personId.HasValue)
            {
                var person = Context.People.Find(personId.Value);
                if (person != null)
                {
                    participantId = GetParticipantId(person);
                }
            }
            if (dependentId.HasValue)
            {
                var person = Context.PersonDependents.Find(dependentId.Value);
                if (person != null)
                {
                    participantId = GetParticipantId(person);
                }
            }
            return participantId;
        }

        private int? GetParticipantId(Person person)
        {
            int? participantId = null;
            var dto = CreateGetSimplePersonDTOsByParticipantIdQuery(person.PersonId).FirstOrDefault();
            if (dto != null && dto.ParticipantId.HasValue)
            {
                participantId = dto.ParticipantId.Value;
            }
            return participantId;
        }

        private async Task<int?> GetParticipantIdAsync(Person person)
        {
            int? participantId = null;
            var dto = await CreateGetSimplePersonDTOsByParticipantIdQuery(person.PersonId).FirstOrDefaultAsync();
            if (dto != null && dto.ParticipantId.HasValue)
            {
                participantId = dto.ParticipantId.Value;
            }
            return participantId;
        }

        private int GetParticipantId(object obj, Type type)
        {
            Contract.Requires(obj != null, "The object must not be null.");
            Contract.Requires(type != null, "The type must not be null.");
            if (typeof(Participant).IsAssignableFrom(type))
            {
                var participant = (Participant)obj;
                return participant.ParticipantId;
            }
            else if (typeof(ParticipantPerson).IsAssignableFrom(type))
            {
                var participantPerson = (ParticipantPerson)obj;
                return participantPerson.ParticipantId;
            }
            else if (typeof(ParticipantExchangeVisitor).IsAssignableFrom(type))
            {
                var visitor = (ParticipantExchangeVisitor)obj;
                return visitor.ParticipantId;
            }
            else
            {
                throw new NotSupportedException(String.Format("The object type [{0}] is not supported.", type.Name));
            }
        }

        private IQueryable<SimplePersonDTO> CreateGetSimplePersonDTOsByParticipantIdQuery(int personId)
        {
            return PersonQueries.CreateGetSimplePersonDTOsQuery(this.Context).Where(x => x.PersonId == personId);
        }

        #endregion
    }
}
