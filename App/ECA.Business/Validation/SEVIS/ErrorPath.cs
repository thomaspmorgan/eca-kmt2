
namespace ECA.Business.Validation.SEVIS
{
    public class ErrorPath
    {
        public string Category { get; set; }
        public string Section { get; set; }
        public string Tab { get; set; }
    }

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

    public sealed class ElementCategorySection
    {
        private readonly string name;
        private readonly int value;

        public static readonly ElementCategorySection General = new ElementCategorySection(1, "General");
        public static readonly ElementCategorySection Pii = new ElementCategorySection(2, "Pii");
        public static readonly ElementCategorySection Contact = new ElementCategorySection(3, "Contact");
        public static readonly ElementCategorySection EduEmp = new ElementCategorySection(4, "EduEmp");
        public static readonly ElementCategorySection pid = new ElementCategorySection(5, "pid");
        
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
    
    public sealed class ElementCategorySectionTab
    {
        private readonly string name;
        private readonly int value;

        public static readonly ElementCategorySectionTab PersonalInfo = new ElementCategorySectionTab(1, "info");
        public static readonly ElementCategorySectionTab SevisInfo = new ElementCategorySectionTab(2, "sevis");
        public static readonly ElementCategorySectionTab ExchangeVisitor = new ElementCategorySectionTab(3, "ev");
        
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
