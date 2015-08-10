using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Fundings
{
    /// <summary>
    /// An AdditionalProjectMoneyFlowBindingModel is used when a client is adding a new money flow and the source is a project.
    /// </summary>
    public class AdditionalProjectMoneyFlowBindingModel : AdditionalMoneyFlowBindingModel<Project>
    {
        /// <summary>
        /// The project id the money flow is for.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Returns the project money flow source type id.
        /// </summary>
        /// <returns>Returns the project money flow source recipient type.</returns>
        public override int GetEntityTypeId()
        {
            return MoneyFlowSourceRecipientType.Project.Id;
        }

        /// <summary>
        /// Returns the project id.
        /// </summary>
        /// <returns>The project id.</returns>
        public override int GetEntityId()
        {
            return this.ProjectId;
        }
    }
}