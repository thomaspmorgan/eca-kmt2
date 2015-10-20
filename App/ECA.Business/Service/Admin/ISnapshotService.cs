using ECA.Business.Queries.Models.Programs;
using System.Threading.Tasks;
using ECA.Business.Queries.Models.Admin;

namespace ECA.Business.Service.Admin
{
    public interface ISnapshotService
    {
        Task<SnapshotDTO> GetProgramCountryCount(int programId);
        
        Task<ProgramSnapshotDTO> GetProgramSnapshotAsync(int programId);

    }
}
