using ECA.Business.Validation.Model.CreateEV;
using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(ProgramValidator))]
    public class Program
    {
        public Program()
        {
            CancelExtension = new CancelProgramExtension();
            DeferAttendence = new DeferProgramAttendence();
            Edit = new EditProgram();
            Extension = new ProgramExtension();
            Shorten = new ShortenProgram();
        }

        public AmendProgram Amend { get; set; }

        public SubjectField EditSubject { get; set; }

        public CancelProgramExtension CancelExtension { get; set; }
        
        public DeferProgramAttendence DeferAttendence { get; set; }
        
        public EditProgram Edit { get; set; }
        
        public ProgramExtension Extension { get; set; }

        public MatriculateProgram Matriculate { get; set; }

        public ShortenProgram Shorten { get; set; }
    }
}
