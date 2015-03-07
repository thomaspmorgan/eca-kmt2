using System;
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
        public bool DoValidateCreateCalled { get; set; }

        public bool DoValidateUpdateCalled { get; set; }

        public bool ShouldThrowException { get; set; }

        public BusinessValidatorBaseTestClass(bool throwException) : base(throwException) { this.ShouldThrowException = throwException; }

        public override System.Collections.Generic.IEnumerable<BusinessValidationResult> DoValidateCreate(CreateTest validationEntity)
        {
            DoValidateCreateCalled = true;
            if (ShouldThrowException)
            {
                return new List<BusinessValidationResult>{new BusinessValidationResult("Error")};
            }
            else
            {
                return new List<BusinessValidationResult>();
            }
            
        }

        public override System.Collections.Generic.IEnumerable<BusinessValidationResult> DoValidateUpdate(UpdateTest validationEntity)
        {
            DoValidateUpdateCalled = true;
            if (ShouldThrowException)
            {
                return new List<BusinessValidationResult> { new BusinessValidationResult("Error") };
            }
            else
            {
                return new List<BusinessValidationResult>();
            }
        }
    }

    [TestClass]
    public class BusinessValidatorBaseTest
    {
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestValidateCreate_ShouldThrowException()
        {
            var testValidator = new BusinessValidatorBaseTestClass(true);
            testValidator.ValidateCreate(new CreateTest());
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestValidateUpdate_ShouldThrowException()
        {
            var testValidator = new BusinessValidatorBaseTestClass(true);
            testValidator.ValidateUpdate(new UpdateTest());
        }
    }
}
