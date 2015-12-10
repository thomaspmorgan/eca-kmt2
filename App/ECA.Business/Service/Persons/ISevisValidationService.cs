using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    public interface ISevisValidationService
    {
        List<ValidationResult> PreSevisValidation(int participantId);
        Task<List<ValidationResult>> PreSevisValidationAsync(int participantId);
    }
}