using ECA.Business.Validation.Sevis.ErrorPaths;
using FluentValidation;
using System;

namespace ECA.Business.Validation.Sevis.Bio
{
    /// <summary>
    /// The DependentValidator is used to validation dependent abstract class instances.
    /// </summary>
    public class DependentValidator : BiographicalValidator<Dependent>
    {
        /// <summary>
        /// The error message to format when a relationship is required on a dependent.
        /// </summary>
        public const string DEPENDENT_RELATIONSHIP_REQUIRED = "The dependent named '{0} {1}' must have the relationship specified.";

        /// <summary>
        /// Creates a new default instance.
        /// </summary>
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

            RuleFor(visitor => visitor.BirthCountryReasonCode)
                .Length(0, BIRTH_COUNTRY_REASON_LENGTH)
                .WithMessage(BIRTH_COUNTRY_REASON_ERROR_MESSAGE)
                .WithState(x => new CountryOfBirthErrorPath());
        }
    }
}
