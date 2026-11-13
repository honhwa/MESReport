using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hangfire;
using Innolux.Api;
using Innolux.Api.Policy;
using Innolux.Core.Extensions;
using Innolux.Core.Extensions.ApiModel;
using Innolux.Core.Extensions.EncryptionDecrypt;
// using Innolux.SAMv4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Microsoft.Win32;
using Oracle.ManagedDataAccess.Client;
using StackExchange.Redis;
using System.Diagnostics;
using System.Security.Permissions;
using System.ComponentModel;
using Autofac.Core;

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
    else if (env == "FSTest")
    {
        //AppSetting.Delete("/AppConfig/FSProd");
        //AppSetting.Delete("/AppConfig/TN");
        config.AddJsonFile($"AppConfig/appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json");
    }
    else if (env == "FSProd")
    {
        //AppSetting.Delete("/AppConfig/FSTest");
        //AppSetting.Delete("/AppConfig/TN");
        config.AddJsonFile($"AppConfig/appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json");
    }
    else if (env == "TN")
    {
        //AppSetting.Delete("/AppConfig/FSTest");
        //AppSetting.Delete("/AppConfig/FSProd");
        config.AddJsonFile($"AppConfig/appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json");
    }
    else if (env == "SHTest")
    {
        AppSetting.Delete("/AppConfig/SHProd");
        config.AddJsonFile($"AppConfig/appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json");
    }
    else if (env == "SHProd")
    {
        AppSetting.Delete("/AppConfig/SHTest");
        config.AddJsonFile($"AppConfig/appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json");
    }
    else
    {
        config.AddJsonFile($"AppConfig/appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true);
    }
});
/*跨域源名称-随意命名*/
string AllowSpecificOrigins = "netcore_api";
ConfigurationManager Configuration = builder.Configuration;
AppConfig.ServerName = Configuration.GetSection("ServerName").Value;
AppConfig.Innodriver_Url = Configuration.GetSection("Innodriver_Url").Value;
AppConfig.Innodriver_FileAddress = Configuration.GetSection("Innodriver_FileAddress").Value;
AppConfig.Excel_Url = Configuration.GetSection("Excel_Url").Value;
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// Add services to the container.
#region 跨域配置

string[] CorsIp = (string[])Configuration.GetSection("CorsIp").Get(typeof(string[]));
builder.Services.AddCors(options =>
{
    // Policy 名稱 CorsPolicy 是自訂的，可以自己改
    options.AddPolicy(AllowSpecificOrigins, policy =>
    {
        // 設定允許跨域的來源，有多個的話可以用 `,` 隔開
        //policy.WithOrigins(CorsIp)
        //    .AllowAnyHeader()
        //    .AllowAnyMethod()
        //    .AllowCredentials().WithExposedHeaders("Content-Disposition");

        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
        //    .AllowCredentials();

    });
});

#endregion
#region Api文档配置
// 抓取swagger的分組配置
var swaggerConfigs = Configuration.GetSection("ApiDoc:Config").Get<List<OpenApiInfo>>();
// 抓取swagger的origin url配置
List<OpenApiServer> servers = (List<OpenApiServer>)Configuration.GetSection("ApiDoc:SwaggerRootPrd").Get(typeof(List<OpenApiServer>));
//注册Swagger生成器，定义一个和多个Swagger 文档
builder.Services.AddSwaggerGen(c =>
{
    // 把origin url添加進去
    foreach (OpenApiServer s in servers)
    {
        c.AddServer(s);
    }
    // 分組配置
    swaggerConfigs.ForEach(s =>
    {
        c.SwaggerDoc(s.Title, s);
    });
    // 分組配置
    c.DocInclusionPredicate((name, des) =>
    {
        // 沒有分組則分配在默認分組
        if (string.IsNullOrEmpty(des.GroupName) && name == "Default") return true;
        // 有就分配在對應位置
        else return name == des.GroupName;
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath, true);
    //Console.WriteLine(Path.Combine(AppContext.BaseDirectory, "Innolux.EFCore.xml"));
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Innolux.EFCore.xml"));
});
#endregion 
#region 调用自定义的依赖注入
//注册appsettings读取类
builder.Services.AddSingleton(new AppSetting(builder.Configuration));
//获取appsetting中的oracle连接字符串
var oracleConn = AppSetting.app(new string[] { "ConnectionStrings", "OracleDbConnection" });
Decrypt decrypt = new();
OracleConnectionStringBuilder oracleStr = new OracleConnectionStringBuilder(oracleConn);
oracleStr.Password = decrypt.Decryption(oracleStr.Password);//调用解密函数
string Oracle = oracleStr.ConnectionString;
Oracle = Oracle.Replace("\"", "");//修正该Oracle连接字符串的格式
builder.Services.InnoluxConfigureServices(Configuration, Oracle);
#endregion
#region  Session注入
builder.Services.AddDistributedMemoryCache();  //表示进程内保持session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
});
builder.Services.AddHttpContextAccessor();  //如果不是在Controller里，需要注入全局HttpContextAccessor
#endregion
#region 全局参数验证--替换api参数验证失败net.core默认的的BadRequest

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = (context) =>
    {
        //如果参数验证错误
        var error = context.ModelState.GetValidationSummary();
        return new JsonResult(new { success = false, code = 400, data = $"参数验证不通过 :{error.ToString()}" });
    };
});

#endregion
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
#region 設置output參數的格式
builder.Services.AddControllers().AddJsonOptions(options =>
{
    //options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    //options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All);
    //options.JsonSerializerOptions.WriteIndented = true;
    //options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});
#endregion
var app = builder.Build();
//手動注入DI
GlobalContext.ServiceProvider = app.Services;
//AppSetting.ChangeFile();
#region 设置documents文件夹静态文件目录

var statFile = new StaticFileOptions();
statFile.FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(AppDomain.CurrentDomain.BaseDirectory);

System.Diagnostics.Debug.WriteLine("-----------------" + AppDomain.CurrentDomain.BaseDirectory);

Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider extensionProvider =
    new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();

extensionProvider.Mappings[".vue"] = "application/javascript";

statFile.ContentTypeProvider = extensionProvider;

InnSqlAgent.IApiBase.init();
app.UseStaticFiles(statFile);
//InnSqlAgent.IApiBase.init();
string contentRoot = Directory.GetCurrentDirectory();
if (!Directory.Exists(Path.Combine(contentRoot, "documents"))) Directory.CreateDirectory(Path.Combine(contentRoot, "documents"));
IFileProvider fileProvider = new PhysicalFileProvider(Path.Combine(contentRoot, "documents"));
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = fileProvider,
    RequestPath = "/documents"
});

#endregion
#region Api文档生成器中间件-与注入进来的服务配套使用

//启用中间件服务生成Swagger作为JSON终结点
app.UseSwagger();
//启用中间件服务对swagger-ui，指定Swagger JSON终结点
app.UseSwaggerUI(c =>
{
    swaggerConfigs.ForEach(f =>
    {
        c.SwaggerEndpoint($"/swagger/{f.Title}/swagger.json", f.Title);
    });
});
#endregion

/*中间件的使用注意顺序*/
app.UseCors(AllowSpecificOrigins);//必须位于UserMvc之前 
app.UseRouting();
app.UseMiddleware(typeof(ExceptionHandlerMiddleWare));  //全局捕获异常中间件-自定义
//先开启认证
app.UseAuthentication();
//授权中间件
app.UseAuthorization();
app.UseSession();  //建立和维护会话状态
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
var psi = new ProcessStartInfo
{
    FileName = "http://localhost:8000/AppConfig/apiWorkList.html",
    UseShellExecute = true
};
Process.Start(psi);
app.Run();
