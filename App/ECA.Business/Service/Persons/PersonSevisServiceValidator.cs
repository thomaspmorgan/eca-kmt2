using ECA.Business.Validation;
using ECA.Core.Service;
using ECA.Data;
using System.Diagnostics.Contracts;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ECA.Business.Service.Persons
{
    public class PersonSevisServiceValidator : DbContextService<EcaContext>, IPersonSevisServiceValidator
    {
        private IParticipantPersonsSevisService participantService;

        public PersonSevisServiceValidator(EcaContext context, IParticipantPersonsSevisService participantService) : base(context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            this.participantService = participantService;
        }

        /// <summary>
        /// Do validation for sevis exchange visitor create
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>Sevis object validation results</returns>        
        public FluentValidation.Results.ValidationResult ValidateSevisCreateEV(int participantId)
        {
            var createEV = participantService.GetCreateExchangeVisitor(participantId);

            var validator = new SEVISBatchCreateUpdateEVValidator();
            var results = validator.Validate(createEV);

            return results;
        }
        
        /// <summary>
        /// Do validation for sevis exchange visitor create
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>Sevis object validation results</returns>
        public async Task<FluentValidation.Results.ValidationResult> ValidateSevisCreateEVAsync(int participantId)
        {
            var createEV = participantService.GetCreateExchangeVisitor(participantId);

            var validator = new SEVISBatchCreateUpdateEVValidator();
            var results = await validator.ValidateAsync(createEV);

            //GetStudentUpdateXml(createEV);

            return results;
        }
        
        /// <summary>
        /// Do validation for sevis exchange visitor update
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>Sevis object validation results</returns>        
        public FluentValidation.Results.ValidationResult ValidateSevisUpdateEV(int participantId)
        {
            var updateEV = participantService.GetUpdateExchangeVisitor(participantId);

            var validator = new SEVISBatchCreateUpdateEVValidator();
            var results = validator.Validate(updateEV);
            
            return results;
        }

        /// <summary>
        /// Do validation for sevis exchange visitor update
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>Sevis object validation results</returns>
        public async Task<FluentValidation.Results.ValidationResult> ValidateSevisUpdateEVAsync(int participantId)
        {
            var updateEV = participantService.GetUpdateExchangeVisitor(participantId);

            var validator = new SEVISBatchCreateUpdateEVValidator();
            var results = await validator.ValidateAsync(updateEV);
            
            //var final = new List<ValidationResult>();
            //foreach (var error in results.Errors)
            //{
            //    final.Add(new ValidationResult(error.ErrorMessage));
            //}
            // update the participant sevis status
            //participantService.UpdateParticipantPersonSevisCommStatus(participantId, final.Count, results.IsValid);

            // temporary to test xml serialization
            //GetStudentUpdateXml(updateVisitor);

            return results;
        }
        
        private void GetStudentUpdateXml(SEVISBatchCreateUpdateEV validationEntity)
            {
            //XmlSerializer serializer = new XmlSerializer(validationEntity.GetType());
            //var settings = new XmlWriterSettings
            //{
            //    NewLineHandling = NewLineHandling.Entitize,
            //    Encoding = System.Text.Encoding.UTF8,
            //    DoNotEscapeUriAttributes = true
            //};
            //using (var stream = new StringWriter())
            //using (var writer = XmlWriter.Create(stream, settings))
            //{
            //    serializer.Serialize(writer, validationEntity);
            //    return stream.ToString();
            //}

            // write file
            XmlSerializer writer = new XmlSerializer(validationEntity.GetType());
            var path = @"C:\temp\SevisBatch.xml";
            //Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//SevisBatch.xml";
            //FileStream file = File.Create(path);
            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                file.Delete();
            }
            XmlWriter xfile = XmlWriter.Create(path);
            writer.Serialize(xfile, validationEntity);
            xfile.Close();
        }
        
        // TODO: for sending XML content to Sevis service
        //var xsdPath = System.AppDomain.CurrentDomain.BaseDirectory;
        //DataSet MyDataSet = new DataSet();
        //MyDataSet.ReadXmlSchema(@"schema.xsd");
        //string entityXml = SerializeToXmlString(validationEntity);
        //MyDataSet.ReadXml(entityXml);
    }
}
