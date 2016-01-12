using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class BatchHeaderValidator : AbstractValidator<BatchHeader>
    {
        public const int ID_MAX_LENGTH = 14;
        public const int ORGID_MAX_LENGTH = 15;
        
        public BatchHeaderValidator()
        {
            RuleFor(header => header.BatchID).Length(1, ID_MAX_LENGTH).WithMessage("Batch header: Batch ID is required and can be up to " + ID_MAX_LENGTH.ToString() + " characters");
            RuleFor(header => header.OrgID).Length(1, ORGID_MAX_LENGTH).WithMessage("Batch header: Org ID is required and can be up to " + ORGID_MAX_LENGTH.ToString() + " characters");
        }
    }
}
