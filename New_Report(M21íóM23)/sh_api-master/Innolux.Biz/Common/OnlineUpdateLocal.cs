using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml.Linq;

namespace OnlineTranslate
{
    public class OnlineUpdateLocal : InnSqlAgent.IApiBase
    {
        public OnlineUpdateLocal()
        {
            parameters["input"] = "輸入(原始文字)";
            parameters["langTo"] = "語言(翻譯)";
            parameters["translation"] = "翻譯文字(空白表示移除)";
            description = "更新本地翻譯內容";
        }

        public override void doWork()
        {
            string input = GetParameterValueInString("input");
            string langTo = GetParameterValueInString("langTo");
            string translation = GetParameterValueInString("translation");

            if (string.IsNullOrWhiteSpace(input))
                throw new Exception("input is required...");

            GTranslate.Translators.AggregateTranslator trans = new GTranslate.Translators.AggregateTranslator();
            if (!trans.IsLanguageSupported(langTo))
                throw new Exception("lagnuage is not supported...");

            OnlineTrans.updateTransData(input, langTo, translation);
            if (string.IsNullOrWhiteSpace(translation))
                returnValue = "Remove " + input + " OK";
            else
                returnValue = "Update " + input + " OK";
        }
    }
}
