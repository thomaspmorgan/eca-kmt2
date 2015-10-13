using ECA.Business.Queries.Models.Programs;
using ECA.Core.Service;
using ECA.Data;
using System.Data.Entity;
using System.Threading.Tasks;
using ECA.Business.Queries.Admin;
using System.Collections.Generic;

namespace ECA.Business.Service.Admin
{
    public class SnapshotService : DbContextService<EcaContext>, ISnapshotService
    {
        public SnapshotService(EcaContext context, List<ISaveAction> saveActions = null) : base(context, saveActions)
        {
            
        }

        public async Task<ProgramSnapshotDTO> GetProgramSnapshotAsync(int programId)
        {
            var dto = await SnapshotQueries.CreateGetProgramSnapshotDTOQuery(this.Context, programId).FirstOrDefaultAsync();
            return dto;
        }

    }
}
