﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by coded UI test builder.
//      Version: 12.0.0.0
//
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------

namespace Login.Test
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text.RegularExpressions;
    using System.Windows.Input;
    using Microsoft.VisualStudio.TestTools.UITest.Extension;
    using Microsoft.VisualStudio.TestTools.UITesting;
    using Microsoft.VisualStudio.TestTools.UITesting.HtmlControls;
    using Microsoft.VisualStudio.TestTools.UITesting.WinControls;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;
    using Mouse = Microsoft.VisualStudio.TestTools.UITesting.Mouse;
    using MouseButtons = System.Windows.Forms.MouseButtons;
    
    
    [GeneratedCode("Coded UITest Builder", "12.0.31101.0")]
    public partial class UIMap
    {
        
        /// <summary>
        /// Login to QA using ECATest Creds
        /// </summary>
        public void LoginQA()
        {
            #region Variable Declarations
            WinEdit uIAddressandsearchusinEdit = this.UINewtabInternetExplorWindow.UIAddressBarClient.UIAddressandsearchusinEdit;
            BrowserWindow uINewtabInternetExplorWindow = this.UINewtabInternetExplorWindow;
            HtmlEdit uIUseraccountEdit = this.UINewtabInternetExplorWindow.UISignintoAzureActiveDDocument.UIUseraccountEdit;
            HtmlEdit uIPasswordEdit = this.UINewtabInternetExplorWindow.UISignintoAzureActiveDDocument.UIPasswordEdit;
            HtmlSpan uISigninPane = this.UINewtabInternetExplorWindow.UISignintoAzureActiveDDocument.UISigninPane;
            #endregion

            // Go to web page 'about:Tabs' using new browser instance
            this.UINewtabInternetExplorWindow.LaunchUrl(new System.Uri(this.LoginQAParams.UINewtabInternetExplorWindowUrl));

            // Click 'Address and search using Google' text box
            Mouse.Click(uIAddressandsearchusinEdit, new Point(49, 3));

            // Go to web page 'https://eca-kmt-qa.azurewebsites.net/'
            uINewtabInternetExplorWindow.NavigateToUrl(new System.Uri(this.LoginQAParams.UINewtabInternetExplorWindowUrl1));

            // Type 'ECATest1@statedept.us' in 'User account' text box
            uIUseraccountEdit.Text = this.LoginQAParams.UIUseraccountEditText;

            // Type '' in 'Password' text box
            uIPasswordEdit.Text = this.LoginQAParams.UIPasswordEditText;

            // Type '********' in 'Password' text box
            uIPasswordEdit.Password = this.LoginQAParams.UIPasswordEditPassword;

            // Click 'Sign in' pane
            Mouse.Click(uISigninPane, new Point(44, 15));
        }
        
        #region Properties
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
        
        public UINewtabInternetExplorWindow UINewtabInternetExplorWindow
        {
            get
            {
                if ((this.mUINewtabInternetExplorWindow == null))
                {
                    this.mUINewtabInternetExplorWindow = new UINewtabInternetExplorWindow();
                }
                return this.mUINewtabInternetExplorWindow;
            }
        }
        #endregion
        
        #region Fields
        private LoginQAParams mLoginQAParams;
        
        private UINewtabInternetExplorWindow mUINewtabInternetExplorWindow;
        #endregion
    }
    
    /// <summary>
    /// Parameters to be passed into 'LoginQA'
    /// </summary>
    [GeneratedCode("Coded UITest Builder", "12.0.31101.0")]
    public class LoginQAParams
    {
        
        #region Fields
        /// <summary>
        /// Go to web page 'about:Tabs' using new browser instance
        /// </summary>
        public string UINewtabInternetExplorWindowUrl = "about:Tabs";
        
        /// <summary>
        /// Go to web page 'https://eca-kmt-qa.azurewebsites.net/'
        /// </summary>
        public string UINewtabInternetExplorWindowUrl1 = "https://eca-kmt-qa.azurewebsites.net/";
        
        /// <summary>
        /// Type 'ECATest1@statedept.us' in 'User account' text box
        /// </summary>
        public string UIUseraccountEditText = "ECATest1@statedept.us";
        
        /// <summary>
        /// Type '' in 'Password' text box
        /// </summary>
        public string UIPasswordEditText = "";
        
        /// <summary>
        /// Type '********' in 'Password' text box
        /// </summary>
        public string UIPasswordEditPassword = "pnl8gvcmh7nq2IDxDyIPucvLUfiP5WCkCWyYApPZam4=";
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "12.0.31101.0")]
    public class UINewtabInternetExplorWindow : BrowserWindow
    {
        
        public UINewtabInternetExplorWindow()
        {
            #region Search Criteria
            this.SearchProperties[UITestControl.PropertyNames.Name] = "New tab";
            this.SearchProperties[UITestControl.PropertyNames.ClassName] = "IEFrame";
            this.WindowTitles.Add("New tab");
            #endregion
        }
        
        public void LaunchUrl(System.Uri url)
        {
            this.CopyFrom(BrowserWindow.Launch(url));
        }
        
        #region Properties
        public UIAddressBarClient UIAddressBarClient
        {
            get
            {
                if ((this.mUIAddressBarClient == null))
                {
                    this.mUIAddressBarClient = new UIAddressBarClient(this);
                }
                return this.mUIAddressBarClient;
            }
        }
        
        public UISignintoAzureActiveDDocument UISignintoAzureActiveDDocument
        {
            get
            {
                if ((this.mUISignintoAzureActiveDDocument == null))
                {
                    this.mUISignintoAzureActiveDDocument = new UISignintoAzureActiveDDocument(this);
                }
                return this.mUISignintoAzureActiveDDocument;
            }
        }
        #endregion
        
        #region Fields
        private UIAddressBarClient mUIAddressBarClient;
        
        private UISignintoAzureActiveDDocument mUISignintoAzureActiveDDocument;
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "12.0.31101.0")]
    public class UIAddressBarClient : WinClient
    {
        
        public UIAddressBarClient(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Search Criteria
            this.SearchProperties[WinControl.PropertyNames.Name] = "Address Bar";
            this.WindowTitles.Add("New tab");
            #endregion
        }
        
        #region Properties
        public WinEdit UIAddressandsearchusinEdit
        {
            get
            {
                if ((this.mUIAddressandsearchusinEdit == null))
                {
                    this.mUIAddressandsearchusinEdit = new WinEdit(this);
                    #region Search Criteria
                    this.mUIAddressandsearchusinEdit.SearchProperties[WinEdit.PropertyNames.Name] = "Address and search using Google";
                    this.mUIAddressandsearchusinEdit.WindowTitles.Add("New tab");
                    #endregion
                }
                return this.mUIAddressandsearchusinEdit;
            }
        }
        #endregion
        
        #region Fields
        private WinEdit mUIAddressandsearchusinEdit;
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "12.0.31101.0")]
    public class UISignintoAzureActiveDDocument : HtmlDocument
    {
        
        public UISignintoAzureActiveDDocument(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Search Criteria
            this.SearchProperties[HtmlDocument.PropertyNames.Id] = null;
            this.SearchProperties[HtmlDocument.PropertyNames.RedirectingPage] = "False";
            this.SearchProperties[HtmlDocument.PropertyNames.FrameDocument] = "False";
            this.FilterProperties[HtmlDocument.PropertyNames.Title] = "Sign in to Azure Active Directory";
            this.FilterProperties[HtmlDocument.PropertyNames.AbsolutePath] = "/login.srf";
            this.FilterProperties[HtmlDocument.PropertyNames.PageUrl] = @"https://login.microsoftonline.com/login.srf?wa=wsignin1.0&wtrealm=https%3a%2f%2flogin.windows.net&wreply=https%3a%2f%2flogin.windows.net%2fstatedept.us%2fwsfederation&wctx=3wEBD09BdXRoMkF1dGhvcml6ZQEPT0F1dGgyQXV0aG9yaXplAQxzdGF0ZWRlcHQudXMBAQEVb3BlbmlkY29ubmVjdC5pZHRva2VuAAABASRlMDM1NmU1NS1lMTI0LTQ1MmMtODM3ZC1hZWI3NTA0MTg1ZmYBJWh0dHBzOi8vZWNhLWttdC1xYS5henVyZXdlYnNpdGVzLm5ldC8AAAAAAAAAAAAAAQEaAAAAASVodHRwczovL2VjYS1rbXQtcWEuYXp1cmV3ZWJzaXRlcy5uZXQvAQkBCGFkYWxfdmVyAQUwLjAuNwEDY2lkASQzMjA5NGJhYi1mOTQ0LTQxODUtYTcyOS04OTU3NDc0ZjZjZmIBCWVycm9yX3VyaQElaHR0cHM6Ly9lY2Eta210LXFhLmF6dXJld2Vic2l0ZXMubmV0LwEPaW5jbHVkZV9hdF9oYXNoAQExAQtpbnRlcmFjdGl2ZQEBMQEFTm9uY2UBJDRiOGFkNTVmLWY5ZDQtNDhiYi1iM2UzLTMwNDhhZjcxNzk5MgECcmMBJGEzYTc5YTQxLTMyZDktNDU3MS1hNjQ4LWY4Y2ZmYzAyYmNiYgEJc2Vzc2lvbklkASQ0ZTY4Yzk0Yi0yODdiLTQwZmUtODRmZC1jYmQwY2RkMDE1NmIBD2xpbWl0X3Rva2Vuc2l6ZQEBMe01&wp=MBI_FED_SSL&id=";
            this.WindowTitles.Add("Sign in to Azure Active Directory");
            #endregion
        }
        
        #region Properties
        public HtmlEdit UIUseraccountEdit
        {
            get
            {
                if ((this.mUIUseraccountEdit == null))
                {
                    this.mUIUseraccountEdit = new HtmlEdit(this);
                    #region Search Criteria
                    this.mUIUseraccountEdit.SearchProperties[HtmlEdit.PropertyNames.Id] = "cred_userid_inputtext";
                    this.mUIUseraccountEdit.SearchProperties[HtmlEdit.PropertyNames.Name] = "login";
                    this.mUIUseraccountEdit.FilterProperties[HtmlEdit.PropertyNames.LabeledBy] = "User account";
                    this.mUIUseraccountEdit.FilterProperties[HtmlEdit.PropertyNames.Type] = "SINGLELINE";
                    this.mUIUseraccountEdit.FilterProperties[HtmlEdit.PropertyNames.Title] = null;
                    this.mUIUseraccountEdit.FilterProperties[HtmlEdit.PropertyNames.Class] = "login_textfield textfield required email field normaltext";
                    this.mUIUseraccountEdit.FilterProperties[HtmlEdit.PropertyNames.ControlDefinition] = "name=\"login\" tabindex=\"1\" class=\"login_t";
                    this.mUIUseraccountEdit.FilterProperties[HtmlEdit.PropertyNames.TagInstance] = "1";
                    this.mUIUseraccountEdit.WindowTitles.Add("Sign in to Azure Active Directory");
                    #endregion
                }
                return this.mUIUseraccountEdit;
            }
        }
        
        public HtmlEdit UIPasswordEdit
        {
            get
            {
                if ((this.mUIPasswordEdit == null))
                {
                    this.mUIPasswordEdit = new HtmlEdit(this);
                    #region Search Criteria
                    this.mUIPasswordEdit.SearchProperties[HtmlEdit.PropertyNames.Id] = "cred_password_inputtext";
                    this.mUIPasswordEdit.SearchProperties[HtmlEdit.PropertyNames.Name] = "passwd";
                    this.mUIPasswordEdit.FilterProperties[HtmlEdit.PropertyNames.LabeledBy] = "Password";
                    this.mUIPasswordEdit.FilterProperties[HtmlEdit.PropertyNames.Type] = "PASSWORD";
                    this.mUIPasswordEdit.FilterProperties[HtmlEdit.PropertyNames.Title] = null;
                    this.mUIPasswordEdit.FilterProperties[HtmlEdit.PropertyNames.Class] = "login_textfield textfield required field normaltext";
                    this.mUIPasswordEdit.FilterProperties[HtmlEdit.PropertyNames.ControlDefinition] = "name=\"passwd\" tabindex=\"2\" class=\"login_";
                    this.mUIPasswordEdit.FilterProperties[HtmlEdit.PropertyNames.TagInstance] = "2";
                    this.mUIPasswordEdit.WindowTitles.Add("Sign in to Azure Active Directory");
                    #endregion
                }
                return this.mUIPasswordEdit;
            }
        }
        
        public HtmlSpan UISigninPane
        {
            get
            {
                if ((this.mUISigninPane == null))
                {
                    this.mUISigninPane = new HtmlSpan(this);
                    #region Search Criteria
                    this.mUISigninPane.SearchProperties[HtmlDiv.PropertyNames.Id] = "cred_sign_in_button";
                    this.mUISigninPane.SearchProperties[HtmlDiv.PropertyNames.Name] = null;
                    this.mUISigninPane.FilterProperties[HtmlDiv.PropertyNames.InnerText] = "Sign in";
                    this.mUISigninPane.FilterProperties[HtmlDiv.PropertyNames.Title] = null;
                    this.mUISigninPane.FilterProperties[HtmlDiv.PropertyNames.Class] = "button normaltext cred_sign_in_button refresh_domain_state";
                    this.mUISigninPane.FilterProperties[HtmlDiv.PropertyNames.ControlDefinition] = "tabindex=\"11\" class=\"button normaltext cred_sign_in_button refresh_domain_state\" " +
                        "id=\"cred_sign_in_button\" role=\"button\" style=\"opacity: 1;\" onclick=\"Post.SubmitC" +
                        "reds();return false;\"";
                    this.mUISigninPane.FilterProperties[HtmlDiv.PropertyNames.TagInstance] = "11";
                    this.mUISigninPane.WindowTitles.Add("Sign in to Azure Active Directory");
                    #endregion
                }
                return this.mUISigninPane;
            }
        }
        #endregion
        
        #region Fields
        private HtmlEdit mUIUseraccountEdit;
        
        private HtmlEdit mUIPasswordEdit;
        
        private HtmlSpan mUISigninPane;
        #endregion
    }
}