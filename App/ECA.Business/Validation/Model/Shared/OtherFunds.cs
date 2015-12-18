using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model.Shared
{
    [Validator(typeof(OtherFundsValidator))]
    public class OtherFunds
    {
        public OtherFunds()
        {
            usGovt = new USGovt();
            international = new International();
            other = new Other();
        }

        public USGovt usGovt { get; set; }

        public International international { get; set; }

        public string EVGovt { get; set; }

        public string BinationalCommission { get; set; }

        public Other other { get; set; }

        public string personal { get; set; }
    }
}