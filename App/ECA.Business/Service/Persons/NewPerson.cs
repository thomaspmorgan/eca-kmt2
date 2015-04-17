using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    public class NewPerson : IAuditable
    {
        public NewPerson(User createdBy, int projectId, string firstName, string lastName, int gender, DateTimeOffset dateOfBirth,
                         int cityOfBirth, List<int> countriesOfCitizenship)
        {
            Contract.Requires(createdBy != null, "The created by user must not be null.");

            this.ProjectId = projectId;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Gender = gender;
            this.DateOfBirth = dateOfBirth;
            this.CityOfBirth = cityOfBirth;
            this.CountriesOfCitizenship = countriesOfCitizenship;
            this.Audit = new Create(createdBy);
        }

        public int ProjectId { get; private set; }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public int Gender { get; private set; }
        public DateTimeOffset DateOfBirth { get; private set; }
        public int CityOfBirth { get; private set; }
        public List<int> CountriesOfCitizenship { get; private set; }

        public Audit Audit { get; private set; }
    }
}
