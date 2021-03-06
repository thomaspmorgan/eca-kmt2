﻿using System;
namespace ECA.Business.Service.Lookup
{
    /// <summary>
    /// An IThemeService is capable of performing crud operations on themes.
    /// </summary>
    public interface IThemeService
    {
        /// <summary>
        /// Returns paged, filtered, and sorted themes in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The themes in the system.</returns>
        ECA.Core.Query.PagedQueryResults<ECA.Business.Queries.Models.Programs.ThemeDTO> Get(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Queries.Models.Programs.ThemeDTO> queryOperator);

        /// <summary>
        /// Returns paged, filtered, and sorted themes in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The themes in the system.</returns>
        System.Threading.Tasks.Task<ECA.Core.Query.PagedQueryResults<ECA.Business.Queries.Models.Programs.ThemeDTO>> GetAsync(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Queries.Models.Programs.ThemeDTO> queryOperator);
    }
}
