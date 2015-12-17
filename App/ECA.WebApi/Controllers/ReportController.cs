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
    /// <summary>
    /// Report Controller controls access to reports
    /// </summary>
    [RoutePrefix("api/Report")]
    [Authorize]
    public class ReportController : ApiController
    {

        private Generator reportGenerator;

        /// <summary>
        /// constructor for ReportController
        /// </summary>
        /// <param name="reportService">The services that implements the reports</param>
        public ReportController(IReportService reportService)
        {
            reportGenerator = new Generator(reportService);
        }

        /// <summary>
        /// Gets a report of countries and their awards for a program
        /// </summary>
        /// <param name="programId">the program id</param>
        /// <param name="countryId">the country id</param>
        /// <param name="format">the output format of the report - pdf, xlsx, or docx</param>
        /// <returns></returns>
        [Route("ProjectAwards")]
        public async Task<HttpResponseMessage> GetProjectAwards(int programId, int countryId, string format)
        {
            return await reportGenerator.ReportProjectAwardsAsync(programId, countryId, format);
        }

        /// <summary>
        /// Gets a report of Awards for regions for a program
        /// </summary>
        /// <param name="programId">the program id</param>
        /// <param name="format">the output format of the report - pdf, xlsx, or docx</param>
        /// <returns></returns>
        [Route("RegionAwards")]
        public async Task<HttpResponseMessage> GetRegionAwards(int programId, string format)
        {
            return await reportGenerator.ReportRegionAwardsAsync(programId, format);
        }

        /// <summary>
        /// Gets a reoort of awards by post for a program
        /// </summary>
        /// <param name="programId">the program id</param>
        /// <param name="format">the output format of the report - pdf, xlsx, or docx</param>
        /// <returns></returns>
        [Route("PostAwards")]
        public async Task<HttpResponseMessage> GetPostAwards(int programId, string format)
        {
            return await reportGenerator.ReportPostAwardsAsync(programId, format);
        }

        /// <summary>
        /// Gets a report of award by focus area for a program
        /// </summary>
        /// <param name="programId">the program id</param>
        /// <param name="format">the output format of the report - pdf, xlsx, or docx</param>
        /// <returns></returns>
        [Route("FocusAwards")]
        public async Task<HttpResponseMessage> GetFocusAwards(int programId, string format)
        {
            return await reportGenerator.ReportFocusAwardsAsync(programId, format);
        }

        /// <summary>
        /// Gets a report of awards by focus area and category for a program
        /// </summary>
        /// <param name="programId">the program id</param>
        /// <param name="format">the output format of the report - pdf, xlsx, or docx</param>
        /// <returns></returns>
        [Route("FocusCategoryAwards")]
        public async Task<HttpResponseMessage> GetFocusCategoryAwards(int programId, string format)
        {
            return await reportGenerator.ReportFocusCategoryAwardsAsync(programId, format);
        }

        /// <summary>
        /// Gets a report of awards by country for a program
        /// </summary>
        /// <param name="programId">the program id</param>
        /// <param name="format">the output format of the report - pdf, xlsx, or docx</param>
        /// <returns></returns>
        [Route("CountryAwards")]
        public async Task<HttpResponseMessage> GetCountryAwards(int programId, string format)
        {
            return await reportGenerator.ReportCountryAwardsAsync(programId, format);
        }

        /// <summary>
        /// Gets a report of awards by an objective for a program
        /// </summary>
        /// <param name="programId">the program id</param>
        /// <param name="objectiveId">the objective id</param>
        /// <param name="format">the output format of the report - pdf, xlsx, or docx</param>
        /// <returns></returns>
        [Route("ObjectiveAwards")]
        public async Task<HttpResponseMessage> GetObjectiveAwards(int programId, int objectiveId, string format)
        {
            return await reportGenerator.ReportObjectiveAwardsAsync(programId, objectiveId, format);
        }

        /// <summary>
        /// Get a report of awards by Year for a program
        /// </summary>
        /// <param name="programId">the program id</param>
        /// <param name="format">the output format of the report - pdf, xlsx, or docx</param>
        /// <returns></returns>
        [Route("YearAwards")]
        public async Task<HttpResponseMessage> GetYearAwards(int programId, string format)
        {
            return await reportGenerator.ReportYearAwardsAsync(programId, format);
        }

        /// <summary>
        /// Get a report of projects with grant number for a program
        /// </summary>
        /// <param name="programId">the program id</param>
        /// <param name="format">the output format of the report - pdf, xlsx, or docx</param>
        /// <returns></returns>
        [Route("ProjectsWithGrantNumber")]
        public async Task<HttpResponseMessage> GetProjectsWithGrantNumber(int programId, string format)
        {
            return await reportGenerator.ReportProjectsWithGrantNumberAsync(programId, format);
        }
    }
}
