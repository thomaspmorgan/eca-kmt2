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
        IQueryable<ProjectAwardDTO> GetProjectAwards(int projectId, int countryId);

        IQueryable<RegionAwardDTO> GetRegionAwards(int projectId);

        string GetProgramName(int programId);

        string GetCountryName(int countryId);
    }

}
