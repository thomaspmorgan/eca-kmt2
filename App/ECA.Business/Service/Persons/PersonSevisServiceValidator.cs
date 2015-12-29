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
using ECA.Business.Validation.Model.Shared;
using ECA.Business.Validation.Model.CreateEV;
using System.Diagnostics.Contracts;
using ECA.Core.Service;
//using System.Data;
//using System.IO;
//using System.Xml.Serialization;

namespace ECA.Business.Service.Persons
{
    public class PersonSevisServiceValidator : DbContextService<EcaContext>, IPersonSevisServiceValidator
    {
        public PersonSevisServiceValidator(EcaContext context) : base(context)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        /// <summary>
        /// Do validation for sevis object, which includes participant person object
        /// </summary>
        /// <param name="participantId">Entity to validate</param>
        /// <returns>validation results</returns>        
        public List<ValidationResult> ValidateSevis(int participantId)
        {
            var updateStudent = GetUpdateExchangeVisitor(participantId);

            var validator = new SEVISBatchCreateUpdateEVValidator();
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
            var updateVisitor = GetUpdateExchangeVisitor(participantId);

            var validator = new SEVISBatchCreateUpdateEVValidator();
            var results = await validator.ValidateAsync(updateVisitor);

            var final = new List<ValidationResult>();
            foreach (var error in results.Errors)
            {
                final.Add(new ValidationResult(error.ErrorMessage));
            }

            // temporary to test xml serialization
            //string temp = GetStudentUpdateXml(updateVisitor);

            return final;
        }

        /// <summary>
        /// Get populated participant person sevis object
        /// </summary>
        /// <param name="context"></param>
        /// <param name="participantId"></param>
        /// <returns></returns>
        public SEVISBatchCreateUpdateEV GetUpdateExchangeVisitor(int participantId)
        {
            //Get student details
            var participant = ParticipantQueries.CreateGetParticipantDTOByIdQuery(this.Context, participantId).FirstOrDefault();
            var participantPerson = ParticipantPersonQueries.CreateGetParticipantPersonDTOByIdQuery(this.Context, participantId).FirstOrDefault();
            var personalPII = PersonQueries.CreateGetPiiByIdQuery(this.Context, (int)participant.PersonId).FirstOrDefault();
            var personalEmail = PersonQueries.CreateGetContactInfoByIdQuery(this.Context, (int)participant.PersonId).Select(x => x.EmailAddresses).FirstOrDefault();
            var mailingAddress = Context.Locations.Where(x => x.LocationId == participantPerson.HomeInstitutionAddressId).FirstOrDefault();
                //personalPII.Addresses.Where(x => x.AddressType == AddressType.Visiting.Value).FirstOrDefault();
            var physicalAddress = Context.Locations.Where(x => x.LocationId == participantPerson.HostInstitutionAddressId).FirstOrDefault();
            //personalPII.Addresses.Where(x => x.AddressType == AddressType.Host.Value).FirstOrDefault();
            var citizenship = Context.Locations.Where(x => x.LocationId == personalPII.CountriesOfCitizenship.FirstOrDefault().Id).FirstOrDefault();

            var ExchVisitor = new ExchangeVisitorUpdate();
            // biographical
            ExchVisitor.Biographical = new BiographicalUpdate
            {
                BirthCity = personalPII.PlaceOfBirth != null ? personalPII.PlaceOfBirth.City : "",
                BirthCountryCode = personalPII.PlaceOfBirth != null ? personalPII.PlaceOfBirth.CountryIso2 : "",
                BirthCountryReason = "",
                BirthDate = personalPII.DateOfBirth != null ? personalPII.DateOfBirth.Value.Date : (DateTime?)null,
                CitizenshipCountryCode = citizenship != null ? citizenship.LocationIso2 : "",
                EmailAddress = personalEmail != null ? personalEmail.Select(x => x.Address).FirstOrDefault() : "",
                FullName = new FullName
                {
                    FirsName = personalPII.FirstName,
                    LastName = personalPII.LastName,
                    Suffix = personalPII.NameSuffix,
                    PreferredName = personalPII.Alias
                },
                Gender = personalPII.GenderId.ToString(),
                PhoneNumber = "",
                PositionCode = "",
                printForm = false,
                Remarks = "Test remark"
            };
            if (mailingAddress != null)
            {
                ExchVisitor.Biographical.MailAddress = new USAddress
                {
                    Address1 = mailingAddress.Street1,
                    Address2 = mailingAddress.Street2,
                    City = mailingAddress.City.LocationName,
                    State = mailingAddress.Division.LocationName,
                    PostalCode = mailingAddress.PostalCode,
                    Explanation = "",
                    ExplanationCode = ""
                };
            }
            if (physicalAddress != null)
            {
                ExchVisitor.Biographical.PermanentResidenceCountryCode = physicalAddress.LocationIso2 != null ? physicalAddress.LocationIso2 : "";
                ExchVisitor.Biographical.USAddress = new USAddress
                {
                    Address1 = physicalAddress.Street1,
                    Address2 = physicalAddress.Street2,
                    City = physicalAddress.City.LocationName,
                    State = physicalAddress.Division.LocationName,
                    PostalCode = physicalAddress.PostalCode,
                    Explanation = "",
                    ExplanationCode = ""
                };
                ExchVisitor.Biographical.ResidentialAddress = new ResidentialAddress
                {
                    BoardingSchool = new BoardingSchool
                    {
                        Name = "",
                        Phone = ""
                    },
                    HostFamily = new HostFamily
                    {
                        PContact = new PContact
                        {
                            FirsName = "",
                            LastName = ""
                        },
                        SContact = new SContact
                        {
                            FirsName = "",
                            LastName = ""
                        },
                        Phone = ""
                    },
                    LCCoordinator = new LCCoordinator
                    {
                        FirsName = "",
                        LastName = ""
                    },
                    ResidentialType = ""
                };
            }
            // financial
            ExchVisitor.FinancialInfo = new FinancialInfoUpdate
            {
                printForm = false,
                ReceivedUSGovtFunds = false,
                ProgramSponsorFunds = "1200",
                OtherFunds = new OtherFunds
                {
                    International = new International
                    {
                        Amount1 = "10",
                        Amount2 = "20",
                        Org1 = "org 1",
                        OtherName1 = "on 1",
                        Org2 = "org 2",
                        OtherName2 = "on 2"
                    },
                    USGovt = new USGovt
                    {
                        Amount1 = "10",
                        Amount2 = "20",
                        Org1 = "govorg 1",
                        OtherName1 = "govon 1",
                        Org2 = "govorg 2",
                        OtherName2 = "govon 2"
                    },
                    Other = new Other
                    {
                        amount = "30",
                        name = "other nm"
                    }
                }
            };
            // program and subject
            ExchVisitor.Program = new Validation.Model.Program
            {
                EditSubject = new SubjectFieldUpdate
                {
                    printForm = false,
                    SubjectFieldCode = "SF code",
                    SubjectFieldRemarks = "SF rmks",
                    ForeignDegreeLevel = "FD lvl",
                    ForeignFieldOfStudy = "F FS",
                    Remarks = "Rmks"
                }
            };
            // Reprint
            ExchVisitor.Reprint = new ReprintFormUpdate
            {
                printForm = false,
                dependentSevisID = "1",
                Reason = "Reprint reason",
                Remarks = "Remarks",
                OtherRemarks = "Other remarks"
            };
            // Reprint 7002
            ExchVisitor.Reprint7002 = new Reprint7002
            {
                print7002 = false,
                SiteId = "1"
            };
            ExchVisitor.requestID = "1";
            ExchVisitor.sevisID = "1";
            ExchVisitor.SiteOfActivity = new SiteOfActivityUpdate
            {

            };
            ExchVisitor.Status = new StatusUpdate
            {                
            };
            ExchVisitor.statusCode = "A";
            ExchVisitor.TIPP = new TippUpdate
            {
            };
            ExchVisitor.UserDefinedA = "UD A";
            ExchVisitor.UserDefinedB = "UD B";
            ExchVisitor.Validate = new ValidateParticipant
            {

            };
            ExchVisitor.userID = participant.PersonId.ToString();
            
            // TODO: complete when dependent feature is available
            //student.createDependent = null;
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
            var updateVisitor = new UpdateExchVisitor
            {
                ExchangeVisitor = ExchVisitor
            };
            var updateVisitorBatch = new SEVISBatchCreateUpdateEV
            {
                userID = "1",
                BatchHeader = batchHeader,
                UpdateEV = updateVisitor
            };

            return updateVisitorBatch;
        }

        private string GetStudentUpdateXml(SEVISBatchCreateUpdateEV validationEntity)
        {
            XmlSerializer serializer = new XmlSerializer(validationEntity.GetType());
            var settings = new XmlWriterSettings
            {
                NewLineHandling = NewLineHandling.Entitize,
                Encoding = System.Text.Encoding.UTF8,
                DoNotEscapeUriAttributes = true
            };
            using (var stream = new StringWriter())
            using (var writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, validationEntity);
                return stream.ToString();
            }
        }
        

        // TODO: for sending XML content to Sevis service
        //var xsdPath = System.AppDomain.CurrentDomain.BaseDirectory;
        //DataSet MyDataSet = new DataSet();
        //MyDataSet.ReadXmlSchema(@"schema.xsd");
        //string entityXml = SerializeToXmlString(validationEntity);
        //MyDataSet.ReadXml(entityXml);
    }
}
