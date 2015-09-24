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
using Microsoft.VisualStudio.TestTools.UITesting.HtmlControls;
using UITest.Core;

namespace LocalLogin
{
    /// <summary>
    /// Summary description for CodedUITest1
    /// </summary>
    [CodedUITest]
    public class LocalLoginTest
    {
        public LocalLoginTest()
        {
        }

        [TestMethod]
        public void KMTLogin()
        {
            /* //Clear IE browser cache
             BrowserWindow.ClearCache();

             //Launch IE browser and navigate to the KMT site.
             BrowserWindow browserwindow = BrowserWindow.Launch(new System.Uri("http://localhost:5556"));

             //Enter username and password
             HtmlEdit Username = new HtmlEdit(browserwindow);
             Username.SearchProperties.Add(HtmlEdit.PropertyNames.TagName, "INPUT", HtmlEdit.PropertyNames.Id, "cred_userid_inputtext");
             Username.Text = "ECATest1@statedept.us";

             HtmlEdit Password = new HtmlEdit(browserwindow);
             Password.SearchProperties.Add(HtmlEdit.PropertyNames.TagName, "INPUT", HtmlEdit.PropertyNames.Id, "cred_password_inputtext");
             Password.Text = "ECATeam!2015_3";

             //Click Sign In button

             HtmlControl SigninButton = new HtmlControl(browserwindow);
             SigninButton.SearchProperties.Add(HtmlControl.PropertyNames.TagName, "SPAN", HtmlControl.PropertyNames.Id, "cred_sign_in_button", HtmlControl.PropertyNames.InnerText, "Sign in");
             Mouse.Click(SigninButton);
                 */

           /* var myinstance = new AuthHelper("Alan Compton");
            var myotherinstance = new AuthHelper("Brian");

            
            myotherinstance.Username = "y";*/
          
           var browserwindow = AuthHelper.KMTLogin();
           /* var contentmenu = new HtmlControl(browserwindow);
            HtmlControl ContentmenuIcon = new HtmlControl(browserwindow);
            ContentmenuIcon.SearchProperties.Add(HtmlControl.PropertyNames.TagName, "BUTTON", HtmlControl.PropertyNames.InnerText, "Toggle navigation");
            */
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
    }
}
