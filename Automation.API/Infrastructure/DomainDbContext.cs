using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Network.Core.Interfaces;
using Network.Core.Models;
using Network.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Network.API.Infrastructure
{
    public class DomainDbContext : DbContext, IUnitOfWork
    {
        private IDbContextTransaction _dbContextTransaction;        
        public DomainDbContext(DbContextOptions<DomainDbContext> dbContextOptions) : base(dbContextOptions)
        {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        #region Dataset
        //Hệ thống
        public DbSet<Sys_AuthToken> Sys_AuthTokens { get; set; }
        public DbSet<Sys_Category> Sys_Categories { get; set; }
        public DbSet<Sys_File> Sys_Files { get; set; }
        public DbSet<Sys_Config> Sys_Configs { get; set; }
        public DbSet<Sys_Organization> Sys_Organizations { get; set; }
        public DbSet<Sys_Permission> Sys_Permissions { get; set; }        
        public DbSet<Sys_Resource> Sys_Resources { get; set; }
        public DbSet<Sys_Role> Sys_Roles { get; set; }        
        public DbSet<Sys_User> Sys_Users { get; set; }
        public DbSet<Sys_User_Role> Sys_Users_Roles { get; set; }
        public DbSet<Sys_Notification> Sys_Notifications { get; set; }
        public DbSet<Speed_ThongTinNhanVienDo> Speed_ThongTinNhanVienDos { get; set; }
        public DbSet<Speed_DataOkla> Speed_DataOklas { get; set; }
        public DbSet<Speed_FileImport> Speed_FileImport { get; set; }
        public DbSet<Sys_EmailSms> Sys_EmailSms { get; set; }
        public DbSet<Speed_SmsEmail> Speed_SmsEmail { get; set; }
        public DbSet<Speed_Debian> Speed_Debian { get; set; }
        public DbSet<I004_1_LSP> I004_1_LSPs { get; set; }
        #endregion

        #region IUnitOfWork
        public void CreateTransaction()
        {
            _dbContextTransaction = Database.BeginTransaction();            
        }
        public void Commit()
        {
            _dbContextTransaction.Commit();
        }
        public void Roolback()
        {
            _dbContextTransaction.Rollback();
            _dbContextTransaction.Dispose();
        }
        public async Task<int> SaveAsync(CancellationToken cancellationToken = default)
        {
            OnBeforeSaveChanges();
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }
        private void OnBeforeSaveChanges()
        {
            //var rs = LoggingExtensions.TrackingAuditLogs(Guid.Empty, "", ChangeTracker);
        }
        #endregion
    }
}
