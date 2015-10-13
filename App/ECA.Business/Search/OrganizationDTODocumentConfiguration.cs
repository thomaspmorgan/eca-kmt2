using ECA.Business.Queries.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Search
{
    /// <summary>
    /// A OrganizationDTODocumentConfiguration is used to configure a OrganizationDTO as an ECADocument.
    /// </summary>
    public class OrganizationDTODocumentConfiguration : DocumentConfiguration<OrganizationDTO, int>
    {
        /// <summary>
        /// The organization dto document type id.
        /// </summary>
        public static Guid ORGANIZATION_DTO_DOCUMENT_TYPE_ID = Guid.Parse("10cc58a5-693b-47e5-af4d-73fd2883ca60");

        /// <summary>
        /// The name of the organization document type.
        /// </summary>
        public const string ORGANIZATION_DOCUMENT_TYPE_NAME = "Organization";

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public OrganizationDTODocumentConfiguration()
        {
            IsDocumentType(ORGANIZATION_DTO_DOCUMENT_TYPE_ID, ORGANIZATION_DOCUMENT_TYPE_NAME);
            HasKey(x => x.OrganizationId);
            HasName(x => x.Name);
            HasDescription(x => x.Description);
            HasAddresses(x => x.Addresses.Select(a => a.ToString()).ToList());            
            HasWebsites(x => new List<string> { x.Website });
        }
    }
}
