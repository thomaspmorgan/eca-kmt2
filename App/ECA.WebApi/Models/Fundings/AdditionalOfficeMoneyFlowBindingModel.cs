using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Fundings
{
    /// <summary>
    /// The AdditionalOfficeMoneyFlowBindingModel is used by a web api client to add a money flow to an office.
    /// </summary>
    public class AdditionalOfficeMoneyFlowBindingModel : AdditionalMoneyFlowBindingModel
    {
        /// <summary>
        /// Gets or sets the office id.
        /// </summary>
        public int OfficeId { get; set; }

        /// <summary>
        /// Returns the organization source recipient type.
        /// </summary>
        /// <returns>The organization source recipient type.</returns>
        public override int GetEntityTypeId()
        {
            return MoneyFlowSourceRecipientType.Office.Id;
        }

        /// <summary>
        /// Returns the office id.
        /// </summary>
        /// <returns>The office id.</returns>
        public override int GetEntityId()
        {
            return this.OfficeId;
        }
    }
}