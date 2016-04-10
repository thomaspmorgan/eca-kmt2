﻿using System.ComponentModel.DataAnnotations;

namespace ECA.Data
{
    public class PersonDependentCitizenCountry
    {
        /// <summary>
        /// Gets or sets the dependent id.
        /// </summary>
        [Key]
        public int DependentId { get; set; }

        /// <summary>
        /// Gets or sets the Location Id.
        /// </summary>
        public int LocationId { get; set; }
    }
}