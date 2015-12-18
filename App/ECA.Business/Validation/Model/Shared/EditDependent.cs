using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using ECA.Business.Validation.Model.Shared;
using ECA.Business.Validation.Model.CreateEV;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(EditDependentValidator))]
    public class EditDependent
    {
        public EditDependent()
        {
            Add = new AddDependent();
            Delete = new DeleteDependent();
            Edit = new PersonalInfo();
            EndStatus = new CancelDependent();
            Terminate = new TerminateDependent();
            FinancialInfo = new FinancialInfo();
            Reprint = new ReprintForm();
            SiteOfActivity = new AddSiteOfActivity();
            TIPP = new AddTIPP();
            AddSite = new TippSite();
            EditSite = new TippSite();
            DeleteSite = new TippSite();
            AddPhase = new TippPhase();
            EditPhase = new TippPhase();
            DeletePhase = new TippPhase();
            UpdateSignatureDates = new SignatureDates();
            program = new Program();
            fullName = new FullName();
        }

        public string dependentSevisID { get; set; }

        public bool PrintForm { get; set; }

        public string UserDefinedA { get; set; }

        public string UserDefinedB { get; set; }

        public AddDependent Add { get; set; }

        public DeleteDependent Delete { get; set; }

        /// <summary>
        /// Edit dependent personal information
        /// </summary>
        public PersonalInfo Edit { get; set; }

        /// <summary>
        /// End the status for a dependent
        /// </summary>
        public CancelDependent EndStatus { get; set; }
        
        /// <summary>
        /// Terminate dependent
        /// </summary>
        public TerminateDependent Terminate { get; set; }

        /// <summary>
        /// Visitor financial info
        /// </summary>
        public FinancialInfo FinancialInfo { get; set; }

        /// <summary>
        /// Reprint Dependent DS-2019
        /// </summary>
        public ReprintForm Reprint { get; set; }

        /// <summary>
        /// Site of Activity Events
        /// </summary>
        public AddSiteOfActivity SiteOfActivity { get; set; }

        /// <summary>
        /// T/IPP information
        /// </summary>
        public AddTIPP TIPP { get; set; }

        /// <summary>
        /// T/IPP add site to a T/IPP phase
        /// </summary>
        public TippSite AddSite { get; set; }

        /// <summary>
        /// T/IPP edit site to a T/IPP phase
        /// </summary>
        public TippSite EditSite { get; set; }

        /// <summary>
        /// T/IPP delete site to a T/IPP phase
        /// </summary>
        public TippSite DeleteSite { get; set; }

        /// <summary>
        /// T/IPP - add phase
        /// </summary>
        public TippPhase AddPhase { get; set; }

        /// <summary>
        /// T/IPP site of activity – edit phase
        /// </summary>
        public TippPhase EditPhase { get; set; }

        /// <summary>
        /// T/IPP delete phase
        /// </summary>
        public TippPhase DeletePhase { get; set; }

        /// <summary>
        /// Supervisor updated signature information
        /// </summary>
        public SignatureDates UpdateSignatureDates { get; set; }
        
        /// <summary>
        /// Visitor program
        /// </summary>
        public Program program { get; set; }
        
        public FullName fullName { get; set; }
        
        public DateTime BirthDate { get; set; }
        
        public string Gender { get; set; }
        
        public string BirthCountryCode { get; set; }

        public string CitizenshipCountryCode { get; set; }
        
        public string Email { get; set; }
        
        public string Relationship { get; set; }
        
        public string Remarks { get; set; }        
    }
}
