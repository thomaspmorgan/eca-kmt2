using ECA.Business.Queries.Models.Persons;
using ECA.Business.Service.Persons;
using ECA.WebApi.Controllers.Persons;
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
        private PeopleController controller;
        
        [TestInitialize]
        public void TestInit()
        {
            mock = new Mock<IPersonService>();
            controller = new PeopleController(mock.Object);
            ControllerHelper.InitializeController(controller);
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
    }
}
