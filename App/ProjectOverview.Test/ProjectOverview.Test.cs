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


namespace ProjectOverview.Test
{
    /// <summary>
    /// This test is compiled of a series of steps to reach the Project Overview for a specific individual Project under a Program.
    /// </summary>
    [CodedUITest]
    public class ProjectOverviewCodedUITest1
    {
        public ProjectOverviewCodedUITest1()
        {
        }

        [TestMethod]
        public void ProjectOverviewCodedUITestMethod1()
        {
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
            this.UIMap.LogintoQA();
            this.UIMap.ContentMenuButton();
            this.UIMap.SelectContentMenuButton();
            this.UIMap.ContentMenu_ProgramsLink();
            this.UIMap.SelectProgramsLink();
            this.UIMap.RefreshAllProgramsPage();
            //this.UIMap.ScrollDownAllPrograms();
            this.UIMap.AllProgramsPageNumber();
            this.UIMap.SelectAllProgramsPageNumber();
            this.UIMap.ScrollDowntoIndividualProgram();
            this.UIMap.IndividualProgramLink();
            this.UIMap.SelectIndividualProgram();
            this.UIMap.RefreshIndividualProgramPage();
            this.UIMap.BranchesandProjectsLink();
            this.UIMap.SelectBranchesandProjectsLink();
            this.UIMap.RefreshBranchesandProjectsPage();
            //this.UIMap.ScrollDownBranchesandProjects();
            this.UIMap.IndividualProjectLink();
            this.UIMap.SelectIndividualProject();
            this.UIMap.RefreshProjectOverviewPage();
            this.UIMap.ProjectsOverviewSection();
            this.UIMap.CloseBrowser();
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
