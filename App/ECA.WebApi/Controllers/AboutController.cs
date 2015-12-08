using System.Linq;
using ECA.Core.Generation;
using ECA.WebApi.Models;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Web.Http;
using System.Web.Http.Description;

namespace ECA.WebApi.Controllers
{
    /// <summary>
    /// The AboutController provides information regarding the current instance of the web api application that is running.
    /// </summary>
    [RoutePrefix("api/About")]
    [Authorize]
    public class AboutController : ApiController
    {
        private IStaticGeneratorValidator validator;

        /// <summary>
        /// Creates a new controller with the given lookup validator.
        /// </summary>
        /// <param name="validator">The validator.</param>
        public AboutController(IStaticGeneratorValidator validator)
        {
            Contract.Requires(validator != null, "The validator must not be null.");
            this.validator = validator;
        }

        /// <summary>
        /// Returns build and assembly information.
        /// </summary>
        /// <returns>Build and assembly information.</returns>
        [ResponseType(typeof(AboutViewModel))]
        public IHttpActionResult Get()
        {
            var model = new AboutViewModel();
            return Ok(model);
        }

        /// <summary>
        /// Returns lookup errors.
        /// </summary>
        /// <returns>Returns lookup errors.</returns>
        [ResponseType(typeof(List<string>))]
        [Route("LookupErrors")]
        public IHttpActionResult GetLookupErrors()
        {
            var ecaErrors = ECA.Data.EcaDataValidator.ValidateAll(this.validator);
            var camErrors = CAM.Data.CamDataValidator.ValidateAll(this.validator);
            var lookupErrors = new List<string>();
            lookupErrors.AddRange(ecaErrors);
            lookupErrors.AddRange(camErrors);
            lookupErrors = lookupErrors.OrderBy(x => x).ToList();
            return Ok(lookupErrors);
        }
    }
}
