using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model.Shared
{
    [Validator(typeof(EVFinancialInfoValidator))]
    public class EVFinancialInfo
    {
        public EVFinancialInfo()
        {
            otherFunds = new OtherFunds();
        }

        public bool receivedUSGovtFunds { get; set; }

        public string ProgramSponsorFunds { get; set; }

        public OtherFunds otherFunds { get; set; }
    }
}
