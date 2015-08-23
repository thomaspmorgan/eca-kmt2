using ECA.Business.Queries.Models.Persons;
using ECA.Data;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ECA.Business.Queries.Persons
{
    /// <summary>
    /// Contains queries against an ECA Context for languageProficiency entities.
    /// </summary>
    public static class LanguageProficiencyQueries
    {
        /// <summary>
        /// Returns a query to get languageProficiency dtos from the given context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to retrieve languageProficiency dtos.</returns>
        public static IQueryable<LanguageProficiencyDTO> CreateGetLanguageProficiencyDTOQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return context.PersonLanguageProficiencies.Select(x => new LanguageProficiencyDTO
            {
                LanguageId = x.LanguageId,
                PersonId = x.PersonId,
                LanguageName = x.Language.LanguageName,
                IsNativeLanguage = x.IsNativeLanguage,
                SpeakingProficiency = x.SpeakingProficiency,
                ReadingProficiency = x.ReadingProficiency,
                ComprehensionProficiency = x.ComprehensionProficiency
            });
        }

        /// <summary>
        /// Returns a query to get the languageProficiency dto for the languageProficiency entity with the given id.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="id">The languageProficiency id.</param>
        /// <returns>The languageProficiency dto with the given id.</returns>
        public static IQueryable<LanguageProficiencyDTO> CreateGetLanguageProficiencyDTOByIdQuery(EcaContext context, int languangeId, int personId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return CreateGetLanguageProficiencyDTOQuery(context).Where(x => x.LanguageId  == languangeId && x.PersonId == personId);
        }
    }
}

