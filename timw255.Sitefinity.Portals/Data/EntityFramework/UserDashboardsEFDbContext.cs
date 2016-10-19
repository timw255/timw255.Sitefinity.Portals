using System;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using timw255.Sitefinity.Portals.Data.EntityFramework.EntityConfigurations;
using timw255.Sitefinity.Portals.Models;

namespace timw255.Sitefinity.Portals.Data.EntityFramework
{
    public class UserDashboardsEFDbContext : DbContext, IUserDashboardsEFDbContext
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDashboardsEFDbContext" /> class.
        /// </summary>
        public UserDashboardsEFDbContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDashboardsEFDbContext" /> class.
        /// </summary>
        /// <param name="dbConnectionString">The db connection string.</param>
        public UserDashboardsEFDbContext(string dbConnectionString)
            : base(dbConnectionString)
        {
        }
        #endregion

        #region Properties

        private DbContextTransaction Transaction { get; set; }

        #endregion

        #region IPortalsEFDbContext

        public DbContextTransaction BeginTransaction()
        {
            if (this.Transaction != null)
            {
                this.Transaction.Dispose();
                this.Transaction = null;
            }
            this.Transaction = this.Database.BeginTransaction();
            return this.Transaction;
        }

        public void RollbackTransaction()
        {
            if (this.Transaction != null)
            {
                try
                {
                    this.Transaction.Rollback();
                }
                finally
                {
                    this.Transaction.Dispose();
                    this.Transaction = null;
                }
            }
        }

        public void CommitTransaction()
        {
            if (this.Transaction != null)
                this.Transaction.Commit();
        }
        #endregion

        #region Entities
        /// <summary>
        /// Gets or sets the UserDashboardDatas.
        /// </summary>
        /// <value>The UserDashboardDatas.</value>
        public DbSet<UserDashboardData> UserDashboardDatas { get; set; }
        #endregion

        #region DbContext method overrides
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserDashboardDataTypeConfiguration());
        }

        protected override void Dispose(bool disposing)
        {
            if (this.Transaction != null)
            {
                this.Transaction.Dispose();
                this.Transaction = null;
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}