using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UITesting.HtmlControls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UITest.Core
{
    public class ContentMenu
    {
        public static void AccessMenu(BrowserWindow browserWindow) 

        {
            // Find and Open / Expand Content Menu with click
            HtmlButton toggleNav = new HtmlButton(browserWindow);
            toggleNav.SearchProperties.Add(HtmlButton.PropertyNames.TagName, "BUTTON", HtmlButton.PropertyNames.InnerText, "Toggle navigation");
            toggleNav.WaitForControlReady();
            Mouse.Click(toggleNav);

            // Verify sections available for selection (Offices, Programs, People, Organizations, Reports- currently functional)

            //offices
            HtmlHyperlink offices = new HtmlHyperlink(browserWindow);
            offices.SearchProperties.Add(HtmlHyperlink.PropertyNames.InnerText, "Offices", HtmlHyperlink.PropertyNames.ControlType, "Hyperlink", HtmlHyperlink.PropertyNames.TagInstance, "2");
            offices.WaitForControlReady();
            Assert.AreEqual(true, offices.Exists);
            
            //programs
            HtmlHyperlink programs = new HtmlHyperlink(browserWindow);
            programs.SearchProperties.Add(HtmlHyperlink.PropertyNames.InnerText, "Programs", HtmlHyperlink.PropertyNames.ControlType, "Hyperlink", HtmlHyperlink.PropertyNames.TagInstance, "3");
            programs.WaitForControlReady();
            Assert.AreEqual(true, programs.Exists);

            //people
            HtmlHyperlink people = new HtmlHyperlink(browserWindow);
            people.SearchProperties.Add(HtmlHyperlink.PropertyNames.InnerText, "People", HtmlHyperlink.PropertyNames.ControlType, "Hyperlink", HtmlHyperlink.PropertyNames.TagInstance, "4");
            people.WaitForControlReady();
            Assert.AreEqual(true, people.Exists);

            //orgs
            HtmlHyperlink organizations = new HtmlHyperlink(browserWindow);
            organizations.SearchProperties.Add(HtmlHyperlink.PropertyNames.InnerText, "Organizations", HtmlHyperlink.PropertyNames.ControlType, "Hyperlink", HtmlHyperlink.PropertyNames.TagInstance, "5");
            organizations.WaitForControlReady();
            Assert.AreEqual(true, organizations.Exists);

            //reports
            HtmlHyperlink reports = new HtmlHyperlink(browserWindow);
            reports.SearchProperties.Add(HtmlHyperlink.PropertyNames.InnerText, "Reports", HtmlHyperlink.PropertyNames.ControlType, "Hyperlink", HtmlHyperlink.PropertyNames.TagInstance, "7");
            reports.WaitForControlReady();
            Assert.AreEqual(true, reports.Exists);

            //collapse content menu
            Mouse.Click(toggleNav);

        }



    }
}
