﻿using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos
{
    public class MetricsFacebookPost : FacebookPost
    {
        public decimal Score { get; set; }
    }
}
