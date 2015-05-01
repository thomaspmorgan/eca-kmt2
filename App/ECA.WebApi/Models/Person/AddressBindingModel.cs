using ECA.Business.Service;
using ECA.Business.Service.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Person
{
    public class AddressBindingModel
    {
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string Street3 { get; set; }
        public int CityId { get; set; }
        public int CountryId { get; set; }
        public string PostalCode { get; set; }

        public HomeAddress ToHomeAddress(User user)
        {
            return new HomeAddress(
                updatedBy: user,
                street1: this.Street1,
                street2: this.Street2,
                street3: this.Street3,
                cityId: this.CityId,
                countryId: this.CountryId,
                postalCode: this.PostalCode
            );
        }
    }
}