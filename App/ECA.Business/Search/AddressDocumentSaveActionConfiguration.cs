using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Search
{
    /// <summary>
    /// An AddressDocumentSaveActionConfiguration is used as a configuration for addresses captured by the DocumentsSaveAction.  It is necessary
    /// so that addresses modified in a DbContext will trigger the correct overall document to be indexed.
    /// </summary>
    public class AddressDocumentSaveActionConfiguration : DocumentSaveActionConfiguration<Address>
    {
        /// <summary>
        /// Creates a new instance of the configuration.  Currently Person indexing is not supported; therefore, documents whose
        /// id is based on a person will force a thrown exception.
        /// </summary>
        public AddressDocumentSaveActionConfiguration()
            : base(
                  x => x.OrganizationId.HasValue ? x.OrganizationId.Value
                  : x.Organization != null ? x.Organization.OrganizationId
                  : x.PersonId.HasValue ? x.PersonId.Value
                  : x.Person != null ? x.Person.PersonId
                  : -1,
                  x =>
                  {
                      if (x.PersonId.HasValue || x.Person != null)
                      {
                          throw new NotSupportedException("Currently person indexing is not supported and therefore changes to a person's address are not supported.");
                      }
                      else
                      {
                          return OrganizationDTODocumentConfiguration.ORGANIZATION_DTO_DOCUMENT_TYPE_ID;
                      }
                  }
                )
        {
            var exclusionRules = new List<Func<Address, bool>>();
            exclusionRules.Add((address) =>
            {
                if (address.PersonId.HasValue || address.Person != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            });
            this.CreatedExclusionRules = exclusionRules;
            this.ModifiedExclusionRules = exclusionRules;
            this.DeletedExclusionRules = exclusionRules;
        }
    }
}
