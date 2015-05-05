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


namespace CreateProgram.Test
{
    /// <summary>
    /// Create Program -under construction- redo select from CM method(selectprogrammenuitem x2 for reloading page- works)
    /// --fails on InputParentProgramData_First
    /// </summary>
    [CodedUITest]
    public class CreateProgram
    {
        public CreateProgram()
        {
        }

        [TestMethod]
        public void CreateProgramCodedUITestMethod1()
        {

            this.UIMap.LogintoQA_ExistingUser();
            //this.UIMap.LogintoQA();
            this.UIMap.AssertContentMenuButton();
            this.UIMap.SelectContentMenuButton();
            this.UIMap.AssertProgramsMenuItemLink();
            this.UIMap.SelectProgramsMenuItem();
            this.UIMap.RefreshAllProgramsPage();
            this.UIMap.SelectContentMenuButton();
            this.UIMap.SelectProgramsMenuItem();
            this.UIMap.AssertCreateProgramButton();
            this.UIMap.SelectCreateProgramButton();
            this.UIMap.AssertCreateProgramModal();
            this.UIMap.AssertEditTextFields_Modal();
            this.UIMap.InputParentProgramData_First();
            this.UIMap.AssertParentProgramInputValues_Modal();
            this.UIMap.AssertCreateButton_Modal();
            this.UIMap.SelectCreateButton_Modal();
            this.UIMap.AssertProgramCreatedWindow();
            this.UIMap.SelectOKButton_ProgramCreated();

            

            


            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
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
