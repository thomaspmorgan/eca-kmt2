using ECA.Business.Queries.Models.Admin;
using ECA.Business.Queries.Models.Programs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Search
{
    /// <summary>
    /// A ProgramDTODocumentConfiguration is used to configure a ProgramDTO as an ECADocument.
    /// </summary>
    public class ProgramDTODocumentConfiguration : DocumentConfiguration<ProgramDTO, int>
    {
        /// <summary>
        /// The program dto document type id.
        /// </summary>
        public static Guid PROGRAM_DTO_DOCUMENT_TYPE_ID = Guid.Parse("1f42ef8b-8000-4fb1-b0a6-c1f927716e9c");

        /// <summary>
        /// The name of the program document type.
        /// </summary>
        public const string PROGRAM_DOCUMENT_TYPE_NAME = "Program";

        /// <summary>
        /// Creates the new default instance.
        /// </summary>
        public ProgramDTODocumentConfiguration()
        {
            IsDocumentType(PROGRAM_DTO_DOCUMENT_TYPE_ID, PROGRAM_DOCUMENT_TYPE_NAME);
            HasKey(x => x.Id);
            HasName(x => x.Name);
            HasDescription(x => x.Description);
            HasOfficeSymbol(x => x.OwnerOfficeSymbol);
            HasStatus(x => x.ProgramStatusName);
            HasPointsOfContact(x => x.Contacts.Select(c => c.Value).ToList());
            HasFoci(x => x.Categories.Select(c => c.Name).ToList());
            HasGoals(x => x.Goals.Select(c => c.Value).ToList());
            HasObjectives(x => x.Objectives.Select(y => y.Name).ToList());
            HasThemes(x => x.Themes.Select(t => t.Value).ToList());
            HasRegions(x => x.Regions.Select(r => r.Name).ToList());
            HasWebsites(x => x.Websites.Select(w => w.Value).ToList());
        }
    }
}
