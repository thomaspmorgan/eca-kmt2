using ECA.Business.Service;
using ECA.Business.Service.Lookup;
using System.Collections.Generic;

namespace ECA.Business.Queries.Models.Persons
{
    /// <summary>
    /// The SimplePersonDependentDTO is used to represent dependents in the ECA system.
    /// </summary>
    public class SimplePersonDependentDTO : SimplePersonDTO, IAuditable
    {
        /// <summary>
        /// Gets the person type id.
        /// </summary>
        public int PersonTypeId { get; set; }

        /// <summary>
        /// Gets and sets the countries of citizenship
        /// </summary>
        public List<SimpleLookupDTO> CountriesOfCitizenship { get; set; }

        /// <summary>
        /// Gets or sets the premanent residence country code
        /// </summary>
        public int PermanentResidenceCountryCode { get; set; }

        /// <summary>
        /// Gets or sets the birth country reason
        /// </summary>
        public string BirthCountryReason { get; set; }
        
        /// <summary>
        /// Gets and sets the audit record
        /// </summary>
        public Audit Audit { get; set; }        
    }
}
