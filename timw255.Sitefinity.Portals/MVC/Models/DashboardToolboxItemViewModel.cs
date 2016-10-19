using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace timw255.Sitefinity.Portals.MVC.Models
{
    public class DashboardToolboxItemViewModel
    {
        public string ControllerType { get; set; }

        public string Title { get; set; }

        public string ThumbnailUrl { get; set; }

        public List<PortalsItemPropertyViewModel> Properties { get; set; }
    }
}
