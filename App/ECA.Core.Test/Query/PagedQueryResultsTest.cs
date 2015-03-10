using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Core.Query;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECA.Core.Data;

namespace ECA.Core.Test.Query
{
    public class PagedQueryResultsTestClass
    {
        public int Id { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            var testClassInstance = obj as PagedQueryResultsTestClass;
            if (testClassInstance == null)
            {
                return false;
            }
            return this.Id == testClassInstance.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }

    [TestClass]
    public class PagedQueryResultsTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var total = 1;
            var list = new List<PagedQueryResultsTestClass>();
            list.Add(new PagedQueryResultsTestClass
            {
                Id = 1
            });

            var pagedQueryResults = new PagedQueryResults<PagedQueryResultsTestClass>(total, list);
            Assert.AreEqual(total, pagedQueryResults.Total);
            CollectionAssert.AreEqual(list, pagedQueryResults.Results);
        }

        [TestMethod]
        public void TestConstructor_NullList()
        {
            var pagedQueryResults = new PagedQueryResults<PagedQueryResultsTestClass>(1, null);
            Assert.IsNotNull(pagedQueryResults.Results);
        }

        #region Extensions
        [TestMethod]
        public async Task TestToPagedQueryResultsExtension()
        {
            var list = new TestDbSet<PagedQueryResultsTestClass>();
            for (var i = 0; i < 10; i++)
            {
                list.Add(new PagedQueryResultsTestClass
                {
                    Id = i
                });
            }

            var start = 0;
            var limit = 1;
            Action<PagedQueryResults<PagedQueryResultsTestClass>> tester = (results) =>
            {
                Assert.AreEqual(list.Count(), results.Total);
                Assert.AreEqual(limit, results.Results.Count);
            };

            var queryable = list.AsQueryable();
            var pagedResults = queryable.ToPagedQueryResults(start, limit);
            var pagedResultsAsync = await queryable.ToPagedQueryResultsAsync(start, limit);

        }

        #endregion
    }
}
