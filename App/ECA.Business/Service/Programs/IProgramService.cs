using ECA.Business.Queries.Models.Programs;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using System.Threading.Tasks;
namespace ECA.Business.Service.Programs
{
    /// <summary>
    /// A ProgramService is capable of performing crud operations on a program.
    /// </summary>
    public interface IProgramService
    {
        /// <summary>
        /// Returns a paged, filtered, and sorted list of programs in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted list of program in the system.</returns>
        PagedQueryResults<SimpleProgramDTO> GetPrograms(QueryableOperator<SimpleProgramDTO> queryOperator);

        /// <summary>
        /// Returns a paged, filtered, and sorted list of programs in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted list of program in the system.</returns>
        Task<PagedQueryResults<SimpleProgramDTO>> GetProgramsAsync(QueryableOperator<SimpleProgramDTO> queryOperator);

        /// <summary>
        /// Returns the program with the given id, or null if it does not exist.
        /// </summary>
        /// <param name="programId">The program id.</param>
        /// <returns>The program, or null if it doesn't exist.</returns>
        Task<ProgramDTO> GetProgramByIdAsync(int programId);

        /// <summary>
        /// Returns the program with the given id, or null if it does not exist.
        /// </summary>
        /// <param name="programId">The program id.</param>
        /// <returns>The program, or null if it doesn't exist.</returns>
        ProgramDTO GetProgramById(int programId);
    }
}
