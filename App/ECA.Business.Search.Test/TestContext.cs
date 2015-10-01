using ECA.Core.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Search.Test
{
    public class SimpleEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }

    public class SimpleEntityConfiguration : DocumentConfiguration<SimpleEntity, int>
    {
        public SimpleEntityConfiguration()
        {
            HasKey(x => x.Id);
            HasName(x => x.Name);
            HasDescription(x => x.Description);
        }
    }

    public class TestContext : DbContext
    {
        public TestContext()
        {
            this.SimpleEntities = new TestDbSet<SimpleEntity>();
        }

        public TestDbSet<SimpleEntity> SimpleEntities { get; set; }
    }
}
