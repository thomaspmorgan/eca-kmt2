using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models
{
    public class FocusCategoryDTO
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string FocusName { get; set; }
    }
}