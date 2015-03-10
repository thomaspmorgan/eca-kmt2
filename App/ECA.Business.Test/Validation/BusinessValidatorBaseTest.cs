using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation;
using System.Collections.Generic;
using ECA.Business.Exceptions;

namespace ECA.Business.Test.Validation
{
    public class CreateTest
    {

    }

    public class UpdateTest
    {

    }

    public class BusinessValidatorBaseTestClass : BusinessValidatorBase<CreateTest, UpdateTest>
    {
        public bool HasErrors { get; set; }

        public bool DoValidateCreateCalled { get; set; }

        public bool DoValidateUpdateCalled { get; set; }


        public BusinessValidatorBaseTestClass(bool throwException, bool hasErrors) : base(throwException) 
        { 
            this.HasErrors = hasErrors;
        }

        public override System.Collections.Generic.IEnumerable<BusinessValidationResult> DoValidateCreate(CreateTest validationEntity)
        {
            DoValidateCreateCalled = true;
            var errors = new List<BusinessValidationResult>();
            if (HasErrors)
            {
                errors.Add(new BusinessValidationResult("Error"));
            }
            return errors;

        }

        public override System.Collections.Generic.IEnumerable<BusinessValidationResult> DoValidateUpdate(UpdateTest validationEntity)
        {
            DoValidateUpdateCalled = true;
            var errors = new List<BusinessValidationResult>();
            if (HasErrors)
            {
                errors.Add(new BusinessValidationResult("Error"));
            }
            return errors;
        }
    }

    [TestClass]
    public class BusinessValidatorBaseTest
    {
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestValidateCreate_ShouldThrowException_HasErrors()
        {
            var testValidator = new BusinessValidatorBaseTestClass(true, true);
            testValidator.ValidateCreate(new CreateTest());
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestValidateUpdate_ShouldThrowException_HasErrors()
        {
            var testValidator = new BusinessValidatorBaseTestClass(true, true);
            testValidator.ValidateUpdate(new UpdateTest());
        }

        [TestMethod]
        public void TestValidateCreate_ShouldThrowException_NoErrors()
        {
            var testValidator = new BusinessValidatorBaseTestClass(true, false);
            var results = testValidator.ValidateCreate(new CreateTest());
            Assert.AreEqual(0, results.ToList().Count);
        }

        [TestMethod]
        public void TestValidateUpdate_ShouldThrowException_NoErrors()
        {
            var testValidator = new BusinessValidatorBaseTestClass(true, false);
            var results = testValidator.ValidateUpdate(new UpdateTest());
            Assert.AreEqual(0, results.ToList().Count);
        }

        [TestMethod]
        public void TestValidateCreate_ShouldNotThrowException_NoErrors()
        {
            var testValidator = new BusinessValidatorBaseTestClass(false, false);
            var results = testValidator.ValidateCreate(new CreateTest());
            Assert.AreEqual(0, results.ToList().Count);
        }

        [TestMethod]
        public void TestValidateUpdate_ShouldNotThrowException_NoErrors()
        {
            var testValidator = new BusinessValidatorBaseTestClass(false, false);
            var results = testValidator.ValidateUpdate(new UpdateTest());
            Assert.AreEqual(0, results.ToList().Count);
        }

        [TestMethod]
        public void TestValidateCreate_ShouldNotThrowException_HasErrors()
        {
            var testValidator = new BusinessValidatorBaseTestClass(false, true);
            var results = testValidator.ValidateCreate(new CreateTest());
            Assert.AreEqual(1, results.ToList().Count);
        }

        [TestMethod]
        public void TestValidateUpdate_ShouldNotThrowException_HasErrors()
        {
            var testValidator = new BusinessValidatorBaseTestClass(false, true);
            var results = testValidator.ValidateUpdate(new UpdateTest());
            Assert.AreEqual(1, results.ToList().Count);
        }
    }
}
