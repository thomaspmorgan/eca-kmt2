using ECA.Business.Validation;
using ECA.Business.Validation.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Threading.Tasks;
using ECA.Data;
using ECA.Business.Queries.Persons;
using System.Linq;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Diagnostics.Contracts;
//using System.Data;
//using System.IO;
//using System.Xml.Serialization;

namespace ECA.Business.Service.Persons
{
    public class PersonSevisServiceValidator : SevisValidatorBase<SEVISBatchCreateUpdateEV>, IPersonSevisServiceValidator
    {        
        /// <summary>
        /// Do validation for sevis object, which includes participant person object
        /// </summary>
        /// <param name="participantId">Entity to validate</param>
        /// <returns>validation results</returns>        
        public List<ValidationResult> ValidateSevis(EcaContext context, int participantId)
        {
            throw new NotImplementedException();

            //var updateStudent = GetStudent(context, participantId);

            //var validator = new SEVISBatchCreateUpdateEVValidator();
            //var results = validator.Validate(updateStudent);

            //var final = new List<ValidationResult>();

            //foreach (var error in results.Errors)
            //{
            //    final.Add(new ValidationResult(error.ErrorMessage));
            //}

            //return final;
        }

        public async Task<List<ValidationResult>> ValidateSevisAsync(EcaContext context, int participantId)
        {
            throw new NotImplementedException();

            //var updateStudent = GetStudent(context, participantId);

            //var validator = new SEVISBatchCreateUpdateEVValidator();
            //var results = await validator.ValidateAsync(updateStudent);

            //var final = new List<ValidationResult>();
            //foreach (var error in results.Errors)
            //{
            //    final.Add(new ValidationResult(error.ErrorMessage));
            //}

            //// temporary to test xml serialization
            ////string temp = GetStudentUpdateXml(updateStudent);

            //return final;
        }

        /// <summary>
        /// Get populated participant person sevis object
        /// </summary>
        /// <param name="context"></param>
        /// <param name="participantId"></param>
        /// <returns></returns>
        public SEVISBatchCreateUpdateEV GetExchangeVisitor(EcaContext context, int participantId)
        {
            throw new NotImplementedException();

            // Get student details
            //var participant = ParticipantQueries.CreateGetParticipantDTOByIdQuery(context, participantId).FirstOrDefault();
            //var personalGeneral = PersonQueries.CreateGetGeneralByIdQuery(context, (int)participant.PersonId).FirstOrDefault();
            //var personalPII = PersonQueries.CreateGetPiiByIdQuery(context, (int)participant.PersonId).FirstOrDefault();
            //var personalEmail = PersonQueries.CreateGetContactInfoByIdQuery(context, (int)participant.PersonId).Select(x => x.EmailAddresses).FirstOrDefault();
            //var studentInfo = PersonQueries.CreateGetEducationsByPersonIdQuery(context, (int)participant.PersonId).FirstOrDefault();
            //var primaryAddress = personalPII.Addresses.Where(x => x.IsPrimary == true).FirstOrDefault();
            //var ecaService = new EcaService(context);
            //var citizenship = ecaService.GetLocationById(personalPII.CountriesOfCitizenship.FirstOrDefault().Id);

            //var student = new Student();
            //student.requestID = "1";
            //student.userID = participant.PersonId.ToString();
            //student.IssueReason = "I";
            //// personal info
            //student.personalInfo = new PersonalInfo
            //{
            //    BirthCountryCode = personalPII.PlaceOfBirth != null ? personalPII.PlaceOfBirth.CountryIso2 : "",
            //    BirthDate = personalPII.DateOfBirth != null ? personalPII.DateOfBirth.Value.Date : (DateTime?) null,
            //    CitizenshipCountryCode = citizenship != null ? citizenship.LocationIso2 : "",
            //    Email = personalEmail != null ? personalEmail.Select(x => x.Address).FirstOrDefault() : "",
            //    fullName = new FullName
            //    {
            //        FirsName = personalPII.FirstName,
            //        LastName = personalPII.LastName,
            //        Suffix = personalPII.NameSuffix,
            //        PreferredName = personalPII.Alias
            //    },
            //    Gender = personalPII.GenderId.ToString(),
            //    VisaType = "H1"
            //};
            //if (primaryAddress != null)
            //{
            //    if (primaryAddress.CountryIso2 == "US")
            //    {
            //        // us address
            //        student.usAddress = new USAddress
            //        {
            //            Address1 = primaryAddress.Street1,
            //            Address2 = primaryAddress.Street2,
            //            City = primaryAddress.City,
            //            PostalCode = primaryAddress.PostalCode
            //        };
            //    }
            //    else if (!string.IsNullOrEmpty(primaryAddress.CountryIso2))
            //    {
            //        // foreign address
            //        // TODO: foreign address required if new student
            //        //if (isNew) {
            //        student.foreignAddress = new ForeignAddress
            //        {
            //            address1 = primaryAddress.Street1,
            //            address2 = primaryAddress.Street2,
            //            city = primaryAddress.City,
            //            postalCode = primaryAddress.PostalCode,
            //            countryCode = primaryAddress.CountryIso2
            //        };
            //        //}
            //    }
            //}
            //if (studentInfo != null)
            //{
            //    // education
            //    student.educationalInfo = new EducationalInfo
            //    {
            //        PrgStartDate = studentInfo.StartDate.DateTime > DateTime.MinValue ? studentInfo.StartDate.DateTime : DateTime.MinValue,
            //        PrgEndDate = studentInfo.EndDate.HasValue ? studentInfo.EndDate.Value.DateTime : DateTime.MinValue
            //        //        eduLevel = new EduLevel
            //        //        {
            //        //            Level = visitorInfo != null ? visitorInfo.EducationLevel : "",
            //        //            OtherRemarks = visitorInfo != null ? visitorInfo.EducationLevelOtherRemarks : ""
            //        //        },
            //        //        engProficiency = new EngProficiency
            //        //        {
            //        //            EngRequired = visitorInfo != null ? visitorInfo.IsEnglishLanguageProficiencyReqd : false,
            //        //            RequirementsMet = visitorInfo != null ? visitorInfo.IsEnglishLanguageProficiencyMet : false,
            //        //            NotRequiredReason = visitorInfo != null ? visitorInfo.EnglishLanguageProficiencyNotReqdReason : ""
            //        //        },
            //        //        LengthOfStudy = visitorInfo != null ? visitorInfo.LengthOfStudy.ToString() : "",
            //        //        PrimaryMajor = visitorInfo != null ? visitorInfo.PrimaryMajor : "",
            //        //        SecondMajor = visitorInfo != null ? visitorInfo.SecondaryMajor : "",
            //        //        Minor = visitorInfo != null ? visitorInfo.Minor : "",
            //        //        Remarks = visitorInfo != null ? studentInfo.Title : ""
            //        //    };
            //        //}
            //        //if (visitorInfo != null)
            //        //{
            //        //    // financial

            //    };
            //}
            //// TODO: complete when dependent feature is available
            //student.createDependent = null;
            //student.Remarks = "";
            ////student.createDependent = new CreateDependent
            ////{
            ////    Dependent = new AddDependent {

            ////    },
            ////    Remarks = ""
            ////};

            //var batchHeader = new BatchHeader
            //{
            //    BatchID = "1",
            //    OrgID = "1"
            //};
            //var createStudent = new CreateStudent
            //{
            //    student = student
            //};
            //var updateStudent = new SEVISBatchUpdateStudent
            //{
            //    userID = "1",
            //    batchHeader = batchHeader,
            //    createStudent = createStudent
            //};

            //return updateStudent;
        }

        //public string GetStudentUpdateXml(SEVISBatchCreateUpdateEV validationEntity)
        //{
        //    XmlSerializer serializer = new XmlSerializer(validationEntity.GetType());
        //    var settings = new XmlWriterSettings
        //    {
        //        NewLineHandling = NewLineHandling.Entitize,
        //        Encoding = System.Text.Encoding.UTF8,
        //        DoNotEscapeUriAttributes = true
        //    };
        //    using (var stream = new StringWriter())
        //    using (var writer = XmlWriter.Create(stream, settings))
        //    {
        //        serializer.Serialize(writer, validationEntity);

        //        return stream.ToString();
        //    }
        //}
        
        public override List<ValidationResult> DoValidateSevis(SEVISBatchCreateUpdateEV validationEntity)
        {
            throw new NotImplementedException();
        }

        // TODO: for sending XML content to Sevis service
        //var xsdPath = System.AppDomain.CurrentDomain.BaseDirectory;
        //DataSet MyDataSet = new DataSet();
        //MyDataSet.ReadXmlSchema(@"schema.xsd");
        //string entityXml = SerializeToXmlString(validationEntity);
        //MyDataSet.ReadXml(entityXml);
    }
}
