using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace timw255.Sitefinity.Portals.MVC.Models
{
    public class DashboardItemViewModel
    {
        public string Title { get; set; }

        public string ControllerType { get; set; }

        public List<PortalsItemPropertyViewModel> Properties { get; set; }

        public string Html { get; set; }
    }
}
