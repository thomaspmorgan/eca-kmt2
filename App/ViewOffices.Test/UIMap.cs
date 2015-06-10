namespace ViewOffices.Test
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
            HtmlHyperlink uIECATest1statedeptusHyperlink = this.UINewtabInternetExplorWindow.UISignintoECAClientDocument.UIECATest1statedeptusHyperlink;
            HtmlEdit uIPasswordEdit = this.UINewtabInternetExplorWindow.UISignintoECAClientDocument.UIPasswordEdit;
            HtmlSpan uISigninPane = this.UINewtabInternetExplorWindow.UISignintoECAClientDocument.UISigninPane;
            #endregion

            // Go to web page 'https://eca-kmt-qa.azurewebsites.net/' using new browser instance
            this.UINewtabInternetExplorWindow.LaunchUrl(new System.Uri(this.LogintoQA_ExistingUserParams.UINewtabInternetExplorWindowUrl));

            // Click 'ECATest1@statedept.us •••' link
            Mouse.Click(uIECATest1statedeptusHyperlink, new Point(115, 35));

            // Type '********' in 'Password' text box
            uIPasswordEdit.Password = this.LogintoQA_ExistingUserParams.UIPasswordEditPassword;

            // Click 'Sign in' pane
            Mouse.Click(uISigninPane, new Point(31, 16));
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
}
