using System.Linq;
using System.Data.Entity;
using ECA.Business.Models.Programs;
using ECA.Business.Queries.Models.Programs;
using ECA.Business.Queries.Programs;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Data;
using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using ECA.Core.Service;

namespace ECA.Business.Service.Programs
{
    /// <summary>
    /// A ProgramService is capable of performing crud operations on a program using entity framework.
    /// </summary>
    public class ProgramService : DbContextService<EcaContext>, IProgramService
    {

        /// <summary>
        /// Creates a new ProgramService with the given context to operator against.
        /// </summary>
        /// <param name="context">The context to operate on.</param>
        public ProgramService(EcaContext context) : base(context)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        #region Get

        /// <summary>
        /// Returns a paged, filtered, and sorted list of programs in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted list of program in the system.</returns>
        public PagedQueryResults<SimpleProgramDTO> GetPrograms(QueryableOperator<SimpleProgramDTO> queryOperator)
        {
            return ProgramQueries.CreateGetSimpleProgramDTOsQuery(this.Context, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
        }

        /// <summary>
        /// Returns a paged, filtered, and sorted list of programs in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted list of program in the system.</returns>
        public Task<PagedQueryResults<SimpleProgramDTO>> GetProgramsAsync(QueryableOperator<SimpleProgramDTO> queryOperator)
        {
            return ProgramQueries.CreateGetSimpleProgramDTOsQuery(this.Context, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
        }

        /// <summary>
        /// Returns the program with the given id, or null if it does not exist.
        /// </summary>
        /// <param name="programId">The program id.</param>
        /// <returns>The program, or null if it doesn't exist.</returns>
        public ProgramDTO GetProgramById(int programId)
        {
            return ProgramQueries.CreateGetPublishedProgramByIdQuery(this.Context, programId).FirstOrDefault();
        }

        /// <summary>
        /// Returns the program with the given id, or null if it does not exist.
        /// </summary>
        /// <param name="programId">The program id.</param>
        /// <returns>The program, or null if it doesn't exist.</returns>
        public Task<ProgramDTO> GetProgramByIdAsync(int programId)
        {
            return ProgramQueries.CreateGetPublishedProgramByIdQuery(this.Context, programId).FirstOrDefaultAsync();
        }

        #endregion

        #region Create

        public void Create(NewEcaProgram program)
        {

        }

        #endregion
    }
}
