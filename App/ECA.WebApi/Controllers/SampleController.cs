using ECA.Core.DynamicLinq;
using System.Data.Entity;
using ECA.Core.DynamicLinq.Filter;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ECA.WebApi.Models;
using System.Web.Http.Description;
using ECA.WebApi.Models.Query;
using System.Web.Http.ModelBinding;
using ECA.Business.Service;
using System.Diagnostics;
using ECA.Business.Queries.Models;
using ECA.WebApi.Models.Projects;
using ECA.Core.Query;

namespace ECA.WebApi.Controllers
{
    public class SampleController : ApiController
    {
        private static ExpressionSorter<ProgramProject> DEFAULT_PROGRAM_PROJECT_SORTER = new ExpressionSorter<ProgramProject>(x => x.ProjectId, SortDirection.Ascending);

        private IProjectService projectService;

        public SampleController(IProjectService projectService)
        {
            Debug.Assert(projectService != null, "The project service must not be null.");
            this.projectService = projectService;
        }

        /// <summary>
        /// Returns a listing of the projects by program.
        /// </summary>
        /// <param name="programId">The program id.</param>
        /// <param name="queryModel">The page, filter and sort information.</param>
        /// <returns>The list of projects by program.</returns>
        [ResponseType(typeof(PagedQueryResults<ProgramProject>))]
        public async Task<IHttpActionResult> GetProjectsByProgramIdAsync(int programId, PagingQueryBindingModel queryModel)
        {
            //paging
            //http://localhost:5555/api/Sample?start=0&limit=10&programId=10


//            [
//{
//property: 'programName',
//value: 'dance',
//comparison: 'like'
//}

//]

            //filter on proram name like dance
            //http://localhost:5555/api/Sample?programId=10&start=0&limit=10&filter=%5B%0D%0A%7B%0D%0Aproperty%3A+%27programName%27%2C%0D%0Avalue%3A+%27dance%27%2C%0D%0Acomparison%3A+%27like%27%0D%0A%7D%0D%0A%0D%0A%5D


//[
//{
//property: 'projectstatusid',
//value: [3,5,7],
//comparison: 'in'
//}
//]


            //filter on project status ids 3,5,7
            //http://localhost:5555/api/Sample?programid=10&start=0&limit=10&filter=%5B%0D%0A%7B%0D%0Aproperty%3A+%27projectstatusid%27%2C%0D%0Avalue%3A+%5B3%2C5%2C7%5D%2C%0D%0Acomparison%3A+%27in%27%0D%0A%7D%0D%0A%5D

            //sort on id
            //http://localhost:5555/api/Sample?start=0&limit=10&sort=%5b%7bproperty%3a+'Id'%2cdirection%3a+'asc'%7d%5d

            if (ModelState.IsValid)
            {
                var results = await this.projectService.GetProjectsByProgramIdAsync(programId, queryModel.ToQueryableOperator<ProgramProject>(DEFAULT_PROGRAM_PROJECT_SORTER));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Creates a new draft project.
        /// </summary>
        /// <param name="model">The draft project.</param>
        /// <returns>The id of the project.</returns>
        [ResponseType(typeof(int))]
        public async Task<IHttpActionResult> PostCreateProjectAsync(DraftProjectBindingModel model)
        {
            if (ModelState.IsValid)
            {
                //var userId = this.User.Identity.Name;
                var userId = 1;
                var project = this.projectService.Create(model.ToDraftProject(userId));
                await this.projectService.SaveChangesAsync();
                var projectId = project.ProjectId;
                return Ok(projectId);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        //[ResponseType(typeof(PagedQueryResults<SimpleProgramDTO>))]
        //public async Task<HttpResponseMessage> GetProgramsAsync([ModelBinder(typeof(PagingQueryBindingModelBinder))] PagingQueryBindingModel queryModel)
        //{
        //    if(ModelState.IsValid)
        //    {

        //        //paging
        //        //http://localhost:5555/api/Sample?start=0&limit=10

        //        //filter on like allows foreign secondary
        //        //http://localhost:5555/api/Sample?start=0&limit=10&filter=%5b%7bproperty%3a+'description'%2c+value%3a+'allows+foreign+secondary'%2c+comparison%3a+'like'%7d%5d

        //        //filter on id
        //        //http://localhost:5555/api/Sample?start=0&limit=10&filter=%5b%7bproperty%3a+'Id'%2c+value%3a+3%2c+comparison%3a+'eq'%7d%5d

        //        //sort on id
        //        //http://localhost:5555/api/Sample?start=0&limit=10&sort=%5b%7bproperty%3a+'Id'%2cdirection%3a+'asc'%7d%5d
        //        using (var context = new EcaContext())
        //        {
        //            //var query = from p in context.Programs
        //            //            select new SimpleProgramDTO
        //            //            {
        //            //                Description = p.Description,
        //            //                Id = p.ProgramId,
        //            //                StartDate = p.StartDate
        //            //            };
        //            var query = context.Programs.Select(p => new SimpleProgramDTO
        //            {
        //                Description = p.Description,
        //                Id = p.ProgramId,
        //                StartDate = p.StartDate
        //            });

        //            var queryableOperator = queryModel.ToQueryableOperator<SimpleProgramDTO>(new ExpressionSorter<SimpleProgramDTO>(x => x.Id, SortDirection.Ascending));
        //            query = query.Apply(queryableOperator);

        //            return Request.CreateResponse(new PagedQueryResults<SimpleProgramDTO>
        //            {
        //                Results = await query.Skip(queryableOperator.Start).Take(queryableOperator.Limit).ToListAsync(),
        //                Total = await query.CountAsync()
        //            });
        //        }
        //    }
        //    else
        //    {
        //        //need to do global model filter found here...
        //        //http://www.asp.net/web-api/overview/formats-and-model-binding/model-validation-in-aspnet-web-api
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        //    }
        //}
    }
}
