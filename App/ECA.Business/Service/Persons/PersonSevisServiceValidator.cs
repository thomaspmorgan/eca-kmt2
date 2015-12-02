using ECA.Business.Validation;
using ECA.Business.Validation.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
//using System.Data;
//using System.IO;
//using System.Xml.Serialization;

namespace ECA.Business.Service.Persons
{
    public class PersonSevisServiceValidator : SevisValidatorBase<SEVISBatchCreateUpdateStudent>
    {
        public override List<ValidationResult> DoValidateSevis(SEVISBatchCreateUpdateStudent validationEntity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Do validation for sevis object, which includes participant person object
        /// </summary>
        /// <param name="validationEntity">Entity to validate</param>
        /// <returns>validation results</returns>        
        public new List<ValidationResult> ValidateSevis(SEVISBatchCreateUpdateStudent validationEntity)
        {
            //var results = new List<ValidationResult>();
            //var actual = Validator.TryValidateObject(validationEntity, new ValidationContext(validationEntity), results, true);

            // temporary object to return validation results
            var batchHeader = new BatchHeader
            {
                BatchID = "1",
                OrgID = "1"
            };
            var createStudent = new CreateStudent
            {
                student = null
            };
            var updateStudent = new SEVISBatchCreateUpdateStudent
            {
                userID = "1",
                batchHeader = batchHeader,
                createStudent = createStudent
            };

            var validator = new SEVISBatchCreateUpdateStudentValidator();
            var results = validator.Validate(updateStudent);

            return results.Errors as List<ValidationResult>;
        }
        
        // TODO: for sending XML content to Sevis service

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
