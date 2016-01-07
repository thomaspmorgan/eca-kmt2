using ECA.Business.Validation.Model;
using ECA.Core.Service;
using ECA.Data;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

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
        public FluentValidation.Results.ValidationResult ValidateSevisCreateEV(int participantId, User user)
        {
            var createEV = participantService.GetCreateExchangeVisitor(participantId, user);

            var validator = new CreateExchVisitorValidator();
            var results = validator.Validate(createEV);

            return results;
        }
        
        /// <summary>
        /// Do validation for sevis exchange visitor create
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>Sevis object validation results</returns>
        public async Task<FluentValidation.Results.ValidationResult> ValidateSevisCreateEVAsync(int participantId, User user)
        {
            var createEV = participantService.GetCreateExchangeVisitor(participantId, user);

            var validator = new CreateExchVisitorValidator();
            var results = await validator.ValidateAsync(createEV);

            return results;
        }
        
        /// <summary>
        /// Do validation for sevis exchange visitor update
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>Sevis object validation results</returns>        
        public FluentValidation.Results.ValidationResult ValidateSevisUpdateEV(int participantId, User user)
        {
            var updateEV = participantService.GetUpdateExchangeVisitor(participantId, user);

            var validator = new UpdateExchVisitorValidator();
            var results = validator.Validate(updateEV);
            
            return results;
        }

        /// <summary>
        /// Do validation for sevis exchange visitor update
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>Sevis object validation results</returns>
        public async Task<FluentValidation.Results.ValidationResult> ValidateSevisUpdateEVAsync(int participantId, User user)
        {
            var updateEV = participantService.GetUpdateExchangeVisitor(participantId, user);

            var validator = new UpdateExchVisitorValidator();
            var results = await validator.ValidateAsync(updateEV);
            
            return results;
        }
        

        #region Temp
        
        //// write to a local file
        //XmlSerializer writer = new XmlSerializer(validationEntity.GetType());
        //var path = @"C:\temp\SevisBatch.xml";
        ////Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//SevisBatch.xml";
        ////FileStream file = File.Create(path);
        //FileInfo file = new FileInfo(path);
        //if (file.Exists)
            //{
        //    file.Delete();
            //}
        //XmlWriter xfile = XmlWriter.Create(path);
        //writer.Serialize(xfile, validationEntity);
        //xfile.Close();

        #endregion
        
    }
}
