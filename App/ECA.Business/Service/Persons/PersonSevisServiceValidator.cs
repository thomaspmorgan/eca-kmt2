using ECA.Business.Validation;
using System;
using System.Collections.Generic;
//using System.Data;
//using System.IO;
//using System.Xml.Serialization;

namespace ECA.Business.Service.Persons
{
    public class PersonSevisServiceValidator : SevisValidatorBase<SEVISBatchCreateUpdateStudent>
    {
        /// <summary>
        /// Person not found
        /// </summary>
        public const string PERSON_NOT_FOUND = "The participant person could not be found.";

        /// <summary>
        /// Do validation for sevis object, which includes participant person object
        /// </summary>
        /// <param name="validationEntity">Entity to validate</param>
        /// <returns>Business validation results</returns>        
        public override IEnumerable<SevisValidationResult> DoValidateSevis(SEVISBatchCreateUpdateStudent validationEntity)
        {
            //if (validationEntity.sevisPerson == null)
            //{
            //    yield return new SevisValidationResult<PersonSevisServiceValidationEntity>(x => x.sevisPerson, PERSON_NOT_FOUND);
            //}

            //if (validationEntity.sevisPerson.FirstName == null)
            //{

            //}

            throw new NotImplementedException();





            //var xsdPath = System.AppDomain.CurrentDomain.BaseDirectory;
            //DataSet MyDataSet = new DataSet();
            //MyDataSet.ReadXmlSchema(@"schema.xsd");
            //string entityXml = SerializeToXmlString(validationEntity);
            //MyDataSet.ReadXml(entityXml);            
        }
        
        //private string SerializeToXmlString(SEVISBatchCreateUpdateStudent validationEntity)
        //{
        //    string retVal = string.Empty;
        //    TextWriter writer = new StringWriter();
        //    XmlSerializer serializer = new XmlSerializer(validationEntity.GetType());
        //    serializer.Serialize(writer, validationEntity);
        //    retVal = writer.ToString();
        //    return retVal;
        //}
    }
}
