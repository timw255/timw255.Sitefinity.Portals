using ServiceStack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure;
using Telerik.Sitefinity.Modules.Pages.Configuration;
using Telerik.Sitefinity.Mvc;
using Telerik.Sitefinity.Mvc.Proxy;
using Telerik.Sitefinity.Services;
using timw255.Sitefinity.Portals.MVC.Models;

namespace timw255.Sitefinity.Portals.MVC.Controllers
{
    public class DashboardController : Controller
    {
        public Guid DashboardId { get; set; }
        
        public ActionResult Index()
        {
            if (SystemManager.IsDesignMode || SystemManager.IsPreviewMode)
            {
                // can't figure out how to assign a value (automatically) so, prompt the user to open the designer
                // actually, doing this gives us the added feature of being able to "tie" dashboards together that exist on different pages. *shrug*
                if (DashboardId == Guid.Empty)
                {
                    return View("EmptyDashboardId");
                }

                // rendering these widgets in design mode would just get messy, best not to do it
                return View("PageEditor");
            }

            // dashboard id is necessary to retrieve the content from UserDashboardsManager so....
            if (DashboardId == Guid.Empty)
            {
                return null;
            }

            var model = GetDashboardModel();

            return View("Default", model);
        }

        public ActionResult Configure()
        {
            if (SystemManager.IsDesignMode || SystemManager.IsPreviewMode)
            {
                // can't figure out how to assign a value (automatically) so, prompt the user to open the designer
                // actually, doing this gives us the benefit of being able to "tie" dashboards together that exist on different pages. *shrug*
                if (DashboardId == Guid.Empty)
                {
                    return View("EmptyDashboardId");
                }

                // rendering these widgets in design mode would just get messy, best not to do it
                return View("PageEditor");
            }

            // dashboard id is necessary to retrieve the content from UserDashboardsManager so....
            if (DashboardId == Guid.Empty)
            {
                return null;
            }

            var model = GetDashboardModel(false);

            return View("Configure", model);
        }

        // pulled from Telerik.Sitefinity.Mvc.Proxy.MvcControllerProxy
        // this is how we get the html output for the dashboard item
        private string ExecuteController(MvcControllerProxy proxy)
        {
            IControllerActionInvoker controllerActionInvoker = ObjectFactory.Resolve<IControllerActionInvoker>();

            proxy.Page = proxy.Page ?? base.HttpContext.CurrentHandler.GetPageHandler();

            string output = string.Empty;
            if (controllerActionInvoker.TryInvokeAction(proxy, out output))
            {
                return output;
            }

            return string.Empty;
        }

        // *sigh* yes....i'm doing this in the controller. Feel free to move it somewhere else. :)
        private DashboardViewModel GetDashboardModel(bool executeControllers = true)
        {
            var dashboardsManager = UserDashboardsManager.GetManager();

            // dashboard data is stored using the user id so, we need to get the user
            var currentUser = PortalsHelpers.GetCurrentUser();

            // get this dashboard's data for the current user
            var dashboard = currentUser.GetDashboard(DashboardId);

            // instantiate a new DashboardViewModel and set the property values that we know we need
            var model = new DashboardViewModel();
            model.DashboardId = DashboardId;
            model.DashboardItems = new List<DashboardItemViewModel>();
            
            // loop through the dashboard items and create view models for them
            foreach (var dashboardItem in dashboard.DashboardItems)
            {
                // get the corresponding Sitefinity ToolboxItem for the given dashboard item type
                ToolboxItem toolboxItem = null;
                if (PortalsHelpers.GetToolboxItem(dashboardItem.ControllerType, out toolboxItem))
                {
                    // MvcControllerProxy is the source of all this sorcery
                    var proxy = new MvcControllerProxy()
                    {
                        ControllerName = toolboxItem.ControllerType
                    };

                    // get the properties that users are able to change on the front end
                    // this will *not* include any user defined values...just properties that *can* be configured
                    var configurableProperties = PortalsHelpers.GetConfigurableProperties(toolboxItem.ControllerType);

                    // this will be the property list (with user values set) that we pass to the view
                    var propertyViewModels = new List<PortalsItemPropertyViewModel>();
                    
                    foreach (var property in configurableProperties)
                    {
                        string value = string.Empty;
                        // if the user has actually set any configurable property value, transfer the value to the controller
                        if (dashboardItem.Properties.Any(p => p.Name == property.Name))
                        {
                            value = dashboardItem.Properties.Single(p => p.Name == property.Name).Value;

                            // type the value correctly
                            object propValue = PortalsHelpers.GetValueFromString(value, property.PropertyType);

                            // fairly confident that this isn't a good way to do this but...it works
                            var backingProperty = configurableProperties.Where(p => p.Name == property.Name).Single().DashboardConfigurableAttribute.BackingProperty ?? property.Name;
                            
                            proxy.Controller.SetProperty(backingProperty, propValue);
                        }
                        
                        // build the view model for the front end
                        var propertyViewModel = new PortalsItemPropertyViewModel();

                        propertyViewModel.Name = property.Name;
                        propertyViewModel.DisplayName = configurableProperties.Where(p => p.Name == property.Name).Single().DashboardConfigurableAttribute.DisplayName ?? property.Name;
                        propertyViewModel.Value = value;

                        propertyViewModels.Add(propertyViewModel);
                    }

                    // build a new DashboardItemViewModel with all the properties we need to render, configure, and save the item
                    var dashboardItemModel = new DashboardItemViewModel();

                    dashboardItemModel.Title = toolboxItem.Title;
                    dashboardItemModel.ControllerType = toolboxItem.ControllerType;
                    dashboardItemModel.Properties = propertyViewModels;
                    dashboardItemModel.Html = executeControllers ? ExecuteController(proxy) : string.Empty;

                    model.DashboardItems.Add(dashboardItemModel);
                }
            }

            return model;
        }
    }
}
