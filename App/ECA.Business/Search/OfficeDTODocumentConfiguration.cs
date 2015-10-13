using ECA.Business.Queries.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Search
{
    /// <summary>
    /// A OfficeDTODocumentConfiguration is used to configure a OfficeDTO as an ECADocument.
    /// </summary>
    public class OfficeDTODocumentConfiguration : DocumentConfiguration<OfficeDTO, int>
    {
        /// <summary>
        /// The office dto document type id.
        /// </summary>
        public static Guid OFFICE_DTO_DOCUMENT_TYPE_ID = Guid.Parse("d71b411e-5ab0-4e23-afca-1fd530b9b4a7");

        /// <summary>
        /// The name of the office document type.
        /// </summary>
        public const string OFFICE_DOCUMENT_TYPE_NAME = "Office";

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public OfficeDTODocumentConfiguration()
        {
            IsDocumentType(OFFICE_DTO_DOCUMENT_TYPE_ID, OFFICE_DOCUMENT_TYPE_NAME);
            HasKey(x => x.Id);
            HasName(x => x.Name);
            HasOfficeSymbol(x => x.OfficeSymbol);
            HasDescription(x => x.Description);
            HasThemes(x => x.Themes.Select(t => t.Value).ToList());
            HasGoals(x => x.Goals.Select(t => t.Value).ToList());
            HasPointsOfContact(x => x.Contacts.Where(c => !String.IsNullOrWhiteSpace(c.Value)).Select(c => c.Value).ToList());
        }
    }
}