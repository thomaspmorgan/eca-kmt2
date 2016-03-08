using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Person;
using ECA.Business.Service;

namespace ECA.WebApi.Test.Models.Person
{
    [TestClass]
    public class UpdatedParticipantExchangeVisitorBindingModelTest
    {
        [TestMethod]
        public void Test()
        {
            var model = new UpdatedParticipantExchangeVisitorBindingModel();
            model.FieldOfStudyId = 1;
            model.FundingGovtAgency1 = 2.0m;
            model.FundingGovtAgency2 = 3.0m;
            model.FundingIntlOrg1 = 4.0m;
            model.FundingIntlOrg2 = 5.0m;
            model.FundingOther = 6.0m;
            model.FundingPersonal = 7.0m;
            model.FundingSponsor = 8.0m;
            model.FundingTotal = 9.0m;
            model.FundingVisBNC = 10.0m;
            model.FundingVisGovt = 11.0m;
            model.GovtAgency1Id = 4;
            model.GovtAgency1OtherName = "other name";
            model.GovtAgency2Id = 5;
            model.GovtAgency2OtherName = "other other name";
            model.IntlOrg1Id = 6;
            model.IntlOrg1OtherName = "other other other name";
            model.IntlOrg2Id = 7;
            model.IntlOrg2OtherName = "other name 2";
            model.OtherName = "other name again";
            model.ParticipantId = 8;
            model.PositionId = 9;
            model.ProgramCategoryId = 10;

            var projectId = 100;
            var user = new User(1);
            var instance = model.ToUpdatedParticipantExchangeVisitor(user, projectId);
            Assert.AreEqual(model.FieldOfStudyId, instance.FieldOfStudyId);
            Assert.AreEqual(model.FundingGovtAgency1, instance.FundingGovtAgency1);
            Assert.AreEqual(model.FundingGovtAgency2, instance.FundingGovtAgency2);
            Assert.AreEqual(model.FundingIntlOrg1, instance.FundingIntlOrg1);
            Assert.AreEqual(model.FundingIntlOrg2, instance.FundingIntlOrg2);
            Assert.AreEqual(model.FundingOther, instance.FundingOther);
            Assert.AreEqual(model.FundingPersonal, instance.FundingPersonal);
            Assert.AreEqual(model.FundingSponsor, instance.FundingSponsor);
            Assert.AreEqual(model.FundingTotal, instance.FundingTotal);
            Assert.AreEqual(model.FundingVisBNC, instance.FundingVisBNC);
            Assert.AreEqual(model.FundingVisGovt, instance.FundingVisGovt);
            Assert.AreEqual(model.GovtAgency1Id, instance.GovtAgency1Id);
            Assert.AreEqual(model.GovtAgency1OtherName, instance.GovtAgency1OtherName);
            Assert.AreEqual(model.GovtAgency2Id, instance.GovtAgency2Id);
            Assert.AreEqual(model.GovtAgency2OtherName, instance.GovtAgency2OtherName);
            Assert.AreEqual(model.IntlOrg1Id, instance.IntlOrg1Id);
            Assert.AreEqual(model.IntlOrg1OtherName, instance.IntlOrg1OtherName);
            Assert.AreEqual(model.IntlOrg2Id, instance.IntlOrg2Id);
            Assert.AreEqual(model.IntlOrg2OtherName, instance.IntlOrg2OtherName);
            Assert.AreEqual(model.OtherName, instance.OtherName);
            Assert.AreEqual(model.ParticipantId, instance.ParticipantId);
            Assert.AreEqual(model.PositionId, instance.PositionId);
            Assert.AreEqual(model.ProgramCategoryId, instance.ProgramCategoryId);
            Assert.AreEqual(projectId, instance.ProjectId);
            Assert.AreEqual(user.Id, instance.Audit.User.Id);
        }
    }
}
