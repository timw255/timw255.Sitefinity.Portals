using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace timw255.Sitefinity.Portals.Models
{
    public class Dashboard
    {
        public Guid DashboardId { get; set; }

        public List<DashboardItem> DashboardItems { get; set; }

        public Dashboard()
        {
            DashboardItems = new List<DashboardItem>();
        }
    }
}
