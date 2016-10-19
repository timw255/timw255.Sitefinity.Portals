using System;
using System.Linq;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.Data.Decorators;
using timw255.Sitefinity.Portals.Data.EntityFramework.Decorators;

namespace timw255.Sitefinity.Portals.Data.EntityFramework
{
    [DataProviderDecorator(typeof(UserDashboardsEFDataProviderDecorator))]
    public interface IUserDashboardsEFDataProvider : IDataProviderBase
    {
        #region Methods
        /// <summary>
        /// Gets or sets the provider context.
        /// </summary>
        /// <value>The provider context.</value>
        UserDashboardsEFDataProviderContext ProviderContext { get; set; }

        /// <summary>
        /// Gets the db context.
        /// </summary>
        /// <value>The db context.</value>
        UserDashboardsEFDbContext Context { get; }
        #endregion
    }
}
