using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service;
using ECA.Data;
using ECA.Business.Service.Persons;
using ECA.Core.Exceptions;

namespace ECA.Business.Test.Service.Persons
{
    [TestClass]
    public class UpdatedParticipantPersonTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var user = new User(1);
            var participantId = 100;
            var projectId = 1;
            var hostInstitutionId = 200;
            var hostInstitutionAddressId = 300;
            var homeInstitutionId = 400;
            var homeInstitutionAddressId = 500;
            var placementOrganizationId = 600;
            var placementOrganizationAddressId = 700;
            var participantTypeId = ParticipantType.Individual.Id;
            var participantStatusId = ParticipantStatus.Active.Id;
            Assert.AreNotEqual(participantTypeId, participantStatusId);
            var instance = new UpdatedParticipantPerson(
                user,
                participantId,
                projectId,
                hostInstitutionId,
                homeInstitutionId,
                hostInstitutionAddressId,
                homeInstitutionAddressId,
                participantTypeId,
                participantStatusId,
                placementOrganizationId,
                placementOrganizationAddressId
                );

            Assert.AreEqual(homeInstitutionAddressId, instance.HomeInstitutionAddressId);
            Assert.AreEqual(homeInstitutionId, instance.HomeInstitutionId);
            Assert.AreEqual(hostInstitutionAddressId, instance.HostInstitutionAddressId);
            Assert.AreEqual(hostInstitutionId, instance.HostInstitutionId);
            Assert.AreEqual(participantId, instance.ParticipantId);
            Assert.AreEqual(participantStatusId, instance.ParticipantStatusId);
            Assert.AreEqual(participantTypeId, instance.ParticipantTypeId);
            Assert.AreEqual(projectId, instance.ProjectId);

            Assert.IsTrue(object.ReferenceEquals(user, instance.Audit.User));
            DateTimeOffset.UtcNow.Should().BeCloseTo(instance.Audit.Date, 20000);
        }

        [TestMethod]
        public void TestConstructor_UnknownParticipantTypeId()
        {
            var user = new User(1);
            var participantId = 100;
            var projectId = 1;
            var hostInstitutionId = 200;
            var hostInstitutionAddressId = 300;
            var homeInstitutionId = 400;
            var homeInstitutionAddressId = 500;
            var placementOrganizationId = 600;
            var placementOrganizationAddressId = 700;
            var participantTypeId = -1;
            var participantStatusId = ParticipantStatus.Active.Id;
            Assert.AreNotEqual(participantTypeId, participantStatusId);
            Action a = () => new UpdatedParticipantPerson(
                user,
                projectId,
                participantId,
                hostInstitutionId,
                homeInstitutionId,
                hostInstitutionAddressId,
                homeInstitutionAddressId,
                participantTypeId,
                participantStatusId,
                placementOrganizationId,
                placementOrganizationAddressId
                );

            var message = String.Format("The participant type id [{0}] is not recognized.", participantTypeId);
            a.ShouldThrow<UnknownStaticLookupException>().WithMessage(message);
        }

        [TestMethod]
        public void TestConstructor_UnknownParticipantStatusId()
        {
            var user = new User(1);
            var participantId = 100;
            var projectId = 1;
            var hostInstitutionId = 200;
            var hostInstitutionAddressId = 300;
            var homeInstitutionId = 400;
            var homeInstitutionAddressId = 500;
            var placementOrganizationId = 600;
            var placementOrganizationAddressId = 700;
            var participantTypeId = ParticipantType.Individual.Id;
            var participantStatusId = -1;
            Assert.AreNotEqual(participantTypeId, participantStatusId);
            Action a = () => new UpdatedParticipantPerson(
                user,
                projectId,
                participantId,
                hostInstitutionId,
                homeInstitutionId,
                hostInstitutionAddressId,
                homeInstitutionAddressId,
                participantTypeId,
                participantStatusId,
                placementOrganizationId,
                placementOrganizationAddressId
                );

            var message = String.Format("The participant status id [{0}] is not recognized.", participantStatusId);
            a.ShouldThrow<UnknownStaticLookupException>().WithMessage(message);
        }

        [TestMethod]
        public void TestConstructor_DoesNotHaveParticipantStatusId()
        {
            var user = new User(1);
            var participantId = 100;
            var projectId = 1;
            var hostInstitutionId = 200;
            var hostInstitutionAddressId = 300;
            var homeInstitutionId = 400;
            var homeInstitutionAddressId = 500;
            var placementOrganizationId = 600;
            var placementOrganizationAddressId = 700;
            var participantTypeId = ParticipantType.Individual.Id;
            int? participantStatusId = null;
            Assert.AreNotEqual(participantTypeId, participantStatusId);
            Action a = () => new UpdatedParticipantPerson(
                user,
                projectId,
                participantId,
                hostInstitutionId,
                homeInstitutionId,
                hostInstitutionAddressId,
                homeInstitutionAddressId,
                participantTypeId,
                participantStatusId,
                placementOrganizationId,
                placementOrganizationAddressId
                );

            a.ShouldNotThrow<UnknownStaticLookupException>();
        }
    }
}
