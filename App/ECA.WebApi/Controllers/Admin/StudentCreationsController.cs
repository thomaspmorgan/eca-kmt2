﻿using ECA.Business.Service.Lookup;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.WebApi.Models.Query;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ECA.WebApi.Controllers.Projects
{
    /// <summary>
    /// The StudentCreationsController handles crud operations on sevis StudentCreations.
    /// </summary>
    [Authorize]
    public class StudentCreationsController : ApiController
    {
        /// <summary>
        /// The default sorter for a list of sevis EducationLevels.
        /// </summary>
        private static readonly ExpressionSorter<SimpleSevisLookupDTO> DEFAULT_SEVIS_COMM_STATUS_DTO_SORTER = 
            new ExpressionSorter<SimpleSevisLookupDTO>(x => x.Description, SortDirection.Ascending);
        private IStudentCreationService service;

        /// <summary>
        /// Creates a new instance with the SEVIS StudentCreations service.
        /// </summary>
        /// <param name="service">The service.</param>
        public StudentCreationsController(IStudentCreationService service)
        {
            Contract.Requires(service != null, "The service must not be null.");
            this.service = service;
        }

        /// <summary>
        /// Returns the SEVIS StudentCreations currently in the system.
        /// </summary>
        /// <param name="queryModel">The query model.</param>
        /// <returns>The project SEVIS StudentCreations currently in the system.</returns>
        [ResponseType(typeof(PagedQueryResults<SimpleSevisLookupDTO>))]
        public async Task<IHttpActionResult> GetStudentCreationsAsync([FromUri]PagingQueryBindingModel<SimpleSevisLookupDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await service.GetAsync(queryModel.ToQueryableOperator(DEFAULT_SEVIS_COMM_STATUS_DTO_SORTER));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}
