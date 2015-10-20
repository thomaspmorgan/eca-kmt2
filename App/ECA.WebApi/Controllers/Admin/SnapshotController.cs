using ECA.WebApi.Security;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Admin;

namespace ECA.WebApi.Controllers.Admin
{
    [RoutePrefix("api")]
    [Authorize]
    public class SnapshotController : ApiController
    {
        private readonly ISnapshotService service;
        private readonly IUserProvider userProvider;

        public SnapshotController(ISnapshotService service, IUserProvider userProvider)
        {
            this.service = service;
            this.userProvider = userProvider;
        }

        /// <summary>
        /// Get number of countries associated with program
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        [ResponseType(typeof(SnapshotDTO))]
        [Route("ProgramCountryCount/{programId:int}")]
        public async Task<SnapshotDTO> GetCountryCount(int programId)
        {
            var dto = await service.GetProgramCountryCount(programId);
            return dto;
        }

        //[ResponseType(typeof(ProgramSnapshotDTO))]
        //[Route("ProgramSnapshot/{programId:int}")]
        //public async Task<IHttpActionResult> GetAsync(int programId)
        //{
        //    if (!ModelState.IsValid) return BadRequest(ModelState);
        //    var dto = await service.GetProgramSnapshotAsync(programId);
        //    return Ok(dto);
        //}


    }
}