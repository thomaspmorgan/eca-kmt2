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
        public async Task<HttpResponseMessage> GetProjectAwards(int programId, int countryId, string format)
        {
            return await reportGenerator.ReportProjectAwardsAsync(programId, countryId, format);
        }

        [Route("RegionAwards")]
        public async Task<HttpResponseMessage> GetRegionAwards(int programId, string format)
        {
            return await reportGenerator.ReportRegionAwardsAsync(programId, format);
        }

        [Route("PostAwards")]
        public async Task<HttpResponseMessage> GetPostAwards(int programId, string format)
        {
            return await reportGenerator.ReportPostAwardsAsync(programId, format);
        }

        [Route("FocusAwards")]
        public async Task<HttpResponseMessage> GetFocusAwards(int programId, string format)
        {
            return await reportGenerator.ReportFocusAwardsAsync(programId, format);
        }

        [Route("FocusCategoryAwards")]
        public async Task<HttpResponseMessage> GetFocusCategoryAwards(int programId, string format)
        {
            return await reportGenerator.ReportFocusCategoryAwardsAsync(programId, format);
        }

        [Route("CountryAwards")]
        public async Task<HttpResponseMessage> GetCountryAwards(int programId, string format)
        {
            return await reportGenerator.ReportCountryAwardsAsync(programId, format);
        }

        [Route("ObjectiveAwards")]
        public async Task<HttpResponseMessage> GetObjectiveAwards(int programId, int objectiveId, string format)
        {
            return await reportGenerator.ReportObjectiveAwardsAsync(programId, objectiveId, format);
        }

        [Route("YearAwards")]
        public async Task<HttpResponseMessage> GetYearAwards(int programId, string format)
        {
            return await reportGenerator.ReportYearAwardsAsync(programId, format);
        }
    }
}
