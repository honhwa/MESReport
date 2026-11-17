
## 項目說明 .NET6 + VScode 开发API接口项目

### 环境安装
安装  .netcore6  
安装包:http://10.189.127.62:9015/s/TJJamktbkytBkC9  
vscode安装插件 c#,Visual Studio IntelliCode,vscode-solution-explorer


## 開發一支API步驟
### 一.Innolux.Api/Controllers (路由定义目录)  
在该目录下新建一个cs文件(ex:OqcController.cs);文件的内容参考 OqcCotroller.cs  
说明:  
1.路由层不做实际的业务处理,所有的业务处理逻辑都放在Biz;需在路由层这里using 处理的Biz 实体;  
2.注意修改 class名称跟构造函数名称;  
3.这里的"Oqc"是swaggerUI的Tag name;

```c#
...
using Innolux.Biz;
using Innolux.Biz.OQC;

public class OqcController : ControllerBase
{
    ...
    private readonly OQCBiz _OQCBiz;

    public OqcController(OQCBiz OQCBiz)
    {
        _OQCBiz=OQCBiz;
        //下面就可以调用oqcbiz 的方法了 
        ...
    }

    /// <summary>
    /// 獲取人員所屬組織以下的工時加班情況
    /// </summary>
    /// <param name="datetime"></param>
    /// <param name="shift"></param>
    /// <returns></returns>
    [HttpGet("/getOqcM")] //method get ,url:/getOqcM
    public ActionResult<ApiResult>
    getlcm2OQCM([FromQuery] string datetime, string shift)
    {
        // string xtoken = Request.Headers["X-Token"].ToString();
        List<OqcPhaseOne> result = _OQCBiz.GetOqcPhaseOne(datetime, shift);
        return result.Count > 0
            ? ApiResult.Ok(result)
            : ApiResult.Error("無數據！");
    }
}
...
```

### 二. 为API的返回值 建立一个Model 去做数据的自动转换  
具体操作：Innolux.EFCore/Model 新增一個文件夾 OQC/OQC.cs  
ex:我们要返回下面格式的对象,我们的Model应该如下  
<img src = "./picture/1.png">
```c#
using System;
using System.Collections.Generic;
using System.Text;

namespace Innolux.EFCore.Model
{
    public class OqcPhaseOne
    {
        /// <summary>
        /// 机种别
        /// </summary>
        public string TYPE { get; set; }

        /// <summary>
        /// 不良数
        /// </summary>
        public string NG_QTY { set; get; }

        /// <summary>
        /// dppm
        /// </summary>
        public string DPPM { set; get; }

        /// <summary>
        /// 目标
        /// </summary>
        public string target { set; get; }

        /// <summary>
        /// 颜色
        /// </summary>
        public string color { set; get; }
    }
}
```
注意：Model 的字段需跟你的sql selectc出来的结果保证一致;没有匹配的字段结果为null  

###  三. 业务BIZ部分  
在该Innolux.Biz目录下新建 OQC/OQCBiz.cs 
```c#
namespace Innolux.Biz.OQC
{
    public class OQCBiz
    {
        //DB连线
        private readonly OQCDbContext _oqc;
        //公用服务
        private readonly ICommonAppService _commonAppService;
        //公用方法
        private readonly Public _common;

        public OQCBiz(
            OQCDbContext oqc,
            ICommonAppService commonAppService,
            Public common
        )
        {
            _oqc = oqc;
            _commonAppService = commonAppService;
            _common = common;
        }

        public List<OqcPhaseOne> GetOqcPhaseOne(string datetime,string shift)
        {
            string sqloqc =
                $@"select type,ng_qty,dppm,target,color from oqc_api_v";
            List<OqcPhaseOne> list_result = _oqc.Database.SqlQuery<OqcPhaseOne>(sqloqc);
            return list_result;
        }
    }
}

```
细节说明：  
1. 新增DB连线; 编辑 Innlux.EFCore/DbContexts.cs 新增
```c#
/// <summary>
/// </summary>
public class OQCDbContext : DbContext
{
/// <summary>
/// OQCDb連線
/// </summary>
/// <param name="options"></param>
    public OQCDbContext(DbContextOptions<OQCDbContext> options) :
        base(options)
    {
    }
}
```
2.编辑 Innolux.API/CutomIOC.cs 新增连线方式
```c#
......
//Oracle-OQCDbContext
services
    .AddDbContext<OQCDbContext>(options =>
    {
        options
            .UseOracle(Configuration
                .GetConnectionString("OQCDbConnection"));
    });
......
```
3. 新增DB连线配置 (账号、密码。。。),编辑Innolux.API/appsettings.json
```c#
...
"ConnectionStrings": {
"PnbhrgaDbConnection": "User Id=NBHRGADA1;Password=NBHRGADA1;PERSIST SECURITY INFO=True;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.174.1.202)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=tnboa19c)))",
"SmoothHrConnection": "Data Source=XXX;User Id=XXX;Password=XXX;initial catalog=smoothhr;Connect Timeout=9000",
"OQCDbConnection" : "User Id=PHSUMDA1;Password=PHSUMDA2016;PERSIST SECURITY INFO=True;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.189.128.60)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=PHARS)))"
},
...
```


4. .编辑 Innolux.API/CutomIOC.cs  新增業務依賴注入
```c#
...
#region 業務依賴注入
    services.AddScoped<OQCBiz>(); //OQC  new add
    services.AddScoped<Public>(); //
    services.AddScoped<MachineBiz>();//機
#endregion
...
```

### 四.运行测试
按F5 command模式运行 or  
点击run debug 进入断点调试模式  
 swaager URL默认 ：http://localhost:5000/swagger/index.html




