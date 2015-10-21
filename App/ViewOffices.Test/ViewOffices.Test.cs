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
using ECA.Core.DynamicLinq;
using ECA.Business.Queries.Models.Office;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.DynamicLinq.Filter;
using UITest.Core;
using Microsoft.VisualStudio.TestTools.UITesting.HtmlControls;


namespace ViewOffices.Test
{
    /// <summary>
    /// View Offices 
    /// Verifies the login to QA; navigate to office page; use search offices box; search results returned; office returned from search; office description; office name; office acronym; and the office name being a hyperlink to navigate to the office.
    /// </summary>
    [CodedUITest]
    public class ViewOffices
    { /*
        private OfficeService officeService;
        private EcaContext context; */

        public ViewOffices()
        {
        }

        [TestInitialize]
        public void TestInit()
        {/*
            var connectionString = @"Server=tcp:BE00003110,1433;Database=ECA_Local;Persist Security Info=True;User ID=ECA;Password=wisconsin-89;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;MultipleActiveResultSets=True";
            context = new EcaContext(connectionString);
            officeService = new OfficeService(context); */
        }

        [TestCleanup]
        public void TestCleanup()
        {
            /* officeService.Dispose();
             officeService = null; */
        }

        [TestMethod]
        public void ViewOfficesPage()
        {
            /* var defaultSorter = new ExpressionSorter<SimpleOfficeDTO>(x => x.Name, SortDirection.Ascending);
            var nameFilter = new ExpressionFilter<SimpleOfficeDTO>(x => x.Name, ComparisonType.Like, "Cultural Heritage");
            var queryOperator = new QueryableOperator<SimpleOfficeDTO>(0, 100, defaultSorter, new List<IFilter> { nameFilter }, null);

            var dtos = officeService.GetOffices(queryOperator);
            var testOffice = dtos.Results.First();
            var searchText = testOffice.Name;
            var linkText = testOffice.Name;
            var total = dtos.Total; */

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

             //showing count
             HtmlDiv showingOff = new HtmlDiv(browserWindow);
             showingOff.SearchProperties.Add(HtmlDiv.PropertyNames.TagName, "DIV", HtmlDiv.PropertyNames.InnerText, "Showing 1 - 25 of 56 offices");
             showingOff.WaitForControlReady();
             Assert.AreEqual(true, showingOff.Exists);

            //showingOff DB Verify-- use sql connection for verifying the office count
            var connectionString = "Data Source=(local);User Id=ECA;Password=wisconsin-89;Database=ECA_Local;Pooling=False";
            using (var context = new EcaContext(connectionString))
            using (var service = new OfficeService(context))
            {
                var defaultSorter = new ExpressionSorter<SimpleOfficeDTO>(x => x.OfficeSymbol, SortDirection.Ascending);
                var queryOperator = new QueryableOperator<SimpleOfficeDTO>(0, 10, defaultSorter);
                var results = service.GetOffices(queryOperator);
                Assert.AreEqual(results.Total, 56);
            }
            
            //verify the view count of Offices with Cultural in the name
            using (var context = new EcaContext(connectionString))
            using (var service = new OfficeService(context))
            {
                var defaultSorter = new ExpressionSorter<SimpleOfficeDTO>(x => x.OfficeSymbol, SortDirection.Ascending);
                var filter = new ExpressionFilter<SimpleOfficeDTO>(x => x.Name, ComparisonType.Like, "Cultural");
                var queryOperator = new QueryableOperator<SimpleOfficeDTO>(0, 10, defaultSorter, new List<IFilter> { filter });
                var results = service.GetOffices(queryOperator);
                Assert.AreEqual(results.Total, 3);
            }






            //old code
            /* this.UIMap.LogintoQA_ExistingUser();
            //this.UIMap.LogintoQA();
            this.UIMap.AssertContentMenuButton();
            this.UIMap.SelectContentMenuButton();
            this.UIMap.AssertOfficesContentMenuLink();
            this.UIMap.SelectOfficesContentMenuLink();
            this.UIMap.RefreshAllOffices_ECAOfficeDirectoryPage();
            this.UIMap.SelectContentMenuButton();
            this.UIMap.SelectOfficesContentMenu_Link();
            //this.UIMap.RefreshAllOffices_ECAOfficeDirectoryPage();
            this.UIMap.AssertECAOfficeDirectoryPage();
            this.UIMap.AssertSearchOffices();
            this.UIMap.InputSearchOfficeText();
            this.UIMap.RefreshAllOffices_ECAOfficeDirectoryPage();
            this.UIMap.InputSearchOfficeText();
            this.UIMap.AssertSearchOfficesTextResults();
            this.UIMap.AssertAvailableOfficeinList();
            //this.UIMap.AssertOfficeAvailableinList();
            this.UIMap.CloseBrowserWindow(); */
            //end old code

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
