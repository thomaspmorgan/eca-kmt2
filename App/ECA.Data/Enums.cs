using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    public enum ActorType
    {
        Person,
        Organization
    }

    public enum NameType
    {
        GivenName,
        FamilyName,
        MiddleName,
        Patronym,
        Alias,
    }

    public enum Gender
    {
        Male,
        Female,
        Other,
        NotSpecified
    }

    public enum SocialMediaType
    {
        Facebook,
        LinkedIn,
        Twitter,
        Weibo
    }

    public enum MoneyFlowType
    {
        Inbound,
        Outbound
    }

    public enum MoneyFlowStatus
    {
        Projected,
        Actual
    }

    public enum ItineraryStatus
    {
        Planned,
        InProgress,
        Completed
    }

    public enum PersonStatus
    {
        Ticketed,
        Planned,
        Completed,
        EarlyReturn
    }

    public enum ProgramType
    {
        ProgramType1,
        ProgramType2,
        ProgramType3
    }

    public enum ProjectType
    {
        ProjectType1,
        ProjectType2
    }

    public enum ProjectStatus
    {
        Planning,
        InProgress,
        Complete
    }

    public enum Method
    {
        Bus,
        Plane,
        Train,
        Taxi
    }

    public enum PhoneNumberType
    {
        Home,
        Work,
        Cell
    }

}
