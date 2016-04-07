using ECA.Business.Validation.Sevis.ErrorPaths;
using ECA.Business.Validation.Sevis.Exceptions;
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
        public const string DEPENDENT_RELATIONSHIP_REQUIRED = "The {0}, {1}, must have the relationship specified.";

        /// <summary>
        /// The error message to format when a dependent is to old to be a dependent.
        /// </summary>
        public const string DEPENDENT_IS_TO_OLD_ERROR_MESSAGE = "The {0}, {1}, must be {2} years of age or younger.";

        /// <summary>
        /// The error message to format when a permanent residence country is not specified via a home address.
        /// </summary>
        public static string PERMANENT_RESIDENCE_COUNTRY_CODE_ERROR_MESSAGE = "The Permanent Residence Country is required for the {0}, {1}.";

        /// <summary>
        /// The error message to format when a permanent residence country is not supported by sevis.
        /// </summary>
        public static string PERMANENT_RESIDENCE_COUNTRY_NOT_SUPPORTED = "The Permanent Residence Country Code '{0}' is not supported for the {1}, {2}.  Please set a different Permanent Residence Country.";

        /// <summary>
        /// The dependent person type.
        /// </summary>
        public const string PERSON_TYPE = "dependent";

        /// <summary>
        /// The maximum age of a dependent.
        /// </summary>
        public const int MAX_DEPENDENT_AGE = 21;

        /// <summary>
        /// Creates a new default instance.
        /// </summary>
        public DependentValidator()
            : base()
        {
            RuleFor(x => x.Relationship)
                .NotNull()
                .WithMessage(DEPENDENT_RELATIONSHIP_REQUIRED, GetPersonTypeDelegate(), GetNameDelegate())
                .WithState(x => new DependentErrorPath(x.PersonId));

            RuleFor(visitor => visitor.BirthCountryReasonCode)
                .Length(0, BIRTH_COUNTRY_REASON_LENGTH)
                .WithMessage(BIRTH_COUNTRY_REASON_ERROR_MESSAGE, GetPersonTypeDelegate(), GetNameDelegate(), (d) => BIRTH_COUNTRY_REASON_LENGTH)
                .WithState(x => new DependentErrorPath(x.PersonId));

            When(x => x.BirthDate.HasValue && x.IsChildDependent(), () =>
            {
                RuleFor(x => x).Must(d =>
                {
                    var age = d.GetAge();
                    return age != -1 && age < MAX_DEPENDENT_AGE;
                })
                .WithMessage(DEPENDENT_IS_TO_OLD_ERROR_MESSAGE, GetPersonTypeDelegate(), GetNameDelegate(), (d) => MAX_DEPENDENT_AGE)
                .WithState(x => new DependentErrorPath(x.PersonId));
            });

            RuleFor(visitor => visitor.PermanentResidenceCountryCode)
                .NotNull()
                .WithMessage(PERMANENT_RESIDENCE_COUNTRY_CODE_ERROR_MESSAGE, GetPersonTypeDelegate(), GetNameDelegate())
                .WithState(x => GetPermanentResidenceCountryCodeErrorPath(x));
                
            When(x => x.PermanentResidenceCountryCode != null, () =>
            {
                RuleFor(x => x.PermanentResidenceCountryCode)
                .Must((code) =>
                {
                    try
                    {
                        var codeType = code.GetCountryCodeWithType();
                        return true;
                    }
                    catch (CodeTypeConversionException)
                    {
                        return false;
                    }
                })
                .WithMessage(PERMANENT_RESIDENCE_COUNTRY_NOT_SUPPORTED, (p) => p.PermanentResidenceCountryCode, GetPersonTypeDelegate(), GetNameDelegate())
                .WithState(x => GetPermanentResidenceCountryCodeErrorPath(x));
            });
        }

        /// <summary>
        /// Returns a delegate capable of creating a full name string of the dependent.
        /// </summary>
        /// <returns>A delegate capable of creating a full name string of the dependent.</returns>
        public override Func<Dependent, object> GetNameDelegate()
        {
            return (p) =>
            {
                return p.FullName != null ? String.Format("{0} {1}", p.FullName.FirstName, p.FullName.LastName).Trim() : String.Empty;
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

        /// <summary>
        /// Returns the dependent birth date error path.
        /// </summary>
        /// <param name="instance">The dependent.</param>
        /// <returns>The dependent birth date error path.</returns>
        public override ErrorPath GetBirthDateErrorPath(Dependent instance)
        {
            return GetDependentErrorPath(instance);
        }

        /// <summary>
        /// Returns the dependent gender error path.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>The error path.</returns>
        public override ErrorPath GetGenderErrorPath(Dependent instance)
        {
            return GetDependentErrorPath(instance);
        }

        /// <summary>
        /// Returns the dependent birth city error path.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>The error path.</returns>
        public override ErrorPath GetBirthCityErrorPath(Dependent instance)
        {
            return GetDependentErrorPath(instance);
        }

        /// <summary>
        /// Returns the dependent birth country code error path.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>The error path.</returns>
        public override ErrorPath GetBirthCountryCodeErrorPath(Dependent instance)
        {
            return GetDependentErrorPath(instance);
        }

        /// <summary>
        /// Returns the dependent citizneship country code error path.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>The error path.</returns>
        public override ErrorPath GetCitizenshipCountryCodeErrorPath(Dependent instance)
        {
            return GetDependentErrorPath(instance);
        }

        /// <summary>
        /// Returns the dependent permanent residence country code error path.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>The error path.</returns>
        public override ErrorPath GetPermanentResidenceCountryCodeErrorPath(Dependent instance)
        {
            return GetDependentErrorPath(instance);
        }

        /// <summary>
        /// Returns the dependent email address error path.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>The error path.</returns>
        public override ErrorPath GetEmailAddressErrorPath(Dependent instance)
        {
            return GetDependentErrorPath(instance);
        }

        /// <summary>
        /// Returns the dependent phone number error path.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>The error path.</returns>
        public override ErrorPath GetPhoneNumberErrorPath(Dependent instance)
        {
            return GetDependentErrorPath(instance);
        }

        /// <summary>
        /// Returns the dependent error path.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>The error path.</returns>
        public DependentErrorPath GetDependentErrorPath(Dependent instance)
        {
            return new DependentErrorPath(instance.PersonId);
        }
    }
}
