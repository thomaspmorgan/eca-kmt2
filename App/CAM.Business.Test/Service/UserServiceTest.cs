using System;
using FluentAssertions;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAM.Business.Service;
using System.Threading.Tasks;
using CAM.Data;
using CAM.Business.Model;

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
            context.AccountStatus.Add(accountStatus);
            Action<User> tester = (u) =>
            {
                Assert.AreEqual(accountStatus.Status, u.AccountStatusText);

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
            context.AccountStatus.Add(accountStatus);

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
            context.AccountStatus.Add(accountStatus);

            Assert.IsFalse(userAccount.LastAccessed.HasValue);
            Assert.AreEqual(0, context.SaveChangesCount);
            Assert.AreEqual(0, context.SaveChangesAsyncCount);
            var result = await service.GetUserByIdAsync(id);
            DateTimeOffset.Now.Should().BeCloseTo(userAccount.LastAccessed.Value, 2000);
            Assert.AreEqual(0, context.SaveChangesCount);
            Assert.AreEqual(1, context.SaveChangesAsyncCount);
        }

        #endregion
    }
}
