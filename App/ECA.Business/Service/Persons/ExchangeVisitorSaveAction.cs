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
        private IExchangeVisitorValidationService validationService;

        public ExchangeVisitorSaveAction(IExchangeVisitorValidationService validationService)
        {
            Contract.Requires(validationService != null, "The validation service must not be null.");
            this.validationService = validationService;

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

        public IList<object> GetCreatedEntities(DbContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var createdParticipants = GetParticipantEntities(context, EntityState.Added).ToList();
            return createdParticipants;
        }

        public IList<object> GetModifiedEntities(DbContext context)
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
                .Where(x => participantEntityTypes.Contains(x.Entity.GetType().BaseType))
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
            this.CreatedObjects = GetCreatedEntities(context).ToList();
            this.ModifiedObjects = GetModifiedEntities(context).ToList();
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
            //throw new NotImplementedException();
        }

        public async Task AfterSaveChangesAsync(DbContext context)
        {
            //var allParticipantObjects = this.ModifiedObjects.Union(this.CreatedObjects).ToList();
            //var ids = await GetParticipantIdsAsync(allParticipantObjects);
            //foreach(var id in ids)
            //{
            //    var participant = await this.Context.Participants.FindAsync(id);
            //    Contract.Assert(participant != null, "The participant should be found.");
            //    await validationService.RunParticipantSevisValidationAsync(this.SystemUser, participant.ProjectId, participant.ParticipantId);
            //}
            //await Context.SaveChangesAsync();
        }



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
            return ids.Distinct().ToList();
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
            return ids.Distinct().ToList();
        }
        #endregion
    }
}
