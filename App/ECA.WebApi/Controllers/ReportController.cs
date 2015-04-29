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

namespace ECA.WebApi.Controllers
{
    [RoutePrefix("api/Report")]
    public class ReportController : ApiController
    {
        private IReportService reportService;

        /// <summary>
        /// Creates a new ReportController with the given repor service.
        /// </summary>
        /// <param name="reportService">The program service.</param>
        public ReportController(IReportService reportService)
        {
            Contract.Requires(reportService != null, "The program service must not be null.");
            this.reportService = reportService;
        }

        [HttpGet]
        [Route("ProjectAwards")]
        public HttpResponseMessage GetProjectAwards(int programId, int countryId)
        {
            Contract.Requires(programId != null, "The parameter programId must not be null.");
            Contract.Requires(countryId != null, "The parameter countryId must not be null.");

            byte[] bytes;

            var reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            
            var rds = new ReportDataSource("ProjectAwardDS", reportService.GetProjectAwards(programId, countryId).ToList() );
            string programName = reportService.GetProgramName(programId);
            string countryName = reportService.GetCountryName(countryId);
            reportViewer.Reset();
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.LocalReport.ReportEmbeddedResource = "ECA.WebApi.Reports.ProjectAwards.rdlc";
            reportViewer.LocalReport.SetParameters(new ReportParameter("Country", countryName));
            reportViewer.LocalReport.SetParameters(new ReportParameter("Program", programName));
                
            
            Warning[] warnings;

            string[] streamids;

            string mimeType;

            string encoding;

            string extension;


            bytes = reportViewer.LocalReport.Render("Pdf", null, out mimeType, out encoding, out extension, out streamids, out warnings);


            //var cd = new System.Net.Mime.ContentDisposition

            //{

            //    FileName = string.Format("ProjectAwards.pdf"),

            //    Inline = true,

            //};



            var result = new HttpResponseMessage(HttpStatusCode.OK);

            Stream stream = new MemoryStream(bytes);

            result.Content = new StreamContent(stream);

            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

            return result;

        }


    }
}
