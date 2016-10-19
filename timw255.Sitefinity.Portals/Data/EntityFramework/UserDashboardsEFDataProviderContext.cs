using System;
using System.Linq;

namespace timw255.Sitefinity.Portals.Data.EntityFramework
{
    public class UserDashboardsEFDataProviderContext
    {
        #region Properties
        /// <summary>
        /// Gets or sets the provider key.
        /// </summary>
        /// <value>The provider key.</value>
        public string ProviderKey { get; set; }

        /// <summary>
        /// Gets or sets the connection id.
        /// </summary>
        /// <value>The connection id.</value>
        public string ConnectionId { get; set; }
        #endregion
    }
}
