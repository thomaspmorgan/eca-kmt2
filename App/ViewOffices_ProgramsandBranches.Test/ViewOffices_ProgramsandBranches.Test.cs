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


namespace ViewOffices_ProgramsandBranches.Test
{
    /// <summary>
    /// View Offices Programs & Branches-- doesn't like navigate to Offices page from Content Menu--
    /// </summary>
    [CodedUITest]
    public class ViewOffices_ProgramsandBranchesCodedUITest1
    {
        public ViewOffices_ProgramsandBranchesCodedUITest1()
        {
        }

        [TestMethod]
        public void ViewOffices_ProgramsandBranchesCodedUITestMethod1()
        {
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
            this.UIMap.RemoveExistingECAUser();
            this.UIMap.LogintoQA();
            this.UIMap.AssertContentMenuButton();
            this.UIMap.SelectContentMenuButton();
            this.UIMap.AssertOfficesContentMenuLink();
            /*this.UIMap.SelectOfficesContentMenuLink();*/
            this.UIMap.URLNav_ECAOfficeDirect();
           /*this.UIMap.AssertOfficeName();
            this.UIMap.SelectOfficeName();
            this.UIMap.RefreshIndividualOfficePage();
            this.UIMap.AssertIndividualOffice_BranchesandPrograms();
            this.UIMap.SelectBranchesandProgramsTab();
            //this.UIMap.RefreshBranchesandProgramsTab();
            this.UIMap.AssertIndividualOffice_BranchList();
            this.UIMap.AssertSearchProgramsTextBox();
            this.UIMap.AssertIndividualOffice_Program();
            this.UIMap.AssertIndividualOffice_ProgramNameDescription();
            this.UIMap.NavigatetoOfficeDirectory();*/
            this.UIMap.AssertSecondOffice();
            this.UIMap.SelectSecondOffice();
            this.UIMap.AssertSecondOffice_BranchesandPrograms();
            this.UIMap.SelectSecondBranchesandPrograms();
            this.UIMap.SearchBoxText_Input();
            this.UIMap.AssertSearchBoxTextInputValue();
            this.UIMap.ClearSearchProgramsBox();
            this.UIMap.AssertSubProgramIndent();
            this.UIMap.CloseBrowserWindow();
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
