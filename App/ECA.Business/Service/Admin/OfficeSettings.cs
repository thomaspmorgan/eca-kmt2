using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    public class OfficeSettings
    {
        /// <summary>
        /// The default objective label value.
        /// </summary>
        public const string OBJECTIVE_DEFAULT_LABEL = "Objective";

        /// <summary>
        /// The default category label value.
        /// </summary>
        public const string CATEGORY_DEFAULT_LABEL = "Category";

        /// <summary>
        /// The default focus label value.
        /// </summary>
        public const string FOCUS_DEFAULT_LABEL = "Focus";

        /// <summary>
        /// The default justification label value.
        /// </summary>
        public const string JUSTIFICATION_DEFAULT_LABEL = "Justification";

        /// <summary>
        /// Gets or sets the objective label.
        /// </summary>
        public string ObjectiveLabel { get; set; }

        /// <summary>
        /// Gets or sets the category label.
        /// </summary>
        public string CategoryLabel { get; set; }

        /// <summary>
        /// Gets or sets the focus label.
        /// </summary>
        public string FocusLabel { get; set; }

        /// <summary>
        /// Gets or sets the justification label.
        /// </summary>
        public string JustificationLabel { get; set; }

        /// <summary>
        /// True, if at least one objective value is required by this office.
        /// </summary>
        public bool IsObjectiveRequired { get; set; }

        /// <summary>
        /// True, if at least one category value is required by this office.
        /// </summary>
        public bool IsCategoryRequired { get; set; }

        /// <summary>
        /// Returns a formatted string of this OfficeSetting.
        /// </summary>
        /// <returns>A formatted string of this OfficeSetting.</returns>
        public override string ToString()
        {
            return String.Format("ObjectiveLabel:  [{0}], CategoryLabel:  [{1}], FocusLabel:  [{2}], JustificationLabel:  [{3}], IsObjectiveRequired:  [{4}], IsCategoryRequired:  [{5}]",
                this.ObjectiveLabel,
                this.CategoryLabel,
                this.FocusLabel,
                this.JustificationLabel,
                this.IsObjectiveRequired,
                this.IsCategoryRequired
                );
        }
    }
}
