﻿using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class DeleteTippSiteValidator : AbstractValidator<DeleteTippSite>
    {
        public const int ID_MAX_LENGTH = 50;

        public DeleteTippSiteValidator()
        {
            RuleFor(tipp => tipp.SiteId).NotNull().WithMessage("Delete T/IPP Site: T/IPP Site ID is required").Length(1, ID_MAX_LENGTH).WithMessage("Delete T/IPP Site: T/IPP ID can be up to " + ID_MAX_LENGTH.ToString() + " characters"); ;
        }
    }
}