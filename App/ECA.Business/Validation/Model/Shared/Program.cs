using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(ProgramValidator))]
    public class Program
    {
        public Program()
        {
            cancelExtension = new CancelProgramExtension();
            deferAttendence = new DeferProgramAttendence();
            edit = new EditProgram();
            extension = new ProgramExtension();
            shorten = new ShortenProgram();
        }

        public CancelProgramExtension cancelExtension { get; set; }
        
        public DeferProgramAttendence deferAttendence { get; set; }
        
        public EditProgram edit { get; set; }
        
        public ProgramExtension extension { get; set; }
        
        public ShortenProgram shorten { get; set; }        
    }
}
