using ECA.Business.Queries.Models.Persons;
using ECA.Business.Service;
using ECA.Business.Service.Persons;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Data;
using ECA.WebApi.Controllers.Persons;
using ECA.WebApi.Models.Admin;
using ECA.WebApi.Models.Person;
using ECA.WebApi.Models.Query;
using ECA.WebApi.Security;
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
        private Mock<IUserProvider> userProvider;
        private Mock<IEmailAddressHandler> emailAddressHandler;
        private Mock<IPhoneNumberHandler> phoneNumberHandler;
        private ContactsController controller;

        [TestInitialize]
        public void TestInit()
        {
            userProvider = new Mock<IUserProvider>();
            serviceMock = new Mock<IContactService>();
            serviceMock.Setup(x => x.GetContactsAsync(It.IsAny<QueryableOperator<ContactDTO>>()))
                .ReturnsAsync(new PagedQueryResults<ContactDTO>(1, new List<ContactDTO>()));
            emailAddressHandler = new Mock<IEmailAddressHandler>();
            phoneNumberHandler = new Mock<IPhoneNumberHandler>();

            controller = new ContactsController(serviceMock.Object, userProvider.Object, phoneNumberHandler.Object, emailAddressHandler.Object);
        }

        #region Create
        [TestMethod]
        public async Task TestPostCreateAsync()
        {
            userProvider.Setup(x => x.GetCurrentUser()).Returns(new DebugWebApiUser());
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new User(1));
            serviceMock.Setup(x => x.GetContactByIdAsync(It.IsAny<int>())).ReturnsAsync(new ContactDTO());
            serviceMock.Setup(x => x.CreateAsync(It.IsAny<AdditionalPointOfContact>())).ReturnsAsync(new Contact());

            var model = new AdditionalPointOfContactBindingModel();
            model.FullName = "name";
            model.Position = "position";

            var response = await controller.PostCreateContactAsync(model);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<Contact>));
            userProvider.Verify(x => x.GetCurrentUser(), Times.Once());
            userProvider.Verify(x => x.GetBusinessUser(It.IsAny<IWebApiUser>()), Times.Once());
            serviceMock.Verify(x => x.CreateAsync(It.IsAny<AdditionalPointOfContact>()), Times.Once());
            serviceMock.Verify(x => x.SaveChangesAsync(), Times.Once());
            serviceMock.Verify(x => x.GetContactByIdAsync(It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public async Task TestPostCreateAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var model = new AdditionalPointOfContactBindingModel();
            model.FullName = "name";
            model.Position = "position";

            var response = await controller.PostCreateContactAsync(model);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));

        }
        #endregion

        #region Get

        [TestMethod]
        public async Task TestGetContactsAsync()
        {
            var response = await controller.GetContactsAsync(new PagingQueryBindingModel<ContactDTO>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<ContactDTO>>));
        }

        [TestMethod]
        public async Task TestGetContactsAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetContactsAsync(new PagingQueryBindingModel<ContactDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        #endregion
        
    }
}
