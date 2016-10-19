using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Telerik.Sitefinity.Services;
using timw255.Sitefinity.Portals.MVC.Models;

namespace timw255.Sitefinity.Portals.MVC.Controllers
{
    public class DashboardToolboxController : Controller
    {
        public ActionResult Index()
        {
            if (SystemManager.IsDesignMode)
            {
                // rendering these widgets in design mode would just get messy, best not to do it
                return View("PageEditor");
            }

            return View("Default");
        }

        public ActionResult Configure()
        {
            if (SystemManager.IsDesignMode)
            {
                // rendering these widgets in design mode would just get messy, best not to do it
                return View("PageEditor");
            }
                
            var model = new DashboardToolboxViewModel();
            model.DashboardToolboxItems = new List<DashboardToolboxItemViewModel>();

            // get all the toolbox items that the currently logged in user has access to
            var toolboxItems = PortalsHelpers.GetToolboxItems();

            // loop through the toolbox items and create view models for them
            foreach (var toolboxItem in toolboxItems)
            {
                var item = new DashboardToolboxItemViewModel();

                item.ControllerType = toolboxItem.Value.ControllerType;
                item.Title = toolboxItem.Value.Title;
                item.ThumbnailUrl = toolboxItem.Value.CssClass;

                // get all the configurable properties for this item
                // (this is necessary for the front end designers to have correct information)
                var configurableProperties = PortalsHelpers.GetConfigurableProperties(toolboxItem.Value.ControllerType);

                // this will be the property list that we pass to the view
                var propertyViewModels = new List<PortalsItemPropertyViewModel>();

                foreach (var property in configurableProperties)
                {
                    // build the view model for the front end
                    var propertyViewModel = new PortalsItemPropertyViewModel();

                    propertyViewModel.Name = property.Name;
                    propertyViewModel.DisplayName = configurableProperties.Where(p => p.Name == property.Name).Single().DashboardConfigurableAttribute.DisplayName ?? property.Name;
                    propertyViewModel.Value = string.Empty;

                    propertyViewModels.Add(propertyViewModel);
                }

                item.Properties = propertyViewModels;

                model.DashboardToolboxItems.Add(item);
            }

            return View("Configure", model);
        }
    }
}
