using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Azure.Search.Models;

namespace ECA.Business.Search.Test
{

    [TestClass]
    public class SimpleDocumentTest
    {
        //[TestMethod]
        //public void TestConstructor()
        //{
        //    var testDocument = new TestDocument();
        //    testDocument.Id = 1;
        //    testDocument.Description = "desc";
        //    testDocument.Name = "name";
        //    testDocument.Subtitle = "subtitle";

        //    var ecaDocument = new SimpleDocument(testDocument);
        //    Assert.AreEqual(testDocument.GetDocumentType(), ecaDocument.DocumentType);
        //    Assert.IsNotNull(ecaDocument.Key);
        //    CollectionAssert.AreEqual(testDocument.GetDocumentFields(), ecaDocument.AdditionalFields.ToList());

        //    foreach(var field in testDocument.GetDocumentFields())
        //    {
        //        Assert.IsTrue(ecaDocument.ContainsKey(field));
        //        Assert.AreEqual(testDocument.GetValue(field), ecaDocument[field]);
        //    }
        //}

        
    }

}
