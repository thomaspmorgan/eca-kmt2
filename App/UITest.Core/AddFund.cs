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
    public class OutgoingFund
    {
        public static void AddFundOut(BrowserWindow browserWindow)
        {
            //add funding item prop
            HtmlButton addNewFund = new HtmlButton(browserWindow);
            addNewFund.SearchProperties.Add(HtmlButton.PropertyNames.TagName, "BUTTON", HtmlButton.PropertyNames.InnerText, "ADD FUNDING ITEM", HtmlButton.PropertyNames.TagInstance, "3");
            addNewFund.WaitForControlReady();
            Assert.AreEqual(true, addNewFund.Exists);

            //select add funding item
            Mouse.Click(addNewFund);

            //outgoing tab prop (modal)
            HtmlSpan Outgoing = new HtmlSpan(browserWindow);
            Outgoing.SearchProperties.Add(HtmlSpan.PropertyNames.TagName, "SPAN", HtmlSpan.PropertyNames.InnerText, "Outgoing", HtmlSpan.PropertyNames.TagInstance, "31");
            Outgoing.WaitForControlReady();
            Assert.AreEqual(true, Outgoing.Exists);

            //select outgoing tab
            Mouse.Click(Outgoing);

            // office outgoing modal props
            //

            //Status***

            //status label
            HtmlLabel Status = new HtmlLabel(browserWindow);
            Status.SearchProperties.Add(HtmlLabel.PropertyNames.InnerText, "Status *", HtmlLabel.PropertyNames.TagName, "LABEL", HtmlLabel.PropertyNames.TagInstance, "15");
            Status.WaitForControlReady();
            Assert.AreEqual(true, Status.Exists);

            //status field
            HtmlComboBox StatusField = new HtmlComboBox(browserWindow);
            StatusField.SearchProperties.Add(HtmlComboBox.PropertyNames.TagName, "SELECT", HtmlComboBox.PropertyNames.Id, "moneyFlowStatus", HtmlComboBox.PropertyNames.InnerText, "ActualAppropriatedEstimated", HtmlComboBox.PropertyNames.TagInstance, "3");
            StatusField.WaitForControlReady();
            Assert.AreEqual(true, StatusField.Exists);

            //Transaction date***

            //transaction date label
            HtmlLabel TransDTE = new HtmlLabel(browserWindow);
            TransDTE.SearchProperties.Add(HtmlLabel.PropertyNames.TagName, "LABEL", HtmlLabel.PropertyNames.InnerText, "Transaction Date *", HtmlLabel.PropertyNames.TagInstance, "16");
            TransDTE.WaitForControlReady();
            Assert.AreEqual(true, TransDTE.Exists);

            //transaction date field
            HtmlEdit TransDTEField = new HtmlEdit(browserWindow);
            TransDTEField.SearchProperties.Add(HtmlEdit.PropertyNames.TagName, "INPUT", HtmlEdit.PropertyNames.Name, "transactionDate", HtmlEdit.PropertyNames.TagInstance, "22");
            TransDTEField.WaitForControlReady();
            Assert.AreEqual(true, TransDTEField.Exists);

            //transaction date picker calendar
            HtmlButton TransDTECal = new HtmlButton(browserWindow);
            TransDTECal.SearchProperties.Add(HtmlButton.PropertyNames.TagName, "BUTTON", HtmlButton.PropertyNames.InnerText, "event", HtmlButton.PropertyNames.TagInstance, "13");
            TransDTECal.WaitForControlReady();
            Assert.AreEqual(true, TransDTECal.Exists);

            //Reference Fiscal Year***

            //reference fiscal year label
            HtmlLabel RefFY = new HtmlLabel(browserWindow);
            RefFY.SearchProperties.Add(HtmlLabel.PropertyNames.InnerText, "Reference Fiscal Year *", HtmlLabel.PropertyNames.TagName, "LABEL", HtmlLabel.PropertyNames.TagInstance, "17");
            RefFY.WaitForControlReady();
            Assert.AreEqual(true, RefFY.Exists);

            //reference fiscal year field
            HtmlEdit RefFYField = new HtmlEdit(browserWindow);
            RefFYField.SearchProperties.Add(HtmlEdit.PropertyNames.Id, "fiscalYear", HtmlEdit.PropertyNames.TagName, "SELECT", HtmlEdit.PropertyNames.InnerText, "2005200620072008200920102011201220132014201520162017", HtmlEdit.PropertyNames.TagInstance, "4");
            RefFYField.WaitForControlReady();
            Assert.AreEqual(true, RefFYField.Exists);

            //select funding source name label
            HtmlLabel SelectSource = new HtmlLabel(browserWindow);
            SelectSource.SearchProperties.Add(HtmlLabel.PropertyNames.InnerText,
                "Select a Funding Item that will be the Source for the new Funding Item", PropertyExpressionOperator.Contains);
            SelectSource.WaitForControlReady();

            //select source funding field
            HtmlCustom FundSource = new HtmlCustom(browserWindow);
            FundSource.SearchProperties.Add(HtmlCustom.PropertyNames.InnerText,
                   "Select a source funding item...", PropertyExpressionOperator.Contains);
            FundSource.WaitForControlReady();

            //remaining unassigned funds label
            HtmlLabel RemainingFunds = new HtmlLabel(browserWindow);
            RemainingFunds.SearchProperties.Add(HtmlLabel.PropertyNames.InnerText,
                    "Remaining Unassigned Funds From Source", PropertyExpressionOperator.Contains);
            RemainingFunds.WaitForControlReady();

            //NEED TO MATCH THE SOURCE NAME DISPLAYED TO THE ONE ABOVE WHEN SELECTED TO VERIFY

            //funding amount label
            HtmlLabel Amount = new HtmlLabel(browserWindow);
            Amount.SearchProperties.Add(HtmlLabel.PropertyNames.InnerText, "Amount *");
            Amount.WaitForControlReady();
            Assert.AreEqual(true, Amount.Exists);

            //funding amount field
            HtmlEdit AmountField = new HtmlEdit(browserWindow);
            AmountField.SearchProperties.Add(HtmlEdit.PropertyNames.TagName, "INPUT", HtmlEdit.PropertyNames.Id, "amount");
            AmountField.WaitForControlReady();
            Assert.AreEqual(true, AmountField.Exists);

            //description label
            HtmlLabel Description = new HtmlLabel(browserWindow);
            Description.SearchProperties.Add(HtmlLabel.PropertyNames.InnerText, "Description *");
            Description.WaitForControlReady();
            Assert.AreEqual(true, Description.Exists);

            //description field
            HtmlEdit DescField = new HtmlEdit(browserWindow);
            DescField.SearchProperties.Add(HtmlEdit.PropertyNames.Id, "description");
            DescField.WaitForControlReady();
            Assert.AreEqual(true, DescField.Exists);

            //save button
            HtmlButton SaveFund = new HtmlButton(browserWindow);
            SaveFund.SearchProperties.Add(HtmlButton.PropertyNames.InnerText, "Save", HtmlButton.PropertyNames.TagName, "BUTTON");
            SaveFund.WaitForControlReady();
            Assert.AreEqual(true, SaveFund.Exists);

            //cancel button
            HtmlButton CancelFund = new HtmlButton(browserWindow);
            CancelFund.SearchProperties.Add(HtmlButton.PropertyNames.InnerText, "Cancel", HtmlButton.PropertyNames.TagName, "BUTTON");
            CancelFund.WaitForControlReady();
            Assert.AreEqual(true, CancelFund.Exists);

            //END OF MODAL PROPS
        }

    }
}

namespace UITest.Core
{
    public class IncomingFund
    {
        public static void AddFundIn(BrowserWindow browserWindow)
        {
            //add funding item prop
            HtmlButton addNewFund = new HtmlButton(browserWindow);
            addNewFund.SearchProperties.Add(HtmlButton.PropertyNames.TagName, "BUTTON", HtmlButton.PropertyNames.InnerText, "ADD FUNDING ITEM", HtmlButton.PropertyNames.TagInstance, "3");
            addNewFund.WaitForControlReady();
            Assert.AreEqual(true, addNewFund.Exists);

            //select add funding item
            Mouse.Click(addNewFund);

            //outgoing tab prop (modal)
            HtmlSpan Incoming = new HtmlSpan(browserWindow);
            Incoming.SearchProperties.Add(HtmlSpan.PropertyNames.TagName, "SPAN", HtmlSpan.PropertyNames.InnerText, "Incoming", HtmlSpan.PropertyNames.TagInstance, "44");
            Incoming.WaitForControlReady();
            Assert.AreEqual(true, Incoming.Exists);

            //select outgoing tab
            Mouse.Click(Incoming);

            // office outgoing modal props
            //

            //Status***

            //status label
            HtmlLabel Status = new HtmlLabel(browserWindow);
            Status.SearchProperties.Add(HtmlLabel.PropertyNames.InnerText, "Status *", HtmlLabel.PropertyNames.TagName, "LABEL", HtmlLabel.PropertyNames.TagInstance, "15");
            Status.WaitForControlReady();
            Assert.AreEqual(true, Status.Exists);

            //status field
            HtmlComboBox StatusField = new HtmlComboBox(browserWindow);
            StatusField.SearchProperties.Add(HtmlComboBox.PropertyNames.TagName, "SELECT", HtmlComboBox.PropertyNames.Id, "moneyFlowStatus", HtmlComboBox.PropertyNames.InnerText, "ActualAppropriatedEstimated", HtmlComboBox.PropertyNames.TagInstance, "3");
            StatusField.WaitForControlReady();
            Assert.AreEqual(true, StatusField.Exists);

            //Transaction date***

            //transaction date label
            HtmlLabel TransDTE = new HtmlLabel(browserWindow);
            TransDTE.SearchProperties.Add(HtmlLabel.PropertyNames.TagName, "LABEL", HtmlLabel.PropertyNames.InnerText, "Transaction Date *", HtmlLabel.PropertyNames.TagInstance, "16");
            TransDTE.WaitForControlReady();
            Assert.AreEqual(true, TransDTE.Exists);

            //transaction date field
            HtmlEdit TransDTEField = new HtmlEdit(browserWindow);
            TransDTEField.SearchProperties.Add(HtmlEdit.PropertyNames.TagName, "INPUT", HtmlEdit.PropertyNames.Name, "transactionDate", HtmlEdit.PropertyNames.TagInstance, "22");
            TransDTEField.WaitForControlReady();
            Assert.AreEqual(true, TransDTEField.Exists);

            //transaction date picker calendar
            HtmlButton TransDTECal = new HtmlButton(browserWindow);
            TransDTECal.SearchProperties.Add(HtmlButton.PropertyNames.TagName, "BUTTON", HtmlButton.PropertyNames.InnerText, "event", HtmlButton.PropertyNames.TagInstance, "13");
            TransDTECal.WaitForControlReady();
            Assert.AreEqual(true, TransDTECal.Exists);

            //Reference Fiscal Year***

            //reference fiscal year label
            HtmlLabel RefFY = new HtmlLabel(browserWindow);
            RefFY.SearchProperties.Add(HtmlLabel.PropertyNames.InnerText, "Reference Fiscal Year *", HtmlLabel.PropertyNames.TagName, "LABEL", HtmlLabel.PropertyNames.TagInstance, "17");
            RefFY.WaitForControlReady();
            Assert.AreEqual(true, RefFY.Exists);

            //reference fiscal year field
            HtmlEdit RefFYField = new HtmlEdit(browserWindow);
            RefFYField.SearchProperties.Add(HtmlEdit.PropertyNames.Id, "fiscalYear", HtmlEdit.PropertyNames.TagName, "SELECT", HtmlEdit.PropertyNames.InnerText, "2005200620072008200920102011201220132014201520162017", HtmlEdit.PropertyNames.TagInstance, "4");
            RefFYField.WaitForControlReady();
            Assert.AreEqual(true, RefFYField.Exists);

            //select funding source name label
            HtmlLabel SelectSource = new HtmlLabel(browserWindow);
            SelectSource.SearchProperties.Add(HtmlLabel.PropertyNames.InnerText,
                        "Select a Funding Item that will be the Source for the new Funding Item", PropertyExpressionOperator.Contains);
            SelectSource.WaitForControlReady();

            //select source funding field
            HtmlCustom FundSource = new HtmlCustom(browserWindow);
            FundSource.SearchProperties.Add(HtmlCustom.PropertyNames.InnerText,
                           "Select a source funding item...", PropertyExpressionOperator.Contains);
            FundSource.WaitForControlReady();

            //remaining unassigned funds label
            HtmlLabel RemainingFunds = new HtmlLabel(browserWindow);
            RemainingFunds.SearchProperties.Add(HtmlLabel.PropertyNames.InnerText,
                            "Remaining Unassigned Funds From Source", PropertyExpressionOperator.Contains);
            RemainingFunds.WaitForControlReady();

            //NEED TO MATCH THE SOURCE NAME DISPLAYED TO THE ONE ABOVE WHEN SELECTED TO VERIFY

            //funding amount label
            HtmlLabel Amount = new HtmlLabel(browserWindow);
            Amount.SearchProperties.Add(HtmlLabel.PropertyNames.InnerText, "Amount *");
            Amount.WaitForControlReady();
            Assert.AreEqual(true, Amount.Exists);

            //funding amount field
            HtmlEdit AmountField = new HtmlEdit(browserWindow);
            AmountField.SearchProperties.Add(HtmlEdit.PropertyNames.TagName, "INPUT", HtmlEdit.PropertyNames.Id, "amount");
            AmountField.WaitForControlReady();
            Assert.AreEqual(true, AmountField.Exists);

            //description label
            HtmlLabel Description = new HtmlLabel(browserWindow);
            Description.SearchProperties.Add(HtmlLabel.PropertyNames.InnerText, "Description *");
            Description.WaitForControlReady();
            Assert.AreEqual(true, Description.Exists);

            //description field
            HtmlEdit DescField = new HtmlEdit(browserWindow);
            DescField.SearchProperties.Add(HtmlEdit.PropertyNames.Id, "description");
            DescField.WaitForControlReady();
            Assert.AreEqual(true, DescField.Exists);

            //save button
            HtmlButton SaveFund = new HtmlButton(browserWindow);
            SaveFund.SearchProperties.Add(HtmlButton.PropertyNames.InnerText, "Save", HtmlButton.PropertyNames.TagName, "BUTTON");
            SaveFund.WaitForControlReady();
            Assert.AreEqual(true, SaveFund.Exists);

            //cancel button
            HtmlButton CancelFund = new HtmlButton(browserWindow);
            CancelFund.SearchProperties.Add(HtmlButton.PropertyNames.InnerText, "Cancel", HtmlButton.PropertyNames.TagName, "BUTTON");
            CancelFund.WaitForControlReady();
            Assert.AreEqual(true, CancelFund.Exists);

            //END MODAL PROPS

        }
    }
}
