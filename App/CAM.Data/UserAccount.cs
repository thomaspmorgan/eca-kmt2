//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CAM.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserAccount
    {
        public int PrincipalId { get; set; }
        public System.Guid AdGuid { get; set; }
        public Nullable<System.DateTimeOffset> LastAccessed { get; set; }
        public int AccountStatusId { get; set; }
        public Nullable<System.DateTimeOffset> PermissionsRevisedOn { get; set; }
        public string Note { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string DisplayName { get; set; }
        public string EmailAddress { get; set; }
        public System.DateTimeOffset CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTimeOffset RevisedOn { get; set; }
        public int RevisedBy { get; set; }
        public Nullable<System.DateTimeOffset> ExpiredDate { get; set; }
        public Nullable<System.DateTimeOffset> SuspendedDate { get; set; }
        public Nullable<System.DateTimeOffset> RevokedDate { get; set; }
        public Nullable<System.DateTimeOffset> RestoredDate { get; set; }
    
        public virtual Principal Principal { get; set; }
        public virtual AccountStatus AccountStatus { get; set; }
    }
}