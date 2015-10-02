﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Search;

namespace ECA.Business.Test.Search
{
    [TestClass]
    public class ProjectDTODocumentConfigurationTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var instance = new ProjectDTODocumentConfiguration();
            Assert.AreEqual(ProjectDTODocumentConfiguration.PROJECT_DTO_DOCUMENT_TYPE_ID, instance.DocumentTypeId);
            Assert.AreEqual(ProjectDTODocumentConfiguration.PROJECT_DOCUMENT_TYPE_NAME, instance.DocumentTypeName);
            Assert.IsNotNull(instance.IdDelegate);
            Assert.IsNotNull(instance.NameDelegate);
            Assert.IsNotNull(instance.DescriptionDelegate);
            Assert.IsNotNull(instance.FociDelegate);
            Assert.IsNotNull(instance.GoalsDelegate);
            Assert.IsNotNull(instance.OfficeSymbolDelegate);
            Assert.IsNotNull(instance.PointsOfContactDelegate);
            Assert.IsNotNull(instance.ThemesDelegate);
            Assert.IsNotNull(instance.DocumentTypeId);
            Assert.IsNotNull(instance.DocumentTypeName);
        }
    }
}
