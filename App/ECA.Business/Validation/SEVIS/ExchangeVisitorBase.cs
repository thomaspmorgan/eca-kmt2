using ECA.Business.Queries.Models.Persons;
using ECA.Business.Sevis.Model;
using FluentValidation;
using System;
using System.Collections.Generic;
using FluentValidation.Results;
using ECA.Business.Queries.Models.Persons.ExchangeVisitor;
using ECA.Business.Validation.Sevis.Bio;
using ECA.Data;
using System.Linq;
using System.Diagnostics.Contracts;
using ECA.Business.Validation.Sevis.Finance;
using ECA.Business.Service;
using ECA.Business.Queries.Models.Admin;

namespace ECA.Business.Validation.Sevis
{
    public abstract class ExchangeVisitorBase
    {
        public ExchangeVisitorBase()
        {

        }

        /// <summary>
        /// Gets the person info.
        /// </summary>
        public Bio.Person Person { get; private set; }

        /// <summary>
        /// Gets the financial info.
        /// </summary>
        public FinancialInfo FinancialInfo { get; private set; }

        /// <summary>
        /// Gets the category code.
        /// </summary>
        public string CategoryCode { get; private set; }

        /// <summary>
        /// Gets the mail address.
        /// </summary>
        public USAddress MailAddress { get; private set; }

        /// <summary>
        /// Gets the us address.
        /// </summary>
        public USAddress USAddress { get; private set; }

        /// <summary>
        /// Gets the occupation category code.
        /// </summary>
        public string OccupationCategoryCode { get; private set; }

        /// <summary>
        /// Gets the position code.
        /// </summary>
        public string PositionCode { get; private set; }

        /// <summary>
        /// Gets the program end date.
        /// </summary>
        public DateTime ProgramEndDate { get; private set; }

        /// <summary>
        /// Gets the program state date.
        /// </summary>
        public DateTime ProgramStartDate { get; private set; }

        /// <summary>
        /// Gets the subject field.
        /// </summary>
        public string SubjectField { get; private set; }

        /// <summary>
        /// Gets the user.
        /// </summary>
        public User User { get; private set; }

        /// <summary>
        /// Gets the participant id.
        /// </summary>
        public int ParticipantId { get; private set; }

        /// <summary>
        /// Gets the person id.
        /// </summary>
        public int PersonId { get; private set; }

        /// <summary>
        /// Gets the dependents.
        /// </summary>
        public IEnumerable<Dependent> Dependents { get; private set; }


        public void SetAddSiteOfActivity()
        {
            throw new NotImplementedException();
            //will fall into the items of the sevis class
        }


        public void SetAddTIPP()
        {
            throw new NotImplementedException();
            //will fall into the items of the sevis class
        }

        public abstract ValidationResult Validate(IValidator validator);

        /// <summary>
        /// Sets the program start date from the given participant person.
        /// </summary>
        /// <param name="participantPerson">The participant person.</param>
        public void SetStartDate(ParticipantPerson participantPerson)
        {
            this.ProgramStartDate = participantPerson.StartDate.HasValue ? participantPerson.StartDate.Value.UtcDateTime : default(DateTime);
        }

        /// <summary>
        /// Sets the program end date from the given participant person.
        /// </summary>
        /// <param name="participantPerson">The participant person.</param>
        public void SetEndDate(ParticipantPerson participantPerson)
        {
            this.ProgramEndDate = participantPerson.EndDate.HasValue ? participantPerson.EndDate.Value.UtcDateTime : default(DateTime);
        }

        /// <summary>
        /// Sets the category code with the given program category code, or null if it is null.
        /// </summary>
        /// <param name="category">The program category code.</param>
        public void SetCategoryCode(ProgramCategory category)
        {
            if (category != null)
            {
                this.CategoryCode = category.ProgramCategoryCode;
            }
            else
            {
                this.CategoryCode = null;
            }
        }

        /// <summary>
        /// Sets the position code with the given position, or null, if the position is null.
        /// </summary>
        /// <param name="position">the position.</param>
        public void SetPositionCode(Position position)
        {
            if (position != null)
            {
                this.PositionCode = position.PositionCode;
            }
            else
            {
                this.PositionCode = null;
            }

        }

        /// <summary>
        /// Sets the occupation category with the given category code.
        /// </summary>
        /// <param name="occupationCategoryCode">The occupation category code.</param>
        public void SetOccupationCategoryCode(string occupationCategoryCode)
        {
            this.OccupationCategoryCode = occupationCategoryCode;
        }

        /// <summary>
        /// Sets the person to the person that is created by the given biography.
        /// </summary>
        /// <param name="biography"></param>
        public void SetPerson(BiographicalDTO biography)
        {
            if (biography != null)
            {
                this.Person = biography.GetPerson();
                this.PersonId = biography.PersonId;
            }
            else
            {
                this.Person = null;
                this.PersonId = 0;
            }
        }

        /// <summary>
        /// Sets the ParticipantId from the given participant.
        /// </summary>
        /// <param name="participant">The participant.</param>
        public void SetParticipantId(Participant participant)
        {
            Contract.Requires(participant != null, "The participant must be set.");
            this.ParticipantId = participant.ParticipantId;
        }

        /// <summary>
        /// Sets the subject field with the given field of study.
        /// </summary>
        /// <param name="fieldOfStudy"></param>
        public void SetSubjectField(FieldOfStudy fieldOfStudy)
        {
            if (fieldOfStudy != null)
            {
                this.SubjectField = fieldOfStudy.FieldOfStudyCode;
            }
            else
            {
                this.SubjectField = null;
            }
        }

        /// <summary>
        /// Sets the us address from the given address dto.
        /// </summary>
        /// <param name="address">The address.</param>
        public void SetUSAddress(AddressDTO address)
        {
            if (address != null)
            {
                var usAddress = address.GetUSAddress();
                this.USAddress = usAddress;
            }
            else
            {
                this.USAddress = null;
            }
        }

        /// <summary>
        /// Sets the mailing address from the given address.
        /// </summary>
        /// <param name="address">The mailing address.</param>
        public void SetMailAddress(AddressDTO address)
        {
            if (address != null)
            {
                var usAddress = address.GetUSAddress();
                this.MailAddress = usAddress;
            }
            else
            {
                this.MailAddress = null;
            }
        } 

        /// <summary>
        /// Sets the dependents of this exchange visitor.
        /// </summary>
        /// <param name="dependents">The exchange visitor's dependents.</param>
        public void SetDependents(IEnumerable<Dependent> dependents)
        {
            if(dependents != null)
            {
                this.Dependents = dependents;
            }
            else
            {
                this.Dependents = new List<Dependent>();
            }
        }

        /// <summary>
        /// Sets the financial info with the given instance.
        /// </summary>
        /// <param name="financialInfo">The financial info.</param>
        public void SetFinancialInfo(FinancialInfo financialInfo)
        {
            if (financialInfo != null)
            {
                this.FinancialInfo = financialInfo;
            }
            else
            {
                this.FinancialInfo = null;
            }
        }
    }

}
