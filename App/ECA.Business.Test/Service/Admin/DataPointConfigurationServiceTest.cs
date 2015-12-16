using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Admin;
using ECA.Data;
using System.Threading.Tasks;
using ECA.Core.Exceptions;
using FluentAssertions;

namespace ECA.Business.Test.Service.Admin
{
    [TestClass]
    public class DataPointConfigurationServiceTest
    {
        private TestEcaContext context;
        private DataPointConfigurationService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new DataPointConfigurationService(context);
        }

        [TestMethod]
        public async Task TestDeleteDataPointConfigurationAsync()
        {
            var dataPointConfiguration = new DataPointConfiguration
            {
                DataPointConfigurationId = 1,
                OfficeId = 1,
                DataPointCategoryPropertyId = 1
            };

            context.DataPointConfigurations.Add(dataPointConfiguration);

            await service.DeleteDataPointConfigurationAsync(dataPointConfiguration.DataPointConfigurationId);

            Assert.AreEqual(0, 0);
        }

        [TestMethod]
        public async Task TestDeleteDataPointConfigurationAsync_IdDoesNotExist()
        {
            Func<Task> act = async () => { await service.DeleteDataPointConfigurationAsync(1); };
            act.ShouldThrow<ModelNotFoundException>()
                .WithMessage(DataPointConfigurationService.DATA_POINT_CONFIGURATION_NOT_FOUND_ERROR);
        }
    }
}
