using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;
using ECA.Business.Service.Admin;
using ECA.Data;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Business.Queries.Models.Office;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Filter;
using System.Diagnostics;


namespace ViewOffices_ProgramsandBranches.Test
{
    /// <summary>
    /// View Offices Programs & Branches-- Verifies the navigation to the ECA Office Directory page; Office Name display and available hyperlink to navigate to the individual office; Selecting the hyperlink for the individual office and navigating to the Office Overview page; branches & programs tab availablility; navigating to branches & programs under the currently selected office; branch listing availability; Programs list search text box; indented sub-program display; program name display; program name hyperlink availability; program description display; navigating back to the ECA Office Directory; Office Search text box functionality; Office search returned result; Selecting 2nd office and navigating to the branches and programs tab; Program search functionality; Program search text input accepted. 
    /// </summary>
    [CodedUITest]
    public class ViewOffices_ProgramsandBranchesCodedUITest1
    {
        private OfficeService officeService;
        private EcaContext context;

        public ViewOffices_ProgramsandBranchesCodedUITest1()
        {
        }

        [TestInitialize]
        public void TestInit()
        {
            //var connectionString = @"Server=tcp:dx4ykgy2iu.database.windows.net,1433;Database=ECA_Dev;Persist Security Info=True;User ID=ECA@dx4ykgy2iu;Password=wisconsin-89;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;MultipleActiveResultSets=True";
            context = new EcaContext();//(connectionString);
            officeService = new OfficeService(context);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            officeService.Dispose();
            officeService = null;
        }

        [TestMethod]
        public void ViewOffices_ProgramsandBranchesCodedUITestMethod1()
        {

            var list = new List<string>();
            list.Add("brian");
            list.Add("alan");
            list.Add("brandon");

            var query = list.Where(x => x.Contains('i')).Where(x => x.Contains('a'));
            var executedQuery = query.FirstOrDefault();//list.Where(x => x == 1).ToList();


            var defaultSorter = new ExpressionSorter<SimpleOfficeDTO>(x => x.Name, SortDirection.Ascending);
            var orgIdFilter = new ExpressionFilter<SimpleOfficeDTO>(x => x.OrganizationId, ComparisonType.Equal, 1414);
            var queryOperator = new QueryableOperator<SimpleOfficeDTO>(0, 100, defaultSorter, new List<IFilter> { orgIdFilter }, null);

            var dtos = officeService.GetOffices(queryOperator);

            orgIdFilter.Value = 1036;
            var dtos2 = officeService.GetOffices(queryOperator);
            var myOffice = officeService.GetOfficeById(1036);


            
            Assert.AreEqual(1, dtos.Total, "There should only be one office.");
            var testOffice = dtos.Results.First();
            //var officeQuery = context.Organizations.Where(x => x.Name.Contains("cultural"));//.FirstOrDefault();
            //var sql = officeQuery.ToString();
            //var office = officeQuery.ToList();

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
            this.UIMap.AssertOfficeName(testOffice.Name);
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
            this.UIMap.RefreshBranchesandProgramsTab();

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
