using Hangfire;
using Hangfire.Console;
using Hangfire.Dashboard.Management.Metadata;
using Hangfire.Server;
using System.ComponentModel;
using System.Text;

namespace Innolux.Client.Job
{
    [ManagementPage("演示","iGIB")]
    public class Demo
    {
        [Hangfire.Dashboard.Management.Support.Job]
        [DisplayName("調用內部方法")]
        [Description("Hello ~~~~~")]
        [AutomaticRetry(Attempts = 3)]   //自動重試
        [DisableConcurrentExecution(90)] //禁止使用並行
        public void Action(string ClassName,string ConstructArgs,string Function,string MethodArgs, PerformContext context = null)
        {
            string[] Cargs = ConstructArgs.Split(",");
            string[] Margs = MethodArgs.Split(",");
            //context.WriteLine(ConstructArgs);
            //context.WriteLine(MethodArgs);
            Console.WriteLine($"Now:{DateTime.Now}，即將執行{ClassName}.{Function}");
            //Thread.Sleep(30000);
        }
        [Hangfire.Dashboard.Management.Support.Job]
        [DisplayName("呼叫內部方法")]
        [Description("呼叫內部方法")]
        [AutomaticRetry(Attempts = 3)]   //自動重試
        [DisableConcurrentExecution(90)] //禁止使用並行
        public void Action(PerformContext context = null, IJobCancellationToken cancellationToken = null)
        {
            if (cancellationToken.ShutdownToken.IsCancellationRequested)
            {
                return;
            }

            context.WriteLine($"測試用，Now:{DateTime.Now}");
            //Thread.Sleep(30000);
        }
    }
}
