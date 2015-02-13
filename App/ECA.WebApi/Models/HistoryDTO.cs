using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models
{
    public class HistoryDTO
    {
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public int RevisedBy { get; set; }
        public DateTimeOffset RevisedOn { get; set; }
    }
}