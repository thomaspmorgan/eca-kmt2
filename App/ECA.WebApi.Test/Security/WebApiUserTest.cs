using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;
using ECA.WebApi.Security;
using System.Threading.Tasks;
using CAM.Business.Service;

namespace ECA.WebApi.Test.Security
{
    [TestClass]
    public class WebApiUserTest
    {

        [TestInitialize]
        public void TestInit()
        {

        }

        [TestMethod]
        public void TestConstructor()
        {
            var now = DateTime.UtcNow;
            var nowInSecondsAfterEpoch = (int)(now - WebApiUser.EPOCH).TotalSeconds;
            var yesterday = now.AddDays(-1.0);
            var yesterdayInSecondsAfterEpoch = (int)(yesterday - WebApiUser.EPOCH).TotalSeconds;
            var tomorrow = now.AddDays(1.0);
            var tomorrowInSecondsAfterEpoch = (int)(tomorrow - WebApiUser.EPOCH).TotalSeconds;
            var givenName = "given";
            var surName = "name";
            var fullName = "fullName";
            var email = "someone@isp.com";
            var id = Guid.NewGuid();

            var claims = new List<Claim>();
            
            claims.Add(new Claim(WebApiUser.EMAIL_KEY, email));
            claims.Add(new Claim(WebApiUser.EXPIRATION_DATE_KEY, tomorrowInSecondsAfterEpoch.ToString()));
            claims.Add(new Claim(WebApiUser.FULL_NAME_KEY, fullName));
            claims.Add(new Claim(WebApiUser.GIVEN_NAME_KEY, givenName));
            claims.Add(new Claim(WebApiUser.ISSUED_AT_TIME_KEY, nowInSecondsAfterEpoch.ToString()));
            claims.Add(new Claim(WebApiUser.SURNAME_KEY, surName));
            claims.Add(new Claim(WebApiUser.USER_ID_KEY, id.ToString()));
            claims.Add(new Claim(WebApiUser.VALID_NOT_BEFORE_DATE_KEY, yesterdayInSecondsAfterEpoch.ToString()));

            var claimsIdentity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            var user = new WebApiUser(claimsPrincipal);
            Assert.AreEqual(givenName, user.GivenName);
            Assert.AreEqual(surName, user.SurName);
            Assert.AreEqual(fullName, user.FullName);
            Assert.AreEqual(email, user.Email);
            Assert.AreEqual(id, user.Id);

            now.Should().BeCloseTo(user.TokenIssuedDate, 1000);
            yesterday.Should().BeCloseTo(user.TokenValidAfterDate, 1000);
            tomorrow.Should().BeCloseTo(user.TokenExpirationDate, 1000);
        }

        [TestMethod]
        public void TestSetUserId()
        {
            var id = Guid.NewGuid();
            var claims = new List<Claim>();
            claims.Add(new Claim(WebApiUser.USER_ID_KEY, id.ToString()));

            var claimsPrincipal = new ClaimsPrincipal();
            var user = new WebApiUser(claimsPrincipal);
            Assert.AreEqual(Guid.Empty, user.Id);
            user.SetUserId(claims);
            Assert.AreEqual(id, user.Id);
        }

        [TestMethod]
        public void TestSetUserId_InvalidGuid_ShouldNotThrow()
        {
            var id = "hello";
            var claims = new List<Claim>();
            claims.Add(new Claim(WebApiUser.USER_ID_KEY, id.ToString()));

            var claimsPrincipal = new ClaimsPrincipal();
            var user = new WebApiUser(claimsPrincipal);
            Assert.AreEqual(Guid.Empty, user.Id);
            user.Invoking(x => x.SetUserId(claims)).ShouldNotThrow();
        }

        [TestMethod]
        public void TestSetUserEmail()
        {
            var email = "someone@isp.com";
            var claims = new List<Claim>();
            claims.Add(new Claim(WebApiUser.EMAIL_KEY, email));

            var claimsPrincipal = new ClaimsPrincipal();
            var user = new WebApiUser(claimsPrincipal);
            user.SetUserEmail(claims);
            Assert.AreEqual(email, user.Email);
        }

        [TestMethod]
        public void TestSetGivenName()
        {
            var name = "joe";
            var claims = new List<Claim>();
            claims.Add(new Claim(WebApiUser.GIVEN_NAME_KEY, name));

            var claimsPrincipal = new ClaimsPrincipal();
            var user = new WebApiUser(claimsPrincipal);
            user.SetGivenName(claims);
            Assert.AreEqual(name, user.GivenName);
        }

        [TestMethod]
        public void TestSetSurname()
        {
            var name = "smith";
            var claims = new List<Claim>();
            claims.Add(new Claim(WebApiUser.SURNAME_KEY, name));

            var claimsPrincipal = new ClaimsPrincipal();
            var user = new WebApiUser(claimsPrincipal);
            user.SetSurname(claims);
            Assert.AreEqual(name, user.SurName);
        }

        [TestMethod]
        public void TestSetFullName()
        {
            var name = "joe smith";
            var claims = new List<Claim>();
            claims.Add(new Claim(WebApiUser.FULL_NAME_KEY, name));

            var claimsPrincipal = new ClaimsPrincipal();
            var user = new WebApiUser(claimsPrincipal);
            user.SetFullName(claims);
            Assert.AreEqual(name, user.FullName);
        }

        [TestMethod]
        public void TestSetTokenIssueDate()
        {
            var now = DateTime.UtcNow;
            var nowInSecondsAfterEpoch = (int)(now - WebApiUser.EPOCH).TotalSeconds;
            var claims = new List<Claim>();
            claims.Add(new Claim(WebApiUser.ISSUED_AT_TIME_KEY, nowInSecondsAfterEpoch.ToString()));

            var claimsPrincipal = new ClaimsPrincipal();
            var user = new WebApiUser(claimsPrincipal);
            user.SetTokenIssueDate(claims);
            now.Should().BeCloseTo(user.TokenIssuedDate, 1000);
        }

        [TestMethod]
        public void TestSetTokenIssueDate_InvalidTime()
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(WebApiUser.ISSUED_AT_TIME_KEY, "abc"));

            var claimsPrincipal = new ClaimsPrincipal();
            var user = new WebApiUser(claimsPrincipal);
            user.Invoking(x => x.SetTokenIssueDate(claims)).ShouldNotThrow();
        }

        [TestMethod]
        public void TestSetTokenExpirationDate()
        {
            var now = DateTime.UtcNow;
            var nowInSecondsAfterEpoch = (int)(now - WebApiUser.EPOCH).TotalSeconds;
            var claims = new List<Claim>();
            claims.Add(new Claim(WebApiUser.EXPIRATION_DATE_KEY, nowInSecondsAfterEpoch.ToString()));

            var claimsPrincipal = new ClaimsPrincipal();
            var user = new WebApiUser(claimsPrincipal);
            user.SetTokenExpirationDate(claims);
            now.Should().BeCloseTo(user.TokenExpirationDate, 1000);
        }

        [TestMethod]
        public void TestSetTokenExpirationDate_InvalidTime()
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(WebApiUser.EXPIRATION_DATE_KEY, "abc"));

            var claimsPrincipal = new ClaimsPrincipal();
            var user = new WebApiUser(claimsPrincipal);
            user.Invoking(x => x.SetTokenExpirationDate(claims)).ShouldNotThrow();
        }

        [TestMethod]
        public void TestSetTokenValidAfterDate()
        {
            var now = DateTime.UtcNow;
            var nowInSecondsAfterEpoch = (int)(now - WebApiUser.EPOCH).TotalSeconds;
            var claims = new List<Claim>();
            claims.Add(new Claim(WebApiUser.VALID_NOT_BEFORE_DATE_KEY, nowInSecondsAfterEpoch.ToString()));

            var claimsPrincipal = new ClaimsPrincipal();
            var user = new WebApiUser(claimsPrincipal);
            user.SetTokenValidAfterDate(claims);
            now.Should().BeCloseTo(user.TokenValidAfterDate, 1000);
        }

        [TestMethod]
        public void TestSetTokenValidAfterDate_InvalidTime()
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(WebApiUser.VALID_NOT_BEFORE_DATE_KEY, "abc"));

            var claimsPrincipal = new ClaimsPrincipal();
            var user = new WebApiUser(claimsPrincipal);
            user.Invoking(x => x.SetTokenValidAfterDate(claims)).ShouldNotThrow();
        }

        [TestMethod]
        public void TestGetUsername()
        {
            var email = "someone@isp.com";
            var claims = new List<Claim>();
            claims.Add(new Claim(WebApiUser.EMAIL_KEY, email));

            var claimsPrincipal = new ClaimsPrincipal();
            var user = new WebApiUser(claimsPrincipal);
            user.SetUserEmail(claims);
            Assert.AreEqual(email, user.GetUsername());
        }

        #region Setters with no claims
        [TestMethod]
        public void TestSetFullName_NoClaims_ShouldNotThrowAnyExceptions()
        {
            var claims = new List<Claim>();

            var claimsPrincipal = new ClaimsPrincipal();
            var user = new WebApiUser(claimsPrincipal);
            Assert.AreEqual(Guid.Empty, user.Id);

            user.Invoking(x => x.SetFullName(claims)).ShouldNotThrow();
        }

        [TestMethod]
        public void TestSetGivenName_NoClaims_ShouldNotThrowAnyExceptions()
        {
            var claims = new List<Claim>();

            var claimsPrincipal = new ClaimsPrincipal();
            var user = new WebApiUser(claimsPrincipal);
            Assert.AreEqual(Guid.Empty, user.Id);

            user.Invoking(x => x.SetGivenName(claims)).ShouldNotThrow();
        }

        [TestMethod]
        public void TestSetSurname_NoClaims_ShouldNotThrowAnyExceptions()
        {
            var claims = new List<Claim>();

            var claimsPrincipal = new ClaimsPrincipal();
            var user = new WebApiUser(claimsPrincipal);
            Assert.AreEqual(Guid.Empty, user.Id);

            user.Invoking(x => x.SetSurname(claims)).ShouldNotThrow();
        }

        [TestMethod]
        public void TestSetTokenExpirationDate_NoClaims_ShouldNotThrowAnyExceptions()
        {
            var claims = new List<Claim>();

            var claimsPrincipal = new ClaimsPrincipal();
            var user = new WebApiUser(claimsPrincipal);
            Assert.AreEqual(Guid.Empty, user.Id);

            user.Invoking(x => x.SetTokenExpirationDate(claims)).ShouldNotThrow();
        }

        [TestMethod]
        public void TestSetTokenIssueDate_NoClaims_ShouldNotThrowAnyExceptions()
        {
            var claims = new List<Claim>();

            var claimsPrincipal = new ClaimsPrincipal();
            var user = new WebApiUser(claimsPrincipal);
            Assert.AreEqual(Guid.Empty, user.Id);

            user.Invoking(x => x.SetTokenIssueDate(claims)).ShouldNotThrow();
        }

        [TestMethod]
        public void TestSetTokenValidAfterDate_NoClaims_ShouldNotThrowAnyExceptions()
        {
            var claims = new List<Claim>();

            var claimsPrincipal = new ClaimsPrincipal();
            var user = new WebApiUser(claimsPrincipal);
            Assert.AreEqual(Guid.Empty, user.Id);

            user.Invoking(x => x.SetTokenValidAfterDate(claims)).ShouldNotThrow();
        }

        [TestMethod]
        public void TestSetUserId_NoClaims_ShouldNotThrowAnyExceptions()
        {
            var claims = new List<Claim>();

            var claimsPrincipal = new ClaimsPrincipal();
            var user = new WebApiUser(claimsPrincipal);
            Assert.AreEqual(Guid.Empty, user.Id);

            user.Invoking(x => x.SetUserId(claims)).ShouldNotThrow();
        }

        #endregion
    }
}
