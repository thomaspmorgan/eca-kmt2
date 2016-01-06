using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    public interface IPersonSevisServiceValidator
    {
        FluentValidation.Results.ValidationResult ValidateSevisCreateEV(int participantId);
        Task<FluentValidation.Results.ValidationResult> ValidateSevisCreateEVAsync(int participantId);
        FluentValidation.Results.ValidationResult ValidateSevisUpdateEV(int participantId);
        Task<FluentValidation.Results.ValidationResult> ValidateSevisUpdateEVAsync(int participantId);
    }
}