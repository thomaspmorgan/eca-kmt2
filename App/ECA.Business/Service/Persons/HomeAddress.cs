using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    public class HomeAddress : IAuditable
    {
        private User updatedBy;

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
        public string Street1 { get; private set; }
        public string Street2 { get; private set; }
        public string Street3 { get; private set; }
        public int CityId { get; private set; }
        public int CountryId { get; private set; }
        public string PostalCode { get; private set; }
        public Audit Audit { get; private set; }
    }
}
