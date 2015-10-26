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
using UITest.Core;
using Microsoft.VisualStudio.TestTools.UITesting.HtmlControls;
using ECA.Business.Service.Programs;
using ECA.Business.Queries.Models.Admin;

namespace ViewOfficesProgramsandBranches.Test
{
    /// <summary>
    /// View Offices Programs & Branches-- Verifies the navigation to the ECA Office Directory page; Office Name display and available hyperlink to navigate to the individual office; Selecting the hyperlink for the individual office and navigating to the Office Overview page; branches & programs tab availablility; navigating to branches & programs under the currently selected office; branch listing availability; Programs list search text box; indented sub-program display; program name display; program name hyperlink availability; program description display; navigating back to the ECA Office Directory; Office Search text box functionality; Office search returned result; Selecting 2nd office and navigating to the branches and programs tab; Program search functionality; Program search text input accepted. 
    /// </summary>
    [CodedUITest]
    public class ViewOfficesProgramsandBranches
    {
        /*private OfficeService officeService;
        private EcaContext context;*/

        public ViewOfficesProgramsandBranches()
        {
        }
        /*
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
        }*/

        [TestMethod]
        public void ViewOffices_ProgramsandBranches()
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

            //verify chcOffice overview
            HtmlDiv chcOffHeader = new HtmlDiv(browserWindow);
            chcOffHeader.SearchProperties.Add(HtmlDiv.PropertyNames.TagName, "DIV", HtmlDiv.PropertyNames.InnerText, "OFFICE Cultural Heritage Center ECA/P/C Permalink: http://localhost:5556/#/offices/1036/overview#top", HtmlDiv.PropertyNames.TagInstance, "15");
            chcOffHeader.WaitForControlReady();
            Assert.AreEqual(true, chcOffHeader.Exists);

            //verify description
            HtmlDiv chcOffDescription = new HtmlDiv(browserWindow);
            chcOffDescription.SearchProperties.Add(HtmlDiv.PropertyNames.TagName, "DIV", HtmlDiv.PropertyNames.InnerText, "The Cultural Heritage Center supports the protection and preservation of cultural heritage and serves as a center of expertise on global cultural heritage protection issues. The Center administers U.S. responsibilities related to the 1970 UNESCO Convention on the Means of Prohibiting and Preventing the Illicit Import, Export, and Transfer of Ownership of Cultural Property, including the Cultural Property Advisory Committee and bilateral agreements; the U.S. Ambassadors Fund for Cultural Preservation (AFCP); the Cultural Antiquities Task Force; and efforts to coordinate protection and preservation of cultural heritage in disaster situations.", HtmlDiv.PropertyNames.TagInstance, "31");
            chcOffDescription.WaitForControlReady();
            Assert.AreEqual(true, chcOffDescription.Exists);

            //verify themes
            HtmlDiv chcOffThemes = new HtmlDiv(browserWindow);
            chcOffThemes.SearchProperties.Add(HtmlDiv.PropertyNames.TagName, "DIV", HtmlDiv.PropertyNames.InnerText, "Themes American Studies Civilian Security Conflict Prevention, Mitigation, and Response Democracy/Good Governance/Rule of Law Diversity Entrepreneurship/Job Creation Environment Humanitarian Assistance, Disaster Mitigation Markets and Competitiveness Sustainable Economic Growth & Well-Being Trade & Investment Transitions in Frontline States Transnational Threats - Crime, Narcotics, Trafficking in Person Travel and Tourism", HtmlDiv.PropertyNames.TagInstance, "35", HtmlControl.PropertyNames.ControlType, "Pane");
            chcOffThemes.WaitForControlReady();
            Assert.AreEqual(true, chcOffThemes.Exists);

            //Html chcOffThemes = new HtmlPane(browserWindow);
            //chcOffThemes.SearchProperties.Add(HtmlDiv.PropertyNames.TagName, "DIV", HtmlDiv.PropertyNames.InnerText, "Themes American Studies Civilian Security Conflict Prevention, Mitigation, and Response Democracy/Good Governance/Rule of Law Diversity Entrepreneurship/Job Creation Environment Humanitarian Assistance, Disaster Mitigation Markets and Competitiveness Sustainable Economic Growth & Well-Being Trade & Investment Transitions in Frontline States Transnational Threats - Crime, Narcotics, Trafficking in Person Travel and Tourism", HtmlDiv.PropertyNames.TagInstance, "35", HtmlControl.PropertyNames.ControlType, "Pane");
            //chcOffThemes.WaitForControlReady();
            //Assert.AreEqual(true, chcOffThemes.Exists);

            //verify goals
            HtmlDiv chcOffGoals = new HtmlDiv(browserWindow);
            chcOffGoals.SearchProperties.Add(HtmlDiv.PropertyNames.TagName, "DIV", HtmlDiv.PropertyNames.InnerText, "Goals Democracy and Human Rights", HtmlDiv.PropertyNames.TagInstance, "38", HtmlControl.PropertyNames.ControlType, "Pane");
            chcOffGoals.WaitForControlReady();
            Assert.AreEqual(true, chcOffGoals.Exists);

            //verify POC(s)
            HtmlDiv chcOffPoc = new HtmlDiv(browserWindow);
            chcOffPoc.SearchProperties.Add(HtmlDiv.PropertyNames.TagName, "DIV", HtmlDiv.PropertyNames.InnerText, "Points of Contact Cultural Heritage Center Main Line Maria Kouroupas", HtmlDiv.PropertyNames.TagInstance, "45", HtmlControl.PropertyNames.ControlType, "Pane");
            chcOffPoc.WaitForControlReady();
            Assert.AreEqual(true, chcOffPoc.Exists);

            //select branches and programs tab
            HtmlHyperlink chcBranchProgTab = new HtmlHyperlink(browserWindow);
            chcBranchProgTab.SearchProperties.Add(HtmlHyperlink.PropertyNames.TagName, "A", HtmlHyperlink.PropertyNames.InnerText, "Branches & Programs", HtmlHyperlink.PropertyNames.TagInstance, "19");
            chcBranchProgTab.WaitForControlReady();
            Assert.AreEqual(true, chcBranchProgTab.Exists);
            Mouse.Click(chcBranchProgTab);

            //Identify Showing # - # of ##
            HtmlDiv showBPList = new HtmlDiv(browserWindow);
            showBPList.SearchProperties.Add(HtmlDiv.PropertyNames.TagName, "DIV", HtmlDiv.PropertyNames.InnerText, "Showing 1 - 10 of 10 programs", HtmlDiv.PropertyNames.TagInstance, "33");
            showBPList.WaitForControlReady();
            Assert.AreEqual(true, showBPList.Exists);

            //verify branches and programs tab section
            var connectionString = "Data Source=(local);User Id=ECA;Password=wisconsin-89;Database=ECA_Local;Pooling=False";
            using (var context = new EcaContext(connectionString))
            using (var service = new OfficeService(context))
            {
                var start = 0;
                var limit = 10;
                
                //var officeId = 1036;

                var defaultSorter = new ExpressionSorter<OrganizationProgramDTO>(x => x.Name, SortDirection.Ascending);
                var queryOperator = new QueryableOperator<OrganizationProgramDTO>(0, 10, defaultSorter);
                var results = service.GetPrograms(1036, queryOperator);
                var expectedString = String.Format("Showing {0} - {1} of {2} programs", start +1, limit, results.Total);
                    Assert.AreEqual(expectedString, showBPList.InnerText);

            }









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
