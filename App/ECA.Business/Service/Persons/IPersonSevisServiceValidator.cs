using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ECA.Business.Validation;

namespace ECA.Business.Service.Persons
{
    public interface IPersonSevisServiceValidator
    {
        SEVISBatchCreateUpdateEV GetUpdateExchangeVisitor(int participantId);
        List<ValidationResult> ValidateSevis(int participantId);
        Task<List<ValidationResult>> ValidateSevisAsync(int participantId);
    }
}