using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Admin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ECA.WebApi.Controllers.Admin
{
    /// <summary>
    /// The OfficesController is capable of performing crud operations for an office.
    /// </summary>
    public class OfficesController : ApiController
    {
        private IOfficeService service;

        /// <summary>
        /// Creates a new controller instance.
        /// </summary>
        /// <param name="service">The service.</param>
        public OfficesController(IOfficeService service)
        {
            Debug.Assert(service != null, "The office service must not be null.");
            this.service = service;
        }

        /// <summary>
        /// Returns the office with the given id.
        /// </summary>
        /// <param name="id">The id of the office.</param>
        /// <returns>The office.</returns>
        [ResponseType(typeof(OfficeDTO))]
        public async Task<IHttpActionResult> GetOfficeByIdAsync(int id)
        {
            var dto = await this.service.GetOfficeByIdAsync(id);
            if (dto != null)
            {
                return Ok(dto);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
