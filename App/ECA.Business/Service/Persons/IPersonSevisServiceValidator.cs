using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ECA.Business.Validation;
using ECA.Data;

namespace ECA.Business.Service.Persons
{
    public interface IPersonSevisServiceValidator
    {
        SEVISBatchCreateUpdateEV GetExchangeVisitor(EcaContext context, int participantId);
        List<ValidationResult> ValidateSevis(EcaContext context, int participantId);
        Task<List<ValidationResult>> ValidateSevisAsync(EcaContext context, int participantId);
    }
}