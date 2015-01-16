using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECA.Data;
using System.Data.Entity;

namespace ECA.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new ECA.Data.EcaContext())
            {
                System.Console.Write("Enter a org type: ");
                var name = System.Console.ReadLine();
                var history = new History();
                history.CreatedBy = 0;
                history.CreatedOn = DateTime.Now;
                history.RevisedBy = 0;
                history.RevisedOn = DateTime.Now;
                var orgType = new OrganizationType { OrganizationTypeId = 1, OrganizationTypeName = name, History = history };
                db.OrganizationTypes.Add(orgType);
                db.SaveChanges();
                System.Console.ReadKey();
            }
        }
    }
}
