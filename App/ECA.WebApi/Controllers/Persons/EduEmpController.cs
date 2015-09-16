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

        [ResponseType(typeof(PagedQueryResults<EducationEmploymentDTO>))]
        public async Task<IHttpActionResult> GetAsync([FromUri]PagingQueryBindingModel<EducationEmploymentDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var dtos = await service.GetAsync(queryModel.ToQueryableOperator(DEFAULT_SORTER));
                return Ok(dtos);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        
        [ResponseType(typeof(EducationEmploymentDTO))]
        public async Task<IHttpActionResult> PostAsync(PersonEduEmpBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = userProvider.GetCurrentUser();
                var businessUser = userProvider.GetBusinessUser(currentUser);
                if (model.PersonOfEducation != null)
                {
                    var eduemp = await service.CreateEducationAsync(model.ToPersonEduEmp(businessUser));
                    await service.SaveChangesAsync();
                    var dto = await service.GetEducationByIdAsync(eduemp.ProfessionEducationId);
                    return Ok(dto);
                }
                else if (model.PersonOfProfession != null)
                {
                    var eduemp = await service.CreateEmploymentAsync(model.ToPersonEduEmp(businessUser));
                    await service.SaveChangesAsync();
                    var dto = await service.GetEmploymentByIdAsync(eduemp.ProfessionEducationId);
                    return Ok(dto);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }




    }
}