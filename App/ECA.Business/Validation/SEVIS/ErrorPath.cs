
using ECA.Core.Generation;
using System.Diagnostics.Contracts;

namespace ECA.Business.Validation.SEVIS
{

    public class SevisErrorType
    {
        public static StaticLookup Email
        {
            get
            {
                return new StaticLookup("Email", 1);
            }
        }

        public static StaticLookup PhoneNumber
        {
            get
            {
                return new StaticLookup("Phone Number", 2);
            }
        }

        public static StaticLookup Address
        {
            get
            {
                return new StaticLookup("Address", 3);
            }
        }

        public static StaticLookup StartDate
        {
            get
            {
                return new StaticLookup("Start Date", 4);
            }
        }

        public static StaticLookup EndDate
        {
            get
            {
                return new StaticLookup("End Date", 5);
            }
        }

        public static StaticLookup PositionCode
        {
            get
            {
                return new StaticLookup("Position", 6);
            }
        }

        public static StaticLookup ProgramCategoryCode
        {
            get
            {
                return new StaticLookup("Program Category", 7);
            }
        }

        public static StaticLookup FieldOfStudy
        {
            get
            {
                return new StaticLookup("Field Of Study", 8);
            }
        }

        public static StaticLookup Funding
        {
            get
            {
                return new StaticLookup("Funding", 9);
            }
        }

        public static StaticLookup FullName
        {
            get
            {
                return new StaticLookup("Full Name", 10);
            }
        }

        public static StaticLookup BirthDate
        {
            get
            {
                return new StaticLookup("Birth Date", 11);
            }
        }

        public static StaticLookup Gender
        {
            get
            {
                return new StaticLookup("Gender", 12);
            }
        }

        public static StaticLookup CityOfBirth
        {
            get
            {
                return new StaticLookup("City of Birth", 13);
            }
        }

        public static StaticLookup CountryOfBirth
        {
            get
            {
                return new StaticLookup("Country of Birth", 14);
            }
        }

        public static StaticLookup Citizenship
        {
            get
            {
                return new StaticLookup("Citizenship", 15);
            }
        }

        public static StaticLookup PermanentResidenceCountry
        {
            get
            {
                return new StaticLookup("Permanent Country of Residence", 16);
            }
        }
    }

    /// <summary>
    /// Routing path
    /// </summary>
    public class ErrorPath
    {
        public int SevisErrorTypeId { get; set; }

        public string SevisErrorTypeName { get; set; }

        public void SetByStaticLookup(StaticLookup lookup)
        {
            Contract.Requires(lookup != null, "The lookup must not be null.");
            this.SevisErrorTypeId = lookup.Id;
            this.SevisErrorTypeName = lookup.Value;
        }
    }
    
}
