using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// Business model to update pii
    /// </summary>
    public class UpdateGeneral : IAuditable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="updatedBy">The user updating the pii</param>
        /// <param name="personId">The person id</param>
        /// <param name="prominentCategories">A list of prominentCategories (Ids) for the person</param>
        public UpdateGeneral(
            User updatedBy,
            int personId,
            List<int> prominentCategories
            )
        {
            Contract.Requires(updatedBy != null, "The updated by user must not be null.");
            this.PersonId = personId;
            this.ProminentCategories = prominentCategories;
            this.Audit = new Create(updatedBy);
        }

        /// <summary>
        /// Gets or sets the person id
        /// </summary>
        public int PersonId { get; private set; }

        /// <summary>
        /// Gets or sets the prominent categories list of ids
        /// </summary>
        public List<int> ProminentCategories { get; set; }
   
        /// <summary>
        /// Gets or sets the audit record
        /// </summary>
        public Audit Audit { get; private set; }
    }
}
