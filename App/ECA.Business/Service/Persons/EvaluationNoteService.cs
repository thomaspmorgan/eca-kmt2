using ECA.Business.Queries.Persons;
using ECA.Core.Exceptions;
using ECA.Core.Service;
using ECA.Core.Query;
using ECA.Core.DynamicLinq;
using ECA.Data;
using System;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using ECA.Business.Queries.Models.Persons;
using NLog;
using System.Collections.Generic;

namespace ECA.Business.Service.Persons
{
    public class EvaluationNoteService : DbContextService<EcaContext>, IEvaluationNoteService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly Action<Person, int> throwIfPersonEntityNotFound;
        private readonly Action<PersonEvaluationNote, int> throwIfEvaluationNoteNotFound;

        public EvaluationNoteService(EcaContext context, List<ISaveAction> saveActions = null) : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
            throwIfEvaluationNoteNotFound = (evalnote, id) =>
            {
                if (evalnote == null)
                {
                    throw new ModelNotFoundException(String.Format("The Evaluation Note with id [{0}] was not found.", id));
                }
            };
            
            throwIfPersonEntityNotFound = (person, id) =>
            {
                if (person == null)
                {
                    throw new ModelNotFoundException(String.Format("The person with id[{0}] was not found.", id));
                }
            };
        }

        public PagedQueryResults<EvaluationNoteDTO> Get(QueryableOperator<EvaluationNoteDTO> queryOperator)
        {
            var results = GetEvaluationNoteDTOQuery(queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            logger.Trace("Loaded evaluation notes with query operator = [{0}].", queryOperator);
            return results;
        }

        public async Task<PagedQueryResults<EvaluationNoteDTO>> GetAsync(QueryableOperator<EvaluationNoteDTO> queryOperator)
        {
            var results = await GetEvaluationNoteDTOQuery(queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            logger.Trace("Loaded evaluation notes with query operator = [{0}].", queryOperator);
            return results;
        }

        public EvaluationNoteDTO GetById(int id)
        {
            var dto = EvaluationNoteQueries.CreateGetEvaluationNoteDTOByIdQuery(this.Context, id).FirstOrDefault();
            logger.Info("Retrieved the evaluation note dto with the given id [{0}].", id);
            return dto;
        }

        public async Task<EvaluationNoteDTO> GetByIdAsync(int id)
        {
            var dto = await EvaluationNoteQueries.CreateGetEvaluationNoteDTOByIdQuery(this.Context, id).FirstOrDefaultAsync();
            logger.Info("Retrieved the evaluation note dto with the given id [{0}].", id);
            return dto;
        }

        private IQueryable<EvaluationNoteDTO> GetEvaluationNoteDTOQuery(QueryableOperator<EvaluationNoteDTO> queryOperator)
        {
            var query = GetSelectDTOQuery();
            query = query.Apply(queryOperator);
            return query;
        }

        protected IQueryable<EvaluationNoteDTO> GetSelectDTOQuery()
        {
            return Context.PersonEvaluationNotes.Select(x => new EvaluationNoteDTO
            {
                EvaluationNoteId = x.EvaluationNoteId,
                EvaluationNote = x.EvaluationNote
            });
        }

        public PersonEvaluationNote Create(NewPersonEvaluationNote personEvalnote)
        {
            var person = this.Context.People.Find(personEvalnote.PersonId);
            return DoCreate(personEvalnote, person);
        }

        public async Task<PersonEvaluationNote> CreateAsync(NewPersonEvaluationNote personEvalnote)
        {
            var person = await this.Context.People.FindAsync(personEvalnote.PersonId);
            return DoCreate(personEvalnote, person);
        }

        private PersonEvaluationNote DoCreate(NewPersonEvaluationNote personEvalnote, Person person)
        {
            throwIfPersonEntityNotFound(person, personEvalnote.PersonId);
            return personEvalnote.AddPersonEvaluationNote(person);
        }

        public void Update(UpdatedPersonEvaluationNote updatedEvalnote)
        {
            var evalnote = Context.PersonEvaluationNotes.Find(updatedEvalnote.EvaluationNoteId);
            DoUpdate(updatedEvalnote, evalnote);
        }

        public async Task UpdateAsync(UpdatedPersonEvaluationNote updatedEvalnote)
        {
            var evalnote = await Context.PersonEvaluationNotes.FindAsync(updatedEvalnote.EvaluationNoteId);
            DoUpdate(updatedEvalnote, evalnote);
        }

        private void DoUpdate(UpdatedPersonEvaluationNote updatedEvalnote, PersonEvaluationNote modelToUpdate)
        {
            Contract.Requires(updatedEvalnote != null, "The updatedEvalnote must not be null.");
            throwIfEvaluationNoteNotFound(modelToUpdate, updatedEvalnote.EvaluationNoteId);
            modelToUpdate.EvaluationNoteId = updatedEvalnote.EvaluationNoteId;
            modelToUpdate.EvaluationNote = updatedEvalnote.EvaluationNote;
            updatedEvalnote.Update.SetHistory(modelToUpdate);
        }

        public void Delete(int evalnoteId)
        {
            var evalnote = Context.PersonEvaluationNotes.Find(evalnoteId);
            DoDelete(evalnote);
        }

        public async Task DeleteAsync(int evalnoteId)
        {
            var evalnote = await Context.PersonEvaluationNotes.FindAsync(evalnoteId);
            DoDelete(evalnote);
        }

        private void DoDelete(PersonEvaluationNote evalnoteToDelete)
        {
            if (evalnoteToDelete != null)
            {
                Context.PersonEvaluationNotes.Remove(evalnoteToDelete);
            }
        }
        
    }
}
