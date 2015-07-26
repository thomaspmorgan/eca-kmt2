namespace Login.Test
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
        /// Open browser; navigate to QA site; select ECATest1 cred; input pw; click sign in.
        /// </summary>
        /// 
        

        public void LoginQA()
        {
            #region Variable Declarations
            HtmlEdit uIUseraccountEdit = this.UISigninInternetExplorWindow.UISigninDocument.UIUseraccountEdit;
            HtmlTable uIEcatest1_statedept_uTable = this.UINewtabInternetExplorWindow.UISignintoECAClientDocument.UIEcatest1_statedept_uTable;
            HtmlEdit uIPasswordEdit = this.UINewtabInternetExplorWindow.UISignintoECAClientDocument.UIPasswordEdit;
            HtmlSpan uISigninPane = this.UINewtabInternetExplorWindow.UISignintoECAClientDocument.UISigninPane;
            #endregion

            // Go to web page 'http://localhost:5556/index.html#/#top' using new browser instance
            this.UINewtabInternetExplorWindow.LaunchUrl(new System.Uri(this.LoginQAParams.UINewtabInternetExplorWindowUrl));

            // Type 'ECATest1@statedept.us' in Username box
            uIUseraccountEdit.Text = this.LoginQAParams.UIUseraccountEditText;

            // Click 'ecatest1_statedept_us' table
            //Mouse.Click(uIEcatest1_statedept_uTable, new Point(65, 47));

            // Type '********' in 'Password' text box
            uIPasswordEdit.Password = this.LoginQAParams.UIPasswordEditPassword;

            // Double-Click 'Sign in' pane
            Mouse.DoubleClick(uISigninPane, new Point(31, 18));
        }

        public virtual LoginQAParams LoginQAParams
        {
            get
            {
                if ((this.mLoginQAParams == null))
                {
                    this.mLoginQAParams = new LoginQAParams();
                }
                return this.mLoginQAParams;
            }
        }

        private LoginQAParams mLoginQAParams;

        /// <summary>
        /// Open browser; navigate to QA; select ECATest1 user; enter password; click sign in.
        /// </summary>
        public void LogintoQA_ExistingUser()
        {
            #region Variable Declarations
            HtmlCell uIECATest1statedeptusCell = this.UINewtabInternetExplorWindow.UISignintoECAClientDocument4.UIEcatest1_statedept_uTable.UIECATest1statedeptusCell;
            HtmlEdit uIPasswordEdit = this.UINewtabInternetExplorWindow.UISignintoECAClientDocument4.UIPasswordEdit;
            HtmlSpan uISigninPane = this.UINewtabInternetExplorWindow.UISignintoECAClientDocument4.UISigninPane;
            #endregion

            // Go to web page 'https://eca-kmt-qa.azurewebsites.net/' using new browser instance
            //this.UINewtabInternetExplorWindow.LaunchUrl(new System.Uri(this.LogintoQA_ExistingUserParams.UINewtabInternetExplorWindowUrl));

            // Go to web page 'http://localhost:5556/index.html#/#top' using new browser instance
            this.UINewtabInternetExplorWindow.LaunchUrl(new System.Uri(this.LogintoQA_ExistingUserParams.UINewtabInternetExplorWindowUrl));

            // Click 'ECATest1@statedept.us' cell
            Mouse.Click(uIECATest1statedeptusCell, new Point(65, 47));
            
            // Type '********' in 'Password' text box
            uIPasswordEdit.Password = this.LogintoQA_ExistingUserParams.UIPasswordEditPassword;

            // Click 'Sign in' pane
            Mouse.Click(uISigninPane, new Point(20, 13));
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
    /// Parameters to be passed into 'LoginQA'
    /// </summary>
    [GeneratedCode("Coded UITest Builder", "12.0.31101.0")]
    public class LoginQAParams
    {

        #region Fields
        /// <summary>
        /// Go to web page 'http://localhost:5556/index.html#/#top' using new browser instance
        /// </summary>
        public string UINewtabInternetExplorWindowUrl = "http://localhost:5556/index.html#/#top";

        // Type "ECATest1@statedept.us" in Username text box
        public string UIUseraccountEditText = "ECATest1@statedept.us";

        /// <summary>
        /// Type '********' in 'Password' text box
        /// </summary>
        public string UIPasswordEditPassword = "pnl8gvcmh7n9Hp5j+06Q16vTeHomf4bql8vy/6wcjU0=";
        #endregion
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
        //public string UINewtabInternetExplorWindowUrl = "https://eca-kmt-qa.azurewebsites.net/";

        /// <summary>
        /// Go to web page 'http://localhost:5556/index.html#/#top' using new browser instance
        /// </summary>
        public string UINewtabInternetExplorWindowUrl = "http://localhost:5556/index.html#/#top";

        /// <summary>
        /// Type '********' in 'Password' text box
        /// </summary>
        public string UIPasswordEditPassword = "pnl8gvcmh7n9Hp5j+06Q16vTeHomf4bql8vy/6wcjU0=";
        #endregion
}
}
