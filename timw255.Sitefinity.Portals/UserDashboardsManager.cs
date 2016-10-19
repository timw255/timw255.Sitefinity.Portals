using System;
using System.Linq;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Data;
using timw255.Sitefinity.Portals.Configuration;
using timw255.Sitefinity.Portals.Models;

namespace timw255.Sitefinity.Portals
{
    public class UserDashboardsManager : ManagerBase<UserDashboardsDataProvider>
    {
        #region Construction
        /// <summary>
        /// Initializes a new instance of the <see cref="UserDashboardsManager" /> class.
        /// </summary>
        public UserDashboardsManager()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDashboardsManager" /> class.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        public UserDashboardsManager(string providerName)
            : base(providerName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDashboardsManager" /> class.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="transactionName">Name of the transaction.</param>
        public UserDashboardsManager(string providerName, string transactionName)
            : base(providerName, transactionName)
        {
        }
        #endregion

        #region Public and overriden methods
        /// <summary>
        /// Gets the default provider delegate.
        /// </summary>
        /// <value>The default provider delegate.</value>
        protected override GetDefaultProvider DefaultProviderDelegate
        {
            get
            {
                return () => Config.Get<PortalsConfig>().DefaultProvider;
            }
        }

        /// <summary>
        /// Gets the name of the module.
        /// </summary>
        /// <value>The name of the module.</value>
        public override string ModuleName
        {
            get
            {
                return PortalsModule.ModuleName;
            }
        }

        /// <summary>
        /// Gets the providers settings.
        /// </summary>
        /// <value>The providers settings.</value>
        protected override ConfigElementDictionary<string, DataProviderSettings> ProvidersSettings
        {
            get
            {
                return Config.Get<PortalsConfig>().Providers;
            }
        }

        /// <summary>
        /// Get an instance of the Portals manager using the default provider.
        /// </summary>
        /// <returns>Instance of the Portals manager</returns>
        public static UserDashboardsManager GetManager()
        {
            return ManagerBase<UserDashboardsDataProvider>.GetManager<UserDashboardsManager>();
        }

        /// <summary>
        /// Get an instance of the Portals manager by explicitly specifying the required provider to use
        /// </summary>
        /// <param name="providerName">Name of the provider to use, or null/empty string to use the default provider.</param>
        /// <returns>Instance of the Portals manager</returns>
        public static UserDashboardsManager GetManager(string providerName)
        {
            return ManagerBase<UserDashboardsDataProvider>.GetManager<UserDashboardsManager>(providerName);
        }

        /// <summary>
        /// Get an instance of the Portals manager by explicitly specifying the required provider to use
        /// </summary>
        /// <param name="providerName">Name of the provider to use, or null/empty string to use the default provider.</param>
        /// <param name="transactionName">Name of the transaction.</param>
        /// <returns>Instance of the Portals manager</returns>
        public static UserDashboardsManager GetManager(string providerName, string transactionName)
        {
            return ManagerBase<UserDashboardsDataProvider>.GetManager<UserDashboardsManager>(providerName, transactionName);
        }

        /// <summary>
        /// Creates a UserDashboardData.
        /// </summary>
        /// <returns>The created UserDashboardData.</returns>
        public UserDashboardData CreateUserDashboardData()
        {
            return this.Provider.CreateUserDashboardData();
        }

        /// <summary>
        /// Updates the UserDashboardData.
        /// </summary>
        /// <param name="entity">The UserDashboardData entity.</param>
        public void UpdateUserDashboardData(UserDashboardData entity)
        {
            this.Provider.UpdateUserDashboardData(entity);
        }

        /// <summary>
        /// Deletes the UserDashboardData.
        /// </summary>
        /// <param name="entity">The UserDashboardData entity.</param>
        public void DeleteUserDashboardData(UserDashboardData entity)
        {
            this.Provider.DeleteUserDashboardData(entity);
        }

        /// <summary>
        /// Gets the UserDashboardData by a specified ID.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <returns>The UserDashboardData.</returns>
        public UserDashboardData GetUserDashboardData(Guid id)
        {
            return this.Provider.GetUserDashboardData(id);
        }

        /// <summary>
        /// Gets a query of all the UserDashboardData items.
        /// </summary>
        /// <returns>The UserDashboardData items.</returns>
        public IQueryable<UserDashboardData> GetUserDashboardDatas()
        {
            return this.Provider.GetUserDashboardDatas();
        }
        #endregion
    }
}