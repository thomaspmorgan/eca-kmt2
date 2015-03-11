﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by coded UI test builder.
//      Version: 12.0.0.0
//
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------

namespace Viewhome.Test
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
        /// Perform Close Browser by clicking (X)
        /// </summary>
        public void CloseBrowser()
        {
            #region Variable Declarations
            BrowserWindow uINewtabInternetExplorWindow = this.UINewtabInternetExplorWindow;
            #endregion

            // Perform Close on Browser Window
            uINewtabInternetExplorWindow.Close();
        }
        
        /// <summary>
        /// Close Browser Button check
        /// </summary>
        public void CloseBrowserButton()
        {
            #region Variable Declarations
            WinButton uICloseButton = this.UINewtabInternetExplorWindow.UIHttpsecakmtqaazureweTitleBar.UICloseButton;
            #endregion

            // Verify that the 'ControlType' property of 'Close' button equals 'Button'
            Assert.AreEqual(this.CloseBrowserButtonExpectedValues.UICloseButtonControlType, uICloseButton.ControlType.ToString(), "Can\'t close browser");
        }
        
        /// <summary>
        /// Check for News Link
        /// </summary>
        public void NewsLink()
        {
            #region Variable Declarations
            HtmlHyperlink uINews3Hyperlink = this.UINewtabInternetExplorWindow.UIHttpsecakmtqaazureweDocument.UITopPane.UINews3Hyperlink;
            #endregion

            // Verify that the 'InnerText' property of 'News (3)' link equals 'News (3)'
            Assert.AreEqual(this.NewsLinkExpectedValues.UINews3HyperlinkInnerText, uINews3Hyperlink.InnerText, "No News(3) Link");
        }
        
        /// <summary>
        /// Check for Notifications & Activity Link
        /// </summary>
        public void Notifications_ActivityLink()
        {
            #region Variable Declarations
            HtmlHyperlink uINotificationsActivitHyperlink = this.UINewtabInternetExplorWindow.UIHttpsecakmtqaazureweDocument.UITopPane.UINotificationsActivitHyperlink;
            #endregion

            // Verify that the 'InnerText' property of 'Notifications & Activity' link equals 'Notifications & Activity'
            Assert.AreEqual(this.Notifications_ActivityLinkExpectedValues.UINotificationsActivitHyperlinkInnerText, uINotificationsActivitHyperlink.InnerText, "No link for Notifications & Activity tab");
        }
        
        /// <summary>
        /// Open Browser and Enter Creds
        /// </summary>
        public void OpenBrowserEnterCreds()
        {
            #region Variable Declarations
            WinEdit uIAddressandsearchusinEdit = this.UINewtabInternetExplorWindow.UIAddressBarClient.UIAddressandsearchusinEdit;
            WinEdit uIItemEdit = this.UINewtabInternetExplorWindow.UIItemWindow.UIItemEdit;
            BrowserWindow uINewtabInternetExplorWindow = this.UINewtabInternetExplorWindow;
            HtmlEdit uIUseraccountEdit = this.UINewtabInternetExplorWindow.UISignintoAzureActiveDDocument.UIUseraccountEdit;
            HtmlEdit uIPasswordEdit = this.UINewtabInternetExplorWindow.UISignintoAzureActiveDDocument.UIPasswordEdit;
            #endregion

            // Go to web page 'about:Tabs' using new browser instance
            this.UINewtabInternetExplorWindow.LaunchUrl(new System.Uri(this.OpenBrowserEnterCredsParams.UINewtabInternetExplorWindowUrl));

            // Click 'Address and search using Google' text box
            Mouse.Click(uIAddressandsearchusinEdit, new Point(79, 0));

            // Type 'https://eca-kmt-qa.azurewebsites.net/' in text box
            Keyboard.SendKeys(uIItemEdit, this.OpenBrowserEnterCredsParams.UIItemEditSendKeys, ModifierKeys.None);

            // Go to web page 'https://eca-kmt-qa.azurewebsites.net/'
            uINewtabInternetExplorWindow.NavigateToUrl(new System.Uri(this.OpenBrowserEnterCredsParams.UINewtabInternetExplorWindowUrl1));

            // Type 'ECATest1@statedept.us' in 'User account' text box
            uIUseraccountEdit.Text = this.OpenBrowserEnterCredsParams.UIUseraccountEditText;

            // Type '********' in 'Password' text box
            uIPasswordEdit.Password = this.OpenBrowserEnterCredsParams.UIPasswordEditPassword;
        }
        
        /// <summary>
        /// Click News Link
        /// </summary>
        public void SelectNewsLink()
        {
            #region Variable Declarations
            HtmlHyperlink uINews3Hyperlink = this.UINewtabInternetExplorWindow.UIHttpsecakmtqaazureweDocument.UITopPane.UINews3Hyperlink;
            #endregion

            // Click 'News (3)' link
            Mouse.Click(uINews3Hyperlink, new Point(28, 3));
        }
        
        /// <summary>
        /// Click Notifications & Activitiy Link
        /// </summary>
        public void SelectNotifications_ActivityLink()
        {
            #region Variable Declarations
            HtmlHyperlink uINotificationsActivitHyperlink = this.UINewtabInternetExplorWindow.UIHttpsecakmtqaazureweDocument.UITopPane.UINotificationsActivitHyperlink;
            #endregion

            // Click 'Notifications & Activity' link
            Mouse.Click(uINotificationsActivitHyperlink, new Point(97, 4));
        }
        
        /// <summary>
        /// Click Your Shortcuts link
        /// </summary>
        public void SelectYourShortcutsLink()
        {
            #region Variable Declarations
            HtmlHyperlink uIYourShortcutsHyperlink = this.UINewtabInternetExplorWindow.UIHttpsecakmtqaazureweDocument.UITopPane.UIYourShortcutsHyperlink;
            #endregion

            // Click 'Your Shortcuts' link
            Mouse.Click(uIYourShortcutsHyperlink, new Point(66, 1));
        }
        
        /// <summary>
        /// Click the sign in button to login with creds
        /// </summary>
        public void SignIn()
        {
            #region Variable Declarations
            HtmlSpan uISigninPane = this.UINewtabInternetExplorWindow.UISignintoAzureActiveDDocument.UISigninPane;
            #endregion

            // Double-Click 'Sign in' pane
            Mouse.DoubleClick(uISigninPane, new Point(33, 14));
        }
        
        /// <summary>
        /// Check for Sign In button
        /// </summary>
        public void SignInButton()
        {
            #region Variable Declarations
            HtmlSpan uISigninPane = this.UINewtabInternetExplorWindow.UISignintoAzureActiveDDocument.UISigninPane;
            #endregion

            // Verify that the 'InnerText' property of 'Sign in' pane equals 'Sign in'
            Assert.AreEqual(this.SignInButtonExpectedValues.UISigninPaneInnerText, uISigninPane.InnerText, "No Sign In button");
        }
        
        /// <summary>
        /// Check for Your Shortcuts Link
        /// </summary>
        public void YourShortcutsLink()
        {
            #region Variable Declarations
            HtmlHyperlink uIYourShortcutsHyperlink = this.UINewtabInternetExplorWindow.UIHttpsecakmtqaazureweDocument.UITopPane.UIYourShortcutsHyperlink;
            #endregion

            // Verify that the 'InnerText' property of 'Your Shortcuts' link equals 'Your Shortcuts'
            Assert.AreEqual(this.YourShortcutsLinkExpectedValues.UIYourShortcutsHyperlinkInnerText, uIYourShortcutsHyperlink.InnerText, "No Your Shortcuts Link");
        }
        
        #region Properties
        public virtual CloseBrowserButtonExpectedValues CloseBrowserButtonExpectedValues
        {
            get
            {
                if ((this.mCloseBrowserButtonExpectedValues == null))
                {
                    this.mCloseBrowserButtonExpectedValues = new CloseBrowserButtonExpectedValues();
                }
                return this.mCloseBrowserButtonExpectedValues;
            }
        }
        
        public virtual NewsLinkExpectedValues NewsLinkExpectedValues
        {
            get
            {
                if ((this.mNewsLinkExpectedValues == null))
                {
                    this.mNewsLinkExpectedValues = new NewsLinkExpectedValues();
                }
                return this.mNewsLinkExpectedValues;
            }
        }
        
        public virtual Notifications_ActivityLinkExpectedValues Notifications_ActivityLinkExpectedValues
        {
            get
            {
                if ((this.mNotifications_ActivityLinkExpectedValues == null))
                {
                    this.mNotifications_ActivityLinkExpectedValues = new Notifications_ActivityLinkExpectedValues();
                }
                return this.mNotifications_ActivityLinkExpectedValues;
            }
        }
        
        public virtual OpenBrowserEnterCredsParams OpenBrowserEnterCredsParams
        {
            get
            {
                if ((this.mOpenBrowserEnterCredsParams == null))
                {
                    this.mOpenBrowserEnterCredsParams = new OpenBrowserEnterCredsParams();
                }
                return this.mOpenBrowserEnterCredsParams;
            }
        }
        
        public virtual SignInButtonExpectedValues SignInButtonExpectedValues
        {
            get
            {
                if ((this.mSignInButtonExpectedValues == null))
                {
                    this.mSignInButtonExpectedValues = new SignInButtonExpectedValues();
                }
                return this.mSignInButtonExpectedValues;
            }
        }
        
        public virtual YourShortcutsLinkExpectedValues YourShortcutsLinkExpectedValues
        {
            get
            {
                if ((this.mYourShortcutsLinkExpectedValues == null))
                {
                    this.mYourShortcutsLinkExpectedValues = new YourShortcutsLinkExpectedValues();
                }
                return this.mYourShortcutsLinkExpectedValues;
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
        private CloseBrowserButtonExpectedValues mCloseBrowserButtonExpectedValues;
        
        private NewsLinkExpectedValues mNewsLinkExpectedValues;
        
        private Notifications_ActivityLinkExpectedValues mNotifications_ActivityLinkExpectedValues;
        
        private OpenBrowserEnterCredsParams mOpenBrowserEnterCredsParams;
        
        private SignInButtonExpectedValues mSignInButtonExpectedValues;
        
        private YourShortcutsLinkExpectedValues mYourShortcutsLinkExpectedValues;
        
        private UINewtabInternetExplorWindow mUINewtabInternetExplorWindow;
        #endregion
    }
    
    /// <summary>
    /// Parameters to be passed into 'CloseBrowserButton'
    /// </summary>
    [GeneratedCode("Coded UITest Builder", "12.0.31101.0")]
    public class CloseBrowserButtonExpectedValues
    {
        
        #region Fields
        /// <summary>
        /// Verify that the 'ControlType' property of 'Close' button equals 'Button'
        /// </summary>
        public string UICloseButtonControlType = "Button";
        #endregion
    }
    
    /// <summary>
    /// Parameters to be passed into 'NewsLink'
    /// </summary>
    [GeneratedCode("Coded UITest Builder", "12.0.31101.0")]
    public class NewsLinkExpectedValues
    {
        
        #region Fields
        /// <summary>
        /// Verify that the 'InnerText' property of 'News (3)' link equals 'News (3)'
        /// </summary>
        public string UINews3HyperlinkInnerText = "News (3)";
        #endregion
    }
    
    /// <summary>
    /// Parameters to be passed into 'Notifications_ActivityLink'
    /// </summary>
    [GeneratedCode("Coded UITest Builder", "12.0.31101.0")]
    public class Notifications_ActivityLinkExpectedValues
    {
        
        #region Fields
        /// <summary>
        /// Verify that the 'InnerText' property of 'Notifications & Activity' link equals 'Notifications & Activity'
        /// </summary>
        public string UINotificationsActivitHyperlinkInnerText = "Notifications & Activity";
        #endregion
    }
    
    /// <summary>
    /// Parameters to be passed into 'OpenBrowserEnterCreds'
    /// </summary>
    [GeneratedCode("Coded UITest Builder", "12.0.31101.0")]
    public class OpenBrowserEnterCredsParams
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
        /// Go to web page 'https://eca-kmt-qa.azurewebsites.net/'
        /// </summary>
        public string UINewtabInternetExplorWindowUrl1 = "https://eca-kmt-qa.azurewebsites.net/";
        
        /// <summary>
        /// Type 'ECATest1@statedept.us' in 'User account' text box
        /// </summary>
        public string UIUseraccountEditText = "ECATest1@statedept.us";
        
        /// <summary>
        /// Type '********' in 'Password' text box
        /// </summary>
        public string UIPasswordEditPassword = "pnl8gvcmh7nq2IDxDyIPucvLUfiP5WCkCWyYApPZam4=";
        #endregion
    }
    
    /// <summary>
    /// Parameters to be passed into 'SignInButton'
    /// </summary>
    [GeneratedCode("Coded UITest Builder", "12.0.31101.0")]
    public class SignInButtonExpectedValues
    {
        
        #region Fields
        /// <summary>
        /// Verify that the 'InnerText' property of 'Sign in' pane equals 'Sign in'
        /// </summary>
        public string UISigninPaneInnerText = "Sign in";
        #endregion
    }
    
    /// <summary>
    /// Parameters to be passed into 'YourShortcutsLink'
    /// </summary>
    [GeneratedCode("Coded UITest Builder", "12.0.31101.0")]
    public class YourShortcutsLinkExpectedValues
    {
        
        #region Fields
        /// <summary>
        /// Verify that the 'InnerText' property of 'Your Shortcuts' link equals 'Your Shortcuts'
        /// </summary>
        public string UIYourShortcutsHyperlinkInnerText = "Your Shortcuts";
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
            this.WindowTitles.Add("Sign in to Azure Active Directory");
            this.WindowTitles.Add("https://eca-kmt-qa.azurewebsites.net/");
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
        
        public UIItemWindow UIItemWindow
        {
            get
            {
                if ((this.mUIItemWindow == null))
                {
                    this.mUIItemWindow = new UIItemWindow(this);
                }
                return this.mUIItemWindow;
            }
        }
        
        public UIItemWindow1 UIItemWindow1
        {
            get
            {
                if ((this.mUIItemWindow1 == null))
                {
                    this.mUIItemWindow1 = new UIItemWindow1(this);
                }
                return this.mUIItemWindow1;
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
        
        public UIHttpsecakmtqaazureweDocument UIHttpsecakmtqaazureweDocument
        {
            get
            {
                if ((this.mUIHttpsecakmtqaazureweDocument == null))
                {
                    this.mUIHttpsecakmtqaazureweDocument = new UIHttpsecakmtqaazureweDocument(this);
                }
                return this.mUIHttpsecakmtqaazureweDocument;
            }
        }
        
        public UIHttpsecakmtqaazureweTitleBar UIHttpsecakmtqaazureweTitleBar
        {
            get
            {
                if ((this.mUIHttpsecakmtqaazureweTitleBar == null))
                {
                    this.mUIHttpsecakmtqaazureweTitleBar = new UIHttpsecakmtqaazureweTitleBar(this);
                }
                return this.mUIHttpsecakmtqaazureweTitleBar;
            }
        }
        #endregion
        
        #region Fields
        private UIAddressBarClient mUIAddressBarClient;
        
        private UIItemWindow mUIItemWindow;
        
        private UIItemWindow1 mUIItemWindow1;
        
        private UISignintoAzureActiveDDocument mUISignintoAzureActiveDDocument;
        
        private UIHttpsecakmtqaazureweDocument mUIHttpsecakmtqaazureweDocument;
        
        private UIHttpsecakmtqaazureweTitleBar mUIHttpsecakmtqaazureweTitleBar;
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
    public class UIItemWindow : WinWindow
    {
        
        public UIItemWindow(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Search Criteria
            this.SearchProperties[WinWindow.PropertyNames.AccessibleName] = "Address and search using Google";
            this.SearchProperties[WinWindow.PropertyNames.ClassName] = "Edit";
            this.WindowTitles.Add("New tab");
            #endregion
        }
        
        #region Properties
        public WinEdit UIItemEdit
        {
            get
            {
                if ((this.mUIItemEdit == null))
                {
                    this.mUIItemEdit = new WinEdit(this);
                    #region Search Criteria
                    this.mUIItemEdit.WindowTitles.Add("New tab");
                    #endregion
                }
                return this.mUIItemEdit;
            }
        }
        #endregion
        
        #region Fields
        private WinEdit mUIItemEdit;
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "12.0.31101.0")]
    public class UIItemWindow1 : WinWindow
    {
        
        public UIItemWindow1(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Search Criteria
            this.SearchProperties[WinWindow.PropertyNames.ClassName] = "DirectUIHWND";
            this.WindowTitles.Add("New tab");
            #endregion
        }
        
        #region Properties
        public WinClient UINewtabInternetExplorClient
        {
            get
            {
                if ((this.mUINewtabInternetExplorClient == null))
                {
                    this.mUINewtabInternetExplorClient = new WinClient(this);
                    #region Search Criteria
                    this.mUINewtabInternetExplorClient.WindowTitles.Add("New tab");
                    #endregion
                }
                return this.mUINewtabInternetExplorClient;
            }
        }
        #endregion
        
        #region Fields
        private WinClient mUINewtabInternetExplorClient;
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
            this.FilterProperties[HtmlDocument.PropertyNames.PageUrl] = @"https://login.microsoftonline.com/login.srf?wa=wsignin1.0&wtrealm=https%3a%2f%2flogin.windows.net&wreply=https%3a%2f%2flogin.windows.net%2fstatedept.us%2fwsfederation&wctx=3wEBD09BdXRoMkF1dGhvcml6ZQEPT0F1dGgyQXV0aG9yaXplAQxzdGF0ZWRlcHQudXMBAQEVb3BlbmlkY29ubmVjdC5pZHRva2VuAAABASRlMDM1NmU1NS1lMTI0LTQ1MmMtODM3ZC1hZWI3NTA0MTg1ZmYBJWh0dHBzOi8vZWNhLWttdC1xYS5henVyZXdlYnNpdGVzLm5ldC8AAAAAAAAAAAAAAQEaAAAAASVodHRwczovL2VjYS1rbXQtcWEuYXp1cmV3ZWJzaXRlcy5uZXQvAQkBCGFkYWxfdmVyAQUwLjAuNwEDY2lkASQ2NzY1ZGY4ZC0zOTQzLTQ4MDItYjRjMy04ZDFkNzFjZWMxNjcBCWVycm9yX3VyaQElaHR0cHM6Ly9lY2Eta210LXFhLmF6dXJld2Vic2l0ZXMubmV0LwEPaW5jbHVkZV9hdF9oYXNoAQExAQtpbnRlcmFjdGl2ZQEBMQEFTm9uY2UBJDhlNzI3NjRmLWQyZjYtNDdlNC1hOGI0LWJhYWVmY2M5ZmYzMwECcmMBJGZkZmNmYmU5LWZhOTctNDlhYy05ZmJmLTg5Nzg0ZTRkMDkxNQEJc2Vzc2lvbklkASRiNWYyNzg3Ny0xMzhmLTQxOGItOTJmZC0yNjhlMzE5MGE4ZjMBD2xpbWl0X3Rva2Vuc2l6ZQEBMe01&wp=MBI_FED_SSL&id=";
            this.WindowTitles.Add("Sign in to Azure Active Directory");
            #endregion
        }
        
        #region Properties
        public HtmlDiv UIBackground_page_overPane
        {
            get
            {
                if ((this.mUIBackground_page_overPane == null))
                {
                    this.mUIBackground_page_overPane = new HtmlDiv(this);
                    #region Search Criteria
                    this.mUIBackground_page_overPane.SearchProperties[HtmlDiv.PropertyNames.Id] = "background_page_overlay";
                    this.mUIBackground_page_overPane.SearchProperties[HtmlDiv.PropertyNames.Name] = null;
                    this.mUIBackground_page_overPane.FilterProperties[HtmlDiv.PropertyNames.InnerText] = null;
                    this.mUIBackground_page_overPane.FilterProperties[HtmlDiv.PropertyNames.Title] = null;
                    this.mUIBackground_page_overPane.FilterProperties[HtmlDiv.PropertyNames.Class] = "overlay ie_legacy";
                    this.mUIBackground_page_overPane.FilterProperties[HtmlDiv.PropertyNames.ControlDefinition] = "class=\"overlay ie_legacy\" id=\"background_page_overlay\" style=\"visibility: visible" +
                        "; opacity: 0.6; background-color: rgb(0, 114, 198);\"";
                    this.mUIBackground_page_overPane.FilterProperties[HtmlDiv.PropertyNames.TagInstance] = "4";
                    this.mUIBackground_page_overPane.WindowTitles.Add("Sign in to Azure Active Directory");
                    #endregion
                }
                return this.mUIBackground_page_overPane;
            }
        }
        
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
        private HtmlDiv mUIBackground_page_overPane;
        
        private HtmlEdit mUIUseraccountEdit;
        
        private HtmlEdit mUIPasswordEdit;
        
        private HtmlSpan mUISigninPane;
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "12.0.31101.0")]
    public class UIHttpsecakmtqaazureweDocument : HtmlDocument
    {
        
        public UIHttpsecakmtqaazureweDocument(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Search Criteria
            this.SearchProperties[HtmlDocument.PropertyNames.Id] = null;
            this.SearchProperties[HtmlDocument.PropertyNames.RedirectingPage] = "False";
            this.SearchProperties[HtmlDocument.PropertyNames.FrameDocument] = "False";
            this.FilterProperties[HtmlDocument.PropertyNames.Title] = null;
            this.FilterProperties[HtmlDocument.PropertyNames.AbsolutePath] = "/";
            this.FilterProperties[HtmlDocument.PropertyNames.PageUrl] = "https://eca-kmt-qa.azurewebsites.net/#/#top";
            this.WindowTitles.Add("https://eca-kmt-qa.azurewebsites.net/");
            #endregion
        }
        
        #region Properties
        public UITopPane UITopPane
        {
            get
            {
                if ((this.mUITopPane == null))
                {
                    this.mUITopPane = new UITopPane(this);
                }
                return this.mUITopPane;
            }
        }
        #endregion
        
        #region Fields
        private UITopPane mUITopPane;
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "12.0.31101.0")]
    public class UITopPane : HtmlDiv
    {
        
        public UITopPane(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Search Criteria
            this.SearchProperties[HtmlDiv.PropertyNames.Id] = "top";
            this.SearchProperties[HtmlDiv.PropertyNames.Name] = null;
            this.FilterProperties[HtmlDiv.PropertyNames.InnerText] = "ECA Knowledge Management Tool\r\n\r\n\r\n Your";
            this.FilterProperties[HtmlDiv.PropertyNames.Title] = null;
            this.FilterProperties[HtmlDiv.PropertyNames.Class] = "ng-scope";
            this.FilterProperties[HtmlDiv.PropertyNames.ControlDefinition] = "class=\"ng-scope\" id=\"top\" ng-click=\"closeMenus()\" autoscroll=\"true\" ui-view=\"\"";
            this.FilterProperties[HtmlDiv.PropertyNames.TagInstance] = "24";
            this.WindowTitles.Add("https://eca-kmt-qa.azurewebsites.net/");
            #endregion
        }
        
        #region Properties
        public HtmlHyperlink UINotificationsActivitHyperlink
        {
            get
            {
                if ((this.mUINotificationsActivitHyperlink == null))
                {
                    this.mUINotificationsActivitHyperlink = new HtmlHyperlink(this);
                    #region Search Criteria
                    this.mUINotificationsActivitHyperlink.SearchProperties[HtmlHyperlink.PropertyNames.InnerText] = "Notifications & Activity";
                    this.mUINotificationsActivitHyperlink.FilterProperties[HtmlHyperlink.PropertyNames.Href] = "#/";
                    this.mUINotificationsActivitHyperlink.FilterProperties[HtmlHyperlink.PropertyNames.Class] = "ng-binding";
                    this.mUINotificationsActivitHyperlink.FilterProperties[HtmlHyperlink.PropertyNames.ControlDefinition] = "class=\"ng-binding\" href=\"#/\" ui-sref=\".n";
                    this.mUINotificationsActivitHyperlink.FilterProperties[HtmlHyperlink.PropertyNames.TagInstance] = "2";
                    this.mUINotificationsActivitHyperlink.WindowTitles.Add("https://eca-kmt-qa.azurewebsites.net//#/#top");
                    #endregion
                }
                return this.mUINotificationsActivitHyperlink;
            }
        }
        
        public HtmlHyperlink UINews3Hyperlink
        {
            get
            {
                if ((this.mUINews3Hyperlink == null))
                {
                    this.mUINews3Hyperlink = new HtmlHyperlink(this);
                    #region Search Criteria
                    this.mUINews3Hyperlink.SearchProperties[HtmlHyperlink.PropertyNames.Id] = null;
                    this.mUINews3Hyperlink.SearchProperties[HtmlHyperlink.PropertyNames.Name] = null;
                    this.mUINews3Hyperlink.SearchProperties[HtmlHyperlink.PropertyNames.Target] = null;
                    this.mUINews3Hyperlink.SearchProperties[HtmlHyperlink.PropertyNames.InnerText] = "News (3)";
                    this.mUINews3Hyperlink.FilterProperties[HtmlHyperlink.PropertyNames.AbsolutePath] = null;
                    this.mUINews3Hyperlink.FilterProperties[HtmlHyperlink.PropertyNames.Title] = null;
                    this.mUINews3Hyperlink.FilterProperties[HtmlHyperlink.PropertyNames.Href] = "#/";
                    this.mUINews3Hyperlink.FilterProperties[HtmlHyperlink.PropertyNames.Class] = "ng-binding";
                    this.mUINews3Hyperlink.FilterProperties[HtmlHyperlink.PropertyNames.ControlDefinition] = "class=\"ng-binding\" href=\"#/\" ui-sref=\".n";
                    this.mUINews3Hyperlink.FilterProperties[HtmlHyperlink.PropertyNames.TagInstance] = "3";
                    this.mUINews3Hyperlink.WindowTitles.Add("https://eca-kmt-qa.azurewebsites.net/");
                    #endregion
                }
                return this.mUINews3Hyperlink;
            }
        }
        
        public HtmlHyperlink UIYourShortcutsHyperlink
        {
            get
            {
                if ((this.mUIYourShortcutsHyperlink == null))
                {
                    this.mUIYourShortcutsHyperlink = new HtmlHyperlink(this);
                    #region Search Criteria
                    this.mUIYourShortcutsHyperlink.SearchProperties[HtmlHyperlink.PropertyNames.Id] = null;
                    this.mUIYourShortcutsHyperlink.SearchProperties[HtmlHyperlink.PropertyNames.Name] = null;
                    this.mUIYourShortcutsHyperlink.SearchProperties[HtmlHyperlink.PropertyNames.Target] = null;
                    this.mUIYourShortcutsHyperlink.SearchProperties[HtmlHyperlink.PropertyNames.InnerText] = "Your Shortcuts";
                    this.mUIYourShortcutsHyperlink.FilterProperties[HtmlHyperlink.PropertyNames.AbsolutePath] = null;
                    this.mUIYourShortcutsHyperlink.FilterProperties[HtmlHyperlink.PropertyNames.Title] = null;
                    this.mUIYourShortcutsHyperlink.FilterProperties[HtmlHyperlink.PropertyNames.Href] = "#/";
                    this.mUIYourShortcutsHyperlink.FilterProperties[HtmlHyperlink.PropertyNames.Class] = "ng-binding";
                    this.mUIYourShortcutsHyperlink.FilterProperties[HtmlHyperlink.PropertyNames.ControlDefinition] = "class=\"ng-binding\" href=\"#/\" ui-sref=\".s";
                    this.mUIYourShortcutsHyperlink.FilterProperties[HtmlHyperlink.PropertyNames.TagInstance] = "1";
                    this.mUIYourShortcutsHyperlink.WindowTitles.Add("https://eca-kmt-qa.azurewebsites.net/");
                    #endregion
                }
                return this.mUIYourShortcutsHyperlink;
            }
        }
        #endregion
        
        #region Fields
        private HtmlHyperlink mUINotificationsActivitHyperlink;
        
        private HtmlHyperlink mUINews3Hyperlink;
        
        private HtmlHyperlink mUIYourShortcutsHyperlink;
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "12.0.31101.0")]
    public class UIHttpsecakmtqaazureweTitleBar : WinTitleBar
    {
        
        public UIHttpsecakmtqaazureweTitleBar(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Search Criteria
            this.WindowTitles.Add("https://eca-kmt-qa.azurewebsites.net/");
            #endregion
        }
        
        #region Properties
        public WinButton UICloseButton
        {
            get
            {
                if ((this.mUICloseButton == null))
                {
                    this.mUICloseButton = new WinButton(this);
                    #region Search Criteria
                    this.mUICloseButton.SearchProperties[WinButton.PropertyNames.Name] = "Close";
                    this.mUICloseButton.WindowTitles.Add("https://eca-kmt-qa.azurewebsites.net/");
                    #endregion
                }
                return this.mUICloseButton;
            }
        }
        #endregion
        
        #region Fields
        private WinButton mUICloseButton;
        #endregion
    }
}