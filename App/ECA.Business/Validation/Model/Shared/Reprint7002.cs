namespace ECA.Business.Validation.Model.CreateEV
{
    /// <summary>
    /// Request if DS-7002 in PDF document is returned with SEVIS Batch download
    /// </summary>
    public class Reprint7002
    {
        public bool print7002 { get; set; }

        public string SiteId { get; set; }
    }
}