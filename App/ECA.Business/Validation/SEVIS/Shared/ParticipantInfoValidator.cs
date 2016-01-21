﻿using ECA.Business.Validation.Model.CreateEV;
using ECA.Business.Validation.SEVIS;
using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class ParticipantInfoValidator : AbstractValidator<ParticipantInfo>
    {
        public const int EMAIL_MAX_LENGTH = 255;
        public const int FOS_MAX_LENGTH = 100;
        public const int DEGREE_MAX_LENGTH = 100;
        
        public ParticipantInfoValidator()
        {
            RuleFor(visitor => visitor.EmailAddress).Length(1, EMAIL_MAX_LENGTH).WithMessage("Participant Info: Email is required and can be up to " + EMAIL_MAX_LENGTH.ToString() + " characters").WithMessage("Participant Info: Email is invalid").WithState(x => new ErrorPath { Category = ElementCategory.Person.ToString(), CategorySub = ElementCategorySub.PersonalInfo.ToString(), Section = ElementCategorySection.PII.ToString(), Tab = ElementCategorySectionTab.PersonalInfo.ToString() }).EmailAddress().WithMessage("Participant Info: Email is invalid");
            RuleFor(visitor => visitor.FieldOfStudy).Length(1, FOS_MAX_LENGTH).WithMessage("Participant Info: Field of Study is required and can be up to " + FOS_MAX_LENGTH.ToString() + " characters").WithState(x => new ErrorPath { Category = ElementCategory.Project.ToString(), CategorySub = ElementCategorySub.Participant.ToString(), Tab = ElementCategorySectionTab.ExchVisitor.ToString() });
            RuleFor(visitor => visitor.TypeOfDegree).Length(1, DEGREE_MAX_LENGTH).WithMessage("Participant Info: Type of Degree is required and can be up to " + DEGREE_MAX_LENGTH.ToString() + " characters").WithState(x => new ErrorPath { Category = ElementCategory.Project.ToString(), CategorySub = ElementCategorySub.Participant.ToString(), Tab = ElementCategorySectionTab.ExchVisitor.ToString() });
            RuleFor(visitor => visitor.DateAwardedOrExpected).NotNull().WithMessage("Participant Info: Date awarded or expected is required").WithState(x => new ErrorPath { Category = ElementCategory.Project.ToString(), CategorySub = ElementCategorySub.Participant.ToString(), Tab = ElementCategorySectionTab.ExchVisitor.ToString() });
        }
    }
}