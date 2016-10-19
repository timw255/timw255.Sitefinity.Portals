using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace timw255.Sitefinity.Portals.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class DashboardConfigurable : Attribute
    {
        public string DisplayName { get; set; }

        public string BackingProperty { get; set; }
    }
}
