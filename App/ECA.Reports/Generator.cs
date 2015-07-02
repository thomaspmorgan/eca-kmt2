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
    public class Generator
    {
        private IReportService reportService;

        public Generator(IReportService reportService)
        {
            this.reportService = reportService;
        }

        public  async Task<HttpResponseMessage> ReportRegionAwardsAsync(int programId)
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


            return GetReport(reportViewer, out bytes);
        }


        public async Task<HttpResponseMessage> ReportProjectAwardsAsync(int programId, int countryId)
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

            return GetReport(reportViewer, out bytes);
        }



        public async Task<HttpResponseMessage> ReportPostAwardsAsync(int programId)
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

            return GetReport(reportViewer, out bytes);
        }

        public async Task<HttpResponseMessage> ReportFocusAwardsAsync(int programId)
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

            return GetReport(reportViewer, out bytes);
        }

        public async Task<HttpResponseMessage> ReportFocusCategoryAwardsAsync(int programId)
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

            return GetReport(reportViewer, out bytes);
        }

        private HttpResponseMessage GetReport(ReportViewer reportViewer, out byte[] bytes)
        {
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;

            bytes = reportViewer.LocalReport.Render("Pdf", null, out mimeType, out encoding, out extension, out streamids, out warnings);

            var result = new HttpResponseMessage(HttpStatusCode.OK);

            Stream stream = new MemoryStream(bytes);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

            return result;
        }
    }
}
