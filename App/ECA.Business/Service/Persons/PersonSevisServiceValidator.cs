using ECA.Business.Validation;
using ECA.Business.Validation.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Threading.Tasks;
//using System.Data;
//using System.IO;
//using System.Xml.Serialization;

namespace ECA.Business.Service.Persons
{
    public class PersonSevisServiceValidator : SevisValidatorBase<SEVISBatchUpdateStudent>
    {
        public override List<ValidationResult> DoValidateSevis(SEVISBatchUpdateStudent validationEntity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Do validation for sevis object, which includes participant person object
        /// </summary>
        /// <param name="participantId">Entity to validate</param>
        /// <returns>validation results</returns>        
        public List<ValidationResult> ValidateSevis(int participantId)
        {
            // TODO: get full sevis object
            
            // ****** temporary object to return validation results ***********
            var batchHeader = new BatchHeader
            {
                BatchID = "1",
                OrgID = "1"
            };
            var createStudent = new CreateStudent
            {
                student = null
            };
            var updateStudent = new SEVISBatchUpdateStudent
            {
                userID = "1",
                batchHeader = batchHeader,
                createStudent = createStudent
            };

            var validator = new SEVISBatchUpdateStudentValidator();
            var results = validator.Validate(updateStudent);

            var final = new List<ValidationResult>();

            foreach (var error in results.Errors)
            {
                final.Add(new ValidationResult(error.ErrorMessage));
            }
            
            return final;
        }

        public async Task<List<ValidationResult>> ValidateSevisAsync(int participantId)
        {
            // TODO: get full sevis object

            // ****** temporary object to return validation results ***********
            var batchHeader = new BatchHeader
            {
                BatchID = "1",
                OrgID = "1"
            };
            var createStudent = new CreateStudent
            {
                student = null
            };
            var updateStudent = new SEVISBatchUpdateStudent
            {
                userID = "1",
                batchHeader = batchHeader,
                createStudent = createStudent
            };

            var validator = new SEVISBatchUpdateStudentValidator();
            var results = await validator.ValidateAsync(updateStudent);
            
            var final = new List<ValidationResult>();
            foreach (var error in results.Errors)
        {
                final.Add(new ValidationResult(error.ErrorMessage));
            }

            return final;
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
