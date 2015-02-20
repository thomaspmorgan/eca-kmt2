using ECA.Business.Models;
using ECA.Business.Queries;
using ECA.Business.Queries.Models;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service
{
    public class ProjectService : IProjectService, ISaveable
    {
        private EcaContext context;

        public ProjectService(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            this.context = context;
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
            };
            var newHistory = draftProject.History.AsHistory();
            project.History = newHistory; 
            context.Projects.Add(project);
            return project;
        }
        #endregion

        #region Query

        public PagedQueryResults<ProgramProject> GetProjectsByProgramId(int programId, QueryableOperator<ProgramProject> queryOperator)
        {
            return GetProgramProjectsPagedQueryResults(programId, queryOperator).Compute();
        }

        public Task<PagedQueryResults<ProgramProject>> GetProjectsByProgramIdAsync(int programId, QueryableOperator<ProgramProject> queryOperator)
        {
            return GetProgramProjectsPagedQueryResults(programId, queryOperator).ComputeAsync();
        }

        private PagedQueryResults<ProgramProject> GetProgramProjectsPagedQueryResults(int programId, QueryableOperator<ProgramProject> queryOperator)
        {
            var query = ProjectQueries.CreateGetProjectsByProgramQuery(this.context, programId, queryOperator);
            var results = new PagedQueryResults<ProgramProject>(query, queryOperator.Start, queryOperator.Limit);
            return results;
        }

        #endregion

        #region IDispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern. 
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.context.Dispose();
                this.context = null;
            }
        }

        #endregion

        public int SaveChanges()
        {
            return context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }
    }
}
