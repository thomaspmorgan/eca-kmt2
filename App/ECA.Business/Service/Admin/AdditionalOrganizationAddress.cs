using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    public class AdditionalOrganizationAddress : AdditionalAddress<Organization>
    {
        private readonly int organizationId;

        public AdditionalOrganizationAddress(
            User creator,
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
            : base(creator, addressTypeId, addressDisplayName, street1, street2, street3, postalCode, locationName, countryId, cityId, divisionId)
        {
            Contract.Requires(creator != null, "The creator must not be null.");
            this.organizationId = organizationId;
        }

        public override int GetAddressableEntityId()
        {
            return organizationId;
        }
    }
}
