using ECA.Business.Service;
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
        /// <param name="focusId">The focus by id.</param>
        /// <param name="website">The website of the program.</param>
        /// <param name="goalIds">The goals of the program by id.</param>
        /// <param name="pointOfContactIds">The points contact by id.</param>
        /// <param name="themeIds">The themese of the program by id.</param>
        /// <param name="regionIds">The regions the program is operating in by id.</param>
        public DraftProgram(
            User createdBy,
            string name,
            string description,
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            int ownerOrganizationId,
            int? parentProgramId,
            int focusId,
            string website,
            List<int> goalIds,
            List<int> pointOfContactIds,
            List<int> themeIds,
            List<int> regionIds,
            List<int> categoryIds,
            List<int> objectiveIds)
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
                focusId: focusId,
                programRowVersion: null,
                website: website,
                goalIds: goalIds,
                pointOfContactIds: pointOfContactIds,
                themeIds: themeIds,
                regionIds: regionIds,
                categoryIds: categoryIds,
                objectiveIds: objectiveIds
                )
        {
            Contract.Requires(createdBy != null, "The created by user must not be null.");
            this.Audit = new Create(createdBy);
        }
    }
}
