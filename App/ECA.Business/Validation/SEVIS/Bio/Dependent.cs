using ECA.Business.Sevis.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Validation.Sevis.Bio
{
    /// <summary>
    /// A Dependent instance is used to specify what action will be taken on a sevis registered exchange visitor dependent, such as
    /// adding a new dependenting, deleting a sevis registered dependent, or editing a sevis registered dependent.
    /// </summary>
    public abstract class Dependent : Biography, IUserDefinable
    {
        /// <summary>
        /// Gets or sets the user defined a field.
        /// </summary>
        public string UserDefinedA { get; private set; }

        /// <summary>
        /// Gets or sets the user defined b field.
        /// </summary>
        public string UserDefinedB { get; private set; }

        /// <summary>
        /// Sets the UserDefinedA field to the participant ic.
        /// </summary>
        /// <param name="participantId">The participant id.</param>
        public void SetParticipantId(int participantId)
        {
            this.UserDefinedA = participantId.ToString();
        }

        /// <summary>
        /// Sets the UserDefinedB field to the person id.
        /// </summary>
        /// <param name="personId">The person id.</param>
        public void SetPersonId(int personId)
        {
            this.UserDefinedB = personId.ToString();
        }

        /// <summary>
        /// Returns the person id of this dependent.
        /// </summary>
        /// <returns>The person id of this dependent.</returns>
        public int? GetPersonId()
        {
            if (!String.IsNullOrWhiteSpace(this.UserDefinedB))
            {
                return Int32.Parse(this.UserDefinedB);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the participant id i.e. the participant id of the person that is participating in the project and who this person
        /// is a dependent of.
        /// </summary>
        /// <returns>The participant id.</returns>
        public int? GetParticipantId()
        {
            if (!String.IsNullOrWhiteSpace(this.UserDefinedA))
            {
                return Int32.Parse(this.UserDefinedA);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets or sets the relationship.
        /// </summary>
        public string Relationship { get; set; }

        /// <summary>
        /// Returns a SEVISEVBatchTypeExchangeVisitorDependent(Add|Delete|Edit|EndStatus|Reprint|Terminate) instance used when performing
        /// a sevis registered exchange visitor's dependent details.
        /// </summary>
        /// <returns>A SEVISEVBatchTypeExchangeVisitorDependent(Add|Delete|Edit|EndStatus|Reprint|Terminate) instance.</returns>
        public abstract object GetSevisExhangeVisitorDependentInstance();
    }
}
