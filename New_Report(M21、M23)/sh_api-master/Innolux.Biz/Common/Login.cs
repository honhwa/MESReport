using Innolux.Core.Extensions.ApiModel;
using InnSqlAgent;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Ocsp;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;

namespace Innolux.Biz.Common
{
    public class Login : ILoginUser
    {
        string tokenContent = "";
        public string TokenKey
        {
            get { return "ReportToken"; }
        }
        //public string tokenName
        //{
        //    get { return "token123"; }
        //}
        string _Token = "";
        public string Token
        {
            get { return _Token; }
            set
            {
                _Token = value;
                try
                {
                    tokenContent = Encoding.UTF8.GetString(Convert.FromBase64String(_Token));
                }
                catch { }
            }
        }

        bool _CanGetDataSet = true;
        public bool CanGetDataSet
        {
            get { return _CanGetDataSet; }
        }

        bool _CanExecuteSQL = true;
        public bool CanExecuteSQL
        {
            get { return _CanExecuteSQL; }
        }

        public object GetUserData(string name)
        {
            return tokenContent;
        }

        public bool ValidateApiWork(IApiBase apiWork)
        {
            //if (string.IsNullOrWhiteSpace(tokenContent)) return false;
            //2023-06-01 通过 worklist.html访问  验证token [reporttoken: ApiWorkList  ]
            if (tokenContent.IndexOf("ApiWorkList") > -1) return true;
   
            //2023-06-01 卡取条件：前端 SHUI程式访问 验证token [reporttoken: SHUIxxxx ]
            //if (tokenContent.IndexOf(AppConfig.Token_ID) == -1) return false;
            if (tokenContent.IndexOf("SHUI") == -1) return false;

            //非浏览器访问 禁用
            if (apiWork.httpContext.Request.Headers["User-Agent"].ToString().IndexOf("Chrome") == -1) return false;
            
            return true;
        }
    }
}
