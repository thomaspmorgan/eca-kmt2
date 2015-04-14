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
    /// View Offices Programs & Branches-- Verifies the navigation to the ECA Office Directory page; Office Name display and available hyperlink to navigate to the individual office; Selecting the hyperlink for the individual office and navigating to the Office Overview page; branches & programs tab availablility; navigating to branches & programs under the currently selected office; branch listing availability; Programs list search text box; indented sub-program display; program name display; program name hyperlink availability; program description display; navigating back to the ECA Office Directory; Office Search text box functionality; Office search returned result; Selecting 2nd office and navigating to the branches and programs tab; Program search functionality; Program search text input accepted. 
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
            /*this.UIMap.RemoveExistingECAUser();
            this.UIMap.LogintoQA();*/
            this.UIMap.LogintoQA_ExistingUser();
            this.UIMap.AssertContentMenuButton();
            this.UIMap.SelectContentMenuButton();
            this.UIMap.AssertOfficesContentMenuLink();
            this.UIMap.SelectOfficesLink_ContentMenu();
            this.UIMap.RefreshECAOfficeDirectory();
            this.UIMap.SelectContentMenuButton();
            this.UIMap.SelectOfficesLink_ContentMenu();

            /*this.UIMap.SelectOfficesContentMenuLink();
            this.UIMap.URLNav_ECAOfficeDirect();*/
            this.UIMap.AssertOfficeName();
            this.UIMap.SelectOfficeName();
            this.UIMap.RefreshIndividualOfficePage();
            //if the refresh doesn't work after the selection fails, then reselect with the next line
            //this.UIMap.SelectOfficeName();

            this.UIMap.AssertIndividualOffice_BranchesandPrograms();
            this.UIMap.SelectBranchesandProgramsTab();
            this.UIMap.RefreshBranchesandProgramsTab();
            this.UIMap.SelectBranchesandProgramsTab();
            this.UIMap.AssertIndividualOffice_BranchList();
            this.UIMap.AssertSearchProgramsTextBox();
            this.UIMap.AssertSubProgramIndent();
            this.UIMap.AssertIndividualOffice_Program();
            this.UIMap.AssertIndividualOffice_ProgramNameDescription();

            //begin second office test
            this.UIMap.NavigatetoOfficeDirectory();
            //refresh page to load and attempt to navigate again on no action from NavigatetoOfficeDirectory method
            this.UIMap.RefreshBranchesandProgramsTab();
            this.UIMap.NavigatetoOfficeDirectory();
            this.UIMap.AssertSearchOfficesTextBox_SecOffice();
            this.UIMap.SearchOfficesTextInput_SecOffice();
            this.UIMap.RefreshECAOfficeDirectory();
            this.UIMap.SearchOfficesTextInput_SecOffice();
            this.UIMap.AssertOfficeSearchResult_SecOffice();

            /*this.UIMap.AssertSecondOffice(); this is no longer needed with the assert on the result of the search*/
            this.UIMap.SelectSecondOffice();
            this.UIMap.AssertSecondOffice_BranchesandPrograms();
            this.UIMap.SelectSecondBranchesandPrograms();
            //if no action- refresh and reselect Branches&Progs Tab
            this.UIMap.RefreshOfficeOverview_SecOffice();
            this.UIMap.SelectSecondBranchesandPrograms();
            this.UIMap.SearchBoxText_Input();
            this.UIMap.AssertSearchBoxTextInputValue();
            this.UIMap.ClearSearchProgramsBox();
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
