using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;


namespace ECA.Data
{
    public class EcaInitializer : CreateDatabaseIfNotExists<EcaContext>
    {
        protected override void Seed(EcaContext context)
        {
            base.Seed(context);
            //var programs = new List<Program>
            //{
            //    new Program{Name="Program 1", Description="Description 1", StartDate=DateTime.Parse("2014-1-1"), EndDate=DateTime.Parse("2014-12-31")},
            //    new Program{Name="Program 2", Description="Description 2", StartDate=DateTime.Parse("2014-2-1"), EndDate=DateTime.Parse("2014-11-31")},
            //};
            //programs.ForEach(p => context.Programs.Add(p));
            //context.SaveChanges();

        }
    }
}
