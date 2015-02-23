




#region LocationType
namespace ECA.Data
{
	public partial class LocationType
	{
		//SELECT * FROM LocationType
		public static StaticLookup Region { get { return new StaticLookup("Region", 2); } }
		public static StaticLookup Country { get { return new StaticLookup("Country", 3); } }
		public static StaticLookup State { get { return new StaticLookup("State", 4); } }
		public static StaticLookup City { get { return new StaticLookup("City", 5); } }
		public static StaticLookup Building { get { return new StaticLookup("Building", 6); } }
		public static StaticLookup Post { get { return new StaticLookup("Post", 7); } }
		public static StaticLookup Place { get { return new StaticLookup("Place", 8); } }
	}
}
#endregion

#region ProjectStatus
namespace ECA.Data
{
	public partial class ProjectStatus
	{
		//SELECT * FROM ProjectStatus
		public static StaticLookup Active { get { return new StaticLookup("Active", 1); } }
		public static StaticLookup Pending { get { return new StaticLookup("Pending", 2); } }
		public static StaticLookup Completed { get { return new StaticLookup("Completed", 4); } }
		public static StaticLookup Draft { get { return new StaticLookup("Draft", 5); } }
		public static StaticLookup Canceled { get { return new StaticLookup("Canceled", 6); } }
		public static StaticLookup Other { get { return new StaticLookup("Other", 7); } }
	}
}
#endregion

#region ProgramType
namespace ECA.Data
{
	public partial class ProgramType
	{
		//SELECT * FROM ProgramType
		public static StaticLookup Program { get { return new StaticLookup("Program", 1); } }
		public static StaticLookup Office { get { return new StaticLookup("Office", 2); } }
		public static StaticLookup Branch { get { return new StaticLookup("Branch", 3); } }
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
