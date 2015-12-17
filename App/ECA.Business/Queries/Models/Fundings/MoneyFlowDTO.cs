using System;

namespace ECA.Business.Queries.Models.Fundings
{
    /// <summary>
    /// The money flow dto is used to represent a transfer of money from one entity to another in the eca system for a business layer client.
    /// 
    /// A money flow is from the perspective of the entity and money coming in or going out, i.e. incoming or outgoing.  The
    /// SourceRecipientX refers to either the recipient or the source of that money, i.e. where it is going to or coming from.  For
    /// example, a project may have been granted money by a program.  Therefore, the EntityId and EntityTypeId will be referencing the project
    /// since it has money coming into it and the SourceRecipient will be refering to the program as the source since it has money leaving it.
    /// 
    /// Vice Versa, the money flow dto for the program would have it's EntityId and EntityTypeId referencing the program, and it's source
    /// recipient being the project and its id and type.
    /// </summary>
    public class SimpleMoneyFlowDTO
    {
        /// <summary>
        /// Gets or sets the money flow line item id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the entity id i.e. the project id, program id, etc where money is coming into or leaving from.
        /// </summary>
        public int EntityId { get; set; }

        /// <summary>
        /// Gets or sets the entity type id, i.e. the money flow source recipient type id of the entity that money is coming
        /// into or leaving from.
        /// </summary>
        public int EntityTypeId { get; set; }

        /// <summary>
        /// Gets or sets the transaction date.
        /// </summary>
        public DateTimeOffset TransactionDate { get; set; }

        /// <summary>
        /// Gets or sets the entity type id of the money flow's source or recipient entity, i.e. money flow source recipient type id.
        /// </summary>
        public int SourceRecipientEntityTypeId { get; set; }

        /// <summary>
        /// Gets or sets the id of the entity the money flow is going to or coming from.
        /// </summary>
        public int? SourceRecipientEntityId { get; set; }

        /// <summary>
        /// Gets or sets the amount of the money flow.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the status id, i.e. the money flow status id.
        /// </summary>
        public int MoneyFlowStatusId { get; set; }

        /// <summary>
        /// Gets or sets the fiscal year.
        /// </summary>
        public int FiscalYear { get; set; }

        /// <summary>
        /// Gets or sets the parent money flow id.
        /// </summary>
        public int? ParentMoneyFlowId { get; set; }
    }


    /// <summary>
    /// The money flow dto is used to represent a transfer of money from one entity to another in the eca system for a business layer client.
    /// 
    /// A money flow is from the perspective of the entity and money coming in or going out, i.e. incoming or outgoing.  The
    /// SourceRecipientX refers to either the recipient or the source of that money, i.e. where it is going to or coming from.  For
    /// example, a project may have been granted money by a program.  Therefore, the EntityId and EntityTypeId will be referencing the project
    /// since it has money coming into it and the SourceRecipient will be refering to the program as the source since it has money leaving it.
    /// 
    /// Vice Versa, the money flow dto for the program would have it's EntityId and EntityTypeId referencing the program, and it's source
    /// recipient being the project and its id and type.
    /// </summary>
    public class MoneyFlowDTO : SimpleMoneyFlowDTO
    {
        /// <summary>
        /// Gets or sets the source type name of the incoming or outgoing money flow, i.e. money flow source recipient type name.
        /// </summary>
        public string SourceRecipientTypeName { get; set; }

        /// <summary>
        /// Gets or sets the name of the entity the money flow is going to or coming from, i.e. the project name, program name, etc.
        /// </summary>
        public string SourceRecipientName { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the status, i.e. the money flow status name.
        /// </summary>
        public string MoneyFlowStatus { get; set; }

        /// <summary>
        /// Gets or sets the type i.e. money flow type i.e. incoming, outgoing, etc.
        /// </summary>
        public string MoneyFlowType { get; set; }

        /// <summary>
        /// Gets or sets the participant type.
        /// </summary>
        public int? ParticipantTypeId { get; set; }

        /// <summary>
        /// Gets or sets the participant type name.
        /// </summary>
        public string ParticipantTypeName { get; set; }

        /// <summary>
        /// Gets or sets the grant number.
        /// </summary>
        public string GrantNumber { get; set; }
    }
}
