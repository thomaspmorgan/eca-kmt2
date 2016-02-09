using ECA.Business.Validation.SEVIS;
using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class SubjectFieldValidator : AbstractValidator<SubjectField>
    {
        public const int FIELD_CODE_MAX_LENGTH = 7;
        public const int FOREIGN_FIELD_MAX_LENGTH = 100;
        public const int REMARKS_MAX_LENGTH = 500;

        public SubjectFieldValidator()
        {
            RuleFor(visitor => visitor.SubjectFieldCode).NotNull().WithMessage("Subject Field: Subject or field of study is required").Length(1, FIELD_CODE_MAX_LENGTH).WithMessage("Subject Field: Subject or field of study can be up to " + FIELD_CODE_MAX_LENGTH.ToString() + " characters").WithState(x => new ErrorPath { Category = ElementCategory.Project.ToString(), CategorySub = ElementCategorySub.Participant.ToString(), Tab = ElementCategorySectionTab.ExchVisitor.ToString() });
            RuleFor(visitor => visitor.ForeignDegreeLevel).Length(0, FOREIGN_FIELD_MAX_LENGTH).WithMessage("Subject Field: Foreign Degree Level can be up to " + FOREIGN_FIELD_MAX_LENGTH.ToString() + " characters").WithState(x => new ErrorPath { Category = ElementCategory.Project.ToString(), CategorySub = ElementCategorySub.Participant.ToString(), Tab = ElementCategorySectionTab.ExchVisitor.ToString() });
            RuleFor(visitor => visitor.ForeignFieldOfStudy).Length(0, FOREIGN_FIELD_MAX_LENGTH).WithMessage("Subject Field: Foreign Field of Study can be up to " + FOREIGN_FIELD_MAX_LENGTH.ToString() + " characters").WithState(x => new ErrorPath { Category = ElementCategory.Project.ToString(), CategorySub = ElementCategorySub.Participant.ToString(), Tab = ElementCategorySectionTab.ExchVisitor.ToString() });
            RuleFor(visitor => visitor.Remarks).NotNull().WithMessage("Subject Field: Remarks are required").Length(1, REMARKS_MAX_LENGTH).WithMessage("Subject Field: Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters").WithState(x => new ErrorPath { Category = ElementCategory.Project.ToString(), CategorySub = ElementCategorySub.Participant.ToString(), Tab = ElementCategorySectionTab.ExchVisitor.ToString() });
        }
    }
}