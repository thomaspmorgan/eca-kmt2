using ECA.Business.Validation.Model.Shared;
using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(ProgramValidator))]
    public class Program
    {
        public Program()
        {
            Amend = new AmendProgram();
            EditSubject = new SubjectFieldUpdate();
            Extension = new ProgramExtension();
            Matriculate = new MatriculateProgram();
            Shorten = new ShortenProgramUpdate();
        }

        public AmendProgram Amend { get; set; }

        public SubjectFieldUpdate EditSubject { get; set; }
                
        public ProgramExtension Extension { get; set; }

        public MatriculateProgram Matriculate { get; set; }

        public ShortenProgramUpdate Shorten { get; set; }
    }
}
