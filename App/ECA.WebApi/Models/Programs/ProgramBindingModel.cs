using ECA.Business.Models.Programs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Programs
{
    public class ProgramBindingModel : DraftProgramBindingModel
    {
        /// <summary>
        /// The Program Id.
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// The status of the program.
        /// </summary>
        [Required]
        public int ProgramStatusId { get; set; }

        /// <summary>
        /// Returns a business entity capable of updating system programs.
        /// </summary>
        /// <param name="userId">The user performing the update.</param>
        /// <returns>The EcaProgram business entity.</returns>
        public EcaProgram ToEcaProgram(int userId)
        {
            return new EcaProgram(
                updatedBy: new Business.Service.User(userId),
                id: this.Id,
                name: this.Name,
                description: this.Description,
                startDate: this.StartDate,
                endDate: this.EndDate,
                ownerOrganizationId: this.OwnerOrganizationId,
                parentProgramId: this.ParentProgramId,
                programStatusId: this.ProgramStatusId,
                focus: this.Focus,
                website: this.Website,
                goalIds: this.Goals,
                pointOfContactIds: this.Contacts,
                themeIds: this.Themes
                );
        }
    }
}