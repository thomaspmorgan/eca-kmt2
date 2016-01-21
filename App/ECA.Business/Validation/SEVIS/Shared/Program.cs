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

        /// <summary>
        /// Amend a program
        /// </summary>
        public AmendProgram Amend { get; set; }

        /// <summary>
        /// Edit subject or field of study
        /// </summary>
        public SubjectFieldUpdate EditSubject { get; set; }
        
        /// <summary>
        /// Extension within maximum duration of stay
        /// </summary>
        public ProgramExtension Extension { get; set; }

        /// <summary>
        /// Matriculation of exchange visitor
        /// </summary>
        public MatriculateProgram Matriculate { get; set; }

        /// <summary>
        /// Shorten a program before its end date
        /// </summary>
        public ShortenProgramUpdate Shorten { get; set; }
    }
}
