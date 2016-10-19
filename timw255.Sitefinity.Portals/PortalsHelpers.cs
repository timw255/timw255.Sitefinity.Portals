using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Modules.Pages.Configuration;
using Telerik.Sitefinity.Security;
using Telerik.Sitefinity.Security.Claims;
using Telerik.Sitefinity.Security.Model;
using Telerik.Sitefinity.Utilities.TypeConverters;
using timw255.Sitefinity.Portals.Attributes;
using timw255.Sitefinity.Portals.Models;

namespace timw255.Sitefinity.Portals
{
    public static class PortalsHelpers
    {
        // get a specific ToolboxItem if the user has access to it
        public static bool GetToolboxItem(string controllerType, out ToolboxItem toolboxItem)
        {
            KeyValuePair<string, ToolboxItem> item = GetToolboxItems().FirstOrDefault(i => i.Key == controllerType);

            // we're only working with Mvc widgets here so...filter others out
            if (!item.Equals(new KeyValuePair<string, ToolboxItem>()) && item.Value.ControlType == "Telerik.Sitefinity.Mvc.Proxy.MvcControllerProxy")
            {
                toolboxItem = item.Value;
                return true;
            }

            toolboxItem = null;
            return false;
        }

        // this was modeled after http://docs.sitefinity.com/tutorial-use-an-item-parameter-to-implement-role-based-toolbox-filtering
        // more information can be found there
        public static Dictionary<string, ToolboxItem> GetToolboxItems()
        {
            Guid userId = SecurityManager.GetCurrentUserId();

            ConfigManager manager = ConfigManager.GetManager();
            var toolboxesConfig = manager.GetSection<ToolboxesConfig>();
            Toolbox toolbox = toolboxesConfig.Toolboxes["PortalControls"];

            var toolboxItems = new Dictionary<string, ToolboxItem>();

            foreach (var section in toolbox.Sections)
            {
                foreach (ToolboxItem toolboxItem in section.Tools)
                {
                    if (!toolboxItems.ContainsKey(toolboxItem.ControllerType))
                    {
                        var allowedRoles = toolboxItem.Parameters["AllowedRoles"];
                        var disallowedRoles = toolboxItem.Parameters["DisallowedRoles"];

                        bool hasAllowedRoles = !string.IsNullOrEmpty(allowedRoles);
                        bool isAllowedByRole = false;

                        bool hasDisllowedRoles = !string.IsNullOrEmpty(disallowedRoles);
                        bool isDisallowedByRole = false;

                        if (hasAllowedRoles)
                        {
                            // there are "allow" rules
                            var allowedRoleNames = allowedRoles.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim());
                            
                            // check the allowed roles against the user roles
                            if (IsUserInRole(userId, allowedRoleNames))
                            {
                                isAllowedByRole = true;
                            }
                        }

                        if (hasDisllowedRoles)
                        {
                            // there are disallow rules
                            // these will override an "allow"...(can't be too cautious!)
                            var disallowedRoleNames = disallowedRoles.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim());

                            // check the allowed roles against the user roles
                            if (IsUserInRole(userId, disallowedRoleNames))
                            {
                                isDisallowedByRole = true;
                            }
                        }

                        if (((!hasAllowedRoles) || (hasAllowedRoles && isAllowedByRole)) && !isDisallowedByRole)
                        {
                            toolboxItems.Add(toolboxItem.ControllerType, toolboxItem);
                        }
                    }
                }
            }

            return toolboxItems;
        }

        public static List<PortalsItemProperty> GetConfigurableProperties(string controllerType)
        {
            var properties = new List<PortalsItemProperty>();

            if (string.IsNullOrEmpty(controllerType))
            {
                return properties;
            }
            
            ToolboxItem toolboxItem = null;
            if (GetToolboxItem(controllerType, out toolboxItem))
            {
                // the controllerType that was passed in seems to be legit. Let's get an instance and start examining
                IControllerFactory controllerFactory = ControllerBuilder.Current.GetControllerFactory();
                var requestContext = new System.Web.Routing.RequestContext();

                Controller controller = (Controller)controllerFactory.CreateController(requestContext, controllerType);

                // configurable properties are ones marked with the DashboardConfigurable attribute
                var configurableProperties = controller.GetType().GetProperties().Where(prop => prop.IsDefined(typeof(DashboardConfigurable), true));

                foreach (var property in configurableProperties)
                {
                    var p = new PortalsItemProperty();

                    p.PropertyType = property.PropertyType;
                    p.Name = property.Name;
                    p.Value = string.Empty;
                    p.DashboardConfigurableAttribute = property.GetCustomAttribute<DashboardConfigurable>(false);

                    properties.Add(p);
                }
            }
            
            return properties;
        }
        
        public static User GetCurrentUser()
        {
            var userIdentity = ClaimsManager.GetCurrentIdentity();
            UserManager manager = UserManager.GetManager(userIdentity.MembershipProvider);

            var user = manager.GetUser(userIdentity.Name);

            return user;
        }

        private static bool IsUserInRole(Guid userId, IEnumerable<string> roleNames)
        {
            foreach (var roleName in roleNames)
            {
                if (IsUserInRole(userId, roleName.Trim()))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsUserInRole(Guid userId, string roleName)
        {
            bool isUserInRole = false;

            UserManager userManager = UserManager.GetManager();
            RoleManager roleManager = RoleManager.GetManager(SecurityManager.ApplicationRolesProviderName);
            
            bool roleExists = roleManager.RoleExists(roleName);

            if (roleExists)
            {
                User user = userManager.GetUser(userId);
                isUserInRole = roleManager.IsUserInRole(user.Id, roleName);
            }

            return isUserInRole;
        }

        // snagged from Telerik.Sitefinity.Configuration.ConfigElement
        public static object GetValueFromString(string stringValue, Type type, ConfigProperty property = null)
        {
            object obj;
            if (type == typeof(string))
            {
                obj = stringValue;
            }
            else if (type == typeof(bool))
            {
                obj = bool.Parse(stringValue);
            }
            else if (type == typeof(DateTime))
            {
                obj = DateTime.Parse(stringValue, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            }
            else if (type == typeof(float) || type == typeof(double) || type == typeof(decimal))
            {
                obj = TypeDescriptor.GetConverter(type).ConvertFromInvariantString(stringValue);
            }
            else if (type != typeof(Type))
            {
                TypeConverter converter = null;
                if (property != null)
                {
                    converter = property.Converter;
                }
                if (converter == null)
                {
                    converter = TypeDescriptor.GetConverter(type);
                }
                if (converter == null || !converter.CanConvertFrom(typeof(string)))
                {
                    throw new NotSupportedException((property == null ? string.Format("No appropriate conversion for configuration value of type {0}.", type) : string.Format("No appropriate conversion for {0} configuration property {1} with actual type {2}.", property.Type, property.Name, type)));
                }
                obj = converter.ConvertFromString(stringValue);
            }
            else
            {
                obj = TypeResolutionService.ResolveType(stringValue, false);
                if (obj == null)
                {
                    obj = stringValue;
                }
            }
            return obj;
        }

        // snagged from Telerik.Sitefinity.Configuration.ConfigElement
        public static string GetStringValue(object value, ConfigProperty property = null)
        {
            if (value == null)
            {
                return null;
            }
            Type type = value.GetType();
            if (type == typeof(string))
            {
                return (string)value;
            }
            if (type == typeof(DateTime))
            {
                return ((DateTime)value).ToString("u");
            }
            if (type == typeof(float) || type == typeof(double) || type == typeof(decimal))
            {
                return Convert.ToString(value, CultureInfo.InvariantCulture);
            }
            if (type == typeof(Type))
            {
                Type type1 = (Type)value;
                return string.Concat(type1.FullName, ", ", type1.Assembly.GetName().Name);
            }
            TypeConverter converter = null;
            if (property != null)
            {
                converter = property.Converter;
            }
            if (converter == null)
            {
                converter = TypeDescriptor.GetConverter(type);
            }
            if (converter == null || !converter.CanConvertTo(typeof(string)))
            {
                throw new ArgumentException("Unsupported configuration value type.");
            }
            return converter.ConvertToString(value);
        }
    }
}
