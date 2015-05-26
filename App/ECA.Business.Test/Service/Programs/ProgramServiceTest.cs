using ECA.Business.Exceptions;
using ECA.Business.Models.Programs;
using ECA.Business.Models.Admin;
using ECA.Business.Queries.Models.Programs;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service;
using ECA.Business.Service.Programs;
using ECA.Business.Validation;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Filter;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Exceptions;
using ECA.Core.Query;
using ECA.Data;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.QualityTools.Testing.Fakes;
using ECA.Business.Service.Admin;

namespace ECA.Business.Test.Service.Programs
{
    [TestClass]
    public class ProgramServiceTest
    {
        private TestEcaContext context;
        private ProgramService service;
        private Mock<IOfficeService> officeService;
        private OfficeSettings mockOfficeSettings;
        private Mock<IBusinessValidator<ProgramServiceValidationEntity, ProgramServiceValidationEntity>> mockValidator;

        [TestInitialize]
        public void TestInit()
        {
            mockOfficeSettings = new OfficeSettings();
            officeService = new Mock<IOfficeService>();
            officeService.Setup(x => x.GetOfficeSettings(It.IsAny<int>())).Returns(mockOfficeSettings);
            officeService.Setup(x => x.GetOfficeSettingsAsync(It.IsAny<int>())).ReturnsAsync(mockOfficeSettings);

            mockValidator = new Mock<IBusinessValidator<ProgramServiceValidationEntity, ProgramServiceValidationEntity>>();
            mockValidator.Setup(x => x.ValidateCreate(It.IsAny<ProgramServiceValidationEntity>())).Returns(new List<BusinessValidationResult>());
            mockValidator.Setup(x => x.ValidateUpdate(It.IsAny<ProgramServiceValidationEntity>())).Returns(new List<BusinessValidationResult>());

            context = new TestEcaContext();
            service = new ProgramService(context, officeService.Object, mockValidator.Object);
        }

        [TestCleanup]
        public void TestCleanup()
        {

        }

        #region Get
        [TestMethod]
        public async Task TestGetProgramById_CheckProperties_HasParentProgram()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var now = DateTime.UtcNow;
            var creatorId = 1;
            var revisorId = 2;
            var rowVersion = new byte[1] { (byte)1 };

            var contact = new Contact
            {
                ContactId = 100
            };
            var theme = new Theme
            {
                ThemeId = 2,
                ThemeName = "theme"
            };
            var goal = new Goal
            {
                GoalId = 4,
            };

            var country = new Location
            {
                LocationId = 500,
                LocationName = "country",
                LocationIso = "countryIso",
                LocationTypeId = LocationType.Country.Id,
            };
            var region = new Location
            {
                LocationName = "region",
                LocationId = 3,
                LocationIso = "locationIso",
                LocationTypeId = LocationType.Region.Id
            };
            country.Region = region;

            var parentProgram = new Program
            {
                ProgramId = 10,
            };
            var owner = new Organization
            {
                OrganizationId = 30,
                Description = "owner desc",
                Name = "owner",
                OfficeSymbol = "symbol"
            };
            var focusOfficeSetting = new OfficeSetting
            {
                Name = OfficeSetting.FOCUS_SETTING_KEY,
                Value = "focus",
                OfficeId = owner.OrganizationId,
                Office = owner,
            };
            var objectiveOfficeSetting = new OfficeSetting
            {
                Name = OfficeSetting.OBJECTIVE_SETTING_KEY,
                Value = "objective",
                OfficeId = owner.OrganizationId,
                Office = owner,
            };

            owner.OfficeSettings.Add(focusOfficeSetting);
            owner.OfficeSettings.Add(objectiveOfficeSetting);

            var focus = new Focus
            {
                FocusId = 1,
                FocusName = "focus",
                Office = owner,
                OfficeId = owner.OrganizationId
            };
            var category = new Category
            {
                CategoryId = 1,
                CategoryName = "category",
                Focus = focus,
                FocusId = focus.FocusId,
            };
            var justification = new Justification
            {
                JustificationId = 1,
                JustificationName = "justification",
                Office = owner,
                OfficeId = owner.OrganizationId
            };
            var objective = new Objective
            {
                Justification = justification,
                JustificationId = justification.JustificationId,
                ObjectiveId = 1,
                ObjectiveName = "obj",
            };
            var program = new Program
            {
                ProgramId = 1,
                Name = "name",
                Description = "description",
                ParentProgram = parentProgram,
                RowVersion = rowVersion,
                StartDate = DateTimeOffset.UtcNow,                
                History = new History
                {
                    CreatedBy = creatorId,
                    CreatedOn = yesterday,
                    RevisedBy = revisorId,
                    RevisedOn = now
                },
                Owner = owner
            };

            program.Categories.Add(category);
            program.Objectives.Add(objective);
            program.Contacts.Add(contact);
            program.Goals.Add(goal);
            program.Regions.Add(region);

            context.Foci.Add(focus);
            context.Categories.Add(category);
            context.Justifications.Add(justification);
            context.Objectives.Add(objective);
            context.OfficeSettings.Add(focusOfficeSetting);
            context.OfficeSettings.Add(objectiveOfficeSetting);
            context.Organizations.Add(owner);
            context.Programs.Add(program);
            context.Contacts.Add(contact);
            context.Themes.Add(theme);
            context.Goals.Add(goal);
            context.Locations.Add(country);
            context.Programs.Add(parentProgram);
            context.Locations.Add(region);

            Action<ProgramDTO> tester = (publishedProgram) =>
            {
                CollectionAssert.AreEqual(program.Contacts.Select(x => x.ContactId).ToList(), publishedProgram.Contacts.Select(x => x.Id).ToList());
                CollectionAssert.AreEqual(program.Contacts.Select(x => x.FullName).ToList(), publishedProgram.Contacts.Select(x => x.Value).ToList());


                CollectionAssert.AreEqual(
                    context.Locations.Where(x => x.LocationTypeId == LocationType.Country.Id).Select(x => x.LocationId).ToList(),
                    publishedProgram.CountryIsos.Select(x => x.Id).ToList());
                CollectionAssert.AreEqual(
                    context.Locations.Where(x => x.LocationTypeId == LocationType.Country.Id).Select(x => x.LocationIso).ToList(),
                    publishedProgram.CountryIsos.Select(x => x.Value).ToList());


                CollectionAssert.AreEqual(
                    context.Locations.Where(x => x.LocationTypeId == LocationType.Region.Id).Select(x => x.LocationId).ToList(),
                    publishedProgram.RegionIsos.Select(x => x.Id).ToList());
                CollectionAssert.AreEqual(
                    context.Locations.Where(x => x.LocationTypeId == LocationType.Region.Id).Select(x => x.LocationIso).ToList(),
                    publishedProgram.RegionIsos.Select(x => x.Value).ToList());

                CollectionAssert.AreEqual(
                    context.Categories.Select(x => x.CategoryId).ToList(),
                    publishedProgram.Categories.Select(x => x.Id).ToList());
                CollectionAssert.AreEqual(
                    context.Categories.Select(x => x.CategoryName).ToList(),
                    publishedProgram.Categories.Select(x => x.Name).ToList());
                CollectionAssert.AreEqual(
                    context.Foci.Select(x => x.FocusName).ToList(),
                    publishedProgram.Categories.Select(x => x.FocusName).ToList());

                CollectionAssert.AreEqual(
                    context.Objectives.Select(x => x.ObjectiveId).ToList(),
                    publishedProgram.Objectives.Select(x => x.Id).ToList());
                CollectionAssert.AreEqual(
                    context.Objectives.Select(x => x.ObjectiveName).ToList(),
                    publishedProgram.Objectives.Select(x => x.Name).ToList());
                CollectionAssert.AreEqual(
                    context.Justifications.Select(x => x.JustificationName).ToList(),
                    publishedProgram.Objectives.Select(x => x.JustificationName).ToList());

                CollectionAssert.AreEqual(context.Goals.Select(x => x.GoalName).ToList(), publishedProgram.Goals.Select(x => x.Value).ToList());
                CollectionAssert.AreEqual(context.Goals.Select(x => x.GoalId).ToList(), publishedProgram.Goals.Select(x => x.Id).ToList());

                CollectionAssert.AreEqual(rowVersion, publishedProgram.RowVersion);

                Assert.AreEqual(program.Description, publishedProgram.Description);

                Assert.AreEqual(program.ProgramId, publishedProgram.Id);
                Assert.AreEqual(program.Name, publishedProgram.Name);
                Assert.AreEqual(parentProgram.ProgramId, publishedProgram.ParentProgramId);

                Assert.AreEqual(now, publishedProgram.RevisedOn);
                Assert.AreEqual(program.StartDate, publishedProgram.StartDate);
                Assert.AreEqual(owner.Name, publishedProgram.OwnerName);
                Assert.AreEqual(owner.Description, publishedProgram.OwnerDescription);
                Assert.AreEqual(owner.OrganizationId, publishedProgram.OwnerOrganizationId);
                Assert.AreEqual(owner.OfficeSymbol, publishedProgram.OwnerOfficeSymbol);
                Assert.AreEqual(focusOfficeSetting.Value, publishedProgram.OwnerOrganizationCategoryLabel);
                Assert.AreEqual(objectiveOfficeSetting.Value, publishedProgram.OwnerOrganizationObjectiveLabel);

            };
            var result = service.GetProgramById(program.ProgramId);
            var resultAsync = await service.GetProgramByIdAsync(program.ProgramId);
            tester(result);
            tester(resultAsync);
        }


        [TestMethod]
        public async Task TestGetProgramById_NoOfficeSettings()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var now = DateTime.UtcNow;
            var creatorId = 1;
            var revisorId = 2;

            var program = new Program
            {
                ProgramId = 1,
                Name = "name",
                Description = "description",
                ParentProgram = null,
                StartDate = DateTimeOffset.UtcNow,
                History = new History
                {
                    CreatedBy = creatorId,
                    CreatedOn = yesterday,
                    RevisedBy = revisorId,
                    RevisedOn = now
                }
            };

            var owner = new Organization
            {
                OrganizationId = 30,
                Description = "owner desc",
                Name = "owner",
                OfficeSymbol = "symbol"
            };
            program.Owner = owner;
            context.Programs.Add(program);
            context.Organizations.Add(owner);

            Action<ProgramDTO> tester = (publishedProgram) =>
            {
                Assert.AreEqual(0, context.OfficeSettings.Count());
                Assert.AreEqual(OfficeSettings.CATEGORY_DEFAULT_LABEL, publishedProgram.OwnerOrganizationCategoryLabel);
                Assert.AreEqual(OfficeSettings.OBJECTIVE_DEFAULT_LABEL, publishedProgram.OwnerOrganizationObjectiveLabel);
                
            };
            var result = service.GetProgramById(program.ProgramId);
            var resultAsync = await service.GetProgramByIdAsync(program.ProgramId);
            tester(result);
            tester(resultAsync);
        }

        [TestMethod]
        public async Task TestGetProgramById_DoesNotHaveParentProgram()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var now = DateTime.UtcNow;
            var creatorId = 1;
            var revisorId = 2;

            var program = new Program
            {
                ProgramId = 1,
                Name = "name",
                Description = "description",
                ParentProgram = null,
                StartDate = DateTimeOffset.UtcNow,
                History = new History
                {
                    CreatedBy = creatorId,
                    CreatedOn = yesterday,
                    RevisedBy = revisorId,
                    RevisedOn = now
                }
            };
            var focusOfficeSetting = new OfficeSetting
            {
                Name = OfficeSetting.FOCUS_SETTING_KEY,
                Value = "focus"
            };
            var justificationOfficeSetting = new OfficeSetting
            {
                Name = OfficeSetting.JUSTIFICATION_SETTING_KEY,
                Value = "justification"
            };
            var owner = new Organization
            {
                OrganizationId = 30,
                Description = "owner desc",
                Name = "owner",
                OfficeSymbol = "symbol"
            };
            owner.OfficeSettings.Add(focusOfficeSetting);
            owner.OfficeSettings.Add(justificationOfficeSetting);
            program.Owner = owner;
            context.Programs.Add(program);
            context.Organizations.Add(owner);
            context.OfficeSettings.Add(focusOfficeSetting);
            context.OfficeSettings.Add(justificationOfficeSetting);
            Action<ProgramDTO> tester = (publishedProgram) =>
            {
                Assert.IsFalse(publishedProgram.ParentProgramId.HasValue);
            };
            var result = service.GetProgramById(program.ProgramId);
            var resultAsync = await service.GetProgramByIdAsync(program.ProgramId);
            tester(result);
            tester(resultAsync);
        }

        [TestMethod]
        public async Task TestGetProgramById_ProgramDoesNotExist()
        {
            Assert.IsNull(service.GetProgramById(-1));
            Assert.IsNull(await service.GetProgramByIdAsync(-1));
        }

        [TestMethod]
        public async Task TestGetPrograms_Filter()
        {
            using (ShimsContext.Create())
            {
                var list = new List<OrganizationProgramDTO>();
                var dto1 = new OrganizationProgramDTO
                {
                    Description = "desc1",
                    Name = "org 1",
                    NumChildren = 2,
                    OfficeSymbol = "eca",
                    OrgName = "eca org",
                    Owner_OrganizationId = 2,
                    OwnerId = 3,
                    ParentProgram_ProgramId = 4,
                    ProgramId = 5,
                    ProgramLevel = 6,
                    ProgramStatusId = ProgramStatus.Active.Id,
                    SortOrder = 7
                };
                var dto2 = new OrganizationProgramDTO
                {
                    Description = "desc2",
                    Name = "org 2",
                    NumChildren = 20,
                    OfficeSymbol = "eca",
                    OrgName = "eca org",
                    Owner_OrganizationId = 20,
                    OwnerId = 30,
                    ParentProgram_ProgramId = 40,
                    ProgramId = 50,
                    ProgramLevel = 60,
                    ProgramStatusId = ProgramStatus.Active.Id,
                    SortOrder = 70
                };
                list.Add(dto1);
                list.Add(dto2);
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.DatabaseGet = (c) =>
                {
                    var shimDb = new System.Data.Entity.Fakes.ShimDatabase();
                    shimDb.SqlQueryOf1StringObjectArray<OrganizationProgramDTO>(
                        (sql, parameters) =>
                        {
                            var shimDbSql = new System.Data.Entity.Infrastructure.Fakes.ShimDbRawSqlQuery<OrganizationProgramDTO>();
                            shimDbSql.ToArrayAsync = () =>
                            {
                                return Task.FromResult<OrganizationProgramDTO[]>(list.ToArray());
                            };
                            return shimDbSql;
                        }
                    );
                    return shimDb;
                };
                System.Linq.Fakes.ShimEnumerable.ToArrayOf1IEnumerableOfM0<OrganizationProgramDTO>((e) =>
                {
                    return list.ToArray();
                });
                Action<PagedQueryResults<OrganizationProgramDTO>> tester = (queryResults) =>
                {
                    Assert.AreEqual(1, queryResults.Total);
                    var results = queryResults.Results;
                    Assert.AreEqual(1, results.Count);
                    var firstResult = results.First();
                    Assert.IsTrue(object.ReferenceEquals(dto1, firstResult));
                };
                var queryOperator = new QueryableOperator<OrganizationProgramDTO>(0, 10, new ExpressionSorter<OrganizationProgramDTO>(x => x.Name, SortDirection.Ascending));
                queryOperator.Filters.Add(new ExpressionFilter<OrganizationProgramDTO>(x => x.Name, ComparisonType.Equal, dto1.Name));
                var serviceResults = service.GetPrograms(queryOperator);
                var serviceResultsAsync = await service.GetProgramsAsync(queryOperator);
                tester(serviceResults);
                tester(serviceResultsAsync);
            }
        }

        [TestMethod]
        public async Task TestGetPrograms_DefaultSort()
        {
            using (ShimsContext.Create())
            {
                var list = new List<OrganizationProgramDTO>();
                var dto1 = new OrganizationProgramDTO
                {
                    Description = "desc1",
                    Name = "org 1",
                    NumChildren = 2,
                    OfficeSymbol = "eca",
                    OrgName = "eca org",
                    Owner_OrganizationId = 2,
                    OwnerId = 3,
                    ParentProgram_ProgramId = 4,
                    ProgramId = 5,
                    ProgramLevel = 6,
                    ProgramStatusId = ProgramStatus.Active.Id,
                    SortOrder = 7
                };
                var dto2 = new OrganizationProgramDTO
                {
                    Description = "desc2",
                    Name = "org 2",
                    NumChildren = 20,
                    OfficeSymbol = "eca",
                    OrgName = "eca org",
                    Owner_OrganizationId = 20,
                    OwnerId = 30,
                    ParentProgram_ProgramId = 40,
                    ProgramId = 50,
                    ProgramLevel = 60,
                    ProgramStatusId = ProgramStatus.Active.Id,
                    SortOrder = 70
                };
                list.Add(dto1);
                list.Add(dto2);
                list = list.OrderByDescending(x => x.Name).ToList();
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.DatabaseGet = (c) =>
                {
                    var shimDb = new System.Data.Entity.Fakes.ShimDatabase();
                    shimDb.SqlQueryOf1StringObjectArray<OrganizationProgramDTO>(
                        (sql, parameters) =>
                        {
                            var shimDbSql = new System.Data.Entity.Infrastructure.Fakes.ShimDbRawSqlQuery<OrganizationProgramDTO>();
                            shimDbSql.ToArrayAsync = () =>
                            {
                                return Task.FromResult<OrganizationProgramDTO[]>(list.ToArray());
                            };
                            return shimDbSql;
                        }
                    );
                    return shimDb;
                };
                System.Linq.Fakes.ShimEnumerable.ToArrayOf1IEnumerableOfM0<OrganizationProgramDTO>((e) =>
                {
                    return list.ToArray();
                });
                Action<PagedQueryResults<OrganizationProgramDTO>> tester = (queryResults) =>
                {
                    Assert.AreEqual(2, queryResults.Total);
                    var results = queryResults.Results;
                    Assert.AreEqual(2, results.Count);
                    var firstResult = results.First();
                    Assert.IsTrue(object.ReferenceEquals(dto1, firstResult));
                };
                var queryOperator = new QueryableOperator<OrganizationProgramDTO>(0, 10, new ExpressionSorter<OrganizationProgramDTO>(x => x.Name, SortDirection.Ascending));
                var serviceResults = service.GetPrograms(queryOperator);
                var serviceResultsAsync = await service.GetProgramsAsync(queryOperator);
                tester(serviceResults);
                tester(serviceResultsAsync);
            }
        }

        [TestMethod]
        public async Task TestGetPrograms_Sort()
        {
            using (ShimsContext.Create())
            {
                var list = new List<OrganizationProgramDTO>();
                var dto1 = new OrganizationProgramDTO
                {
                    Description = "desc1",
                    Name = "org 1",
                    NumChildren = 2,
                    OfficeSymbol = "eca",
                    OrgName = "eca org",
                    Owner_OrganizationId = 2,
                    OwnerId = 3,
                    ParentProgram_ProgramId = 4,
                    ProgramId = 5,
                    ProgramLevel = 6,
                    ProgramStatusId = ProgramStatus.Active.Id,
                    SortOrder = 7
                };
                var dto2 = new OrganizationProgramDTO
                {
                    Description = "desc2",
                    Name = "org 2",
                    NumChildren = 20,
                    OfficeSymbol = "eca",
                    OrgName = "eca org",
                    Owner_OrganizationId = 20,
                    OwnerId = 30,
                    ParentProgram_ProgramId = 40,
                    ProgramId = 50,
                    ProgramLevel = 60,
                    ProgramStatusId = ProgramStatus.Active.Id,
                    SortOrder = 70
                };
                list.Add(dto1);
                list.Add(dto2);
                list = list.OrderBy(x => x.NumChildren).ToList();
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.DatabaseGet = (c) =>
                {
                    var shimDb = new System.Data.Entity.Fakes.ShimDatabase();
                    shimDb.SqlQueryOf1StringObjectArray<OrganizationProgramDTO>(
                        (sql, parameters) =>
                        {
                            var shimDbSql = new System.Data.Entity.Infrastructure.Fakes.ShimDbRawSqlQuery<OrganizationProgramDTO>();
                            shimDbSql.ToArrayAsync = () =>
                            {
                                return Task.FromResult<OrganizationProgramDTO[]>(list.ToArray());
                            };
                            return shimDbSql;
                        }
                    );
                    return shimDb;
                };
                System.Linq.Fakes.ShimEnumerable.ToArrayOf1IEnumerableOfM0<OrganizationProgramDTO>((e) =>
                {
                    return list.ToArray();
                });
                Action<PagedQueryResults<OrganizationProgramDTO>> tester = (queryResults) =>
                {
                    Assert.AreEqual(2, queryResults.Total);
                    var results = queryResults.Results;
                    Assert.AreEqual(2, results.Count);
                    var firstResult = results.First();
                    Assert.IsTrue(object.ReferenceEquals(dto2, firstResult));
                };
                var queryOperator = new QueryableOperator<OrganizationProgramDTO>(0, 10, new ExpressionSorter<OrganizationProgramDTO>(x => x.Name, SortDirection.Ascending));
                queryOperator.Sorters.Add(new ExpressionSorter<OrganizationProgramDTO>(x => x.NumChildren, SortDirection.Descending));
                var serviceResults = service.GetPrograms(queryOperator);
                var serviceResultsAsync = await service.GetProgramsAsync(queryOperator);
                tester(serviceResults);
                tester(serviceResultsAsync);
            }
        }

        [TestMethod]
        public async Task TestGetPrograms_Paging()
        {
            using (ShimsContext.Create())
            {
                var list = new List<OrganizationProgramDTO>();
                var dto1 = new OrganizationProgramDTO
                {
                    Description = "desc1",
                    Name = "org 1",
                    NumChildren = 2,
                    OfficeSymbol = "eca",
                    OrgName = "eca org",
                    Owner_OrganizationId = 2,
                    OwnerId = 3,
                    ParentProgram_ProgramId = 4,
                    ProgramId = 5,
                    ProgramLevel = 6,
                    ProgramStatusId = ProgramStatus.Active.Id,
                    SortOrder = 7
                };
                var dto2 = new OrganizationProgramDTO
                {
                    Description = "desc2",
                    Name = "org 2",
                    NumChildren = 20,
                    OfficeSymbol = "eca",
                    OrgName = "eca org",
                    Owner_OrganizationId = 20,
                    OwnerId = 30,
                    ParentProgram_ProgramId = 40,
                    ProgramId = 50,
                    ProgramLevel = 60,
                    ProgramStatusId = ProgramStatus.Active.Id,
                    SortOrder = 70
                };
                list.Add(dto1);
                list.Add(dto2);
                list = list.OrderBy(x => x.NumChildren).ToList();
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.DatabaseGet = (c) =>
                {
                    var shimDb = new System.Data.Entity.Fakes.ShimDatabase();
                    shimDb.SqlQueryOf1StringObjectArray<OrganizationProgramDTO>(
                        (sql, parameters) =>
                        {
                            var shimDbSql = new System.Data.Entity.Infrastructure.Fakes.ShimDbRawSqlQuery<OrganizationProgramDTO>();
                            shimDbSql.ToArrayAsync = () =>
                            {
                                return Task.FromResult<OrganizationProgramDTO[]>(list.ToArray());
                            };
                            return shimDbSql;
                        }
                    );
                    return shimDb;
                };
                System.Linq.Fakes.ShimEnumerable.ToArrayOf1IEnumerableOfM0<OrganizationProgramDTO>((e) =>
                {
                    return list.ToArray();
                });
                Action<PagedQueryResults<OrganizationProgramDTO>> tester = (queryResults) =>
                {
                    Assert.AreEqual(2, queryResults.Total);
                    var results = queryResults.Results;
                    Assert.AreEqual(1, results.Count);
                    var firstResult = results.First();
                    Assert.IsTrue(object.ReferenceEquals(dto1, firstResult));
                };
                var queryOperator = new QueryableOperator<OrganizationProgramDTO>(0, 1, new ExpressionSorter<OrganizationProgramDTO>(x => x.Name, SortDirection.Ascending));
                var serviceResults = service.GetPrograms(queryOperator);
                var serviceResultsAsync = await service.GetProgramsAsync(queryOperator);
                tester(serviceResults);
                tester(serviceResultsAsync);
            }
        }
        #endregion

        #region Create
        [TestMethod]
        public void TestCreate_CheckProperties()
        {
            var parentProgramId = 3;
            var parentProgram = new Program { ProgramId = parentProgramId };
            context.Programs.Add(parentProgram);
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });

            var userId = 1;
            var user = new User(userId);
            var name = "name";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var website = "http://www.google.com";
            var pointOfContactIds = new List<int>();
            var themeIds = new List<int>();
            var goalIds = new List<int>();
            var regionIds = new List<int>();
            var categoryIds = new List<int>();
            var objectiveIds = new List<int>();

            var draftProgram = new DraftProgram(
               createdBy: user,
               name: name,
               description: description,
               startDate: startDate,
               endDate: endDate,
               ownerOrganizationId: ownerId,
               parentProgramId: parentProgramId,
               website: website,
               goalIds: goalIds,
               pointOfContactIds: pointOfContactIds,
               themeIds: themeIds,
               regionIds: regionIds,
               categoryIds: categoryIds,
               objectiveIds: objectiveIds
               );

            var program = service.Create(draftProgram);
            mockValidator.Verify(x => x.ValidateCreate(It.IsAny<ProgramServiceValidationEntity>()), Times.Once());
            Assert.IsNotNull(program);
            Assert.IsNotNull(program.ParentProgram);
            Assert.AreEqual(0, program.Contacts.Count);
            Assert.AreEqual(0, program.Themes.Count);
            Assert.AreEqual(0, program.Goals.Count);

            Assert.AreEqual(user.Id, program.History.CreatedBy);
            Assert.AreEqual(user.Id, program.History.RevisedBy);
            DateTimeOffset.UtcNow.Should().BeCloseTo(program.History.RevisedOn, DbContextHelper.DATE_PRECISION);
            DateTimeOffset.UtcNow.Should().BeCloseTo(program.History.CreatedOn, DbContextHelper.DATE_PRECISION);

            Assert.AreEqual(name, program.Name);
            Assert.AreEqual(description, program.Description);
            Assert.AreEqual(startDate, program.StartDate);
            Assert.AreEqual(endDate, program.EndDate);
            Assert.AreEqual(ownerId, program.OwnerId);
            Assert.AreEqual(parentProgramId, program.ParentProgram.ProgramId);
            Assert.AreEqual(ProgramStatus.Draft.Id, program.ProgramStatusId);
            Assert.AreEqual(website, program.Website);
        }

        [TestMethod]
        public void TestCreate_DoesNotHaveParentProgram()
        {
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });

            var userId = 1;
            var user = new User(userId);
            var name = "name";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var website = "http://www.google.com";
            var pointOfContactIds = new List<int>();
            var themeIds = new List<int>();
            var goalIds = new List<int>();
            var regionIds = new List<int>();
            var categoryIds = new List<int>();
            var objectiveIds = new List<int>();

            var draftProgram = new DraftProgram(
               createdBy: user,
               name: name,
               description: description,
               startDate: startDate,
               endDate: endDate,
               ownerOrganizationId: ownerId,
               parentProgramId: null,
               website: website,
               goalIds: goalIds,
               pointOfContactIds: pointOfContactIds,
               themeIds: themeIds,
               regionIds: regionIds,
               categoryIds: categoryIds,
               objectiveIds: objectiveIds
               );

            var program = service.Create(draftProgram);
            Assert.IsNotNull(program);
            Assert.IsNull(program.ParentProgram);

        }

        [TestMethod]
        public async Task TestCreateAsync_CheckProperties()
        {
            var parentProgramId = 3;
            var parentProgram = new Program { ProgramId = parentProgramId };
            context.Programs.Add(parentProgram);
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });
            var userId = 1;
            var user = new User(userId);
            var name = "name";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            
            var website = "http://www.google.com";
            var pointOfContactIds = new List<int>();
            var themeIds = new List<int>();
            var goalIds = new List<int>();
            var regionIds = new List<int>();
            var categoryIds = new List<int>();
            var objectiveIds = new List<int>();

            var draftProgram = new DraftProgram(
               createdBy: user,
               name: name,
               description: description,
               startDate: startDate,
               endDate: endDate,
               ownerOrganizationId: ownerId,
               parentProgramId: parentProgramId,
               website: website,
               goalIds: goalIds,
               pointOfContactIds: pointOfContactIds,
               themeIds: themeIds,
               regionIds: regionIds,
               categoryIds: categoryIds,
               objectiveIds: objectiveIds
               );

            var program = await service.CreateAsync(draftProgram);
            mockValidator.Verify(x => x.ValidateCreate(It.IsAny<ProgramServiceValidationEntity>()), Times.Once());
            Assert.IsNotNull(program);
            Assert.IsNotNull(program.ParentProgram);
            Assert.AreEqual(0, program.Contacts.Count);
            Assert.AreEqual(0, program.Themes.Count);
            Assert.AreEqual(0, program.Goals.Count);

            Assert.AreEqual(user.Id, program.History.CreatedBy);
            Assert.AreEqual(user.Id, program.History.RevisedBy);
            DateTimeOffset.UtcNow.Should().BeCloseTo(program.History.RevisedOn, DbContextHelper.DATE_PRECISION);
            DateTimeOffset.UtcNow.Should().BeCloseTo(program.History.CreatedOn, DbContextHelper.DATE_PRECISION);

            Assert.AreEqual(name, program.Name);
            Assert.AreEqual(description, program.Description);
            Assert.AreEqual(startDate, program.StartDate);
            Assert.AreEqual(endDate, program.EndDate);
            Assert.AreEqual(ownerId, program.OwnerId);
            Assert.AreEqual(parentProgramId, program.ParentProgram.ProgramId);
            Assert.AreEqual(ProgramStatus.Draft.Id, program.ProgramStatusId);
            Assert.AreEqual(website, program.Website);
        }

        [TestMethod]
        public async Task TestCreateAsync_DoesNotHaveParentProgram()
        {
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });
            var focusId = 100;
            var focus = new Focus { FocusId = focusId };
            context.Foci.Add(focus);

            var userId = 1;
            var user = new User(userId);
            var name = "name";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);

            var website = "http://www.google.com";
            var pointOfContactIds = new List<int>();
            var themeIds = new List<int>();
            var goalIds = new List<int>();
            var regionIds = new List<int>();
            var categoryIds = new List<int>();
            var objectiveIds = new List<int>();

            var draftProgram = new DraftProgram(
               createdBy: user,
               name: name,
               description: description,
               startDate: startDate,
               endDate: endDate,
               ownerOrganizationId: ownerId,
               parentProgramId: null,
               website: website,
               goalIds: goalIds,
               pointOfContactIds: pointOfContactIds,
               themeIds: themeIds,
               regionIds: regionIds,
               categoryIds: categoryIds,
               objectiveIds: objectiveIds
               );

            var program = await service.CreateAsync(draftProgram);
            Assert.IsNotNull(program);
            Assert.IsNull(program.ParentProgram);

        }


        [TestMethod]
        public async Task TestCreateAsync_CheckFocus()
        {
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });
            var focusId = 100;
            var focus = new Focus { FocusId = focusId };
            context.Foci.Add(focus);
            var contact = new Contact
            {
                ContactId = 1,
                FullName = "contact name"
            };
            var userId = 1;
            var user = new User(userId);
            var name = "name";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var parentProgramId = 3;
            var website = "http://www.google.com";
            var pointOfContactIds = new List<int> { contact.ContactId };
            var themeIds = new List<int>();
            var goalIds = new List<int>();
            var regionIds = new List<int>();
            var categoryIds = new List<int>();
            var objectiveIds = new List<int>();

            var draftProgram = new DraftProgram(
               createdBy: user,
               name: name,
               description: description,
               startDate: startDate,
               endDate: endDate,
               ownerOrganizationId: ownerId,
               parentProgramId: parentProgramId,
               website: website,
               goalIds: goalIds,
               pointOfContactIds: pointOfContactIds,
               themeIds: themeIds,
               regionIds: regionIds,
               categoryIds: categoryIds,
               objectiveIds: objectiveIds
               );

            var program = await service.CreateAsync(draftProgram);
            Assert.IsNotNull(program);
        }

        [TestMethod]
        public void TestCreate_CheckContacts()
        {
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });
            var focusId = 100;
            var focus = new Focus { FocusId = focusId };
            context.Foci.Add(focus);
            var contact = new Contact
            {
                ContactId = 1,
                FullName = "contact name"
            };
            var userId = 1;
            var user = new User(userId);
            var name = "name";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var parentProgramId = 3;
            var website = "http://www.google.com";
            var pointOfContactIds = new List<int> { contact.ContactId };
            var themeIds = new List<int>();
            var goalIds = new List<int>();
            var regionIds = new List<int>();
            var categoryIds = new List<int>();
            var objectiveIds = new List<int>();

            var draftProgram = new DraftProgram(
               createdBy: user,
               name: name,
               description: description,
               startDate: startDate,
               endDate: endDate,
               ownerOrganizationId: ownerId,
               parentProgramId: parentProgramId,
               website: website,
               goalIds: goalIds,
               pointOfContactIds: pointOfContactIds,
               themeIds: themeIds,
               regionIds: regionIds,
               categoryIds: categoryIds,
               objectiveIds: objectiveIds
               );

            var program = service.Create(draftProgram);
            Assert.IsNotNull(program);
            Assert.AreEqual(1, program.Contacts.Count);
            Assert.AreEqual(0, program.Themes.Count);
            Assert.AreEqual(0, program.Goals.Count);
            Assert.AreEqual(contact.ContactId, program.Contacts.First().ContactId);
        }

        [TestMethod]
        public async Task TestCreateAsync_CheckContacts()
        {
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });
            var contact = new Contact
            {
                ContactId = 1,
                FullName = "contact name"
            };
            var userId = 1;
            var user = new User(userId);
            var name = "name";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var parentProgramId = 3;
            var website = "http://www.google.com";
            var pointOfContactIds = new List<int> { contact.ContactId };
            var themeIds = new List<int>();
            var goalIds = new List<int>();
            var regionIds = new List<int>();
            var categoryIds = new List<int>();
            var objectiveIds = new List<int>();

            var draftProgram = new DraftProgram(
               createdBy: user,
               name: name,
               description: description,
               startDate: startDate,
               endDate: endDate,
               ownerOrganizationId: ownerId,
               parentProgramId: parentProgramId,
               website: website,
               goalIds: goalIds,
               pointOfContactIds: pointOfContactIds,
               themeIds: themeIds,
               regionIds: regionIds,
               categoryIds: categoryIds,
               objectiveIds: objectiveIds
               );

            var program = await service.CreateAsync(draftProgram);
            Assert.IsNotNull(program);
            Assert.AreEqual(1, program.Contacts.Count);
            Assert.AreEqual(0, program.Themes.Count);
            Assert.AreEqual(0, program.Goals.Count);
            Assert.AreEqual(contact.ContactId, program.Contacts.First().ContactId);
        }

        [TestMethod]
        public void TestCreate_CheckGoals()
        {
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });

            var goal = new Goal
            {
                GoalId = 1,
                GoalName = "contact name"
            };
            var userId = 1;
            var user = new User(userId);
            var name = "name";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var parentProgramId = 3;
            var website = "http://www.google.com";
            var goalIds = new List<int> { goal.GoalId };
            var themeIds = new List<int>();
            var contactIds = new List<int>();
            var regionIds = new List<int>();
            var categoryIds = new List<int>();
            var objectiveIds = new List<int>();

            var draftProgram = new DraftProgram(
               createdBy: user,
               name: name,
               description: description,
               startDate: startDate,
               endDate: endDate,
               ownerOrganizationId: ownerId,
               parentProgramId: parentProgramId,
               website: website,
               goalIds: goalIds,
               pointOfContactIds: contactIds,
               themeIds: themeIds,
               regionIds: regionIds,
               categoryIds: categoryIds,
               objectiveIds: objectiveIds
               );

            var program = service.Create(draftProgram);
            Assert.IsNotNull(program);
            Assert.AreEqual(0, program.Contacts.Count);
            Assert.AreEqual(0, program.Themes.Count);
            Assert.AreEqual(1, program.Goals.Count);
            Assert.AreEqual(goal.GoalId, program.Goals.First().GoalId);
        }

        [TestMethod]
        public async Task TestCreateAsync_CheckGoals()
        {
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });

            var goal = new Goal
            {
                GoalId = 1,
                GoalName = "contact name"
            };
            var userId = 1;
            var user = new User(userId);
            var name = "name";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var parentProgramId = 3;
            var website = "http://www.google.com";
            var goalIds = new List<int> { goal.GoalId };
            var themeIds = new List<int>();
            var contactIds = new List<int>();
            var regionIds = new List<int>();
            var categoryIds = new List<int>();
            var objectiveIds = new List<int>();

            var draftProgram = new DraftProgram(
               createdBy: user,
               name: name,
               description: description,
               startDate: startDate,
               endDate: endDate,
               ownerOrganizationId: ownerId,
               parentProgramId: parentProgramId,
               website: website,
               goalIds: goalIds,
               pointOfContactIds: contactIds,
               themeIds: themeIds,
               regionIds: regionIds,
               categoryIds: categoryIds,
               objectiveIds: objectiveIds
               );

            var program = await service.CreateAsync(draftProgram);
            Assert.IsNotNull(program);
            Assert.AreEqual(0, program.Contacts.Count);
            Assert.AreEqual(0, program.Themes.Count);
            Assert.AreEqual(1, program.Goals.Count);
            Assert.AreEqual(goal.GoalId, program.Goals.First().GoalId);
        }

        [TestMethod]
        public void TestCreate_CheckThemes()
        {
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });

            var theme = new Theme
            {
                ThemeId = 1,
                ThemeName = "contact name"
            };
            var userId = 1;
            var user = new User(userId);
            var name = "name";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var parentProgramId = 3;
            var website = "http://www.google.com";
            var goalIds = new List<int>();
            var themeIds = new List<int> { theme.ThemeId };
            var contactIds = new List<int>();
            var regionIds = new List<int>();
            var categoryIds = new List<int>();
            var objectiveIds = new List<int>();

            var draftProgram = new DraftProgram(
               createdBy: user,
               name: name,
               description: description,
               startDate: startDate,
               endDate: endDate,
               ownerOrganizationId: ownerId,
               parentProgramId: parentProgramId,
               website: website,
               goalIds: goalIds,
               pointOfContactIds: contactIds,
               themeIds: themeIds,
               regionIds: regionIds,
               categoryIds: categoryIds,
               objectiveIds: objectiveIds
               );

            var program = service.Create(draftProgram);
            Assert.IsNotNull(program);
            Assert.AreEqual(0, program.Contacts.Count);
            Assert.AreEqual(1, program.Themes.Count);
            Assert.AreEqual(0, program.Goals.Count);
            Assert.AreEqual(theme.ThemeId, program.Themes.First().ThemeId);
        }

        [TestMethod]
        public async Task TestCreateAsync_CheckThemes()
        {
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });
            var focusId = 100;
            var focus = new Focus { FocusId = focusId };
            context.Foci.Add(focus);
            var theme = new Theme
            {
                ThemeId = 1,
                ThemeName = "contact name"
            };
            var userId = 1;
            var user = new User(userId);
            var name = "name";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var parentProgramId = 3;
            var website = "http://www.google.com";
            var goalIds = new List<int>();
            var themeIds = new List<int> { theme.ThemeId };
            var contactIds = new List<int>();
            var regionIds = new List<int>();
            var categoryIds = new List<int>();
            var objectiveIds = new List<int>();

            var draftProgram = new DraftProgram(
               createdBy: user,
               name: name,
               description: description,
               startDate: startDate,
               endDate: endDate,
               ownerOrganizationId: ownerId,
               parentProgramId: parentProgramId,
               website: website,
               goalIds: goalIds,
               pointOfContactIds: contactIds,
               themeIds: themeIds,
               regionIds: regionIds,
               categoryIds: categoryIds,
               objectiveIds: objectiveIds
               );

            var program = await service.CreateAsync(draftProgram);
            Assert.IsNotNull(program);
            Assert.AreEqual(0, program.Contacts.Count);
            Assert.AreEqual(1, program.Themes.Count);
            Assert.AreEqual(0, program.Goals.Count);
            Assert.AreEqual(theme.ThemeId, program.Themes.First().ThemeId);
        }

        [TestMethod]
        public void TestCreate_CheckRegions()
        {
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });
            var focusId = 100;
            var focus = new Focus { FocusId = focusId };
            context.Foci.Add(focus);
            var region = new Location { LocationId = 1, LocationTypeId = LocationType.Region.Id };
            context.Locations.Add(region);
            var userId = 1;
            var user = new User(userId);
            var name = "name";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var parentProgramId = 3;
            var website = "http://www.google.com";
            var goalIds = new List<int>();
            var themeIds = new List<int>();
            var contactIds = new List<int>();
            var regionIds = new List<int> { region.LocationId };
            var categoryIds = new List<int>();
            var objectiveIds = new List<int>();

            var draftProgram = new DraftProgram(
               createdBy: user,
               name: name,
               description: description,
               startDate: startDate,
               endDate: endDate,
               ownerOrganizationId: ownerId,
               parentProgramId: parentProgramId,
               website: website,
               goalIds: goalIds,
               pointOfContactIds: contactIds,
               themeIds: themeIds,
               regionIds: regionIds,
               categoryIds: categoryIds,
               objectiveIds: objectiveIds
               );

            var program = service.Create(draftProgram);
            Assert.IsNotNull(program);
            Assert.AreEqual(0, program.Contacts.Count);
            Assert.AreEqual(0, program.Themes.Count);
            Assert.AreEqual(0, program.Goals.Count);
            Assert.AreEqual(1, program.Regions.Count);
            Assert.AreEqual(region.LocationId, program.Regions.First().LocationId);
        }

        [TestMethod]
        public async Task TestCreateAsync_CheckRegions()
        {
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });
            var focusId = 100;
            var focus = new Focus { FocusId = focusId };
            context.Foci.Add(focus);
            var region = new Location { LocationId = 1, LocationTypeId = LocationType.Region.Id };
            context.Locations.Add(region);
            var userId = 1;
            var user = new User(userId);
            var name = "name";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var parentProgramId = 3;
            var website = "http://www.google.com";
            var goalIds = new List<int>();
            var themeIds = new List<int>();
            var contactIds = new List<int>();
            var regionIds = new List<int> { region.LocationId };
            var categoryIds = new List<int>();
            var objectiveIds = new List<int>();

            var draftProgram = new DraftProgram(
               createdBy: user,
               name: name,
               description: description,
               startDate: startDate,
               endDate: endDate,
               ownerOrganizationId: ownerId,
               parentProgramId: parentProgramId,
               website: website,
               goalIds: goalIds,
               pointOfContactIds: contactIds,
               themeIds: themeIds,
               regionIds: regionIds,
               categoryIds: categoryIds,
               objectiveIds: objectiveIds
               );

            var program = await service.CreateAsync(draftProgram);
            Assert.IsNotNull(program);
            Assert.AreEqual(0, program.Contacts.Count);
            Assert.AreEqual(0, program.Themes.Count);
            Assert.AreEqual(0, program.Goals.Count);
            Assert.AreEqual(1, program.Regions.Count);
            Assert.AreEqual(region.LocationId, program.Regions.First().LocationId);
        }
        #endregion

        #region Update
        [TestMethod]
        public void TestUpdate_CheckProperties()
        {
            Assert.AreEqual(0, context.Programs.Count());
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var now = DateTime.UtcNow;
            var creatorId = 1;
            var revisorId = 2;
            var originalRowVersion = new byte[1] { (byte)0 };
            var parentProgram = new Program
            {
                ProgramId = 2
            };

            var program = new Program
            {
                ProgramId = 1,
                Name = "old name",
                Description = "old description",
                StartDate = DateTimeOffset.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1.0),
                ProgramStatusId = ProgramStatus.Draft.Id,
                ParentProgram = null,
                Website = "old website",
                History = new History
                {
                    CreatedBy = creatorId,
                    CreatedOn = yesterday,
                    RevisedBy = creatorId,
                    RevisedOn = yesterday
                },
            };
            context.Programs.Add(program);
            context.Programs.Add(parentProgram);
            Assert.AreEqual(2, context.Programs.Count());

            var newName = "new name";
            var newDescription = "new description";
            var newStartDate = DateTimeOffset.UtcNow.AddDays(10.0);
            var newEndDate = DateTimeOffset.UtcNow.AddDays(12.0);
            var newProgramStatusId = ProgramStatus.Completed.Id;
            var newWebsite = "new website";
            var newParentProgramId = parentProgram.ProgramId;
            var updatedRowVersion = new byte[1] { (byte)1 };

            var updatedEcaProgram = new EcaProgram(
                updatedBy: new User(revisorId),
                id: program.ProgramId,
                name: newName,
                description: newDescription,
                startDate: newStartDate,
                endDate: newEndDate,
                ownerOrganizationId: ownerId,
                parentProgramId: newParentProgramId,
                programStatusId: newProgramStatusId,
                programRowVersion: updatedRowVersion,
                website: newWebsite,
                goalIds: null,
                pointOfContactIds: null,
                themeIds: null,
                regionIds: null,
                categoryIds: null,
                objectiveIds: null
                );
            service.Update(updatedEcaProgram);

            mockValidator.Verify(x => x.ValidateUpdate(It.IsAny<ProgramServiceValidationEntity>()), Times.Once());

            var updatedProgram = context.Programs.First();
            Assert.AreEqual(newDescription, updatedProgram.Description);
            Assert.AreEqual(newEndDate, updatedProgram.EndDate);
            Assert.AreEqual(newName, updatedProgram.Name);
            Assert.AreEqual(ownerId, updatedProgram.OwnerId);
            Assert.AreEqual(ownerId, updatedProgram.Owner.OrganizationId);
            Assert.AreEqual(program.ProgramId, updatedProgram.ProgramId);
            Assert.AreEqual(newProgramStatusId, updatedProgram.ProgramStatusId);
            Assert.AreEqual(newStartDate, updatedProgram.StartDate);
            Assert.AreEqual(newWebsite, updatedProgram.Website);

            Assert.AreEqual(yesterday, updatedProgram.History.CreatedOn);
            Assert.AreEqual(creatorId, updatedProgram.History.CreatedBy);
            Assert.AreEqual(revisorId, updatedProgram.History.RevisedBy);
            DateTimeOffset.UtcNow.Should().BeCloseTo(updatedProgram.History.RevisedOn, DbContextHelper.DATE_PRECISION);
            CollectionAssert.AreEqual(updatedRowVersion, updatedProgram.RowVersion);

        }

        [TestMethod]
        public async Task TestUpdateAsync_CheckProperties()
        {
            Assert.AreEqual(0, context.Programs.Count());
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var now = DateTime.UtcNow;
            var creatorId = 1;
            var revisorId = 2;
            var originalRowVersion = new byte[1] { (byte)1 };
            var parentProgram = new Program
            {
                ProgramId = 2
            };
            var program = new Program
            {
                ProgramId = 1,
                Name = "old name",
                Description = "old description",
                StartDate = DateTimeOffset.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1.0),
                ProgramStatusId = ProgramStatus.Draft.Id,
                ParentProgram = null,
                RowVersion = originalRowVersion,
                Website = "old website",
                History = new History
                {
                    CreatedBy = creatorId,
                    CreatedOn = yesterday,
                    RevisedBy = creatorId,
                    RevisedOn = yesterday
                },
            };
            context.Programs.Add(program);
            context.Programs.Add(parentProgram);
            Assert.AreEqual(2, context.Programs.Count());

            var newName = "new name";
            var newDescription = "new description";
            var newStartDate = DateTimeOffset.UtcNow.AddDays(10.0);
            var newEndDate = DateTimeOffset.UtcNow.AddDays(12.0);
            var newProgramStatusId = ProgramStatus.Completed.Id;
            var newWebsite = "new website";
            var newParentProgramId = parentProgram.ProgramId;
            var updatedRowVersion = new byte[1] { (byte)1 };

            var updatedEcaProgram = new EcaProgram(
                updatedBy: new User(revisorId),
                id: program.ProgramId,
                name: newName,
                description: newDescription,
                startDate: newStartDate,
                endDate: newEndDate,
                ownerOrganizationId: ownerId,
                parentProgramId: newParentProgramId,
                programStatusId: newProgramStatusId,
                programRowVersion: updatedRowVersion,
                website: newWebsite,
                goalIds: null,
                pointOfContactIds: null,
                themeIds: null,
                regionIds: null,
                categoryIds: null,
                objectiveIds: null
                );
            await service.UpdateAsync(updatedEcaProgram);
            mockValidator.Verify(x => x.ValidateUpdate(It.IsAny<ProgramServiceValidationEntity>()), Times.Once());

            var updatedProgram = context.Programs.First();
            Assert.AreEqual(newDescription, updatedProgram.Description);
            Assert.AreEqual(newEndDate, updatedProgram.EndDate);
            Assert.AreEqual(newName, updatedProgram.Name);
            Assert.AreEqual(ownerId, updatedProgram.OwnerId);
            Assert.AreEqual(ownerId, updatedProgram.Owner.OrganizationId);
            Assert.AreEqual(program.ProgramId, updatedProgram.ProgramId);
            Assert.AreEqual(newProgramStatusId, updatedProgram.ProgramStatusId);
            Assert.AreEqual(newStartDate, updatedProgram.StartDate);
            Assert.AreEqual(newWebsite, updatedProgram.Website);

            Assert.AreEqual(yesterday, updatedProgram.History.CreatedOn);
            Assert.AreEqual(creatorId, updatedProgram.History.CreatedBy);

            CollectionAssert.AreEqual(updatedRowVersion, updatedProgram.RowVersion);

            Assert.AreEqual(revisorId, updatedProgram.History.RevisedBy);
            DateTimeOffset.UtcNow.Should().BeCloseTo(updatedProgram.History.RevisedOn, DbContextHelper.DATE_PRECISION);
        }

        [TestMethod]
        public void TestUpdate_CheckGoals()
        {
            Assert.AreEqual(0, context.Programs.Count());
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var now = DateTime.UtcNow;
            var creatorId = 1;
            var revisorId = 2;
            var parentProgram = new Program
            {
                ProgramId = 2
            };
            var focus = new Focus
            {
                FocusId = 100
            };
            var program = new Program
            {
                ProgramId = 1,
                Name = "old name",
                Description = "old description",
                StartDate = DateTimeOffset.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1.0),
                ProgramStatusId = ProgramStatus.Draft.Id,
                ParentProgram = null,
                Website = "old website",
                History = new History
                {
                    CreatedBy = creatorId,
                    CreatedOn = yesterday,
                    RevisedBy = revisorId,
                    RevisedOn = now
                },
            };
            context.Programs.Add(program);
            context.Programs.Add(parentProgram);
            Assert.AreEqual(2, context.Programs.Count());

            var newName = "new name";
            var newDescription = "new description";
            var newStartDate = DateTimeOffset.UtcNow.AddDays(10.0);
            var newEndDate = DateTimeOffset.UtcNow.AddDays(12.0);
            var newProgramStatusId = ProgramStatus.Completed.Id;
            var newWebsite = "new website";
            var newParentProgramId = parentProgram.ProgramId;

            var updatedEcaProgram = new EcaProgram(
                updatedBy: new User(revisorId),
                id: program.ProgramId,
                name: newName,
                description: newDescription,
                startDate: newStartDate,
                endDate: newEndDate,
                ownerOrganizationId: ownerId,
                parentProgramId: newParentProgramId,
                programStatusId: newProgramStatusId,
                programRowVersion: new byte[0],
                website: newWebsite,
                goalIds: new List<int> { 1 },
                pointOfContactIds: null,
                themeIds: null,
                regionIds: null,
                                categoryIds: null,
                objectiveIds: null
                );
            service.Update(updatedEcaProgram);

            var updatedProgram = context.Programs.First();
            Assert.AreEqual(1, updatedProgram.Goals.Count);
            Assert.AreEqual(0, updatedProgram.Regions.Count);
            Assert.AreEqual(0, updatedProgram.Themes.Count);
            Assert.AreEqual(0, updatedProgram.Contacts.Count);

            Assert.AreEqual(updatedEcaProgram.GoalIds.First(), updatedProgram.Goals.First().GoalId);
        }

        [TestMethod]
        public async Task TestUpdateAsync_CheckGoals()
        {
            Assert.AreEqual(0, context.Programs.Count());
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var now = DateTime.UtcNow;
            var creatorId = 1;
            var revisorId = 2;
            var parentProgram = new Program
            {
                ProgramId = 2
            };

            var program = new Program
            {
                ProgramId = 1,
                Name = "old name",
                Description = "old description",
                StartDate = DateTimeOffset.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1.0),
                ProgramStatusId = ProgramStatus.Draft.Id,
                ParentProgram = null,
                Website = "old website",
                History = new History
                {
                    CreatedBy = creatorId,
                    CreatedOn = yesterday,
                    RevisedBy = revisorId,
                    RevisedOn = now
                },
            };
            context.Programs.Add(program);
            context.Programs.Add(parentProgram);
            Assert.AreEqual(2, context.Programs.Count());

            var newName = "new name";
            var newDescription = "new description";
            var newStartDate = DateTimeOffset.UtcNow.AddDays(10.0);
            var newEndDate = DateTimeOffset.UtcNow.AddDays(12.0);
            var newProgramStatusId = ProgramStatus.Completed.Id;
            var newWebsite = "new website";
            var newParentProgramId = parentProgram.ProgramId;

            var updatedEcaProgram = new EcaProgram(
                updatedBy: new User(revisorId),
                id: program.ProgramId,
                name: newName,
                description: newDescription,
                startDate: newStartDate,
                endDate: newEndDate,
                ownerOrganizationId: ownerId,
                parentProgramId: newParentProgramId,
                programStatusId: newProgramStatusId,
                programRowVersion: new byte[0],
                website: newWebsite,
                goalIds: new List<int> { 1 },
                pointOfContactIds: null,
                themeIds: null,
                regionIds: null,
                                categoryIds: null,
                objectiveIds: null
                );
            await service.UpdateAsync(updatedEcaProgram);

            var updatedProgram = context.Programs.First();
            Assert.AreEqual(1, updatedProgram.Goals.Count);
            Assert.AreEqual(0, updatedProgram.Regions.Count);
            Assert.AreEqual(0, updatedProgram.Themes.Count);
            Assert.AreEqual(0, updatedProgram.Contacts.Count);

            Assert.AreEqual(updatedEcaProgram.GoalIds.First(), updatedProgram.Goals.First().GoalId);
        }

        [TestMethod]
        public void TestUpdate_CheckThemes()
        {
            Assert.AreEqual(0, context.Programs.Count());
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var now = DateTime.UtcNow;
            var creatorId = 1;
            var revisorId = 2;
            var parentProgram = new Program
            {
                ProgramId = 2
            };
            var program = new Program
            {
                ProgramId = 1,
                Name = "old name",
                Description = "old description",
                StartDate = DateTimeOffset.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1.0),
                ProgramStatusId = ProgramStatus.Draft.Id,
                ParentProgram = null,
                Website = "old website",
                History = new History
                {
                    CreatedBy = creatorId,
                    CreatedOn = yesterday,
                    RevisedBy = revisorId,
                    RevisedOn = now
                },
            };
            context.Programs.Add(program);
            context.Programs.Add(parentProgram);
            Assert.AreEqual(2, context.Programs.Count());

            var newName = "new name";
            var newDescription = "new description";
            var newStartDate = DateTimeOffset.UtcNow.AddDays(10.0);
            var newEndDate = DateTimeOffset.UtcNow.AddDays(12.0);
            var newProgramStatusId = ProgramStatus.Completed.Id;
            var newWebsite = "new website";
            var newParentProgramId = parentProgram.ProgramId;

            var updatedEcaProgram = new EcaProgram(
                updatedBy: new User(revisorId),
                id: program.ProgramId,
                name: newName,
                description: newDescription,
                startDate: newStartDate,
                endDate: newEndDate,
                ownerOrganizationId: ownerId,
                parentProgramId: newParentProgramId,
                programStatusId: newProgramStatusId,
                programRowVersion: new byte[0],
                website: newWebsite,
                goalIds: null,
                pointOfContactIds: null,
                themeIds: new List<int> { 1 },
                regionIds: null,
                                categoryIds: null,
                objectiveIds: null
                );
            service.Update(updatedEcaProgram);

            var updatedProgram = context.Programs.First();
            Assert.AreEqual(0, updatedProgram.Goals.Count);
            Assert.AreEqual(0, updatedProgram.Regions.Count);
            Assert.AreEqual(1, updatedProgram.Themes.Count);
            Assert.AreEqual(0, updatedProgram.Contacts.Count);
            Assert.AreEqual(updatedEcaProgram.ThemeIds.First(), updatedProgram.Themes.First().ThemeId);
        }

        [TestMethod]
        public async Task TestUpdateAsync_CheckThemes()
        {
            Assert.AreEqual(0, context.Programs.Count());
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var now = DateTime.UtcNow;
            var creatorId = 1;
            var revisorId = 2;
            var parentProgram = new Program
            {
                ProgramId = 2
            };
            var program = new Program
            {
                ProgramId = 1,
                Name = "old name",
                Description = "old description",
                StartDate = DateTimeOffset.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1.0),
                ProgramStatusId = ProgramStatus.Draft.Id,
                ParentProgram = null,
                Website = "old website",
                History = new History
                {
                    CreatedBy = creatorId,
                    CreatedOn = yesterday,
                    RevisedBy = revisorId,
                    RevisedOn = now
                },
            };
            context.Programs.Add(program);
            context.Programs.Add(parentProgram);
            Assert.AreEqual(2, context.Programs.Count());

            var newName = "new name";
            var newDescription = "new description";
            var newStartDate = DateTimeOffset.UtcNow.AddDays(10.0);
            var newEndDate = DateTimeOffset.UtcNow.AddDays(12.0);
            var newProgramStatusId = ProgramStatus.Completed.Id;
            var newWebsite = "new website";
            var newParentProgramId = parentProgram.ProgramId;

            var updatedEcaProgram = new EcaProgram(
                updatedBy: new User(revisorId),
                id: program.ProgramId,
                name: newName,
                description: newDescription,
                startDate: newStartDate,
                endDate: newEndDate,
                ownerOrganizationId: ownerId,
                parentProgramId: newParentProgramId,
                programStatusId: newProgramStatusId,
                programRowVersion: new byte[0],
                website: newWebsite,
                goalIds: null,
                pointOfContactIds: null,
                themeIds: new List<int> { 1 },
                regionIds: null,
                                categoryIds: null,
                objectiveIds: null
                );
            await service.UpdateAsync(updatedEcaProgram);

            var updatedProgram = context.Programs.First();
            Assert.AreEqual(0, updatedProgram.Goals.Count);
            Assert.AreEqual(0, updatedProgram.Regions.Count);
            Assert.AreEqual(1, updatedProgram.Themes.Count);
            Assert.AreEqual(0, updatedProgram.Contacts.Count);
            Assert.AreEqual(updatedEcaProgram.ThemeIds.First(), updatedProgram.Themes.First().ThemeId);
        }

        [TestMethod]
        public void TestUpdate_CheckPointOfContacts()
        {
            Assert.AreEqual(0, context.Programs.Count());
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var now = DateTime.UtcNow;
            var creatorId = 1;
            var revisorId = 2;
            var parentProgram = new Program
            {
                ProgramId = 2
            };

            var program = new Program
            {
                ProgramId = 1,
                Name = "old name",
                Description = "old description",
                StartDate = DateTimeOffset.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1.0),

                ProgramStatusId = ProgramStatus.Draft.Id,
                ParentProgram = null,
                Website = "old website",
                History = new History
                {
                    CreatedBy = creatorId,
                    CreatedOn = yesterday,
                    RevisedBy = revisorId,
                    RevisedOn = now
                },
            };
            context.Programs.Add(program);
            context.Programs.Add(parentProgram);
            Assert.AreEqual(2, context.Programs.Count());

            var newName = "new name";
            var newDescription = "new description";
            var newStartDate = DateTimeOffset.UtcNow.AddDays(10.0);
            var newEndDate = DateTimeOffset.UtcNow.AddDays(12.0);
            var newProgramStatusId = ProgramStatus.Completed.Id;
            var newWebsite = "new website";
            var newParentProgramId = parentProgram.ProgramId;

            var updatedEcaProgram = new EcaProgram(
                updatedBy: new User(revisorId),
                id: program.ProgramId,
                name: newName,
                description: newDescription,
                startDate: newStartDate,
                endDate: newEndDate,
                ownerOrganizationId: ownerId,
                parentProgramId: newParentProgramId,
                programStatusId: newProgramStatusId,
                programRowVersion: new byte[0],
                website: newWebsite,
                goalIds: null,
                pointOfContactIds: new List<int> { 1 },
                themeIds: null,
                regionIds: null,
                                categoryIds: null,
                objectiveIds: null
                );
            service.Update(updatedEcaProgram);

            var updatedProgram = context.Programs.First();
            Assert.AreEqual(0, updatedProgram.Goals.Count);
            Assert.AreEqual(0, updatedProgram.Regions.Count);
            Assert.AreEqual(0, updatedProgram.Themes.Count);
            Assert.AreEqual(1, updatedProgram.Contacts.Count);
            Assert.AreEqual(updatedEcaProgram.ContactIds.First(), updatedProgram.Contacts.First().ContactId);
        }

        [TestMethod]
        public async Task TestUpdateAsync_CheckPointOfContacts()
        {
            Assert.AreEqual(0, context.Programs.Count());
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var now = DateTime.UtcNow;
            var creatorId = 1;
            var revisorId = 2;
            var parentProgram = new Program
            {
                ProgramId = 2
            };
            var program = new Program
            {
                ProgramId = 1,
                Name = "old name",
                Description = "old description",
                StartDate = DateTimeOffset.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1.0),
                ProgramStatusId = ProgramStatus.Draft.Id,
                ParentProgram = null,
                Website = "old website",
                History = new History
                {
                    CreatedBy = creatorId,
                    CreatedOn = yesterday,
                    RevisedBy = revisorId,
                    RevisedOn = now
                },
            };
            context.Programs.Add(program);
            context.Programs.Add(parentProgram);
            Assert.AreEqual(2, context.Programs.Count());

            var newName = "new name";
            var newDescription = "new description";
            var newStartDate = DateTimeOffset.UtcNow.AddDays(10.0);
            var newEndDate = DateTimeOffset.UtcNow.AddDays(12.0);
            var newProgramStatusId = ProgramStatus.Completed.Id;
            var newWebsite = "new website";
            var newParentProgramId = parentProgram.ProgramId;

            var updatedEcaProgram = new EcaProgram(
                updatedBy: new User(revisorId),
                id: program.ProgramId,
                name: newName,
                description: newDescription,
                startDate: newStartDate,
                endDate: newEndDate,
                ownerOrganizationId: ownerId,
                parentProgramId: newParentProgramId,
                programStatusId: newProgramStatusId,
                programRowVersion: new byte[0],
                website: newWebsite,
                goalIds: null,
                pointOfContactIds: new List<int> { 1 },
                themeIds: null,
                regionIds: null,
                                categoryIds: null,
                objectiveIds: null
                );
            await service.UpdateAsync(updatedEcaProgram);

            var updatedProgram = context.Programs.First();
            Assert.AreEqual(0, updatedProgram.Goals.Count);
            Assert.AreEqual(0, updatedProgram.Regions.Count);
            Assert.AreEqual(0, updatedProgram.Themes.Count);
            Assert.AreEqual(1, updatedProgram.Contacts.Count);
            Assert.AreEqual(updatedEcaProgram.ContactIds.First(), updatedProgram.Contacts.First().ContactId);
        }

        [TestMethod]
        public void TestUpdate_CheckRegions()
        {
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });
            var region = new Location { LocationId = 1, LocationTypeId = LocationType.Region.Id };
            context.Locations.Add(region);
            Assert.AreEqual(0, context.Programs.Count());
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var now = DateTime.UtcNow;
            var creatorId = 1;
            var revisorId = 2;
            var parentProgram = new Program
            {
                ProgramId = 2
            };

            var program = new Program
            {
                ProgramId = 1,
                Name = "old name",
                Description = "old description",
                StartDate = DateTimeOffset.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1.0),
                ProgramStatusId = ProgramStatus.Draft.Id,
                ParentProgram = null,
                Website = "old website",
                History = new History
                {
                    CreatedBy = creatorId,
                    CreatedOn = yesterday,
                    RevisedBy = revisorId,
                    RevisedOn = now
                },
            };
            context.Programs.Add(program);
            context.Programs.Add(parentProgram);
            Assert.AreEqual(2, context.Programs.Count());

            var newName = "new name";
            var newDescription = "new description";
            var newStartDate = DateTimeOffset.UtcNow.AddDays(10.0);
            var newEndDate = DateTimeOffset.UtcNow.AddDays(12.0);
            var newProgramStatusId = ProgramStatus.Completed.Id;
            var newWebsite = "new website";
            var newParentProgramId = parentProgram.ProgramId;

            var updatedEcaProgram = new EcaProgram(
                updatedBy: new User(revisorId),
                id: program.ProgramId,
                name: newName,
                description: newDescription,
                startDate: newStartDate,
                endDate: newEndDate,
                ownerOrganizationId: ownerId,
                parentProgramId: newParentProgramId,
                programStatusId: newProgramStatusId,
                programRowVersion: new byte[0],
                website: newWebsite,
                goalIds: null,
                pointOfContactIds: null,
                themeIds: null,
                regionIds: new List<int> { region.LocationId },
                                categoryIds: null,
                objectiveIds: null
                );
            service.Update(updatedEcaProgram);

            var updatedProgram = context.Programs.First();
            Assert.AreEqual(0, updatedProgram.Goals.Count);
            Assert.AreEqual(1, updatedProgram.Regions.Count);
            Assert.AreEqual(0, updatedProgram.Themes.Count);
            Assert.AreEqual(0, updatedProgram.Contacts.Count);
            Assert.AreEqual(updatedEcaProgram.RegionIds.First(), updatedProgram.Regions.First().LocationId);
        }

        [TestMethod]
        public async Task TestUpdateAsync_CheckRegions()
        {
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });
            var region = new Location { LocationId = 1, LocationTypeId = LocationType.Region.Id };
            context.Locations.Add(region);
            Assert.AreEqual(0, context.Programs.Count());
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var now = DateTime.UtcNow;
            var creatorId = 1;
            var revisorId = 2;
            var parentProgram = new Program
            {
                ProgramId = 2
            };
            var program = new Program
            {
                ProgramId = 1,
                Name = "old name",
                Description = "old description",
                StartDate = DateTimeOffset.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1.0),
                ProgramStatusId = ProgramStatus.Draft.Id,
                ParentProgram = null,
                Website = "old website",
                History = new History
                {
                    CreatedBy = creatorId,
                    CreatedOn = yesterday,
                    RevisedBy = revisorId,
                    RevisedOn = now
                },
            };

            context.Programs.Add(program);
            context.Programs.Add(parentProgram);
            Assert.AreEqual(2, context.Programs.Count());

            var newName = "new name";
            var newDescription = "new description";
            var newStartDate = DateTimeOffset.UtcNow.AddDays(10.0);
            var newEndDate = DateTimeOffset.UtcNow.AddDays(12.0);
            var newProgramStatusId = ProgramStatus.Completed.Id;
            var newWebsite = "new website";
            var newParentProgramId = parentProgram.ProgramId;

            var updatedEcaProgram = new EcaProgram(
                updatedBy: new User(revisorId),
                id: program.ProgramId,
                name: newName,
                description: newDescription,
                startDate: newStartDate,
                endDate: newEndDate,
                ownerOrganizationId: ownerId,
                parentProgramId: newParentProgramId,
                programStatusId: newProgramStatusId,
                programRowVersion: new byte[0],
                website: newWebsite,
                goalIds: null,
                pointOfContactIds: null,
                themeIds: null,
                regionIds: new List<int> { region.LocationId },
                                categoryIds: null,
                objectiveIds: null
                );
            await service.UpdateAsync(updatedEcaProgram);

            var updatedProgram = context.Programs.First();
            Assert.AreEqual(0, updatedProgram.Goals.Count);
            Assert.AreEqual(1, updatedProgram.Regions.Count);
            Assert.AreEqual(0, updatedProgram.Themes.Count);
            Assert.AreEqual(0, updatedProgram.Contacts.Count);
            Assert.AreEqual(updatedEcaProgram.RegionIds.First(), updatedProgram.Regions.First().LocationId);
        }

        [TestMethod]
        [ExpectedException(typeof(ModelNotFoundException))]
        public void TestUpdate_ModelNotFoundException()
        {
            var newOwnerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = newOwnerId });
            context.Foci.Add(new Focus { FocusId = 1 });
            var newName = "new name";
            var newDescription = "new description";
            var newStartDate = DateTimeOffset.UtcNow.AddDays(10.0);
            var newEndDate = DateTimeOffset.UtcNow.AddDays(12.0);
            var newProgramStatusId = ProgramStatus.Completed.Id;
            var newWebsite = "new website";
            var newParentProgramId = 12;

            var updatedEcaProgram = new EcaProgram(
                updatedBy: new User(1),
                id: 1,
                name: newName,
                description: newDescription,
                startDate: newStartDate,
                endDate: newEndDate,
                ownerOrganizationId: newOwnerId,
                parentProgramId: newParentProgramId,
                programStatusId: newProgramStatusId,
                programRowVersion: new byte[0],
                website: newWebsite,
                goalIds: null,
                pointOfContactIds: null,
                themeIds: null,
                regionIds: null,
                                categoryIds: null,
                objectiveIds: null
                );
            service.Update(updatedEcaProgram);
        }

        [TestMethod]
        [ExpectedException(typeof(ModelNotFoundException))]
        public async Task TestUpdateAsync_ModelNotFoundException()
        {
            var newOwnerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = newOwnerId });
            context.Foci.Add(new Focus { FocusId = 1 });
            var newName = "new name";
            var newDescription = "new description";
            var newStartDate = DateTimeOffset.UtcNow.AddDays(10.0);
            var newEndDate = DateTimeOffset.UtcNow.AddDays(12.0);
            var newProgramStatusId = ProgramStatus.Completed.Id;
            var newWebsite = "new website";

            var newParentProgramId = 12;

            var updatedEcaProgram = new EcaProgram(
                updatedBy: new User(1),
                id: 1,
                name: newName,
                description: newDescription,
                startDate: newStartDate,
                endDate: newEndDate,
                ownerOrganizationId: newOwnerId,
                parentProgramId: newParentProgramId,
                programStatusId: newProgramStatusId,
                programRowVersion: new byte[0],
                website: newWebsite,
                goalIds: null,
                pointOfContactIds: null,
                themeIds: null,
                regionIds: null,
                                categoryIds: null,
                objectiveIds: null
                );
            await service.UpdateAsync(updatedEcaProgram);
        }

        #endregion

        [TestMethod]
        public void TestSetRegions()
        {
            var original = new Location { LocationId = 1 };

            var program = new Program();
            program.Regions.Add(original);

            var newLocation = new Location { LocationId = 2 };
            var newLocationIds = new List<int> { newLocation.LocationId };
            service.SetRegions(newLocationIds, program);
            Assert.AreEqual(1, program.Regions.Count);
            Assert.AreEqual(newLocation.LocationId, program.Regions.First().LocationId);
        }

        [TestMethod]
        public async Task TestGetLocationTypeIds()
        {
            var expectedTypeIds = new List<int> { LocationType.Region.Id };
            var region = new Location { LocationId = 1, LocationTypeId = LocationType.Region.Id };

            context.Locations.Add(region);
            CollectionAssert.AreEqual(expectedTypeIds, service.GetLocationTypeIds(new List<int> { region.LocationId }));
            CollectionAssert.AreEqual(expectedTypeIds, await service.GetLocationTypeIdsAsync(new List<int> { region.LocationId }));
        }
    }
}

