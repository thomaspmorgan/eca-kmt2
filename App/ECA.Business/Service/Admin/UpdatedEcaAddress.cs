using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    public class UpdatedEcaAddress : EcaAddress
    {
        public UpdatedEcaAddress(
            User updator,
            int addressId,
            int addressTypeId,
            string addressDisplayName,
            string street1,
            string street2,
            string street3,
            string postalCode,
            string locationName,
            int countryId,
            int cityId,
            int? divisionId,
            int organizationId
            )
            : base(addressTypeId, addressDisplayName, street1, street2, street3, postalCode, locationName, countryId, cityId, divisionId)
        {
            Contract.Requires(updator != null, "The updator must not be null.");
            this.Update = new Update(updator);
            this.AddressId = addressId;
        }

        public int AddressId { get; private set; }

        public Audit Update { get; private set; }
    }
}
