using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using System.Security.Claims;
using System.Threading;

namespace ECA.WebApi.Common
{
    public class CurrentUser
    {
        public string UserId { get; private set; }
        public string UserName { get; private set; }
        public string DisplayName { get; private set; }
        public string AlternateEmail { get; private set; }
        public string JobTitle { get; private set; }
        public string[] MissionCodes { get; private set; }
        public string MissionNames { get; private set; }

        public string GroupName
        {
            get
            {
                if (IsGlobalAdministrator) {
                    return SecurityGroups.GlobalAdministrator;
                }
                if (IsPostAdministrator) {
                    return SecurityGroups.PostAdministrator;
                }
                if (IsApprover) {
                    return SecurityGroups.Approver;
                }
                if (IsStandardUser) {
                    return SecurityGroups.StandardUser;
                }
                if (IsGuest) {
                    return SecurityGroups.Guest;
                }
                return null;
            }
        }

        public int GroupLevel { get { return SecurityGroups.Level(GroupName); } }

        public bool IsGuest { get { return IsMemberOf(SecurityGroups.Guest); } }
        public bool IsStandardUser { get { return IsMemberOf(SecurityGroups.StandardUser); } }
        public bool IsApprover { get { return IsMemberOf(SecurityGroups.Approver); } }
        public bool IsPostAdministrator { get { return IsMemberOf(SecurityGroups.PostAdministrator); } }
        public bool IsGlobalAdministrator { get { return IsMemberOf(SecurityGroups.GlobalAdministrator); } }

        public bool IsAnonymous { get { return UserId == string.Empty; } }

        public static CurrentUser SignedInUser
        {
            get
            {
                var retval = new CurrentUser {
                    MissionCodes = new string[] { },
                    MissionNames = string.Empty
                };
                var principal = ClaimsPrincipal.Current;
                var principal1 = Thread.CurrentPrincipal.Identity;
                if (principal != null) {
                    var identity = principal.Identity;
                    if (identity != null && identity.IsAuthenticated) {
                        var userId = principal.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
                        var conn = AdGraph.Instance.Connection;
                        var user = conn.Get<User>(userId);
                        retval._groupNames = SecurityGroups.Instance.GetMemberGroupNames(user);
                        retval.UserId = user.ObjectId;
                        retval.UserName = user.UserPrincipalName;
                        retval.DisplayName = user.DisplayName;
                        retval.AlternateEmail = user.OtherMails.FirstOrDefault();
                        retval.JobTitle = user.JobTitle;

                        //retval._groupNames = new List<string> { SecurityGroups.StandardUser };
                        //retval.UserId = userId;
                        //retval.UserName = "gadmin";
                        //retval.DisplayName = "General Admin";
                        //retval.AlternateEmail = "gadmin@statedept.us";
                        //retval.JobTitle = "Head Honcho";
                    }
                }
                return retval;
            }
        }

        public static CurrentUser OpenNetUser
        {
            get
            {
                return new CurrentUser {
                    UserId = string.Empty,
                    UserName = SecurityGroups.Guest.ToLower(),
                    DisplayName = SecurityGroups.Guest,
                    AlternateEmail = string.Empty,
                    JobTitle = SecurityGroups.Guest,
                    MissionCodes = new string[] { },
                    _groupNames = new List<string> { SecurityGroups.Guest },
                    _missionNames = new string[] { }
                };
            }
        }

        private IList<string> _groupNames;
        private string[] _missionNames;

        // The department field in Active Directory is used for mission codes.
        // Multiple mission codes can be specified by separating them with any
        // character that is not a valid mission code character. Valid mission
        // code characters are letters, numbers, and the hyphen. The hyphen is
        // included so that missions such as USUN-GENEVA can be specified.
        private static Regex _missionCodeSeparator = new Regex("[^A-Z0-9-]+", RegexOptions.IgnoreCase);

        private bool IsMemberOf(string groupName)
        {
            return _groupNames != null && _groupNames.Contains(groupName);
        }


    }
}