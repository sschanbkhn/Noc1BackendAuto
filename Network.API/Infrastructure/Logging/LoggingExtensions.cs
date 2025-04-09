using Core.Infrastructure.Models.Logs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Network.Core.Models;
using static Network.Core.Enumeration.Sys_Enum;

namespace Network.API.Infrastructure.Logging
{
    public static class LoggingExtensions
    {
        
        private static void UseCoreLogger(this IWebHostEnvironment env, LoggingOptions options)
        {
            var assemblyName = Assembly.GetEntryAssembly()?.GetName().Name;

            var logsPath = Path.Combine(env.ContentRootPath, "logs");
            Directory.CreateDirectory(logsPath);
            var loggerConfiguration = new LoggerConfiguration();

            loggerConfiguration = loggerConfiguration                
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .Enrich.With<ActivityEnricher>()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithProperty("Assembly", assemblyName)
                .Enrich.WithProperty("Application", env.ApplicationName)
                .Enrich.WithProperty("EnvironmentName", env.EnvironmentName)
                .Enrich.WithProperty("ContentRootPath", env.ContentRootPath)
                .Enrich.WithProperty("WebRootPath", env.WebRootPath)
                .Enrich.WithExceptionDetails()
                .Filter.ByIncludingOnly((logEvent) =>
                {                    
                    if (logEvent.Level >= options.File.MinimumLogEventLevel)
                    {
                        var sourceContext = logEvent.Properties.ContainsKey("SourceContext")
                             ? logEvent.Properties["SourceContext"].ToString()
                             : null;

                        var logLevel = GetLogLevel(sourceContext, options);

                        return logEvent.Level >= logLevel;
                    }

                    return false;
                })
                .WriteTo.File(Path.Combine(logsPath, "Log.txt"),
                    retainedFileCountLimit: 30,
                    fileSizeLimitBytes: 10 * 1024 * 1024,
                    rollOnFileSizeLimit: true,
                    shared: true,
                    flushToDiskInterval: TimeSpan.FromSeconds(1),
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{SourceContext}] [TraceId: {TraceId}] {Message:lj}{NewLine}{Exception}",
                    restrictedToMinimumLevel: options.File.MinimumLogEventLevel);
                    

            Log.Logger = loggerConfiguration.CreateLogger();
        }

        private static LoggingOptions SetDefault(IConfiguration Configuration)
        {
            LoggingOptions options = null;
            AppSettings appSettings = new AppSettings();
            Configuration.Bind(appSettings);
            if (appSettings != null)
            {
                options = appSettings.Logging;
            }
            options ??= new LoggingOptions
            {
            };

            options.LogLevel ??= new Dictionary<string, string>();

            if (!options.LogLevel.ContainsKey("Default"))
            {
                options.LogLevel["Default"] = Serilog.Events.LogEventLevel.Warning.ToString();
            }

            options.File ??= new FileOptions
            {
                MinimumLogEventLevel = Serilog.Events.LogEventLevel.Warning,
            };

            return options;
        }

        private static Serilog.Events.LogEventLevel GetLogLevel(string context, LoggingOptions options)
        {
            context = context.Replace("\"", string.Empty);
            string level = "Default";
            var matches = options.LogLevel.Keys.Where(k => context.StartsWith(k));

            if (matches.Any())
            {
                level = matches.Max();
            }

            return (Serilog.Events.LogEventLevel)System.Enum.Parse(typeof(Serilog.Events.LogEventLevel), options.LogLevel[level], true);
        }

        public static IWebHostBuilder UseCoreLogger(this IWebHostBuilder builder)
        {
            
            builder.ConfigureLogging((context, logging) =>
            {
                logging.Configure(options =>
                {
                    options.ActivityTrackingOptions = ActivityTrackingOptions.SpanId | ActivityTrackingOptions.TraceId | ActivityTrackingOptions.ParentId;
                });
                
                logging.AddSerilog();
                
                LoggingOptions options = SetDefault(context.Configuration);

                context.HostingEnvironment.UseCoreLogger(options);
            });

            return builder;
        }
        public static List<AuditLogEntry> TrackingAuditLogs (Guid userId, string userName, ChangeTracker ChangeTracker)
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditLogEntry>();
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is AuditLog || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;
                var auditEntry = new AuditLogEntry(entry);
                auditEntry.TableName = entry.Entity.GetType().Name;
                auditEntry.UserId = userId;
                auditEntry.UserName = userName;
                auditEntries.Add(auditEntry);
                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.ObjectId[propertyName] = property.CurrentValue;
                        continue;
                    }
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = AuditType.Create;
                            auditEntry.NewObject[propertyName] = property.CurrentValue;
                            break;
                        case EntityState.Deleted:
                            auditEntry.AuditType = AuditType.Delete;
                            auditEntry.OldObject[propertyName] = property.OriginalValue;
                            break;
                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.AuditType = AuditType.Update;
                                auditEntry.OldObject[propertyName] = property.OriginalValue;
                                auditEntry.NewObject[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
            }
            return auditEntries;
        }
    }
}
