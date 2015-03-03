using System.Linq;
using System.Data.Entity;
using ECA.Business.Queries.Admin;
using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    public class ProjectService : DbContextService<EcaContext>, IProjectService
    {
        public ProjectService(EcaContext context) : base(context)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        #region Create

        public Project Create(DraftProject draftProject)
        {
            Contract.Requires(draftProject != null, "The draft project must not be null.");
            return DoCreate(draftProject);
        }

        private Project DoCreate(DraftProject draftProject)
        {
            var project = new Project
            {
                Name = draftProject.Name,
                ProjectStatusId = draftProject.StatusId,
                ProgramId = draftProject.ProgramId
            };
            draftProject.History.SetHistory(project);
            this.Context.Projects.Add(project);
            return project;
        }

        #endregion

        #region Query

        /// <summary>
        /// Returns the sorted, filtered, and paged projects in the program with the given program id.
        /// </summary>
        /// <param name="programId">The program id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted projects.</returns>
        public PagedQueryResults<SimpleProjectDTO> GetProjectsByProgramId(int programId, QueryableOperator<SimpleProjectDTO> queryOperator)
        {
            return ProjectQueries.CreateGetProjectsByProgramQuery(this.Context, programId, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
        }

        /// <summary>
        /// Returns the sorted, filtered, and paged projects in the program with the given program id.
        /// </summary>
        /// <param name="programId">The program id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted projects.</returns>
        public Task<PagedQueryResults<SimpleProjectDTO>> GetProjectsByProgramIdAsync(int programId, QueryableOperator<SimpleProjectDTO> queryOperator)
        {
            return ProjectQueries.CreateGetProjectsByProgramQuery(this.Context, programId, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
        }

        public Task<ProjectDTO> GetProjectByIdAsync(int projectId)
        {
            return ProjectQueries.CreateGetProjectByIdQuery(this.Context, projectId).FirstOrDefaultAsync();
        }

        public ProjectDTO GetProjectById(int projectId)
        {
            return ProjectQueries.CreateGetProjectByIdQuery(this.Context, projectId).FirstOrDefault();
        }

        #endregion
    }
}
