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


namespace CreateParticipant.Test
{
    /// <summary>
    /// Summary description for CodedUITest1
    /// </summary>
    [CodedUITest]
    public class CreateParticipantCodedUITest1
    {
        public CreateParticipantCodedUITest1()
        {
        }

        [TestMethod]
        public void CreateParticpantCodedUITestMethod1()
        {
            this.UIMap.LogintoQA_ExistingUser();
            this.UIMap.AssertContentMenuButton();
            this.UIMap.SelectContentMenuButton_ProgramsLink();
            this.UIMap.AssertIndividualProgram();
            this.UIMap.SelectIndividualProgram();
            this.UIMap.RefreshBrowserWindow();
            this.UIMap.AssertBranches_ProjectsTab();
            this.UIMap.SelectBranches_ProjectsTab();
            this.UIMap.AssertIndividualProject();
            this.UIMap.SelectIndividualProject();
            this.UIMap.RefreshBrowserWindow();
            this.UIMap.SelectIndividualProject();
            this.UIMap.RefreshBrowserWindow();
            this.UIMap.AssertParticipantsTab_IndProject();
            this.UIMap.SelectParticipantsTab_IndProject();
            this.UIMap.AssertParticipantList();
            this.UIMap.SelectParticipantList();
            this.UIMap.AssertParticipantAddButton();
            this.UIMap.SelectParticipantADD();
            this.UIMap.AssertADDParticipantModal_PersonalInformation();

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
