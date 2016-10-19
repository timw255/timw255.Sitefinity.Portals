using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telerik.Sitefinity.Security.Model;
using timw255.Sitefinity.Portals.Models;

namespace timw255.Sitefinity.Portals
{
    public static class PortalsExtensions
    {
        // this handles returning the current dashboard information...or, if one doesn't exist, a "blank" one to render
        public static Dashboard GetDashboard(this User currentUser, Guid dashboardId)
        {
            var dashboardsManager = UserDashboardsManager.GetManager();

            var userDashboardData = dashboardsManager.GetUserDashboardDatas().Where(d => d.DashboardId == dashboardId && d.UserId == currentUser.Id).FirstOrDefault();

            if (userDashboardData == null)
            {
                userDashboardData = dashboardsManager.CreateUserDashboardData();
                userDashboardData.DashboardId = dashboardId;
                userDashboardData.UserId = currentUser.Id;
                userDashboardData.DashboardContent = new List<DashboardItem>().ToJson();

                dashboardsManager.SaveChanges();
            }

            List<DashboardItem> dashboardItems = new List<DashboardItem>();

            if (!string.IsNullOrWhiteSpace(userDashboardData.DashboardContent))
            {
                dashboardItems.AddRange(userDashboardData.DashboardContent.FromJson<List<DashboardItem>>());
            }
            
            var dashboard = new Dashboard()
            {
                DashboardId = userDashboardData.DashboardId,
                DashboardItems = dashboardItems
            };

            return dashboard;
        }

        // handy to quickly save the dashboard data
        public static void SaveDashboard(this User currentUser, Guid dashboardId, List<DashboardItem> dashboardItems)
        {
            var dashboardsManager = UserDashboardsManager.GetManager();

            var userDashboardData = dashboardsManager.GetUserDashboardDatas().Where(d => d.DashboardId == dashboardId && d.UserId == currentUser.Id).FirstOrDefault();

            foreach (var dashboardItem in dashboardItems)
            {
                dashboardItem.Properties.RemoveAll(p => p.Value == string.Empty);
            }

            userDashboardData.DashboardContent = dashboardItems.ToJson();

            dashboardsManager.SaveChanges();
        }

        // handy to quickly delete the dashboard data
        public static void DeleteDashboard(this User currentUser, Guid dashboardId)
        {
            var dashboardsManager = UserDashboardsManager.GetManager();

            var userDashboardData = dashboardsManager.GetUserDashboardDatas().Where(d => d.DashboardId == dashboardId && d.UserId == currentUser.Id).FirstOrDefault();

            if (userDashboardData != null)
            {
                dashboardsManager.DeleteUserDashboardData(userDashboardData);

                dashboardsManager.SaveChanges();
            }
        }

        // it is what it is...
        public static void SetProperty(this object target, string compoundProperty, object value)
        {
            string[] bits = compoundProperty.Split('.');
            for (int i = 0; i < bits.Length - 1; i++)
            {
                PropertyInfo propertyToGet = target.GetType().GetProperty(bits[i]);
                if (propertyToGet == null)
                {
                    return;
                }
                target = propertyToGet.GetValue(target, null);
            }
            PropertyInfo propertyToSet = target.GetType().GetProperty(bits.Last());
            
            propertyToSet.SetValue(target, value, null);
        }
    }
}
