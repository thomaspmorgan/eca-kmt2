using ECA.Business.Service.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Admin
{
    /// <summary>
    /// The binding model for data point configuration
    /// </summary>
    public class DataPointConfigurationBindingModel
    {
        /// <summary>
        /// The office id
        /// </summary>
        public int? OfficeId { get; set; }

        /// <summary>
        /// The program id
        /// </summary>
        public int? ProgramId { get; set; }

        /// <summary>
        /// The project id
        /// </summary>
        public int? ProjectId { get; set; }

        /// <summary>
        /// The data point category property id
        /// </summary>
        public int CategoryPropertyId { get; set; }

        /// <summary>
        /// Converts binding model to business model
        /// </summary>
        /// <returns></returns>
        public NewDataPointConfiguration ToNewDataPointConfiguration()
        {
            return new NewDataPointConfiguration(OfficeId, ProgramId, ProjectId, CategoryPropertyId);
        }
    }
}