using System.ComponentModel.DataAnnotations;

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
