using System;
using FluentAssertions;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Admin;
using ECA.Data;
using ECA.Business.Queries.Models.Admin;
using System.Threading.Tasks;
using ECA.Business.Service;
using Microsoft.QualityTools.Testing.Fakes;
using ECA.Core.Exceptions;

namespace ECA.Business.Test.Service.Admin
{
    [TestClass]
    public class PhoneNumberServiceTest
    {
        private TestEcaContext context;
        private PhoneNumberService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new PhoneNumberService(context);
        }

        #region Get
        [TestMethod]
        public async Task TestGetById_CheckPersonProperties()
        {
            var number = "1234";

            var person = new Person
            {
                PersonId = 10,
                FullName = "full name"
            };
            var type = new PhoneNumberType
            {
                PhoneNumberTypeId = PhoneNumberType.Home.Id,
                PhoneNumberTypeName = PhoneNumberType.Home.Value
            };
            var phoneNumber = new PhoneNumber
            {
                Number = number,
                PhoneNumberTypeId = type.PhoneNumberTypeId,
                PhoneNumberType = type,
                IsPrimary = true,
                PersonId = person.PersonId,
                Person = person,
                PhoneNumberId = 10
            };
            context.PhoneNumberTypes.Add(type);
            context.People.Add(person);
            context.PhoneNumbers.Add(phoneNumber);

            Action<PhoneNumberDTO> tester = (dto) =>
            {
                Assert.AreEqual(phoneNumber.PhoneNumberId, dto.Id);
                Assert.IsFalse(dto.ContactId.HasValue);
                Assert.AreEqual(phoneNumber.IsPrimary, dto.IsPrimary);
                Assert.AreEqual(phoneNumber.Number, dto.Number);
                Assert.AreEqual(person.PersonId, dto.PersonId);
                Assert.AreEqual(type.PhoneNumberTypeName, dto.PhoneNumberType);
                Assert.AreEqual(type.PhoneNumberTypeId, dto.PhoneNumberTypeId);
            };

            var result = service.GetById(phoneNumber.PhoneNumberId);
            var resultAsync = await service.GetByIdAsync(phoneNumber.PhoneNumberId);
            tester(result);
            tester(resultAsync);
        }

        [TestMethod]
        public async Task TestGetById_CheckContactProperties()
        {
            var number = "1234";

            var contact = new Contact
            {
                ContactId = 10,
                FullName = "full name"
            };
            var type = new PhoneNumberType
            {
                PhoneNumberTypeId = PhoneNumberType.Home.Id,
                PhoneNumberTypeName = PhoneNumberType.Home.Value
            };
            var phoneNumber = new PhoneNumber
            {
                Number = number,
                PhoneNumberTypeId = type.PhoneNumberTypeId,
                PhoneNumberType = type,
                IsPrimary = true,
                ContactId = contact.ContactId,
                Contact = contact,
                PhoneNumberId = 10
            };
            context.PhoneNumberTypes.Add(type);
            context.Contacts.Add(contact);
            context.PhoneNumbers.Add(phoneNumber);

            Action<PhoneNumberDTO> tester = (dto) =>
            {
                Assert.AreEqual(phoneNumber.PhoneNumberId, dto.Id);
                Assert.IsFalse(dto.PersonId.HasValue);
                Assert.AreEqual(phoneNumber.IsPrimary, dto.IsPrimary);
                Assert.AreEqual(phoneNumber.Number, dto.Number);
                Assert.AreEqual(contact.ContactId, dto.ContactId);
                Assert.AreEqual(type.PhoneNumberTypeName, dto.PhoneNumberType);
                Assert.AreEqual(type.PhoneNumberTypeId, dto.PhoneNumberTypeId);
            };

            var result = service.GetById(phoneNumber.PhoneNumberId);
            var resultAsync = await service.GetByIdAsync(phoneNumber.PhoneNumberId);
            tester(result);
            tester(resultAsync);
        }

        [TestMethod]
        public async Task TestGetById_PhoneNumberDoesNotExist()
        {
            Action<PhoneNumberDTO> tester = (dto) =>
            {
                Assert.IsNull(dto);
            };

            var result = service.GetById(1);
            var resultAsync = await service.GetByIdAsync(1);
            tester(result);
            tester(resultAsync);
        }
        #endregion

        #region Create
        [TestMethod]
        public async Task TestCreate_CheckProperties()
        {
            using (ShimsContext.Create())
            {
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.SetOf1<Person>((c) =>
                {
                    return context.People;
                });
                var userId = 1;
                var personId = 10;
                Person person = null;
                var phoneNumberType = new PhoneNumberType
                {
                    PhoneNumberTypeId = PhoneNumberType.Home.Id,
                    PhoneNumberTypeName = PhoneNumberType.Home.Value
                };
                var user = new User(userId);
                var newPhone = new NewPersonPhoneNumber(user, phoneNumberType.PhoneNumberTypeId, "1234", personId, true);
                Action beforeTester = () =>
                {
                    Assert.AreEqual(0, context.PhoneNumbers.Count());
                };
                Action afterTester = () =>
                {
                    Assert.AreEqual(1, context.People.Count());
                    Assert.AreEqual(1, person.PhoneNumbers.Count());

                    var firstPhone = person.PhoneNumbers.First();
                    Assert.AreEqual(newPhone.IsPrimary, firstPhone.IsPrimary);
                    Assert.AreEqual(newPhone.PhoneNumberTypeId, firstPhone.PhoneNumberTypeId);
                    Assert.AreEqual(newPhone.Number, firstPhone.Number);
                    Assert.AreEqual(newPhone.IsPrimary, firstPhone.IsPrimary);

                    Assert.IsNull(firstPhone.Contact);
                    Assert.IsFalse(firstPhone.ContactId.HasValue);

                    Assert.AreEqual(userId, firstPhone.History.CreatedBy);
                    Assert.AreEqual(userId, firstPhone.History.RevisedBy);
                    DateTimeOffset.Now.Should().BeCloseTo(firstPhone.History.CreatedOn, 20000);
                    DateTimeOffset.Now.Should().BeCloseTo(firstPhone.History.RevisedOn, 20000);
                };
                context.SetupActions.Add(() =>
                {
                    person = new Person
                    {
                        PersonId = personId
                    };
                    context.People.Add(person);
                    context.PhoneNumberTypes.Add(phoneNumberType);
                });
                context.Revert();
                beforeTester();
                service.Create(newPhone);
                afterTester();

                context.Revert();
                beforeTester();
                await service.CreateAsync(newPhone);
                afterTester();
            }
        }

        [TestMethod]
        public async Task TestCreate_OtherPhoneNumbersPrimary()
        {
            using (ShimsContext.Create())
            {
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.SetOf1<Person>((c) =>
                {
                    return context.People;
                });
                var userId = 1;
                var personId = 10;
                Person person = null;
                var phoneNumberType = new PhoneNumberType
                {
                    PhoneNumberTypeId = PhoneNumberType.Home.Id,
                    PhoneNumberTypeName = PhoneNumberType.Home.Value
                };
                PhoneNumber primaryPhone1 = null;
                PhoneNumber primaryPhone2 = null;
                var user = new User(userId);
                var newPhone = new NewPersonPhoneNumber(user, phoneNumberType.PhoneNumberTypeId, "1234", personId, true);
                Action beforeTester = () =>
                {
                    Assert.AreEqual(2, context.PhoneNumbers.Count());
                    Assert.IsTrue(primaryPhone1.IsPrimary.Value);
                    Assert.IsTrue(primaryPhone2.IsPrimary.Value);
                };
                Action afterTester = () =>
                {
                    Assert.AreEqual(1, context.People.Count());
                    Assert.AreEqual(2, context.PhoneNumbers.Count());
                    //the phone number is added to the person, not the context                    
                    Assert.IsTrue(person.PhoneNumbers.First().IsPrimary.Value);
                    Assert.IsFalse(primaryPhone1.IsPrimary.Value);
                    Assert.IsFalse(primaryPhone2.IsPrimary.Value);
                };
                context.SetupActions.Add(() =>
                {
                    person = new Person
                    {
                        PersonId = personId
                    };
                    primaryPhone1 = new PhoneNumber
                    {
                        PhoneNumberId = 10,
                        IsPrimary = true,
                        Person = person,
                        PersonId = person.PersonId
                    };
                    primaryPhone2 = new PhoneNumber
                    {
                        PhoneNumberId = 20,
                        IsPrimary = true,
                        Person = person,
                        PersonId = person.PersonId
                    };
                    context.People.Add(person);
                    context.PhoneNumbers.Add(primaryPhone1);
                    context.PhoneNumbers.Add(primaryPhone2);
                    context.PhoneNumberTypes.Add(phoneNumberType);
                });
                context.Revert();
                beforeTester();
                service.Create(newPhone);
                afterTester();

                context.Revert();
                beforeTester();
                await service.CreateAsync(newPhone);
                afterTester();
            }
        }


        [TestMethod]
        public async Task TestCreate_PersonDoesNotExist()
        {
            using (ShimsContext.Create())
            {
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.SetOf1<Person>((c) =>
                {
                    return context.People;
                });
                var userId = 1;
                var personId = 10;
                var phoneNumberType = new PhoneNumberType
                {
                    PhoneNumberTypeId = PhoneNumberType.Home.Id,
                    PhoneNumberTypeName = PhoneNumberType.Home.Value
                };
                var user = new User(userId);
                var newPhone = new NewPersonPhoneNumber(user, phoneNumberType.PhoneNumberTypeId, "1234", personId, true);

                context.SetupActions.Add(() =>
                {
                    context.PhoneNumberTypes.Add(phoneNumberType);
                });
                var message = String.Format("The phoneNumberable entity with id [{0}] was not found.", personId);
                context.Revert();
                Action a = () => service.Create(newPhone);
                Func<Task> f = () => service.CreateAsync(newPhone);

                a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
                f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            }
        }
        #endregion

        #region Update
        [TestMethod]
        public async Task TestUpdate_Person_CheckProperties()
        {
            using (ShimsContext.Create())
            {
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.SetOf1<Person>((c) =>
                {
                    return context.People;
                });
                var creatorId = 1;
                var yesterday = DateTimeOffset.Now.AddDays(-1.0);
                var user = new User(5);
                var phoneNumberId = 10;
                Person person = new Person
                {
                    PersonId = 10,
                };
                PhoneNumberType type = new PhoneNumberType
                {
                    PhoneNumberTypeId = PhoneNumberType.Home.Id,
                    PhoneNumberTypeName = PhoneNumberType.Home.Value
                };
                PhoneNumber phoneNumber = null;

                var number = "12345";
                var isPrimary = true;
                var updatedPhone = new UpdatedPhoneNumber(user, phoneNumberId, number, type.PhoneNumberTypeId, isPrimary);

                Action afterTester = () =>
                {
                    Assert.AreEqual(1, context.PhoneNumbers.Count());
                    var firstNumber = context.PhoneNumbers.First();
                    Assert.AreEqual(number, firstNumber.Number);
                    Assert.AreEqual(isPrimary, firstNumber.IsPrimary);
                    Assert.AreEqual(type.PhoneNumberTypeId, firstNumber.PhoneNumberTypeId);
                    Assert.AreEqual(phoneNumberId, firstNumber.PhoneNumberId);
                    
                    Assert.AreEqual(creatorId, firstNumber.History.CreatedBy);
                    Assert.AreEqual(user.Id, firstNumber.History.RevisedBy);
                    Assert.AreEqual(yesterday, firstNumber.History.CreatedOn);
                    DateTimeOffset.Now.Should().BeCloseTo(firstNumber.History.RevisedOn, 20000);
                };

                context.SetupActions.Add(() =>
                {
                    phoneNumber = new PhoneNumber
                    {
                        PhoneNumberId = phoneNumberId,
                        Person = person,
                        PersonId = person.PersonId
                    };
                    phoneNumber.History.CreatedBy = creatorId;
                    phoneNumber.History.RevisedBy = creatorId;
                    phoneNumber.History.CreatedOn = yesterday;
                    phoneNumber.History.RevisedOn = yesterday;
                    context.PhoneNumbers.Add(phoneNumber);
                    context.PhoneNumberTypes.Add(type);
                });
                context.Revert();
                service.Update(updatedPhone);
                afterTester();

                context.Revert();
                await service.UpdateAsync(updatedPhone);
                afterTester();

            }
        }

        [TestMethod]
        public async Task TestUpdate_Contact_CheckProperties()
        {
            using (ShimsContext.Create())
            {
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.SetOf1<Person>((c) =>
                {
                    return context.People;
                });
                var creatorId = 1;
                var yesterday = DateTimeOffset.Now.AddDays(-1.0);
                var user = new User(5);
                var phoneNumberId = 10;
                Contact contact = new Contact
                {
                    ContactId = 10,
                };
                PhoneNumberType type = new PhoneNumberType
                {
                    PhoneNumberTypeId = PhoneNumberType.Home.Id,
                    PhoneNumberTypeName = PhoneNumberType.Home.Value
                };
                PhoneNumber phoneNumber = null;

                var number = "12345";
                var isPrimary = true;
                var updatedPhone = new UpdatedPhoneNumber(user, phoneNumberId, number, type.PhoneNumberTypeId, isPrimary);

                Action afterTester = () =>
                {
                    Assert.AreEqual(1, context.PhoneNumbers.Count());
                    var firstNumber = context.PhoneNumbers.First();
                    Assert.AreEqual(number, firstNumber.Number);
                    Assert.AreEqual(isPrimary, firstNumber.IsPrimary);
                    Assert.AreEqual(type.PhoneNumberTypeId, firstNumber.PhoneNumberTypeId);
                    Assert.AreEqual(phoneNumberId, firstNumber.PhoneNumberId);

                    Assert.AreEqual(creatorId, firstNumber.History.CreatedBy);
                    Assert.AreEqual(user.Id, firstNumber.History.RevisedBy);
                    Assert.AreEqual(yesterday, firstNumber.History.CreatedOn);
                    DateTimeOffset.Now.Should().BeCloseTo(firstNumber.History.RevisedOn, 20000);
                };

                context.SetupActions.Add(() =>
                {
                    phoneNumber = new PhoneNumber
                    {
                        PhoneNumberId = phoneNumberId,
                        Contact = contact,
                        ContactId = contact.ContactId
                    };
                    phoneNumber.History.CreatedBy = creatorId;
                    phoneNumber.History.RevisedBy = creatorId;
                    phoneNumber.History.CreatedOn = yesterday;
                    phoneNumber.History.RevisedOn = yesterday;
                    context.PhoneNumbers.Add(phoneNumber);
                    context.PhoneNumberTypes.Add(type);
                });
                context.Revert();
                service.Update(updatedPhone);
                afterTester();

                context.Revert();
                await service.UpdateAsync(updatedPhone);
                afterTester();

            }
        }

        [TestMethod]
        public async Task TestUpdate_OtherPhoneNumbersPrimary()
        {
            using (ShimsContext.Create())
            {
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.SetOf1<Person>((c) =>
                {
                    return context.People;
                });
                var creatorId = 1;
                var yesterday = DateTimeOffset.Now.AddDays(-1.0);
                var user = new User(5);
                var phoneNumberId = 10;
                Person person = new Person
                {
                    PersonId = 10,
                };
                Person otherPerson = new Person
                {
                    PersonId = 20
                };
                PhoneNumberType type = new PhoneNumberType
                {
                    PhoneNumberTypeId = PhoneNumberType.Home.Id,
                    PhoneNumberTypeName = PhoneNumberType.Home.Value
                };
                PhoneNumber phoneNumber = null;
                PhoneNumber primaryPhone1 = null;
                PhoneNumber primaryPhone2 = null;
                PhoneNumber otherPersonPrimaryPhoneNumber = null;
                var number = "12345";
                var isPrimary = true;
                var updatedPhone = new UpdatedPhoneNumber(user, phoneNumberId, number, type.PhoneNumberTypeId, isPrimary);

                Action beforeTester = () =>
                {
                    Assert.IsTrue(primaryPhone1.IsPrimary.Value);
                    Assert.IsTrue(primaryPhone2.IsPrimary.Value);
                };

                Action afterTester = () =>
                {
                    Assert.IsTrue(phoneNumber.IsPrimary.Value);
                    Assert.IsFalse(primaryPhone1.IsPrimary.Value);
                    Assert.IsFalse(primaryPhone2.IsPrimary.Value);
                    Assert.IsTrue(otherPersonPrimaryPhoneNumber.IsPrimary.Value);
                };

                context.SetupActions.Add(() =>
                {
                    otherPersonPrimaryPhoneNumber = new PhoneNumber
                    {
                        PhoneNumberId = 100,
                        IsPrimary = true,
                        Person = otherPerson,
                        PersonId = otherPerson.PersonId
                    };
                    primaryPhone1 = new PhoneNumber
                    {
                        PhoneNumberId = 20,
                        IsPrimary = true,
                        Person = person,
                        PersonId = person.PersonId
                    };
                    primaryPhone2 = new PhoneNumber
                    {
                        PhoneNumberId = 30,
                        IsPrimary = true,
                        Person = person,
                        PersonId = person.PersonId
                    };
                    phoneNumber = new PhoneNumber
                    {
                        PhoneNumberId = phoneNumberId,
                        Person = person,
                        PersonId = person.PersonId
                    };
                    context.PhoneNumbers.Add(phoneNumber);
                    context.PhoneNumbers.Add(primaryPhone1);
                    context.PhoneNumbers.Add(primaryPhone2);
                    context.PhoneNumbers.Add(otherPersonPrimaryPhoneNumber);
                    context.PhoneNumberTypes.Add(type);
                });
                context.Revert();
                beforeTester();
                service.Update(updatedPhone);
                afterTester();

                context.Revert();
                beforeTester();
                await service.UpdateAsync(updatedPhone);
                afterTester();

            }
        }

        [TestMethod]
        public async Task TestUpdate_PhoneNumberDoesNotExist()
        {
            using (ShimsContext.Create())
            {
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.SetOf1<Person>((c) =>
                {
                    return context.People;
                });
                var yesterday = DateTimeOffset.Now.AddDays(-1.0);
                var user = new User(5);
                var phoneNumberId = 10;
                PhoneNumberType type = new PhoneNumberType
                {
                    PhoneNumberTypeId = PhoneNumberType.Home.Id,
                    PhoneNumberTypeName = PhoneNumberType.Home.Value
                };

                var number = "12345";
                var isPrimary = true;
                var updatedPhone = new UpdatedPhoneNumber(user, phoneNumberId, number, type.PhoneNumberTypeId, isPrimary);
                
                Action a = () => service.Update(updatedPhone);
                Func<Task> f = () => service.UpdateAsync(updatedPhone);
                var message = String.Format("The phone number with id [{0}] was not found.", phoneNumberId);
                a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
                f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            }
        }
        #endregion

        #region Delete
        [TestMethod]
        public async Task TestDelete()
        {
            using (ShimsContext.Create())
            {
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.SetOf1<Person>((c) =>
                {
                    return context.People;
                });
                var phoneNumberType = new PhoneNumberType
                {
                    PhoneNumberTypeId = PhoneNumberType.Home.Id,
                    PhoneNumberTypeName = PhoneNumberType.Home.Value
                };
                var phoneNumber = new PhoneNumber
                {
                    PhoneNumberId = 1
                };
                Action beforeTester = () =>
                {
                    Assert.AreEqual(1, context.PhoneNumbers.Count());
                };
                Action afterTester = () =>
                {
                    Assert.AreEqual(0, context.PhoneNumbers.Count());
                };
                context.SetupActions.Add(() =>
                {
                    context.PhoneNumbers.Add(phoneNumber);
                    context.PhoneNumberTypes.Add(phoneNumberType);
                });
                context.Revert();
                beforeTester();
                service.Delete(phoneNumber.PhoneNumberId);
                afterTester();

                context.Revert();
                beforeTester();
                await service.DeleteAsync(phoneNumber.PhoneNumberId);
                afterTester();
            }
        }

        [TestMethod]
        public async Task TestDelete_PhoneNumberDoesNotExist()
        {
            using (ShimsContext.Create())
            {
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.SetOf1<Person>((c) =>
                {
                    return context.People;
                });
                var phoneNumberType = new PhoneNumberType
                {
                    PhoneNumberTypeId = PhoneNumberType.Home.Id,
                    PhoneNumberTypeName = PhoneNumberType.Home.Value
                };
                Action beforeTester = () =>
                {
                    Assert.AreEqual(0, context.PhoneNumbers.Count());
                };
                Action afterTester = () =>
                {
                    Assert.AreEqual(0, context.PhoneNumbers.Count());
                };
                context.SetupActions.Add(() =>
                {
                    context.PhoneNumberTypes.Add(phoneNumberType);
                });
                context.Revert();
                beforeTester();
                Action a = () => service.Delete(1);
                Func<Task> f = () => service.DeleteAsync(1);
                a.ShouldNotThrow();
                f.ShouldNotThrow();
                beforeTester();
            }
        }
        #endregion

    }
}
