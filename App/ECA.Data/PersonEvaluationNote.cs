using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    public class PersonEvaluationNote: IHistorical
    {
        [Key]
        public int EvaluationNoteId { get; set; }
        public int PersonId { get; set; }
        public string EvaluationNote { get; set; }
        public History History { get; set; }

        public Person Person { get; set; }

        public PersonEvaluationNote()
        {
            History = new History();
        }

    }
}
