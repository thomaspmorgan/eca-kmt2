﻿using System;
using System.Collections.Generic;

namespace ECA.Business.Models.Programs
{
    /// <summary>
    /// A EcaProgram is a program is a non-draft program in the ECA system.
    /// </summary>
    public class EcaProgram
    {
        /// <summary>
        /// Creates a new default instance and initializes IEnumerable properties.
        /// </summary>
        public EcaProgram()
        {
            this.ContactIds = new List<int>();
            this.CountryIds = new List<int>();
            this.CountryIsos = new List<string>();
            this.GoalIds = new List<int>();
            this.ThemeIds = new List<int>();
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
        /// Gets or sets the Theme Ids.
        /// </summary>
        public IEnumerable<int> ThemeIds { get; set; }

        /// <summary>
        /// Gets or sets the Country Ids.
        /// </summary>
        public IEnumerable<int> CountryIds { get; set; }

        /// <summary>
        /// Gets or sets the Country ISOs.
        /// </summary>
        public IEnumerable<string> CountryIsos { get; set; }

        /// <summary>
        /// Gets or sets the Goal Ids.
        /// </summary>
        public IEnumerable<int> GoalIds { get; set; }

        /// <summary>
        /// Gets or sets the Contact Ids.
        /// </summary>
        public IEnumerable<int> ContactIds { get; set; }

    }
}