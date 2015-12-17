using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    public class NewDataPointConfiguration
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="officeId">The office id</param>
        /// <param name="programId">The program id</param>
        /// <param name="projectId">The project id</param>
        /// <param name="dataPointCategoryPropertyId">The data point category property id</param>
        public NewDataPointConfiguration(int? officeId, int? programId, int? projectId, int dataPointCategoryPropertyId)
        {
            this.OfficeId = officeId;
            this.ProgramId = programId;
            this.ProjectId = projectId;
            this.DataPointCategoryPropertyId = dataPointCategoryPropertyId;
        }  

        /// <summary>
        /// The office id
        /// </summary>
        public int? OfficeId { get; set; }

        /// <summary>
        /// The program id
        /// </summary>
        public int? ProgramId { get; set; }

        /// <summary>
        /// The project id
        /// </summary>
        public int? ProjectId { get; set; }

        /// <summary>
        /// The data point category property id
        /// </summary>
        public int DataPointCategoryPropertyId { get; set; }
    }
}
