using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Data;
using ECA.Business.Queries.Persons;

namespace ECA.Business.Test.Queries.Persons
{
    [TestClass]
    public class PersonQueriesTest
    {
        private TestEcaContext context;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
        }

        [TestMethod]
        public void TestCreateGetSimplePersonDTOsQuery_CheckFullName_HasAllNameValuesSet()
        {
            var person = new Person
            {
                Alias = "alias",
                FamilyName = "family",
                FirstName = "firstName",
                GivenName = "givenName",
                LastName = "lastName",
                MiddleName = "middleName",
                NamePrefix = "Mr.",
                NameSuffix = "III",
                Patronym = "patronym",
                Gender = new Gender
                {
                    GenderId = Gender.Female.Id,
                    GenderName = Gender.Female.Value
                }
            };
            context.People.Add(person);
            context.Genders.Add(person.Gender);
            var expectedName = String.Format("{0} {1} {2} {3} {4} {5} ({6})",
                person.NamePrefix,
                person.FirstName,
                person.MiddleName,
                person.LastName,
                person.Patronym,
                person.NameSuffix,
                person.Alias);

            var result = PersonQueries.CreateGetSimplePersonDTOsQuery(context).First();
            Assert.AreEqual(expectedName, result.FullName);
        }

        [TestMethod]
        public void TestCreateGetSimplePersonDTOsQuery_CheckFullName_NoNamesSet()
        {
            var person = new Person
            {   
                Gender = new Gender
                {
                    GenderId = Gender.Female.Id,
                    GenderName = Gender.Female.Value
                }
            };
            context.People.Add(person);
            context.Genders.Add(person.Gender);
            var result = PersonQueries.CreateGetSimplePersonDTOsQuery(context).First();
            Assert.AreEqual(String.Empty, result.FullName);
        }

        [TestMethod]
        public void TestCreateGetSimplePersonDTOsQuery_CheckFullName_AllNamesEmpty()
        {
            var person = new Person
            {
                Alias = String.Empty,
                FamilyName = String.Empty,
                FirstName = String.Empty,
                GivenName = String.Empty,
                LastName = String.Empty,
                MiddleName = String.Empty,
                NamePrefix = String.Empty,
                NameSuffix = String.Empty,
                Patronym = String.Empty,
                Gender = new Gender
                {
                    GenderId = Gender.Female.Id,
                    GenderName = Gender.Female.Value
                }
            };
            context.People.Add(person);
            context.Genders.Add(person.Gender);
            var result = PersonQueries.CreateGetSimplePersonDTOsQuery(context).First();
            Assert.AreEqual(String.Empty, result.FullName);
        }

        [TestMethod]
        public void TestCreateGetSimplePersonDTOsQuery_CheckFullName_AliasOnly()
        {
            var person = new Person
            {
                Alias = "alias",
                Gender = new Gender
                {
                    GenderId = Gender.Female.Id,
                    GenderName = Gender.Female.Value
                }
            };
            context.People.Add(person);
            context.Genders.Add(person.Gender);
            var expectedName = String.Format("({0})", person.Alias);

            var result = PersonQueries.CreateGetSimplePersonDTOsQuery(context).First();
            Assert.AreEqual(expectedName, result.FullName);
        }

        [TestMethod]
        public void TestCreateGetSimplePersonDTOsQuery_CheckFullName_FirstNameOnly()
        {
            var person = new Person
            {
                FirstName = "first",
                Gender = new Gender
                {
                    GenderId = Gender.Female.Id,
                    GenderName = Gender.Female.Value
                }
            };
            context.People.Add(person);
            context.Genders.Add(person.Gender);
            var expectedName = String.Format("{0}", person.FirstName);

            var result = PersonQueries.CreateGetSimplePersonDTOsQuery(context).First();
            Assert.AreEqual(expectedName, result.FullName);
        }

        [TestMethod]
        public void TestCreateGetSimplePersonDTOsQuery_CheckFullName_MiddleNameOnly()
        {
            var person = new Person
            {
                MiddleName = "middle",
                Gender = new Gender
                {
                    GenderId = Gender.Female.Id,
                    GenderName = Gender.Female.Value
                }
            };
            context.People.Add(person);
            context.Genders.Add(person.Gender);
            var expectedName = String.Format("{0}", person.MiddleName);

            var result = PersonQueries.CreateGetSimplePersonDTOsQuery(context).First();
            Assert.AreEqual(expectedName, result.FullName);
        }

        [TestMethod]
        public void TestCreateGetSimplePersonDTOsQuery_CheckFullName_LastNameOnly()
        {
            var person = new Person
            {
                LastName = "last",
                Gender = new Gender
                {
                    GenderId = Gender.Female.Id,
                    GenderName = Gender.Female.Value
                }
            };
            context.People.Add(person);
            context.Genders.Add(person.Gender);
            var expectedName = String.Format("{0}", person.LastName);

            var result = PersonQueries.CreateGetSimplePersonDTOsQuery(context).First();
            Assert.AreEqual(expectedName, result.FullName);
        }

        [TestMethod]
        public void TestCreateGetSimplePersonDTOsQuery_CheckFullName_PatronymOnly()
        {
            var person = new Person
            {
                Patronym = "last",
                Gender = new Gender
                {
                    GenderId = Gender.Female.Id,
                    GenderName = Gender.Female.Value
                }
            };
            context.People.Add(person);
            context.Genders.Add(person.Gender);
            var expectedName = String.Format("{0}", person.Patronym);

            var result = PersonQueries.CreateGetSimplePersonDTOsQuery(context).First();
            Assert.AreEqual(expectedName, result.FullName);
        }

        [TestMethod]
        public void TestCreateGetSimplePersonDTOsQuery_CheckFullName_NameSuffixOnly()
        {
            var person = new Person
            {
                NameSuffix = "last",
                Gender = new Gender
                {
                    GenderId = Gender.Female.Id,
                    GenderName = Gender.Female.Value
                }
            };
            context.People.Add(person);
            context.Genders.Add(person.Gender);
            var expectedName = String.Format("{0}", person.NameSuffix);

            var result = PersonQueries.CreateGetSimplePersonDTOsQuery(context).First();
            Assert.AreEqual(expectedName, result.FullName);
        }
    }
}
