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
    public static class FocusQueries
    {
        /// <summary>
        /// Returns the filtered and sorted foci in the system.
        /// </summary>
        /// <param name="context">the context to query.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The filtered and sorted foci in the system.</returns>
        public static IQueryable<FocusDTO> CreateFociDTOsQuery(EcaContext context, QueryableOperator<FocusDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            var query = CreateGetFocusDTOQuery(context);
            query = query.Apply(queryOperator);
            return query;
        }

        /// <summary>
        /// Returns the focus dto with the given focus id.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="focusId">the id of the focus.</param>
        /// <returns>The focus dto.</returns>
        public static IQueryable<FocusDTO> CreateGetFocusByIdQuery(EcaContext context, int focusId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = CreateGetFocusDTOQuery(context).Where(x => x.Id == focusId);
            return query;
        }

        private static IQueryable<FocusDTO> CreateGetFocusDTOQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = context.Foci.Select(x => new FocusDTO
            {
                Id = x.FocusId,
                Name = x.FocusName
            });
            return query;
        }
    }
}
