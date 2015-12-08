using ECA.Business.Queries.Models.Admin;
using FluentAssertions;
using ECA.Business.Queries.Models.Office;
using ECA.Business.Service.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Filter;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.Data;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Test.Service.Admin
{
    [TestClass]
    public class OfficeServiceTest
    {
        private TestEcaContext context;
        private OfficeService service;

        [TestInitialize]
        public void TestInit()
        {

            context = new TestEcaContext();
            service = new OfficeService(context);
        }

        #region Get
        [TestMethod]
        public async Task TestGetOfficeById_CheckProperties()
        {
            var now = DateTimeOffset.UtcNow;
            var office = new Organization
            {
                OrganizationId = 1,
                OrganizationTypeId = OrganizationType.Office.Id,
                Name = "office",
                Description = "office desc",
                OfficeSymbol = "symbol"

            };
            office.History.RevisedOn = now;
            var contact = new Contact
            {
                ContactId = 2,
                FullName = "full name",
            };
            var program = new Program
            {
                ProgramId = 3,
                Owner = office
            };
            var goal = new Goal
            {
                GoalId = 4,
                GoalName = "goal"
            };
            var theme = new Theme
            {
                ThemeId = 5,
                ThemeName = "theme"
            };

            office.OwnerPrograms.Add(program);
            office.Contacts.Add(contact);
            program.Themes.Add(theme);
            program.Goals.Add(goal);

            context.Organizations.Add(office);
            context.Contacts.Add(contact);
            context.Programs.Add(program);
            context.Goals.Add(goal);
            context.Themes.Add(theme);

            Action<OfficeDTO> tester = (dto) =>
            {
                Assert.IsNotNull(dto);
                Assert.AreEqual(office.OrganizationId, dto.Id);
                Assert.AreEqual(office.Name, dto.Name);
                Assert.AreEqual(office.OfficeSymbol, dto.OfficeSymbol);
                Assert.AreEqual(office.Description, dto.Description);
                Assert.AreEqual(office.History.RevisedOn, dto.RevisedOn);

                Assert.AreEqual(1, dto.Contacts.Count());
                Assert.AreEqual(contact.ContactId, dto.Contacts.First().Id);
                Assert.AreEqual(contact.FullName, dto.Contacts.First().Value);

                Assert.AreEqual(1, dto.Goals.Count());
                Assert.AreEqual(goal.GoalId, dto.Goals.First().Id);
                Assert.AreEqual(goal.GoalName, dto.Goals.First().Value);

                Assert.AreEqual(1, dto.Themes.Count());
                Assert.AreEqual(theme.ThemeId, dto.Themes.First().Id);
                Assert.AreEqual(theme.ThemeName, dto.Themes.First().Value);
            };

            var serviceResults = service.GetOfficeById(office.OrganizationTypeId);
            var serviceResultsAsync = await service.GetOfficeByIdAsync(office.OrganizationId);

            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetOfficeById_OfficeDoesNotExist()
        {
            var org = new Organization
            {
                OrganizationId = 1,
                OrganizationTypeId = OrganizationType.Office.Id
            };
            context.Organizations.Add(org);
            Action<OfficeDTO> tester = (dto) =>
            {
                Assert.IsNull(dto);
            };

            var serviceResults = service.GetOfficeById(org.OrganizationId + 1);
            var serviceResultsAsync = await service.GetOfficeByIdAsync(org.OrganizationId + 1);

            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetOfficeById_NoPrograms()
        {
            var office = new Organization
            {
                OrganizationId = 1,
                OrganizationTypeId = OrganizationType.Office.Id,
                Name = "office",
                Description = "office desc"
            };
            context.Organizations.Add(office);
            Action<OfficeDTO> tester = (dto) =>
            {
                Assert.IsNotNull(dto);
                Assert.AreEqual(office.OrganizationId, dto.Id);
                Assert.AreEqual(office.Name, dto.Name);
                Assert.AreEqual(office.Description, dto.Description);
                Assert.AreEqual(0, dto.Contacts.Count());
                Assert.AreEqual(0, dto.Goals.Count());
                Assert.AreEqual(0, dto.Themes.Count());
            };

            var serviceResults = service.GetOfficeById(office.OrganizationTypeId);
            var serviceResultsAsync = await service.GetOfficeByIdAsync(office.OrganizationId);

            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetOfficeById_MultiplePrograms()
        {
            //multiple programs are sharing the same themes and goals so make sure we're getting distinct
            var office = new Organization
            {
                OrganizationId = 1,
                OrganizationTypeId = OrganizationType.Office.Id,
                Name = "office",
                Description = "office desc"
            };
            var contact = new Contact
            {
                ContactId = 2,
                FullName = "full name",
            };
            var program = new Program
            {
                ProgramId = 3,
                Owner = office
            };
            var program2 = new Program
            {
                ProgramId = 10,
                Owner = office
            };
            var goal = new Goal
            {
                GoalId = 4,
                GoalName = "goal"
            };
            var theme = new Theme
            {
                ThemeId = 5,
                ThemeName = "theme"
            };
            office.OwnerPrograms.Add(program);
            office.OwnerPrograms.Add(program2);
            office.Contacts.Add(contact);
            program.Themes.Add(theme);
            program2.Themes.Add(theme);

            program.Goals.Add(goal);
            program2.Goals.Add(goal);

            context.Organizations.Add(office);
            context.Contacts.Add(contact);
            context.Programs.Add(program);
            context.Programs.Add(program2);
            context.Goals.Add(goal);
            context.Themes.Add(theme);

            Action<OfficeDTO> tester = (dto) =>
            {
                Assert.IsNotNull(dto);
                Assert.AreEqual(office.OrganizationId, dto.Id);
                Assert.AreEqual(office.Name, dto.Name);
                Assert.AreEqual(office.Description, dto.Description);

                Assert.AreEqual(1, dto.Contacts.Count());
                Assert.AreEqual(1, dto.Goals.Count());
                Assert.AreEqual(1, dto.Themes.Count());
            };

            var serviceResults = service.GetOfficeById(office.OrganizationTypeId);
            var serviceResultsAsync = await service.GetOfficeByIdAsync(office.OrganizationId);

            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetOfficeById_OrganizationIsNotOffice()
        {
            var office = new Organization
            {
                OrganizationId = 1,
                OrganizationTypeId = OrganizationType.Other.Id,
                Name = "office",
                Description = "office desc"
            };
            context.Organizations.Add(office);

            Action<OfficeDTO> tester = (dto) =>
            {
                Assert.IsNull(dto);
            };

            var serviceResults = service.GetOfficeById(office.OrganizationId);
            var serviceResultsAsync = await service.GetOfficeByIdAsync(office.OrganizationId);

            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetOfficeById_OrganizationIsAnOffice()
        {
            var office = new Organization
            {
                OrganizationId = 1,
                OrganizationTypeId = OrganizationType.Office.Id,
                Name = "office",
                Description = "office desc"
            };
            context.Organizations.Add(office);

            Action<OfficeDTO> tester = (dto) =>
            {
                Assert.IsNotNull(dto);
            };

            var serviceResults = service.GetOfficeById(office.OrganizationId);
            var serviceResultsAsync = await service.GetOfficeByIdAsync(office.OrganizationId);

            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetOfficeById_OrganizationIsADivison()
        {
            var office = new Organization
            {
                OrganizationId = 1,
                OrganizationTypeId = OrganizationType.Division.Id,
                Name = "office",
                Description = "office desc"
            };
            context.Organizations.Add(office);

            Action<OfficeDTO> tester = (dto) =>
            {
                Assert.IsNotNull(dto);
            };

            var serviceResults = service.GetOfficeById(office.OrganizationId);
            var serviceResultsAsync = await service.GetOfficeByIdAsync(office.OrganizationId);

            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetOfficeById_OrganizationIsABranch()
        {
            var office = new Organization
            {
                OrganizationId = 1,
                OrganizationTypeId = OrganizationType.Branch.Id,
                Name = "office",
                Description = "office desc"
            };
            context.Organizations.Add(office);

            Action<OfficeDTO> tester = (dto) =>
            {
                Assert.IsNotNull(dto);
            };

            var serviceResults = service.GetOfficeById(office.OrganizationId);
            var serviceResultsAsync = await service.GetOfficeByIdAsync(office.OrganizationId);

            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        #endregion

        #region Get Child Programs
        [TestMethod]
        public async Task TestGetChildOfficesAsync_CheckProperties()
        {
            var parentOfficeId = 1;
            var childOfficeId = 2;
            var organizationType = new OrganizationType
            {
                OrganizationTypeId = OrganizationType.Branch.Id,
                OrganizationTypeName = OrganizationType.Branch.Value
            };
            var parentOffice = new Organization
            {
                OrganizationId = parentOfficeId
            };
            var childOffice = new Organization
            {
                Description = "desc",
                Name = "name",
                OfficeSymbol = "symbol",
                OrganizationId = childOfficeId,
                OrganizationType = organizationType,
                OrganizationTypeId = organizationType.OrganizationTypeId,
                ParentOrganization = parentOffice,
            };

            context.OrganizationTypes.Add(organizationType);
            context.Organizations.Add(parentOffice);
            context.Organizations.Add(childOffice);

            Action<List<SimpleOfficeDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Count);
                var testChildOffice = results.First();
                Assert.AreEqual(childOffice.Description, testChildOffice.Description);
                Assert.AreEqual(childOffice.Name, testChildOffice.Name);
                Assert.AreEqual(childOffice.OfficeSymbol, testChildOffice.OfficeSymbol);
                Assert.AreEqual(childOffice.OrganizationId, testChildOffice.OrganizationId);
                Assert.AreEqual(childOffice.OrganizationType.OrganizationTypeName, testChildOffice.OrganizationType);
                Assert.AreEqual(childOffice.OrganizationTypeId, testChildOffice.OrganizationTypeId);
                Assert.AreEqual(1, testChildOffice.OfficeLevel);
                Assert.AreEqual(parentOffice.OrganizationId, testChildOffice.ParentOrganization_OrganizationId);

            };

            var serviceResults = service.GetChildOffices(parentOfficeId);
            var serviceResultsAsync = await service.GetChildOfficesAsync(parentOfficeId);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetChildOfficesAsync_ParentOfficeDoesNotExist()
        {
            var parentOfficeId = 1;
            Action<List<SimpleOfficeDTO>> tester = (results) =>
            {
                Assert.AreEqual(0, results.Count);
            };
            var serviceResults = service.GetChildOffices(parentOfficeId);
            var serviceResultsAsync = await service.GetChildOfficesAsync(parentOfficeId);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetChildOfficesAsync_NoChildOffices()
        {
            var parentOfficeId = 1;
            var organizationType = new OrganizationType
            {
                OrganizationTypeId = OrganizationType.Branch.Id,
                OrganizationTypeName = OrganizationType.Branch.Value
            };
            var parentOffice = new Organization
            {
                OrganizationId = parentOfficeId
            };

            context.OrganizationTypes.Add(organizationType);
            context.Organizations.Add(parentOffice);

            Action<List<SimpleOfficeDTO>> tester = (results) =>
            {
                Assert.AreEqual(0, results.Count);
            };

            var serviceResults = service.GetChildOffices(parentOfficeId);
            var serviceResultsAsync = await service.GetChildOfficesAsync(parentOfficeId);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetChildOfficesAsync_InvalidOrganizationType()
        {
            var parentOfficeId = 1;
            var childOfficeId = 2;
            var organizationType = new OrganizationType
            {
                OrganizationTypeId = OrganizationType.ForeignEducationalInstitution.Id,
                OrganizationTypeName = OrganizationType.ForeignEducationalInstitution.Value
            };
            var parentOffice = new Organization
            {
                OrganizationId = parentOfficeId
            };
            var childOffice = new Organization
            {
                Description = "desc",
                Name = "name",
                OfficeSymbol = "symbol",
                OrganizationId = childOfficeId,
                OrganizationType = organizationType,
                OrganizationTypeId = organizationType.OrganizationTypeId,
                ParentOrganization = parentOffice,
            };

            context.OrganizationTypes.Add(organizationType);
            context.Organizations.Add(parentOffice);
            context.Organizations.Add(childOffice);

            Action<List<SimpleOfficeDTO>> tester = (results) =>
            {
                Assert.AreEqual(0, results.Count);
            };

            var serviceResults = service.GetChildOffices(parentOfficeId);
            var serviceResultsAsync = await service.GetChildOfficesAsync(parentOfficeId);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        #endregion

        #region Get Programs
        [TestMethod]
        public async Task TestGetPrograms_OrganizationDoesExist()
        {
            using (ShimsContext.Create())
            {
                var officeId = 1;
                var list = new List<OrganizationProgramDTO>();
                var dto1 = new OrganizationProgramDTO
                {
                    Description = "A",
                    Name = "A",
                    OrgName = "org",
                    Owner_OrganizationId = officeId,
                    ParentProgram_ProgramId = 2,
                    ProgramId = 20,
                    ProgramLevel = 1

                };
                list.Add(dto1);
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
                Action<PagedQueryResults<OrganizationProgramDTO>> tester = (results) =>
                {
                    Assert.AreEqual(0, results.Total);
                    Assert.AreEqual(0, results.Results.Count);
                };

                var defaultSorter = new ExpressionSorter<OrganizationProgramDTO>(x => x.Name, SortDirection.Ascending);
                var queryOperator = new QueryableOperator<OrganizationProgramDTO>(0, 10, defaultSorter);

                var serviceResults = service.GetPrograms(officeId - 1, queryOperator);
                var serviceResultsAsync = await service.GetProgramsAsync(officeId - 1, queryOperator);
                tester(serviceResults);
                tester(serviceResultsAsync);
            }
        }


        [TestMethod]
        public async Task TestGetPrograms_DefaultSorter()
        {
            using (ShimsContext.Create())
            {
                var officeId = 1;
                var list = new List<OrganizationProgramDTO>();
                var dto1 = new OrganizationProgramDTO
                {
                    Description = "A",
                    Name = "A",
                    OrgName = "org",
                    Owner_OrganizationId = officeId,
                    ParentProgram_ProgramId = 2,
                    ProgramId = 20,
                    ProgramLevel = 1

                };
                var dto2 = new OrganizationProgramDTO
                {
                    Description = "B",
                    Name = "B",
                    OrgName = "org",
                    Owner_OrganizationId = officeId,
                    ParentProgram_ProgramId = 2,
                    ProgramId = 10,
                    ProgramLevel = 1

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
                Action<PagedQueryResults<OrganizationProgramDTO>> tester = (results) =>
                {
                    Assert.AreEqual(2, results.Total);
                    Assert.AreEqual(2, results.Results.Count);
                    Assert.AreEqual(dto1.ProgramId, results.Results.First().ProgramId);
                };

                var defaultSorter = new ExpressionSorter<OrganizationProgramDTO>(x => x.Name, SortDirection.Ascending);
                var queryOperator = new QueryableOperator<OrganizationProgramDTO>(0, 10, defaultSorter);

                var serviceResults = service.GetPrograms(officeId, queryOperator);
                var serviceResultsAsync = await service.GetProgramsAsync(officeId, queryOperator);
                tester(serviceResults);
                tester(serviceResultsAsync);
            }
        }

        [TestMethod]
        public async Task TestGetPrograms_Sorter()
        {
            using (ShimsContext.Create())
            {
                var officeId = 1;
                var list = new List<OrganizationProgramDTO>();
                var dto1 = new OrganizationProgramDTO
                {
                    Description = "A",
                    Name = "A",
                    OrgName = "org",
                    Owner_OrganizationId = officeId,
                    ParentProgram_ProgramId = 2,
                    ProgramId = 20,
                    ProgramLevel = 1

                };
                var dto2 = new OrganizationProgramDTO
                {
                    Description = "B",
                    Name = "B",
                    OrgName = "org",
                    Owner_OrganizationId = officeId,
                    ParentProgram_ProgramId = 2,
                    ProgramId = 10,
                    ProgramLevel = 1

                };
                list.Add(dto1);
                list.Add(dto2);
                list = list.OrderBy(x => x.Name).ToList();
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
                Action<PagedQueryResults<OrganizationProgramDTO>> tester = (results) =>
                {
                    Assert.AreEqual(2, results.Total);
                    Assert.AreEqual(2, results.Results.Count);
                    Assert.AreEqual(dto2.ProgramId, results.Results.First().ProgramId);
                };

                var defaultSorter = new ExpressionSorter<OrganizationProgramDTO>(x => x.Name, SortDirection.Ascending);
                var testSorter = new ExpressionSorter<OrganizationProgramDTO>(x => x.Name, SortDirection.Descending);
                var queryOperator = new QueryableOperator<OrganizationProgramDTO>(0, 10, defaultSorter, null, new List<ISorter> { testSorter });

                var serviceResults = service.GetPrograms(officeId, queryOperator);
                var serviceResultsAsync = await service.GetProgramsAsync(officeId, queryOperator);
                tester(serviceResults);
                tester(serviceResultsAsync);
            }
        }

        [TestMethod]
        public async Task TestGetPrograms_Filter()
        {
            using (ShimsContext.Create())
            {
                var officeId = 1;
                var list = new List<OrganizationProgramDTO>();
                var dto1 = new OrganizationProgramDTO
                {
                    Description = "A",
                    Name = "A",
                    OrgName = "org",
                    Owner_OrganizationId = officeId,
                    ParentProgram_ProgramId = 2,
                    ProgramId = 20,
                    ProgramLevel = 1

                };
                var dto2 = new OrganizationProgramDTO
                {
                    Description = "B",
                    Name = "B",
                    OrgName = "org",
                    Owner_OrganizationId = officeId,
                    ParentProgram_ProgramId = 2,
                    ProgramId = 10,
                    ProgramLevel = 1

                };
                list.Add(dto1);
                list.Add(dto2);
                list = list.OrderBy(x => x.Name).ToList();
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
                Action<PagedQueryResults<OrganizationProgramDTO>> tester = (results) =>
                {
                    Assert.AreEqual(1, results.Total);
                    Assert.AreEqual(1, results.Results.Count);
                    Assert.AreEqual(dto1.ProgramId, results.Results.First().ProgramId);
                };

                var defaultSorter = new ExpressionSorter<OrganizationProgramDTO>(x => x.Name, SortDirection.Ascending);
                var filter = new ExpressionFilter<OrganizationProgramDTO>(x => x.Name, ComparisonType.Equal, dto1.Name);
                var queryOperator = new QueryableOperator<OrganizationProgramDTO>(0, 10, defaultSorter, new List<IFilter> { filter }, null);

                var serviceResults = service.GetPrograms(officeId, queryOperator);
                var serviceResultsAsync = await service.GetProgramsAsync(officeId, queryOperator);
                tester(serviceResults);
                tester(serviceResultsAsync);
            }
        }

        [TestMethod]
        public async Task TestGetPrograms_Keyword()
        {
            using (ShimsContext.Create())
            {
                var officeId = 1;
                var list = new List<OrganizationProgramDTO>();
                var dto1 = new OrganizationProgramDTO
                {
                    Description = "A",
                    Name = "A",
                    OrgName = "org",
                    Owner_OrganizationId = officeId,
                    ParentProgram_ProgramId = 2,
                    ProgramId = 20,
                    ProgramLevel = 1

                };
                var dto2 = new OrganizationProgramDTO
                {
                    Description = "B",
                    Name = "B",
                    OrgName = "org",
                    Owner_OrganizationId = officeId,
                    ParentProgram_ProgramId = 2,
                    ProgramId = 10,
                    ProgramLevel = 1

                };
                list.Add(dto1);
                list.Add(dto2);
                list = list.OrderBy(x => x.Name).ToList();
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
                Action<PagedQueryResults<OrganizationProgramDTO>> tester = (results) =>
                {
                    Assert.AreEqual(1, results.Total);
                    Assert.AreEqual(1, results.Results.Count);
                    Assert.AreEqual(dto1.ProgramId, results.Results.First().ProgramId);
                };

                var defaultSorter = new ExpressionSorter<OrganizationProgramDTO>(x => x.Name, SortDirection.Ascending);
                var filter = new SimpleKeywordFilter<OrganizationProgramDTO>(new HashSet<string> { dto1.Name }, x => x.Name);
                var queryOperator = new QueryableOperator<OrganizationProgramDTO>(0, 10, defaultSorter, new List<IFilter> { filter }, null);

                var serviceResults = service.GetPrograms(officeId, queryOperator);
                var serviceResultsAsync = await service.GetProgramsAsync(officeId, queryOperator);
                tester(serviceResults);
                tester(serviceResultsAsync);
            }
        }


        [TestMethod]
        public async Task TestGetPrograms_Paging()
        {
            using (ShimsContext.Create())
            {
                var officeId = 1;
                var list = new List<OrganizationProgramDTO>();
                var dto1 = new OrganizationProgramDTO
                {
                    Description = "A",
                    Name = "A",
                    OrgName = "org",
                    Owner_OrganizationId = officeId,
                    ParentProgram_ProgramId = 2,
                    ProgramId = 20,
                    ProgramLevel = 1

                };
                var dto2 = new OrganizationProgramDTO
                {
                    Description = "B",
                    Name = "B",
                    OrgName = "org",
                    Owner_OrganizationId = officeId,
                    ParentProgram_ProgramId = 2,
                    ProgramId = 10,
                    ProgramLevel = 1

                };
                list.Add(dto1);
                list.Add(dto2);
                list = list.OrderBy(x => x.Name).ToList();
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
                Action<PagedQueryResults<OrganizationProgramDTO>> tester = (results) =>
                {
                    Assert.AreEqual(2, results.Total);
                    Assert.AreEqual(1, results.Results.Count);
                    Assert.AreEqual(dto1.ProgramId, results.Results.First().ProgramId);
                };

                var defaultSorter = new ExpressionSorter<OrganizationProgramDTO>(x => x.Name, SortDirection.Ascending);
                var queryOperator = new QueryableOperator<OrganizationProgramDTO>(0, 1, defaultSorter, null, null);

                var serviceResults = service.GetPrograms(officeId, queryOperator);
                var serviceResultsAsync = await service.GetProgramsAsync(officeId, queryOperator);
                tester(serviceResults);
                tester(serviceResultsAsync);
            }
        }
        #endregion

        #region Get Offices
        [TestMethod]
        public async Task TestGetOffices_DefaultSorter()
        {
            using (ShimsContext.Create())
            {
                var list = new List<SimpleOfficeDTO>();
                var dto1 = new SimpleOfficeDTO
                {
                    Description = "A",
                    Name = "A",
                    OfficeLevel = 1,
                    OfficeSymbol = "symbol",
                    OrganizationId = 2,
                    OrganizationType = "org",
                    OrganizationTypeId = OrganizationType.Office.Id,
                    ParentOrganization_OrganizationId = 1

                };
                var dto2 = new SimpleOfficeDTO
                {
                    Description = "A",
                    Name = "G",
                    OfficeLevel = 1,
                    OfficeSymbol = "symbol",
                    OrganizationId = 2,
                    OrganizationType = "org",
                    OrganizationTypeId = OrganizationType.Office.Id,
                    ParentOrganization_OrganizationId = 1

                };
                list.Add(dto1);
                list.Add(dto2);
                list = list.OrderBy(x => x.Name).ToList();
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
                Action<PagedQueryResults<SimpleOfficeDTO>> tester = (results) =>
                {
                    Assert.AreEqual(2, results.Total);
                    Assert.AreEqual(2, results.Results.Count);
                    Assert.AreEqual(dto2.OrganizationId, results.Results.First().OrganizationId);
                    Assert.AreEqual(dto1.OrganizationId, results.Results.First().OrganizationId);
                };

                var defaultSorter = new ExpressionSorter<SimpleOfficeDTO>(x => x.Name, SortDirection.Descending);
                var queryOperator = new QueryableOperator<SimpleOfficeDTO>(0, 10, defaultSorter);

                var serviceResults = service.GetOffices(queryOperator);
                var serviceResultsAsync = await service.GetOfficesAsync(queryOperator);
                tester(serviceResults);
                tester(serviceResultsAsync);
            }
        }

        [TestMethod]
        public async Task TestGetOffices_Sorter()
        {
            using (ShimsContext.Create())
            {
                var list = new List<SimpleOfficeDTO>();
                var dto1 = new SimpleOfficeDTO
                {
                    Description = "A",
                    Name = "A",
                    OfficeLevel = 1,
                    OfficeSymbol = "symbol",
                    OrganizationId = 2,
                    OrganizationType = "org",
                    OrganizationTypeId = OrganizationType.Office.Id,
                    ParentOrganization_OrganizationId = 1

                };
                var dto2 = new SimpleOfficeDTO
                {
                    Description = "A",
                    Name = "G",
                    OfficeLevel = 1,
                    OfficeSymbol = "symbol",
                    OrganizationId = 2,
                    OrganizationType = "org",
                    OrganizationTypeId = OrganizationType.Office.Id,
                    ParentOrganization_OrganizationId = 1

                };
                list.Add(dto1);
                list.Add(dto2);
                list = list.OrderBy(x => x.Name).ToList();
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
                Action<PagedQueryResults<SimpleOfficeDTO>> tester = (results) =>
                {
                    Assert.AreEqual(2, results.Total);
                    Assert.AreEqual(2, results.Results.Count);
                    Assert.AreEqual(dto2.OrganizationId, results.Results.First().OrganizationId);
                    Assert.AreEqual(dto1.OrganizationId, results.Results.First().OrganizationId);
                };

                var defaultSorter = new ExpressionSorter<SimpleOfficeDTO>(x => x.Name, SortDirection.Ascending);
                var sorter = new ExpressionSorter<SimpleOfficeDTO>(x => x.Name, SortDirection.Descending);
                var queryOperator = new QueryableOperator<SimpleOfficeDTO>(0, 10, defaultSorter, null, new List<ISorter> { sorter });

                var serviceResults = service.GetOffices(queryOperator);
                var serviceResultsAsync = await service.GetOfficesAsync(queryOperator);
                tester(serviceResults);
                tester(serviceResultsAsync);
            }
        }

        [TestMethod]
        public async Task TestGetOffices_Filter()
        {
            using (ShimsContext.Create())
            {
                var list = new List<SimpleOfficeDTO>();
                var dto1 = new SimpleOfficeDTO
                {
                    Description = "A",
                    Name = "A",
                    OfficeLevel = 1,
                    OfficeSymbol = "symbol",
                    OrganizationId = 2,
                    OrganizationType = "org",
                    OrganizationTypeId = OrganizationType.Office.Id,
                    ParentOrganization_OrganizationId = 1

                };
                var dto2 = new SimpleOfficeDTO
                {
                    Description = "A",
                    Name = "G",
                    OfficeLevel = 1,
                    OfficeSymbol = "symbol",
                    OrganizationId = 2,
                    OrganizationType = "org",
                    OrganizationTypeId = OrganizationType.Office.Id,
                    ParentOrganization_OrganizationId = 1

                };
                list.Add(dto1);
                list.Add(dto2);
                list = list.OrderBy(x => x.Name).ToList();
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
                Action<PagedQueryResults<SimpleOfficeDTO>> tester = (results) =>
                {
                    Assert.AreEqual(1, results.Total);
                    Assert.AreEqual(1, results.Results.Count);
                    Assert.AreEqual(dto1.OrganizationId, results.Results.First().OrganizationId);
                };

                var defaultSorter = new ExpressionSorter<SimpleOfficeDTO>(x => x.Name, SortDirection.Ascending);
                var filter = new ExpressionFilter<SimpleOfficeDTO>(x => x.Name, ComparisonType.Equal, dto1.Name);
                var queryOperator = new QueryableOperator<SimpleOfficeDTO>(0, 10, defaultSorter, new List<IFilter> { filter }, null);

                var serviceResults = service.GetOffices(queryOperator);
                var serviceResultsAsync = await service.GetOfficesAsync(queryOperator);
                tester(serviceResults);
                tester(serviceResultsAsync);
            }
        }

        [TestMethod]
        public async Task TestGetOffices_Keyword()
        {
            using (ShimsContext.Create())
            {
                var list = new List<SimpleOfficeDTO>();
                var dto1 = new SimpleOfficeDTO
                {
                    Description = "A",
                    Name = "A",
                    OfficeLevel = 1,
                    OfficeSymbol = "symbol",
                    OrganizationId = 2,
                    OrganizationType = "org",
                    OrganizationTypeId = OrganizationType.Office.Id,
                    ParentOrganization_OrganizationId = 1

                };
                var dto2 = new SimpleOfficeDTO
                {
                    Description = "A",
                    Name = "G",
                    OfficeLevel = 1,
                    OfficeSymbol = "symbol",
                    OrganizationId = 2,
                    OrganizationType = "org",
                    OrganizationTypeId = OrganizationType.Office.Id,
                    ParentOrganization_OrganizationId = 1

                };
                list.Add(dto1);
                list.Add(dto2);
                list = list.OrderBy(x => x.Name).ToList();
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
                Action<PagedQueryResults<SimpleOfficeDTO>> tester = (results) =>
                {
                    Assert.AreEqual(1, results.Total);
                    Assert.AreEqual(1, results.Results.Count);
                    Assert.AreEqual(dto1.OrganizationId, results.Results.First().OrganizationId);
                };

                var defaultSorter = new ExpressionSorter<SimpleOfficeDTO>(x => x.Name, SortDirection.Ascending);
                var filter = new SimpleKeywordFilter<SimpleOfficeDTO>(new HashSet<string> { dto1.Name }, x => x.Name);
                var queryOperator = new QueryableOperator<SimpleOfficeDTO>(0, 10, defaultSorter, new List<IFilter> { filter }, null);

                var serviceResults = service.GetOffices(queryOperator);
                var serviceResultsAsync = await service.GetOfficesAsync(queryOperator);
                tester(serviceResults);
                tester(serviceResultsAsync);
            }
        }

        [TestMethod]
        public async Task TestGetOffices_Paging()
        {
            using (ShimsContext.Create())
            {
                var list = new List<SimpleOfficeDTO>();
                var dto1 = new SimpleOfficeDTO
                {
                    Description = "A",
                    Name = "A",
                    OfficeLevel = 1,
                    OfficeSymbol = "symbol",
                    OrganizationId = 2,
                    OrganizationType = "org",
                    OrganizationTypeId = OrganizationType.Office.Id,
                    ParentOrganization_OrganizationId = 1

                };
                var dto2 = new SimpleOfficeDTO
                {
                    Description = "A",
                    Name = "G",
                    OfficeLevel = 1,
                    OfficeSymbol = "symbol",
                    OrganizationId = 2,
                    OrganizationType = "org",
                    OrganizationTypeId = OrganizationType.Office.Id,
                    ParentOrganization_OrganizationId = 1

                };
                list.Add(dto1);
                list.Add(dto2);
                list = list.OrderBy(x => x.Name).ToList();
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
                Action<PagedQueryResults<SimpleOfficeDTO>> tester = (results) =>
                {
                    Assert.AreEqual(2, results.Total);
                    Assert.AreEqual(1, results.Results.Count);
                    Assert.AreEqual(dto1.OrganizationId, results.Results.First().OrganizationId);
                };

                var defaultSorter = new ExpressionSorter<SimpleOfficeDTO>(x => x.Name, SortDirection.Ascending);
                var queryOperator = new QueryableOperator<SimpleOfficeDTO>(0, 1, defaultSorter);

                var serviceResults = service.GetOffices(queryOperator);
                var serviceResultsAsync = await service.GetOfficesAsync(queryOperator);
                tester(serviceResults);
                tester(serviceResultsAsync);
            }
        }
        #endregion

        #region Settings
        [TestMethod]
        public async Task TestGetSettings()
        {
            var office = new Organization
            {
                OrganizationId = 1,
            };
            var officeSetting = new OfficeSetting
            {
                Name = "Name",
                Office = office,
                OfficeId = office.OrganizationId,
                OfficeSettingId = 1,
                Value = "value"
            };
            office.OfficeSettings.Add(officeSetting);
            context.OfficeSettings.Add(officeSetting);
            context.Organizations.Add(office);

            Action<IEnumerable<OfficeSettingDTO>> tester = (settings) =>
            {
                Assert.AreEqual(1, settings.Count());
                var first = settings.First();
                Assert.AreEqual(officeSetting.Name, first.Name);
                Assert.AreEqual(officeSetting.OfficeId, first.OfficeId);
                Assert.AreEqual(officeSetting.Value, first.Value);
            };

            var serviceResult = service.GetSettings(office.OrganizationId);
            var serviceResultAsync = await service.GetSettingsAsync(office.OrganizationId);
            tester(serviceResult);
            tester(serviceResultAsync);
        }

        [TestMethod]
        public async Task TestGetSettings_OfficeDoesNotExist()
        {
            Action<IEnumerable<OfficeSettingDTO>> tester = (settings) =>
            {
                Assert.AreEqual(0, settings.Count());
            };
            var serviceResult = service.GetSettings(1);
            var serviceResultAsync = await service.GetSettingsAsync(1);
            tester(serviceResult);
            tester(serviceResultAsync);
        }

        [TestMethod]
        public async Task TestGetSettings_DuplicatedKeys()
        {
            var office = new Organization
            {
                OrganizationId = 1,
            };
            var officeSetting1 = new OfficeSetting
            {
                Name = "Name",
                Office = office,
                OfficeId = office.OrganizationId,
                OfficeSettingId = 1,
                Value = "value1"
            };
            var officeSetting2 = new OfficeSetting
            {
                Name = "Name",
                Office = office,
                OfficeId = office.OrganizationId,
                OfficeSettingId = 1,
                Value = "value2"
            };
            context.OfficeSettings.Add(officeSetting1);
            context.OfficeSettings.Add(officeSetting2);
            service.Invoking(x => x.GetSettings(office.OrganizationId)).ShouldThrow<NotSupportedException>()
                .WithMessage(String.Format("The office with id [{0}] has duplicated settings with keys [{1}].", office.OrganizationId, "Name"));

            Func<Task> f = async () =>
            {
                await service.GetSettingsAsync(office.OrganizationId);
            };

            f.ShouldThrow<NotSupportedException>()
                .WithMessage(String.Format("The office with id [{0}] has duplicated settings with keys [{1}].", office.OrganizationId, "Name"));

        }

        [TestMethod]
        public async Task TestGetValue()
        {
            var office = new Organization
            {
                OrganizationId = 1,
            };
            var officeSetting = new OfficeSetting
            {
                Name = "Name",
                Office = office,
                OfficeId = office.OrganizationId,
                OfficeSettingId = 1,
                Value = "value"
            };
            office.OfficeSettings.Add(officeSetting);
            context.OfficeSettings.Add(officeSetting);
            context.Organizations.Add(office);

            Action<string> tester = (setting) =>
            {
                Assert.AreEqual(setting, officeSetting.Value);
            };

            var serviceResult = service.GetValue(office.OrganizationId, officeSetting.Name);
            var serviceResultAsync = await service.GetValueAsync(office.OrganizationId, officeSetting.Name);
            tester(serviceResult);
            tester(serviceResultAsync);
        }

        [TestMethod]
        public async Task TestGetValue_KeyDoesNotExist()
        {
            var office = new Organization
            {
                OrganizationId = 1,
            };
            context.Organizations.Add(office);

            Action<string> tester = (setting) =>
            {
                Assert.IsNull(setting);
            };

            var serviceResult = service.GetValue(office.OrganizationId, "x");
            var serviceResultAsync = await service.GetValueAsync(office.OrganizationId, "x");
            tester(serviceResult);
            tester(serviceResultAsync);
        }

        [TestMethod]
        public async Task TestGetValue_OfficeDoesNotExist()
        {
            var office = new Organization
            {
                OrganizationId = 1,
            };
            var officeSetting = new OfficeSetting
            {
                Name = "Name",
                Office = office,
                OfficeId = office.OrganizationId,
                OfficeSettingId = 1,
                Value = "value"
            };
            office.OfficeSettings.Add(officeSetting);
            context.OfficeSettings.Add(officeSetting);
            context.Organizations.Add(office);
            Action<string> tester = (setting) =>
            {
                Assert.IsNull(setting);
            };

            var serviceResult = service.GetValue(-1, officeSetting.Name);
            var serviceResultAsync = await service.GetValueAsync(-1, officeSetting.Name);
            tester(serviceResult);
            tester(serviceResultAsync);
        }

        [TestMethod]
        public void TestGetStringValue()
        {
            var name = "name";
            var value = "value";
            var defaultValue = "default";
            var setting = new OfficeSettingDTO
            {
                Id = 1,
                Name = name,
                OfficeId = 2,
                Value = value
            };
            var settings = new List<OfficeSettingDTO>
            {
                setting
            };

            var testValue = service.GetStringValue(name, settings, defaultValue);
            Assert.AreEqual(value, testValue);
        }

        [TestMethod]
        public void TestGetStringValue_ShouldReturnDefaultValue()
        {
            var name = "name";
            var value = "value";
            var defaultValue = "default";
            var setting = new OfficeSettingDTO
            {
                Id = 1,
                Name = name,
                OfficeId = 2,
                Value = value
            };
            var settings = new List<OfficeSettingDTO>
            {
                setting
            };

            var testValue = service.GetStringValue("idontexist", settings, defaultValue);
            Assert.AreEqual(defaultValue, testValue);
        }

        [TestMethod]
        public void TestGetStringValue_NoSettings()
        {
            var name = "name";
            var value = "value";
            var defaultValue = "default";
            var setting = new OfficeSettingDTO
            {
                Id = 1,
                Name = name,
                OfficeId = 2,
                Value = value
            };
            var settings = new List<OfficeSettingDTO>();

            var testValue = service.GetStringValue(name, settings, defaultValue);
            Assert.AreEqual(defaultValue, testValue);
        }

        [TestMethod]
        public void TestGetStringValueAsBool()
        {
            var name = "name";
            var value = true;
            var defaultValue = false;
            var setting = new OfficeSettingDTO
            {
                Id = 1,
                Name = name,
                OfficeId = 2,
                Value = value.ToString()
            };
            var settings = new List<OfficeSettingDTO>
            {
                setting
            };

            var testValue = service.GetStringValueAsBool(name, settings, defaultValue);
            Assert.AreEqual(value, testValue);
        }

        [TestMethod]
        public void TestGetStringValueAsBool_DefaultValue()
        {
            var name = "name";
            var value = false;
            var defaultValue = true;
            var setting = new OfficeSettingDTO
            {
                Id = 1,
                Name = name,
                OfficeId = 2,
                Value = value.ToString()
            };
            var settings = new List<OfficeSettingDTO>
            {
                setting
            };

            var testValue = service.GetStringValueAsBool("idontexist", settings, defaultValue);
            Assert.AreEqual(defaultValue, testValue);
        }

        [TestMethod]
        public void TestGetStringValueAsBool_NoSettings()
        {
            var name = "name";
            var value = false;
            var defaultValue = true;
            var setting = new OfficeSettingDTO
            {
                Id = 1,
                Name = name,
                OfficeId = 2,
                Value = value.ToString()
            };
            var settings = new List<OfficeSettingDTO>();
            var testValue = service.GetStringValueAsBool(name, settings, defaultValue);
            Assert.AreEqual(defaultValue, testValue);
        }

        [TestMethod]
        public void TestGetStringValueAsBool_UnableToParseBoolString()
        {
            var name = "name";
            var value = "xyz";
            var defaultValue = true;
            var setting = new OfficeSettingDTO
            {
                Id = 1,
                Name = name,
                OfficeId = 2,
                Value = value
            };
            var settings = new List<OfficeSettingDTO>
            {
                setting
            };
            var testValue = service.GetStringValueAsBool(name, settings, defaultValue);
            Assert.AreEqual(defaultValue, testValue);
        }

        [TestMethod]
        public void TestGetStringValueAsInt()
        {
            var name = "name";
            var value = 1;
            var defaultValue = -1;
            var setting = new OfficeSettingDTO
            {
                Id = 1,
                Name = name,
                OfficeId = 2,
                Value = value.ToString()
            };
            var settings = new List<OfficeSettingDTO>
            {
                setting
            };

            var testValue = service.GetStringValueAsInt(name, settings, defaultValue);
            Assert.AreEqual(value, testValue);
        }

        [TestMethod]
        public void TestGetStringValueAsInt_DefaultValue()
        {
            var name = "name";
            var value = 1;
            var defaultValue = 2;
            var setting = new OfficeSettingDTO
            {
                Id = 1,
                Name = name,
                OfficeId = 2,
                Value = value.ToString()
            };
            var settings = new List<OfficeSettingDTO>
            {
                setting
            };

            var testValue = service.GetStringValueAsInt("idontexist", settings, defaultValue);
            Assert.AreEqual(defaultValue, testValue);
        }

        [TestMethod]
        public void TestGetStringValueAsInt_NoSettings()
        {
            var name = "name";
            var value = 0;
            var defaultValue = 1;
            var setting = new OfficeSettingDTO
            {
                Id = 1,
                Name = name,
                OfficeId = 2,
                Value = value.ToString()
            };
            var settings = new List<OfficeSettingDTO>();
            var testValue = service.GetStringValueAsInt(name, settings, defaultValue);
            Assert.AreEqual(defaultValue, testValue);
        }

        [TestMethod]
        public void TestGetStringValueAsInt_UnableToParseIntString()
        {
            var name = "name";
            var value = "xyz";
            var defaultValue = 1;
            var setting = new OfficeSettingDTO
            {
                Id = 1,
                Name = name,
                OfficeId = 2,
                Value = value
            };
            var settings = new List<OfficeSettingDTO>
            {
                setting
            };
            var testValue = service.GetStringValueAsInt(name, settings, defaultValue);
            Assert.AreEqual(defaultValue, testValue);
        }


        [TestMethod]
        public void TestHasSetting()
        {
            var name = "name";
            var value = "xyz";
            var setting = new OfficeSettingDTO
            {
                Id = 1,
                Name = name,
                OfficeId = 2,
                Value = value
            };
            var settings = new List<OfficeSettingDTO>
            {
                setting
            };
            Assert.IsTrue(service.HasSetting(name, settings));
        }

        [TestMethod]
        public void TestHasSetting_NoSettings()
        {
            var name = "name";
            var value = "xyz";
            var setting = new OfficeSettingDTO
            {
                Id = 1,
                Name = name,
                OfficeId = 2,
                Value = value
            };
            var settings = new List<OfficeSettingDTO>();
            Assert.IsFalse(service.HasSetting(name, settings));
        }

        [TestMethod]
        public void TestHasSetting_DoesNotHaveSetting()
        {
            var name = "name";
            var value = "xyz";
            var setting = new OfficeSettingDTO
            {
                Id = 1,
                Name = name,
                OfficeId = 2,
                Value = value
            };
            var settings = new List<OfficeSettingDTO>();
            Assert.IsFalse(service.HasSetting("idontexist", settings));
        }

        [TestMethod]
        public async Task TestGetOfficeSettings_CheckLabels_LabelsHaveValues()
        {
            var office = new Organization
            {
                OrganizationId = 1,
                Name = "office"
            };
            var objectiveLabelSetting = new OfficeSetting
            {
                Name = OfficeSetting.OBJECTIVE_SETTING_KEY,
                Office = office,
                OfficeId = office.OrganizationId,
                OfficeSettingId = 1,
                Value = "obj"
            };
            var categoryLabelSetting = new OfficeSetting
            {
                Name = OfficeSetting.CATEGORY_SETTING_KEY,
                Office = office,
                OfficeId = office.OrganizationId,
                OfficeSettingId = 2,
                Value = "cat"
            };
            var focusLabelSetting = new OfficeSetting
            {
                Name = OfficeSetting.FOCUS_SETTING_KEY,
                Office = office,
                OfficeId = office.OrganizationId,
                OfficeSettingId = 3,
                Value = "foc"
            };
            var justificationLabelSetting = new OfficeSetting
            {
                Name = OfficeSetting.JUSTIFICATION_SETTING_KEY,
                Office = office,
                OfficeId = office.OrganizationId,
                OfficeSettingId = 4,
                Value = "jus"
            };
            context.OfficeSettings.Add(objectiveLabelSetting);
            context.OfficeSettings.Add(categoryLabelSetting);
            context.OfficeSettings.Add(focusLabelSetting);
            context.OfficeSettings.Add(justificationLabelSetting);
            context.Organizations.Add(office);

            Action<OfficeSettings> tester = (testOfficeSettings) =>
            {
                Assert.AreEqual(objectiveLabelSetting.Value, testOfficeSettings.ObjectiveLabel);
                Assert.AreEqual(categoryLabelSetting.Value, testOfficeSettings.CategoryLabel);
                Assert.AreEqual(focusLabelSetting.Value, testOfficeSettings.FocusLabel);
                Assert.AreEqual(justificationLabelSetting.Value, testOfficeSettings.JustificationLabel);
            };

            var serviceResult = service.GetOfficeSettings(office.OrganizationId);
            var serviceResultAsync = await service.GetOfficeSettingsAsync(office.OrganizationId);
            tester(serviceResult);
            tester(serviceResultAsync);
        }

        [TestMethod]
        public async Task TestGetOfficeSettings_CheckLabels_LabelsDoNotValues()
        {
            var office = new Organization
            {
                OrganizationId = 1,
                Name = "office"
            };
            context.Organizations.Add(office);

            Action<OfficeSettings> tester = (testOfficeSettings) =>
            {
                Assert.AreEqual(OfficeSettings.OBJECTIVE_DEFAULT_LABEL, testOfficeSettings.ObjectiveLabel);
                Assert.AreEqual(OfficeSettings.CATEGORY_DEFAULT_LABEL, testOfficeSettings.CategoryLabel);
                Assert.AreEqual(OfficeSettings.FOCUS_DEFAULT_LABEL, testOfficeSettings.FocusLabel);
                Assert.AreEqual(OfficeSettings.JUSTIFICATION_DEFAULT_LABEL, testOfficeSettings.JustificationLabel);
            };

            var serviceResult = service.GetOfficeSettings(office.OrganizationId);
            var serviceResultAsync = await service.GetOfficeSettingsAsync(office.OrganizationId);
            tester(serviceResult);
            tester(serviceResultAsync);
        }

        [TestMethod]
        public async Task TestGetOfficeSettings_CheckIsObjectiveRequired_NoSettings()
        {
            var office = new Organization
            {
                OrganizationId = 1,
                Name = "office"
            };
            context.Organizations.Add(office);

            Action<OfficeSettings> tester = (testOfficeSettings) =>
            {
                Assert.IsFalse(testOfficeSettings.IsObjectiveRequired);
            };

            var serviceResult = service.GetOfficeSettings(office.OrganizationId);
            var serviceResultAsync = await service.GetOfficeSettingsAsync(office.OrganizationId);
            tester(serviceResult);
            tester(serviceResultAsync);
        }

        [TestMethod]
        public async Task TestGetOfficeSettings_CheckIsCategoryRequired_NoSettings()
        {
            var office = new Organization
            {
                OrganizationId = 1,
                Name = "office"
            };
            context.Organizations.Add(office);

            Action<OfficeSettings> tester = (testOfficeSettings) =>
            {
                Assert.IsFalse(testOfficeSettings.IsCategoryRequired);
            };

            var serviceResult = service.GetOfficeSettings(office.OrganizationId);
            var serviceResultAsync = await service.GetOfficeSettingsAsync(office.OrganizationId);
            tester(serviceResult);
            tester(serviceResultAsync);
        }

        [TestMethod]
        public async Task TestGetOfficeSettings_CheckIsObjectiveRequired_HasObjectiveSetting()
        {
            var office = new Organization
            {
                OrganizationId = 1,
                Name = "office"
            };
            var objectiveLabelSetting = new OfficeSetting
            {
                Name = OfficeSetting.OBJECTIVE_SETTING_KEY,
                Office = office,
                OfficeId = office.OrganizationId,
                OfficeSettingId = 1,
                Value = "obj"
            };
            context.Organizations.Add(office);
            context.OfficeSettings.Add(objectiveLabelSetting);

            Action<OfficeSettings> tester = (testOfficeSettings) =>
            {
                Assert.IsTrue(testOfficeSettings.IsObjectiveRequired);
            };

            var serviceResult = service.GetOfficeSettings(office.OrganizationId);
            var serviceResultAsync = await service.GetOfficeSettingsAsync(office.OrganizationId);
            tester(serviceResult);
            tester(serviceResultAsync);
        }

        [TestMethod]
        public async Task TestGetOfficeSettings_CheckIsObjectiveRequired_HasJustificationSetting()
        {
            var office = new Organization
            {
                OrganizationId = 1,
                Name = "office"
            };
            var setting = new OfficeSetting
            {
                Name = OfficeSetting.JUSTIFICATION_SETTING_KEY,
                Office = office,
                OfficeId = office.OrganizationId,
                OfficeSettingId = 1,
                Value = "setting"
            };
            context.Organizations.Add(office);
            context.OfficeSettings.Add(setting);

            Action<OfficeSettings> tester = (testOfficeSettings) =>
            {
                Assert.IsTrue(testOfficeSettings.IsObjectiveRequired);
            };

            var serviceResult = service.GetOfficeSettings(office.OrganizationId);
            var serviceResultAsync = await service.GetOfficeSettingsAsync(office.OrganizationId);
            tester(serviceResult);
            tester(serviceResultAsync);
        }

        [TestMethod]
        public async Task TestGetOfficeSettings_CheckIsObjectiveRequired_DoesNotHaveObjectiveOrJustificationSetting()
        {
            var office = new Organization
            {
                OrganizationId = 1,
                Name = "office"
            };
            var setting = new OfficeSetting
            {
                Name = OfficeSetting.FOCUS_SETTING_KEY,
                Office = office,
                OfficeId = office.OrganizationId,
                OfficeSettingId = 1,
                Value = "setting"
            };
            context.Organizations.Add(office);
            context.OfficeSettings.Add(setting);

            Action<OfficeSettings> tester = (testOfficeSettings) =>
            {
                Assert.IsFalse(testOfficeSettings.IsObjectiveRequired);
            };

            var serviceResult = service.GetOfficeSettings(office.OrganizationId);
            var serviceResultAsync = await service.GetOfficeSettingsAsync(office.OrganizationId);
            tester(serviceResult);
            tester(serviceResultAsync);
        }

        [TestMethod]
        public async Task TestGetOfficeSettings_CheckIsCategoryRequired_HasCategorySetting()
        {
            var office = new Organization
            {
                OrganizationId = 1,
                Name = "office"
            };
            var objectiveLabelSetting = new OfficeSetting
            {
                Name = OfficeSetting.CATEGORY_SETTING_KEY,
                Office = office,
                OfficeId = office.OrganizationId,
                OfficeSettingId = 1,
                Value = "obj"
            };
            context.Organizations.Add(office);
            context.OfficeSettings.Add(objectiveLabelSetting);

            Action<OfficeSettings> tester = (testOfficeSettings) =>
            {
                Assert.IsTrue(testOfficeSettings.IsCategoryRequired);
            };

            var serviceResult = service.GetOfficeSettings(office.OrganizationId);
            var serviceResultAsync = await service.GetOfficeSettingsAsync(office.OrganizationId);
            tester(serviceResult);
            tester(serviceResultAsync);
        }

        [TestMethod]
        public async Task TestGetOfficeSettings_CheckIsCategoryRequired_HasFocusSetting()
        {
            var office = new Organization
            {
                OrganizationId = 1,
                Name = "office"
            };
            var setting = new OfficeSetting
            {
                Name = OfficeSetting.FOCUS_SETTING_KEY,
                Office = office,
                OfficeId = office.OrganizationId,
                OfficeSettingId = 1,
                Value = "setting"
            };
            context.Organizations.Add(office);
            context.OfficeSettings.Add(setting);

            Action<OfficeSettings> tester = (testOfficeSettings) =>
            {
                Assert.IsTrue(testOfficeSettings.IsCategoryRequired);
            };

            var serviceResult = service.GetOfficeSettings(office.OrganizationId);
            var serviceResultAsync = await service.GetOfficeSettingsAsync(office.OrganizationId);
            tester(serviceResult);
            tester(serviceResultAsync);
        }

        [TestMethod]
        public async Task TestGetOfficeSettings_CheckIsCategoryRequired_DoesNotHaveCategoryOrFocusSetting()
        {
            var office = new Organization
            {
                OrganizationId = 1,
                Name = "office"
            };
            var setting = new OfficeSetting
            {
                Name = OfficeSetting.OBJECTIVE_SETTING_KEY,
                Office = office,
                OfficeId = office.OrganizationId,
                OfficeSettingId = 1,
                Value = "setting"
            };
            context.Organizations.Add(office);
            context.OfficeSettings.Add(setting);

            Action<OfficeSettings> tester = (testOfficeSettings) =>
            {
                Assert.IsFalse(testOfficeSettings.IsCategoryRequired);
            };

            var serviceResult = service.GetOfficeSettings(office.OrganizationId);
            var serviceResultAsync = await service.GetOfficeSettingsAsync(office.OrganizationId);
            tester(serviceResult);
            tester(serviceResultAsync);
        }

        [TestMethod]
        public async Task TestGetOfficeSettings_CheckMaxAndMinFoci_HasCategorySetting_HasMaxMinSettings()
        {
            var office = new Organization
            {
                OrganizationId = 1,
                Name = "office"
            };
            var objectiveLabelSetting = new OfficeSetting
            {
                Name = OfficeSetting.CATEGORY_SETTING_KEY,
                Office = office,
                OfficeId = office.OrganizationId,
                OfficeSettingId = 1,
                Value = "obj"
            };
            var minFociSetting = new OfficeSetting
            {
                Name = OfficeSetting.MIN_FOCUS_KEY,
                Office = office,
                OfficeId = office.OrganizationId,
                OfficeSettingId = 2,
                Value = "1"
            };
            var maxFociSetting = new OfficeSetting
            {
                Name = OfficeSetting.MAX_FOCUS_KEY,
                Office = office,
                OfficeId = office.OrganizationId,
                OfficeSettingId = 2,
                Value = "2"
            };
            context.Organizations.Add(office);
            context.OfficeSettings.Add(objectiveLabelSetting);
            context.OfficeSettings.Add(minFociSetting);
            context.OfficeSettings.Add(maxFociSetting);

            Action<OfficeSettings> tester = (testOfficeSettings) =>
            {
                Assert.IsTrue(testOfficeSettings.IsCategoryRequired);
                Assert.AreEqual(Int32.Parse(minFociSetting.Value), testOfficeSettings.MinimumRequiredFoci);
                Assert.AreEqual(Int32.Parse(maxFociSetting.Value), testOfficeSettings.MaximumRequiredFoci);
            };

            var serviceResult = service.GetOfficeSettings(office.OrganizationId);
            var serviceResultAsync = await service.GetOfficeSettingsAsync(office.OrganizationId);
            tester(serviceResult);
            tester(serviceResultAsync);
        }

        [TestMethod]
        public async Task TestGetOfficeSettings_CheckMaxAndMinFoci_HasCategorySetting_DoesNotHaveMaxMinSettings()
        {
            var office = new Organization
            {
                OrganizationId = 1,
                Name = "office"
            };
            var objectiveLabelSetting = new OfficeSetting
            {
                Name = OfficeSetting.CATEGORY_SETTING_KEY,
                Office = office,
                OfficeId = office.OrganizationId,
                OfficeSettingId = 1,
                Value = "obj"
            };
            context.Organizations.Add(office);
            context.OfficeSettings.Add(objectiveLabelSetting);

            Action<OfficeSettings> tester = (testOfficeSettings) =>
            {
                Assert.IsTrue(testOfficeSettings.IsCategoryRequired);
                Assert.AreEqual(1, testOfficeSettings.MinimumRequiredFoci);
                Assert.AreEqual(1, testOfficeSettings.MaximumRequiredFoci);
            };

            var serviceResult = service.GetOfficeSettings(office.OrganizationId);
            var serviceResultAsync = await service.GetOfficeSettingsAsync(office.OrganizationId);
            tester(serviceResult);
            tester(serviceResultAsync);
        }

        [TestMethod]
        public async Task TestGetOfficeSettings_CheckMaxAndMinFoci_DoesNotHaveCategorySetting()
        {
            var office = new Organization
            {
                OrganizationId = 1,
                Name = "office"
            };
            var minFociSetting = new OfficeSetting
            {
                Name = OfficeSetting.MIN_FOCUS_KEY,
                Office = office,
                OfficeId = office.OrganizationId,
                OfficeSettingId = 2,
                Value = "1"
            };
            var maxFociSetting = new OfficeSetting
            {
                Name = OfficeSetting.MAX_FOCUS_KEY,
                Office = office,
                OfficeId = office.OrganizationId,
                OfficeSettingId = 2,
                Value = "2"
            };
            context.Organizations.Add(office);
            context.OfficeSettings.Add(minFociSetting);
            context.OfficeSettings.Add(maxFociSetting);

            Action<OfficeSettings> tester = (testOfficeSettings) =>
            {
                Assert.IsFalse(testOfficeSettings.IsCategoryRequired);
                Assert.AreEqual(-1, testOfficeSettings.MinimumRequiredFoci);
                Assert.AreEqual(-1, testOfficeSettings.MaximumRequiredFoci);
            };

            var serviceResult = service.GetOfficeSettings(office.OrganizationId);
            var serviceResultAsync = await service.GetOfficeSettingsAsync(office.OrganizationId);
            tester(serviceResult);
            tester(serviceResultAsync);
        }

        #endregion

        #region Data Point Configurations
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
                CollectionAssert.AreEqual(new List<int> { dto1.OrganizationId }, serviceResults.ToList());
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
                CollectionAssert.AreEqual(new List<int> { dto1.OrganizationId, dto2.OrganizationId }, serviceResults.ToList());
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
        public async Task TestGetOfficeDataPointConfigurationsAsync()
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

                var serviceResult = await service.GetOfficeDataPointConfigurationsAsync(dataPointConfig.OfficeId.Value);
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
        public async Task TestGetOfficeDataPointConfigurationsAsync_Empty()
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

                var serviceResult = await service.GetOfficeDataPointConfigurationsAsync(1);
                Assert.AreEqual(0, serviceResult.Count);
            }
        }

        [TestMethod]
        public async Task GetOfficeDataPointConfigurationsAsync_OneParentOffice()
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

                var serviceResult = await service.GetOfficeDataPointConfigurationsAsync(dto2.OrganizationId);
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
        #endregion
    }
}
