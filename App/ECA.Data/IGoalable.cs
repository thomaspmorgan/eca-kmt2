using System;
namespace ECA.Data
{
    public interface IGoalable
    {
        System.Collections.Generic.ICollection<Goal> Goals { get; set; }
    }
}
