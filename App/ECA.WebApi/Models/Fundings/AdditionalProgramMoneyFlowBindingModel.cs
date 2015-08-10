using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Fundings
{
    /// <summary>
    /// The AdditionalProgramMoneyFlowBindingModel is used by a web api client to add a money flow to a program.
    /// </summary>
    public class AdditionalProgramMoneyFlowBindingModel : AdditionalMoneyFlowBindingModel<Program>
    {
        /// <summary>
        /// Gets or sets the program id.
        /// </summary>
        public int ProgramId { get; set; }

        /// <summary>
        /// Returns the program source recipient type.
        /// </summary>
        /// <returns>The program source recipient type.</returns>
        public override int GetEntityTypeId()
        {
            return MoneyFlowSourceRecipientType.Program.Id;
        }

        /// <summary>
        /// Returns the program id.
        /// </summary>
        /// <returns>The program id.</returns>
        public override int GetEntityId()
        {
            return this.ProgramId;
        }
    }
}