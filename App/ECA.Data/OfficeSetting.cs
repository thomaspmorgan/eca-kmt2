using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    /// <summary>
    /// A Focus is a concentration that a project encompasses.
    /// </summary>
    public class OfficeSetting
    {
        /// <summary>
        /// The office setting key for justifications.
        /// </summary>
        public const string JUSTIFICATION_SETTING_KEY = "Justification";

        /// <summary>
        /// The office setting key for categories.
        /// </summary>
        public const string CATEGORY_SETTING_KEY = "Category";

        /// <summary>
        /// The office setting key for objective.
        /// </summary>
        public const string OBJECTIVE_SETTING_KEY = "Objective";

        /// <summary>
        /// The office setting key for focus.
        /// </summary>
        public const string FOCUS_SETTING_KEY = "Focus";

        /// <summary>
        /// The max length of the focus.
        /// </summary>
        public const int NAME_MAX_LENGTH = 50;

        /// <summary>
        /// Creates a new Focus.
        /// </summary>
        public OfficeSetting()
        {
        }

        /// <summary>
        /// Gets or set the Id.
        /// </summary>
        public int OfficeSettingId { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        ///
        [MaxLength(NAME_MAX_LENGTH)]
        public string Name { get; set; }

        
        /// <summary>
        /// Gets or sets the Value.
        /// </summary>
        ///
        [MaxLength(NAME_MAX_LENGTH)]
        public string Value { get; set; }

        /// <summary>
        /// Office for the Focus area
        /// </summary>
        public int OfficeId { get; set; }

        /// <summary>
        /// Office of the Focus Area
        /// </summary>
        [ForeignKey("OfficeId")]
        public Organization Office { get; set; }

    }
}