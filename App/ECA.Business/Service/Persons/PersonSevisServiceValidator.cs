using ECA.Business.Validation;
using ECA.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public override IEnumerable<ValidationResult> DoValidateSevis(SEVISBatchCreateUpdateStudent validationEntity)
        {
            var items = new Dictionary<object, object> { { EcaContext.VALIDATABLE_CONTEXT_KEY, null } };
            var vc = new ValidationContext(validationEntity, null, items);
            var results = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(validationEntity, vc, results, true);

            return results;        
        }
        
        //var xsdPath = System.AppDomain.CurrentDomain.BaseDirectory;
        //DataSet MyDataSet = new DataSet();
        //MyDataSet.ReadXmlSchema(@"schema.xsd");
        //string entityXml = SerializeToXmlString(validationEntity);
        //MyDataSet.ReadXml(entityXml);    

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
