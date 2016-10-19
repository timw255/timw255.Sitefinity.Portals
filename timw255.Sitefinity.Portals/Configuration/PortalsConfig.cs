using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Localization;
using Telerik.Sitefinity.Modules.GenericContent.Configuration;
using Telerik.Sitefinity.Web.Configuration;
using timw255.Sitefinity.Portals.Data.EntityFramework;

namespace timw255.Sitefinity.Portals.Configuration
{
    /// <summary>
    /// Represents the configuration section for Portals module.
    /// </summary>
    [ObjectInfo(Title = "Portals Config Title", Description = "Portals Config Description")]
    public class PortalsConfig : ModuleConfigBase
    {
        /// <summary>
        /// Gets or sets the name of the default data provider. 
        /// </summary>
        [DescriptionResource(typeof(ConfigDescriptions), "DefaultProvider")]
        [ConfigurationProperty("defaultProvider", DefaultValue = "UserDashboardsEFDataProvider")]
        public override string DefaultProvider
        {
            get
            {
                return (string)this["defaultProvider"];
            }
            set
            {
                this["defaultProvider"] = value;
            }
        }

        #region Public and overriden methods
        protected override void InitializeDefaultProviders(ConfigElementDictionary<string, DataProviderSettings> providers)
        {
            providers.Add(new DataProviderSettings(providers)
            {
                Name = "UserDashboardsEFDataProvider",
                Title = "Default Portals",
                Description = "A provider that stores portal data in database using Entity Framework.",
                ProviderType = typeof(UserDashboardsEFDataProvider),
                Parameters = new NameValueCollection() { { "applicationName", "/Portals" } }
            });
        }
        #endregion
    }
}