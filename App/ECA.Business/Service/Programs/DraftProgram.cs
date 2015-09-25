using ECA.Business.Service;
using ECA.Business.Service.Programs;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Models.Programs
{
    /// <summary>
    /// A DraftProgram is an EcaProgram that is not ready to be published.
    /// </summary>
    public class DraftProgram : EcaProgram
    {
        /// <summary>
        /// Creates a new draft program.
        /// </summary>
        /// <param name="createdBy">The user creating a new program.</param>
        /// <param name="name">The name of the program.</param>
        /// <param name="description">The description.</param>
        /// <param name="startDate">The program start date.</param>
        /// <param name="endDate">The program end date.</param>
        /// <param name="ownerOrganizationId">The owner organization by id.</param>
        /// <param name="parentProgramId">The parent program by id.</param>
        /// <param name="goalIds">The goals of the program by id.</param>
        /// <param name="pointOfContactIds">The points contact by id.</param>
        /// <param name="themeIds">The themese of the program by id.</param>
        /// <param name="regionIds">The regions the program is operating in by id.</param>
        /// <param name="categoryIds">The focus categories by id.</param>
        /// <param name="objectiveIds">The objectivs by id.</param>
        /// <param name="websites">The websites</param>
        public DraftProgram(
            User createdBy,
            string name,
            string description,
            DateTimeOffset startDate,
            DateTimeOffset? endDate,
            int ownerOrganizationId,
            int? parentProgramId,
            List<int> goalIds,
            List<int> pointOfContactIds,
            List<int> themeIds,
            List<int> regionIds,
            List<int> categoryIds,
            List<int> objectiveIds,
            List<WebsiteDTO> websites)
            : base(
                updatedBy: createdBy,
                id: 0,
                name: name,
                description: description,
                startDate: startDate,
                endDate: endDate,
                ownerOrganizationId: ownerOrganizationId,
                parentProgramId: parentProgramId,
                programStatusId: ProgramStatus.Draft.Id,
                programRowVersion: null,
                goalIds: goalIds,
                pointOfContactIds: pointOfContactIds,
                themeIds: themeIds,
                regionIds: regionIds,
                categoryIds: categoryIds,
                objectiveIds: objectiveIds,
                websites: websites
                )
        {
            Contract.Requires(createdBy != null, "The created by user must not be null.");
            this.Audit = new Create(createdBy);
        }
    }
}
