using ECA.Business.Validation;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<IQueryable<SevisValidationResult>> TestSevisValidation(UpdatedParticipantPersonSevisValidationEntity validationEntity)
        {
            Contract.Requires(validationEntity != null, "The validation entity must not be null.");
            var validator = new PersonSevisServiceValidator();
            var results = await validator.ValidateUpdate(validationEntity).AsQueryable().ToListAsync();

            return results.AsQueryable();
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
