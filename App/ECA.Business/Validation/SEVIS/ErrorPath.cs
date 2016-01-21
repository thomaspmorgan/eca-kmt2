
namespace ECA.Business.Validation.SEVIS
{
    /// <summary>
    /// Routing path
    /// </summary>
    public class ErrorPath
    {
        public string Category { get; set; }
        public string CategorySub { get; set; }
        public string Section { get; set; }
        public string Tab { get; set; }
    }

    /// <summary>
    /// Routing category
    /// </summary>
    public sealed class ElementCategory
    {
        private readonly string name;
        private readonly int value;

        public static readonly ElementCategory Person = new ElementCategory(1, "people");
        public static readonly ElementCategory Project = new ElementCategory(2, "projects");

        private ElementCategory(int value, string name)
        {
            this.name = name;
            this.value = value;
        }

        public override string ToString()
        {
            return name;
        }
    }

    /// <summary>
    /// Routing sub category
    /// </summary>
    public sealed class ElementCategorySub
    {
        private readonly string name;
        private readonly int value;

        public static readonly ElementCategorySub PersonalInfo = new ElementCategorySub(1, "personalinformation");
        public static readonly ElementCategorySub Participant = new ElementCategorySub(2, "participants");
        public static readonly ElementCategorySub MoneyFlows = new ElementCategorySub(3, "moneyflows");

        private ElementCategorySub(int value, string name)
        {
            this.name = name;
            this.value = value;
        }

        public override string ToString()
        {
            return name;
        }
    }

    /// <summary>
    /// Page section id of bookmark
    /// </summary>
    public sealed class ElementCategorySection
    {
        private readonly string name;
        private readonly int value;

        public static readonly ElementCategorySection General = new ElementCategorySection(1, "general");
        public static readonly ElementCategorySection PII = new ElementCategorySection(2, "pii");
        public static readonly ElementCategorySection Contact = new ElementCategorySection(3, "contact");
        public static readonly ElementCategorySection EducationEmployment = new ElementCategorySection(4, "eduemp");

        private ElementCategorySection(int value, string name)
        {
            this.name = name;
            this.value = value;
        }

        public override string ToString()
        {
            return name;
        }
    }
    
    /// <summary>
    /// Optional tab id to navigate to a tab inside a section
    /// </summary>
    public sealed class ElementCategorySectionTab
    {
        private readonly string name;
        private readonly int value;

        public static readonly ElementCategorySectionTab PersonalInfo = new ElementCategorySectionTab(1, "personalinfo");
        public static readonly ElementCategorySectionTab Funding = new ElementCategorySectionTab(2, "funding");
        public static readonly ElementCategorySectionTab Sevis = new ElementCategorySectionTab(3, "sevis");
        public static readonly ElementCategorySectionTab ExchVisitor = new ElementCategorySectionTab(4, "ev");

        private ElementCategorySectionTab(int value, string name)
        {
            this.name = name;
            this.value = value;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
