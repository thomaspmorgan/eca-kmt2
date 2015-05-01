using ECA.Business.Queries.Models.Persons;
using ECA.Business.Service.Persons;
using ECA.WebApi.Models.Person;
using ECA.WebApi.Security;
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
        private IUserProvider userProvider;

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="service">The service to inject</param>
        /// <param name="userProvider">The user provider.</param>
        public PeopleController(IPersonService service, IUserProvider userProvider)
        {
            Contract.Requires(service != null, "The participant service must not be null.");
            Contract.Requires(userProvider != null, "The user provider must not be null.");
            this.service = service;
            this.userProvider = userProvider;
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
            var pii = await service.GetPiiByIdAsync(personId);
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
            var contactInfo = await service.GetContactInfoByIdAsync(personId);
            if (contactInfo != null)
            {
                return Ok(contactInfo);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Post method to create a person
        /// </summary>
        /// <param name="model">The model to create</param>
        /// <returns>Success or error</returns>
        public async Task<IHttpActionResult> PostPersonAsync(PersonBindingModel model)
        {   
            if (ModelState.IsValid)
            {
                var currentUser = userProvider.GetCurrentUser();
                var businessUser = userProvider.GetBusinessUser(currentUser);
                var person = await service.CreateAsync(model.ToNewPerson(businessUser));
                await service.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [Route("People/Pii")]
        public async Task<IHttpActionResult> PutPiiAsync(PiiBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = userProvider.GetCurrentUser();
                var businessUser = userProvider.GetBusinessUser(currentUser);
                var person = await service.UpdatePiiAsync(model.ToUpdatePii(businessUser));
                await service.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
