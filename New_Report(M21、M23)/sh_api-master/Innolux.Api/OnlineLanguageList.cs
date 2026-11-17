using System.Collections.Generic;
using System.Linq;

namespace OnlineTranslate
{
    public class OnlineLanguageList : InnSqlAgent.IApiBase
    {
        public OnlineLanguageList()
        {
            //parameters["query"] = "查詢條件";
            description = "翻譯服務/語言列表";
        }
        public override void doWork()
        {
            string[] langs = { "es", "en", "zh-CN", "zh-TW" };//西班牙，英語，簡中，繁中
            //string query = GetParameterValueInString("query").ToLower();
            List<object> list = new List<object>();
            foreach (var lang in GTranslate.Language.LanguageDictionary)
            {
                //if (!string.IsNullOrWhiteSpace(query) && lang.Value.ToString().ToLower().IndexOf(query) == -1) continue;
                if (!langs.Contains(lang.Value.ISO6391)) continue;
                list.Add(new
                {
                    name = lang.Value.Name,
                    code = lang.Value.ISO6391,
                    code2 = lang.Value.ISO6393,
                    desc = lang.Value.NativeName
                });
            };
            returnValue = new
            {
                services = "BingTranslator,GoogleTranslator2,YandexTranslator",
                language = list
            };
        }
    }
}
