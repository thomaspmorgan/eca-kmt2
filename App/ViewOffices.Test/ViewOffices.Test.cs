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


namespace ViewOffices.Test
{
    /// <summary>
    /// View Offices --under construction-- doesn't like the Offices Link, won't navigate to All Offices
    /// </summary>
    [CodedUITest]
    public class ViewOfficesCodedUITest1
    {
        public ViewOfficesCodedUITest1()
        {
        }

        [TestMethod]
        public void ViewOfficesCodedUITestMethod1()
        {

            this.UIMap.LogintoQA_ExistingUser();
            //this.UIMap.LogintoQA();
            this.UIMap.AssertContentMenuButton();
            this.UIMap.SelectContentMenuButton();
            this.UIMap.AssertOfficesContentMenuLink();
            /*this.UIMap.SelectOfficesContentMenuLink();*/
            this.UIMap.SelectOfficesContentMenu_Link();
            this.UIMap.RefreshAllOffices_ECAOfficeDirectoryPage();
            this.UIMap.AssertECAOfficeDirectoryPage();
            this.UIMap.AssertSearchOffices();
            this.UIMap.AssertAvailableOfficeinList();
            this.UIMap.AssertOfficeAvailableinList();
            this.UIMap.CloseBrowserWindow();

            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
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

        public UIMap UIMap
        {
            get
            {
                if ((this.map == null))
                {
                    this.map = new UIMap();
                }

                return this.map;
            }
        }

        private UIMap map;
    }
}
