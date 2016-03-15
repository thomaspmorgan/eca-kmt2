using ECA.Business.Validation.Sevis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Persons.ExchangeVisitor
{
    public class SubjectFieldDTO
    {
        /// <summary>
        /// Gets or sets the subject field code for example: 12.1234.
        /// </summary>
        public string SubjectFieldCode { get; set; }

        /// <summary>
        /// Gets or sets the foreign degree level.
        /// </summary>
        public string ForeignDegreeLevel { get; set; }

        /// <summary>
        /// Gets or sets the foreign field of study.
        /// </summary>
        public string ForeignFieldOfStudy { get; set; }

        /// <summary>
        /// Gets or sets the remarks.
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// Returns the SubjectField instead from this dto.
        /// </summary>
        /// <returns>The SubjectField instance from this dto.</returns>
        public SubjectField GetSubjectField()
        {
            return new SubjectField(
                subjectFieldCode: this.SubjectFieldCode,
                foreignDegreeLevel: this.ForeignDegreeLevel,
                foreignFieldOfStudy: this.ForeignFieldOfStudy,
                remarks: this.Remarks
                );
        }
    }
}
