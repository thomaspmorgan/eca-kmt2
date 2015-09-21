using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Fundings;
using System.Threading.Tasks;
using ECA.Data;
using ECA.Core.Query;
using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using System.Collections.Generic;

namespace ECA.Business.Test.Service.Fundings
{
    [TestClass]
    public class MoneyFlowSourceRecipientTypeServiceTest
    {
        private TestEcaContext context;
        private MoneyFlowSourceRecipientTypeService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new MoneyFlowSourceRecipientTypeService(context);
        }

        [TestCleanup]
        public void TestCleanup()
        {

        }

        #region Get
        [TestMethod]
        public async Task TestGet_CheckProperties()
        {
            var moneyFlowSourceRecipientType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = 1,
                TypeName= "type"
                
            };
            context.MoneyFlowSourceRecipientTypes.Add(moneyFlowSourceRecipientType);
            Action<PagedQueryResults<MoneyFlowSourceRecipientTypeDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(moneyFlowSourceRecipientType.MoneyFlowSourceRecipientTypeId, firstResult.Id);
                Assert.AreEqual(moneyFlowSourceRecipientType.TypeName, firstResult.Name);
            };
            var defaultSorter = new ExpressionSorter<MoneyFlowSourceRecipientTypeDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<MoneyFlowSourceRecipientTypeDTO>(0, 10, defaultSorter);

            var serviceResults = service.Get(queryOperator);
            var serviceResultsAsync = await service.GetAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetRecipientMoneyFlowTypes_CheckProperties()
        {
            var moneyFlowSourceRecipientType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = 1,
                TypeName = "type"
            };
            var peerMoneyFlowSourceRecipientType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = 2,
                TypeName = "peer type"
            };
            var setting = new MoneyFlowSourceRecipientTypeSetting
            {
                Id = 1,
                IsReceipient = true,
                MoneyFlowSourceRecipientType = moneyFlowSourceRecipientType,
                MoneyFlowSourceRecipientTypeId = moneyFlowSourceRecipientType.MoneyFlowSourceRecipientTypeId,
                PeerMoneyFlowSourceRecipientType = peerMoneyFlowSourceRecipientType,
                PeerMoneyFlowSourceRecipientTypeId = peerMoneyFlowSourceRecipientType.MoneyFlowSourceRecipientTypeId
            };
            moneyFlowSourceRecipientType.MoneyFlowSourceRecipientTypeSettings.Add(setting);
            context.MoneyFlowSourceRecipientTypes.Add(moneyFlowSourceRecipientType);
            context.MoneyFlowSourceRecipientTypes.Add(peerMoneyFlowSourceRecipientType);
            context.MoneyFlowSourceRecipientTypeSettings.Add(setting);

            Action<List<MoneyFlowSourceRecipientTypeDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Count);
                var firstResult = results.First();
                Assert.AreEqual(peerMoneyFlowSourceRecipientType.MoneyFlowSourceRecipientTypeId, firstResult.Id);
                Assert.AreEqual(peerMoneyFlowSourceRecipientType.TypeName, firstResult.Name);
            };

            var serviceResults = service.GetRecipientMoneyFlowTypes(moneyFlowSourceRecipientType.MoneyFlowSourceRecipientTypeId);
            var serviceResultsAsync = await service.GetRecipientMoneyFlowTypesAsync(moneyFlowSourceRecipientType.MoneyFlowSourceRecipientTypeId);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetRecipientMoneyFlowTypes_NoRecipientSettings()
        {
            var moneyFlowSourceRecipientType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = 1,
                TypeName = "type"
            };
            var peerMoneyFlowSourceRecipientType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = 2,
                TypeName = "peer type"
            };
            var setting = new MoneyFlowSourceRecipientTypeSetting
            {
                Id = 1,
                IsReceipient = false,
                MoneyFlowSourceRecipientType = moneyFlowSourceRecipientType,
                MoneyFlowSourceRecipientTypeId = moneyFlowSourceRecipientType.MoneyFlowSourceRecipientTypeId,
                PeerMoneyFlowSourceRecipientType = peerMoneyFlowSourceRecipientType,
                PeerMoneyFlowSourceRecipientTypeId = peerMoneyFlowSourceRecipientType.MoneyFlowSourceRecipientTypeId
            };
            moneyFlowSourceRecipientType.MoneyFlowSourceRecipientTypeSettings.Add(setting);
            context.MoneyFlowSourceRecipientTypes.Add(moneyFlowSourceRecipientType);
            context.MoneyFlowSourceRecipientTypes.Add(peerMoneyFlowSourceRecipientType);
            context.MoneyFlowSourceRecipientTypeSettings.Add(setting);

            Action<List<MoneyFlowSourceRecipientTypeDTO>> tester = (results) =>
            {
                Assert.AreEqual(0, results.Count);
            };

            var serviceResults = service.GetRecipientMoneyFlowTypes(moneyFlowSourceRecipientType.MoneyFlowSourceRecipientTypeId);
            var serviceResultsAsync = await service.GetRecipientMoneyFlowTypesAsync(moneyFlowSourceRecipientType.MoneyFlowSourceRecipientTypeId);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetRecipientMoneyFlowTypes_NoSettings()
        {
            var moneyFlowSourceRecipientType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = 1,
                TypeName = "type"
            };
            var peerMoneyFlowSourceRecipientType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = 2,
                TypeName = "peer type"
            };
            context.MoneyFlowSourceRecipientTypes.Add(moneyFlowSourceRecipientType);
            context.MoneyFlowSourceRecipientTypes.Add(peerMoneyFlowSourceRecipientType);

            Action<List<MoneyFlowSourceRecipientTypeDTO>> tester = (results) =>
            {
                Assert.AreEqual(0, results.Count);
            };

            var serviceResults = service.GetRecipientMoneyFlowTypes(moneyFlowSourceRecipientType.MoneyFlowSourceRecipientTypeId);
            var serviceResultsAsync = await service.GetRecipientMoneyFlowTypesAsync(moneyFlowSourceRecipientType.MoneyFlowSourceRecipientTypeId);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetRecipientMoneyFlowTypes_MoneyFlowSourceRecipientTypeDoesNotExist()
        {
            Action<List<MoneyFlowSourceRecipientTypeDTO>> tester = (results) =>
            {
                Assert.AreEqual(0, results.Count);
            };

            var serviceResults = service.GetRecipientMoneyFlowTypes(0);
            var serviceResultsAsync = await service.GetRecipientMoneyFlowTypesAsync(0);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetSourceMoneyFlowTypes_CheckProperties()
        {
            var moneyFlowSourceRecipientType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = 1,
                TypeName = "type"
            };
            var peerMoneyFlowSourceRecipientType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = 2,
                TypeName = "peer type"
            };
            var setting = new MoneyFlowSourceRecipientTypeSetting
            {
                Id = 1,
                IsSource = true,
                MoneyFlowSourceRecipientType = moneyFlowSourceRecipientType,
                MoneyFlowSourceRecipientTypeId = moneyFlowSourceRecipientType.MoneyFlowSourceRecipientTypeId,
                PeerMoneyFlowSourceRecipientType = peerMoneyFlowSourceRecipientType,
                PeerMoneyFlowSourceRecipientTypeId = peerMoneyFlowSourceRecipientType.MoneyFlowSourceRecipientTypeId
            };
            moneyFlowSourceRecipientType.MoneyFlowSourceRecipientTypeSettings.Add(setting);
            context.MoneyFlowSourceRecipientTypes.Add(moneyFlowSourceRecipientType);
            context.MoneyFlowSourceRecipientTypes.Add(peerMoneyFlowSourceRecipientType);
            context.MoneyFlowSourceRecipientTypeSettings.Add(setting);

            Action<List<MoneyFlowSourceRecipientTypeDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Count);
                var firstResult = results.First();
                Assert.AreEqual(peerMoneyFlowSourceRecipientType.MoneyFlowSourceRecipientTypeId, firstResult.Id);
                Assert.AreEqual(peerMoneyFlowSourceRecipientType.TypeName, firstResult.Name);
            };

            var serviceResults = service.GetSourceMoneyFlowTypes(moneyFlowSourceRecipientType.MoneyFlowSourceRecipientTypeId);
            var serviceResultsAsync = await service.GetSourceMoneyFlowTypesAsync(moneyFlowSourceRecipientType.MoneyFlowSourceRecipientTypeId);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetSourceMoneyFlowTypes_NoSources()
        {
            var moneyFlowSourceRecipientType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = 1,
                TypeName = "type"
            };
            var peerMoneyFlowSourceRecipientType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = 2,
                TypeName = "peer type"
            };
            var setting = new MoneyFlowSourceRecipientTypeSetting
            {
                Id = 1,
                IsSource = false,
                MoneyFlowSourceRecipientType = moneyFlowSourceRecipientType,
                MoneyFlowSourceRecipientTypeId = moneyFlowSourceRecipientType.MoneyFlowSourceRecipientTypeId,
                PeerMoneyFlowSourceRecipientType = peerMoneyFlowSourceRecipientType,
                PeerMoneyFlowSourceRecipientTypeId = peerMoneyFlowSourceRecipientType.MoneyFlowSourceRecipientTypeId
            };
            moneyFlowSourceRecipientType.MoneyFlowSourceRecipientTypeSettings.Add(setting);
            context.MoneyFlowSourceRecipientTypes.Add(moneyFlowSourceRecipientType);
            context.MoneyFlowSourceRecipientTypes.Add(peerMoneyFlowSourceRecipientType);
            context.MoneyFlowSourceRecipientTypeSettings.Add(setting);

            Action<List<MoneyFlowSourceRecipientTypeDTO>> tester = (results) =>
            {
                Assert.AreEqual(0, results.Count);
            };

            var serviceResults = service.GetSourceMoneyFlowTypes(moneyFlowSourceRecipientType.MoneyFlowSourceRecipientTypeId);
            var serviceResultsAsync = await service.GetSourceMoneyFlowTypesAsync(moneyFlowSourceRecipientType.MoneyFlowSourceRecipientTypeId);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetSourceMoneyFlowTypes_NoSettings()
        {
            var moneyFlowSourceRecipientType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = 1,
                TypeName = "type"
            };
            var peerMoneyFlowSourceRecipientType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = 2,
                TypeName = "peer type"
            };
            context.MoneyFlowSourceRecipientTypes.Add(moneyFlowSourceRecipientType);
            context.MoneyFlowSourceRecipientTypes.Add(peerMoneyFlowSourceRecipientType);           

            Action<List<MoneyFlowSourceRecipientTypeDTO>> tester = (results) =>
            {
                Assert.AreEqual(0, results.Count);
            };

            var serviceResults = service.GetSourceMoneyFlowTypes(moneyFlowSourceRecipientType.MoneyFlowSourceRecipientTypeId);
            var serviceResultsAsync = await service.GetSourceMoneyFlowTypesAsync(moneyFlowSourceRecipientType.MoneyFlowSourceRecipientTypeId);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetSourceMoneyFlowTypes_MoneyFlowSourceRecipientTypeDoesNotExist()
        {
            Action<List<MoneyFlowSourceRecipientTypeDTO>> tester = (results) =>
            {
                Assert.AreEqual(0, results.Count);
            };

            var serviceResults = service.GetSourceMoneyFlowTypes(0);
            var serviceResultsAsync = await service.GetSourceMoneyFlowTypesAsync(0);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        #endregion
    }
}
