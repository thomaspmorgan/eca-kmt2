using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Admin;
using ECA.Data;
using System.Threading.Tasks;
using ECA.Core.Exceptions;
using FluentAssertions;
using ECA.Business.Exceptions;
using ECA.Business.Queries.Models.Office;
using System.Collections.Generic;
using Microsoft.QualityTools.Testing.Fakes;
using System.Linq;

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

        [TestMethod]
        public async Task TestGetParentOfficeIds_HasOneParentOffice()
        {
            using (ShimsContext.Create())
            {
                var list = new List<SimpleOfficeDTO>();

                var dto1 = new SimpleOfficeDTO
                {
                    OrganizationId = 1,
                    OrganizationTypeId = OrganizationType.Office.Id,
                    OrganizationType = OrganizationType.Office.Value,
                    OfficeSymbol = "eca",
                    Name = "org 1",
                    Description = "description",
                    Path = "1",
                    OfficeLevel = 1

                };
                var dto2 = new SimpleOfficeDTO
                {
                    OrganizationId = 2,
                    OrganizationTypeId = OrganizationType.Office.Id,
                    OrganizationType = OrganizationType.Office.Value,
                    ParentOrganization_OrganizationId = 1,
                    OfficeSymbol = "eca",
                    Name = "org 2",
                    Description = "description",
                    Path = "1-1",
                    OfficeLevel = 2
                };
                list.Add(dto1);
                list.Add(dto2);
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.DatabaseGet = (c) =>
                {
                    var shimDb = new System.Data.Entity.Fakes.ShimDatabase();
                    shimDb.SqlQueryOf1StringObjectArray<SimpleOfficeDTO>(
                        (sql, parameters) =>
                        {
                            var shimDbSql = new System.Data.Entity.Infrastructure.Fakes.ShimDbRawSqlQuery<SimpleOfficeDTO>();
                            shimDbSql.ToArrayAsync = () =>
                            {
                                return Task.FromResult<SimpleOfficeDTO[]>(list.ToArray());
                            };
                            return shimDbSql;
                        }
                    );
                    return shimDb;
                };
                System.Linq.Fakes.ShimEnumerable.ToArrayOf1IEnumerableOfM0<SimpleOfficeDTO>((e) =>
                {
                    return list.ToArray();
                });

                var serviceResults = await service.GetParentOfficeIds(dto2.OrganizationId);
                Assert.AreEqual(1, serviceResults.Count);
                CollectionAssert.AreEqual(new List<int> { dto1.OrganizationId }, serviceResults);
            }
        }

        [TestMethod]
        public async Task TestGetParentOfficeIds_HasTwoParentOffices()
        {
            using (ShimsContext.Create())
            {
                var list = new List<SimpleOfficeDTO>();

                var dto1 = new SimpleOfficeDTO
                {
                    OrganizationId = 1,
                    OrganizationTypeId = OrganizationType.Office.Id,
                    OrganizationType = OrganizationType.Office.Value,
                    OfficeSymbol = "eca",
                    Name = "org 1",
                    Description = "description",
                    Path = "1",
                    OfficeLevel = 1

                };
                var dto2 = new SimpleOfficeDTO
                {
                    OrganizationId = 2,
                    OrganizationTypeId = OrganizationType.Office.Id,
                    OrganizationType = OrganizationType.Office.Value,
                    ParentOrganization_OrganizationId = 1,
                    OfficeSymbol = "eca",
                    Name = "org 2",
                    Description = "description",
                    Path = "1-1",
                    OfficeLevel = 2
                };
                var dto3 = new SimpleOfficeDTO
                {
                    OrganizationId = 3,
                    OrganizationTypeId = OrganizationType.Office.Id,
                    OrganizationType = OrganizationType.Office.Value,
                    ParentOrganization_OrganizationId = 2,
                    OfficeSymbol = "eca",
                    Name = "org 3",
                    Description = "description",
                    Path = "1-1-1",
                    OfficeLevel = 3
                };
                list.Add(dto1);
                list.Add(dto2);
                list.Add(dto3);
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.DatabaseGet = (c) =>
                {
                    var shimDb = new System.Data.Entity.Fakes.ShimDatabase();
                    shimDb.SqlQueryOf1StringObjectArray<SimpleOfficeDTO>(
                        (sql, parameters) =>
                        {
                            var shimDbSql = new System.Data.Entity.Infrastructure.Fakes.ShimDbRawSqlQuery<SimpleOfficeDTO>();
                            shimDbSql.ToArrayAsync = () =>
                            {
                                return Task.FromResult<SimpleOfficeDTO[]>(list.ToArray());
                            };
                            return shimDbSql;
                        }
                    );
                    return shimDb;
                };
                System.Linq.Fakes.ShimEnumerable.ToArrayOf1IEnumerableOfM0<SimpleOfficeDTO>((e) =>
                {
                    return list.ToArray();
                });

                var serviceResults = await service.GetParentOfficeIds(dto3.OrganizationId);
                Assert.AreEqual(2, serviceResults.Count);
                CollectionAssert.AreEqual(new List<int> { dto1.OrganizationId, dto2.OrganizationId }, serviceResults);
            }
        }

        [TestMethod]
        public async Task TestGetParentOfficeIds_HasNoParentOffice()
        {
            using (ShimsContext.Create())
            {
                var list = new List<SimpleOfficeDTO>();

                var dto1 = new SimpleOfficeDTO
                {
                    OrganizationId = 1,
                    OrganizationTypeId = OrganizationType.Office.Id,
                    OrganizationType = OrganizationType.Office.Value,
                    OfficeSymbol = "eca",
                    Name = "org 1",
                    Description = "description",
                    Path = "1",
                    OfficeLevel = 1

                };
                list.Add(dto1);
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.DatabaseGet = (c) =>
                {
                    var shimDb = new System.Data.Entity.Fakes.ShimDatabase();
                    shimDb.SqlQueryOf1StringObjectArray<SimpleOfficeDTO>(
                        (sql, parameters) =>
                        {
                            var shimDbSql = new System.Data.Entity.Infrastructure.Fakes.ShimDbRawSqlQuery<SimpleOfficeDTO>();
                            shimDbSql.ToArrayAsync = () =>
                            {
                                return Task.FromResult<SimpleOfficeDTO[]>(list.ToArray());
                            };
                            return shimDbSql;
                        }
                    );
                    return shimDb;
                };
                System.Linq.Fakes.ShimEnumerable.ToArrayOf1IEnumerableOfM0<SimpleOfficeDTO>((e) =>
                {
                    return list.ToArray();
                });

                var serviceResults = await service.GetParentOfficeIds(dto1.OrganizationId);
                Assert.AreEqual(0, serviceResults.Count);
            }
        }

        [TestMethod]
        public async Task TestGetDataPointConfigurationsAsync_NullParameters()
        {
            var result = await service.GetDataPointConfigurationsAsync(null, null, null);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task TestGetDataPointConfigurationsAsync_Office()
        {
            using (ShimsContext.Create())
            {
                var list = new List<SimpleOfficeDTO>();
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.DatabaseGet = (c) =>
                {
                    var shimDb = new System.Data.Entity.Fakes.ShimDatabase();
                    shimDb.SqlQueryOf1StringObjectArray<SimpleOfficeDTO>(
                        (sql, parameters) =>
                        {
                            var shimDbSql = new System.Data.Entity.Infrastructure.Fakes.ShimDbRawSqlQuery<SimpleOfficeDTO>();
                            shimDbSql.ToArrayAsync = () =>
                            {
                                return Task.FromResult<SimpleOfficeDTO[]>(list.ToArray());
                            };
                            return shimDbSql;
                        }
                    );
                    return shimDb;
                };
                System.Linq.Fakes.ShimEnumerable.ToArrayOf1IEnumerableOfM0<SimpleOfficeDTO>((e) =>
                {
                    return list.ToArray();
                });

                var dataPointCategory = new DataPointCategory
                {
                    DataPointCategoryId = DataPointCategory.Office.Id,
                    DataPointCategoryName = DataPointCategory.Office.Value
                };

                var dataPointProperty = new DataPointProperty
                {
                    DataPointPropertyId = DataPointProperty.Themes.Id,
                    DataPointPropertyName = DataPointProperty.Themes.Value
                };

                var dataPointCategoryProperty = new DataPointCategoryProperty
                {
                    DataPointCategoryPropertyId = 1,
                    DataPointCategoryId = dataPointCategory.DataPointCategoryId,
                    DataPointCategory = dataPointCategory,
                    DataPointPropertyId = dataPointProperty.DataPointPropertyId,
                    DataPointProperty = dataPointProperty
                };

                context.DataPointCategoryProperties.Add(dataPointCategoryProperty);

                var dataPointConfig = new DataPointConfiguration
                {
                    DataPointConfigurationId = 1,
                    OfficeId = 1,
                    DataPointCategoryPropertyId = dataPointCategoryProperty.DataPointCategoryPropertyId
                };

                context.DataPointConfigurations.Add(dataPointConfig);

                var serviceResult = await service.GetDataPointConfigurationsAsync(dataPointConfig.OfficeId.Value, null, null);
                var result = serviceResult.FirstOrDefault();
                Assert.AreEqual(dataPointConfig.DataPointConfigurationId, result.DataPointConfigurationId);
                Assert.AreEqual(dataPointConfig.OfficeId, result.OfficeId);
                Assert.AreEqual(dataPointConfig.DataPointCategoryPropertyId, result.CategoryPropertyId);
                Assert.AreEqual(dataPointCategory.DataPointCategoryId, result.CategoryId);
                Assert.AreEqual(dataPointCategory.DataPointCategoryName, result.CategoryName);
                Assert.AreEqual(dataPointProperty.DataPointPropertyId, result.PropertyId);
                Assert.AreEqual(dataPointProperty.DataPointPropertyName, result.PropertyName);
                Assert.AreEqual(true, result.IsRequired);
                Assert.AreEqual(false, result.IsInherited);
            }
        }

        [TestMethod]
        public async Task TestGetDataPointConfigurationsAsync_EmptyOffice()
        {
            using (ShimsContext.Create())
            {
                var list = new List<SimpleOfficeDTO>();
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.DatabaseGet = (c) =>
                {
                    var shimDb = new System.Data.Entity.Fakes.ShimDatabase();
                    shimDb.SqlQueryOf1StringObjectArray<SimpleOfficeDTO>(
                        (sql, parameters) =>
                        {
                            var shimDbSql = new System.Data.Entity.Infrastructure.Fakes.ShimDbRawSqlQuery<SimpleOfficeDTO>();
                            shimDbSql.ToArrayAsync = () =>
                            {
                                return Task.FromResult<SimpleOfficeDTO[]>(list.ToArray());
                            };
                            return shimDbSql;
                        }
                    );
                    return shimDb;
                };
                System.Linq.Fakes.ShimEnumerable.ToArrayOf1IEnumerableOfM0<SimpleOfficeDTO>((e) =>
                {
                    return list.ToArray();
                });

                var serviceResult = await service.GetDataPointConfigurationsAsync(1, null, null);
                Assert.AreEqual(0, serviceResult.Count);
            }
        }

        [TestMethod]
        public async Task TestGetDataPointConfigurationsAsync_OneParentOffice()
        {
            using (ShimsContext.Create())
            {
                var list = new List<SimpleOfficeDTO>();

                var dto1 = new SimpleOfficeDTO
                {
                    OrganizationId = 1,
                    OrganizationTypeId = OrganizationType.Office.Id,
                    OrganizationType = OrganizationType.Office.Value,
                    OfficeSymbol = "eca",
                    Name = "org 1",
                    Description = "description",
                    Path = "1",
                    OfficeLevel = 1

                };
                var dto2 = new SimpleOfficeDTO
                {
                    OrganizationId = 2,
                    OrganizationTypeId = OrganizationType.Office.Id,
                    OrganizationType = OrganizationType.Office.Value,
                    ParentOrganization_OrganizationId = 1,
                    OfficeSymbol = "eca",
                    Name = "org 2",
                    Description = "description",
                    Path = "1-1",
                    OfficeLevel = 2
                };
                list.Add(dto1);
                list.Add(dto2);
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.DatabaseGet = (c) =>
                {
                    var shimDb = new System.Data.Entity.Fakes.ShimDatabase();
                    shimDb.SqlQueryOf1StringObjectArray<SimpleOfficeDTO>(
                        (sql, parameters) =>
                        {
                            var shimDbSql = new System.Data.Entity.Infrastructure.Fakes.ShimDbRawSqlQuery<SimpleOfficeDTO>();
                            shimDbSql.ToArrayAsync = () =>
                            {
                                return Task.FromResult<SimpleOfficeDTO[]>(list.ToArray());
                            };
                            return shimDbSql;
                        }
                    );
                    return shimDb;
                };
                System.Linq.Fakes.ShimEnumerable.ToArrayOf1IEnumerableOfM0<SimpleOfficeDTO>((e) =>
                {
                    return list.ToArray();
                });

                var dataPointCategory = new DataPointCategory
                {
                    DataPointCategoryId = DataPointCategory.Office.Id,
                    DataPointCategoryName = DataPointCategory.Office.Value
                };

                var dataPointProperty = new DataPointProperty
                {
                    DataPointPropertyId = DataPointProperty.Themes.Id,
                    DataPointPropertyName = DataPointProperty.Themes.Value
                };

                var dataPointCategoryProperty = new DataPointCategoryProperty
                {
                    DataPointCategoryPropertyId = 1,
                    DataPointCategoryId = dataPointCategory.DataPointCategoryId,
                    DataPointCategory = dataPointCategory,
                    DataPointPropertyId = dataPointProperty.DataPointPropertyId,
                    DataPointProperty = dataPointProperty
                };

                context.DataPointCategoryProperties.Add(dataPointCategoryProperty);

                var dataPointConfig = new DataPointConfiguration
                {
                    DataPointConfigurationId = 1,
                    OfficeId = 1,
                    DataPointCategoryPropertyId = dataPointCategoryProperty.DataPointCategoryPropertyId
                };

                context.DataPointConfigurations.Add(dataPointConfig);

                var serviceResult = await service.GetDataPointConfigurationsAsync(dto2.OrganizationId, null, null);
                var result = serviceResult.FirstOrDefault();
                Assert.AreEqual(dataPointConfig.DataPointConfigurationId, result.DataPointConfigurationId);
                Assert.AreEqual(dto2.OrganizationId, result.OfficeId);
                Assert.AreEqual(dataPointConfig.DataPointCategoryPropertyId, result.CategoryPropertyId);
                Assert.AreEqual(dataPointCategory.DataPointCategoryId, result.CategoryId);
                Assert.AreEqual(dataPointCategory.DataPointCategoryName, result.CategoryName);
                Assert.AreEqual(dataPointProperty.DataPointPropertyId, result.PropertyId);
                Assert.AreEqual(dataPointProperty.DataPointPropertyName, result.PropertyName);
                Assert.AreEqual(true, result.IsRequired);
                Assert.AreEqual(true, result.IsInherited);
            }
        }

    }
}
