using ECA.Business.Queries.Persons;
using ECA.Business.Validation.Model;
using ECA.Core.Service;
using ECA.Data;
using FluentValidation;
using FluentValidation.Results;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    public class ExchangeVisitorSaveAction : ISaveAction
    {
        private IExchangeVisitorService exchangeVisitorService;

        /// <summary>
        /// Creates a new save action with the document type id and the app settings.
        /// </summary>
        /// <param name="documentTypeId">The document type id.  This id should correspond to the same guid
        /// as the document type in the document configuration.</param>
        public ExchangeVisitorSaveAction(IExchangeVisitorService exchangeVisitorService)
        {
            Contract.Requires(exchangeVisitorService != null, "The exchange visitor service must not be null.");
            this.exchangeVisitorService = exchangeVisitorService;

            this.SystemUser = new User(1);//come back to this
        }

        /// <summary>
        /// Gets the system user.
        /// </summary>
        public User SystemUser { get; private set; }

        /// <summary>
        /// Gets the added entities.
        /// </summary>
        public List<object> CreatedObjects { get; private set; }

        /// <summary>
        /// Gets the modified entities.
        /// </summary>
        public List<object> ModifiedObjects { get; private set; }

        public EcaContext Context { get; set; }

        public IList<object> GetCreatedParticipants(DbContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var createdParticipants = GetParticipantEntities(context, EntityState.Added).ToList();
            return createdParticipants;
        }

        public IList<object> GetModifiedDocumentEntities(DbContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var modifiedParticipants = GetParticipantEntities(context, EntityState.Modified).ToList();
            return modifiedParticipants;
        }

        public IList<object> GetParticipantEntities(DbContext context, EntityState state)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var changedEntities = context.ChangeTracker.Entries().Where(x => x.State == state).ToList();

            var participantEntityTypes = GetParticipantTypes();
            var changedParticipantEntities = changedEntities
                .Where(x => participantEntityTypes.Contains(x.GetType()))
                .Select(x => x.Entity)
                .ToList();
            return changedParticipantEntities;
        }

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
            this.CreatedObjects = GetCreatedParticipants(context).ToList();
            this.ModifiedObjects = GetModifiedDocumentEntities(context).ToList();
        }

        #region SaveAction
        public void BeforeSaveChanges(DbContext context)
        {
            OnBeforeSaveChanges(context);
        }

        public Task BeforeSaveChangesAsync(DbContext context)
        {
            OnBeforeSaveChanges(context);
            return Task.FromResult<object>(null);
        }

        public void AfterSaveChanges(DbContext context)
        {
            throw new NotImplementedException();
        }

        public async Task AfterSaveChangesAsync(DbContext context)
        {
            //var addedParticipantIds = await GetParticipantIdsAsync(this.CreatedObjects);
            //var modifiedParticipantIds = await GetParticipantIdsAsync(this.ModifiedObjects);
            //var allParticipantIds = addedParticipantIds.Union(modifiedParticipantIds).Distinct().ToList();

            //var validatableParticipantIds = await ExchangeVisitorQueries.CreateGetValidatableParticipantsByParticipantIdsQuery(this.Context, allParticipantIds).ToListAsync();
            //var nonValidatableParticipantIds = allParticipantIds.Except(validatableParticipantIds.Select(x => x.ParticipantId).ToList());
            //if (validatableParticipantIds.Count > 0)
            //{
            //    foreach(var validatableParticipants in validatableParticipantIds)
            //    {
            //        var participant = await this.Context.ParticipantPersons.FindAsync(validatableParticipants.ParticipantId);
            //        ValidationResult result;
            //        if (String.IsNullOrWhiteSpace(participant.SevisId))
            //        {
            //            var createExchangeVisitor = await exchangeVisitorService.GetCreateExchangeVisitorAsync(this.SystemUser, validatableParticipants.ProjectId, participant.ParticipantId);
            //            var validator = new CreateExchVisitorValidator();
            //            result = await validator.ValidateAsync(createExchangeVisitor);

            //        }
            //        else
            //        {
            //            var updateExchangeVisitor = await exchangeVisitorService.GetUpdateExchangeVisitorAsync(this.SystemUser, validatableParticipants.ProjectId, participant.ParticipantId);
            //            var validator = new UpdateExchVisitorValidator();
            //            result = await validator.ValidateAsync(updateExchangeVisitor);
            //        }
            //        await SaveParticipantPersonValidationResultsAsync(participant.ParticipantId, result);
            //        UpdateValidatedParticipantPersonSevisCommStatus(participant.ParticipantId, result);                    
            //    };
            //}
        }

        //public async Task SaveParticipantPersonValidationResultsAsync(int participantId, ValidationResult result)
        //{
        //    if (!result.IsValid)
        //    {
        //        var participantPerson = await Context.ParticipantPersons.FindAsync(participantId);
        //        participantPerson.SevisValidationResult = JsonConvert.SerializeObject(result);
        //    }
        //}

        //public ParticipantPersonSevisCommStatus UpdateValidatedParticipantPersonSevisCommStatus(int participantId, ValidationResult result)
        //{
        //    if(result.IsValid && result.Errors.Count == 0)
        //    {
        //        return AddParticipantPersonSevisCommStatus(participantId, SevisCommStatus.QueuedToSubmit.Id);
        //    }
        //    else
        //    {
        //        return AddParticipantPersonSevisCommStatus(participantId, SevisCommStatus.InformationRequired.Id);
        //    }
        //}

        //public List<ParticipantPersonSevisCommStatus> UpdateNonValidatableParticipantSevisCommStatus(List<int> participantIds)
        //{
        //    var statuses = new List<ParticipantPersonSevisCommStatus>();
        //    foreach (var participantId in participantIds)
        //    {
        //        statuses.Add(AddParticipantPersonSevisCommStatus(participantId, SevisCommStatus.InformationRequired.Id));
        //    }
        //    return statuses;
        //}

        



        public async Task<List<int>> GetParticipantIdsAsync(List<object> objects)
        {
            Contract.Requires(objects != null, "The objects must not be null.");
            var ids = new List<int>();
            foreach (var obj in objects)
            {
                var type = obj.GetType();
                if (type == typeof(Participant))
                {
                    var participant = (Participant)obj;
                    ids.Add(participant.ParticipantId);
                }
                else if (type == typeof(ParticipantPerson))
                {
                    var participantPerson = (ParticipantPerson)obj;
                    ids.Add(participantPerson.ParticipantId);
                }
                else if (type == typeof(ParticipantExchangeVisitor))
                {
                    var visitor = (ParticipantExchangeVisitor)obj;
                    ids.Add(visitor.ParticipantId);
                }
                else if (type == typeof(Person))
                {
                    throw new NotSupportedException();
                }
                else
                {
                    throw new NotSupportedException(String.Format("The object type [{0}] is not supported.", type.Name));
                }
            }
            return ids;
        }

        public List<int> GetParticipantIds(List<object> objects)
        {
            Contract.Requires(objects != null, "The objects must not be null.");
            var ids = new List<int>();
            foreach (var obj in objects)
            {
                var type = obj.GetType();
                if (type == typeof(Participant))
                {
                    var participant = (Participant)obj;
                    ids.Add(participant.ParticipantId);
                }
                else if (type == typeof(ParticipantPerson))
                {
                    var participantPerson = (ParticipantPerson)obj;
                    ids.Add(participantPerson.ParticipantId);
                }
                else if (type == typeof(ParticipantExchangeVisitor))
                {
                    var visitor = (ParticipantExchangeVisitor)obj;
                    ids.Add(visitor.ParticipantId);
                }
                else if (type == typeof(Person))
                {
                    throw new NotSupportedException();
                }
                else
                {
                    throw new NotSupportedException(String.Format("The object type [{0}] is not supported.", type.Name));
                }
            }
            return ids;
        }
        #endregion
    }
}
