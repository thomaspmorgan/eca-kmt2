using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data.Enums
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
        Program,
        Office,
        Branch
    }

    public enum ProjectType
    {
        ProjectType1,
        ProjectType2
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
