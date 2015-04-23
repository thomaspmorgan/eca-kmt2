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


namespace Viewhome.Test
{
    /// <summary>
    /// This test logs the user in and verifies that the three tabs on the homepage/landing page for the QA site are functional and available for display/view.
    /// </summary>
    [CodedUITest]
    public class ViewHomeCodedUITest1
    {
        public ViewHomeCodedUITest1()
        {
        }

        [TestMethod]
        public void ViewHomeCodedUITestMethod1()
        {
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.

            this.UIMap.LogintoQA_ExistingUser();

            /*this.UIMap.OpenBrowserEnterCreds();
            this.UIMap.SignInButton();
            this.UIMap.SignIn();*/
            this.UIMap.Notifications_ActivityLink();
            this.UIMap.SelectNotifications_ActivityLink();
            this.UIMap.NewsLink();
            this.UIMap.SelectNewsLink();
            this.UIMap.YourShortcutsLink();
            this.UIMap.SelectYourShortcutsLink();
            this.UIMap.CloseBrowserButton();
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
