using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Model.CreateEV;

namespace ECA.Business.Test.Validation.Model.CreateEV
{
    [TestClass]
    public class ExchangeVisitorValidatorTest
    {
        private ExchangeVisitor GetValidExchangeVisitor()
        {
            return new ExchangeVisitor
            {
                AddSiteOfActivity = null,
                AddTIPP = null,
                Biographical = null,
                CategoryCode = null,
                CreateDependent = null,
                FinancialInfo = null,
                MailAddress = null,
                OccupationCategoryCode = null,
                PositionCode = null,
                PrgEndDate = DateTime.Now, 
                PrgStartDate = DateTime.Now,
                ResidentialAddress = null,
                SubjectField = null,
                USAddress = null,
                requestID = "request",
                userID = "1"
            };
        }

        //[TestMethod]
        public void TestAddSiteOfActivity_ShouldRunValidator()
        {
            var instance = GetValidExchangeVisitor();
            var validator = new ExchangeVisitorValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            instance.AddSiteOfActivity = new AddSiteOfActivity();
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);

            //basically where I left off is that I know we need to be unit testing the validators.  I think I have
            //most of the create side of the validators tested and went back tot the exchangevisitor to work my way
            //back down the class structure to ensure all of the validators are hit.  I still need to make sure the 
            //lower validators all call their child validators, like FinancialInfoValidator(Test) does
            //then I'll need to move to the update validators...


        }
    }
}
