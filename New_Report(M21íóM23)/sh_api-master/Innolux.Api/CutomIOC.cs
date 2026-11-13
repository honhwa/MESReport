using Innolux.Biz.Common;
//using Innolux.Biz.Test;
using Innolux.Core.Extensions;
using Innolux.EFCore;
using Microsoft.EntityFrameworkCore;
//using Innolux.Biz.FS.Common;
using Innolux.Biz.FileOperation;
//using IBM.EntityFrameworkCore;
//using IBM.EntityFrameworkCore.Storage.Internal;


namespace Innolux.Api
{
    public static class CutomIOC
    {
        public static void InnoluxConfigureServices(
            this IServiceCollection services,
            IConfiguration Configuration,
            string Oracle
        )
        {

            #region DB配置
            // Oracle 連綫方式
            services
                .AddDbContext<InnoluxDbContext>(options =>
                {
                    options
                        .UseOracle(Oracle);//连接字串填写位置
                });
            //SQLserver 連綫方式
            services
                .AddDbContext<SQLserverDbContext>(options =>
                {
                    options
                        .UseSqlServer(Configuration
                            .GetConnectionString("SQLserverDbConnection"));//Configuration.GetConnectionString("SQLserverDbConnection")这个是从appsetting中的SQLserverDbConnection提对应的sqlserver连接字串
                });
            //LCMDBR
            services
                .AddDbContext<LCMDBRDbContext>(options =>
                {
                    options
                        .UseOracle(Configuration
                            .GetConnectionString("LCMDBRDbConnection"));
                });
            //
            //
            services
                .AddDbContext<PHAMRTDbConnection>(options =>
                {
                    options
                        .UseOracle(Configuration
                            .GetConnectionString("PHAMRTDbConnection"));
                });
            //services.
            //    AddDbContext<DB2DbContext>(options =>
            //    {
            //        options.UseDb2(
            //            Configuration.GetConnectionString("DB2DbConnection"),
            //        p => p.SetServerInfo(IBMDBServerType.LUW, IBMDBServerVersion.LUW_11_01_1010)
            //        );
            //    });


            #endregion



            #region 基本依賴注入
            //seesion幫助類注入
            services.AddScoped<SessionHelper>();
            #endregion



            #region 業務依賴注入
            services.AddScoped<FileUploadBiz>();//上传文件
            services.AddScoped<ShowFileBiz>();//查询文件
            services.AddScoped<FileDownloadBiz>();//下载文件
            services.AddScoped<ExcelUploadBiz>();//下载Excel文件
            services.AddScoped<DeleteFileBiz>();//删除Excel文件
            services.AddScoped<AddFolderBiz>();//删除Excel文件
            //services.AddScoped<DB2TestBiz>();

            #endregion

        }
    }
}
