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

        public void SetStartDate(ParticipantPerson participantPerson)
        {
            this.ProgramStartDate = participantPerson.StartDate.HasValue ? participantPerson.StartDate.Value.UtcDateTime : default(DateTime);
        }

        public void SetEndDate(ParticipantPerson participantPerson)
        {
            this.ProgramEndDate = participantPerson.EndDate.HasValue ? participantPerson.EndDate.Value.UtcDateTime : default(DateTime);

        }


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

        public void SetOccupationCategoryCode(string occupationCategoryCode)
        {
            this.OccupationCategoryCode = occupationCategoryCode;
        }

        public void SetPerson(BiographicalDTO biography)
        {
            if (biography != null)
            {
                this.Person = biography.GetPerson();
            }
            else
            {
                this.Person = null;
            }
        }

        public void SetParticipantId(Participant participant)
        {
            Contract.Requires(participant != null, "The participant must be set.");
            this.ParticipantId = participant.ParticipantId;
        }

        public void SetUserId(User user)
        {
            if (user != null)
            {
                this.User = user;
            }
            else
            {
                this.User = null;
            }
        }

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

        public void SetPersonId(BiographicalDTO biography)
        {
            Contract.Requires(biography != null, "The biography must be defined.");
            this.PersonId = biography.PersonId;
        }


        public void SetDependents(IEnumerable<Dependent> dependents)
        {
            this.Dependents = dependents;
        }


        public void SetFinancialInfo(FinancialInfo financialInfo)
        {
            if (financialInfo != null)
            {
                this.FinancialInfo = null;
            }
            else
            {
                this.FinancialInfo = null;
            }
        }



    }

    public class NewExchangeVisitor :
        ExchangeVisitorBase,
        IFormPrintable,
        IUserIdentifiable,
        IRequestIdentifiable
    {


        #region IFormPrintable
        public bool PrintForm { get; set; }
        #endregion

        #region  IRequestIdentifiable
        public string RequestId { get; set; }

        #endregion

        #region IUserIdentifiable
        public string UserId { get; set; }
        #endregion


        public override ValidationResult Validate(IValidator validator)
        {
            throw new NotImplementedException();
        }


        public SEVISEVBatchTypeExchangeVisitor GetSEVISBatchTypeExchangeVisitor()
        {
            return new SEVISEVBatchTypeExchangeVisitor
            {

            };
        }
    }

    public class UpdatedExchangeVisitor :
        ExchangeVisitorBase,
        IFormPrintable,
        IUserIdentifiable,
        IRequestIdentifiable
    {

        #region IFormPrintable
        public bool PrintForm { get; set; }
        #endregion

        #region  IRequestIdentifiable
        public string RequestId { get; set; }
        #endregion

        #region IUserIdentifiable
        public string UserId { get; set; }
        #endregion

        /// <summary>
        /// Returns all update sevis batch objects.  For example, if a participant has been sent to sevis and
        /// a name has changed, a dependent has been added, and another dependent has been updated.  This collection
        /// will contain all update sevis exchange visitor objects to perform those updates.
        /// </summary>
        /// <returns>All update sevis batch objects.</returns>
        public IEnumerable<SEVISEVBatchTypeExchangeVisitor1> GetSEVISEVBatchTypeExchangeVisitor1Collection()
        {
            return Enumerable.Empty<SEVISEVBatchTypeExchangeVisitor1>();
        }

        public override ValidationResult Validate(IValidator validator)
        {
            throw new NotImplementedException();
        }
    }
}
