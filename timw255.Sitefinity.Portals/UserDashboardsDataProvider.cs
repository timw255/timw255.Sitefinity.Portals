using System;
using System.Linq;
using Telerik.Sitefinity.Data;
using timw255.Sitefinity.Portals.Models;

namespace timw255.Sitefinity.Portals
{
    public abstract class UserDashboardsDataProvider : DataProviderBase
    {
        #region Public and overriden methods
        /// <summary>
        /// Gets the known types.
        /// </summary>
        public override Type[] GetKnownTypes()
        {
            if (knownTypes == null)
            {
                knownTypes = new Type[]
                {
                    typeof(UserDashboardData)
                };
            }
            return knownTypes;
        }

        /// <summary>
        /// Gets the root key.
        /// </summary>
        /// <value>The root key.</value>
        public override string RootKey
        {
            get
            {
                return "UserDashboardsDataProvider";
            }
        }
        #endregion

        #region Abstract methods
        /// <summary>
        /// Creates a new UserDashboardData and returns it.
        /// </summary>
        /// <returns>The new UserDashboardData.</returns>
        public abstract UserDashboardData CreateUserDashboardData();

        /// <summary>
        /// Gets a UserDashboardData by a specified ID.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <returns>The UserDashboardData.</returns>
        public abstract UserDashboardData GetUserDashboardData(Guid id);

        /// <summary>
        /// Gets a query of all the UserDashboardData items.
        /// </summary>
        /// <returns>The UserDashboardData items.</returns>
        public abstract IQueryable<UserDashboardData> GetUserDashboardDatas();

        /// <summary>
        /// Updates the UserDashboardData.
        /// </summary>
        /// <param name="entity">The UserDashboardData entity.</param>
        public abstract void UpdateUserDashboardData(UserDashboardData entity);

        /// <summary>
        /// Deletes the UserDashboardData.
        /// </summary>
        /// <param name="entity">The UserDashboardData entity.</param>
        public abstract void DeleteUserDashboardData(UserDashboardData entity);
        #endregion

        #region Private fields and constants
        private static Type[] knownTypes;
        #endregion
    }
}