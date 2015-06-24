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
        IQueryable<ProjectAwardDTO> GetProjectAwards(int programId, int countryId);

        IQueryable<RegionAwardDTO> GetRegionAwards(int programId);

        IQueryable<PostAwardDTO> GetPostAwards(int programId);

        IQueryable<FocusAwardDTO> GetFocusAwards(int programId);

        IQueryable<FocusCategoryAwardDTO> GetFocusCategoryAwards(int programId);

        string GetProgramName(int programId);

        string GetCountryName(int countryId);
    }

}
