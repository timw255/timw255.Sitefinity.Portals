using System;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using timw255.Sitefinity.Portals.Models;

namespace timw255.Sitefinity.Portals.Data.EntityFramework.EntityConfigurations
{
    public class UserDashboardDataTypeConfiguration : EntityTypeConfiguration<UserDashboardData>
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="UserDashboardDataTypeConfiguration" /> class.
        /// </summary>
        public UserDashboardDataTypeConfiguration()
        {
            this.ToTable("Portals_UserDashboardDatas");
            this.HasKey(x => x.Id);
            this.Property(x => x.UserId).IsRequired();
            this.Property(x => x.DashboardId).IsRequired();
            this.Property(x => x.DashboardContent).IsRequired().IsMaxLength();
            this.Property(x => x.LastModified);
            this.Property(x => x.DateCreated);
            this.Property(x => x.ApplicationName);
        }
        #endregion
    }
}