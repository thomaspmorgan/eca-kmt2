using ECA.Business.Service;
using ECA.Business.Service.Persons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Person
{
    public class PersonBindingModel
    {
        [Required]
        public int ProjectId { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public int Gender { get; set; }
        [Required]
        public DateTimeOffset DateOfBirth { get; set; }
        [Required]
        public int CityOfBirth { get; set; }
        [Required]
        public List<int> CountriesOfCitizenship { get; set; }

        public NewPerson ToNewPerson(int userId)
        {
            return new NewPerson(new User(userId), this.ProjectId, this.FirstName, this.LastName, this.Gender, this.DateOfBirth,
                                 this.CityOfBirth, this.CountriesOfCitizenship);
        }
    }
}