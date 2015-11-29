using ECA.Business.Queries.Admin;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Queries.Persons;
using ECA.Core.Exceptions;
using ECA.Core.Service;
using ECA.Core.Query;
using ECA.Core.DynamicLinq;
using ECA.Data;
using NLog;
using System;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using ECA.Business.Queries.Models.Persons;
using System.Collections.Generic;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// The LanguageProficiencyService is capable of handling crud operations on a LanguageProficiency entity
    /// </summary>
    public class LanguageProficiencyService : DbContextService<EcaContext>, ECA.Business.Service.Persons.ILanguageProficiencyService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly Action<Person, int> throwIfPersonEntityNotFound;
        private readonly Action<PersonLanguageProficiency, int> throwIfLanguageProficiencyNotFound;

        /// <summary>
        /// Creates a new instance and initializes the context..
        /// </summary>
        /// <param name="saveActions">The save actions.</param>
        /// <param name="context">The context to operate against.</param>
        public LanguageProficiencyService(EcaContext context, List<ISaveAction> saveActions = null)
            : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
            throwIfLanguageProficiencyNotFound = (languageProficiency, id) =>
            {
                if (languageProficiency == null)
                {
                    throw new ModelNotFoundException(String.Format("The LanguageProficiency with id [{0}] was not found.", id));
                }
            };

            throwIfPersonEntityNotFound = (person, id) =>
                {
                    if(person == null)
                    {
                        throw new ModelNotFoundException(String.Format("The person with id[{0}] was not found.", id));
                    }
                };
        }

        #region Get

        /// <summary>
        /// Returns a paged, filtered, and sorted instance of dtos.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted dtos.</returns>
        public PagedQueryResults<LanguageProficiencyDTO> Get(QueryableOperator<LanguageProficiencyDTO> queryOperator)
        {
            var results = GetLanguageProficiencyDTOQuery(queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            logger.Trace("Loaded languageProficiency with query operator = [{0}].", queryOperator);
            return results;
        }

        /// <summary>
        /// Returns a paged, filtered, and sorted instance of dtos.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted dtos.</returns>
        public async Task<PagedQueryResults<LanguageProficiencyDTO>> GetAsync(QueryableOperator<LanguageProficiencyDTO> queryOperator)
        {
            var results = await GetLanguageProficiencyDTOQuery(queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            logger.Trace("Loaded languageProficiency with query operator = [{0}].", queryOperator);
            return results;
        }


        private IQueryable<LanguageProficiencyDTO> GetLanguageProficiencyDTOQuery(QueryableOperator<LanguageProficiencyDTO> queryOperator)
        {
            var query = GetSelectDTOQuery();
            query = query.Apply(queryOperator);
            return query;
        }

        protected IQueryable<LanguageProficiencyDTO> GetSelectDTOQuery()
        {
            return Context.PersonLanguageProficiencies.Select(x => new LanguageProficiencyDTO
            {
                LanguageId = x.LanguageId,
                PersonId = x.PersonId,
                LanguageName = x.Language.LanguageName,
                IsNativeLanguage = x.IsNativeLanguage,
                SpeakingProficiency = x.SpeakingProficiency,
                ReadingProficiency = x.ReadingProficiency,
                ComprehensionProficiency = x.ComprehensionProficiency,
            });
        }

        public LanguageProficiencyDTO GetById(int languageId, int personId)
        {
            var dto = LanguageProficiencyQueries.CreateGetLanguageProficiencyDTOByIdQuery(this.Context, languageId, personId).FirstOrDefault();
            logger.Info("Retrieved the languageProficiency dto with the given language id [{0}], person id [{1}]", languageId, personId);
            return dto;
        }

        /// <summary>
        /// Retrieves the languageProficiency dto with the given id.
        /// </summary>
        /// <param name="id">The id of the languageProficiency.</param>
        /// <returns>The languageProficiency dto.</returns>
        public async Task<LanguageProficiencyDTO> GetByIdAsync(int languageId, int personId)
        {
            var dto = await LanguageProficiencyQueries.CreateGetLanguageProficiencyDTOByIdQuery(this.Context, languageId, personId).FirstOrDefaultAsync();
            logger.Info("Retrieved the languageProficiency dto with the given language id [{0}], person id [{1}]", languageId, personId);
            return dto;
        }
        #endregion

        #region Create
        /// <summary>
        /// Creates a new languageProficiency in the ECA system.
        /// </summary>
        /// <param name="languageProficiency">The languageProficiency.</param>
        /// <returns>The created languageProficiency entity.</returns>
        public PersonLanguageProficiency Create(NewPersonLanguageProficiency languageProficiency)
        {
            var person = this.Context.People.Find(languageProficiency.PersonId);
            if (languageProficiency.IsNativeLanguage)
            {
                SetAllLanguagesNotNative(languageProficiency.PersonId, languageProficiency.LanguageId);
            }
            return DoCreate(languageProficiency, person);
        }

        /// <summary>
        /// Creates a new languageProficiency in the ECA system.
        /// </summary>
        /// <param name="languageProficiency">The languageProficiency.</param>
        /// <returns>The created languageProficiency entity.</returns>
        public async Task<PersonLanguageProficiency> CreateAsync(NewPersonLanguageProficiency languageProficiency) 
        {
            var person = await this.Context.People.FindAsync(languageProficiency.PersonId);
            if (languageProficiency.IsNativeLanguage)
            {
                await SetAllLanguagesNotNativeAsync(languageProficiency.PersonId, languageProficiency.LanguageId);
            }
            return DoCreate(languageProficiency, person);
        }

        private PersonLanguageProficiency DoCreate(NewPersonLanguageProficiency languageProficiency, Person person) 
        {
            throwIfPersonEntityNotFound(person, languageProficiency.PersonId);
            return languageProficiency.AddPersonLanguageProficiency(person);
        }

        private async Task SetAllLanguagesNotNativeAsync(int personId, int languageId)
        {
            await this.Context.PersonLanguageProficiencies.Where(x => x.PersonId == personId  && x.LanguageId != languageId).ForEachAsync(x => x.IsNativeLanguage = false);
        }

        private void SetAllLanguagesNotNative(int personId, int languageId)
        {
            var personLanguageProficiencies = this.Context.PersonLanguageProficiencies.Where(x => x.PersonId == personId && x.LanguageId != languageId);
            foreach (var language in personLanguageProficiencies)
                language.IsNativeLanguage = false;
        }
        #endregion

        #region Update

        /// <summary>
        /// Updates the ECA system's languageProficiency data with the given updated languageProficiency.
        /// </summary>
        /// <param name="updatedLanguageProficiency">The updated languageProficiency.</param>
        public void Update(UpdatedPersonLanguageProficiency updatedLanguageProficiency)
        {
            var languageProficiency = Context.PersonLanguageProficiencies.Find(updatedLanguageProficiency.LanguageId, updatedLanguageProficiency.PersonId);
            DoUpdate(updatedLanguageProficiency, languageProficiency);
        }

        /// <summary>
        /// Updates the ECA system's LanguageProficiency data with the given updated updatedLanguageProficiency.
        /// </summary>
        /// <param name="updatedLanguageProficiency">The updated LanguageProficiency.</param>
        public async Task UpdateAsync(UpdatedPersonLanguageProficiency updatedLanguageProficiency)
        {
            var languageProficiency = await Context.PersonLanguageProficiencies.FindAsync(updatedLanguageProficiency.LanguageId, updatedLanguageProficiency.PersonId);
            DoUpdate(updatedLanguageProficiency, languageProficiency);
        }

        private void DoUpdate(UpdatedPersonLanguageProficiency updatedLanguageProficiency, PersonLanguageProficiency modelToUpdate)
        {
            Contract.Requires(updatedLanguageProficiency != null, "The updatedLanguageProficiency must not be null.");
            throwIfLanguageProficiencyNotFound(modelToUpdate, updatedLanguageProficiency.LanguageId);
            if(updatedLanguageProficiency.NewLanguageId.HasValue && updatedLanguageProficiency.NewLanguageId != modelToUpdate.LanguageId)
            {
                DoDelete(modelToUpdate);
                User user = updatedLanguageProficiency.Update.User;
                var newLanguageProficiency = new NewPersonLanguageProficiency(updatedLanguageProficiency.Update.User, updatedLanguageProficiency.PersonId, updatedLanguageProficiency.NewLanguageId.Value,
                    updatedLanguageProficiency.IsNativeLanguage, updatedLanguageProficiency.SpeakingProficiency, updatedLanguageProficiency.ReadingProficiency, updatedLanguageProficiency.ComprehensionProficiency);
                var person = Context.People.Find(updatedLanguageProficiency.PersonId);
                var temp = DoCreate(newLanguageProficiency, person);
            }
            else
            {
                modelToUpdate.LanguageId = (updatedLanguageProficiency.NewLanguageId.HasValue) ? updatedLanguageProficiency.NewLanguageId.Value : updatedLanguageProficiency.LanguageId;
                modelToUpdate.IsNativeLanguage = updatedLanguageProficiency.IsNativeLanguage;
                modelToUpdate.SpeakingProficiency = updatedLanguageProficiency.SpeakingProficiency;
                modelToUpdate.ReadingProficiency = updatedLanguageProficiency.ReadingProficiency;
                modelToUpdate.ComprehensionProficiency = updatedLanguageProficiency.ComprehensionProficiency;
                updatedLanguageProficiency.Update.SetHistory(modelToUpdate);
            }
            if (updatedLanguageProficiency.IsNativeLanguage)
            {
                SetAllLanguagesNotNative(updatedLanguageProficiency.PersonId, updatedLanguageProficiency.LanguageId);
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// Deletes the LanguageProficiency entry with the given id.
        /// </summary>
        /// <param name="languageId">The languageId of the LanguageProficiency to delete.</param>
        /// <param name="personId">The languageId of the LanguageProficiency to delete.</param>
        public void Delete(int languageId, int personId)
        {
            var languageProficiency = Context.PersonLanguageProficiencies.Find(languageId, personId);
            DoDelete(languageProficiency);
        }

        /// <summary>
        /// Deletes the LanguageProficiency entry with the given id.
        /// </summary>
        /// <param name="languageId">The languageId of the LanguageProficiency to delete.</param>
        /// <param name="personId">The languageId of the LanguageProficiency to delete.</param>
        public async Task DeleteAsync(int languageId, int personId)
        {
            var languageProficiency = await Context.PersonLanguageProficiencies.FindAsync(languageId, personId);
            DoDelete(languageProficiency);
        }

        private void DoDelete(PersonLanguageProficiency languageProficiencyToDelete)
        {
            if (languageProficiencyToDelete != null)
            {
                Context.PersonLanguageProficiencies.Remove(languageProficiencyToDelete);
            }
        }
        #endregion
    }
}
