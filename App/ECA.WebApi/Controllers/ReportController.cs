using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Reporting.WebForms;
using ECA.Business.Service.Reports;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net.Http.Headers;
using ECA.Business.Service;
using ECA.Reports;
using System.Threading.Tasks;

namespace ECA.WebApi.Controllers
{
    [RoutePrefix("api/Report")]
    [Authorize]
    public class ReportController : ApiController
    {

        private Generator reportGenerator;

        public ReportController(IReportService reportService)
        {
            reportGenerator = new Generator(reportService);
        }

        [Route("ProjectAwards")]
        public async Task<HttpResponseMessage> GetProjectAwards(int programId, int countryId)
        {
            return await reportGenerator.ReportProjectAwardsAsync(programId, countryId);
        }

        [Route("RegionAwards")]
        public async Task<HttpResponseMessage> GetRegionAwards(int programId)
        {
            return await reportGenerator.ReportRegionAwardsAsync(programId);
        }

        [Route("PostAwards")]
        public async Task<HttpResponseMessage> GetPostAwards(int programId)
        {
            return await reportGenerator.ReportPostAwardsAsync(programId);
        }

        [Route("FocusAwards")]
        public async Task<HttpResponseMessage> GetFocusAwards(int programId)
        {
            return await reportGenerator.ReportFocusAwardsAsync(programId);
        }

        [Route("FocusCategoryAwards")]
        public async Task<HttpResponseMessage> GetFocusCategoryAwards(int programId)
        {
            return await reportGenerator.ReportFocusCategoryAwardsAsync(programId);
        }

        [Route("CountryAwards")]
        public async Task<HttpResponseMessage> GetCountryAwards(int programId)
        {
            return await reportGenerator.ReportCountryAwardsAsync(programId);
        }

        [Route("ObjectiveAwards")]
        public async Task<HttpResponseMessage> GetObjectiveAwards(int programId, int objectiveId)
        {
            return await reportGenerator.ReportObjectiveAwardsAsync(programId, objectiveId);
        }

        [Route("YearAwards")]
        public async Task<HttpResponseMessage> GetYearAwards(int programId)
        {
            return await reportGenerator.ReportYearAwardsAsync(programId);
        }
    }
}
