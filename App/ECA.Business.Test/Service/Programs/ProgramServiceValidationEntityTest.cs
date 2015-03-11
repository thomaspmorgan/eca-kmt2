﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Programs;
using System.Collections.Generic;
using ECA.Data;

namespace ECA.Business.Test.Service.Programs
{
    [TestClass]
    public class ProgramServiceValidationEntityTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var regionIds = new List<int> { 1 };
            var focus = new Focus();
            var owner = new Organization();
            var parentProgramId = 2;
            var parentProgram = new Program();
            var name = "hello";
            var description = "desc";
            var contactIds = new List<int>();
            var entity = new ProgramServiceValidationEntity(
                name: name,
                description: description,
                regionLocationTypeIds: regionIds,
                contactIds: contactIds,
                focus: focus,
                owner: owner,
                parentProgramId: parentProgramId, 
                parentProgram: parentProgram
                );

            Assert.IsTrue(Object.ReferenceEquals(regionIds, entity.RegionLocationTypeIds));
            Assert.IsTrue(Object.ReferenceEquals(focus, entity.Focus));
            Assert.IsTrue(Object.ReferenceEquals(owner, entity.OwnerOrganization));
            Assert.IsTrue(Object.ReferenceEquals(parentProgram, entity.ParentProgram));
            Assert.IsTrue(Object.ReferenceEquals(name, entity.Name));
            Assert.IsTrue(Object.ReferenceEquals(description, entity.Description));
            Assert.IsTrue(Object.ReferenceEquals(contactIds, entity.ContactIds));

            Assert.AreEqual(parentProgramId, entity.ParentProgramId);
        }
    }
}