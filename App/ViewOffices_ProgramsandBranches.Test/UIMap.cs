namespace ViewOffices_ProgramsandBranches.Test
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
        /// Verify the (Office of Academic Exchanges) control type and innertext are available.
        /// </summary>
        public void AssertOfficeName()
        {
            #region Variable Declarations
            HtmlHyperlink uIOfficeofAcademicExchHyperlink = this.UINewtabInternetExplorWindow.UIHttpsecakmtqaazureweDocument6.UISortlistPane.UIOfficeofAcademicExchHyperlink;
            #endregion

            // Wait for 10 seconds for user delay between actions; Verify that the 'ControlType' property of 'Office of Academic Exchange Programs' link contains 'Hyperlink'
            Playback.Wait(10000);
            StringAssert.Contains(uIOfficeofAcademicExchHyperlink.ControlType.ToString(), "Hyperlink", "No (Office of Academic Exchange Programs) hyperlink control type.");

            // Verify that the 'InnerText' property of 'Office of Academic Exchange Programs' link contains 'Office of Academic Exchange Programs'
            StringAssert.Contains(uIOfficeofAcademicExchHyperlink.InnerText.ToString(), "Office of Academic Exchange Programs", "No (Office of Academic Exchange Programs) InnerText available.");
        }

        //public virtual AssertOfficeNameExpectedValues AssertOfficeNameExpectedValues
        //{
        //    get
        //    {
        //        if ((this.mAssertOfficeNameExpectedValues == null))
        //        {
        //            this.mAssertOfficeNameExpectedValues = new AssertOfficeNameExpectedValues();
        //        }
        //        return this.mAssertOfficeNameExpectedValues;
        //    }
        //}

        //private AssertOfficeNameExpectedValues mAssertOfficeNameExpectedValues;
    }
//    /// <summary>
//    /// Parameters to be passed into 'AssertOfficeName'
//    /// </summary>
//    [GeneratedCode("Coded UITest Builder", "12.0.31101.0")]
//    public class AssertOfficeNameExpectedValues
//    {

//        #region Fields
//        /// <summary>
//        /// Wait for 10 seconds for user delay between actions; Verify that the 'ControlType' property of 'Office of Academic Exchange Programs' link contains 'Hyperlink'
//        /// </summary>
//        public string UIOfficeofAcademicExchHyperlinkControlType = "Hyperlink";

//        /// <summary>
//        /// Verify that the 'InnerText' property of 'Office of Academic Exchange Programs' link contains 'Office of Academic Exchange Programs'
//        /// </summary>
//        public string UIOfficeofAcademicExchHyperlinkInnerText = "Office of Academic Exchange Programs";
//        #endregion
//}
}
