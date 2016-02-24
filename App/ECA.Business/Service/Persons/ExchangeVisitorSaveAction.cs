using ECA.Business.Queries.Models.Persons;
using ECA.Business.Queries.Persons;
using ECA.Core.Service;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
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
        public ExchangeVisitorSaveAction(IExchangeVisitorValidationService validationService, Func<User> userProvider)
        {
            Contract.Requires(validationService != null, "The validation service must not be null.");
            Contract.Requires(userProvider != null, "The user provider must not be null.");
            this.validationService = validationService;
            this.User = userProvider();
            this.CreatedObjects = new List<object>();
            this.ModifiedObjects = new List<object>();
        }

        /// <summary>
        /// Gets the current user.
        /// </summary>
        public User User { get; private set; }

        /// <summary>
        /// Gets the added entities.
        /// </summary>
        public List<object> CreatedObjects { get; private set; }

        /// <summary>
        /// Gets the modified entities.
        /// </summary>
        public List<object> ModifiedObjects { get; private set; }

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
            var createdParticipants = GetParticipantEntities(context, EntityState.Added).ToList();
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
            var modifiedParticipants = GetParticipantEntities(context, EntityState.Modified).ToList();
            return modifiedParticipants;
        }

        /// <summary>
        /// Returns the list of objects whose entity type should be watched for potential participant changes.
        /// </summary>
        /// <param name="context">The context to get entities from.</param>
        /// <param name="state">The entity state.</param>
        /// <returns>The list of objects that relate to a participant.</returns>
        public IList<object> GetParticipantEntities(DbContext context, EntityState state)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var changedEntities = context.ChangeTracker.Entries().Where(x => x.State == state).ToList();

            var participantEntityTypes = GetParticipantTypes();
            var changedParticipantEntities = changedEntities
                .Where(x => participantEntityTypes.Contains(x.Entity.GetType().BaseType))
                .Select(x => x.Entity)
                .ToList();
            return changedParticipantEntities;
        }

        /// <summary>
        /// Returns the list of object types that need to be watched for potential participant changes.
        /// </summary>
        /// <returns>The list of object types that need to be watched for potential participant changes.</returns>
        public List<Type> GetParticipantTypes()
        {
            var participantEntityTypes = new List<Type>();
            participantEntityTypes.Add(typeof(Participant));
            participantEntityTypes.Add(typeof(ParticipantPerson));
            participantEntityTypes.Add(typeof(ParticipantExchangeVisitor));
            participantEntityTypes.Add(typeof(Person));
            return participantEntityTypes;
        }

        private void OnBeforeSaveChanges(DbContext context)
        {
            Contract.Requires(context is EcaContext, "The given context must be an EcaContext instance.");
            this.Context = (EcaContext)context;
            this.CreatedObjects = GetCreatedEntities(context).ToList();
            this.ModifiedObjects = GetModifiedEntities(context).ToList();
        }

        #region SaveAction

        /// <summary>
        /// Locates participant related entities that have been created or modified in the context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The task.</returns>
        public void BeforeSaveChanges(DbContext context)
        {
            OnBeforeSaveChanges(context);
        }

        /// <summary>
        /// Locates participant related entities that have been created or modified in the context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The task.</returns>
        public Task BeforeSaveChangesAsync(DbContext context)
        {
            OnBeforeSaveChanges(context);
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Performs sevis validation on participant related entities if any were found.
        /// </summary>
        /// <param name="context">The context that has created or modified participant related entities.</param>
        public void AfterSaveChanges(DbContext context)
        {
            var allParticipantObjects = GetUnionedCreatedAndModifiedObjects();
            var ids = GetParticipantIds(allParticipantObjects);
            if (ids.Count > 0)
            {
                foreach (var id in ids)
                {
                    var participant = this.Context.Participants.Find(id);
                    Contract.Assert(participant != null, "The participant should be found.");
                    validationService.RunParticipantSevisValidation(this.User, participant.ProjectId, participant.ParticipantId);
                }
                Context.SaveChanges();
            }
        }

        /// <summary>
        /// Performs sevis validation on participant related entities if any were found.
        /// </summary>
        /// <param name="context">The context that has created or modified participant related entities.</param>
        public async Task AfterSaveChangesAsync(DbContext context)
        {
            var allParticipantObjects = GetUnionedCreatedAndModifiedObjects();
            var ids = await GetParticipantIdsAsync(allParticipantObjects);
            if (ids.Count > 0)
            {
                foreach (var id in ids)
                {
                    var participant = await this.Context.Participants.FindAsync(id);
                    Contract.Assert(participant != null, "The participant should be found.");
                    await validationService.RunParticipantSevisValidationAsync(this.User, participant.ProjectId, participant.ParticipantId);
                }
                await Context.SaveChangesAsync();
            }
        }

        public List<object> GetUnionedCreatedAndModifiedObjects()
        {
            return this.ModifiedObjects.Union(this.CreatedObjects).ToList();
        }

        /// <summary>
        /// Returns the participant ids of the created or modified entities that should have their validation rechecked.
        /// </summary>
        /// <param name="objects">The objects to retrieve the participant ids from.</param>
        /// <returns>The list of participant ids.</returns>
        public async Task<List<int>> GetParticipantIdsAsync(List<object> objects)
        {
            Contract.Requires(objects != null, "The objects must not be null.");
            var ids = new List<int>();
            foreach (var obj in objects)
            {
                var baseType = obj.GetType().BaseType;
                if (baseType == typeof(Person))
                {
                    var person = (Person)obj;
                    if (person.PersonTypeId == PersonType.Participant.Id)
                    {
                        var dto = await CreateGetSimplePersonDTOsByParticipantIdQuery(person.PersonId).FirstOrDefaultAsync();
                        if (dto != null && dto.ParticipantId.HasValue)
                        {
                            ids.Add(dto.ParticipantId.Value);
                        }
                    }
                    else if (person.PersonTypeId == PersonType.Dependent.Id)
                    {
                        var dto = await PersonQueries.CreateGetRelatedPersonByDependentFamilyMemberQuery(this.Context, person.PersonId).FirstOrDefaultAsync();
                        if (dto != null && dto.ParticipantId.HasValue)
                        {
                            ids.Add(dto.ParticipantId.Value);
                        }
                    }
                    else
                    {
                        throw new NotSupportedException("The person by person type is not supported.");
                    }
                }
                else
                {
                    ids.Add(GetParticipantId(obj, baseType));
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
            foreach (var obj in objects)
            {
                var baseType = obj.GetType().BaseType;
                if (baseType == typeof(Person))
                {
                    var person = (Person)obj;
                    if (person.PersonTypeId == PersonType.Participant.Id)
                    {
                        var dto = CreateGetSimplePersonDTOsByParticipantIdQuery(person.PersonId).FirstOrDefault();
                        if (dto != null && dto.ParticipantId.HasValue)
                        {
                            ids.Add(dto.ParticipantId.Value);
                        }
                    }
                    else if (person.PersonTypeId == PersonType.Dependent.Id)
                    {
                        var dto = PersonQueries.CreateGetRelatedPersonByDependentFamilyMemberQuery(this.Context, person.PersonId).FirstOrDefault();
                        if (dto != null && dto.ParticipantId.HasValue)
                        {
                            ids.Add(dto.ParticipantId.Value);
                        }
                    }
                    else
                    {
                        throw new NotSupportedException("The person by person type is not supported.");
                    }
                }
                else
                {
                    ids.Add(GetParticipantId(obj, baseType));
                }
            }
            return ids.Distinct().ToList();
        }

        private int GetParticipantId(object obj, Type type)
        {
            Contract.Requires(obj != null, "The object must not be null.");
            Contract.Requires(type != null, "The type must not be null.");
            if (type == typeof(Participant))
            {
                var participant = (Participant)obj;
                return participant.ParticipantId;
            }
            else if (type == typeof(ParticipantPerson))
            {
                var participantPerson = (ParticipantPerson)obj;
                return participantPerson.ParticipantId;
            }
            else if (type == typeof(ParticipantExchangeVisitor))
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
