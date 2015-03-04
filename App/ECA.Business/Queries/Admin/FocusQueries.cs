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
        public static IQueryable<FocusDTO> CreateFociDTOsQuery(EcaContext context, QueryableOperator<FocusDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            var query = context.Foci.Select(x => new FocusDTO
            {
                Id = x.FocusId,
                Name = x.FocusName
            });
            query = query.Apply(queryOperator);
            return query;
        }
    }
}
