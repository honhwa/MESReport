namespace Innolux.Biz.Common
{
    public class GetApiWorks : InnSqlAgent.IApiBase
    {
        public GetApiWorks()
        {
            parameters["listdb"] = "列出所有數據庫連線名";
            parameters["query"] = "查詢名稱或描述";
            description = "查詢ApiWorks";
        }

        //執行前叫用
        protected override void beforeWork()
        {
        }

        //執行成功後叫用
        protected override void afterWork()
        {
        }

        //執行報錯時叫用
        protected override void error(ref Exception ex)
        {
        }

        //主邏輯
        public override void doWork()
        {
            if (parameters.ContainsKey("listdb") && bool.TryParse(parameters["listdb"].ToString(), out bool b) && b)
            {
                returnValue = listdb();
                return;
            }
            string query = "";
            if (parameters.ContainsKey("query"))
                query = parameters["query"].ToString();

            bool debug = query.IndexOf("-a") > -1;
            if (debug)
                query = query.Replace("-a", "");

            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (var v in GetApiWorks(query))
            {
                if ((v.className.StartsWith("apiWorkTemplate.") || v is GetApiWorks) && !debug)
                    continue;
                else
                {
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    dict["apiName"] = v.name;
                    dict["className"] = v.className;
                    dict["description"] = v.description;
                    dict["parameters"] = v.parameters;
                    list.Add(dict);
                }
            }
            returnValue = list;
        }
        List<Dictionary<string, string>> listdb()
        {
            List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
            foreach (string file in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "sqlExtension.json", SearchOption.AllDirectories))
            {
                try
                {
                    var o = Newtonsoft.Json.JsonConvert.DeserializeObject(File.ReadAllText(file)) as Newtonsoft.Json.Linq.JObject;
                    if (o == null) continue;

                    foreach (Newtonsoft.Json.Linq.JProperty p in o.Properties())
                    {
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        list.Add(dic);

                        dic["dbName"] = p.Name;
                        Newtonsoft.Json.Linq.JObject dbobj = p.Value as Newtonsoft.Json.Linq.JObject;
                        dic["hostName"] = (dbobj.SelectToken("dbHost") as Newtonsoft.Json.Linq.JValue).Value.ToString();
                        dic["dbType"] = (dbobj.SelectToken("dbConnectionType") as Newtonsoft.Json.Linq.JValue).Value.ToString();
                        dic["initCatelog"] = (dbobj.SelectToken("dbInitCatelog") as Newtonsoft.Json.Linq.JValue).Value.ToString();
                        dic["userName"] = (dbobj.SelectToken("dbUser") as Newtonsoft.Json.Linq.JValue).Value.ToString();
                    }

                    break;
                }
                catch { }
            }
            return list;
        }
    }
}
