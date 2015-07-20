namespace ProjectMoneyFlow.Test
{
    using Microsoft.VisualStudio.TestTools.UITesting.HtmlControls;
    using Microsoft.VisualStudio.TestTools.UITesting.WinControls;
    using System;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using Microsoft.VisualStudio.TestTools.UITest.Extension;
    using Microsoft.VisualStudio.TestTools.UITesting;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;
    using Mouse = Microsoft.VisualStudio.TestTools.UITesting.Mouse;
    using MouseButtons = System.Windows.Forms.MouseButtons;
    using System.Drawing;
    using System.Windows.Input;
    using System.Text.RegularExpressions;


    public partial class UIMap
    {

        /// <summary>
        /// Open browser; navigate to QA site; select ECATest user; enter password; click sign in.
        /// </summary>
        public void LogintoQA_ExistingUser()
        {
            #region Variable Declarations
            HtmlHyperlink uIECATest1statedeptusHyperlink = this.UINewtabInternetExplorWindow.UISignintoECAClientDocument1.UIECATest1statedeptusHyperlink;
            HtmlEdit uIPasswordEdit = this.UINewtabInternetExplorWindow.UISignintoECAClientDocument1.UIPasswordEdit;
            HtmlSpan uISigninPane = this.UINewtabInternetExplorWindow.UISignintoECAClientDocument1.UISigninPane;
            #endregion

            // Go to web page 'https://eca-kmt-qa.azurewebsites.net/' using new browser instance
            this.UINewtabInternetExplorWindow.LaunchUrl(new System.Uri(this.LogintoQA_ExistingUserParams.UINewtabInternetExplorWindowUrl));

            // Click 'ECATest1@statedept.us •••' link
            Mouse.Click(uIECATest1statedeptusHyperlink, new Point(121, 38));

            // Type '********' in 'Password' text box
            uIPasswordEdit.Password = this.LogintoQA_ExistingUserParams.UIPasswordEditPassword;

            // Click 'Sign in' pane
            Mouse.Click(uISigninPane, new Point(30, 17));
        }

        public virtual LogintoQA_ExistingUserParams LogintoQA_ExistingUserParams
        {
            get
            {
                if ((this.mLogintoQA_ExistingUserParams == null))
                {
                    this.mLogintoQA_ExistingUserParams = new LogintoQA_ExistingUserParams();
                }
                return this.mLogintoQA_ExistingUserParams;
            }
        }

        private LogintoQA_ExistingUserParams mLogintoQA_ExistingUserParams;

        /// <summary>
        /// Open browser; enter login creds for ECATest; click sign in
        /// </summary>
        public void LogintoQA()
        {
            #region Variable Declarations
            WinEdit uIAddressandsearchusinEdit = this.UINewtabInternetExplorWindow.UIAddressBarClient.UIAddressandsearchusinEdit;
            WinEdit uIItemEdit = this.UINewtabInternetExplorWindow.UIItemWindow.UIItemEdit;
            WinButton uIGotohttpsecakmtqaazuButton = this.UINewtabInternetExplorWindow.UIPageControlToolBar.UIGotohttpsecakmtqaazuButton;
            HtmlEdit uIUseraccountEdit = this.UINewtabInternetExplorWindow.UISignintoAzureActiveDDocument.UIUseraccountEdit;
            HtmlEdit uIPasswordEdit = this.UINewtabInternetExplorWindow.UISignintoAzureActiveDDocument.UIPasswordEdit;
            HtmlSpan uISigninPane = this.UINewtabInternetExplorWindow.UISignintoAzureActiveDDocument.UISigninPane;
            #endregion

            // Go to web page 'about:Tabs' using new browser instance
            this.UINewtabInternetExplorWindow.LaunchUrl(new System.Uri(this.LogintoQAParams.UINewtabInternetExplorWindowUrl));

            // Click 'Address and search using Google' text box
            Mouse.Click(uIAddressandsearchusinEdit, new Point(26, 10));

            // Type 'https://eca-kmt-qa.azurewebsites.net/' in text box
            Keyboard.SendKeys(uIItemEdit, this.LogintoQAParams.UIItemEditSendKeys, ModifierKeys.None);

            // Click 'Go to “https://eca-kmt-qa.azurewebsites.net/” (Alt...' button
            Mouse.Click(uIGotohttpsecakmtqaazuButton, new Point(4, 11));

            // Type 'ECATest1@statedept.us' in 'User account' text box
            uIUseraccountEdit.Text = this.LogintoQAParams.UIUseraccountEditText;

            // Type '{Tab}' in 'User account' text box
            Keyboard.SendKeys(uIUseraccountEdit, this.LogintoQAParams.UIUseraccountEditSendKeys, ModifierKeys.None);

            // Type '********' in 'Password' text box
            uIPasswordEdit.Password = this.LogintoQAParams.UIPasswordEditPassword;

            // Double-Click 'Sign in' pane
            Mouse.DoubleClick(uISigninPane, new Point(30, 13));
        }

        public virtual LogintoQAParams LogintoQAParams
        {
            get
            {
                if ((this.mLogintoQAParams == null))
                {
                    this.mLogintoQAParams = new LogintoQAParams();
                }
                return this.mLogintoQAParams;
            }
        }

        private LogintoQAParams mLogintoQAParams;
    }
    /// <summary>
    /// Parameters to be passed into 'LogintoQA_ExistingUser'
    /// </summary>
    [GeneratedCode("Coded UITest Builder", "12.0.31101.0")]
    public class LogintoQA_ExistingUserParams
    {

        #region Fields
        /// <summary>
        /// Go to web page 'https://eca-kmt-qa.azurewebsites.net/' using new browser instance
        /// </summary>
        public string UINewtabInternetExplorWindowUrl = "https://eca-kmt-qa.azurewebsites.net/";

        /// <summary>
        /// Type '********' in 'Password' text box
        /// </summary>
        public string UIPasswordEditPassword = "pnl8gvcmh7n9Hp5j+06Q16vTeHomf4bql8vy/6wcjU0=";
        #endregion
    }
    /// <summary>
    /// Parameters to be passed into 'LogintoQA'
    /// </summary>
    [GeneratedCode("Coded UITest Builder", "12.0.31101.0")]
    public class LogintoQAParams
    {

        #region Fields
        /// <summary>
        /// Go to web page 'about:Tabs' using new browser instance
        /// </summary>
        public string UINewtabInternetExplorWindowUrl = "about:Tabs";

        /// <summary>
        /// Type 'https://eca-kmt-qa.azurewebsites.net/' in text box
        /// </summary>
        public string UIItemEditSendKeys = "https://eca-kmt-qa.azurewebsites.net/";

        /// <summary>
        /// Type 'ECATest1@statedept.us' in 'User account' text box
        /// </summary>
        public string UIUseraccountEditText = "ECATest1@statedept.us";

        /// <summary>
        /// Type '{Tab}' in 'User account' text box
        /// </summary>
        public string UIUseraccountEditSendKeys = "{Tab}";

        /// <summary>
        /// Type '********' in 'Password' text box
        /// </summary>
        public string UIPasswordEditPassword = "pnl8gvcmh7nq2IDxDyIPucvLUfiP5WCkCWyYApPZam4=";
        #endregion
}
}
