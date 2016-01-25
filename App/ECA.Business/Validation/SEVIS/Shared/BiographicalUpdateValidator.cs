﻿using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class BiographicalUpdateValidator : AbstractValidator<BiographicalUpdate>
    {
        public const int PHONE_MAX_LENGTH = 10;
        public const int POSITION_MAX_LENGTH = 25;
        public const int REMARKS_MAX_LENGTH = 500;

        public BiographicalUpdateValidator()
        {
            RuleFor(bio => bio.PhoneNumber).Length(0, PHONE_MAX_LENGTH).WithMessage("EV Biographical Info: Phone number can be up to " + PHONE_MAX_LENGTH.ToString() + " characters");
            RuleFor(bio => bio.PositionCode).Length(0, POSITION_MAX_LENGTH).WithMessage("EV Biographical Info: Position can be up to " + POSITION_MAX_LENGTH.ToString() + " characters");
            RuleFor(bio => bio.Remarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("EV Biographical Info: Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
        }
    }
}