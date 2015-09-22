using ECA.Business.Queries.Models.Persons;
using ECA.Data;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ECA.Business.Queries.Persons
{
    /// <summary>
    /// Contains queries against an ECA Context for membership entities.
    /// </summary>
    public static class ProfessionEducationQueries
    {
        /// <summary>
        /// Returns a query to get membership dtos from the given context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to retrieve membership dtos.</returns>
        public static IQueryable<EducationEmploymentDTO> CreateGetProfessionEducationDTOQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return context.ProfessionEducations.Select(x => new EducationEmploymentDTO
            {
                ProfessionEducationId = x.ProfessionEducationId,
                Title = x.Title,
                Role = x.Role,
                StartDate = x.DateFrom,
                EndDate = x.DateTo,
                OrganizationId = x.OrganizationId,
                PersonOfEducation_PersonId = x.PersonOfEducation_PersonId,
                PersonOfProfession_PersonId = x.PersonOfProfession_PersonId
            });
        }

        /// <summary>
        /// Returns a query to get the membership dto for the membership entity with the given id.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="id">The membership id.</param>
        /// <returns>The membership dto with the given id.</returns>
        public static IQueryable<EducationEmploymentDTO> CreateGetProfessionEducationDTOByIdQuery(EcaContext context, int id)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return CreateGetProfessionEducationDTOQuery(context).Where(x => x.ProfessionEducationId == id);
        }
    }
}

