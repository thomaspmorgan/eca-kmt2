﻿using ECA.Business.Queries.Models.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Validation.Sevis
{
    public interface IBiographical
    {
        BiographicalDTO Biography { get; set; }
    }
}
