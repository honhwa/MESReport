using Innolux.Core.Extensions;
using Innolux.Core.Extensions.ApiModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;

namespace ReportWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SQLAgentController : Controller
    {
        [Route("GetDataSet")]
        [HttpGet]
        public object GetDataSet()
        {
            //InnSqlAgent.SqlAgent.HttpContext = HttpContext;
            //List<string> list = new();
            //list.Add(InnSqlAgent.SqlAgent.GetDataSet());
            //return list.Count > 0 ? ApiResult.Ok(list) : ApiResult.Fail("無數據");
            InnSqlAgent.SqlAgent.HttpContext = HttpContext;
            return InnSqlAgent.SqlAgent.GetDataSet();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("GetDataSet")]
        [HttpPost]
        public object GetDataSet2()
        {
            //InnSqlAgent.SqlAgent.HttpContext = HttpContext;
            ////List<string> list = new();
            ////list.Add(InnSqlAgent.SqlAgent.GetDataSet());
            //string str = InnSqlAgent.SqlAgent.GetDataSet();
            //DataSet ds = JsonConvert.DeserializeObject<DataSet>(str);
            //DataTable dt = ds.Tables[0];
            //string s = JsonConvert.SerializeObject(ApiResult.Ok<DataTable>(dt));
            //return s;
            InnSqlAgent.SqlAgent.HttpContext = HttpContext;
            return InnSqlAgent.SqlAgent.GetDataSet();
        }

        [Route("GetDataSetMulti")]
        [HttpGet]
        public object GetDataSetMulti()
        {
            InnSqlAgent.SqlAgent.HttpContext = HttpContext;
            return InnSqlAgent.SqlAgent.GetDataSetMulti();
        }

        [Route("GetDataSetMulti")]
        [HttpPost]
        public object GetDataSetMulti2()
        {
            InnSqlAgent.SqlAgent.HttpContext = HttpContext;
            return InnSqlAgent.SqlAgent.GetDataSetMulti();
        }

        [Route("ExecuteSql")]
        [HttpGet]
        public object ExecuteSql()
        {
            InnSqlAgent.SqlAgent.HttpContext = HttpContext;
            return InnSqlAgent.SqlAgent.ExecuteSQL();
        }

        [Route("ExecuteSql")]
        [HttpPost]
        public object ExecuteSql2()
        {
            InnSqlAgent.SqlAgent.HttpContext = HttpContext;
            return InnSqlAgent.SqlAgent.ExecuteSQL();
        }

        [Route("ExecuteSqlMulti")]
        [HttpGet]
        public object ExecuteSqlMulti()
        {
            InnSqlAgent.SqlAgent.HttpContext = HttpContext;
            return InnSqlAgent.SqlAgent.ExecuteSQLMulti();
        }

        [Route("ExecuteSqlMulti")]
        [HttpPost]
        public object ExecuteSqlMulti2()
        {
            InnSqlAgent.SqlAgent.HttpContext = HttpContext;
            return InnSqlAgent.SqlAgent.ExecuteSQLMulti();
        }

        [Route("ApiWork")]
        [HttpGet]
        public object ApiWork()
        {
            InnSqlAgent.SqlAgent.HttpContext = HttpContext;
            return InnSqlAgent.SqlAgent.ApiWork();
        }

        [Route("ApiWork")]
        [HttpPost]
        public object ApiWork2()
        {
            InnSqlAgent.SqlAgent.HttpContext = HttpContext;
            return InnSqlAgent.SqlAgent.ApiWork();
        }

        [Route("ApiWorkMulti")]
        [HttpGet]
        public object ApiWorkMulti()
        {
            InnSqlAgent.SqlAgent.HttpContext = HttpContext;
            return InnSqlAgent.SqlAgent.ApiWorkMulti();
        }

        [Route("ApiWorkMulti")]
        [HttpPost]
        public object ApiWorkMulti2()
        {
            InnSqlAgent.SqlAgent.HttpContext = HttpContext;
            return InnSqlAgent.SqlAgent.ApiWorkMulti();
        }
    }
}
