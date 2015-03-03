using ECA.Business.Models.Programs;
using ECA.Business.Queries.Models.Programs;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
namespace ECA.Business.Service.Programs
{
    /// <summary>
    /// A ProgramService is capable of performing crud operations on a program.
    /// </summary>
    [ContractClass(typeof(IProgramService))]
    public interface IProgramService : ISaveable
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

        /// <summary>
        /// Creates a new program in the ECA system with a status of draft.
        /// </summary>
        /// <param name="draftProgram">The draft program.</param>
        /// <returns>The saved program.</returns>
        Program Create(DraftProgram draftProgram);
    }

    /// <summary>
    /// 
    /// </summary>
    [ContractClassFor(typeof(IProgramService))]
    public abstract class ProgramServiceContract : IProgramService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public PagedQueryResults<SimpleProgramDTO> GetPrograms(QueryableOperator<SimpleProgramDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            Contract.Ensures(Contract.Result<PagedQueryResults<SimpleProgramDTO>>() != null, "The value returned must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public Task<PagedQueryResults<SimpleProgramDTO>> GetProgramsAsync(QueryableOperator<SimpleProgramDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            Contract.Ensures(Contract.Result<PagedQueryResults<SimpleProgramDTO>>() != null, "The value returned must not be null.");
            return Task.FromResult<PagedQueryResults<SimpleProgramDTO>>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public Task<ProgramDTO> GetProgramByIdAsync(int programId)
        {
            return Task.FromResult<ProgramDTO>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public ProgramDTO GetProgramById(int programId)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="draftProgram"></param>
        /// <returns></returns>
        public Program Create(DraftProgram draftProgram)
        {
            Contract.Requires(draftProgram != null, "The draft program must not be null.");
            Contract.Ensures(Contract.Result<Program>() != null, "The returned program must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saveActions"></param>
        /// <returns></returns>
        public int SaveChanges(System.Collections.Generic.IList<ISaveAction> saveActions = null)
        {
            return 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saveActions"></param>
        /// <returns></returns>
        public Task<int> SaveChangesAsync(System.Collections.Generic.IList<ISaveAction> saveActions = null)
        {
            return Task.FromResult<int>(1);
        }
    }
}
