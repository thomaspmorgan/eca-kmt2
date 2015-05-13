using ECA.Business.Queries.Models.Persons;
using ECA.Business.Service.Persons;
using ECA.Core.Service;
using ECA.Data;
using ECA.WebApi.Controllers.Persons;
using ECA.WebApi.Models.Person;
using ECA.WebApi.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace ECA.WebApi.Test.Controllers.Persons
{
    [TestClass]
    public class PersonControllerTest
    {
        private Mock<IPersonService> mock;
        private Mock<IUserProvider> userProvider;
        private PeopleController controller;
        
        [TestInitialize]
        public void TestInit()
        {
            mock = new Mock<IPersonService>();
            userProvider = new Mock<IUserProvider>();
            controller = new PeopleController(mock.Object, userProvider.Object);
        }

        #region Get Pii By Id
        [TestMethod]
        public async Task TestGetPiiById()
        {
            mock.Setup(x => x.GetPiiByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new PiiDTO());
            var response = await controller.GetPiiByIdAsync(1);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PiiDTO>));
        }

        [TestMethod]
        public async Task TestGetPiiById_InvalidModel()
        {
            mock.Setup(x => x.GetPiiByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(null);
            var response = await controller.GetPiiByIdAsync(1);
            Assert.IsInstanceOfType(response, typeof(NotFoundResult));
        }
        #endregion

        #region Get Contact Info By Id
        [TestMethod]
        public async Task TestGetContactInfoById()
        {
            mock.Setup(x => x.GetContactInfoByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new ContactInfoDTO());
            var response = await controller.GetContactInfoByIdAsync(1);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<ContactInfoDTO>));
        }

        [TestMethod]
        public async Task TestGetContactInfoById_InvalidModel()
        {
            mock.Setup(x => x.GetContactInfoByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(null);
            var response = await controller.GetContactInfoByIdAsync(1);
            Assert.IsInstanceOfType(response, typeof(NotFoundResult));
        }
        #endregion

        #region Post
        [TestMethod]
        public async Task TestPostPersonAsync()
        {
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new Business.Service.User(0));
            mock.Setup(x => x.CreateAsync(It.IsAny<NewPerson>()))
                .ReturnsAsync(new Person());
            mock.Setup(x => x.SaveChangesAsync(It.IsAny<List<ISaveAction>>())).ReturnsAsync(1);
            var response = await controller.PostPersonAsync(new PersonBindingModel());
            mock.Verify(x => x.SaveChangesAsync(It.IsAny<List<ISaveAction>>()), Times.Once());
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }

        [TestMethod]
        public async Task TestPostPersonAsync_Invalid()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.PostPersonAsync(new PersonBindingModel());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        #endregion
    }
}
