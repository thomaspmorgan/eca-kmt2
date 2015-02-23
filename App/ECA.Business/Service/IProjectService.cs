using ECA.Business.Models;
using ECA.Business.Queries.Models;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using System;
using System.Threading.Tasks;
namespace ECA.Business.Service
{
    public interface IProjectService : ISaveable
    {
        ECA.Data.Project Create(DraftProject project);

        PagedQueryResults<ProgramProject> GetProjectsByProgramId(int programId, QueryableOperator<ProgramProject> queryOperator);

        Task<PagedQueryResults<ProgramProject>> GetProjectsByProgramIdAsync(int programId, QueryableOperator<ProgramProject> queryOperator);
    }
}
