using ECA.Business.Service.Persons;
using ECA.Business.Queries.Models.Persons;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ECA.WebApi.Models.Person;
using ECA.WebApi.Security;
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
        private readonly IEduEmpService service;
        private IUserProvider userProvider;

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

        #region Education

        /// <summary>
        /// Returns education info associated with a person
        /// </summary>
        /// <param name="personId">The person id to find educations info for</param>
        /// <returns>Education info associated with person</returns>
        [ResponseType(typeof(IList<EducationEmploymentDTO>))]
        [Route("EduEmp/{personId:int}/Education")]
        public async Task<IHttpActionResult> GetEducationsByPersonIdAsync(int personId)
        {
            var educations = await service.GetEducationByIdAsync(personId);
            if (educations != null)
            {
                return Ok(educations);
            }
            else
            {
                return NotFound();
            }
        }

        [ResponseType(typeof(EducationEmploymentDTO))]
        [Route("EduEmp/{personId:int}/Education")]
        public async Task<IHttpActionResult> PostEducationAsync(PersonEduEmpBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = userProvider.GetCurrentUser();
                var businessUser = userProvider.GetBusinessUser(currentUser);
                var education = await service.CreateEducationAsync(model.ToPersonEduEmp(businessUser));
                await service.SaveChangesAsync();
                var dto = await service.GetEducationByIdAsync(education.ProfessionEducationId);
                return Ok(dto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        
        [ResponseType(typeof(EducationEmploymentDTO))]
        [Route("EduEmp/{personId:int}/Education")]
        public async Task<IHttpActionResult> PutEducationAsync(UpdatedPersonEduEmpBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = userProvider.GetCurrentUser();
                var businessUser = userProvider.GetBusinessUser(currentUser);
                await service.UpdateEducationAsync(model.ToUpdatedPersonEduEmp(businessUser));
                await service.SaveChangesAsync();
                var dto = await service.GetEducationByIdAsync(model.ProfessionEducationId);
                return Ok(dto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [ResponseType(typeof(OkResult))]
        [Route("EduEmp/{personId:int}/Education/{id:int}")]
        public async Task<IHttpActionResult> DeleteEducation(int id)
        {
            await service.DeleteAsync(id);
            await service.SaveChangesAsync();
            return Ok();
        }
        
        #endregion

        #region Employment

        /// <summary>
        /// Returns employment info associated with a person
        /// </summary>
        /// <param name="personId">The person id to find employments info for</param>
        /// <returns>Employment info associated with person</returns>
        [ResponseType(typeof(IList<EducationEmploymentDTO>))]
        [Route("EduEmp/{personId:int}/Employment")]
        public async Task<IHttpActionResult> GetEmploymentsByPersonIdAsync(int personId)
        {
            var employments = await service.GetEmploymentByIdAsync(personId);
            if (employments != null)
            {
                return Ok(employments);
            }
            else
            {
                return NotFound();
            }
        }
        
        [ResponseType(typeof(EducationEmploymentDTO))]
        [Route("EduEmp/{personId:int}/Employment")]
        public async Task<IHttpActionResult> PostEmploymentAsync(PersonEduEmpBindingModel model)
        {
            if (ModelState.IsValid)
            {
            var currentUser = userProvider.GetCurrentUser();
            var businessUser = userProvider.GetBusinessUser(currentUser);
            var education = await service.CreateEmploymentAsync(model.ToPersonEduEmp(businessUser));
                await service.SaveChangesAsync();
            var dto = await service.GetEmploymentByIdAsync(education.ProfessionEducationId);
                return Ok(dto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [ResponseType(typeof(EducationEmploymentDTO))]
        [Route("EduEmp/{personId:int}/Employment")]
        public async Task<IHttpActionResult> PutEmploymentAsync(UpdatedPersonEduEmpBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = userProvider.GetCurrentUser();
                var businessUser = userProvider.GetBusinessUser(currentUser);
                await service.UpdateEmploymentAsync(model.ToUpdatedPersonEduEmp(businessUser));
                await service.SaveChangesAsync();
                var dto = await service.GetEmploymentByIdAsync(model.ProfessionEducationId);
                return Ok(dto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [ResponseType(typeof(OkResult))]
        [Route("EduEmp/{personId:int}/Employment/{id:int}")]
        public async Task<IHttpActionResult> DeleteEmployment(int id)
        {
            await service.DeleteAsync(id);
            await service.SaveChangesAsync();
            return Ok();
        }
        
        #endregion

    }
}