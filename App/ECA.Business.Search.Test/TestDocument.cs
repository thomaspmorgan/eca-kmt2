using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Search.Test
{
    public class OtherTestDocument
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Subtitle { get; set; }

        public string AdditionalField { get; set; }
    }

    public class TestDocument// : IDocumentable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Subtitle { get; set; }

        public string AdditionalField { get; set; }
    }
}
