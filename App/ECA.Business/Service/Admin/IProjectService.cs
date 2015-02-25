using ECA.Business.Models;
using ECA.Business.Queries.Models;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using System;
using System.Threading.Tasks;
namespace ECA.Business.Service.Admin
{
    public interface IProjectService : ISaveable
    {
        ECA.Data.Project Create(DraftProject project);

        PagedQueryResults<SimpleProjectDTO> GetProjectsByProgramId(int programId, QueryableOperator<SimpleProjectDTO> queryOperator);

        Task<PagedQueryResults<SimpleProjectDTO>> GetProjectsByProgramIdAsync(int programId, QueryableOperator<SimpleProjectDTO> queryOperator);
    }
}
