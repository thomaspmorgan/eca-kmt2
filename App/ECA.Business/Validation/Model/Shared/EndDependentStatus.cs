namespace ECA.Business.Validation.Model
{
    /// <summary>
    /// End the status for a dependent
    /// </summary>
    public class EndDependentStatus
    {
        public EndDependentStatus()
        {            
        }

        /// <summary>
        /// Dependent sevis id
        /// </summary>
        public string dependentSevisID { get; set; }

        /// <summary>
        /// End status reason code
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// Explanation for selecting "Other" as the reason
        /// </summary>
        public string OtherRemarks { get; set; }

        /// <summary>
        /// Remarks for end status
        /// </summary>
        public string Remarks { get; set; }
    }
}