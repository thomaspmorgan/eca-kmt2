using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Admin;
using ECA.Data;
using System.Threading.Tasks;
using ECA.Core.Exceptions;
using FluentAssertions;
using ECA.Business.Exceptions;

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

        [TestMethod]
        public async Task TestCreateDataPointConfigurationAsync()
        {
            var organizationType = new OrganizationType
            {
                OrganizationTypeId = OrganizationType.Office.Id,
                OrganizationTypeName = OrganizationType.Office.Value
            };

            var office = new Organization
            {
                OrganizationId = 1,
                OrganizationType = organizationType
            };

            context.Organizations.Add(office);

            var newDataPointConfiguration = new NewDataPointConfiguration(office.OrganizationId, null, null, 1);
            var dataPointConfiguration = await service.CreateDataPointConfigurationAsync(newDataPointConfiguration);

            Assert.AreEqual(newDataPointConfiguration.OfficeId, dataPointConfiguration.OfficeId);
            Assert.AreEqual(newDataPointConfiguration.DataPointCategoryPropertyId, dataPointConfiguration.DataPointCategoryPropertyId);
        }

        [TestMethod]
        public async Task TestCreateDataPointConfigurationAsync_ModelHasNoResourceId()
        {
            var newDataPointConfiguration = new NewDataPointConfiguration(null, null, null, 1);
            Func<Task> act = async () => { await service.CreateDataPointConfigurationAsync(newDataPointConfiguration); };
            act.ShouldThrow<EcaBusinessException>()
                .WithMessage(DataPointConfigurationService.MODEL_HAS_LESS_THAN_OR_MORE_THAN_ONE_RESOURCE_ID_ERROR);
        }

        [TestMethod]
        public async Task TestCreateDataPointConfigurationAsync_ModelHasMoreThanOneResourceId()
        {
            var newDataPointConfiguration = new NewDataPointConfiguration(1, 1, null, 1);
            Func<Task> act = async () => { await service.CreateDataPointConfigurationAsync(newDataPointConfiguration); };
            act.ShouldThrow<EcaBusinessException>()
                .WithMessage(DataPointConfigurationService.MODEL_HAS_LESS_THAN_OR_MORE_THAN_ONE_RESOURCE_ID_ERROR);
        }

        [TestMethod]
        public async Task TestCreateDataPointConfigurationAsync_DataPointConfigurationAlreadyExists()
        {
            var dataPointConfiguration = new DataPointConfiguration
            {
                OfficeId = 1,
                DataPointCategoryPropertyId = 1
            };

            context.DataPointConfigurations.Add(dataPointConfiguration);

            var newDataPointConfiguration = new NewDataPointConfiguration(1, null, null, 1);
            Func<Task> act = async () => { await service.CreateDataPointConfigurationAsync(newDataPointConfiguration); };
            act.ShouldThrow<EcaBusinessException>()
                .WithMessage(DataPointConfigurationService.DATA_POINT_CONFIGURATION_ALREADY_EXISTS_ERROR);
        }
    }
}
