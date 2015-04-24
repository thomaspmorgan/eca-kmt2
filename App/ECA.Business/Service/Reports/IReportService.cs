using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECA.Business.Queries.Models.Reports;

namespace ECA.Business.Service.Reports
{
    public interface IReportService
    {
        IQueryable<ProjectAwardDTO> GetProjectAwards(int year, int countryId);
    }
}
