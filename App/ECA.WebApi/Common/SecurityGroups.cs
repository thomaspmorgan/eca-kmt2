using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.ActiveDirectory.GraphClient;

namespace ECA.WebApi.Common
{
    public class SecurityGroups
    {
        public const string Guest = "Guest";
        public const string StandardUser = "Standard User";
        public const string Approver = "Approver";
        public const string PostAdministrator = "Post Administrator";
        public const string GlobalAdministrator = "Global Administrator";

        public static SecurityGroups Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new SecurityGroups();
                    }
                }

                return _instance;
            }
        }

        public bool IsMemberOf(string groupName)
        {
            return GetMemberGroupNames().Contains(groupName);
        }

        public IList<string> GetMemberGroupNames(User user = null)
        {
            //var conn = AdGraph.Instance.Connection;
            //if (user == null)
            //{
            //    var userId = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            //    user = conn.Get<User>(userId);
            //}
            //var list = conn.CheckMemberGroups(user, _groupNames.Keys.ToList());
            //return _groupNames.Where(g => list.Contains(g.Key)).Select(g => g.Value).ToList();
            return null;
        }

        public string GetGroupId(string groupName)
        {
            return _groupIds[groupName];
        }

        public void AssignGroup(string userId, string groupName)
        {
            //var conn = AdGraph.Instance.Connection;
            //var user = conn.Get<User>(userId);
            //var data = conn.GetLinkedObjects(user, LinkProperty.MemberOf, null);
            //if (data != null && data.Results != null)
            //{
            //    foreach (GraphObject obj in data.Results)
            //    {
            //        var group = obj as Group;
            //        if (group != null)
            //        {
            //            conn.DeleteLink(group, user, LinkProperty.Members);
            //        }
            //    }
            //}
            //if (!string.IsNullOrWhiteSpace(groupName))
            //{
            //    var newGroup = conn.Get<Group>(_groupIds[groupName]);
            //    conn.AddLink(newGroup, user, LinkProperty.Members);
            //}
        }

        public static int Level(string groupName)
        {
            switch (groupName)
            {
                case GlobalAdministrator:
                    return 5;
                case PostAdministrator:
                    return 4;
                case Approver:
                    return 3;
                case StandardUser:
                    return 2;
                case Guest:
                    return 1;
                default:
                    return 0;
            }
        }

        private static volatile SecurityGroups _instance;
        private static object _lock = new object();

        // Maps group ids to display names.
        private IDictionary<string, string> _groupNames = null;

        // Maps display names to group ids.
        private IDictionary<string, string> _groupIds = null;

        private SecurityGroups()
        {
            _groupNames = new Dictionary<string, string>();
            _groupIds = new Dictionary<string, string>();
            //var list = AdGraph.Instance.Connection.List<Group>(null, null);
            //if (list != null && list.Results != null)
            //{
            //    foreach (var group in list.Results)
            //    {
            //        _groupNames.Add(group.ObjectId, group.DisplayName);
            //        _groupIds.Add(group.DisplayName, group.ObjectId);
            //    }
            //}
        }
    }
}