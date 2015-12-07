using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(AuthDropBelowFCValidator))]
    public class AuthDropBelowFC
    {
        public AddAuthDrop addAuthDrop { get; set; }
        
        public CancelAuthDrop cancelAuthDrop { get; set; }
        
        public EditAuthDrop editAuthDrop { get; set; }        
    }
}
