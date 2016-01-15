using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model.Shared
{
    [Validator(typeof(DeleteTippSiteValidator))]
    public class DeleteTippSite
    {
        public DeleteTippSite()
        { }

        public string SiteId { get; set; }
    }
}