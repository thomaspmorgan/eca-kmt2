namespace ECA.Business.Validation.Model.CreateEV
{
    public class AddSiteOfActivity
    {
        public AddSiteOfActivity()
        {
            siteOfActivity = new SiteOfActivity();
        }

        /// <summary>
        /// xsi:type = SOA or EXEMPT
        /// </summary>
        public SiteOfActivity siteOfActivity { get; set; }
    }
}