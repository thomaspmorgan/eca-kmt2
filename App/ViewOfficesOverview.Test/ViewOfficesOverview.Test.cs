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

namespace ViewOfficesOverview.Test
{
    /// <summary>
    /// Verifies login to qa; navigate to ECA Office Directory; ECA Office Directory landing page; All Offices search box; search box functionality; search box results... 
    /// </summary>
    [CodedUITest]
    public class ViewOfficesOverviewCodedUITest1
    {
        public ViewOfficesOverviewCodedUITest1()
        {
        }

        [TestMethod]
        public void ViewOfficesOverviewCodedUITestMethod1()
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
            chcOffThemes.SearchProperties.Add(HtmlDiv.PropertyNames.TagName, "DIV", HtmlDiv.PropertyNames.InnerText, "Themes American Studies Arts Arts & Culture Civilian Security Conflict Prevention, Mitigation, and Response Democracy / Good Governance / Rule of Law Diversity Entrepreneurship / Job Creation Environment Humanitarian Assistance, Disaster Mitigation Markets and Competitiveness Sustainable Economic Growth & Well - Being Trade & Investment Transitions in Frontline States Transnational Threats - Crime, Narcotics, Trafficking in Person Travel and Tourism", HtmlDiv.PropertyNames.TagInstance, "35");
            chcOffThemes.WaitForControlReady();
            Assert.AreEqual(true, chcOffThemes.Exists);

            //verify goals
            HtmlDiv chcOffGoals = new HtmlDiv(browserWindow);
            chcOffGoals.SearchProperties.Add(HtmlDiv.PropertyNames.TagName, "DIV", HtmlDiv.PropertyNames.InnerText, "Goals Democracy and Human Rights Economic Development Global Economic Growth", HtmlDiv.PropertyNames.TagInstance, "38");
            chcOffGoals.WaitForControlReady();
            Assert.AreEqual(true, chcOffGoals.Exists);

            //verify POC(s)
            HtmlDiv chcOffPoc = new HtmlDiv(browserWindow);
            chcOffPoc.SearchProperties.Add(HtmlDiv.PropertyNames.TagName, "DIV", HtmlDiv.PropertyNames.InnerText, "Points of Contact Cultural Heritage Center Main Line Maria Kouroupas", HtmlDiv.PropertyNames.TagInstance, "45");
            chcOffPoc.WaitForControlReady();
            Assert.AreEqual(true, chcOffPoc.Exists);


            //old code
            //this.UIMap.LogintoQA_existing();
            //this.UIMap.NavigatetoECAOfficeDirectory_viaContentMenu();
            ////this.UIMap.NavigatetoECAOfficeDirectory();
            //this.UIMap.RefreshBrowser();
            //this.UIMap.NavigatetoECAOfficeDirectory_viaContentMenu();
            //this.UIMap.AssertAllOfficesBanner();
            //this.UIMap.AssertSearchOfficesTextBox();
            //this.UIMap.SearchBox_textinput();
            //this.UIMap.RefreshBrowser();
            //this.UIMap.SearchBox_textinput();
            //this.UIMap.AssertSearchShowingResults();
            //this.UIMap.AssertSearchResultTopList();
            //this.UIMap.CloseBrowserWindow();
            //// To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
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
