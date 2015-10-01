using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Persons;
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
    public class EvaluationServiceTest
    {
        private TestEcaContext context;
        private EvaluationNoteService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new EvaluationNoteService(context);
        }

        [TestMethod]
        public async Task TestGetEvaluations_CheckProperties()
        {
            var evaluation = new PersonEvaluationNote
            {
                EvaluationNoteId = 1,
                EvaluationNote = "Test evaluation note"
            };

            context.PersonEvaluationNotes.Add(evaluation);

            Action<PagedQueryResults<EvaluationNoteDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(evaluation.EvaluationNoteId, firstResult.EvaluationNoteId);
                Assert.AreEqual(evaluation.EvaluationNote, firstResult.EvaluationNote);
            };
            var defaultSorter = new ExpressionSorter<EvaluationNoteDTO>(x => x.EvaluationNoteId, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<EvaluationNoteDTO>(0, 10, defaultSorter);

            var serviceResults = service.Get(queryOperator);
            var serviceResultsAsync = await service.GetAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetEvaluations_DefaultSorter()
        {
            var evaluation1 = new PersonEvaluationNote
            {
                EvaluationNoteId = 1
            };
            var evaluation2 = new PersonEvaluationNote
            {
                EvaluationNoteId = 2
            };
            context.PersonEvaluationNotes.Add(evaluation1);
            context.PersonEvaluationNotes.Add(evaluation2);

            Action<PagedQueryResults<EvaluationNoteDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(evaluation2.EvaluationNoteId, firstResult.EvaluationNoteId);
            };
            var defaultSorter = new ExpressionSorter<EvaluationNoteDTO>(x => x.EvaluationNoteId, SortDirection.Descending);
            var queryOperator = new QueryableOperator<EvaluationNoteDTO>(0, 1, defaultSorter);

            var serviceResults = service.Get(queryOperator);
            var serviceResultsAsync = await service.GetAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetEvaluations_Filter()
        {
            var evaluation1 = new PersonEvaluationNote
            {
                EvaluationNoteId = 1
            };
            var evaluation2 = new PersonEvaluationNote
            {
                EvaluationNoteId = 2
            };
            context.PersonEvaluationNotes.Add(evaluation1);
            context.PersonEvaluationNotes.Add(evaluation2);

            Action<PagedQueryResults<EvaluationNoteDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(evaluation1.EvaluationNoteId, firstResult.EvaluationNoteId);
            };
            var defaultSorter = new ExpressionSorter<EvaluationNoteDTO>(x => x.EvaluationNoteId, SortDirection.Descending);
            var queryOperator = new QueryableOperator<EvaluationNoteDTO>(0, 1, defaultSorter);
            queryOperator.Filters.Add(new ExpressionFilter<EvaluationNoteDTO>(x => x.EvaluationNoteId, ComparisonType.Equal, evaluation1.EvaluationNoteId));

            var serviceResults = service.Get(queryOperator);
            var serviceResultsAsync = await service.GetAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetEvaluations_Sort()
        {
            var evaluation1 = new PersonEvaluationNote
            {
                EvaluationNoteId = 1
            };
            var evaluation2 = new PersonEvaluationNote
            {
                EvaluationNoteId = 2
            };
            context.PersonEvaluationNotes.Add(evaluation1);
            context.PersonEvaluationNotes.Add(evaluation2);

            Action<PagedQueryResults<EvaluationNoteDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(evaluation2.EvaluationNoteId, firstResult.EvaluationNoteId);
            };
            var defaultSorter = new ExpressionSorter<EvaluationNoteDTO>(x => x.EvaluationNoteId, SortDirection.Descending);
            var queryOperator = new QueryableOperator<EvaluationNoteDTO>(0, 1, defaultSorter);
            queryOperator.Sorters.Add(new ExpressionSorter<EvaluationNoteDTO>(x => x.EvaluationNoteId, SortDirection.Descending));

            var serviceResults = service.Get(queryOperator);
            var serviceResultsAsync = await service.GetAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetEvaluations_Paging()
        {
            var evaluation1 = new PersonEvaluationNote
            {
                EvaluationNoteId = 1
            };
            var evaluation2 = new PersonEvaluationNote
            {
                EvaluationNoteId = 2
            };
            context.PersonEvaluationNotes.Add(evaluation1);
            context.PersonEvaluationNotes.Add(evaluation2);

            Action<PagedQueryResults<EvaluationNoteDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(evaluation2.EvaluationNoteId, firstResult.EvaluationNoteId);
            };
            var defaultSorter = new ExpressionSorter<EvaluationNoteDTO>(x => x.EvaluationNoteId, SortDirection.Descending);
            var queryOperator = new QueryableOperator<EvaluationNoteDTO>(0, 1, defaultSorter);

            var serviceResults = service.Get(queryOperator);
            var serviceResultsAsync = await service.GetAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        
    }
}
