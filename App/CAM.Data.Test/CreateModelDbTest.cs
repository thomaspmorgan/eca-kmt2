using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using CAM.Data;

namespace ECA.Data.Test
{
    [TestClass]
    public class CreateModelDbTest
    {
        //[TestMethod]
        public void TestCreateDatabase()
        {
            using (var context = new CamModel(@"Data Source=(LocalDb)\v11.0;Initial Catalog=TestCam;Integrated Security=True"))
            {
                Database.SetInitializer<CamModel>(new DropCreateDatabaseAlways<CamModel>());
                context.Database.Initialize(true);
            }
        }
    }
}
