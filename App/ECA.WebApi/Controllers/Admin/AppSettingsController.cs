using ECA.WebApi.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace ECA.WebApi.Controllers.Admin
{
    /// <summary>
    /// App settings controller
    /// </summary>
    public class AppSettingsController : ApiController
    {
        /// <summary>
        /// Gets app settings
        /// </summary>
        /// <returns>App settings</returns>
        [ResponseType(typeof(AppSettingsViewModel))]
        public IHttpActionResult Get()
        {
            var model = new AppSettingsViewModel();
            return Ok(model);
        }
    }
}
