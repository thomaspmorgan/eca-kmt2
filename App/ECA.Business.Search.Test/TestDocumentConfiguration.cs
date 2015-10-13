using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Search.Test
{
    public class TestDocumentConfiguration : DocumentConfiguration<TestDocument, int>
    {
        public static Guid TEST_DOCUMENT_DOCUMENT_TYPE_ID = Guid.Parse("02ddc89d-9557-4de5-b1d4-2de9e1130c5c");

        public const string TEST_DOCUMENT_DOCUMENT_TYPE_NAME = "testdocument";

        public TestDocumentConfiguration() : this(true) { }

        public TestDocumentConfiguration(bool shouldConfigure)
        {
            if (shouldConfigure)
            {
                HasDescription(x => x.Description);
                HasFoci(x => x.Foci);
                HasGoals(x => x.Goals);
                HasKey(x => x.Id);
                HasName(x => x.Name);
                HasStartDate(x => x.StartDate);
                HasEndDate(x => x.EndDate);
                HasStatus(x => x.Status);
                HasRegions(x => x.Regions);
                HasCountries(x => x.Countries);
                HasLocations(x => x.Locations);
                HasWebsites(x => x.Websites);
                HasObjectives(x => x.Objectives);
                HasOfficeSymbol(x => x.OfficeSymbol);
                HasPointsOfContact(x => x.PointsOfContact);
                HasThemes(x => x.Themes);
                HasAddresses(x => x.Addresses);
                HasPhoneNumbers(x => x.PhoneNumbers);
                IsDocumentType(TEST_DOCUMENT_DOCUMENT_TYPE_ID, TEST_DOCUMENT_DOCUMENT_TYPE_NAME);
            }
                       
        }
    }

    public class OtherTestDocumentConfiguration : DocumentConfiguration<OtherTestDocument, int>
    {
        public static Guid OTHER_TEST_DOCUMENT_DOCUMENT_TYPE_ID = Guid.Parse("4dcea74b-9cf5-41f6-a3b9-315f8a6686f2");

        public const string OTHER_TEST_DOCUMENT_DOCUMENT_TYPE_NAME = "other test document";

        public OtherTestDocumentConfiguration()
        {
            HasDescription(x => x.Description);
            HasFoci(x => x.Foci);
            HasGoals(x => x.Goals);
            HasKey(x => x.Id);
            HasName(x => x.Name);
            HasObjectives(x => x.Objectives);
            HasOfficeSymbol(x => x.OfficeSymbol);
            HasPointsOfContact(x => x.PointsOfContact);
            HasThemes(x => x.Themes);
            IsDocumentType(OTHER_TEST_DOCUMENT_DOCUMENT_TYPE_ID, OTHER_TEST_DOCUMENT_DOCUMENT_TYPE_NAME);
        }
    }
}
