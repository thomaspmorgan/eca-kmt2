using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Claims;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using RE = System.Text.RegularExpressions;

namespace ECA.WebApi.Common
{
    public class AdGraph
    {
        private static volatile AdGraph _instance;
        private static object _lock = new object();

        private AuthenticationResult _token = null;
        private GraphConnection _conn = null;

        public static AdGraph Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new AdGraph();
                    }
                }

                return _instance;
            }
        }

        // Creates a new group (if it does not already exist).
        // Returns the group id of the created or existing group.
        public string CreateGroup(string displayName, string description)
        {
            var groupId = GetGroupId(displayName);
            if (groupId == null)
            {
                var group = new Group
                {
                    DisplayName = displayName,
                    Description = description,
                    MailNickname = invalidCharacters.Replace(displayName, ""),
                    MailEnabled = false,
                    SecurityEnabled = true
                };
                groupId = Connection.Add<Group>(group).ObjectId;
            }
            return groupId;
        }

        public string GroupId(string groupName)
        {
            return GetGroupId(groupName);
        }

        public string GetHighestGroupName(User user)
        {
            var names = new List<string>();
            var list = Connection.GetLinkedObjects(user, LinkProperty.MemberOf, null);
            foreach (DirectoryObject obj in list.Results)
            {
                Group group = obj as Group;
                if (group != null)
                {
                    names.Add(group.DisplayName);
                }
            }
            if (names.Any())
            {
                names.Sort((a, b) => SecurityGroups.Level(b).CompareTo(SecurityGroups.Level(a)));
                return names.First();
            }
            else
            {
                return null;
            }
        }

        private AdGraph()
        {
            this._token = null;
            this._conn = null;
        }

        private static AuthenticationResult GetToken()
        {
            var tenant = AppSettings.AdTenantId;
            var clientId = AppSettings.AdClientId;
            var appKey = AppSettings.AdClientSecret;
            var tokenAuthority = "https://login.windows.net/" + tenant;
            var targetResource = "https://graph.windows.net";
            var credential = new ClientCredential(clientId, appKey);
            var context = new AuthenticationContext(tokenAuthority, validateAuthority: false);
            return context.AcquireToken(targetResource, credential);
        }

        private static bool IsExpiring(AuthenticationResult token)
        {
            var exp = token.ExpiresOn.ToUniversalTime().AddMinutes(2);
            var now = DateTimeOffset.Now.ToUniversalTime();
            return exp.CompareTo(now) < 0;
        }

        public GraphConnection Connection
        {
            get
            {
                if (_token == null || IsExpiring(_token))
                {
                    _token = GetToken();
                    _conn = null;
                }
                if (_conn == null)
                {
                    var requestId = Guid.NewGuid();
                    var graphSettings = new GraphSettings();
                    graphSettings.GraphDomainName = "graph.windows.net";
                    graphSettings.ApiVersion = "2013-11-08";
                    _conn = new GraphConnection(_token.AccessToken, requestId, graphSettings);
                }
                return _conn;
            }
        }

        // Converts a group display name to a group id.
        // Returns null if the group does not exist.
        private string GetGroupId(string displayName)
        {
            var groups = Connection.List<Group>(null, null);
            if (groups == null || groups.Results == null)
            {
                return null;
            }
            var group = groups.Results.FirstOrDefault(g => g.DisplayName == displayName);
            if (group == null)
            {
                return null;
            }
            return group.ObjectId;
        }

        private string GetUserId(ClaimsPrincipal principal = null)
        {
            if (principal == null)
            {
                principal = ClaimsPrincipal.Current;
            }
            if (principal == null)
            {
                throw new Exception("Not authenticated");
            }
            var claim = principal.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier");
            if (claim == null)
            {
                throw new Exception("Claim not found");
            }
            return claim.Value;
        }

        // Used when creating the mail nickname.
        // Only letters and digits are allowed.
        private static RE.Regex invalidCharacters = new RE.Regex(@"[^A-Za-z0-9]+");
    }
}