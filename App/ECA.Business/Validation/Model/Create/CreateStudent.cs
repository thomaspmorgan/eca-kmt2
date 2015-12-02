
namespace ECA.Business.Validation.Model
{
    /// <summary>
    /// Top-level object to hold a new create student object
    /// </summary>
    public class CreateStudent
    {
        public CreateStudent()
        {
            student = new Student();
        }

        public Student student { get; set; }
    }
}
