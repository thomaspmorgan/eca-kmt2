using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model.Shared
{
    [Validator(typeof(DeleteSiteOfActivityValidator))]
    public class DeleteSiteOfActivity
    {
        public DeleteSiteOfActivity()
        { }
        
        public string SiteId { get; set; }
    }
}