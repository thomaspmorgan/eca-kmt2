using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Programs;
using System.Collections.Generic;
using ECA.Data;

namespace ECA.WebApi.Test.Models.Programs
{
    [TestClass]
    public class ProgramBindingModelTest
    {
        [TestMethod]
        public void TestToEcaProgram()
        {
            var model = new ProgramBindingModel();
            model.Id = 100;
            model.ProgramStatusId = ProgramStatus.Active.Id;
            model.Description = "desc";
            model.EndDate = DateTimeOffset.UtcNow;
            model.Focus = "focus";
            model.Goals = new List<int> { 1 };
            model.Name = "name";
            model.OwnerOrganizationId = 2;
            model.ParentProgramId = 3;
            model.Contacts = new List<int> { 2 };
            model.StartDate = DateTimeOffset.UtcNow;
            model.Themes = new List<int> { 3 };
            model.Regions = new List<int> { 4 };
            model.Website = "website";
            var userId = 1;

            var ecaProgram = model.ToEcaProgram(userId);
            Assert.AreEqual(model.Description, ecaProgram.Description);
            Assert.AreEqual(model.EndDate, ecaProgram.EndDate);
            Assert.AreEqual(model.Focus, ecaProgram.Focus);
            Assert.AreEqual(model.Name, ecaProgram.Name);
            Assert.AreEqual(model.OwnerOrganizationId, ecaProgram.OwnerOrganizationId);
            Assert.AreEqual(model.ParentProgramId, ecaProgram.ParentProgramId);
            Assert.AreEqual(model.Id, ecaProgram.Id);
            Assert.AreEqual(model.ProgramStatusId, ecaProgram.ProgramStatusId);
            Assert.AreEqual(model.StartDate, ecaProgram.StartDate);
            Assert.AreEqual(model.Website, ecaProgram.Website);

            CollectionAssert.AreEqual(model.Goals, ecaProgram.GoalIds);
            CollectionAssert.AreEqual(model.Contacts, ecaProgram.ContactIds);
            CollectionAssert.AreEqual(model.Themes, ecaProgram.ThemeIds);
            CollectionAssert.AreEqual(model.Regions, ecaProgram.RegionIds);
            Assert.AreEqual(userId, ecaProgram.Audit.User.Id);

        }
    }
}
