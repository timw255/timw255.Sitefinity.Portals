using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace timw255.Sitefinity.Portals.Models
{
    public class DashboardToolboxItem
    {
        public string ControllerName { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string ThumbnailUrl { get; set; }

        public List<PortalsItemProperty> Properties { get; set; }

        public DashboardToolboxItem()
        {
            Properties = new List<PortalsItemProperty>();
        }
    }
}
