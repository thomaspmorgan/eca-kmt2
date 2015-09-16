using ECA.Business.Service.Persons;
using ECA.Business.Queries.Models.Persons;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.WebApi.Models.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ECA.WebApi.Models.Person;
using ECA.WebApi.Security;
using System.Web.Http.Results;

namespace ECA.WebApi.Controllers.Persons
{
    [RoutePrefix("api")]
    [Authorize]
    public class EduEmpController : ApiController
    {
        private static readonly ExpressionSorter<EducationEmploymentDTO> DEFAULT_SORTER = new ExpressionSorter<EducationEmploymentDTO>(x => x.StartDate, SortDirection.Descending);

        private readonly IEduEmpService service;
        private IUserProvider userProvider;
        
        public EduEmpController(IEduEmpService service, IUserProvider userProvider)
        {
            Contract.Requires(service != null, "The service must not be null.");
            Contract.Requires(userProvider != null, "The userProvider must not be null.");
            this.service = service;
            this.userProvider = userProvider;
        }

        #region Education

        /// <summary>
        /// Returns educations info associated with a person
        /// </summary>
        /// <param name="personId">The person id to find educations info for</param>
        /// <returns>Educations info associated with person</returns>
        [ResponseType(typeof(IList<EducationEmploymentDTO>))]
        [Route("People/{personId:int}/Education")]
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
        [Route("People/{personId:int}/Education")]
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
        [Route("People/{personId:int}/Education")]
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
        [Route("People/{personId:int}/Education/{id:int}")]
        public async Task<IHttpActionResult> DeleteEducation(int id)
        {
            await service.DeleteAsync(id);
                    await service.SaveChangesAsync();
            return Ok();
        }
        
        #endregion

        #region Employment

        /// <summary>
        /// Returns employments info associated with a person
        /// </summary>
        /// <param name="personId">The person id to find employments info for</param>
        /// <returns>Employents info associated with person</returns>
        [ResponseType(typeof(IList<EducationEmploymentDTO>))]
        [Route("People/{personId:int}/Employment")]
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
        [Route("People/{personId:int}/Employment")]
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
        [Route("People/{personId:int}/Employment")]
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
        [Route("People/{personId:int}/Employment/{id:int}")]
        public async Task<IHttpActionResult> DeleteEmployment(int id)
        {
            await service.DeleteAsync(id);
            await service.SaveChangesAsync();
            return Ok();
        }


        #endregion

    }
}