using ECA.Business.Queries.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Search
{
    /// <summary>
    /// A ProjectDTODocumentConfiguration is used to configure a ProjectDTO as an ECADocument.
    /// </summary>
    public class ProjectDTODocumentConfiguration : DocumentConfiguration<ProjectDTO, int>
    {
        /// <summary>
        /// The project dto document type id.
        /// </summary>
        public static Guid PROJECT_DTO_DOCUMENT_TYPE_ID = Guid.Parse("c886883f-627c-4b2f-87e9-2f77ccb569c7");

        /// <summary>
        /// The name of the project document.
        /// </summary>
        public const string PROJECT_DOCUMENT_TYPE_NAME = "Project";
        
        /// <summary>
        /// Creates a new default instance.
        /// </summary>
        public ProjectDTODocumentConfiguration()
        {
            IsDocumentType(PROJECT_DTO_DOCUMENT_TYPE_ID, PROJECT_DOCUMENT_TYPE_NAME);
            HasKey(x => x.Id);
            HasName(x => x.Name);
            HasDescription(x => x.Description);
            HasStatus(x => x.Status);
            HasLocations(x => x.Locations.Where(l => !String.IsNullOrWhiteSpace(l.Name)).Select(l => l.Name).Distinct().ToList());
            HasOfficeSymbol(x => x.OwnerOfficeSymbol);
            HasPointsOfContact(x => x.Contacts.Select(c => c.Value).ToList());
            HasFoci(x => x.Categories.Select(c => c.Name).ToList());
            HasGoals(x => x.Goals.Select(c => c.Value).ToList());
            HasObjectives(x => x.Objectives.Select(y => y.Name).ToList());
            HasThemes(x => x.Themes.Select(t => t.Value).ToList());
            HasStartDate(x => x.StartDate);
            HasEndDate(x => x.EndDate);
        }
    }
}
