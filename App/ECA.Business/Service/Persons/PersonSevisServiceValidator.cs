using ECA.Business.Validation;
using ECA.Business.Validation.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Threading.Tasks;
using ECA.Data;
using ECA.Business.Queries.Persons;
using System.Linq;
//using System.Data;
//using System.IO;
//using System.Xml.Serialization;

namespace ECA.Business.Service.Persons
{
    public class PersonSevisServiceValidator : SevisValidatorBase<SEVISBatchUpdateStudent>, IPersonSevisServiceValidator
    {        
        /// <summary>
        /// Do validation for sevis object, which includes participant person object
        /// </summary>
        /// <param name="participantId">Entity to validate</param>
        /// <returns>validation results</returns>        
        public List<ValidationResult> ValidateSevis(EcaContext context, int participantId)
        {
            var updateStudent = GetStudent(context, participantId);
            
            var validator = new SEVISBatchUpdateStudentValidator();
            var results = validator.Validate(updateStudent);

            var final = new List<ValidationResult>();

            foreach (var error in results.Errors)
            {
                final.Add(new ValidationResult(error.ErrorMessage));
            }
            
            return final;
        }

        public async Task<List<ValidationResult>> ValidateSevisAsync(EcaContext context, int participantId)
            {
            var updateStudent = GetStudent(context, participantId);

            var validator = new SEVISBatchUpdateStudentValidator();
            var results = await validator.ValidateAsync(updateStudent);

            var final = new List<ValidationResult>();
            foreach (var error in results.Errors)
            {
                final.Add(new ValidationResult(error.ErrorMessage));
            }
            
            return final;
        }

        /// <summary>
        /// Get populated participant person sevis object
        /// </summary>
        /// <param name="context"></param>
        /// <param name="participantId"></param>
        /// <returns></returns>
        public SEVISBatchUpdateStudent GetStudent(EcaContext context, int participantId)
        {
            // Get student details
            var participant = ParticipantQueries.CreateGetParticipantDTOByIdQuery(context, participantId).FirstOrDefault();
            var personalGeneral = PersonQueries.CreateGetGeneralByIdQuery(context, (int)participant.PersonId).FirstOrDefault();
            var personalPII = PersonQueries.CreateGetPiiByIdQuery(context, (int)participant.PersonId).FirstOrDefault();
            var personalEmail = PersonQueries.CreateGetContactInfoByIdQuery(context, (int)participant.PersonId).Select(x => x.EmailAddresses).FirstOrDefault();
            var studentInfo = PersonQueries.CreateGetEducationsByPersonIdQuery(context, (int)participant.PersonId).FirstOrDefault();
            var visitorInfo = ParticipantStudentVisitorQueries.CreateGetParticipantStudentVisitorDTOByIdQuery(context, participantId).FirstOrDefault();
            var primaryAddress = personalPII.Addresses.Where(x => x.IsPrimary == true).FirstOrDefault();
            var ecaService = new EcaService(context);
            var citizenship = ecaService.GetLocationById(personalPII.CountriesOfCitizenship.FirstOrDefault().Id);

            var student = new Student();
            student.requestID = "1";
            student.userID = participant.PersonId.ToString();
            student.IssueReason = "I";
            // personal info
            student.personalInfo = new PersonalInfo
            {
                BirthCountryCode = personalPII.PlaceOfBirth.CountryIso,
                BirthDate = personalPII.DateOfBirth.Value.Date,
                CitizenshipCountryCode = citizenship.LocationIso,
                Email = personalEmail.Select(x => x.Address).FirstOrDefault(),
                fullName = new FullName
                {
                    FirsName = personalPII.FirstName,
                    LastName = personalPII.LastName,
                    NameSuffix = personalPII.NameSuffix,
                    PreferredName = personalPII.Alias
                },
                Gender = personalPII.GenderId.ToString(),
                VisaType = "H1"
            };
            // us address
            student.usAddress = new USAddress
            {
                address1 = primaryAddress.Street1,
                address2 = primaryAddress.Street2,
                city = primaryAddress.City,
                postalCode = primaryAddress.PostalCode
            };
            // foreign address
            //if (isNew) { }
            student.foreignAddress = new ForeignAddress
            {
                address1 = primaryAddress.Street1,
                address2 = primaryAddress.Street2,
                city = primaryAddress.City,
                postalCode = primaryAddress.PostalCode,
                countryCode = primaryAddress.Country
            };
            if (studentInfo != null && visitorInfo != null)
            {
                // education
                student.educationalInfo = new EducationalInfo
                {
                    PrgStartDate = studentInfo.StartDate.DateTime,
                    PrgEndDate = studentInfo.EndDate.Value.DateTime,
                    eduLevel = new EduLevel
                    {
                        Level = visitorInfo.EducationLevel,
                        OtherRemarks = visitorInfo.EducationLevelOtherRemarks
                    },
                    engProficiency = new EngProficiency
                    {
                        EngRequired = visitorInfo.IsEnglishLanguageProficiencyReqd,
                        RequirementsMet = visitorInfo.IsEnglishLanguageProficiencyMet,
                        NotRequiredReason = visitorInfo.EnglishLanguageProficiencyNotReqdReason
                    },
                    LengthOfStudy = visitorInfo.LengthOfStudy.ToString(),
                    PrimaryMajor = visitorInfo.PrimaryMajor,
                    SecondMajor = visitorInfo.SecondaryMajor,
                    Minor = visitorInfo.Minor,
                    Remarks = studentInfo.Title
                };
                // financial
                student.financialInfo = new FinancialInfo
                {
                    AcademicTerm = "",
                    Expense = new Expense
                    {
                        DependentExp = (int)visitorInfo.DependentExpense,
                        LivingExpense = (int)visitorInfo.LivingExpense,
                        Tuition = (int)visitorInfo.TuitionExpense,
                        Other = new ExpenseOther
                        {
                            Amount = (int)visitorInfo.OtherExpense,
                            Remarks = visitorInfo.ExpenseRemarks
                        }
                    },
                    Funding = new Funding
                    {
                        Employment = (int)visitorInfo.EmploymentFunding,
                        Personal = (int)visitorInfo.PersonalFunding,
                        School = new School
                        {
                            Amount = (int)visitorInfo.SchoolFunding,
                            Remarks = visitorInfo.SchoolFundingRemarks
                        },
                        Other = new FundingOther
                        {
                            Amount = (int)visitorInfo.OtherFunding,
                            Remarks = visitorInfo.OtherFundingRemarks
                        }
                    }
                };
            }
            student.createDependent = null;
            student.Remarks = "";
            //student.createDependent = new CreateDependent
            //{
            //    Dependent = new AddDependent {

            //    },
            //    Remarks = ""
            //};

            var batchHeader = new BatchHeader
            {
                BatchID = "1",
                OrgID = "1"
            };
            var createStudent = new CreateStudent
            {
                student = student
            };
            var updateStudent = new SEVISBatchUpdateStudent
            {
                userID = "1",
                batchHeader = batchHeader,
                createStudent = createStudent
            };

            return updateStudent;
        }
        
        public override List<ValidationResult> DoValidateSevis(SEVISBatchUpdateStudent validationEntity)
        {
            throw new NotImplementedException();
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
