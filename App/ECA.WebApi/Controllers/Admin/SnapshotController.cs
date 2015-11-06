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
        [Route("ProgramCounts/{programId:int}")]
        public SnapshotCountModelDTO GetProgramCounts(int programId)
        {
            var dto = service.GetProgramCounts(programId);
            return dto;
        }
        
        ///// <summary>
        ///// Get count of related projects associated with a program
        ///// </summary>
        ///// <param name="programId"></param>
        ///// <returns></returns>
        //[ResponseType(typeof(SnapshotDTO))]
        //[Route("ProgramRelatedProjectsCount/{programId:int}")]
        //public SnapshotDTO GetProgramRelatedProjectsCount(int programId)
        //{
        //    var dto = service.GetProgramRelatedProjectsCount(programId);
        //    return dto;
        //}

        ///// <summary>
        ///// Get count of participants associated with a program
        ///// </summary>
        ///// <param name="programId"></param>
        ///// <returns></returns>
        //[ResponseType(typeof(SnapshotDTO))]
        //[Route("ProgramParticipantCount/{programId:int}")]
        //public SnapshotDTO GetProgramParticipantCount(int programId)
        //{
        //    var dto = service.GetProgramParticipantCount(programId);
        //    return dto;
        //}

        ///// <summary>
        ///// Get total budget for a program
        ///// </summary>
        ///// <param name="programId"></param>
        ///// <returns></returns>
        //[ResponseType(typeof(SnapshotDTO))]
        //[Route("ProgramBudgetTotal/{programId:int}")]
        //public SnapshotDTO GetProgramBudgetTotal(int programId)
        //{
        //    var dto = service.GetProgramBudgetTotal(programId);
        //    return dto;
        //}

        ///// <summary>
        ///// Get count of funding sources associated with a program
        ///// </summary>
        ///// <param name="programId"></param>
        ///// <returns></returns>
        //[ResponseType(typeof(SnapshotDTO))]
        //[Route("ProgramFundingSourcesCount/{programId:int}")]
        //public SnapshotDTO GetProgramFundingSourcesCount(int programId)
        //{
        //    var dto = service.GetProgramFundingSourcesCount(programId);
        //    return dto;
        //}

        ///// <summary>
        ///// Get count of countries associated with a program
        ///// </summary>
        ///// <param name="programId"></param>
        ///// <returns></returns>
        //[ResponseType(typeof(SnapshotDTO))]
        //[Route("ProgramCountryCount/{programId:int}")]
        //public async Task<SnapshotDTO> GetCountryCount(int programId)
        //{
        //    var dto = await service.GetProgramCountryCountAsync(programId);
        //    return dto;
        //}

        ///// <summary>
        ///// Get count of beneficiaries associated with a program
        ///// </summary>
        ///// <param name="programId"></param>
        ///// <returns></returns>
        //[ResponseType(typeof(SnapshotDTO))]
        //[Route("ProgramBeneficiaryCount/{programId:int}")]
        //public SnapshotDTO GetProgramBeneficiaryCount(int programId)
        //{
        //    var dto = service.GetProgramBeneficiaryCount(programId);
        //    return dto;
        //}

        ///// <summary>
        ///// Get count of impact stories associated with a program
        ///// </summary>
        ///// <param name="programId"></param>
        ///// <returns></returns>
        //[ResponseType(typeof(SnapshotDTO))]
        //[Route("ProgramImpactStoryCount/{programId:int}")]
        //public async Task<SnapshotDTO> GetProgramImpactStoryCount(int programId)
        //{
        //    var dto = await service.GetProgramImpactStoryCount(programId);
        //    return dto;
        //}

        ///// <summary>
        ///// Get count of alumni associated with a program
        ///// </summary>
        ///// <param name="programId"></param>
        ///// <returns></returns>
        //[ResponseType(typeof(SnapshotDTO))]
        //[Route("ProgramAlumniCount/{programId:int}")]
        //public SnapshotDTO GetProgramAlumniCount(int programId)
        //{
        //    var dto = service.GetProgramAlumniCount(programId);
        //    return dto;
        //}

        ///// <summary>
        ///// Get count of prominence associated with a program
        ///// </summary>
        ///// <param name="programId"></param>
        ///// <returns></returns>
        //[ResponseType(typeof(SnapshotDTO))]
        //[Route("ProgramProminenceCount/{programId:int}")]
        //public async Task<SnapshotDTO> GetProgramProminenceCount(int programId)
        //{
        //    var dto = await service.GetProgramProminenceCount(programId);
        //    return dto;
        //}

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
        
    }
}