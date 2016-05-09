using System;
using FluentAssertions;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Persons;
using System.Threading.Tasks;
using ECA.Data;
using ECA.Business.Queries.Models.Persons;
using ECA.Core.Query;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.DynamicLinq.Filter;
using ECA.Business.Validation;
using Moq;
using ECA.Business.Service;
using ECA.Business.Service.Admin;
using System.Collections.Generic;

namespace ECA.Business.Test.Service.Persons
{
    [TestClass]
    public class ContactServiceTest
    {
        private TestEcaContext context;
        private ContactService service;
        private Mock<IBusinessValidator<AdditionalPointOfContactValidationEntity, object>> validator;

        [TestInitialize]
        public void TestInit()
        {
            validator = new Mock<IBusinessValidator<AdditionalPointOfContactValidationEntity, object>>();
            context = new TestEcaContext();
            service = new ContactService(context, validator.Object);
        }


        #region Get
        [TestMethod]
        public async Task TestGetContactById_CheckProperties()
        {
            var contact = new Contact
            {
                ContactId = 1,
                FullName = "full name",
                Position = "position"
            };
            var emailAddressType = new EmailAddressType
            {
                EmailAddressTypeId = EmailAddressType.Home.Id,
                EmailAddressTypeName = EmailAddressType.Home.Value,
            };
            var email = new EmailAddress
            {
                Address = "someone@isp.com",
                Contact = contact,
                ContactId = contact.ContactId,
                EmailAddressId = 2,
                EmailAddressType = emailAddressType,
                EmailAddressTypeId = emailAddressType.EmailAddressTypeId,
            };
            var phoneNumberType = new PhoneNumberType
            {
                PhoneNumberTypeId = PhoneNumberType.Cell.Id,
                PhoneNumberTypeName = PhoneNumberType.Cell.Value
            };
            var phoneNumber = new PhoneNumber
            {
                Contact = contact,
                ContactId = contact.ContactId,
                Number = "555-5555",
                PhoneNumberId = 3,
                PhoneNumberType = phoneNumberType,
                PhoneNumberTypeId = phoneNumberType.PhoneNumberTypeId
            };
            context.PhoneNumberTypes.Add(phoneNumberType);
            context.PhoneNumbers.Add(phoneNumber);
            context.EmailAddresses.Add(email);
            context.EmailAddressTypes.Add(emailAddressType);
            context.Contacts.Add(contact);
            Action<ContactDTO> tester = (serviceResult) =>
            {
                Assert.IsNotNull(serviceResult);
                Assert.AreEqual(contact.ContactId, serviceResult.Id);
                Assert.AreEqual(contact.FullName, serviceResult.FullName);
                Assert.AreEqual(contact.Position, serviceResult.Position);

                Assert.AreEqual(1, serviceResult.EmailAddresses.Count());
                var firstEmail = serviceResult.EmailAddresses.First();
                Assert.AreEqual(email.Address, firstEmail.Address);
                Assert.AreEqual(email.EmailAddressTypeId, firstEmail.EmailAddressTypeId);
                Assert.AreEqual(emailAddressType.EmailAddressTypeName, firstEmail.EmailAddressType);
                Assert.AreEqual(contact.ContactId, firstEmail.ContactId);
                Assert.IsNull(firstEmail.PersonId);
                Assert.AreEqual(email.EmailAddressId, firstEmail.Id);

                Assert.AreEqual(1, serviceResult.PhoneNumbers.Count());
                var firstPhoneNumber = serviceResult.PhoneNumbers.First();
                Assert.AreEqual(phoneNumberType.PhoneNumberTypeId, firstPhoneNumber.PhoneNumberTypeId);
                Assert.AreEqual(phoneNumberType.PhoneNumberTypeName, firstPhoneNumber.PhoneNumberType);
                Assert.AreEqual(phoneNumber.PhoneNumberId, firstPhoneNumber.Id);
                Assert.AreEqual(phoneNumber.Number, firstPhoneNumber.Number);
                Assert.AreEqual(contact.ContactId, firstPhoneNumber.ContactId);
                Assert.IsNull(firstPhoneNumber.PersonId);
            };

            var serviceResults = service.GetContactById(contact.ContactId);
            var serviceResultsAsync = await service.GetContactByIdAsync(contact.ContactId);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetContactById_ContactDoesNotExist()
        {
            Action<ContactDTO> tester = (serviceResult) =>
            {
                Assert.AreEqual(0, context.Contacts.Count());
                Assert.IsNull(serviceResult);
            };

            var serviceResults = service.GetContactById(1);
            var serviceResultsAsync = await service.GetContactByIdAsync(1);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetContacts_CheckProperties()
        {
            var contact = new Contact
            {
                ContactId = 1,
                FullName = "full name",
                Position = "position"
            };
            var emailAddressType = new EmailAddressType
            {
                EmailAddressTypeId = EmailAddressType.Home.Id,
                EmailAddressTypeName = EmailAddressType.Home.Value,
            };
            var email = new EmailAddress
            {
                Address = "someone@isp.com",
                Contact = contact,
                ContactId = contact.ContactId,
                EmailAddressId = 2,
                EmailAddressType = emailAddressType,
                EmailAddressTypeId = emailAddressType.EmailAddressTypeId,
                IsPrimary = true
            };
            var phoneNumberType = new PhoneNumberType
            {
                PhoneNumberTypeId = PhoneNumberType.Cell.Id,
                PhoneNumberTypeName = PhoneNumberType.Cell.Value
            };
            var phoneNumber = new PhoneNumber
            {
                Contact = contact,
                ContactId = contact.ContactId,
                Number = "555-5555",
                PhoneNumberId = 3,
                PhoneNumberType = phoneNumberType,
                PhoneNumberTypeId = phoneNumberType.PhoneNumberTypeId,
                IsPrimary = true
            };
            context.PhoneNumberTypes.Add(phoneNumberType);
            context.PhoneNumbers.Add(phoneNumber);
            context.EmailAddresses.Add(email);
            context.EmailAddressTypes.Add(emailAddressType);
            context.Contacts.Add(contact);
            Action<PagedQueryResults<ContactDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(contact.ContactId, firstResult.Id);
                Assert.AreEqual(contact.FullName, firstResult.FullName);
                Assert.AreEqual(contact.Position, firstResult.Position);

                Assert.AreEqual(1, firstResult.EmailAddresses.Count());
                var firstEmail = firstResult.EmailAddresses.First();
                Assert.AreEqual(email.Address, firstEmail.Address);
                Assert.AreEqual(email.EmailAddressTypeId, firstEmail.EmailAddressTypeId);
                Assert.AreEqual(emailAddressType.EmailAddressTypeName, firstEmail.EmailAddressType);
                Assert.AreEqual(contact.ContactId, firstEmail.ContactId);
                Assert.IsNull(firstEmail.PersonId);
                Assert.AreEqual(email.EmailAddressId, firstEmail.Id);
                Assert.IsTrue(firstEmail.IsPrimary.Value);

                Assert.AreEqual(1, firstResult.PhoneNumbers.Count());
                var firstPhoneNumber = firstResult.PhoneNumbers.First();
                Assert.AreEqual(phoneNumberType.PhoneNumberTypeId, firstPhoneNumber.PhoneNumberTypeId);
                Assert.AreEqual(phoneNumberType.PhoneNumberTypeName, firstPhoneNumber.PhoneNumberType);
                Assert.AreEqual(phoneNumber.PhoneNumberId, firstPhoneNumber.Id);
                Assert.AreEqual(phoneNumber.Number, firstPhoneNumber.Number);
                Assert.AreEqual(contact.ContactId, firstPhoneNumber.ContactId);
                Assert.IsNull(firstPhoneNumber.PersonId);
                Assert.IsTrue(firstPhoneNumber.IsPrimary.Value);
            };
            var defaultSorter = new ExpressionSorter<ContactDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<ContactDTO>(0, 10, defaultSorter);

            var serviceResults = service.GetContacts(queryOperator);
            var serviceResultsAsync = await service.GetContactsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetContacts_DefaultSorter()
        {
            var contact1 = new Contact
            {
                ContactId = 1,
            };
            var contact2 = new Contact
            {
                ContactId = 2,
            };
            context.Contacts.Add(contact1);
            context.Contacts.Add(contact2);
            Action<PagedQueryResults<ContactDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(contact2.ContactId, firstResult.Id);
            };
            var defaultSorter = new ExpressionSorter<ContactDTO>(x => x.Id, SortDirection.Descending);
            var queryOperator = new QueryableOperator<ContactDTO>(0, 1, defaultSorter);

            var serviceResults = service.GetContacts(queryOperator);
            var serviceResultsAsync = await service.GetContactsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetContacts_Filter()
        {
            var contact1 = new Contact
            {
                ContactId = 1,
            };
            var contact2 = new Contact
            {
                ContactId = 2,
            };
            context.Contacts.Add(contact1);
            context.Contacts.Add(contact2);
            Action<PagedQueryResults<ContactDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(contact1.ContactId, firstResult.Id);
            };
            var defaultSorter = new ExpressionSorter<ContactDTO>(x => x.Id, SortDirection.Descending);
            var queryOperator = new QueryableOperator<ContactDTO>(0, 1, defaultSorter);
            queryOperator.Filters.Add(new ExpressionFilter<ContactDTO>(x => x.Id, ComparisonType.Equal, contact1.ContactId));

            var serviceResults = service.GetContacts(queryOperator);
            var serviceResultsAsync = await service.GetContactsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetContacts_Sort()
        {
            var contact1 = new Contact
            {
                ContactId = 1,
            };
            var contact2 = new Contact
            {
                ContactId = 2,
            };
            context.Contacts.Add(contact2);
            context.Contacts.Add(contact1);

            Action<PagedQueryResults<ContactDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(contact2.ContactId, firstResult.Id);
            };
            var defaultSorter = new ExpressionSorter<ContactDTO>(x => x.Id, SortDirection.Descending);
            var queryOperator = new QueryableOperator<ContactDTO>(0, 1, defaultSorter);
            queryOperator.Sorters.Add(new ExpressionSorter<ContactDTO>(x => x.Id, SortDirection.Descending));

            var serviceResults = service.GetContacts(queryOperator);
            var serviceResultsAsync = await service.GetContactsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetContacts_Paging()
        {
            var contact1 = new Contact
            {
                ContactId = 1,
            };
            var contact2 = new Contact
            {
                ContactId = 2,
            };
            context.Contacts.Add(contact2);
            context.Contacts.Add(contact1);

            Action<PagedQueryResults<ContactDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(1, results.Results.Count);
            };
            var defaultSorter = new ExpressionSorter<ContactDTO>(x => x.Id, SortDirection.Descending);
            var queryOperator = new QueryableOperator<ContactDTO>(0, 1, defaultSorter);

            var serviceResults = service.GetContacts(queryOperator);
            var serviceResultsAsync = await service.GetContactsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        #endregion

        #region Create
        [TestMethod]
        public async Task TestCreate_CheckProperties()
        {
            var userId = 1;
            var user = new User(userId);
            var phoneNumber1 = "555-555-1212";
            var phoneNumber2 = "123-456-7890";
            var email1 = "someone@isp.com";
            var email2 = "me@gmail.com";
            var fullName = "Full Name";
            var position = "Star Lord";
            var isPrimaryEmail = true;
            var isPrimaryPhone = true;

            var newEmail1 = new NewEmailAddress(user, EmailAddressType.Business.Id, email1, isPrimaryEmail);
            var newEmail2 = new NewEmailAddress(user, EmailAddressType.Home.Id, email2, false);

            var newPhone1 = new NewPhoneNumber(user, PhoneNumberType.Cell.Id, phoneNumber1, isPrimaryPhone);
            var newPhone2 = new NewPhoneNumber(user, PhoneNumberType.Home.Id, phoneNumber2, false);

            var additionalPointOfContact = new AdditionalPointOfContact(user, fullName, position, new List<NewEmailAddress> { newEmail1, newEmail2 }, new List<NewPhoneNumber> { newPhone1, newPhone2 });

            Action beforeTester = () =>
            {
                Assert.AreEqual(0, context.PhoneNumbers.Count());
                Assert.AreEqual(0, context.EmailAddresses.Count());
                Assert.AreEqual(0, context.Contacts.Count());
            };
            Action<AdditionalPointOfContactValidationEntity> validationEntityTester = (entity) =>
            {
                Assert.AreEqual(fullName, entity.FullName);
                Assert.AreEqual(position, entity.Position);
                Assert.AreEqual(1, entity.NumberOfPrimaryEmailAddresses);
                Assert.AreEqual(1, entity.NumberOfPrimaryPhoneNumbers);
            };
            Action afterTester = () =>
            {
                Assert.AreEqual(1, context.Contacts.Count());
                Assert.AreEqual(2, context.EmailAddresses.Count());
                Assert.AreEqual(2, context.PhoneNumbers.Count());

                var pointOfContact = context.Contacts.First();
                Assert.AreEqual(fullName, pointOfContact.FullName);
                Assert.AreEqual(position, pointOfContact.Position);
                Assert.AreEqual(userId, pointOfContact.History.CreatedBy);
                Assert.AreEqual(userId, pointOfContact.History.RevisedBy);
                DateTimeOffset.Now.Should().BeCloseTo(pointOfContact.History.CreatedOn, 20000);
                DateTimeOffset.Now.Should().BeCloseTo(pointOfContact.History.RevisedOn, 20000);
                Assert.AreEqual(2, pointOfContact.EmailAddresses.Count);
                Assert.AreEqual(2, pointOfContact.PhoneNumbers.Count);

                var firstEmail = pointOfContact.EmailAddresses.First();
                var secondEmail = pointOfContact.EmailAddresses.Last();
                Assert.IsTrue(context.EmailAddresses.Contains(firstEmail));
                Assert.IsTrue(context.EmailAddresses.Contains(secondEmail));
                Assert.AreEqual(newEmail1.EmailAddressTypeId, firstEmail.EmailAddressTypeId);
                Assert.AreEqual(newEmail1.Address, firstEmail.Address);
                Assert.IsTrue(firstEmail.IsPrimary.Value);
                Assert.AreEqual(newEmail2.EmailAddressTypeId, secondEmail.EmailAddressTypeId);
                Assert.AreEqual(newEmail2.Address, secondEmail.Address);
                Assert.IsFalse(secondEmail.IsPrimary.Value);

                Assert.AreEqual(userId, firstEmail.History.CreatedBy);
                Assert.AreEqual(userId, firstEmail.History.RevisedBy);
                DateTimeOffset.Now.Should().BeCloseTo(firstEmail.History.CreatedOn, 20000);
                DateTimeOffset.Now.Should().BeCloseTo(firstEmail.History.RevisedOn, 20000);

                Assert.AreEqual(userId, secondEmail.History.CreatedBy);
                Assert.AreEqual(userId, secondEmail.History.RevisedBy);
                DateTimeOffset.Now.Should().BeCloseTo(secondEmail.History.CreatedOn, 20000);
                DateTimeOffset.Now.Should().BeCloseTo(secondEmail.History.RevisedOn, 20000);
                
                var firstPhone = pointOfContact.PhoneNumbers.First();
                var secondPhone = pointOfContact.PhoneNumbers.Last();
                Assert.IsTrue(context.PhoneNumbers.Contains(firstPhone));
                Assert.IsTrue(context.PhoneNumbers.Contains(secondPhone));
                Assert.AreEqual(newPhone1.PhoneNumberTypeId, firstPhone.PhoneNumberTypeId);
                Assert.AreEqual(newPhone1.Number, firstPhone.Number);
                Assert.IsTrue(firstPhone.IsPrimary.Value);
                Assert.AreEqual(newPhone2.PhoneNumberTypeId, secondPhone.PhoneNumberTypeId);
                Assert.AreEqual(newPhone2.Number, secondPhone.Number);
                Assert.IsFalse(secondPhone.IsPrimary.Value);

                Assert.AreEqual(userId, firstPhone.History.CreatedBy);
                Assert.AreEqual(userId, firstPhone.History.RevisedBy);
                DateTimeOffset.Now.Should().BeCloseTo(firstPhone.History.CreatedOn, 20000);
                DateTimeOffset.Now.Should().BeCloseTo(firstPhone.History.RevisedOn, 20000);

                Assert.AreEqual(userId, secondPhone.History.CreatedBy);
                Assert.AreEqual(userId, secondPhone.History.RevisedBy);
                DateTimeOffset.Now.Should().BeCloseTo(secondPhone.History.CreatedOn, 20000);
                DateTimeOffset.Now.Should().BeCloseTo(secondPhone.History.RevisedOn, 20000);
            };
            validator.Setup(x => x.ValidateCreate(It.IsAny<AdditionalPointOfContactValidationEntity>())).Callback(validationEntityTester);
            context.Revert();
            beforeTester();
            service.Create(additionalPointOfContact);
            afterTester();

            context.Revert();
            beforeTester();
            await service.CreateAsync(additionalPointOfContact);
            afterTester();
        }

        [TestMethod]
        public async Task TestCreate_PositionIsNull()
        {
            var userId = 1;
            var user = new User(userId);
            var fullName = "Full Name";
            string position = null;

            var additionalPointOfContact = new AdditionalPointOfContact(user, fullName, position, new List<NewEmailAddress>(), new List<NewPhoneNumber>());

            Action beforeTester = () =>
            {
                Assert.AreEqual(0, context.PhoneNumbers.Count());
                Assert.AreEqual(0, context.EmailAddresses.Count());
                Assert.AreEqual(0, context.Contacts.Count());
            };
            Action<AdditionalPointOfContactValidationEntity> validationEntityTester = (entity) =>
            {
                Assert.AreEqual(position, entity.Position);
            };
            Action afterTester = () =>
            {
                Assert.AreEqual(1, context.Contacts.Count());                
                var pointOfContact = context.Contacts.First();
                Assert.AreEqual(position, pointOfContact.Position);
                
            };
            validator.Setup(x => x.ValidateCreate(It.IsAny<AdditionalPointOfContactValidationEntity>())).Callback(validationEntityTester);
            context.Revert();
            beforeTester();
            service.Create(additionalPointOfContact);
            afterTester();

            context.Revert();
            beforeTester();
            await service.CreateAsync(additionalPointOfContact);
            afterTester();
        }

        [TestMethod]
        public async Task TestCreate_PositionIsEmpty()
        {
            var userId = 1;
            var user = new User(userId);
            var fullName = "Full Name";
            string position = String.Empty;

            var additionalPointOfContact = new AdditionalPointOfContact(user, fullName, position, new List<NewEmailAddress>(), new List<NewPhoneNumber>());

            Action beforeTester = () =>
            {
                Assert.AreEqual(0, context.PhoneNumbers.Count());
                Assert.AreEqual(0, context.EmailAddresses.Count());
                Assert.AreEqual(0, context.Contacts.Count());
            };
            Action<AdditionalPointOfContactValidationEntity> validationEntityTester = (entity) =>
            {
                Assert.AreEqual(position, entity.Position);
            };
            Action afterTester = () =>
            {
                Assert.AreEqual(1, context.Contacts.Count());
                var pointOfContact = context.Contacts.First();
                Assert.IsNull(pointOfContact.Position);

            };
            validator.Setup(x => x.ValidateCreate(It.IsAny<AdditionalPointOfContactValidationEntity>())).Callback(validationEntityTester);
            context.Revert();
            beforeTester();
            service.Create(additionalPointOfContact);
            afterTester();

            context.Revert();
            beforeTester();
            await service.CreateAsync(additionalPointOfContact);
            afterTester();
        }

        [TestMethod]
        public async Task TestCreate_PositionIsWhitespace()
        {
            var userId = 1;
            var user = new User(userId);
            var fullName = "Full Name";
            string position = " ";

            var additionalPointOfContact = new AdditionalPointOfContact(user, fullName, position, new List<NewEmailAddress>(), new List<NewPhoneNumber>());

            Action beforeTester = () =>
            {
                Assert.AreEqual(0, context.PhoneNumbers.Count());
                Assert.AreEqual(0, context.EmailAddresses.Count());
                Assert.AreEqual(0, context.Contacts.Count());
            };
            Action<AdditionalPointOfContactValidationEntity> validationEntityTester = (entity) =>
            {
                Assert.AreEqual(position, entity.Position);
            };
            Action afterTester = () =>
            {
                Assert.AreEqual(1, context.Contacts.Count());
                var pointOfContact = context.Contacts.First();
                Assert.IsNull(pointOfContact.Position);

            };
            validator.Setup(x => x.ValidateCreate(It.IsAny<AdditionalPointOfContactValidationEntity>())).Callback(validationEntityTester);
            context.Revert();
            beforeTester();
            service.Create(additionalPointOfContact);
            afterTester();

            context.Revert();
            beforeTester();
            await service.CreateAsync(additionalPointOfContact);
            afterTester();
        }

        [TestMethod]
        public async Task TestCreate_LikeContactEmailAddress()
        {
            var userId = 1;
            var user = new User(userId);
            var email1 = "someone@isp.com";
            var fullName = "Full Name";
            var position = "Star Lord";
            var isPrimary = true;

            var newEmail1 = new NewEmailAddress(user, EmailAddressType.Business.Id, email1, isPrimary);

            var additionalPointOfContact = new AdditionalPointOfContact(user, fullName, position, new List<NewEmailAddress> { newEmail1 }, new List<NewPhoneNumber>());

            Action beforeTester = () =>
            {
                Assert.AreEqual(0, context.PhoneNumbers.Count());
                Assert.AreEqual(1, context.EmailAddresses.Count());
                Assert.AreEqual(0, context.Contacts.Count());
            };
            Action<AdditionalPointOfContactValidationEntity> validationEntityTester = (entity) =>
            {
                Assert.AreEqual(fullName, entity.FullName);
                Assert.AreEqual(position, entity.Position);
            };
            Action afterTester = () =>
            {
                Assert.AreEqual(1, context.Contacts.Count());
            };
            context.SetupActions.Add(() =>
            {
                context.EmailAddresses.Add(new EmailAddress
                {
                    ContactId = 1,
                    Address = email1
                });
            });
            validator.Setup(x => x.ValidateCreate(It.IsAny<AdditionalPointOfContactValidationEntity>())).Callback(validationEntityTester);
            context.Revert();
            beforeTester();
            service.Create(additionalPointOfContact);
            afterTester();

            context.Revert();
            beforeTester();
            await service.CreateAsync(additionalPointOfContact);
            afterTester();
        }

        [TestMethod]
        public async Task TestCreate_EmailAddressValueIsNull()
        {
            var userId = 1;
            var user = new User(userId);
            string email1 = null;
            var fullName = "Full Name";
            var position = "Star Lord";
            var isPrimary = true;

            var newEmail1 = new NewEmailAddress(user, EmailAddressType.Business.Id, email1, isPrimary);
            var additionalPointOfContact = new AdditionalPointOfContact(user, fullName, position, new List<NewEmailAddress> { newEmail1 }, new List<NewPhoneNumber>());

            Action beforeTester = () =>
            {
                Assert.AreEqual(0, context.PhoneNumbers.Count());
                Assert.AreEqual(0, context.EmailAddresses.Count());
                Assert.AreEqual(0, context.Contacts.Count());
            };
            Action afterTester = () =>
            {
                Assert.AreEqual(1, context.Contacts.Count());
                Assert.AreEqual(0, context.EmailAddresses.Count());
            };
            context.Revert();
            beforeTester();
            service.Create(additionalPointOfContact);
            afterTester();

            context.Revert();
            beforeTester();
            await service.CreateAsync(additionalPointOfContact);
            afterTester();
        }

        [TestMethod]
        public async Task TestCreate_EmailAddressValueIsEmpty()
        {
            var userId = 1;
            var user = new User(userId);
            string email1 = String.Empty;
            var fullName = "Full Name";
            var position = "Star Lord";
            var isPrimary = true;

            var newEmail1 = new NewEmailAddress(user, EmailAddressType.Business.Id, email1, isPrimary);
            var additionalPointOfContact = new AdditionalPointOfContact(user, fullName, position, new List<NewEmailAddress> { newEmail1 }, new List<NewPhoneNumber>());

            Action beforeTester = () =>
            {
                Assert.AreEqual(0, context.PhoneNumbers.Count());
                Assert.AreEqual(0, context.EmailAddresses.Count());
                Assert.AreEqual(0, context.Contacts.Count());
            };
            Action afterTester = () =>
            {
                Assert.AreEqual(1, context.Contacts.Count());
                Assert.AreEqual(0, context.EmailAddresses.Count());
            };
            context.Revert();
            beforeTester();
            service.Create(additionalPointOfContact);
            afterTester();

            context.Revert();
            beforeTester();
            await service.CreateAsync(additionalPointOfContact);
            afterTester();
        }

        [TestMethod]
        public async Task TestCreate_EmailAddressValueIsWhitespace()
        {
            var userId = 1;
            var user = new User(userId);
            string email1 = String.Empty;
            var fullName = "Full Name";
            var position = "Star Lord";
            var isPrimary = true;

            var newEmail1 = new NewEmailAddress(user, EmailAddressType.Business.Id, email1, isPrimary);
            var additionalPointOfContact = new AdditionalPointOfContact(user, fullName, position, new List<NewEmailAddress> { newEmail1 }, new List<NewPhoneNumber>());

            Action beforeTester = () =>
            {
                Assert.AreEqual(0, context.PhoneNumbers.Count());
                Assert.AreEqual(0, context.EmailAddresses.Count());
                Assert.AreEqual(0, context.Contacts.Count());
            };
            Action afterTester = () =>
            {
                Assert.AreEqual(1, context.Contacts.Count());
                Assert.AreEqual(0, context.EmailAddresses.Count());
            };
            context.Revert();
            beforeTester();
            service.Create(additionalPointOfContact);
            afterTester();

            context.Revert();
            beforeTester();
            await service.CreateAsync(additionalPointOfContact);
            afterTester();
        }

        [TestMethod]
        public async Task TestCreate_PhoneNumberIsNull()
        {
            var userId = 1;
            var user = new User(userId);
            string phone1 = null;
            var fullName = "Full Name";
            var position = "Star Lord";
            var isPrimary = true;

            var newPhone1 = new NewPhoneNumber(user, PhoneNumberType.Cell.Id, phone1, isPrimary);
            var additionalPointOfContact = new AdditionalPointOfContact(user, fullName, position, new List<NewEmailAddress>(), new List<NewPhoneNumber> { newPhone1 });

            Action beforeTester = () =>
            {
                Assert.AreEqual(0, context.PhoneNumbers.Count());
                Assert.AreEqual(0, context.EmailAddresses.Count());
                Assert.AreEqual(0, context.Contacts.Count());
            };
            Action afterTester = () =>
            {
                Assert.AreEqual(1, context.Contacts.Count());
                Assert.AreEqual(0, context.PhoneNumbers.Count());
            };
            context.Revert();
            beforeTester();
            service.Create(additionalPointOfContact);
            afterTester();

            context.Revert();
            beforeTester();
            await service.CreateAsync(additionalPointOfContact);
            afterTester();
        }

        [TestMethod]
        public async Task TestCreate_PhoneNumberIsEmpty()
        {
            var userId = 1;
            var user = new User(userId);
            string phone1 = String.Empty;
            var fullName = "Full Name";
            var position = "Star Lord";
            var isPrimary = true;

            var newPhone1 = new NewPhoneNumber(user, PhoneNumberType.Cell.Id, phone1, isPrimary);
            var additionalPointOfContact = new AdditionalPointOfContact(user, fullName, position, new List<NewEmailAddress>(), new List<NewPhoneNumber> { newPhone1 });

            Action beforeTester = () =>
            {
                Assert.AreEqual(0, context.PhoneNumbers.Count());
                Assert.AreEqual(0, context.EmailAddresses.Count());
                Assert.AreEqual(0, context.Contacts.Count());
            };
            Action afterTester = () =>
            {
                Assert.AreEqual(1, context.Contacts.Count());
                Assert.AreEqual(0, context.PhoneNumbers.Count());
            };
            context.Revert();
            beforeTester();
            service.Create(additionalPointOfContact);
            afterTester();

            context.Revert();
            beforeTester();
            await service.CreateAsync(additionalPointOfContact);
            afterTester();
        }

        [TestMethod]
        public async Task TestCreate_PhoneNumberIsWhitespace()
        {
            var userId = 1;
            var user = new User(userId);
            string phone1 = " ";
            var fullName = "Full Name";
            var position = "Star Lord";
            var isPrimary = true;

            var newPhone1 = new NewPhoneNumber(user, PhoneNumberType.Cell.Id, phone1, isPrimary);
            var additionalPointOfContact = new AdditionalPointOfContact(user, fullName, position, new List<NewEmailAddress>(), new List<NewPhoneNumber> { newPhone1 });

            Action beforeTester = () =>
            {
                Assert.AreEqual(0, context.PhoneNumbers.Count());
                Assert.AreEqual(0, context.EmailAddresses.Count());
                Assert.AreEqual(0, context.Contacts.Count());
            };
            Action afterTester = () =>
            {
                Assert.AreEqual(1, context.Contacts.Count());
                Assert.AreEqual(0, context.PhoneNumbers.Count());
            };
            context.Revert();
            beforeTester();
            service.Create(additionalPointOfContact);
            afterTester();

            context.Revert();
            beforeTester();
            await service.CreateAsync(additionalPointOfContact);
            afterTester();
        }
        #endregion
        
    }
}
