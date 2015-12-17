using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Reporting.WebForms;
using ECA.Business.Service.Reports;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net.Http.Headers;
using ECA.Business.Service;


namespace ECA.Reports
{
    /// <summary>
    /// Generates reports for the data supplied by the report service, for the report controller
    /// </summary>
    public class Generator
    {
        private IReportService reportService;

        /// <summary>
        /// The constructor for the report generator
        /// </summary>
        /// <param name="reportService">the service that gets the data for the reports</param>
        public Generator(IReportService reportService)
        {
            this.reportService = reportService;
        }

        /// <summary>
        /// Generates a report for Regions by program
        /// </summary>
        /// <param name="programId">the program id</param>
        /// <param name="format">the format of the report (pdf, xlsx, docx)</param>
        /// <returns></returns>
        public  async Task<HttpResponseMessage> ReportRegionAwardsAsync(int programId, string format)
        {

            byte[] bytes;

            var reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            var regionAwards = await reportService.GetRegionAwardsAsync(programId);
            var rds = new ReportDataSource("RegionAwardDS", regionAwards);
            string programName = reportService.GetProgramName(programId);
            reportViewer.Reset();
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.LocalReport.ReportEmbeddedResource = "ECA.Reports.RegionAwards.rdlc";
            reportViewer.LocalReport.SetParameters(new ReportParameter("Program", programName));


            return GetReport(reportViewer, format, out bytes);
        }

        /// <summary>
        /// Generates a report for a program by country
        /// </summary>
        /// <param name="programId">the program id</param>
        /// <param name="countryId">the country id</param>
        /// <param name="format">the format of the report (pdf, xlsx, docx)</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> ReportProjectAwardsAsync(int programId, int countryId, string format)
        {
            byte[] bytes;

            var reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            var projectAwards = await reportService.GetProjectAwardsAsync(programId, countryId);
            var rds = new ReportDataSource("ProjectAwardDS", projectAwards);
            string programName = reportService.GetProgramName(programId);
            string countryName = reportService.GetCountryName(countryId);
            reportViewer.Reset();
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.LocalReport.ReportEmbeddedResource = "ECA.Reports.ProjectAwards.rdlc";
            reportViewer.LocalReport.SetParameters(new ReportParameter("Country", countryName));
            reportViewer.LocalReport.SetParameters(new ReportParameter("Program", programName));

            return GetReport(reportViewer, format, out bytes);
        }


        /// <summary>
        /// Generates a report of Awards by Posts for a program
        /// </summary>
        /// <param name="programId">the program id</param>
        /// <param name="format">the format of the report (pdf, xlsx, docx)</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> ReportPostAwardsAsync(int programId, string format)
        {
            byte[] bytes;

            var reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            var postAwards = await reportService.GetPostAwardsAsync(programId);
            var rds = new ReportDataSource("PostAwardDS", postAwards);
            string programName = reportService.GetProgramName(programId);

            reportViewer.Reset();
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.LocalReport.ReportEmbeddedResource = "ECA.Reports.PostAwards.rdlc";
            reportViewer.LocalReport.SetParameters(new ReportParameter("Program", programName));

            return GetReport(reportViewer, format, out bytes);
        }

        /// <summary>
        /// Generates a report of Awards by focus for a program
        /// </summary>
        /// <param name="programId">the program id</param>
        /// <param name="format">the format of the report (pdf, xlsx, docx)</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> ReportFocusAwardsAsync(int programId, string format)
        {
            byte[] bytes;

            var reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            var focusAwards = await reportService.GetFocusAwardsAsync(programId);
            var rds = new ReportDataSource("FocusAwardDS", focusAwards);
            string programName = reportService.GetProgramName(programId);

            reportViewer.Reset();
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.LocalReport.ReportEmbeddedResource = "ECA.Reports.FocusAwards.rdlc";
            reportViewer.LocalReport.SetParameters(new ReportParameter("Program", programName));

            return GetReport(reportViewer, format, out bytes);
        }

        /// <summary>
        /// Generates a report of Awards by Focus and Category for a program
        /// </summary>
        /// <param name="programId">the program id</param>
        /// <param name="format">the format of the report (pdf, xlsx, docx)</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> ReportFocusCategoryAwardsAsync(int programId, string format)
        {
            byte[] bytes;

            var reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            var focusCategoryAwards = await reportService.GetFocusCategoryAwardsAsync(programId);
            var rds = new ReportDataSource("FocusCategoryAwardDS", focusCategoryAwards);
            string programName = reportService.GetProgramName(programId);

            reportViewer.Reset();
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.LocalReport.ReportEmbeddedResource = "ECA.Reports.FocusCategoryAwards.rdlc";
            reportViewer.LocalReport.SetParameters(new ReportParameter("Program", programName));

            return GetReport(reportViewer, format, out bytes);
        }

        /// <summary>
        /// Generates a report of Awards by country for a program
        /// </summary>
        /// <param name="programId">the program id</param>
        /// <param name="format">the format of the report (pdf, xlsx, docx)</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> ReportCountryAwardsAsync(int programId, string format)
        {
            byte[] bytes;

            var reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            var countryAwards = await reportService.GetCountryAwardsAsync(programId);
            var rds = new ReportDataSource("CountryAwardDS", countryAwards);
            string programName = reportService.GetProgramName(programId);

            reportViewer.Reset();
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.LocalReport.ReportEmbeddedResource = "ECA.Reports.CountryAwards.rdlc";
            reportViewer.LocalReport.SetParameters(new ReportParameter("Program", programName));

            return GetReport(reportViewer, format, out bytes);
        }

        /// <summary>
        /// Generates a report of Awards for a give objective for a program
        /// </summary>
        /// <param name="programId">the program id</param>
        /// <param name="objectiveId">the objective id<</param>
        /// <param name="format">the format of the report (pdf, xlsx, docx)</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> ReportObjectiveAwardsAsync(int programId, int objectiveId, string format)
        {
            byte[] bytes;

            var reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            var objectiveAwards = await reportService.GetObjectiveAwardsAsync(programId, objectiveId);
            var rds = new ReportDataSource("ObjectiveAwardDS", objectiveAwards);
            string programName = reportService.GetProgramName(programId);

            reportViewer.Reset();
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.LocalReport.ReportEmbeddedResource = "ECA.Reports.ObjectiveAwards.rdlc";
            reportViewer.LocalReport.SetParameters(new ReportParameter("Program", programName));

            return GetReport(reportViewer, format, out bytes);
        }

        /// <summary>
        /// Generates a report of Awards by Year for a program
        /// </summary>
        /// <param name="programId">the program id<</param>
        /// <param name="format">the format of the report (pdf, xlsx, docx)</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> ReportYearAwardsAsync(int programId, string format)
        {
            byte[] bytes;

            var reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            var yearAwards = await reportService.GetYearAwardsAsync(programId);
            var rds = new ReportDataSource("YearAwardDS", yearAwards);
            string programName = reportService.GetProgramName(programId);

            reportViewer.Reset();
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.LocalReport.ReportEmbeddedResource = "ECA.Reports.YearAwards.rdlc";
            reportViewer.LocalReport.SetParameters(new ReportParameter("Program", programName));

            return GetReport(reportViewer, format, out bytes);
        }


        /// <summary>
        /// Generates a report of Project with Grant Number for a program
        /// </summary>
        /// <param name="programId">the program id<</param>
        /// <param name="format">the format of the report (pdf, xlsx, docx)</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> ReportProjectsWithGrantNumberAsync(int programId, string format)
        {
            byte[] bytes;

            var reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            var projectsWithGrantNumber = await reportService.GetProjectsWithGrantNumberAsync(programId);
            var rds = new ReportDataSource("ProjectsWithGrantNumberDS", projectsWithGrantNumber);
            string programName = reportService.GetProgramName(programId);

            reportViewer.Reset();
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.LocalReport.ReportEmbeddedResource = "ECA.Reports.ProjectsWithGrantNumber.rdlc";
            reportViewer.LocalReport.SetParameters(new ReportParameter("Program", programName));

            return GetReport(reportViewer, format, out bytes);
        }

        /// <summary>
        /// private function to generate the report with the data source set
        /// </summary>
        /// <param name="reportViewer"></param>
        /// <param name="format"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private HttpResponseMessage GetReport(ReportViewer reportViewer, string format, out byte[] bytes)
        {
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            if (format == "docx")
            {
                format = "WORDOPENXML";
            }
            else if (format == "xlsx")
            {
                format = "EXCELOPENXML";
            }
            else
            {
                format = "PDF";
            };

            bytes = reportViewer.LocalReport.Render(format, null, out mimeType, out encoding, out extension, out streamids, out warnings);

            var result = new HttpResponseMessage(HttpStatusCode.OK);

            Stream stream = new MemoryStream(bytes);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(mimeType);

            return result;
        }
    }
}
