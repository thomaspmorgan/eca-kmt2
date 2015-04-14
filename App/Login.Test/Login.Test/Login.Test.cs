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


namespace Login.Test
{
    /// <summary>
    /// Simple login to the QA site with the ECAState creds
    /// </summary>
    [CodedUITest]
    public class LoginCodedUITest1
    {
        public LoginCodedUITest1()
        {
        }

        [TestMethod]
        public void LoginCodedUITestMethod1()
        {
            // loginQA will take you to the homepage of QA. Add more depth after UIMap.LoginQA
            this.UIMap.LogintoQA_ExistingUser();
            //this.UIMap.LoginQA();

        }

        #region Additional test attributes

        // You can use the following additional attributes as you write your tests:

        ////Use TestInitialize to run code before running each test 
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{        
        //    // Add here
        //}

        ////Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{        
        //    // Add here
        //}

        #endregion

        /// <summary>
        ///
        ///Verify the login to QA
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
