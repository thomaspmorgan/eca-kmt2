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
        public const string DEPENDENT_RELATIONSHIP_REQUIRED = "The {0}, {1} must have the relationship specified.";

        /// <summary>
        /// The dependent person type.
        /// </summary>
        public const string PERSON_TYPE = "Dependent";

        /// <summary>
        /// Creates a new default instance.
        /// </summary>
        public DependentValidator()
            : base()
        {
            RuleFor(x => x.Relationship)
                .NotNull()
                .WithMessage(DEPENDENT_RELATIONSHIP_REQUIRED, GetPersonTypeDelegate(), GetNameDelegate())
                .WithState(x => new DependentErrorPath());

            RuleFor(visitor => visitor.BirthCountryReasonCode)
                .Length(0, BIRTH_COUNTRY_REASON_LENGTH)
                .WithMessage(BIRTH_COUNTRY_REASON_ERROR_MESSAGE, GetPersonTypeDelegate(), GetNameDelegate(), (d) => BIRTH_COUNTRY_REASON_LENGTH)
                .WithState(x => new CountryOfBirthErrorPath());
        }

        /// <summary>
        /// Returns a delegate capable of creating a full name string of the dependent.
        /// </summary>
        /// <returns>A delegate capable of creating a full name string of the dependent.</returns>
        public override Func<Dependent, object> GetNameDelegate()
        {
            return (p) =>
            {
                return p.FullName != null ? String.Format("{0} {1}", p.FullName.FirstName, p.FullName.LastName) : String.Empty;
            };
        }

        /// <summary>
        /// Returns the person type.
        /// </summary>
        /// <param name="instance">The dependent.</param>
        /// <returns>The person type.</returns>
        public override string GetPersonType(Dependent instance)
        {
            return PERSON_TYPE;
        }
    }
}
