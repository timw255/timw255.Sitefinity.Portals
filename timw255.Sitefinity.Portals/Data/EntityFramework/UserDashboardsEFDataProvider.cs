using System;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using Telerik.Microsoft.Practices.Unity;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Data.Decorators;
using timw255.Sitefinity.Portals.Data.EntityFramework.Decorators;
using timw255.Sitefinity.Portals.Models;


namespace timw255.Sitefinity.Portals.Data.EntityFramework
{
    public class UserDashboardsEFDataProvider : UserDashboardsDataProvider, IUserDashboardsEFDataProvider
    {
        #region DashboardsDataProvider
        protected override void Initialize(string providerName, NameValueCollection config, Type managerType, bool initializeDecorator)
        {
            if (!ObjectFactory.IsTypeRegistered(typeof(UserDashboardsEFDataProviderDecorator)))
                ObjectFactory.Container.RegisterType<IDataProviderDecorator, UserDashboardsEFDataProviderDecorator>(typeof(UserDashboardsEFDataProviderDecorator).FullName);

            base.Initialize(providerName, config, managerType, initializeDecorator);
        }

        public override IQueryable<UserDashboardData> GetUserDashboardDatas()
        {
            return this.Context.UserDashboardDatas.Where(p => p.ApplicationName == this.ApplicationName);
        }

        public override UserDashboardData GetUserDashboardData(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be Empty Guid");

            return this.Context.UserDashboardDatas.Find(id);
        }

        public override UserDashboardData CreateUserDashboardData()
        {
            Guid id = Guid.NewGuid();
            var item = new UserDashboardData(id, this.ApplicationName);

            return this.Context.UserDashboardDatas.Add(item);
        }

        public override void UpdateUserDashboardData(UserDashboardData entity)
        {
            var context = this.Context;

            if (context.Entry(entity).State == EntityState.Detached)
                context.UserDashboardDatas.Attach(entity);

            context.Entry(entity).State = EntityState.Modified;
            entity.LastModified = DateTime.UtcNow;
        }

        public override void DeleteUserDashboardData(UserDashboardData entity)
        {
            var context = this.Context;

            if (context.Entry(entity).State == EntityState.Detached)
                context.UserDashboardDatas.Attach(entity);

            context.UserDashboardDatas.Remove(entity);
        }
        #endregion

        #region IPortalsEFDataProvider
        public UserDashboardsEFDataProviderContext ProviderContext { get; set; }

        public UserDashboardsEFDbContext Context
        {
            get
            {
                return (UserDashboardsEFDbContext)this.GetTransaction();
            }
        }
        #endregion
    }
}