
using System.Collections.Generic;

namespace ECA.Business.Queries.Models.Admin
{
    public class SnapshotGraphDTO
    {
        public string key { get; set; }

        public List<KeyValuePair<int, int>> values { get; set; }
    }
}
