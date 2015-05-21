using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ECA.Business.Queries.Admin
{
    /// <summary>
    /// Queries for organization
    /// </summary>
    public static class OrganizationQueries
    {
        /// <summary>
        /// Query to get a list of organizations
        /// </summary>
        /// <param name="context">The context to query</param>
        /// <param name="queryOperator">The query operator to apply</param>
        /// <returns>List of organizations</returns>
        public static IQueryable<SimpleOrganizationDTO> CreateGetSimpleOrganizationsDTOQuery(EcaContext context, QueryableOperator<SimpleOrganizationDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");

            var query = context.Organizations
                .Include(x => x.Addresses)
                .Where(x => x.OrganizationTypeId != OrganizationType.Office.Id)
                .Select(x => new SimpleOrganizationDTO
                {
                    Name = x.Name,
                    OrganizationType = x.OrganizationType.OrganizationTypeName,
                    Status = x.Status,
                    //Location = x.Addresses.FirstOrDefault().Location.Country.LocationName
                });
            query = query.Apply(queryOperator);
            return query;
        }
    }
}
