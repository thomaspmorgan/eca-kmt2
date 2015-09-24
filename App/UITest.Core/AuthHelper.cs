using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UITesting.HtmlControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UITest.Core
{
    public class AuthHelper
    {
        /*public AuthHelper()
        {

        }
        
        public AuthHelper(string username)
        {
            this.Username = username;
        }

        public string Username
        {
            get;
            set;
        }
        */
        public static BrowserWindow KMTLogin()
        {
            //Clear IE browser cache
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

            return browserwindow;
        }
    }
}
