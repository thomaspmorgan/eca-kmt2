using System;

namespace ECA.Business.Queries.Models.Persons
{
    public class EvaluationNoteDTO
    {
        public int EvaluationNoteId { get; set; }
        public string EvaluationNote { get; set; }
        public int PersonId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTimeOffset AddedOn { get; set; }
        public string EmailAddress { get; set; }
    }
}
