using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Persons;
using System.Reflection;
using System.Threading.Tasks;
using ECA.Data;
using ECA.Business.Queries.Models.Persons;
using ECA.Core.Query;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.DynamicLinq.Filter;

namespace ECA.Business.Test.Service.Persons
{
    [TestClass]
    public class ContactServiceTest
    {
        private TestEcaContext context;
        private ContactService service;

        [TestInitialize]
        public void TestInit()
        {
            context = DbContextHelper.GetInMemoryContext();
            service = new ContactService(context);
        }


        #region Get
        [TestMethod]
        public async Task TestGetThemes_CheckProperties()
        {
            var contact = new Contact
            {
                ContactId = 1,
                FullName = "full name",
                Position = "position"
            };
            context.Contacts.Add(contact);
            Action<PagedQueryResults<ContactDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(contact.ContactId, firstResult.Id);
                Assert.AreEqual(contact.FullName, firstResult.FullName);
                Assert.AreEqual(contact.Position, firstResult.Position);
            };
            var defaultSorter = new ExpressionSorter<ContactDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<ContactDTO>(0, 10, defaultSorter);

            var serviceResults = service.GetContacts(queryOperator);
            var serviceResultsAsync = await service.GetContactsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetThemes_DefaultSorter()
        {
            var contact1 = new Contact
            {
                ContactId = 1,
            };
            var contact2 = new Contact
            {
                ContactId = 2,
            };
            context.Contacts.Add(contact1);
            context.Contacts.Add(contact2);
            Action<PagedQueryResults<ContactDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(contact2.ContactId, firstResult.Id);
            };
            var defaultSorter = new ExpressionSorter<ContactDTO>(x => x.Id, SortDirection.Descending);
            var queryOperator = new QueryableOperator<ContactDTO>(0, 1, defaultSorter);

            var serviceResults = service.GetContacts(queryOperator);
            var serviceResultsAsync = await service.GetContactsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetThemes_Filter()
        {
            var contact1 = new Contact
            {
                ContactId = 1,
            };
            var contact2 = new Contact
            {
                ContactId = 2,
            };
            context.Contacts.Add(contact1);
            context.Contacts.Add(contact2);
            Action<PagedQueryResults<ContactDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(contact1.ContactId, firstResult.Id);
            };
            var defaultSorter = new ExpressionSorter<ContactDTO>(x => x.Id, SortDirection.Descending);
            var queryOperator = new QueryableOperator<ContactDTO>(0, 1, defaultSorter);
            queryOperator.Filters.Add(new ExpressionFilter<ContactDTO>(x => x.Id, ComparisonType.Equal, contact1.ContactId));

            var serviceResults = service.GetContacts(queryOperator);
            var serviceResultsAsync = await service.GetContactsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetThemes_Sort()
        {
            var contact1 = new Contact
            {
                ContactId = 1,
            };
            var contact2 = new Contact
            {
                ContactId = 2,
            };
            context.Contacts.Add(contact2);
            context.Contacts.Add(contact1);

            Action<PagedQueryResults<ContactDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(contact2.ContactId, firstResult.Id);
            };
            var defaultSorter = new ExpressionSorter<ContactDTO>(x => x.Id, SortDirection.Descending);
            var queryOperator = new QueryableOperator<ContactDTO>(0, 1, defaultSorter);
            queryOperator.Sorters.Add(new ExpressionSorter<ContactDTO>(x => x.Id, SortDirection.Descending));

            var serviceResults = service.GetContacts(queryOperator);
            var serviceResultsAsync = await service.GetContactsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetThemes_Paging()
        {
            var contact1 = new Contact
            {
                ContactId = 1,
            };
            var contact2 = new Contact
            {
                ContactId = 2,
            };
            context.Contacts.Add(contact2);
            context.Contacts.Add(contact1);

            Action<PagedQueryResults<ContactDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(1, results.Results.Count);
            };
            var defaultSorter = new ExpressionSorter<ContactDTO>(x => x.Id, SortDirection.Descending);
            var queryOperator = new QueryableOperator<ContactDTO>(0, 1, defaultSorter);

            var serviceResults = service.GetContacts(queryOperator);
            var serviceResultsAsync = await service.GetContactsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        #endregion
    }
}
