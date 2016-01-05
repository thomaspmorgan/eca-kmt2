using ECA.Business.Validation.Model.CreateEV;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model.Shared
{
    /// <summary>
    /// 
    /// </summary>
    [Validator(typeof(AddTIPPUpdateValidator))]
    public class AddTIPPUpdate
    {
        public AddTIPPUpdate()
        {
            ParticipantInfo = new ParticipantInfoUpdate();
            TippSite = new TippSite();
        }

        public bool print7002 { get; set; }

        public ParticipantInfoUpdate ParticipantInfo { get; set; }

        public TippSite TippSite { get; set; }
    }
}