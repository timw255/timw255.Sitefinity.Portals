using System;
using System.Data.Entity;
using System.Linq;

namespace timw255.Sitefinity.Portals.Data.EntityFramework
{
    public interface IUserDashboardsEFDbContext
    {
        #region Methods
        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <returns></returns>
        DbContextTransaction BeginTransaction();

        /// <summary>
        /// Rollbacks the transaction.
        /// </summary>
        void RollbackTransaction();

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        void CommitTransaction();
        #endregion
    }
}