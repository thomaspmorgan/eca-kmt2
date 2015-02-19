




#region LocationType
namespace ECA.Data
{
	public partial class LocationType
	{
		//SELECT * FROM LocationType
		public static StaticLookup Home { get { return new StaticLookup("Home", 1); } }
		public static StaticLookup Away { get { return new StaticLookup("Away", 2); } }
		public static StaticLookup FarAway { get { return new StaticLookup("Far Away", 3); } }
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
