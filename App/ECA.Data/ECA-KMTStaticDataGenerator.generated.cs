




#region ProjectStatus
namespace ECA.Data
{
	using ECA.Core.Generation;
	public partial class ProjectStatus
	{
		///<summary>
		/// Returns the Active lookup with id 1.
		///</summary>
		public static StaticLookup Active { get { return new StaticLookup("Active", 1); } }
		///<summary>
		/// Returns the Pending lookup with id 2.
		///</summary>
		public static StaticLookup Pending { get { return new StaticLookup("Pending", 2); } }
		///<summary>
		/// Returns the Completed lookup with id 4.
		///</summary>
		public static StaticLookup Completed { get { return new StaticLookup("Completed", 4); } }
		///<summary>
		/// Returns the Draft lookup with id 5.
		///</summary>
		public static StaticLookup Draft { get { return new StaticLookup("Draft", 5); } }
		///<summary>
		/// Returns the Canceled lookup with id 6.
		///</summary>
		public static StaticLookup Canceled { get { return new StaticLookup("Canceled", 6); } }
		///<summary>
		/// Returns the Other lookup with id 7.
		///</summary>
		public static StaticLookup Other { get { return new StaticLookup("Other", 7); } }
	}
}
#endregion

#region SocialMediaType
namespace ECA.Data
{
	using ECA.Core.Generation;
	public partial class SocialMediaType
	{
		///<summary>
		/// Returns the Facebook lookup with id 1.
		///</summary>
		public static StaticLookup Facebook { get { return new StaticLookup("Facebook", 1); } }
		///<summary>
		/// Returns the LinkedIn lookup with id 2.
		///</summary>
		public static StaticLookup Linkedin { get { return new StaticLookup("LinkedIn", 2); } }
		///<summary>
		/// Returns the Twitter lookup with id 3.
		///</summary>
		public static StaticLookup Twitter { get { return new StaticLookup("Twitter", 3); } }
		///<summary>
		/// Returns the Weibo lookup with id 4.
		///</summary>
		public static StaticLookup Weibo { get { return new StaticLookup("Weibo", 4); } }
	}
}
#endregion

#region ProgramType
namespace ECA.Data
{
	using ECA.Core.Generation;
	public partial class ProgramType
	{
		///<summary>
		/// Returns the Program lookup with id 1.
		///</summary>
		public static StaticLookup Program { get { return new StaticLookup("Program", 1); } }
		///<summary>
		/// Returns the Office lookup with id 2.
		///</summary>
		public static StaticLookup Office { get { return new StaticLookup("Office", 2); } }
		///<summary>
		/// Returns the Branch lookup with id 3.
		///</summary>
		public static StaticLookup Branch { get { return new StaticLookup("Branch", 3); } }
	}
}
#endregion

