namespace ECA.Business.Service.Persons
{
    public class PersonSevisServiceValidationEntity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sevisPerson"></param>
        public PersonSevisServiceValidationEntity(UpdatedParticipantPersonSevis sevisPerson)
        {
            this.sevisPerson = sevisPerson;
        }

        /// <summary>
        /// 
        /// </summary>
        public UpdatedParticipantPersonSevis sevisPerson { get; private set; }
    }
}
