using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Programs;
using System.Collections.Generic;

namespace ECA.WebApi.Test.Models.Programs
{
    [TestClass]
    public class DraftProgramBindingModelTest
    {
        [TestMethod]
        public void TestToDraftProgram()
        {
            var model = new DraftProgramBindingModel();
            model.Description = "desc";
            model.EndDate = DateTimeOffset.UtcNow;
            model.FocusId = 100;
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

            var draftProgram = model.ToDraftProgram(userId);
            Assert.AreEqual(model.Description, draftProgram.Description);
            Assert.AreEqual(model.EndDate, draftProgram.EndDate);
            Assert.AreEqual(model.FocusId, draftProgram.FocusId);
            Assert.AreEqual(model.Name, draftProgram.Name);
            Assert.AreEqual(model.OwnerOrganizationId, draftProgram.OwnerOrganizationId);
            Assert.AreEqual(model.ParentProgramId, draftProgram.ParentProgramId);
            Assert.AreEqual(model.StartDate, draftProgram.StartDate);
            Assert.AreEqual(model.Website, draftProgram.Website);

            CollectionAssert.AreEqual(model.Goals, draftProgram.GoalIds);
            CollectionAssert.AreEqual(model.Contacts, draftProgram.ContactIds);
            CollectionAssert.AreEqual(model.Themes, draftProgram.ThemeIds);
            CollectionAssert.AreEqual(model.Regions, draftProgram.RegionIds);
            Assert.AreEqual(userId, draftProgram.Audit.User.Id);

        }
    }
}
