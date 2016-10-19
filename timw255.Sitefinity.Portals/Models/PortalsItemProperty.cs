using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using timw255.Sitefinity.Portals.Attributes;

namespace timw255.Sitefinity.Portals.Models
{
    public class PortalsItemProperty
    {
        [IgnoreDataMember]
        public Type PropertyType { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        [IgnoreDataMember]
        public DashboardConfigurable DashboardConfigurableAttribute { get; set; }
    }
}
