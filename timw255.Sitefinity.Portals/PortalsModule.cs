using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Sitefinity;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Abstractions.VirtualPath;
using Telerik.Sitefinity.Abstractions.VirtualPath.Configuration;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.Fluent.Modules;
using Telerik.Sitefinity.Fluent.Modules.Toolboxes;
using Telerik.Sitefinity.Modules.Pages.Configuration;
using Telerik.Sitefinity.Services;
using Telerik.Sitefinity.Web.UI;
using timw255.Sitefinity.Portals.Configuration;
using timw255.Sitefinity.Portals.Web.UI.UserDashboardDatas;
using System.Web.Http;
using Telerik.Sitefinity.Mvc.Store;
using Telerik.Sitefinity.Mvc.Proxy;
using timw255.Sitefinity.Portals.MVC.Controllers;

namespace timw255.Sitefinity.Portals
{
    /// <summary>
    /// Custom Sitefinity content module 
    /// </summary>
    public class PortalsModule : ModuleBase
    {
        #region Properties
        /// <summary>
        /// Gets the landing page id for the module.
        /// </summary>
        /// <value>The landing page id.</value>
        public override Guid LandingPageId
        {
            get
            {
                return PortalsModule.UserDashboardDataHomePageId;
            }
        }

        /// <summary>
        /// Gets the CLR types of all data managers provided by this module.
        /// </summary>
        /// <value>An array of <see cref="T:System.Type" /> objects.</value>
        public override Type[] Managers
        {
            get
            {
                return PortalsModule.managerTypes;
            }
        }
        #endregion

        #region Module Initialization
        /// <summary>
        /// Initializes the service with specified settings.
        /// This method is called every time the module is initializing (on application startup by default)
        /// </summary>
        /// <param name="settings">The settings.</param>
        public override void Initialize(ModuleSettings settings)
        {
            base.Initialize(settings);

            // Add your initialization logic here

            App.WorkWith()
                .Module(settings.Name)
                    .Initialize()
                    .Localization<PortalsResources>()
                    .Configuration<PortalsConfig>();

            Config.RegisterSection<PortalsConfig>();

            GlobalConfiguration.Configuration.Routes.MapHttpRoute(
                name: "Portals",
                routeTemplate: "Portals/{controller}/{action}"
            );

            // Here is also the place to register to some Sitefinity specific events like Bootstrapper.Initialized or subscribe for an event in with the EventHub class            
            // Please refer to the documentation for additional information http://www.sitefinity.com/documentation/documentationarticles/developers-guide/deep-dive/sitefinity-event-system/ieventservice-and-eventhub
        }

        /// <summary>
        /// Installs this module in Sitefinity system for the first time.
        /// </summary>
        /// <param name="initializer">The Site Initializer. A helper class for installing Sitefinity modules.</param>
        public override void Install(SiteInitializer initializer)
        {
            this.InstallVirtualPaths(initializer);
            this.InstallBackendPages(initializer);
            this.InstallPageWidgets(initializer);
        }

        /// <summary>
        /// Upgrades this module from the specified version.
        /// This method is called instead of the Install method when the module is already installed with a previous version.
        /// </summary>
        /// <param name="initializer">The Site Initializer. A helper class for installing Sitefinity modules.</param>
        /// <param name="upgradeFrom">The version this module us upgrading from.</param>
        public override void Upgrade(SiteInitializer initializer, Version upgradeFrom)
        {
            // Here you can check which one is your prevous module version and execute some code if you need to
            // See the example bolow
            //
            //if (upgradeFrom < new Version("1.0.1.0"))
            //{
            //    some upgrade code that your new version requires
            //}
        }

        /// <summary>
        /// Uninstalls the specified initializer.
        /// </summary>
        /// <param name="initializer">The initializer.</param>
        public override void Uninstall(SiteInitializer initializer)
        {
            base.Uninstall(initializer);
            // Add your uninstall logic here
        }
        #endregion

        #region Public and overriden methods
        /// <summary>
        /// Gets the module configuration.
        /// </summary>
        protected override ConfigSection GetModuleConfig()
        {
            return Config.Get<PortalsConfig>();
        }
        #endregion

        #region Virtual paths
        /// <summary>
        /// Installs module virtual paths.
        /// </summary>
        /// <param name="initializer">The initializer.</param>
        private void InstallVirtualPaths(SiteInitializer initializer)
        {
            // Here you can register your module virtual paths

            var virtualPaths = initializer.Context.GetConfig<VirtualPathSettingsConfig>().VirtualPaths;
            var moduleVirtualPath = PortalsModule.ModuleVirtualPath + "*";
            if (!virtualPaths.ContainsKey(moduleVirtualPath))
            {
                virtualPaths.Add(new VirtualPathElement(virtualPaths)
                {
                    VirtualPath = moduleVirtualPath,
                    ResolverName = "EmbeddedResourceResolver",
                    ResourceLocation = typeof(PortalsModule).Assembly.GetName().Name
                });
            }
        }
        #endregion

        #region Install backend pages
        /// <summary>
        /// Installs the backend pages.
        /// </summary>
        /// <param name="initializer">The initializer.</param>
        private void InstallBackendPages(SiteInitializer initializer)
        {
            // Using our Fluent Api you can add your module backend pages hierarchy in no time
            // Here is an example using resources to localize the title of the page and adding a dummy control
            // to the module page.

            initializer.Installer
                .CreateModuleGroupPage(PortalsModule.UserDashboardDataGroupPageId, "UserDashboardData")
                    .PlaceUnder(CommonNode.TypesOfContent)
                    .LocalizeUsing<PortalsResources>()
                    .SetTitleLocalized("UserDashboardDataGroupPageTitle")
                    .SetUrlNameLocalized("UserDashboardDataGroupPageUrlName")
                    .SetDescriptionLocalized("UserDashboardDataGroupPageDescription")
                    .ShowInNavigation()
                    .AddChildPage(PortalsModule.UserDashboardDataHomePageId, "UserDashboardData")
                        .LocalizeUsing<PortalsResources>()
                        .SetTitleLocalized("UserDashboardDataGroupPageTitle")
                        .SetHtmlTitleLocalized("UserDashboardDataGroupPageTitle")
                        .SetUrlNameLocalized("UserDashboardDataMasterPageUrl")
                        .SetDescriptionLocalized("UserDashboardDataGroupPageDescription")
                        .AddControl(new UserDashboardDatasEmptyPage())
                        .HideFromNavigation()
                    .Done()
                .Done();
        }
        #endregion

        #region Widgets
        /// <summary>
        /// Installs the form widgets.
        /// </summary>
        /// <param name="initializer">The initializer.</param>
        private void InstallFormWidgets(SiteInitializer initializer)
        {
            // Here you can register your custom form widgets in the toolbox.
            // See the example below.

            //string moduleFormWidgetSectionName = "Portals";
            //string moduleFormWidgetSectionTitle = "Portals";
            //string moduleFormWidgetSectionDescription = "Portals";

            //initializer.Installer
            //    .Toolbox(CommonToolbox.FormWidgets)
            //        .LoadOrAddSection(moduleFormWidgetSectionName)
            //            .SetTitle(moduleFormWidgetSectionTitle)
            //            .SetDescription(moduleFormWidgetSectionDescription)
            //            .LoadOrAddWidget<WidgetType>(WidgetNameForDevelopers)
            //                .SetTitle(WidgetTitle)
            //                .SetDescription(WidgetDescription)
            //                .LocalizeUsing<PortalsResources>()
            //                .SetCssClass(WidgetCssClass) // You can use a css class to add an icon (this is optional)
            //            .Done()
            //        .Done()
            //    .Done();
        }

        /// <summary>
        /// Installs the layout widgets.
        /// </summary>
        /// <param name="initializer">The initializer.</param>
        private void InstallLayoutWidgets(SiteInitializer initializer)
        {
            // Here you can register your custom layout widgets in the toolbox.
            // See the example below.

            //string moduleLayoutWidgetSectionName = "Portals";
            //string moduleLayoutWidgetSectionTitle = "Portals";
            //string moduleLayoutWidgetSectionDescription = "Portals";

            //initializer.Installer
            //    .Toolbox(CommonToolbox.Layouts)
            //        .LoadOrAddSection(moduleLayoutWidgetSectionName)
            //            .SetTitle(moduleLayoutWidgetSectionTitle)
            //            .SetDescription(moduleLayoutWidgetSectionDescription)
            //            .LoadOrAddWidget<LayoutControl>(WidgetNameForDevelopers)
            //                .SetTitle(WidgetTitle)
            //                .SetDescription(WidgetDescription)
            //                .LocalizeUsing<PortalsResources>()
            //                .SetCssClass(WidgetCssClass) // You can use a css class to add an icon (Optional)
            //                .SetParameters(new NameValueCollection() 
            //                { 
            //                    { "layoutTemplate", FullPathToTheLayoutWidget },
            //                })
            //            .Done()
            //        .Done()
            //    .Done();
        }

        /// <summary>
        /// Installs the page widgets.
        /// </summary>
        /// <param name="initializer">The initializer.</param>
        private void InstallPageWidgets(SiteInitializer initializer)
        {
            // Here you can register your custom page widgets in the toolbox.
            // See the example below.

            string modulePageWidgetSectionName = "Portals";
            string modulePageWidgetSectionTitle = "Portals";
            string modulePageWidgetSectionDescription = "Portals";

            initializer.Installer
                .Toolbox(CommonToolbox.PageWidgets)
                    .LoadOrAddSection(modulePageWidgetSectionName)
                        .SetTitle(modulePageWidgetSectionTitle)
                        .SetDescription(modulePageWidgetSectionDescription)
                        .LoadOrAddWidget<MvcControllerProxy>("Dashboard")
                            .SetTitle("Dashboard")
                            .SetDescription("Dashboard")
                            .SetCssClass("prtlDashboard")
                            .SetParameters(new NameValueCollection() 
                            { 
                                { "controllerType", typeof(DashboardController).FullName },
                                { "ControllerName", typeof(DashboardController).FullName },
                                { "moduleName", ModuleName }
                            })
                        .Done()
                        .LoadOrAddWidget<MvcControllerProxy>("DashboardToolbox")
                            .SetTitle("Dashboard Toolbox")
                            .SetDescription("Dashboard Toolbox")
                            .SetCssClass("prtlDashboardToolbox")
                            .SetParameters(new NameValueCollection() 
                            { 
                                { "controllerType", typeof(DashboardToolboxController).FullName },
                                { "ControllerName", typeof(DashboardToolboxController).FullName },
                                { "moduleName", ModuleName }
                            })
                        .Done()
                    .Done()
                .Done();
        }
        #endregion

        #region Upgrade methods
        #endregion

        #region Private members & constants
        public const string ModuleName = "Portals";
        internal const string ModuleTitle = "Portals";
        internal const string ModuleDescription = "Adds support for user dashboards.";
        internal const string ModuleVirtualPath = "~/Portals/";

        private static readonly Type[] managerTypes = new Type[] { typeof(UserDashboardsManager) };

        // Pages
        internal static readonly Guid UserDashboardDataGroupPageId = new Guid("cbbceea3-47c2-4bc6-9926-da1ed6b3c318");
        internal static readonly Guid UserDashboardDataHomePageId = new Guid("94c3daca-c11b-4bb2-a4e7-1e6ea34f5bfe");
        #endregion
    }
}