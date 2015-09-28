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

namespace Viewhome.Test
{
    /// <summary>
    /// This test logs the user in and verifies that the three tabs on the homepage/landing page for the QA site are functional and available for display/view.
    /// </summary>
    [CodedUITest]
    public class ViewHome
    {
        public ViewHome()
        {
        }

        [TestMethod]
        public void ViewHomepage()
        {
            //KMT Login with ECATest1 user and pw
            
            var browserWindow = AuthHelper.KMTLogin();

            //check content menu
            ContentMenu.AccessMenu(browserWindow);

            //check hyperlinks for Your Shortcuts, Notifications and Timeline, and News

            //shortcuts
            HtmlHyperlink yourShortcuts = new HtmlHyperlink(browserWindow);
            yourShortcuts.SearchProperties.Add(HtmlHyperlink.PropertyNames.InnerText, "Your Shortcuts", HtmlHyperlink.PropertyNames.ControlType, "Hyperlink", HtmlHyperlink.PropertyNames.TagInstance, "13");
            yourShortcuts.WaitForControlReady();
            Assert.AreEqual(true, yourShortcuts.Exists);

            //notifications and timeline
            HtmlHyperlink notifications = new HtmlHyperlink(browserWindow);
            notifications.SearchProperties.Add(HtmlHyperlink.PropertyNames.InnerText, "Notifications & Timeline", HtmlHyperlink.PropertyNames.ControlType, "Hyperlink", HtmlHyperlink.PropertyNames.TagInstance, "14");
            notifications.WaitForControlReady();
            Assert.AreEqual(true, notifications.Exists);

            //news
            HtmlHyperlink news = new HtmlHyperlink(browserWindow);
            news.SearchProperties.Add(HtmlHyperlink.PropertyNames.InnerText, "News (3)", HtmlHyperlink.PropertyNames.ControlType, "Hyperlink", HtmlHyperlink.PropertyNames.TagInstance, "15");
            news.WaitForControlReady();
            Assert.AreEqual(true, news.Exists);

            //old code
            /*this.UIMap.LogintoQA_ExistingUser();
            this.UIMap.OpenBrowserEnterCreds();
            this.UIMap.SignInButton();
            this.UIMap.SignIn();
            this.UIMap.Notifications_ActivityLink();
            this.UIMap.SelectNotifications_ActivityLink();
            this.UIMap.NewsLink();
            this.UIMap.SelectNewsLink();
            this.UIMap.YourShortcutsLink();
            this.UIMap.SelectYourShortcutsLink();
            this.UIMap.CloseBrowserButton();
            this.UIMap.CloseBrowser();*/
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
