using ECA.Business.Models.Programs;
using ECA.Business.Queries.Models.Programs;
using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace ECA.Business.Service.Programs
{
    /// <summary>
    /// A ProgramService is capable of performing crud operations on a program.
    /// </summary>
    [ContractClass(typeof(ProgramServiceContract))]
    public interface IProgramService : ISaveable
    {
        /// <summary>
        /// Returns a paged, filtered, and sorted list of programs in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted list of program in the system.</returns>
        PagedQueryResults<OrganizationProgramDTO> GetPrograms(QueryableOperator<OrganizationProgramDTO> queryOperator);

        /// <summary>
        /// Returns a paged, filtered, and sorted list of programs in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted list of program in the system.</returns>
        Task<PagedQueryResults<OrganizationProgramDTO>> GetProgramsAsync(QueryableOperator<OrganizationProgramDTO> queryOperator);
        
        /// <summary>
        /// Returns the list of programs in a hierarchy.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The programs.</returns>
        PagedQueryResults<OrganizationProgramDTO> GetProgramsHierarchy(QueryableOperator<OrganizationProgramDTO> queryOperator);
        
        /// <summary>
        /// Returns the list of programs in a hierarchy.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The programs.</returns>
        Task<PagedQueryResults<OrganizationProgramDTO>> GetProgramsHierarchyAsync(QueryableOperator<OrganizationProgramDTO> queryOperator);
       
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

        Task<PagedQueryResults<OrganizationProgramDTO>> GetSubprogramsByProgramAsync(int programId, QueryableOperator<OrganizationProgramDTO> queryOperator);

        PagedQueryResults<OrganizationProgramDTO> GetSubprogramsByProgram(int programId, QueryableOperator<OrganizationProgramDTO> queryOperator);

        /// <summary>
        /// Creates a new program in the ECA system with a status of draft.
        /// </summary>
        /// <param name="draftProgram">The draft program.</param>
        /// <returns>The saved program.</returns>
        Program Create(DraftProgram draftProgram);

        /// <summary>
        /// Creates a new program in the ECA system with a status of draft.
        /// </summary>
        /// <param name="draftProgram">The draft program.</param>
        /// <returns>The saved program.</returns>
        Task<Program> CreateAsync(DraftProgram draftProgram);

        /// <summary>
        /// Updates the system's program with the given updated program.
        /// </summary>
        /// <param name="updatedProgram">The updated program.</param>
        void Update(EcaProgram updatedProgram);

        /// <summary>
        /// Updates the system's program with the given updated program.
        /// </summary>
        /// <param name="updatedProgram">The updated program.</param>
        Task UpdateAsync(EcaProgram updatedProgram);

        /// <summary>
        /// Returns a list of parent programs to the program with the given id.  The root program will be first.
        /// </summary>
        /// <param name="programId">The id of the program to get parent programs for.</param>
        /// <returns>The list of parent programs, root first.</returns>
        List<OrganizationProgramDTO> GetParentPrograms(int programId);

        /// <summary>
        /// Returns a list of parent programs to the program with the given id.  The root program will be first.
        /// </summary>
        /// <param name="programId">The id of the program to get parent programs for.</param>
        /// <returns>The list of parent programs, root first.</returns>
        Task<List<OrganizationProgramDTO>> GetParentProgramsAsync(int programId);
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
        public PagedQueryResults<OrganizationProgramDTO> GetPrograms(QueryableOperator<OrganizationProgramDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            Contract.Ensures(Contract.Result<PagedQueryResults<OrganizationProgramDTO>>() != null, "The value returned must not be null.");
            return null;
        }
        public PagedQueryResults<OrganizationProgramDTO> GetProgramsHierarchy(QueryableOperator<OrganizationProgramDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            Contract.Ensures(Contract.Result<PagedQueryResults<OrganizationProgramDTO>>() != null, "The value returned must not be null.");
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public Task<PagedQueryResults<OrganizationProgramDTO>> GetProgramsAsync(QueryableOperator<OrganizationProgramDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            Contract.Ensures(Contract.Result<PagedQueryResults<OrganizationProgramDTO>>() != null, "The value returned must not be null.");
            return Task.FromResult<PagedQueryResults<OrganizationProgramDTO>>(null);
        }
        public Task<PagedQueryResults<OrganizationProgramDTO>> GetProgramsHierarchyAsync(QueryableOperator<OrganizationProgramDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            Contract.Ensures(Contract.Result<PagedQueryResults<OrganizationProgramDTO>>() != null, "The value returned must not be null.");
            return Task.FromResult<PagedQueryResults<OrganizationProgramDTO>>(null);
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
        /// <param name="programId"></param>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public Task<PagedQueryResults<OrganizationProgramDTO>> GetSubprogramsByProgramAsync(int programId, QueryableOperator<OrganizationProgramDTO> queryOperator)
        {
            return Task.FromResult<PagedQueryResults<OrganizationProgramDTO>>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="programId"></param>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public PagedQueryResults<OrganizationProgramDTO> GetSubprogramsByProgram(int programId, QueryableOperator<OrganizationProgramDTO> queryOperator)
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
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="draftProgram"></param>
        /// <returns></returns>
        public Task<Program> CreateAsync(DraftProgram draftProgram)
        {
            Contract.Requires(draftProgram != null, "The draft program must not be null.");
            return Task.FromResult<Program>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            return 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<int> SaveChangesAsync()
        {
            return Task.FromResult<int>(1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatedProgram"></param>
        public void Update(EcaProgram updatedProgram)
        {
            Contract.Requires(updatedProgram != null, "The updated program must not be null.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatedProgram"></param>
        /// <returns></returns>
        public Task UpdateAsync(EcaProgram updatedProgram)
        {
            Contract.Requires(updatedProgram != null, "The updated program must not be null.");
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public List<OrganizationProgramDTO> GetParentPrograms(int programId)
        {
            Contract.Ensures(Contract.Result<List<OrganizationProgramDTO>>() != null, "The value returned must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public Task<List<OrganizationProgramDTO>> GetParentProgramsAsync(int programId)
        {
            Contract.Ensures(Contract.Result<IList<OrganizationProgramDTO>>() != null, "The value returned must not be null.");
            return Task.FromResult<List<OrganizationProgramDTO>>(null);
        }
    }
}
