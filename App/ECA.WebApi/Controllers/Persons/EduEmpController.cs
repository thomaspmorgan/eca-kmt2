using ECA.Business.Queries.Models.Persons;
using ECA.Business.Service.Persons;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.WebApi.Models.Person;
using ECA.WebApi.Models.Query;
using ECA.WebApi.Security;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;

namespace ECA.WebApi.Controllers.Persons
{
    /// <summary>
    /// The EduEmpController provides clients with the Education and Employment for a person in the eca system.
    /// </summary>
    [RoutePrefix("api")]
    [Authorize]
    public class EduEmpController : ApiController
    {
        private static readonly ExpressionSorter<EducationEmploymentDTO> DEFAULT_SORTER = new ExpressionSorter<EducationEmploymentDTO>(x => x.StartDate, SortDirection.Descending);

        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IEduEmpService service;
        private readonly IUserProvider userProvider;
        
        /// <summary>
        /// Creates a new EduEmpController with the given service.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="userProvider"></param>
        public EduEmpController(IEduEmpService service, IUserProvider userProvider)
        {
            Contract.Requires(service != null, "The service must not be null.");
            Contract.Requires(userProvider != null, "The userProvider must not be null.");
            this.service = service;
            this.userProvider = userProvider;
        }

        /// <summary>
        /// Returns all education info associated with a person
        /// </summary>
        /// <param name="personId">The Id of the person</param>
        /// <param name="queryModel">The query model.</param>
        /// <returns>Education info associated with person</returns>
        [ResponseType(typeof(IList<EducationEmploymentDTO>))]
        [Route("EduEmp/{personId:int}/Educations")]
        public async Task<IHttpActionResult> GetEducationsAsync(int personId, [FromUri]PagingQueryBindingModel<EducationEmploymentDTO> queryModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var educations = await service.GetEducationsAsync(personId, queryModel.ToQueryableOperator(DEFAULT_SORTER));
            return Ok(educations);
        }

        /// <summary>
        /// Returns employment info associated with a person
        /// </summary>
        /// <param name="personId">The person id to find employments info for</param>
        /// <returns>Employment info associated with person</returns>
        [ResponseType(typeof(IList<EducationEmploymentDTO>))]
        [Route("EduEmp/{personId:int}/Employments")]
        public async Task<IHttpActionResult> GetEmploymentsAsync(int personId, [FromUri]PagingQueryBindingModel<EducationEmploymentDTO> queryModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var employments = await service.GetEmploymentsAsync(personId, queryModel.ToQueryableOperator(DEFAULT_SORTER));
            return Ok(employments);
        }

        /// <summary>
        /// Add a new education info associated with a person
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ResponseType(typeof(EducationEmploymentDTO))]
        [Route("EduEmp/{personId:int}/ProfessionEducation")]
        public async Task<IHttpActionResult> PostProfessionEducationAsync(PersonEduEmpBindingModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var currentUser = userProvider.GetCurrentUser();
            var businessUser = userProvider.GetBusinessUser(currentUser);
            var eduemp = await service.CreateProfessionEducationAsync(model.ToPersonEduEmp(businessUser));
            await service.SaveChangesAsync();
            var dto = await service.GetProfessionEducationByIdAsync(eduemp.ProfessionEducationId);
            return Ok(dto);
        }

        /// <summary>
        /// Updated an education info associated with a person
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [ResponseType(typeof(EducationEmploymentDTO))]
        [Route("EduEmp/{personId:int}/ProfessionEducation")]
        public async Task<IHttpActionResult> PutProfessionEducationAsync(UpdatedPersonEduEmpBindingModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var currentUser = userProvider.GetCurrentUser();
            var businessUser = userProvider.GetBusinessUser(currentUser);
            await service.UpdateProfessionEducationAsync(model.ToUpdatedPersonEduEmp(businessUser));
            await service.SaveChangesAsync();
            var dto = await service.GetProfessionEducationByIdAsync(model.ProfessionEducationId);
            return Ok(dto);
        }

        /// <summary>
        /// Delete a education info associated with a person
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(OkResult))]
        [Route("EduEmp/{personId:int}/ProfessionEducation/{id:int}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            await service.DeleteAsync(id);
            await service.SaveChangesAsync();
            return Ok();
        }
        
    }
}