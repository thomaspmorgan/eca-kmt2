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


namespace ProjectMoneyFlow.Test
{
    /// <summary>
    /// Summary description for CodedUITest1
    /// </summary>
    [CodedUITest]
    public class ProjectMoneyFlowCodedUITest1
    {
        public ProjectMoneyFlowCodedUITest1()
        {
        }

        [TestMethod]
        public void ProjectMoneyFlowCodedUITestMethod1()
        {
            //this.UIMap.LogintoQA();
            this.UIMap.LogintoQA_ExistingUser();
            this.UIMap.AssertContentMenuButton();
            this.UIMap.SelectContentMenuButton();
            this.UIMap.AssertProgramsLink_ContentMenu();
            this.UIMap.SelectProgramsLink_ContentMenu();
            //select programs link breaks here-- refresh page and then toggle the menu and select programs again
            this.UIMap.RefreshAllProgramsPage();
            this.UIMap.SelectContentMenuButton();
            this.UIMap.SelectProgramsLink_ContentMenu();
            //this.UIMap.ScrollDownAllProgramsPage();
            //needs wait- for load of individual program
            this.UIMap.AssertLinkforIndividualProgram();
            this.UIMap.SelectIndividualProgramLink();
            this.UIMap.RefreshIndividualProgramPage();
            this.UIMap.AssertBranchesandProjectsLink();
            this.UIMap.SelectBranchesandProjectsLink();
            //this.UIMap.ScrollDownIndividualProjectsPage();
            this.UIMap.AssertIndividualProject();
            this.UIMap.SelectIndividualProjectLink();
            this.UIMap.RefreshProjectOverviewPage();
            this.UIMap.AssertMoneyFlowLink();
            this.UIMap.SelectMoneyFlow();
            this.UIMap.RefreshMoneyFlow();
            //this.UIMap.ScrollDownMoneyFlowList();
            this.UIMap.AssertMoneyFlowList();

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
