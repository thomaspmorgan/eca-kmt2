using System;
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
        ECA.Core.Query.PagedQueryResults<ECA.Business.Queries.Models.Programs.SimpleProgramDTO> GetPrograms(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Queries.Models.Programs.SimpleProgramDTO> queryOperator);

        /// <summary>
        /// Returns a paged, filtered, and sorted list of programs in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted list of program in the system.</returns>
        System.Threading.Tasks.Task<ECA.Core.Query.PagedQueryResults<ECA.Business.Queries.Models.Programs.SimpleProgramDTO>> GetProgramsAsync(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Queries.Models.Programs.SimpleProgramDTO> queryOperator);
    }
}
