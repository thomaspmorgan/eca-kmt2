﻿using ECA.Business.Models;
using ECA.Business.Queries;
using ECA.Business.Queries.Models;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    public class ProjectService : IProjectService, IDisposable
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
                ProgramId = draftProject.ProgramId
            };
            draftProject.History.SetHistory(project);
            context.Projects.Add(project);
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
            return ProjectQueries.CreateGetProjectsByProgramQuery(context, programId, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
        }

        /// <summary>
        /// Returns the sorted, filtered, and paged projects in the program with the given program id.
        /// </summary>
        /// <param name="programId">The program id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted projects.</returns>
        public Task<PagedQueryResults<SimpleProjectDTO>> GetProjectsByProgramIdAsync(int programId, QueryableOperator<SimpleProjectDTO> queryOperator)
        {
            return ProjectQueries.CreateGetProjectsByProgramQuery(context, programId, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
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
