using System.Collections.Generic;
using ECA.WebApi.Security;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Admin;

namespace ECA.WebApi.Controllers.Admin
{
    /// <summary>
    /// Retrieve values for snapshot views
    /// </summary>
    [RoutePrefix("api")]
    [Authorize]
    public class SnapshotController : ApiController
    {
        private readonly ISnapshotService service;
        private readonly IUserProvider userProvider;

        /// <summary>
        /// Creates a new SnapshotController with the given service
        /// </summary>
        /// <param name="service"></param>
        /// <param name="userProvider"></param>
        public SnapshotController(ISnapshotService service, IUserProvider userProvider)
        {
            this.service = service;
            this.userProvider = userProvider;
        }

        /// <summary>
        /// Get snapshot counts for a program
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        [ResponseType(typeof(SnapshotCountModelDTO))]
        [Route("ProgramSnapshotCounts/{programId:int}")]
        public SnapshotCountModelDTO GetProgramCounts(int programId)
        {
            var dto = service.GetProgramCounts(programId);
            return dto;
        }

        #region Graphs

        /// <summary>
        /// Get total budget by year associated with a program
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        [ResponseType(typeof(SnapshotGraphDTO))]
        [Route("ProgramBudgetByYear/{programId:int}")]
        public async Task<SnapshotGraphDTO> GetProgramBudgetByYear(int programId)
        {
            var dto = await service.GetProgramBudgetByYear(programId);
            return dto;
        }

        /// <summary>
        /// Get most funded countries associated with a program
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        [ResponseType(typeof(SnapshotDTO))]
        [Route("ProgramMostFundedCountries/{programId:int}")]
        public async Task<IEnumerable<SnapshotDTO>> GetProgramMostFundedCountries(int programId)
        {
            var dto = await service.GetProgramMostFundedCountries(programId);
            return dto;
        }

        /// <summary>
        /// Get top themes associated with a program
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<string>))]
        [Route("ProgramTopThemes/{programId:int}")]
        public async Task<IEnumerable<string>> GetProgramTopThemes(int programId)
        {
            var dto = await service.GetProgramTopThemes(programId);
            return dto;
        }

        /// <summary>
        /// Get participant locations associated with a program
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<SnapshotDTO>))]
        [Route("ProgramParticipantsByLocation/{programId:int}")]
        public async Task<IEnumerable<SnapshotDTO>> GetProgramParticipantsByLocation(int programId)
        {
            var dto = await service.GetProgramParticipantsByLocation(programId);
            return dto;
        }

        /// <summary>
        /// Get participants by year associated with a program
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        [ResponseType(typeof(SnapshotGraphDTO))]
        [Route("ProgramParticipantsByYear/{programId:int}")]
        public async Task<SnapshotGraphDTO> GetProgramParticipantsByYear(int programId)
        {
            var dto = await service.GetProgramParticipantsByYear(programId);
            return dto;
        }

        /// <summary>
        /// Get participants by gender associated with a program
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        [ResponseType(typeof(SnapshotGraphDTO))]
        [Route("ProgramParticipantGender/{programId:int}")]
        public async Task<SnapshotGraphDTO> GetProgramParticipantGender(int programId)
        {
            var dto = await service.GetProgramParticipantGender(programId);
            return dto;
        }

        /// <summary>
        /// Get participants by age associated with a program
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        [ResponseType(typeof(SnapshotGraphDTO))]
        [Route("ProgramParticipantAge/{programId:int}")]
        public async Task<SnapshotGraphDTO> GetProgramParticipantAge(int programId)
        {
            var dto = await service.GetProgramParticipantAge(programId);
            return dto;
        }

        /// <summary>
        /// Get participants by education associated with a program
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        [ResponseType(typeof(SnapshotGraphDTO))]
        [Route("ProgramParticipantEducation/{programId:int}")]
        public async Task<SnapshotGraphDTO> GetProgramParticipantEducation(int programId)
        {
            var dto = await service.GetProgramParticipantEducation(programId);
            return dto;
        }

        #endregion

    }
}