﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace timw255.Sitefinity.Portals.MVC.Models
{
    public class DashboardViewModel
    {
        public Guid DashboardId { get; set; }

        public List<DashboardItemViewModel> DashboardItems { get; set; }
    }
}
