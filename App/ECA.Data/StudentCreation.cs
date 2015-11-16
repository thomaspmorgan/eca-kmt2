using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data
{
    public class StudentCreation : IHistorical
    {
        private const int DESCRIPTION_CODE_LENGTH = 100;

        public StudentCreation()
        {
            this.History = new History();
        }

        [Key]
        public int StudentCreationId { get; set; }

        public string CreationCode { get; set; }

        [MaxLength(DESCRIPTION_CODE_LENGTH)]
        public string Description { get; set; }

        public History History { get; set; }
    }
}
