using ECA.Business.Queries.Models.Persons;
using ECA.Business.Service.Persons;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ECA.WebApi.Controllers.Persons
{
    /// <summary>
    /// Controller for people
    /// </summary>
    [RoutePrefix("api")]
    [Authorize]
    public class PeopleController : ApiController
    {
        private IPersonService service;

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="service">The service to inject</param>
        public PeopleController(IPersonService service)
        {
            Contract.Requires(service != null, "The participant service must not be null.");
            this.service = service;
        }

        /// <summary>
        /// Returns pii associated with person 
        /// </summary>
        /// <param name="personId">The person id to find pii for</param>
        /// <returns>Pii associated to person</returns>
        [ResponseType(typeof(PiiDTO))]
        [Route("People/{personId:int}/Pii")]
        public async Task<IHttpActionResult> GetPiiByIdAsync(int personId)
        {
            var pii = await this.service.GetPiiByIdAsync(personId);
            if (pii != null)
            {
                return Ok(pii);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Returns contact info associated with a person
        /// </summary>
        /// <param name="personId">The person id to find contact info for</param>
        /// <returns>Contact info associated with person</returns>
        [ResponseType(typeof(ContactInfoDTO))]
        [Route("People/{personId:int}/ContactInfo")]
        public async Task<IHttpActionResult> GetContactInfoByIdAsync(int personId)
        {
            var contactInfo = await this.service.GetContactInfoByIdAsync(personId);
            if (contactInfo != null)
            {
                return Ok(contactInfo);
            }
            else
            {
                return NotFound();
            }
        }

    }
}
