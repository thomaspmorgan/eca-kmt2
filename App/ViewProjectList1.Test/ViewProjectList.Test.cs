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


namespace ViewProjectList1.Test
{
    /// <summary>
    /// Verifies the user can - login to QA; navigate to the All Programs page; select a program; view the program branches and projects section; view the list of projects available for the program; the project title displays; project year displays; project status displays; project region/location displays; filter boxes are available for edit for project list for each title, status, year, and region.
    /// </summary>
    [CodedUITest]
    public class ViewProjectListCodedUITest1
    {
        public ViewProjectListCodedUITest1()
        {
        }

        [TestMethod]
        public void ViewProjectListCodedUITestMethod1()
        {
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
            this.UIMap.LogintoQA_ExistingUser();
            this.UIMap.AssertContentMenuButton();
            this.UIMap.SelectPrograms_ContentMenu();
            this.UIMap.RefreshAllProgramsPage();
            this.UIMap.SelectPrograms_ContentMenu();
            //this.UIMap.AssertPageNum_ProgList();
            //this.UIMap.SelectPageNum_ProgList();
            this.UIMap.RefreshAllProgramsPage();
            this.UIMap.AssertPageNum4_ProgList();
            this.UIMap.SelectPageNum4_ProgList();
            this.UIMap.AssertIndividualProgram();
            this.UIMap.SelectIndividualProgram();
            this.UIMap.AssertIndProgram_BranchProjectTab();
            this.UIMap.SelectIndProg_BranchesProjectTab();
            this.UIMap.AssertIndProg_ProjectList();
            this.UIMap.AssertEditFilterBoxes_ProjectList();
            this.UIMap.AssertCHTestProj();
            this.UIMap.SelectCHTestproj();
            this.UIMap.AssertProj_CatFocus_ObjJust();

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
