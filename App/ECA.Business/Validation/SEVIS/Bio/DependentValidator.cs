using ECA.Business.Validation.Sevis.ErrorPaths;
using FluentValidation;
using System;

namespace ECA.Business.Validation.Sevis.Bio
{
    public class DependentValidator : BiographicalValidator<Dependent>
    {
        public const string DEPENDENT_RELATIONSHIP_REQUIRED = "Biographical:  The dependent named {0} {1} must have the relationship specified.";

        public DependentValidator()
            : base()
        {
            Func<Dependent, string> firstNameDelegate = (d) =>
            {
                return d.FullName != null && d.FullName.FirstName != null ? d.FullName.FirstName : String.Empty;
            };
            Func<Dependent, string> lastNameDelegate = (d) =>
            {
                return d.FullName != null && d.FullName.LastName != null ? d.FullName.LastName : String.Empty;
            };

            RuleFor(x => x.Relationship)
                .NotNull()
                .WithMessage(DEPENDENT_RELATIONSHIP_REQUIRED, firstNameDelegate, lastNameDelegate)
                .WithState(x => new DependentErrorPath());
        }
    }
}
