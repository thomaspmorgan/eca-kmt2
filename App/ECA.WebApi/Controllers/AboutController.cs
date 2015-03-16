using ECA.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace ECA.WebApi.Controllers
{
    /// <summary>
    /// The AboutController provides information regarding the current instance of the web api application that is running.
    /// </summary>
    public class AboutController : ApiController
    {
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
    }
}
