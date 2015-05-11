using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using System;
using System.Threading.Tasks;
namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// An IFocusService is capable of performing crud operations on Foci.
    /// </summary>
    public interface IJustificationObjectiveService
    {
        PagedQueryResults<JustificationObjectiveDTO> GetJustificationObjectivesByOfficeId(int officeId, QueryableOperator<JustificationObjectiveDTO> queryOperator);

        Task<PagedQueryResults<JustificationObjectiveDTO>> GetJustificationObjectivesByOfficeIdAsync(int officeId, QueryableOperator<JustificationObjectiveDTO> queryOperator);
    }
}
