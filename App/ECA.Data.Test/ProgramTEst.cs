using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ECA.Data.Test
{
    [TestClass]
    public class ProgramTest
    {
        private TestEcaContext context;


        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
        }

        [TestMethod]
        public void TestNameIsUnique()
        {
            var existingProgram = new Program
            {
                Name = "  HELLO  ",
                ProgramId = 1
            };
            context.Programs.Add(existingProgram);

            var testProgram = new Program
            {
                ProgramId = 2,
                Name = "  hello ",
                Description = "desc",
                Owner = new Organization()
            };
            var items = new Dictionary<object, object> { {EcaContext.VALIDATABLE_CONTEXT_KEY, context } };
            var vc = new ValidationContext(testProgram, null, items);
            var results = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(testProgram, vc, results);
            Assert.IsFalse(actual);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Name", results.First().MemberNames.First());
            Assert.AreEqual(String.Format("The program with the name [{0}] already exists.", testProgram.Name), results.First().ErrorMessage);
        }
    }
}
