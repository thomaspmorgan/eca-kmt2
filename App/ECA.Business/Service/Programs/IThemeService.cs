using System;
namespace ECA.Business.Service.Programs
{
    public interface IThemeService
    {
        ECA.Core.Query.PagedQueryResults<ECA.Business.Queries.Models.Programs.ThemeDTO> GetThemes(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Queries.Models.Programs.ThemeDTO> queryOperator);
        System.Threading.Tasks.Task<ECA.Core.Query.PagedQueryResults<ECA.Business.Queries.Models.Programs.ThemeDTO>> GetThemesAsync(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Queries.Models.Programs.ThemeDTO> queryOperator);
    }
}
