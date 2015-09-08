using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// Business model to update contact section of person
    /// </summary>
    public class UpdateContactInfo : IAuditable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="updatedBy">The user updating the pii</param>
        /// <param name="personId">The person id</param>
        /// <param name="hasContactAgreement">Whether this person has a contact agreement</param>
        public UpdateContactInfo(
            User updatedBy,
            int personId,
            bool hasContactAgreement
            )
        {
            Contract.Requires(updatedBy != null, "The updated by user must not be null.");
            this.PersonId = personId;
            this.HasContactAgreement = hasContactAgreement;
            this.Audit = new Create(updatedBy);
        }

        /// <summary>
        /// Gets or sets the person id
        /// </summary>
        public int PersonId { get; private set; }

        /// <summary>
        /// Gets or sets whether the person has a contact agreement
        /// </summary>
        public bool HasContactAgreement { get; set; }
   
        /// <summary>
        /// Gets or sets the audit record
        /// </summary>
        public Audit Audit { get; private set; }
    }
}
