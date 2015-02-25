using ECA.Business.Queries.Models;
using ECA.Core.DynamicLinq;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries
{
    public static class ProjectQueries
    {
        public static IQueryable<SimpleProjectDTO> CreateGetProjectsByProgramQuery(EcaContext context, int programId, QueryableOperator<SimpleProjectDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");

            var query = from project in context.Projects
                        let parentProgram = project.ParentProgram
                        let locations = parentProgram.Locations
                        where project.ProgramId == programId
                        select new SimpleProjectDTO
                        {
                            ProgramId = parentProgram.ProgramId,
                            ProjectId = project.ProjectId,
                            ProjectName = project.Name,
                            LocationNames = locations.Select(x => x.LocationName)
                        };

            query = query.Apply(queryOperator);
            return query;
        }
    }
}
