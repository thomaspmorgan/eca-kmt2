﻿using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Filter;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Logging;
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
            service = new OfficeService(context, new TraceLogger());
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
            var focus = new Focus
            {
                FocusId = 6,
                FocusName = "focus"
            };

            office.OwnerPrograms.Add(program);
            office.Contacts.Add(contact);
            program.Focus = focus;
            program.Themes.Add(theme);
            program.Goals.Add(goal);

            context.Organizations.Add(office);
            context.Contacts.Add(contact);
            context.Programs.Add(program);
            context.Goals.Add(goal);
            context.Themes.Add(theme);
            context.Foci.Add(focus);

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

                Assert.AreEqual(1, dto.Foci.Count());
                Assert.AreEqual(focus.FocusId, dto.Foci.First().Id);
                Assert.AreEqual(focus.FocusName, dto.Foci.First().Value);
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
                Assert.AreEqual(0, dto.Foci.Count());
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
            var focus = new Focus
            {
                FocusId = 6,
                FocusName = "focus"
            };

            office.OwnerPrograms.Add(program);
            office.OwnerPrograms.Add(program2);
            office.Contacts.Add(contact);
            program.Themes.Add(theme);
            program2.Themes.Add(theme);

            program.Goals.Add(goal);
            program2.Goals.Add(goal);

            program.Focus = focus;
            program2.Focus = focus;


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
                Assert.AreEqual(1, dto.Foci.Count());
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
                OrganizationTypeId = OrganizationType.Individual.Id,
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
    }
}