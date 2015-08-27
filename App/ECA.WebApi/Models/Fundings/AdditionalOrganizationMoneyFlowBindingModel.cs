using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Fundings
{
    /// <summary>
    /// The AdditionalOfficeMoneyFlowBindingModel is used by a web api client to add a money flow to an organization.
    /// </summary>
    public class AdditionalOrganizationMoneyFlowBindingModel : AdditionalMoneyFlowBindingModel
    {
        /// <summary>
        /// Gets or sets the organization id.
        /// </summary>
        public int OrganizationId { get; set; }

        /// <summary>
        /// Returns the organization source recipient type.
        /// </summary>
        /// <returns>The organization source recipient type.</returns>
        public override int GetEntityTypeId()
        {
            return MoneyFlowSourceRecipientType.Organization.Id;
        }

        /// <summary>
        /// Returns the organization id.
        /// </summary>
        /// <returns>The organization id.</returns>
        public override int GetEntityId()
        {
            return this.OrganizationId;
        }
    }
}