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

        Task<List<ProjectAwardDTO>> GetProjectAwardsAsync(int programId, int countryId);

        IQueryable<RegionAwardDTO> GetRegionAwards(int programId);

        Task<List<RegionAwardDTO>> GetRegionAwardsAsync(int programId);

        IQueryable<PostAwardDTO> GetPostAwards(int programId);

        Task<List<PostAwardDTO>> GetPostAwardsAsync(int programId);

        IQueryable<FocusAwardDTO> GetFocusAwards(int programId);

        Task<List<FocusAwardDTO>> GetFocusAwardsAsync(int programId);

        IQueryable<FocusCategoryAwardDTO> GetFocusCategoryAwards(int programId);

        Task<List<FocusCategoryAwardDTO>> GetFocusCategoryAwardsAsync(int programId);

        IQueryable<CountryAwardDTO> GetCountryAwards(int programId);

        Task<List<CountryAwardDTO>> GetCountryAwardsAsync(int programId);

        IQueryable<ObjectiveAwardDTO> GetObjectiveAwards(int programId, int objectiveId);

        Task<List<ObjectiveAwardDTO>> GetObjectiveAwardsAsync(int programId, int objectiveId);

        IQueryable<YearAwardDTO> GetYearAwards(int programId);

        Task<List<YearAwardDTO>> GetYearAwardsAsync(int programId);

        IQueryable<ProjectWithGrantNumberDTO> GetProjectsWithGrantNumber(int programId);

        Task<List<ProjectWithGrantNumberDTO>> GetProjectsWithGrantNumberAsync(int programId);

        string GetProgramName(int programId);

        string GetCountryName(int countryId);
    }

}
