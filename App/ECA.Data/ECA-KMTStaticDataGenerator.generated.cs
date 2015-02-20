




#region LocationType
namespace ECA.Data
{
	public partial class LocationType
	{
		//SELECT * FROM LocationType
	}
}
#endregion

#region ProjectStatus
namespace ECA.Data
{
	public partial class ProjectStatus
	{
		//SELECT * FROM ProjectStatus
		public static StaticLookup Draft { get { return new StaticLookup("Draft", 1); } }
	}
}
#endregion

#region StaticLookup
namespace ECA.Data
{
	public class StaticLookup
	{
		public StaticLookup(string value, int id)
		{
			this.Value = value;
			this.Id = id;
		}

		public string Value { get; private set; }

		public int Id { get; private set; }
	}
}
#endregion
