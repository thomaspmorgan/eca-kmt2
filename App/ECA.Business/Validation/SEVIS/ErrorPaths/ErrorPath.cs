
using ECA.Core.Generation;
using System.Diagnostics.Contracts;

namespace ECA.Business.Validation.SEVIS.ErrorPaths
{
    /// <summary>
    /// The SevisErrorType class contains lookups for different sevis validation errro types.
    /// </summary>
    public sealed class SevisErrorType
    {
        /// <summary>
        /// Gets the email error type.
        /// </summary>
        public static StaticLookup Email
        {
            get
            {
                return new StaticLookup("Email", 1);
            }
        }

        /// <summary>
        /// Gets the phone number error type.
        /// </summary>
        public static StaticLookup PhoneNumber
        {
            get
            {
                return new StaticLookup("Phone Number", 2);
            }
        }

        /// <summary>
        /// Gets the address error type.
        /// </summary>
        public static StaticLookup Address
        {
            get
            {
                return new StaticLookup("Address", 3);
            }
        }

        /// <summary>
        /// Gets the start date error type.
        /// </summary>
        public static StaticLookup StartDate
        {
            get
            {
                return new StaticLookup("Start Date", 4);
            }
        }

        /// <summary>
        /// Gets the end date error type.
        /// </summary>
        public static StaticLookup EndDate
        {
            get
            {
                return new StaticLookup("End Date", 5);
            }
        }

        /// <summary>
        /// Gets the position code error type.
        /// </summary>
        public static StaticLookup PositionCode
        {
            get
            {
                return new StaticLookup("Position", 6);
            }
        }

        /// <summary>
        /// Gets the program category code error type.
        /// </summary>
        public static StaticLookup ProgramCategoryCode
        {
            get
            {
                return new StaticLookup("Program Category", 7);
            }
        }

        /// <summary>
        /// Gets the field of study error type.
        /// </summary>
        public static StaticLookup FieldOfStudy
        {
            get
            {
                return new StaticLookup("Field Of Study", 8);
            }
        }

        /// <summary>
        /// Gets the sevis funding error type.
        /// </summary>
        public static StaticLookup Funding
        {
            get
            {
                return new StaticLookup("Funding", 9);
            }
        }

        /// <summary>
        /// Gets the full name error type.
        /// </summary>
        public static StaticLookup FullName
        {
            get
            {
                return new StaticLookup("Full Name", 10);
            }
        }

        /// <summary>
        /// Gets the birth date error type.
        /// </summary>
        public static StaticLookup BirthDate
        {
            get
            {
                return new StaticLookup("Birth Date", 11);
            }
        }

        /// <summary>
        /// Gets the gender error type.
        /// </summary>
        public static StaticLookup Gender
        {
            get
            {
                return new StaticLookup("Gender", 12);
            }
        }

        /// <summary>
        /// Gets the city of birth error type.
        /// </summary>
        public static StaticLookup CityOfBirth
        {
            get
            {
                return new StaticLookup("City of Birth", 13);
            }
        }

        /// <summary>
        /// Gets the country of birth error type.
        /// </summary>
        public static StaticLookup CountryOfBirth
        {
            get
            {
                return new StaticLookup("Country of Birth", 14);
            }
        }

        /// <summary>
        /// Gets the countries of citizenship error type.
        /// </summary>
        public static StaticLookup Citizenship
        {
            get
            {
                return new StaticLookup("Citizenship", 15);
            }
        }

        /// <summary>
        /// Gets the permanent residence country error type.
        /// </summary>
        public static StaticLookup PermanentResidenceCountry
        {
            get
            {
                return new StaticLookup("Permanent Country of Residence", 16);
            }
        }
    }

    /// <summary>
    /// An ErrorPath class is used to detail a sevis validation error and where it might be located.
    /// </summary>
    public class ErrorPath
    {
        /// <summary>
        /// Gets or sets the sevis error type id.
        /// </summary>
        public int SevisErrorTypeId { get; set; }

        /// <summary>
        /// Gets or sets the sevis error type name.
        /// </summary>
        public string SevisErrorTypeName { get; set; }

        /// <summary>
        /// Initializes this error path with the given values from the lookup.
        /// </summary>
        /// <param name="lookup">The static lookup.</param>
        public void SetByStaticLookup(StaticLookup lookup)
        {
            Contract.Requires(lookup != null, "The lookup must not be null.");
            this.SevisErrorTypeId = lookup.Id;
            this.SevisErrorTypeName = lookup.Value;
        }
    }
}
