using ECA.Data;
using System.Diagnostics.Contracts;

namespace ECA.Business.Service.Persons
{
    public class NewPersonEvaluationNote
    {
        public NewPersonEvaluationNote(User user, string evaluationNote, int personId)
        {
            PersonId = personId;
            EvaluationNote = evaluationNote;
            Create = new Create(user);
        }

        public int PersonId { get; private set; }
        
        public string EvaluationNote { get; set; }
        
        public Create Create { get; private set; }

        public PersonEvaluationNote AddPersonEvaluationNote(Person person)
        {
            Contract.Requires(person != null, "The person entity must not be null.");
            var evalnote = new PersonEvaluationNote
            {
                EvaluationNote = this.EvaluationNote,
                PersonId = person.PersonId
            };
            this.Create.SetHistory(evalnote);
            person.EvaluationNotes.Add(evalnote);

            return evalnote;
        }

    }
}
