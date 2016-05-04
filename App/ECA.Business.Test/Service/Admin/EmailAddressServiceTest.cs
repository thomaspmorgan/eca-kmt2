using System;
using FluentAssertions;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Admin;
using ECA.Data;
using System.Threading.Tasks;
using ECA.Business.Queries.Models.Admin;
using Microsoft.QualityTools.Testing.Fakes;
using ECA.Business.Service;
using ECA.Core.Exceptions;
using System.Net;
using System.Collections.Generic;
using ECA.Business.Exceptions;

namespace ECA.Business.Test.Service.Admin
{
    [TestClass]
    public class EmailAddressServiceTest
    {
        private TestEcaContext context;
        private EmailAddressService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new EmailAddressService(context);
        }

        #region Get

        [TestMethod]
        public async Task TestGetById()
        {
            var person = new Person
            {
                PersonId = 10,
                FullName = "full name"
            };
            var emailAddressType = new EmailAddressType
            {
                EmailAddressTypeId = EmailAddressType.Home.Id,
                EmailAddressTypeName = EmailAddressType.Home.Value
            };
            var email = new EmailAddress
            {
                Address = "someone@isp.com",
                PersonId = person.PersonId,
                Person = person,
                EmailAddressId = 1,
                EmailAddressType = emailAddressType,
                EmailAddressTypeId = emailAddressType.EmailAddressTypeId,
                IsPrimary = true
            };
            context.EmailAddresses.Add(email);
            context.EmailAddressTypes.Add(emailAddressType);
            context.People.Add(person);
            Action<EmailAddressDTO> tester = (dto) =>
            {
                Assert.IsNotNull(dto);
                Assert.AreEqual(email.IsPrimary, dto.IsPrimary);
                Assert.AreEqual(email.PersonId, dto.PersonId);
                Assert.AreEqual(email.EmailAddressId, dto.Id);
                Assert.AreEqual(email.Address, dto.Address);
                Assert.AreEqual(email.EmailAddressType.EmailAddressTypeName, dto.EmailAddressType);
                Assert.AreEqual(email.EmailAddressTypeId, dto.EmailAddressTypeId);
            };

            var result = service.GetById(email.EmailAddressId);
            var resultAsync = await service.GetByIdAsync(email.EmailAddressId);
            tester(result);
            tester(resultAsync);
        }

        [TestMethod]
        public async Task TestGetById_EmailDoesNotExist()
        {
            Action<EmailAddressDTO> tester = (dto) =>
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
                var personId = 1;
                Person person = null;
                EmailAddressType emailAddressType = new EmailAddressType
                {
                    EmailAddressTypeId = EmailAddressType.Home.Id,
                    EmailAddressTypeName = EmailAddressType.Home.Value
                };
                context.SetupActions.Add(() =>
                {
                    person = new Person
                    {
                        PersonId = personId,
                        FullName = "full name"
                    };
                    context.EmailAddressTypes.Add(emailAddressType);
                    context.People.Add(person);
                });
                var user = new User(1);
                var email = "someone@isp.com";
                var emailAddressTypeId = emailAddressType.EmailAddressTypeId;
                var isPrimary = true;
                var newEmail = new NewPersonEmailAddress(user, emailAddressTypeId, email, isPrimary, personId);
                Action beforeTester = () =>
                {
                    Assert.AreEqual(1, context.People.Count());
                    Assert.AreEqual(0, context.EmailAddresses.Count());
                };
                Action afterTester = () =>
                {
                    Assert.AreEqual(1, context.People.Count());
                    var firstPerson = context.People.First();
                    Assert.AreEqual(1, firstPerson.EmailAddresses.Count);
                    var firstEmail = firstPerson.EmailAddresses.First();
                    Assert.AreEqual(email, firstEmail.Address);
                    Assert.AreEqual(emailAddressTypeId, firstEmail.EmailAddressTypeId);
                    Assert.AreEqual(isPrimary, firstEmail.IsPrimary);

                    Assert.AreEqual(user.Id, firstEmail.History.CreatedBy);
                    Assert.AreEqual(user.Id, firstEmail.History.RevisedBy);
                    DateTimeOffset.Now.Should().BeCloseTo(firstEmail.History.CreatedOn, 20000);
                    DateTimeOffset.Now.Should().BeCloseTo(firstEmail.History.RevisedOn, 20000);
                };
                context.Revert();
                beforeTester();
                service.Create(newEmail);
                afterTester();

                context.Revert();
                beforeTester();
                await service.CreateAsync(newEmail);
                afterTester();
            }
        }

        [TestMethod]
        public async Task TestCreate_OtherEmailAddressesPrimary()
        {
            using (ShimsContext.Create())
            {
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.SetOf1<Person>((c) =>
                {
                    return context.People;
                });
                var personId = 1;
                Person person = null;
                EmailAddressType emailAddressType = new EmailAddressType
                {
                    EmailAddressTypeId = EmailAddressType.Home.Id,
                    EmailAddressTypeName = EmailAddressType.Home.Value
                };
                EmailAddress primaryEmail1 = null;
                EmailAddress primaryEmail2 = null;
                context.SetupActions.Add(() =>
                {
                    person = new Person
                    {
                        PersonId = personId,
                        FullName = "full name"
                    };
                    primaryEmail1 = new EmailAddress
                    {
                        IsPrimary = true,
                        EmailAddressId = 10,
                        PersonId = personId,
                        Person = person
                    };
                    primaryEmail2 = new EmailAddress
                    {
                        IsPrimary = true,
                        EmailAddressId = 20,
                        PersonId = personId,
                        Person = person
                    };
                    person.EmailAddresses.Add(primaryEmail1);
                    person.EmailAddresses.Add(primaryEmail2);
                    context.EmailAddressTypes.Add(emailAddressType);
                    context.People.Add(person);
                    context.EmailAddresses.Add(primaryEmail1);
                    context.EmailAddresses.Add(primaryEmail2);
                });
                var user = new User(1);
                var email = "someone@isp.com";
                var emailAddressTypeId = emailAddressType.EmailAddressTypeId;
                var isPrimary = true;
                var newEmail = new NewPersonEmailAddress(user, emailAddressTypeId, email, isPrimary, personId);
                Action beforeTester = () =>
                {
                    Assert.AreEqual(1, context.People.Count());
                    Assert.AreEqual(2, context.EmailAddresses.Count());
                    Assert.IsTrue(primaryEmail1.IsPrimary.Value);
                    Assert.IsTrue(primaryEmail2.IsPrimary.Value);
                };
                Action afterTester = () =>
                {
                    Assert.AreEqual(3, person.EmailAddresses.Count());
                    Assert.AreEqual(2, context.EmailAddresses.Count());
                    //the new email address is added to the person, not the context
                    Assert.IsTrue(person.EmailAddresses.Last().IsPrimary.Value);
                    Assert.IsFalse(primaryEmail1.IsPrimary.Value);
                    Assert.IsFalse(primaryEmail2.IsPrimary.Value);
                };
                context.Revert();
                beforeTester();
                service.Create(newEmail);
                afterTester();

                context.Revert();
                beforeTester();
                await service.CreateAsync(newEmail);
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
                var personId = 1;
                EmailAddressType emailAddressType = new EmailAddressType
                {
                    EmailAddressTypeId = EmailAddressType.Home.Id,
                    EmailAddressTypeName = EmailAddressType.Home.Value
                };
                context.SetupActions.Add(() =>
                {
                    context.EmailAddressTypes.Add(emailAddressType);
                });
                var user = new User(1);
                var email = "someone@isp.com";
                var emailAddressTypeId = emailAddressType.EmailAddressTypeId;
                var isPrimary = true;
                var newEmail = new NewPersonEmailAddress(user, emailAddressTypeId, email, isPrimary, personId);

                context.Revert();
                var message = String.Format("The sociable entity with id [{0}] was not found.", personId);
                Action a = () => service.Create(newEmail);
                Func<Task> f = () => service.CreateAsync(newEmail);
                a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
                f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            }
        }
        #endregion

        #region Update
        [TestMethod]
        public async Task TestUpdate_PersonEmail_CheckProperties()
        {
            var personId = 1;
            Person person = new Person
            {
                PersonId = personId
            };
            EmailAddressType emailAddressType = new EmailAddressType
            {
                EmailAddressTypeId = EmailAddressType.Home.Id,
                EmailAddressTypeName = EmailAddressType.Home.Value
            };
            var creatorId = 1;
            var updatorId = 2;
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var emailAddressId = 1;
            EmailAddress emailAddressToUpdate = null;
            context.SetupActions.Add(() =>
            {
                emailAddressToUpdate = new EmailAddress
                {
                    EmailAddressId = emailAddressId,
                    Person = person,
                    PersonId = person.PersonId
                };
                emailAddressToUpdate.History.CreatedBy = creatorId;
                emailAddressToUpdate.History.RevisedBy = creatorId;
                emailAddressToUpdate.History.CreatedOn = yesterday;
                emailAddressToUpdate.History.RevisedOn = yesterday;
                person.EmailAddresses.Add(emailAddressToUpdate);
                context.EmailAddressTypes.Add(emailAddressType);
                context.People.Add(person);
                context.EmailAddresses.Add(emailAddressToUpdate);
            });
            var updatedEmailModel = new UpdatedEmailAddress(new User(updatorId), emailAddressId, "someone@isp.com", emailAddressType.EmailAddressTypeId, true);

            Action beforeTester = () =>
            {
                Assert.AreEqual(1, context.People.Count());
                Assert.AreEqual(1, context.EmailAddresses.Count());
                Assert.IsNull(emailAddressToUpdate.Address);
            };
            Action afterTester = () =>
            {
                Assert.AreEqual(1, context.People.Count());
                Assert.AreEqual(1, context.EmailAddresses.Count());
                Assert.AreEqual(updatedEmailModel.Audit.User.Id, emailAddressToUpdate.History.RevisedBy);
                DateTimeOffset.UtcNow.Should().BeCloseTo(emailAddressToUpdate.History.RevisedOn, 20000);
                Assert.AreEqual(creatorId, emailAddressToUpdate.History.CreatedBy);
                Assert.AreEqual(yesterday, emailAddressToUpdate.History.CreatedOn);
                Assert.AreEqual(updatedEmailModel.Address, emailAddressToUpdate.Address);
                Assert.AreEqual(updatedEmailModel.IsPrimary, emailAddressToUpdate.IsPrimary);
                Assert.AreEqual(emailAddressType.EmailAddressTypeId, emailAddressToUpdate.EmailAddressTypeId);
            };

            context.Revert();
            beforeTester();
            service.Update(updatedEmailModel);
            afterTester();

            context.Revert();
            beforeTester();
            await service.UpdateAsync(updatedEmailModel);
            afterTester();
        }

        [TestMethod]
        public async Task TestUpdate_PersonEmail_SevisLocked()
        {
            var personId = 1;
            var participantId = 1;
            Person person = new Person
            {
                PersonId = personId
            };
            var participant = new Participant
            {
                ParticipantId = participantId,
                PersonId = person.PersonId,
                ParticipantStatusId = ParticipantStatus.Active.Id
            };
            List<Participant> participants = new List<Participant>();
            participants.Add(participant);
            person.Participations = participants;
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
            };
            participant.ParticipantPerson = participantPerson;

            var queuedToSubmitStatus = new SevisCommStatus
            {
                SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
                SevisCommStatusName = SevisCommStatus.QueuedToSubmit.Value
            };
            var commStatus = new ParticipantPersonSevisCommStatus
            {
                AddedOn = DateTimeOffset.UtcNow,
                BatchId = "batch id",
                Id = 501,
                ParticipantId = participant.ParticipantId,
                ParticipantPerson = participantPerson,
                SevisCommStatus = queuedToSubmitStatus,
                SevisCommStatusId = queuedToSubmitStatus.SevisCommStatusId,
            };

            participantPerson.ParticipantPersonSevisCommStatuses.Add(commStatus);
            
            EmailAddressType emailAddressType = new EmailAddressType
            {
                EmailAddressTypeId = EmailAddressType.Home.Id,
                EmailAddressTypeName = EmailAddressType.Home.Value
            };
            var creatorId = 1;
            var updatorId = 2;
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var emailAddressId = 1;
            EmailAddress emailAddressToUpdate = null;
            context.SetupActions.Add(() =>
            {
                emailAddressToUpdate = new EmailAddress
                {
                    EmailAddressId = emailAddressId,
                    Person = person,
                    PersonId = person.PersonId
                };
                emailAddressToUpdate.History.CreatedBy = creatorId;
                emailAddressToUpdate.History.RevisedBy = creatorId;
                emailAddressToUpdate.History.CreatedOn = yesterday;
                emailAddressToUpdate.History.RevisedOn = yesterday;

                person.EmailAddresses.Add(emailAddressToUpdate);
                context.EmailAddressTypes.Add(emailAddressType);
                context.People.Add(person);
                context.EmailAddresses.Add(emailAddressToUpdate);
                context.Participants.Add(participant);
                context.ParticipantPersons.Add(participantPerson);
                context.ParticipantPersonSevisCommStatuses.Add(commStatus);
            });
            context.Revert();
            var updatedEmailModel = new UpdatedEmailAddress(new User(updatorId), emailAddressId, "someone@isp.com", emailAddressType.EmailAddressTypeId, true);
            
            var message = String.Format("An update was attempted on participant with id [{0}] but should have failed validation.",
                        participant.ParticipantId);
            
            Action a = () => service.Update(updatedEmailModel);
            Func<Task> f = () => service.UpdateAsync(updatedEmailModel);
            a.ShouldThrow<EcaBusinessException>().WithMessage(message);
            f.ShouldThrow<EcaBusinessException>().WithMessage(message);            
        }

        [TestMethod]
        public async Task TestUpdate_PersonEmail_SevisNotLocked()
        {
            var personId = 1;
            var participantId = 1;
            Person person = new Person
            {
                PersonId = personId
            };
            var participant = new Participant
            {
                ParticipantId = participantId,
                PersonId = person.PersonId,
                ParticipantStatusId = ParticipantStatus.Active.Id
            };
            List<Participant> participants = new List<Participant>();
            participants.Add(participant);
            person.Participations = participants;
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
            };
            participant.ParticipantPerson = participantPerson;

            var queuedToSubmitStatus = new SevisCommStatus
            {
                SevisCommStatusId = SevisCommStatus.InformationRequired.Id,
                SevisCommStatusName = SevisCommStatus.InformationRequired.Value
            };
            var commStatus = new ParticipantPersonSevisCommStatus
            {
                AddedOn = DateTimeOffset.UtcNow,
                BatchId = "batch id",
                Id = 501,
                ParticipantId = participant.ParticipantId,
                ParticipantPerson = participantPerson,
                SevisCommStatus = queuedToSubmitStatus,
                SevisCommStatusId = queuedToSubmitStatus.SevisCommStatusId,
            };

            participantPerson.ParticipantPersonSevisCommStatuses.Add(commStatus);

            EmailAddressType emailAddressType = new EmailAddressType
            {
                EmailAddressTypeId = EmailAddressType.Home.Id,
                EmailAddressTypeName = EmailAddressType.Home.Value
            };
            var creatorId = 1;
            var updatorId = 2;
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var emailAddressId = 1;
            EmailAddress emailAddressToUpdate = null;
            context.SetupActions.Add(() =>
            {
                emailAddressToUpdate = new EmailAddress
                {
                    EmailAddressId = emailAddressId,
                    Person = person,
                    PersonId = person.PersonId
                };
                emailAddressToUpdate.History.CreatedBy = creatorId;
                emailAddressToUpdate.History.RevisedBy = creatorId;
                emailAddressToUpdate.History.CreatedOn = yesterday;
                emailAddressToUpdate.History.RevisedOn = yesterday;

                person.EmailAddresses.Add(emailAddressToUpdate);
                context.EmailAddressTypes.Add(emailAddressType);
                context.People.Add(person);
                context.EmailAddresses.Add(emailAddressToUpdate);
                context.Participants.Add(participant);
                context.ParticipantPersons.Add(participantPerson);
                context.ParticipantPersonSevisCommStatuses.Add(commStatus);
            });
            context.Revert();
            var updatedEmailModel = new UpdatedEmailAddress(new User(updatorId), emailAddressId, "someone@isp.com", emailAddressType.EmailAddressTypeId, true);

            var message = String.Format("An update was attempted on participant with id [{0}] but should have failed validation.",
                        participant.ParticipantId);

            Action a = () => service.Update(updatedEmailModel);
            Func<Task> f = () => service.UpdateAsync(updatedEmailModel);
            a.ShouldNotThrow<EcaBusinessException>();
            f.ShouldNotThrow<EcaBusinessException>();
        }

        [TestMethod]
        public async Task TestUpdate_ContactEmail_CheckProperties()
        {
            var contactId = 1;
            Contact contact = new Contact
            {
                ContactId = contactId
            };
            EmailAddressType emailAddressType = new EmailAddressType
            {
                EmailAddressTypeId = EmailAddressType.Home.Id,
                EmailAddressTypeName = EmailAddressType.Home.Value
            };
            var creatorId = 1;
            var updatorId = 2;
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var emailAddressId = 1;
            EmailAddress emailAddressToUpdate = null;
            context.SetupActions.Add(() =>
            {
                emailAddressToUpdate = new EmailAddress
                {
                    EmailAddressId = emailAddressId,
                    Contact = contact,
                    ContactId = contact.ContactId
                };
                emailAddressToUpdate.History.CreatedBy = creatorId;
                emailAddressToUpdate.History.RevisedBy = creatorId;
                emailAddressToUpdate.History.CreatedOn = yesterday;
                emailAddressToUpdate.History.RevisedOn = yesterday;
                contact.EmailAddresses.Add(emailAddressToUpdate);
                context.EmailAddressTypes.Add(emailAddressType);
                context.Contacts.Add(contact);
                context.EmailAddresses.Add(emailAddressToUpdate);
            });
            var updatedEmailModel = new UpdatedEmailAddress(new User(updatorId), emailAddressId, "someone@isp.com", emailAddressType.EmailAddressTypeId, true);

            Action beforeTester = () =>
            {
                Assert.AreEqual(1, context.Contacts.Count());
                Assert.AreEqual(1, context.EmailAddresses.Count());
                Assert.IsNull(emailAddressToUpdate.Address);
            };
            Action afterTester = () =>
            {
                Assert.AreEqual(1, context.Contacts.Count());
                Assert.AreEqual(1, context.EmailAddresses.Count());
                Assert.AreEqual(updatedEmailModel.Audit.User.Id, emailAddressToUpdate.History.RevisedBy);
                DateTimeOffset.UtcNow.Should().BeCloseTo(emailAddressToUpdate.History.RevisedOn, 20000);
                Assert.AreEqual(creatorId, emailAddressToUpdate.History.CreatedBy);
                Assert.AreEqual(yesterday, emailAddressToUpdate.History.CreatedOn);
                Assert.AreEqual(updatedEmailModel.Address, emailAddressToUpdate.Address);
                Assert.AreEqual(updatedEmailModel.IsPrimary, emailAddressToUpdate.IsPrimary);
                Assert.AreEqual(emailAddressType.EmailAddressTypeId, emailAddressToUpdate.EmailAddressTypeId);
            };

            context.Revert();
            beforeTester();
            service.Update(updatedEmailModel);
            afterTester();

            context.Revert();
            beforeTester();
            await service.UpdateAsync(updatedEmailModel);
            afterTester();
        }

        [TestMethod]
        public async Task TestUpdate_OtherEmailsPrimary()
        {
            var contactId = 1;
            Contact contact = new Contact
            {
                ContactId = contactId
            };
            Contact otherContact = new Contact
            {
                ContactId = 2
            };
            EmailAddressType emailAddressType = new EmailAddressType
            {
                EmailAddressTypeId = EmailAddressType.Home.Id,
                EmailAddressTypeName = EmailAddressType.Home.Value
            };
            var creatorId = 1;
            var updatorId = 2;
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var emailAddressId = 1;
            EmailAddress emailAddressToUpdate = null;
            EmailAddress primaryEmail1 = null;
            EmailAddress primaryEmail2 = null;
            EmailAddress otherContactPrimaryEmail = null;
            context.SetupActions.Add(() =>
            {
                emailAddressToUpdate = new EmailAddress
                {
                    EmailAddressId = emailAddressId,
                    Contact = contact,
                    ContactId = contact.ContactId,
                    IsPrimary = false
                };
                primaryEmail1 = new EmailAddress
                {
                    EmailAddressId = 10,
                    IsPrimary = true,
                    Contact = contact,
                    ContactId = contact.ContactId
                };
                primaryEmail2 = new EmailAddress
                {
                    EmailAddressId = 20,
                    IsPrimary = true,
                    Contact = contact,
                    ContactId = contact.ContactId
                };
                otherContactPrimaryEmail = new EmailAddress
                {
                    EmailAddressId = 30,
                    IsPrimary = true,
                    Contact = otherContact,
                    ContactId = otherContact.ContactId
                };
                emailAddressToUpdate.History.CreatedBy = creatorId;
                emailAddressToUpdate.History.RevisedBy = creatorId;
                emailAddressToUpdate.History.CreatedOn = yesterday;
                emailAddressToUpdate.History.RevisedOn = yesterday;
                contact.EmailAddresses.Add(emailAddressToUpdate);
                contact.EmailAddresses.Add(primaryEmail1);
                contact.EmailAddresses.Add(primaryEmail2);
                otherContact.EmailAddresses.Add(otherContactPrimaryEmail);
                context.EmailAddressTypes.Add(emailAddressType);
                context.Contacts.Add(contact);
                context.EmailAddresses.Add(emailAddressToUpdate);
                context.EmailAddresses.Add(primaryEmail1);
                context.EmailAddresses.Add(primaryEmail2);
                context.EmailAddresses.Add(otherContactPrimaryEmail);
            });
            var updatedEmailModel = new UpdatedEmailAddress(new User(updatorId), emailAddressId, "someone@isp.com", emailAddressType.EmailAddressTypeId, true);

            Action beforeTester = () =>
            {
                Assert.AreEqual(1, context.Contacts.Count());
                Assert.AreEqual(4, context.EmailAddresses.Count());
                Assert.IsNull(emailAddressToUpdate.Address);
                Assert.IsTrue(primaryEmail1.IsPrimary.Value);
                Assert.IsTrue(primaryEmail2.IsPrimary.Value);
                Assert.IsTrue(otherContactPrimaryEmail.IsPrimary.Value);
            };
            Action afterTester = () =>
            {
                Assert.AreEqual(1, context.Contacts.Count());
                Assert.AreEqual(4, context.EmailAddresses.Count());
                Assert.IsTrue(emailAddressToUpdate.IsPrimary.Value);
                Assert.IsFalse(primaryEmail1.IsPrimary.Value);
                Assert.IsFalse(primaryEmail2.IsPrimary.Value);
                Assert.IsTrue(otherContactPrimaryEmail.IsPrimary.Value);
            };

            context.Revert();
            beforeTester();
            service.Update(updatedEmailModel);
            afterTester();

            context.Revert();
            beforeTester();
            await service.UpdateAsync(updatedEmailModel);
            afterTester();
        }

        [TestMethod]
        public async Task TestUpdate_EmailAddressDoesNotExist()
        {
            var personId = 1;
            Person person = new Person
            {
                PersonId = personId
            };
            EmailAddressType emailAddressType = new EmailAddressType
            {
                EmailAddressTypeId = EmailAddressType.Home.Id,
                EmailAddressTypeName = EmailAddressType.Home.Value
            };
            var updatorId = 2;
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var emailAddressId = 1;
            context.EmailAddressTypes.Add(emailAddressType);
            context.People.Add(person);
            var updatedEmailModel = new UpdatedEmailAddress(new User(updatorId), emailAddressId, "someone@isp.com", emailAddressType.EmailAddressTypeId, true);
            var message = String.Format("The email address with id [{0}] was not found.", emailAddressId);
            Action a = () => service.Update(updatedEmailModel);
            Func<Task> f = () => service.UpdateAsync(updatedEmailModel);
            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }
        #endregion

        #region Delete
        [TestMethod]
        public async Task TestDelete()
        {
            var emailToDelete = new EmailAddress
            {
                EmailAddressId = 1
            };
            context.SetupActions.Add(() =>
            {
                context.EmailAddresses.Add(emailToDelete);
            });
            Action beforeTester = () =>
            {
                Assert.AreEqual(1, context.EmailAddresses.Count());
            };
            Action afterTester = () =>
            {
                Assert.AreEqual(0, context.EmailAddresses.Count());
            };
            context.Revert();
            beforeTester();
            service.Delete(emailToDelete.EmailAddressId);
            afterTester();

            context.Revert();
            beforeTester();
            await service.DeleteAsync(emailToDelete.EmailAddressId);
            afterTester();
        }

        [TestMethod]
        public async Task TestDelete_EmailAddressDoesNotExist()
        {
            Action beforeTester = () =>
            {
                Assert.AreEqual(0, context.EmailAddresses.Count());
            };
            Action afterTester = () =>
            {
                Assert.AreEqual(0, context.EmailAddresses.Count());
            };
            context.Revert();
            beforeTester();
            service.Delete(1);
            afterTester();

            context.Revert();
            beforeTester();
            await service.DeleteAsync(1);
            afterTester();
        }
        #endregion
    }
}
