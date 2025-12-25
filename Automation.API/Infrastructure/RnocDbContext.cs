using Microsoft.EntityFrameworkCore;
using Network.API.Model;
using System;

namespace Network.API.Infrastructure
{
    public class RnocDbContext : DbContext
    {
        private readonly string _connectionString;
        
        public RnocDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_connectionString);
                AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            }
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            // Configure hw_bts_data table as keyless entity (no primary key)
            builder.Entity<Hw_BtsData>()
                .ToTable("hw_bts_data")
                .HasNoKey();
                
            // Configure nokia_bts_data table as keyless entity (no primary key)
            builder.Entity<Nokia_BtsData>()
                .ToTable("nokia_bts_data")
                .HasNoKey();
                
            // Configure nokia_bts_data5g table as keyless entity (no primary key)
            builder.Entity<Nokia_BtsData5G>()
                .ToTable("nokia_bts_data5g")
                .HasNoKey();
                
            // Configure zte_bts_data table as keyless entity (no primary key)
            builder.Entity<Zte_BtsData>()
                .ToTable("zte_bts_data")
                .HasNoKey();
                
            // Configure ericsson_bts_data table as keyless entity (no primary key)
            builder.Entity<Ericsson_BtsData>()
                .ToTable("ericsson_bts_data")
                .HasNoKey();

            // Configure daily_4g_summary table
            builder.Entity<Daily_4G_Summary>()
                .ToTable("daily_4g_summary")
                .HasKey(e => new { e.ReportDate, e.Province });

            // Configure daily_5g_summary table
            builder.Entity<Daily_5G_Summary>()
                .ToTable("daily_5g_summary")
                .HasKey(e => new { e.ReportDate, e.Province });
                
            // Configure r001_data_runtime table
            builder.Entity<R001_DataRuntime>()
                .ToTable("r001_data_runtime")
                .HasKey(e => e.Id);
                
            // Configure r001_data_runtime_bad table
            builder.Entity<R001_DataRuntimeBad>()
                .ToTable("r001_data_runtime_bad")
                .HasKey(e => e.Id);
                
            // Configure r001_scheduler_fix_parametter table
            builder.Entity<R001_SchedulerFixParameter>()
                .ToTable("r001_scheduler_fix_parametter")
                .HasKey(e => e.Id);
            
            // Map AuditEntity properties to lowercase column names
            builder.Entity<R001_SchedulerFixParameter>()
                .Property(e => e.Id).HasColumnName("id");
            builder.Entity<R001_SchedulerFixParameter>()
                .Property(e => e.CreatedDateTime).HasColumnName("createddatetime");
            builder.Entity<R001_SchedulerFixParameter>()
                .Property(e => e.CreatedBy).HasColumnName("createdby");
            builder.Entity<R001_SchedulerFixParameter>()
                .Property(e => e.UpdatedDateTime).HasColumnName("updateddatetime");
            builder.Entity<R001_SchedulerFixParameter>()
                .Property(e => e.UpdatedBy).HasColumnName("updatedby");
        }
        
        public DbSet<Hw_BtsData> Hw_BtsDatas { get; set; }
        public DbSet<Nokia_BtsData> Nokia_BtsDatas { get; set; }
        public DbSet<Nokia_BtsData5G> Nokia_BtsData5Gs { get; set; }
        public DbSet<Zte_BtsData> Zte_BtsDatas { get; set; }
        public DbSet<Ericsson_BtsData> Ericsson_BtsDatas { get; set; }
        public DbSet<Daily_4G_Summary> Daily_4G_Summaries { get; set; }
        public DbSet<Daily_5G_Summary> Daily_5G_Summaries { get; set; }
        public DbSet<R001_DataRuntime> R001_DataRuntimes { get; set; }
        public DbSet<R001_DataRuntimeBad> R001_DataRuntimeBads { get; set; }
        public DbSet<R001_SchedulerFixParameter> R001_SchedulerFixParameters { get; set; }
    }
} 