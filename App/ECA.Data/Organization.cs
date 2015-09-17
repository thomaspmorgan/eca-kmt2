using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using ECA.Core.Data;

namespace ECA.Data
{
    /// <summary>
    /// An organization is an entity which participates in the implementation or execution of a project, such as a cooperating organization, a host family or an ECA office.
    /// </summary>
    public class Organization : 
        IPermissable, 
        IHistorical, 
        IContactable,
        IAddressable,
        ISocialable
    {
        /// <summary>
        /// Returns all organization type ids that represent an office.
        /// </summary>
        public static readonly int[] OFFICE_ORGANIZATION_TYPE_IDS = new int[] { OrganizationType.Office.Id, OrganizationType.Division.Id, OrganizationType.Branch.Id };

        /// <summary>
        /// The max length of the organization name.
        /// </summary>
        public const int NAME_MAX_LENGTH = 600;

        /// <summary>
        /// The max length of the office symbol.
        /// </summary>
        public const int OFFICE_SYMBOL_MAX_LENGTH = 128;

        /// <summary>
        /// The max length of the website.
        /// </summary>
        public const int WEBSITE_MAX_LENGTH = 2000;

        /// <summary>
        /// The maximum length of the description.
        /// </summary>
        public const int DESCRIPTION_MAX_LENGTH = 3000;

        /// <summary>
        /// Creates a new instances and initializes all collection properties.
        /// </summary>
        public Organization()
        {
            this.History = new History();
            this.SocialMedias = new HashSet<SocialMedia>();
            this.MoneyFlowSources = new HashSet<MoneyFlow>();
            this.MoneyFlowRecipients = new HashSet<MoneyFlow>();
            this.OwnerPrograms = new HashSet<Program>();
            this.Contacts = new HashSet<Contact>();
            this.Addresses = new HashSet<Address>();
            this.OfficeSettings = new HashSet<OfficeSetting>();
        }

        [Key]
        public int OrganizationId { get; set; }
        [Required]
        public OrganizationType OrganizationType { get; set; }
        public int OrganizationTypeId { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [MaxLength(DESCRIPTION_MAX_LENGTH)]
        [Required]
        public string Description { get; set; }
        
        /// <summary>
        /// Gets or sets the parent organization.
        /// </summary>
        public Organization ParentOrganization { get; set; }

        /// <summary>
        /// Gets or sets the parent organization id.
        /// </summary>
        public int? ParentOrganizationId { get; set; }
        public string Status { get; set; }
        public ICollection<Address> Addresses { get; set; }

        /// <summary>
        /// Gets or sets the points of contact.
        /// </summary>
        public ICollection<Contact> Contacts { get; set; }

        /// <summary>
        /// Gets or sets the name of the organization.
        /// </summary>
        [Required]
        [MaxLength(NAME_MAX_LENGTH)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the office symbol, i.e. abbreviation of the organization.
        /// </summary>
        public string OfficeSymbol { get; set; }

        /// <summary>
        /// Gets or sets the website max length.
        /// </summary>
        [MaxLength(WEBSITE_MAX_LENGTH)]
        public string Website { get; set; }

        /// <summary>
        /// Gets or sets the social media.
        /// </summary>
        public ICollection<SocialMedia> SocialMedias { get; set; }

        //relationships
        public ICollection<MoneyFlow> MoneyFlowSources { get; set; }
        public ICollection<MoneyFlow> MoneyFlowRecipients { get; set; }
        public ICollection<Program> OwnerPrograms { get; set; }

        public ICollection<Justification> Justifications { get; set; }
        public ICollection<Focus> Foci { get; set; }

        public History History { get; set; }

        public ICollection<OfficeSetting> OfficeSettings { get; set; }

        /// <summary>
        /// Returns the organization id.
        /// </summary>
        /// <returns>The organization id.</returns>
        public int GetId()
        {
            return this.OrganizationId;
        }

        /// <summary>
        /// Returns the office permissable type.
        /// </summary>
        /// <returns>Office permissable type.</returns>
        public PermissableType GetPermissableType()
        {
            return PermissableType.Office;
        }

        public int? GetParentId()
        {
            return null;
        }

        public PermissableType GetParentPermissableType()
        {
            throw new NotSupportedException("The organization does not have a parent permissable type.");
        }

        /// <summary>
        /// Returns true if this organization is not an office, otherwise false.
        /// </summary>
        /// <returns>Returns true if this organization is not an office, otherwise false.</returns>
        public bool IsExempt()
        {
            return !OFFICE_ORGANIZATION_TYPE_IDS.Contains(this.OrganizationTypeId);
        }
    }

}
