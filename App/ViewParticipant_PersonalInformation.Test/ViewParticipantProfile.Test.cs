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


namespace ViewParticipantProfile.Test
{
    /// <summary>
    /// Originally View Participant Personal Information fields-- 
    /// 4/13/15 -Changed to View Participant Profile to include Contact section. This test will add on the assertions for each section as developed.
    /// This test verifies the login to QA, navigation to All Participants page, All Participants page display, participants list display, participant name hyperlink available, individual participant profile available by hyperlink navigation; participant profile display; PII section display; PII section fields display; PII fields available and display data when applicable; Contact section display; Contact section fields display; Contact fields available and display data when applicable.
    /// 
    /// Think about adding second participant with full data and access via Participant Search-- Nicole Yoo
    /// </summary>
    [CodedUITest]
    public class ViewParticipantProfileCodedUITest1
    {
        public ViewParticipantProfileCodedUITest1()
        {
        }

        [TestMethod]
        public void ViewParticipantProfileCodedUITestMethod1()
        {
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.

            /*think about setting up the logintoQA to continue on fail for the existing user screen and the LogintoQA_existinguser to continue on fail to proceed to the next step*/

            //this.UIMap.LogintoQA();
            this.UIMap.LogintoQA_ExistingUser();
            this.UIMap.AssertContentMenuButton();
            this.UIMap.SelectContentMenuButton();
            this.UIMap.AssertParticipantsLink();
            this.UIMap.SelectContentMenu_ParticipantsLink();
            this.UIMap.RefreshHomePage();
            this.UIMap.SelectContentMenuButton();
            this.UIMap.SelectContentMenu_ParticipantsLink();
            this.UIMap.AssertParticipantsBanner();
            this.UIMap.AssertParticipantsNameLink();
            this.UIMap.SelectParticipant();
            this.UIMap.RefreshParticipantsPage();
            this.UIMap.SelectParticipant();
            this.UIMap.AssertParticipantNameHeading();
            this.UIMap.RefreshParticipantsPage();
            this.UIMap.AssertPIISectionFields();
            this.UIMap.AssertContactSectionFields();
            //next participant- navigate using breadcrumb back to participants list and then search Steve Pike for General and Edu&Employ sections
            this.UIMap.AssertParticipantsBreadcrumb();
            this.UIMap.SelectParticipantsBreadcrumb();
            this.UIMap.FilterParticipantName_SelectParticipant2();
            this.UIMap.AssertGeneralSection();
            this.UIMap.AssertEducation_EmploymentSection();

            //this.UIMap.AssertParticipantsPersonalInfo(); not needed based on individual section and field assertion
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
