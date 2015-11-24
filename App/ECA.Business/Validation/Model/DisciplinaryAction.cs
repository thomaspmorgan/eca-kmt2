using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class DisciplinaryAction
    {
        [MaxLength(500)]
        public string Explanation { get; set; }

    }
}
