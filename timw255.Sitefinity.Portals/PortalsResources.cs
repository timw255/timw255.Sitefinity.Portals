using System;
using System.Linq;
using Telerik.Sitefinity.Localization;
using Telerik.Sitefinity.Localization.Data;

namespace timw255.Sitefinity.Portals
{
    /// <summary>
    /// Localizable strings for the Portals module
    /// </summary>
    /// <remarks>
    /// You can use Sitefinity Thunder to edit this file.
    /// To do this, open the file's context menu and select Edit with Thunder.
    /// 
    /// If you wish to install this as a part of a custom module,
    /// add this to the module's Initialize method:
    /// App.WorkWith()
    ///     .Module(ModuleName)
    ///     .Initialize()
    ///         .Localization<PortalsResources>();
    /// </remarks>
    /// <see cref="http://www.sitefinity.com/documentation/documentationarticles/developers-guide/how-to/how-to-import-events-from-facebook/creating-the-resources-class"/>
    [ObjectInfo("PortalsResources", ResourceClassId = "PortalsResources", Title = "PortalsResourcesTitle", TitlePlural = "PortalsResourcesTitlePlural", Description = "PortalsResourcesDescription")]
    public class PortalsResources : Resource
    {
        #region Construction
        /// <summary>
        /// Initializes new instance of <see cref="PortalsResources"/> class with the default <see cref="ResourceDataProvider"/>.
        /// </summary>
        public PortalsResources()
        {
        }

        /// <summary>
        /// Initializes new instance of <see cref="PortalsResources"/> class with the provided <see cref="ResourceDataProvider"/>.
        /// </summary>
        /// <param name="dataProvider"><see cref="ResourceDataProvider"/></param>
        public PortalsResources(ResourceDataProvider dataProvider)
            : base(dataProvider)
        {
        }
        #endregion

        #region Class Description
        /// <summary>
        /// Portals Resources
        /// </summary>
        [ResourceEntry("PortalsResourcesTitle",
            Value = "Portals module labels",
            Description = "The title of this class.",
            LastModified = "2016/05/17")]
        public string PortalsResourcesTitle
        {
            get
            {
                return this["PortalsResourcesTitle"];
            }
        }

        /// <summary>
        /// Portals Resources Title plural
        /// </summary>
        [ResourceEntry("PortalsResourcesTitlePlural",
            Value = "Portals module labels",
            Description = "The title plural of this class.",
            LastModified = "2016/05/17")]
        public string PortalsResourcesTitlePlural
        {
            get
            {
                return this["PortalsResourcesTitlePlural"];
            }
        }

        /// <summary>
        /// Contains localizable resources for Portals module.
        /// </summary>
        [ResourceEntry("PortalsResourcesDescription",
            Value = "Contains localizable resources for Portals module.",
            Description = "The description of this class.",
            LastModified = "2016/05/17")]
        public string PortalsResourcesDescription
        {
            get
            {
                return this["PortalsResourcesDescription"];
            }
        }
        #endregion

        #region UserDashboardDatas
        /// <summary>
        /// Messsage: PageTitle
        /// </summary>
        /// <value>Title for the UserDashboardData's page.</value>
        [ResourceEntry("UserDashboardDataGroupPageTitle",
            Value = "UserDashboardData",
            Description = "The title of UserDashboardData's page.",
            LastModified = "2016/05/17")]
        public string UserDashboardDataGroupPageTitle
        {
            get
            {
                return this["UserDashboardDataGroupPageTitle"];
            }
        }

        /// <summary>
        /// Messsage: PageDescription
        /// </summary>
        /// <value>Description for the UserDashboardData's page.</value>
        [ResourceEntry("UserDashboardDataGroupPageDescription",
            Value = "UserDashboardData",
            Description = "The description of UserDashboardData's page.",
            LastModified = "2016/05/17")]
        public string UserDashboardDataGroupPageDescription
        {
            get
            {
                return this["UserDashboardDataGroupPageDescription"];
            }
        }

        /// <summary>
        /// The URL name of UserDashboardData's page.
        /// </summary>
        [ResourceEntry("UserDashboardDataGroupPageUrlName",
            Value = "Portals-UserDashboardData",
            Description = "The URL name of UserDashboardData's page.",
            LastModified = "2016/05/17")]
        public string UserDashboardDataGroupPageUrlName
        {
            get
            {
                return this["UserDashboardDataGroupPageUrlName"];
            }
        }


        /// <summary>
        /// Message displayed to user on empty page
        /// </summary>
        /// <value>Products empty page</value>
        [ResourceEntry("UserDashboardDataEmptyPageMessage",
            Value = "UserDashboardData empty page",
            Description = "Message displayed to user on empty page",
            LastModified = "2016/05/17")]
        public string UserDashboardDataEmptyPageMessage
        {
            get
            {
                return this["UserDashboardDataEmptyPageMessage"];
            }
        }

        /// <summary>
        /// The URL name of UserDashboardData's page.
        /// </summary>
        /// <value>UserDashboardDataMasterPageUrl</value>
        [ResourceEntry("UserDashboardDataMasterPageUrl",
            Value = "UserDashboardDataMasterPageUrl",
            Description = "The URL name of UserDashboardData's page.",
            LastModified = "2016/05/17")]
        public string UserDashboardDataMasterPageUrl
        {
            get
            {
                return this["UserDashboardDataMasterPageUrl"];
            }
        }
        #endregion
    }
}