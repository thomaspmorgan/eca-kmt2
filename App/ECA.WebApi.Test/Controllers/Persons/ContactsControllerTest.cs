using ECA.Business.Queries.Models.Persons;
using ECA.Business.Service.Persons;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.WebApi.Controllers.Persons;
using ECA.WebApi.Models.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace ECA.WebApi.Test.Controllers.Persons
{
    [TestClass]
    public class ContactsControllerTest
    {
        private Mock<IContactService> serviceMock;
        private ContactsController controller;

        [TestInitialize]
        public void TestInit()
        {
            serviceMock = new Mock<IContactService>();
            serviceMock.Setup(x => x.GetContactsAsync(It.IsAny<QueryableOperator<ContactDTO>>()))
                .ReturnsAsync(new PagedQueryResults<ContactDTO>(1, new List<ContactDTO>()));

            controller = new ContactsController(serviceMock.Object);
        }

        #region Get
        [TestMethod]
        public async Task TestGetThemesAsync()
        {
            var response = await controller.GetContactsAsync(new PagingQueryBindingModel<ContactDTO>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<ContactDTO>>));
        }

        [TestMethod]
        public async Task TestGetThemesAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetContactsAsync(new PagingQueryBindingModel<ContactDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion
    }
}
