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

        public HttpResponseMessage ReportRegionAwards(int programId)
        {

            byte[] bytes;

            var reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;

            var rds = new ReportDataSource("RegionAwardDS", reportService.GetRegionAwards(programId).ToList());
            string programName = reportService.GetProgramName(programId);
            reportViewer.Reset();
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.LocalReport.ReportEmbeddedResource = "ECA.Reports.RegionAwards.rdlc";
            reportViewer.LocalReport.SetParameters(new ReportParameter("Program", programName));


            return GetReport(reportViewer, out bytes);
        }

        public HttpResponseMessage ReportProjectAwards(int programId, int countryId)
        {
            byte[] bytes;

            var reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;

            var rds = new ReportDataSource("ProjectAwardDS", reportService.GetProjectAwards(programId, countryId).ToList());
            string programName = reportService.GetProgramName(programId);
            string countryName = reportService.GetCountryName(countryId);
            reportViewer.Reset();
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.LocalReport.ReportEmbeddedResource = "ECA.Reports.ProjectAwards.rdlc";
            reportViewer.LocalReport.SetParameters(new ReportParameter("Country", countryName));
            reportViewer.LocalReport.SetParameters(new ReportParameter("Program", programName));

            return GetReport(reportViewer, out bytes);
        }


        public HttpResponseMessage ReportPostAwards(int programId)
        {
            byte[] bytes;

            var reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;

            var rds = new ReportDataSource("PostAwardDS", reportService.GetPostAwards(programId).ToList());
            string programName = reportService.GetProgramName(programId);

            reportViewer.Reset();
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.LocalReport.ReportEmbeddedResource = "ECA.Reports.PostAwards.rdlc";
            reportViewer.LocalReport.SetParameters(new ReportParameter("Program", programName));

            return GetReport(reportViewer, out bytes);
        }

        private static HttpResponseMessage GetReport(ReportViewer reportViewer, out byte[] bytes)
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
