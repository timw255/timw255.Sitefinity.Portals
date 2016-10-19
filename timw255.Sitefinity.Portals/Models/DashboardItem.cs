using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace timw255.Sitefinity.Portals.Models
{
    public class DashboardItem
    {
        public string Title { get; set; }
        
        public string ControllerType { get; set; }

        public List<PortalsItemProperty> Properties { get; set; }

        public DashboardItem()
        {
            Properties = new List<PortalsItemProperty>();
        }
    }
}
