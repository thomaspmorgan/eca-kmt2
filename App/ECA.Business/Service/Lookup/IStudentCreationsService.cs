﻿using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using System.Threading.Tasks;

namespace ECA.Business.Service.Lookup
{
    /// <summary>
    /// An IStudentCreationService is capable of performing crud operations on SEVIS StudentCreations.
    /// </summary>
    public interface IStudentCreationService
    {
        /// <summary>
        /// Returns paged, filtered and sorted StudentCreations in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The StudentCreations in the system.</returns>
        PagedQueryResults<SimpleSevisLookupDTO> Get(QueryableOperator<SimpleSevisLookupDTO> queryOperator);

        /// <summary>
        /// Returns paged, filtered, and sorted StudentCreations in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The StudentCreations in the system.</returns>
        Task<PagedQueryResults<SimpleSevisLookupDTO>> GetAsync(QueryableOperator<SimpleSevisLookupDTO> queryOperator);
    }
}
