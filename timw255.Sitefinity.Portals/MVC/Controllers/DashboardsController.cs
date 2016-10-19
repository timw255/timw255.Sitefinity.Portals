using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Telerik.Sitefinity.Modules.Pages.Configuration;
using timw255.Sitefinity.Portals.Models;
using timw255.Sitefinity.Portals.MVC.Models;

namespace timw255.Sitefinity.Portals.MVC
{
    public class DashboardsController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Save([FromBody] List<DashboardViewModel> model)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

            // dashboard data is stored using the user id so, we need to get the user
            var currentUser = PortalsHelpers.GetCurrentUser();
            
            // loop through the dashboard items and get them ready for persisting
            foreach (var dashboardModel in model)
            {
                // this will be our final list of items that we'll store
                var dashboardItems = new List<DashboardItem>();

                foreach (var dashboardItem in dashboardModel.DashboardItems)
                {
                    // this ensures that the user has access to the dashboard item they're trying to save
                    ToolboxItem toolboxItem = null;
                    if (PortalsHelpers.GetToolboxItem(dashboardItem.ControllerType, out toolboxItem))
                    {
                        // get the properties that users are able to change on the front end
                        var configurableProperties = PortalsHelpers.GetConfigurableProperties(toolboxItem.ControllerType);

                        var configuredProperties = dashboardItem.Properties;//.FromJson<List<PortalsItemProperty>>();

                        // just to keep things tidy...
                        configuredProperties.RemoveAll(p => p.Value == string.Empty);

                        // if the user has actually set any configurable property values, make sure they're valid
                        foreach (var property in configuredProperties)
                        {
                            // check to make sure a previously configurable property is still configurable
                            if (!configurableProperties.Any(p => p.Name == property.Name))
                            {
                                configuredProperties.Remove(property);
                            }
                        }
                        
                        // done with sanitization, let's build the item we're going to persist
                        var item = new DashboardItem()
                        {
                            ControllerType = dashboardItem.ControllerType,
                            Properties = configuredProperties.Select(p => new PortalsItemProperty() { Name = p.Name, Value = p.Value }).ToList()
                        };

                        dashboardItems.Add(item);
                    }
                }

                currentUser.SaveDashboard(dashboardModel.DashboardId, dashboardItems);
            }

            return response;
        }
    }
}
