using ECA.Business.Validation;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ECA.Business.Service.Persons
{
    public class SevisValidationService
    {
        public SevisValidationService()
        {

        }

        /// <summary>
        /// Test validation on a sevis object.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>List of errors</returns>
        public IEnumerable<SevisValidationResult> TestSevisValidation(UpdatedParticipantPersonSevisValidationEntity entity)
        {
            Contract.Requires(entity != null, "The sevis entity must not be null.");
            var validator = new PersonSevisServiceValidator();

            var results = validator.ValidateUpdate(entity).ToList();

            return results;
        }
        
        //private string SerializeToXmlString(UpdatedParticipantPersonSevis updatedParticipantPersonSevis)
        //{
        //    string retVal = string.Empty;
        //    TextWriter writer = new StringWriter();
        //    XmlSerializer serializer = new XmlSerializer(updatedParticipantPersonSevis.GetType());
        //    serializer.Serialize(writer, updatedParticipantPersonSevis);
        //    retVal = writer.ToString();
        //    return retVal;
        //}

    }
}
