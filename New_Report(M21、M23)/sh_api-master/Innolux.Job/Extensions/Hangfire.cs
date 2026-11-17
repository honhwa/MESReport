using Hangfire;
using Hangfire.Console;
using Hangfire.Dashboard;
using Hangfire.Dashboard.Management;
using Hangfire.SqlServer;
using Innolux.Core.Extensions.ApiModel;
using System.Reflection;

namespace Innolux.Client.Extensions
{
    public static class Hangfire
    {
        public static void AddHangfireConfig(this IServiceCollection services, IConfiguration Configuration)
        {
            
            //GlobalConfiguration.Configuration
            //    .UseDashboardMetric(DashboardMetrics.ServerCount)
            //    .UseDashboardMetric(DashboardMetrics.RecurringJobCount)
            //    .UseDashboardMetric(DashboardMetrics.RetriesCount)
            //    .UseDashboardMetric(DashboardMetrics.EnqueuedCountOrNull)
            //    .UseDashboardMetric(DashboardMetrics.FailedCountOrNull)
            //    .UseDashboardMetric(DashboardMetrics.EnqueuedAndQueueCount)
            //    .UseDashboardMetric(DashboardMetrics.ScheduledCount)
            //    .UseDashboardMetric(DashboardMetrics.ProcessingCount)
            //    .UseDashboardMetric(DashboardMetrics.SucceededCount)
            //    .UseDashboardMetric(DashboardMetrics.FailedCount)
            //    .UseDashboardMetric(DashboardMetrics.DeletedCount)
            //    .UseDashboardMetric(DashboardMetrics.AwaitingCount)
            //    .UseManagementPages(cc => cc.AddJobs(() => { return GetModuleTypes(); }))
            //    .UseSqlServerStorage(Configuration.GetSection("ConnectionStrings:Hangfire").Value)
            //    ;
            services.AddHangfire(c =>
            {
                c
                .UseDashboardMetric(DashboardMetrics.ServerCount)
                .UseDashboardMetric(DashboardMetrics.RecurringJobCount)
                .UseDashboardMetric(DashboardMetrics.RetriesCount)
                .UseDashboardMetric(DashboardMetrics.EnqueuedCountOrNull)
                .UseDashboardMetric(DashboardMetrics.FailedCountOrNull)
                .UseDashboardMetric(DashboardMetrics.EnqueuedAndQueueCount)
                .UseDashboardMetric(DashboardMetrics.ScheduledCount)
                .UseDashboardMetric(DashboardMetrics.ProcessingCount)
                .UseDashboardMetric(DashboardMetrics.SucceededCount)
                .UseDashboardMetric(DashboardMetrics.FailedCount)
                .UseDashboardMetric(DashboardMetrics.DeletedCount)
                .UseDashboardMetric(DashboardMetrics.AwaitingCount)
                .UseManagementPages(cc => cc.AddJobs(() => { return GetModuleTypes(); }))
                .UseSqlServerStorage(Configuration.GetSection("ConnectionStrings:Hangfire").Value)
                .UseConsole()
                ;
            });
            //services.AddHangfireServer(o =>
            //{
            //    o.ServerName = AppConfig.ServerName;
            //    o.WorkerCount = 20;
            //    o.Queues = new[] { "critical", "normal", "low" };
            //});
        }
        private static Type[] GetModuleTypes()
        {
            //var assemblies = new[] { Assembly.GetEntryAssembly(),Assembly.Load("Innolux.Api") };
            //var moduleTypes = assemblies.SelectMany(f =>
            //{
            //    try
            //    {
            //        return f.GetTypes();
            //    }
            //    catch (Exception)
            //    {
            //        return new Type[] { };
            //    }
            //}
            //                                       ) /*.Where(f => f.IsClass && typeof(IClientModule).IsAssignableFrom(f))*/
            //                            .ToArray();
            var assem = Assembly.Load("Innolux.Api");
            var type = assem.GetType("Innolux.Api.Extensions.Demo");
            return new Type[] { type };
            //return moduleTypes;
        }
    }
}
