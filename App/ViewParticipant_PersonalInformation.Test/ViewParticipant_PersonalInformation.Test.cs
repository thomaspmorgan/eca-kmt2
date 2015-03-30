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


namespace ViewParticipant_PersonalInformation.Test
{
    /// <summary>
    /// View Participant Personal Information fields-- doesn't like the content menu link for participant-- 
    /// </summary>
    [CodedUITest]
    public class ViewParticipant_PersonalInformationCodedUITest1
    {
        public ViewParticipant_PersonalInformationCodedUITest1()
        {
        }

        [TestMethod]
        public void ViewParticipant_PersonalInformationCodedUITestMethod1()
        {
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
            this.UIMap.LogintoQA();
            this.UIMap.AssertContentMenuButton();
            this.UIMap.SelectContentMenuButton();
            this.UIMap.AssertParticipantsLink();
            this.UIMap.SelectContentMenu_ParticipantsLink();
            this.UIMap.AssertParticipantsBanner();
            this.UIMap.AssertParticipantsNameLink();
            this.UIMap.SelectParticipant();
            this.UIMap.AssertParticipantsPersonalInfo();
            this.UIMap.AssertParticipantNameHeading();
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
