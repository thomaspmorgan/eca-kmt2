using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// Business model for home address
    /// </summary>
    public class HomeAddress : IAuditable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="updatedBy"></param>
        /// <param name="street1"></param>
        /// <param name="street2"></param>
        /// <param name="street3"></param>
        /// <param name="cityId"></param>
        /// <param name="countryId"></param>
        /// <param name="postalCode"></param>
        public HomeAddress(User updatedBy, string street1, string street2, string street3, int cityId, int countryId, string postalCode)
        {
            this.Street1 = street1;
            this.Street2 = street2;
            this.Street3 = street3;
            this.CityId = cityId;
            this.CountryId = countryId;
            this.PostalCode = postalCode;
            this.Audit = new Create(updatedBy);
        }

        /// <summary>
        /// Get and sets street 1
        /// </summary>
        public string Street1 { get; private set; }

        /// <summary>
        /// Gets and sets street 2
        /// </summary>
        public string Street2 { get; private set; }

        /// <summary>
        /// Gets and sets street 3
        /// </summary>
        public string Street3 { get; private set; }

        /// <summary>
        /// Gets and sets city id
        /// </summary>
        public int CityId { get; private set; }

        /// <summary>
        /// Gets and sets country id
        /// </summary>
        public int CountryId { get; private set; }

        /// <summary>
        /// Gets and sets postal code
        /// </summary>
        public string PostalCode { get; private set; }

        /// <summary>
        /// Gets and sets audit
        /// </summary>
        public Audit Audit { get; private set; }
    }
}
