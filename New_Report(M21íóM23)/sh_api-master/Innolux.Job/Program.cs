using Hangfire;
using Innolux.Client.Extensions;
using Innolux.Core.Extensions.ApiModel;

var builder = WebApplication.CreateBuilder(args);
// 設置使用AppConfig目録內的配置
builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    string? env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    if (string.IsNullOrEmpty(env) || env == "Production")
    {
        config.AddJsonFile($"AppConfig/appsettings.json", optional: true, reloadOnChange: true);
    }
    else if (env == "Development")
    {
        config.AddJsonFile($"AppConfig/appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json");
    }
    else
    {
        config.AddJsonFile($"AppConfig/appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true);
    }
});
/*跨域源名称-随意命名*/
ConfigurationManager Configuration = builder.Configuration;
AppConfig.ServerName = Configuration.GetSection("ServerName").Value;
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


#region log4net 注入
builder.Services.AddLogging(logBuilder =>
{
    logBuilder.ClearProviders();  //清除系統默認的日誌記錄器
                                  //logBuilder.AddFilter("System", LogLevel.Warning); //过滤掉系统默认的一些日志
                                  //logBuilder.AddFilter("Microsoft", LogLevel.Warning);//过滤掉系统默认的一些日志
                                  //logBuilder.AddLog4Net();
    logBuilder.AddLog4Net(new Log4NetProviderOptions()
    {
        Log4NetConfigFileName = "AppConfig/log4net.config",
        Watch = true
    });
});
#endregion
//#region Hangfire註冊
//builder.Services.AddHangfireConfig(Configuration);
//#endregion
#region Hangfire
//builder.Services.AddHangfire(c =>
//{
//    c.UseSqlServerStorage("Data Source=10.189.128.216;User Id=hangfire;Password=hangfire;initial catalog=HangFireUsage;Connect Timeout=9000");
//});
builder.Services.AddHangfireConfig(Configuration);
#endregion
var app = builder.Build();
// Configure the HTTP request pipeline.

app.UseRouting();

app.UseHangfireDashboard("/Jobs");

app.Run();
