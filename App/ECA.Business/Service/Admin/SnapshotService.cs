using ECA.Business.Queries.Models.Programs;
using ECA.Core.Service;
using ECA.Data;
using System.Data.Entity;
using System.Threading.Tasks;
using ECA.Business.Queries.Admin;

namespace ECA.Business.Service.Admin
{
    public class SnapshotService : DbContextService<EcaContext>, ISnapshotService
    {
        public SnapshotService(EcaContext context) : base(context)
        {
            
        }

        public async Task<ProgramSnapshotDTO> GetProgramSnapshotAsync(int programId)
        {
            var dto = await SnapshotQueries.CreateGetProgramSnapshotDTOQuery(this.Context, programId).FirstOrDefaultAsync();
            return dto;
        }

    }
}
