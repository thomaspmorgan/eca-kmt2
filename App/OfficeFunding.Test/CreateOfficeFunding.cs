using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;
using UITest.Core;
using Microsoft.VisualStudio.TestTools.UITesting.HtmlControls;

namespace OfficeFunding.Test
{
    /// <summary>
    /// Summary description for CodedUITest1
    /// </summary>
    [CodedUITest]
    public class OfficeFunding
    {
        public OfficeFunding()
        {
        }

        [TestMethod]
        public void CreateOfficeFund()
        {
            var browserWindow = AuthHelper.KMTLogin();
            ContentMenu.AccessMenu(browserWindow);

            //select offices section
            HtmlHyperlink offices = new HtmlHyperlink(browserWindow);
            offices.SearchProperties.Add(HtmlHyperlink.PropertyNames.InnerText, "Offices", HtmlHyperlink.PropertyNames.ControlType, "Hyperlink", HtmlHyperlink.PropertyNames.TagInstance, "2");
            offices.WaitForControlReady();
            Mouse.Click(offices);

            //Verify office directory
            HtmlDiv officeDirect = new HtmlDiv(browserWindow);
            officeDirect.SearchProperties.Add(HtmlDiv.PropertyNames.InnerText, "ECA Office Directory", HtmlDiv.PropertyNames.TagName, "DIV");
            officeDirect.WaitForControlReady();
            Assert.AreEqual(true, officeDirect.Exists);

            //search field
            HtmlEdit offSearch = new HtmlEdit(browserWindow);
            offSearch.SearchProperties.Add(HtmlEdit.PropertyNames.TagName, "INPUT", HtmlEdit.PropertyNames.ControlType, "Edit", HtmlEdit.PropertyNames.TagInstance, "1");
            offSearch.WaitForControlReady();
            Assert.AreEqual(true, offSearch.Exists);

            //execute search
            Mouse.Click(offSearch);
            offSearch.Text = "Cultural Heritage Center";

            //verify office in results "Cultural Heritage Center"
            HtmlHyperlink chcOffice = new HtmlHyperlink(browserWindow);
            chcOffice.SearchProperties.Add(HtmlHyperlink.PropertyNames.InnerText, "Cultural Heritage Center", HtmlHyperlink.PropertyNames.TagInstance, "17");
            chcOffice.WaitForControlReady();
            Assert.AreEqual(true, chcOffice.Exists);

            //select chcOffice
            Mouse.Click(chcOffice);

            //select Funding Tab
            HtmlHyperlink offFund = new HtmlHyperlink(browserWindow);
            offFund.SearchProperties.Add(HtmlHyperlink.PropertyNames.InnerText, "Funding", HtmlHyperlink.PropertyNames.TagName, "A", HtmlHyperlink.PropertyNames.TagInstance, "22");
            offFund.WaitForControlReady();
            Assert.AreEqual(true, offFund.Exists);
            Mouse.Click(offFund);

            //Add Outgoing Funding Item***
            //
            AddOffFund.AddOffFundOut(browserWindow);
            


            
        }

        #region Additional test attributes

        // You can use the following additional attributes as you write your tests:

        ////Use TestInitialize to run code before running each test 
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{        
        //    // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        //}

        ////Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{        
        //    // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        //}

        #endregion

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        private TestContext testContextInstance;
    }
}
