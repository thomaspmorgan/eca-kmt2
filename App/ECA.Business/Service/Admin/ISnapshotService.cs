using ECA.Business.Queries.Models.Programs;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    public interface ISnapshotService
    {
        Task<ProgramSnapshotDTO> GetProgramSnapshotAsync(int programId);

    }
}
