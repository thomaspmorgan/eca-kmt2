using System.Threading.Tasks;
using ECA.Business.Validation;

namespace ECA.Business.Service.Persons
{
    public interface IPersonSevisServiceValidator
    {
        SEVISBatchCreateUpdateEV GetUpdateExchangeVisitor(int participantId);
        FluentValidation.Results.ValidationResult ValidateSevis(int participantId);
        Task<FluentValidation.Results.ValidationResult> ValidateSevisAsync(int participantId);
    }
}