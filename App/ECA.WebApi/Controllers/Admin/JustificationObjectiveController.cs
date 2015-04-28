using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.WebApi.Models.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ECA.Business.Service.Admin;

namespace ECA.WebApi.Controllers.Admin
{
    /// <summary>
    /// The Focus Controller provides lookup and crud operations for a focus.
    /// </summary>
    public class JustificationObjectivesController : ApiController
    {            
        /// <summary>
        /// The default sorter for a list of objectives.
        /// </summary>
        private static readonly ExpressionSorter<JustificationObjectiveDTO> DEFAULT_JUSTIFICATIONOBJECTIVES_DTO_SORTER = new ExpressionSorter<JustificationObjectiveDTO>(x => x.JustificationName, SortDirection.Ascending);

        private IJustificationObjectiveService service;

        /// <summary>
        /// Creates a new FocusController with the given service.
        /// </summary>
        /// <param name="service">The service.</param>
        public JustificationObjectivesController(IJustificationObjectiveService service)
        {
            Debug.Assert(service != null, "The objective service must not be null.");
            this.service = service;
        }

        [ResponseType(typeof(PagedQueryResults<JustificationObjectiveDTO>))]
        public async Task<IHttpActionResult> GetJustificationObjectivesAsync([FromUri]PagingQueryBindingModel<JustificationObjectiveDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await this.service.GetAsync(queryModel.ToQueryableOperator(DEFAULT_JUSTIFICATIONOBJECTIVES_DTO_SORTER));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
