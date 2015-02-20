using ECA.Business.Queries.Models;
using ECA.Core.DynamicLinq;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries
{
    public static class ProjectQueries
    {
        public static IQueryable<ProgramProject> CreateGetProjectsByProgramQuery(EcaContext context, int programId, QueryableOperator<ProgramProject> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = from project in context.Projects
                        let parentProgram = project.ParentProgram
                        //let regions = project.Regions //not sure why we need to join to region here if we just get first??
                        let status = project.Status
                        let owner = parentProgram.Owner
                        select new ProgramProject
                        {
                            LastRevisedBy = "Some User",
                            LastRevisedOn = project.History.RevisedOn,
                            ProgramId = parentProgram.ProgramId,
                            ProjectId = project.ProjectId,
                            OwnerOrganizationId = owner.OrganizationId
                        };
            query = query.Apply(queryOperator);
            return query;
        }
    }
}
