using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;

namespace ECA.Data.Test
{
    [TestClass]
    public class CreateModelDbTest
    {
        //[TestMethod]
        public void TestCreateDatabase()
        {
            using (var context = new EcaContext(@"Data Source=(LocalDb)\v11.0;Initial Catalog=Test;Integrated Security=True"))
            {
                Database.SetInitializer<EcaContext>(new DropCreateDatabaseAlways<EcaContext>());
                context.Database.Initialize(true);

            }
        }
    }
}
