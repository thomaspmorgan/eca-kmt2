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
    public class AddOffFund
    {
        public static void AddOffFundOut(BrowserWindow browserWindow)
        {
            //add funding item prop
            HtmlButton addNewFund = new HtmlButton(browserWindow);
            addNewFund.SearchProperties.Add(HtmlButton.PropertyNames.TagName, "BUTTON", HtmlButton.PropertyNames.InnerText, "ADD FUNDING ITEM", HtmlButton.PropertyNames.TagInstance, "3");
            addNewFund.WaitForControlReady();
            Assert.AreEqual(true, addNewFund.Exists);

            //select add funding item
            Mouse.Click(addNewFund);

            //outgoing tab prop (modal)
            HtmlSpan offOutgoing = new HtmlSpan(browserWindow);
            offOutgoing.SearchProperties.Add(HtmlSpan.PropertyNames.TagName, "SPAN", HtmlSpan.PropertyNames.InnerText, "Outgoing", HtmlSpan.PropertyNames.TagInstance, "31");
            offOutgoing.WaitForControlReady();
            Assert.AreEqual(true, offOutgoing.Exists);

            //select outgoing tab
            Mouse.Click(offOutgoing);

            // office outgoing modal props
            //
            
            //Status***
            
            //status label
            HtmlLabel offStatus = new HtmlLabel(browserWindow);
            offStatus.SearchProperties.Add(HtmlLabel.PropertyNames.InnerText, "Status *", HtmlLabel.PropertyNames.TagName, "LABEL", HtmlLabel.PropertyNames.TagInstance, "15");
            offStatus.WaitForControlReady();
            Assert.AreEqual(true, offStatus.Exists);

            //status field
            HtmlComboBox offStatusField = new HtmlComboBox(browserWindow);
            offStatusField.SearchProperties.Add(HtmlComboBox.PropertyNames.TagName, "SELECT", HtmlComboBox.PropertyNames.Id, "moneyFlowStatus", HtmlComboBox.PropertyNames.InnerText, "ActualAppropriatedEstimated", HtmlComboBox.PropertyNames.TagInstance, "3");
            offStatusField.WaitForControlReady();
            Assert.AreEqual(true, offStatusField.Exists);

            //Transaction date***

            //transaction date label
            HtmlLabel offTransDTE = new HtmlLabel(browserWindow);
            offTransDTE.SearchProperties.Add(HtmlLabel.PropertyNames.TagName, "LABEL", HtmlLabel.PropertyNames.InnerText, "Transaction Date *", HtmlLabel.PropertyNames.TagInstance, "16");
            offTransDTE.WaitForControlReady();
            Assert.AreEqual(true, offTransDTE.Exists);

            //transaction date field
            HtmlEdit offTransDTEField = new HtmlEdit(browserWindow);
            offTransDTEField.SearchProperties.Add(HtmlEdit.PropertyNames.TagName, "INPUT", HtmlEdit.PropertyNames.Name, "transactionDate", HtmlEdit.PropertyNames.TagInstance, "22");
            offTransDTEField.WaitForControlReady();
            Assert.AreEqual(true, offTransDTEField.Exists);

            //transaction date picker calendar
            HtmlButton offTransDTECal = new HtmlButton(browserWindow);
            offTransDTECal.SearchProperties.Add(HtmlButton.PropertyNames.TagName, "BUTTON", HtmlButton.PropertyNames.InnerText, "event", HtmlButton.PropertyNames.TagInstance, "13");
            offTransDTECal.WaitForControlReady();
            Assert.AreEqual(true, offTransDTECal.Exists);

            //Reference Fiscal Year***

            //reference fiscal year label
            HtmlLabel offRFY = new HtmlLabel(browserWindow);
            offRFY.SearchProperties.Add(HtmlLabel.PropertyNames.InnerText, "Reference Fiscal Year *", HtmlLabel.PropertyNames.TagName, "LABEL", HtmlLabel.PropertyNames.TagInstance, "17");
            offRFY.WaitForControlReady();
            Assert.AreEqual(true, offRFY.Exists);

            //reference fiscal year field
            HtmlEdit offRFYField = new HtmlEdit(browserWindow);
            offRFYField.SearchProperties.Add(HtmlEdit.PropertyNames.Id, "fiscalYear", HtmlEdit.PropertyNames.TagName, "SELECT", HtmlEdit.PropertyNames.InnerText, "2005200620072008200920102011201220132014201520162017", HtmlEdit.PropertyNames.TagInstance, "4");
            offRFYField.WaitForControlReady();
            Assert.AreEqual(true, offRFYField.Exists);

            //







        }




    }
}
