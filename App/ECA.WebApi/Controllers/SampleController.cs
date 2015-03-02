using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.WebApi.Models.Projects;
using ECA.WebApi.Models.Query;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;

namespace ECA.WebApi.Controllers
{
    public class SampleController : ApiController
    {
        private static ExpressionSorter<SimpleProjectDTO> DEFAULT_PROGRAM_PROJECT_SORTER = new ExpressionSorter<SimpleProjectDTO>(x => x.ProjectId, SortDirection.Ascending);

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
        [ResponseType(typeof(PagedQueryResults<SimpleProjectDTO>))]
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
            //http://localhost:5555/api/Sample?programId=10&start=0&limit=10&filter=%5B%7B%0D%0Aproperty%3A+%27programName%27%2C%0D%0Avalue%3A+%27dance%27%2C%0D%0Acomparison%3A+%27like%27%0D%0A%7D%5D


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
                var results = await this.projectService.GetProjectsByProgramIdAsync(programId, queryModel.ToQueryableOperator<SimpleProjectDTO>(DEFAULT_PROGRAM_PROJECT_SORTER));
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
        //[ResponseType(typeof(int))]
        //public async Task<IHttpActionResult> PostCreateProjectAsync(DraftProjectBindingModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //var userId = this.User.Identity.Name;
        //        var userId = 1;
        //        var project = this.projectService.Create(model.ToDraftProject(userId));
        //        //await this.projectService.SaveChangesAsync();
        //        var projectId = project.ProjectId;
        //        return Ok(projectId);
        //    }
        //    else
        //    {
        //        return BadRequest(ModelState);
        //    }
        //}
    }
}
