using AutoMapper;
using ECA.Business.Models.Programs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Programs
{
    /// <summary>
    /// A ProgramBindingModel is used when a client wishes to update a program.
    /// </summary>
    public class ProgramBindingModel : DraftProgramBindingModel
    {
        /// <summary>
        /// A base 64 regular expression.
        /// </summary>
        public const string BASE64_REGULAR_EXPRESSION = @"[^-A-Za-z0-9+/=]|=[^=]|={3,}$";

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
        /// The row version of the program.
        /// </summary>
        [Required]
        [RegularExpression(BASE64_REGULAR_EXPRESSION, ErrorMessage = "The given row version is not a valid base 64 string.")]
        public string RowVersion { get; set; }

        /// <summary>
        /// Returns a business entity capable of updating system programs.
        /// </summary>
        /// <param name="user">The user making the change.</param>
        /// <returns>The EcaProgram business entity.</returns>
        public EcaProgram ToEcaProgram(Business.Service.User user)
        {
            Contract.Assert(this.RowVersion != null, "The row version must not be null.");
            return new EcaProgram(
                updatedBy: user,
                id: this.Id,
                name: this.Name,
                description: this.Description,
                startDate: this.StartDate,
                endDate: this.EndDate,
                ownerOrganizationId: this.OwnerOrganizationId,
                parentProgramId: this.ParentProgramId,
                programStatusId: this.ProgramStatusId,
                focusId: this.FocusId,
                programRowVersion: Convert.FromBase64String(this.RowVersion),
                website: this.Website,
                goalIds: this.Goals,
                pointOfContactIds: this.Contacts,
                themeIds: this.Themes,
                regionIds: this.Regions
                );
        }

    }
}