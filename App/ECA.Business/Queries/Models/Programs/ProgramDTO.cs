using ECA.Business.Service.Lookup;
using ECA.Core.Data;
using System;
using System.Collections.Generic;
using ECA.Business.Queries.Models.Admin;

namespace ECA.Business.Queries.Models.Programs
{
    /// <summary>
    /// A ProgramDTO is used to represent a Program in the ECA system.
    /// </summary>
    public class ProgramDTO : IConcurrent
    {
        /// <summary>
        /// Creates a new default instance and initializes IEnumerable properties.
        /// </summary>
        public ProgramDTO()
        {
            this.Contacts = new List<SimpleLookupDTO>();
            this.CountryIsos = new List<SimpleLookupDTO>();
            this.RegionIsos = new List<SimpleLookupDTO>();
            this.Goals = new List<SimpleLookupDTO>();
            this.Themes = new List<SimpleLookupDTO>();
            this.Categories = new List<FocusCategoryDTO>();
            this.Objectives = new List<JustificationObjectiveDTO>();
        }

        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Start Date.
        /// </summary>
        public DateTimeOffset StartDate { get; set; }

        /// <summary>
        /// Gets or sets the Revised On date.
        /// </summary>
        public DateTimeOffset RevisedOn { get; set; }

        /// <summary>
        /// Gets or sets the Parent Program Id.
        /// </summary>
        public int? ParentProgramId { get; set; }

        /// <summary>
        /// Gets or sets the Parent Program Name.
        /// </summary>
        public string ParentProgramName { get; set; }

        /// <summary>
        /// Gets or sets the Themes.
        /// </summary>
        public IEnumerable<SimpleLookupDTO> Themes { get; set; }

        /// <summary>
        /// Gets or sets the Country Isos.
        /// </summary>
        public IEnumerable<SimpleLookupDTO> CountryIsos { get; set; }

        /// <summary>
        /// Gets or sets the Region Isos.
        /// </summary>
        public IEnumerable<SimpleLookupDTO> RegionIsos { get; set; }

        /// <summary>
        /// Gets or sets the Regions.
        /// </summary>
        public IEnumerable<LocationDTO> Regions { get; set; }

        /// <summary>
        /// Gets or sets the Goals.
        /// </summary>
        public IEnumerable<SimpleLookupDTO> Goals { get; set; }

        /// <summary>
        /// Gets or sets the Contacts.
        /// </summary>
        public IEnumerable<SimpleLookupDTO> Contacts { get; set; }

        /// <summary>
        /// Gets or sets the Categories
        /// </summary>
        public IEnumerable<FocusCategoryDTO> Categories { get; set; }

        /// <summary>
        /// Gets or sets the Justification Objectives
        /// </summary>
        public IEnumerable<JustificationObjectiveDTO> Objectives { get; set; }

        /// <summary>
        /// Gets or sets the focus.
        /// </summary>
        public SimpleLookupDTO Focus { get; set; }

        /// <summary>
        /// Gets or sets the Owner Name.
        /// </summary>
        public string OwnerName { get; set; }

        /// <summary>
        /// Gets or sets the Owner office symbol.
        /// </summary>
        public string OwnerOfficeSymbol { get; set; }

        /// <summary>
        /// Gets or sets the owner description.
        /// </summary>
        public string OwnerDescription { get; set; }

        /// <summary>
        /// Gets or sets the owner organization id.
        /// </summary>
        public int OwnerOrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the owner's category label.
        /// </summary>
        public string OwnerOrganizationCategoryLabel { get; set; }

        /// <summary>
        /// Gets or sets the owner's object label.
        /// </summary>
        public string OwnerOrganizationObjectiveLabel { get; set; }

        /// <summary>
        /// Gets or sets the row version.
        /// </summary>
        public byte[] RowVersion { get; set; }

        /// <summary>
        /// Gets or sets the program status id.
        /// </summary>
        public int ProgramStatusId { get; set; }

        /// <summary>
        /// Gets or sets the website.
        /// </summary>
        public string Website { get; set; }
    }
}