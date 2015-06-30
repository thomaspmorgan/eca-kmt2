using System;
using FluentAssertions;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAM.Business.Service;
using System.Threading.Tasks;
using CAM.Data;
using CAM.Business.Model;
using ECA.Core.Query;
using CAM.Business.Queries.Models;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.DynamicLinq.Filter;

namespace CAM.Business.Test.Service
{
    [TestClass]
    public class UserServiceTest
    {
        private TestInMemoryCamModel context;
        private UserService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestInMemoryCamModel();
            service = new UserService(context);
        }

        #region Create
        [TestMethod]
        public void TestCreate()
        {
            var firstName = "first";
            var lastName = "lastName";
            var email = "email";
            var id = Guid.NewGuid();
            var displayName = "display";
            var newUser = new AzureUser(
                id: id,
                email: email,
                firstName: firstName,
                lastName: lastName,
                displayName: displayName
                );

            Assert.AreEqual(0, context.UserAccounts.Count());
            Assert.AreEqual(0, context.Principals.Count());

            var userAccount = service.Create(newUser);
            Assert.AreEqual(1, context.UserAccounts.Count());
            Assert.AreEqual(1, context.Principals.Count());

            var savedUserAccount = context.UserAccounts.First();
            var savedPrincipal = context.Principals.First();
            Assert.AreEqual(savedUserAccount, savedPrincipal.UserAccount);
            Assert.AreEqual(savedPrincipal, savedUserAccount.Principal);
            Assert.IsTrue(Object.ReferenceEquals(userAccount, savedUserAccount));
            Assert.AreEqual(PrincipalType.Person.Id, savedPrincipal.PrincipalTypeId);

            Assert.AreEqual(firstName, savedUserAccount.FirstName);
            Assert.AreEqual(lastName, savedUserAccount.LastName);
            Assert.AreEqual(email, savedUserAccount.EmailAddress);
            Assert.AreEqual(id, savedUserAccount.AdGuid);
            Assert.AreEqual(displayName, savedUserAccount.DisplayName);
            Assert.AreEqual(AccountStatus.Active.Id, savedUserAccount.AccountStatusId);
            Assert.AreEqual(UserAccount.SYSTEM_USER_ACCOUNT_ID, savedUserAccount.CreatedBy);
            Assert.AreEqual(UserAccount.SYSTEM_USER_ACCOUNT_ID, savedUserAccount.RevisedBy);

            Assert.IsTrue(savedUserAccount.LastAccessed.HasValue);
            DateTimeOffset.Now.Should().BeCloseTo(savedUserAccount.CreatedOn, 2000);
            DateTimeOffset.Now.Should().BeCloseTo(savedUserAccount.RevisedOn, 2000);
            DateTimeOffset.Now.Should().BeCloseTo(savedUserAccount.LastAccessed.Value, 2000);
            Assert.IsNull(savedUserAccount.Note);

            Assert.IsFalse(savedUserAccount.ExpiredDate.HasValue);
            Assert.IsFalse(savedUserAccount.PermissionsRevisedOn.HasValue);
            Assert.IsFalse(savedUserAccount.RestoredDate.HasValue);
            Assert.IsFalse(savedUserAccount.RevokedDate.HasValue);
            Assert.IsFalse(savedUserAccount.SuspendedDate.HasValue);

        }
        #endregion

        #region Test IsValid

        [TestMethod]
        public async Task TestIsUserValid()
        {
            var id = Guid.NewGuid();
            var userAccount = new UserAccount();
            userAccount.AdGuid = id;

            context.UserAccounts.Add(userAccount);
            Action<bool> tester = (b) =>
            {
                Assert.IsTrue(b);
            };

            var result = service.IsUserValid(id);
            var resultAsync = await service.IsUserValidAsync(id);
            tester(result);
            tester(resultAsync);
        }

        [TestMethod]
        public async Task TestIsUserValid_UserDoesNotExist()
        {
            var id = Guid.NewGuid();

            Action<bool> tester = (b) =>
            {
                Assert.IsFalse(b);
            };

            var result = service.IsUserValid(id);
            var resultAsync = await service.IsUserValidAsync(id);
            tester(result);
            tester(resultAsync);
        }

        [TestMethod]
        public void TestIsUserValid_CheckLastAccessedIsUpdated()
        {
            var id = Guid.NewGuid();
            var userAccount = new UserAccount();
            userAccount.AdGuid = id;

            context.UserAccounts.Add(userAccount);
            Assert.AreEqual(0, context.SaveChangesCount);
            Assert.AreEqual(0, context.SaveChangesAsyncCount);

            Assert.IsFalse(userAccount.LastAccessed.HasValue);
            var result = service.IsUserValid(id);
            Assert.AreEqual(1, context.SaveChangesCount);
            Assert.AreEqual(0, context.SaveChangesAsyncCount);
            Assert.IsTrue(userAccount.LastAccessed.HasValue);
            DateTimeOffset.Now.Should().BeCloseTo(userAccount.LastAccessed.Value, 2000);
        }

        [TestMethod]
        public async Task TestIsUserValidAsync_CheckLastAccessedIsUpdated()
        {
            var id = Guid.NewGuid();
            var userAccount = new UserAccount();
            userAccount.AdGuid = id;

            context.UserAccounts.Add(userAccount);
            Assert.AreEqual(0, context.SaveChangesCount);
            Assert.AreEqual(0, context.SaveChangesAsyncCount);

            Assert.IsFalse(userAccount.LastAccessed.HasValue);
            var result = await service.IsUserValidAsync(id);
            Assert.AreEqual(0, context.SaveChangesCount);
            Assert.AreEqual(1, context.SaveChangesAsyncCount);
            Assert.IsTrue(userAccount.LastAccessed.HasValue);
            DateTimeOffset.Now.Should().BeCloseTo(userAccount.LastAccessed.Value, 2000);
        }

        #endregion

        #region Get
        [TestMethod]
        public async Task TestGetUserById_CheckProperties()
        {
            var accountStatus = new AccountStatus
            {
                AccountStatusId = 1,
                Status = "hello"
            };
            var createdDate = DateTimeOffset.Now.AddDays(-1.0);
            var revisedOnDate = DateTimeOffset.Now.AddDays(9.0);

            var expirationDate = DateTimeOffset.Now.AddDays(2.0);
            var revokedDate = DateTimeOffset.Now.AddDays(3.0);
            var restoredOnDate = DateTimeOffset.Now.AddDays(9.0);
            var suspendedDate = DateTimeOffset.Now.AddDays(-4.0);
            var permissionsRevisedDate = DateTimeOffset.Now.AddDays(5.0);
            var today = DateTimeOffset.Now;


            var id = Guid.NewGuid();
            var userAccount = new UserAccount();
            userAccount.AdGuid = id;
            userAccount.AccountStatusId = accountStatus.AccountStatusId;
            userAccount.CreatedBy = 1;
            userAccount.CreatedOn = createdDate;
            userAccount.DisplayName = "test user";
            userAccount.EmailAddress = "someone@isp.com";
            userAccount.ExpiredDate = expirationDate;
            userAccount.FirstName = "first";
            userAccount.LastAccessed = null; //leave this null specifically - should be updated by the call
            userAccount.LastName = "last name";
            userAccount.Note = "note";
            userAccount.PrincipalId = 2;
            userAccount.PermissionsRevisedOn = permissionsRevisedDate;
            userAccount.RestoredDate = restoredOnDate;
            userAccount.RevisedBy = 2;
            userAccount.RevisedOn = revisedOnDate;
            userAccount.RevokedDate = revokedDate;

            userAccount.AccountStatus = accountStatus;

            context.UserAccounts.Add(userAccount);
            context.AccountStatuses.Add(accountStatus);
            Action<User> tester = (u) =>
            {
                Assert.AreEqual(accountStatus.Status, u.AccountStatus);

                Assert.AreEqual(userAccount.AccountStatusId, u.AccountStatusId);
                Assert.AreEqual(userAccount.AdGuid, u.AdGuid);
                Assert.AreEqual(userAccount.DisplayName, u.DisplayName);
                Assert.AreEqual(userAccount.EmailAddress, u.EmailAddress);
                Assert.AreEqual(userAccount.ExpiredDate, u.ExpiredDate);
                Assert.AreEqual(userAccount.FirstName, u.FirstName);
                Assert.AreEqual(userAccount.LastName, u.LastName);
                Assert.AreEqual(userAccount.PrincipalId, u.PrincipalId);
                Assert.AreEqual(userAccount.RestoredDate, u.RestoredDate);
                Assert.AreEqual(userAccount.RevokedDate, u.RevokedDate);
                Assert.AreEqual(userAccount.SuspendedDate, u.SuspendedDate);
            };
            var result = service.GetUserById(id);
            var resultAsync = await service.GetUserByIdAsync(id);
            tester(result);
            tester(resultAsync);
        }

        [TestMethod]
        public async Task TestGetUserByPrincipalId_CheckProperties()
        {
            var accountStatus = new AccountStatus
            {
                AccountStatusId = 1,
                Status = "hello"
            };
            var createdDate = DateTimeOffset.Now.AddDays(-1.0);
            var revisedOnDate = DateTimeOffset.Now.AddDays(9.0);

            var expirationDate = DateTimeOffset.Now.AddDays(2.0);
            var revokedDate = DateTimeOffset.Now.AddDays(3.0);
            var restoredOnDate = DateTimeOffset.Now.AddDays(9.0);
            var suspendedDate = DateTimeOffset.Now.AddDays(-4.0);
            var permissionsRevisedDate = DateTimeOffset.Now.AddDays(5.0);
            var today = DateTimeOffset.Now;


            var id = Guid.NewGuid();
            var userAccount = new UserAccount();
            userAccount.AdGuid = id;
            userAccount.AccountStatusId = accountStatus.AccountStatusId;
            userAccount.CreatedBy = 1;
            userAccount.CreatedOn = createdDate;
            userAccount.DisplayName = "test user";
            userAccount.EmailAddress = "someone@isp.com";
            userAccount.ExpiredDate = expirationDate;
            userAccount.FirstName = "first";
            userAccount.LastAccessed = null; //leave this null specifically - should be updated by the call
            userAccount.LastName = "last name";
            userAccount.Note = "note";
            userAccount.PrincipalId = 2;
            userAccount.PermissionsRevisedOn = permissionsRevisedDate;
            userAccount.RestoredDate = restoredOnDate;
            userAccount.RevisedBy = 2;
            userAccount.RevisedOn = revisedOnDate;
            userAccount.RevokedDate = revokedDate;

            userAccount.AccountStatus = accountStatus;

            context.UserAccounts.Add(userAccount);
            context.AccountStatuses.Add(accountStatus);
            Action<User> tester = (u) =>
            {
                Assert.AreEqual(accountStatus.Status, u.AccountStatus);

                Assert.AreEqual(userAccount.AccountStatusId, u.AccountStatusId);
                Assert.AreEqual(userAccount.AdGuid, u.AdGuid);
                Assert.AreEqual(userAccount.DisplayName, u.DisplayName);
                Assert.AreEqual(userAccount.EmailAddress, u.EmailAddress);
                Assert.AreEqual(userAccount.ExpiredDate, u.ExpiredDate);
                Assert.AreEqual(userAccount.FirstName, u.FirstName);
                Assert.AreEqual(userAccount.LastName, u.LastName);
                Assert.AreEqual(userAccount.PrincipalId, u.PrincipalId);
                Assert.AreEqual(userAccount.RestoredDate, u.RestoredDate);
                Assert.AreEqual(userAccount.RevokedDate, u.RevokedDate);
                Assert.AreEqual(userAccount.SuspendedDate, u.SuspendedDate);
            };
            var result = service.GetUserById(userAccount.PrincipalId);
            var resultAsync = await service.GetUserByIdAsync(userAccount.PrincipalId);
            tester(result);
            tester(resultAsync);
        }

        [TestMethod]
        public async Task TestGetUserById_UserDoesNotExist()
        {
            var id = Guid.NewGuid();
            Action<User> tester = (u) =>
            {
                Assert.IsNull(u);
            };
            var result = service.GetUserById(id);
            var resultAsync = await service.GetUserByIdAsync(id);
            tester(result);
            tester(resultAsync);
        }

        [TestMethod]
        public async Task TestGetUserByPrincipalId_UserDoesNotExist()
        {
            var id = Guid.NewGuid();
            Action<User> tester = (u) =>
            {
                Assert.IsNull(u);
            };
            var result = service.GetUserById(0);
            var resultAsync = await service.GetUserByIdAsync(0);
            tester(result);
            tester(resultAsync);
        }

        [TestMethod]
        public void TestGetUserById_CheckLastAccessedUpdated()
        {
            var accountStatus = new AccountStatus
            {
                AccountStatusId = 1,
                Status = "hello"
            };
            var id = Guid.NewGuid();
            var userAccount = new UserAccount();
            userAccount.AdGuid = id;

            userAccount.AccountStatus = accountStatus;

            context.UserAccounts.Add(userAccount);
            context.AccountStatuses.Add(accountStatus);

            Assert.IsFalse(userAccount.LastAccessed.HasValue);
            Assert.AreEqual(0, context.SaveChangesCount);
            Assert.AreEqual(0, context.SaveChangesAsyncCount);
            var result = service.GetUserById(id);
            DateTimeOffset.Now.Should().BeCloseTo(userAccount.LastAccessed.Value, 2000);
            Assert.AreEqual(1, context.SaveChangesCount);
            Assert.AreEqual(0, context.SaveChangesAsyncCount);
        }

        [TestMethod]
        public async Task TestGetUserByIdAsync_CheckLastAccessedUpdated()
        {
            var accountStatus = new AccountStatus
            {
                AccountStatusId = 1,
                Status = "hello"
            };
            var id = Guid.NewGuid();
            var userAccount = new UserAccount();
            userAccount.AdGuid = id;

            userAccount.AccountStatus = accountStatus;

            context.UserAccounts.Add(userAccount);
            context.AccountStatuses.Add(accountStatus);

            Assert.IsFalse(userAccount.LastAccessed.HasValue);
            Assert.AreEqual(0, context.SaveChangesCount);
            Assert.AreEqual(0, context.SaveChangesAsyncCount);
            var result = await service.GetUserByIdAsync(id);
            DateTimeOffset.Now.Should().BeCloseTo(userAccount.LastAccessed.Value, 2000);
            Assert.AreEqual(0, context.SaveChangesCount);
            Assert.AreEqual(1, context.SaveChangesAsyncCount);
        }

        [TestMethod]
        public async Task TestGetUsers_CheckProperties()
        {
            var user1 = new UserAccount
            {
                AdGuid = Guid.NewGuid(),
                DisplayName = "display1",
                EmailAddress = "email1",
                FirstName = "first1",
                LastName = "last1",
                PrincipalId = 1,
            };
            context.UserAccounts.Add(user1);

            Action<PagedQueryResults<UserDTO>> tester = (testResults) =>
            {
                Assert.AreEqual(1, testResults.Total);
                Assert.AreEqual(1, testResults.Results.Count);
                var firstResult = testResults.Results.First();
                Assert.AreEqual(user1.AdGuid, firstResult.AdGuid);
                Assert.AreEqual(user1.DisplayName, firstResult.DisplayName);
                Assert.AreEqual(user1.EmailAddress, firstResult.Email);
                Assert.AreEqual(user1.FirstName, firstResult.FirstName);
                Assert.AreEqual(user1.PrincipalId, firstResult.PrincipalId);
            };
            var defaultSorter = new ExpressionSorter<UserDTO>(x => x.AdGuid, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<UserDTO>(0, 10, defaultSorter);
            var results = service.GetUsers(queryOperator);
            var resultsAsync = await service.GetUsersAsync(queryOperator);
            tester(results);
            tester(resultsAsync);
        }

        [TestMethod]
        public async Task TestGetUsers_DefaultSort()
        {
            var user1 = new UserAccount
            {
                AdGuid = Guid.NewGuid(),
                DisplayName = "display1",
                EmailAddress = "email1",
                FirstName = "first1",
                LastName = "last1",
                PrincipalId = 1,
            };
            var user2 = new UserAccount
            {
                AdGuid = Guid.NewGuid(),
                DisplayName = "display2",
                EmailAddress = "email2",
                FirstName = "first2",
                LastName = "last2",
                PrincipalId = 2,
            };
            context.UserAccounts.Add(user1);
            context.UserAccounts.Add(user2);

            Action<PagedQueryResults<UserDTO>> tester = (testResults) =>
            {
                Assert.AreEqual(2, testResults.Total);
                Assert.AreEqual(2, testResults.Results.Count);
                Assert.AreEqual(user1.AdGuid, testResults.Results.First().AdGuid);
            };
            var defaultSorter = new ExpressionSorter<UserDTO>(x => x.PrincipalId, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<UserDTO>(0, 10, defaultSorter);
            var results = service.GetUsers(queryOperator);
            var resultsAsync = await service.GetUsersAsync(queryOperator);
            tester(results);
            tester(resultsAsync);
        }

        [TestMethod]
        public async Task TestGetUsers_Sort()
        {
            var user1 = new UserAccount
            {
                AdGuid = Guid.NewGuid(),
                DisplayName = "display1",
                EmailAddress = "email1",
                FirstName = "first1",
                LastName = "last1",
                PrincipalId = 1,
            };
            var user2 = new UserAccount
            {
                AdGuid = Guid.NewGuid(),
                DisplayName = "display2",
                EmailAddress = "email2",
                FirstName = "first2",
                LastName = "last2",
                PrincipalId = 2,
            };
            context.UserAccounts.Add(user1);
            context.UserAccounts.Add(user2);

            Action<PagedQueryResults<UserDTO>> tester = (testResults) =>
            {
                Assert.AreEqual(2, testResults.Total);
                Assert.AreEqual(2, testResults.Results.Count);
                Assert.AreEqual(user2.AdGuid, testResults.Results.First().AdGuid);
            };
            var defaultSorter = new ExpressionSorter<UserDTO>(x => x.PrincipalId, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<UserDTO>(0, 10, defaultSorter);
            queryOperator.Sorters.Add(new ExpressionSorter<UserDTO>(x => x.Email, SortDirection.Descending));
            var results = service.GetUsers(queryOperator);
            var resultsAsync = await service.GetUsersAsync(queryOperator);
            tester(results);
            tester(resultsAsync);
        }

        [TestMethod]
        public async Task TestGetUsers_Filter()
        {
            var user1 = new UserAccount
            {
                AdGuid = Guid.NewGuid(),
                DisplayName = "display1",
                EmailAddress = "email1",
                FirstName = "first1",
                LastName = "last1",
                PrincipalId = 1,
            };
            var user2 = new UserAccount
            {
                AdGuid = Guid.NewGuid(),
                DisplayName = "display2",
                EmailAddress = "email2",
                FirstName = "first2",
                LastName = "last2",
                PrincipalId = 2,
            };
            context.UserAccounts.Add(user1);
            context.UserAccounts.Add(user2);

            Action<PagedQueryResults<UserDTO>> tester = (testResults) =>
            {
                Assert.AreEqual(1, testResults.Total);
                Assert.AreEqual(1, testResults.Results.Count);
                Assert.AreEqual(user1.AdGuid, testResults.Results.First().AdGuid);
            };
            var defaultSorter = new ExpressionSorter<UserDTO>(x => x.PrincipalId, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<UserDTO>(0, 10, defaultSorter);
            queryOperator.Filters.Add(new ExpressionFilter<UserDTO>(x => x.Email, ComparisonType.Equal, user1.EmailAddress));
            var results = service.GetUsers(queryOperator);
            var resultsAsync = await service.GetUsersAsync(queryOperator);
            tester(results);
            tester(resultsAsync);
        }

        [TestMethod]
        public async Task TestGetUsers_Paged()
        {
            var user1 = new UserAccount
            {
                AdGuid = Guid.NewGuid(),
                DisplayName = "display1",
                EmailAddress = "email1",
                FirstName = "first1",
                LastName = "last1",
                PrincipalId = 1,
            };
            var user2 = new UserAccount
            {
                AdGuid = Guid.NewGuid(),
                DisplayName = "display2",
                EmailAddress = "email2",
                FirstName = "first2",
                LastName = "last2",
                PrincipalId = 2,
            };
            context.UserAccounts.Add(user1);
            context.UserAccounts.Add(user2);

            Action<PagedQueryResults<UserDTO>> tester = (testResults) =>
            {
                Assert.AreEqual(2, testResults.Total);
                Assert.AreEqual(1, testResults.Results.Count);
                Assert.AreEqual(user1.AdGuid, testResults.Results.First().AdGuid);
            };
            var defaultSorter = new ExpressionSorter<UserDTO>(x => x.PrincipalId, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<UserDTO>(0, 1, defaultSorter);
            var results = service.GetUsers(queryOperator);
            var resultsAsync = await service.GetUsersAsync(queryOperator);
            tester(results);
            tester(resultsAsync);
        }

        #endregion
    }
}
