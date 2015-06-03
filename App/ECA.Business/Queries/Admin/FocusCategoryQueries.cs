using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Admin
{
    public static class FocusCategoryQueries
    {
        /// <summary>
        /// Returns a query to select focus dtos.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="officeId">The office id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The query.</returns>
        public static IQueryable<FocusCategoryDTO> CreateGetFocusCategoryDTOByOfficeIdQuery(EcaContext context, int officeId, QueryableOperator<FocusCategoryDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            Contract.Requires(context != null, "The context must not be null.");
            var query = context.Categories
                .Where(x => x.Focus.OfficeId == officeId)
                .Select(x => new FocusCategoryDTO
                {
                    Id = x.CategoryId,
                    Name = x.CategoryName,
                    FocusName = x.Focus.FocusName
                });
            query = query.Apply(queryOperator);
            return query;
        }

        /// <summary>
        /// Returns a query to select focus dtos.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="programId">The program id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The query.</returns>
        public static IQueryable<FocusCategoryDTO> CreateGetFocusCategoryDTOByProgramIdQuery(EcaContext context, int programId, QueryableOperator<FocusCategoryDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            Contract.Requires(context != null, "The context must not be null.");
            var query = CreateGetFocusCategoryDTOByProgramIdQuery(context, programId);
            query = query.Apply(queryOperator);
            return query;
        }

        /// <summary>
        /// Returns a query to select focus dtos.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="programId">The program id.</param>
        /// <returns>The query.</returns>
        public static IQueryable<FocusCategoryDTO> CreateGetFocusCategoryDTOByProgramIdQuery(EcaContext context, int programId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = context.Programs.Where(x => x.ProgramId == programId).SelectMany(x => x.Categories).Select(x => new FocusCategoryDTO
            {
                Id = x.CategoryId,
                Name = x.CategoryName,
                FocusName = x.Focus.FocusName
            });
            return query;
        }
    }
}
