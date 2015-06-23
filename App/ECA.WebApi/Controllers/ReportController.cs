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

namespace ECA.WebApi.Controllers
{
    [RoutePrefix("api/Report")]

    public class ReportController : ApiController
    {

        private Generator reportGenerator;

        public ReportController(IReportService reportService)
        {
            reportGenerator = new Generator(reportService);
        }

        [Route("ProjectAwards")]
        public HttpResponseMessage GetProjectAwards(int programId, int countryId)
        {
            return reportGenerator.ReportProjectAwards(programId, countryId);
        }

        [Route("RegionAwards")]
        public HttpResponseMessage GetRegionAwards(int programId)
        {
            return reportGenerator.ReportRegionAwards(programId);
        }

        [Route("PostAwards")]
        public HttpResponseMessage GetPostAwards(int programId)
        {
            return reportGenerator.ReportPostAwards(programId);
        }


    }
}
