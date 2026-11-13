using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace OnlineTranslate
{
    public class OnlineTrans : InnSqlAgent.IApiBase
    {
        public OnlineTrans()
        {
            parameters["input"] = "輸入(以/分隔)";
            parameters["key"] = "回傳時json屬性(以/分隔)";
            parameters["langFrom"] = "語言(從)";
            parameters["langTo"] = "語言(翻譯)";
            parameters["single"] = "單一整句";
            description = "即時翻譯";
        }
        public override void doWork()
        {
            GTranslate.Translators.AggregateTranslator translate = new GTranslate.Translators.AggregateTranslator();
            string input = GetParameterValueInString("input");
            string key = GetParameterValueInString("key");
            string langFrom = GetParameterValueInString("langFrom");
            string langTo = GetParameterValueInString("langTo");
            string single = GetParameterValueInString("single");

            if (string.IsNullOrWhiteSpace(langFrom))
                langFrom = null;
            if (string.IsNullOrWhiteSpace(key))
                key = input;

            if (input.IndexOf('/') == -1 || !string.IsNullOrWhiteSpace(single))
            {
                string translation = getTransData(input, langTo);
                if (!string.IsNullOrWhiteSpace(translation))
                {
                    returnValue = translation;
                    return;
                }

                returnValue = translate.TranslateAsync(input, langTo, langFrom).Result.Translation;
                saveTransData(input, langTo, returnValue.ToString());
                return;
            }

            string[] inputs = input.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            string[] keys = key.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (inputs.Length != keys.Length)
                throw new Exception("count of input and key are different...");

            Dictionary<string, string> dicReturn = new Dictionary<string, string>();
            Dictionary<string, string> dicTrans = new Dictionary<string, string>();
            for (int i = 0; i < keys.Length; i++)
            {
                string s = getTransData(inputs[i], langTo);
                dicReturn[keys[i]] = s;
                if (string.IsNullOrEmpty(s))
                    dicTrans[keys[i]] = inputs[i];
            }
            if (dicTrans.Count == 0)
            {
                returnValue = dicReturn;
                return;
            }

            inputs = dicTrans.Values.ToArray();
            keys = dicTrans.Keys.ToArray();
            string[] translations = translate.TranslateAsync(string.Join('/', inputs), langTo, langFrom).Result.Translation.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < keys.Length; i++)
                dicReturn[keys[i]] = translations[i];

            saveTransData(inputs, langTo, translations);

            returnValue = dicReturn;
        }

        static DataSet _dsLocalLang = new DataSet();
        static OnlineTrans()
        {
            try
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "localLanguage.xml"))
                    _dsLocalLang.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "localLanguage.xml", XmlReadMode.ReadSchema);

            }
            catch { }
        }
        public static void updateTransData(string text, string toLang, string translation)
        {
            try
            {
                lock (_dsLocalLang)
                {
                    DataTable dt = null;
                    if (_dsLocalLang.Tables.Contains(toLang))
                        dt = _dsLocalLang.Tables[toLang];
                    else
                    {
                        dt = new DataTable();
                        dt.TableName = toLang;
                        dt.Columns.Add("text", typeof(string));
                        dt.Columns.Add("translation", typeof(string));
                        dt.PrimaryKey = new DataColumn[] { dt.Columns["text"] };
                        _dsLocalLang.Tables.Add(dt);
                    }

                    DataRow row = dt.Rows.Find(text);
                    if (row == null)
                    {
                        row = dt.NewRow();
                        row["text"] = text;
                        row["translation"] = translation;
                        dt.Rows.Add(row);
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(translation))
                            dt.Rows.Remove(row);
                        else
                            row["translation"] = translation;
                    }

                    _dsLocalLang.WriteXml(AppDomain.CurrentDomain.BaseDirectory + "localLanguage.xml", XmlWriteMode.WriteSchema);

                }
            }
            catch { }
        }
        public static void saveTransData(string text, string toLang, string translation)
        {
            try
            {
                lock (_dsLocalLang)
                {
                    DataTable dt = null;
                    if (_dsLocalLang.Tables.Contains(toLang))
                        dt = _dsLocalLang.Tables[toLang];
                    else
                    {
                        dt = new DataTable();
                        dt.TableName = toLang;
                        dt.Columns.Add("text", typeof(string));
                        dt.Columns.Add("translation", typeof(string));
                        dt.PrimaryKey = new DataColumn[] { dt.Columns["text"] };
                        _dsLocalLang.Tables.Add(dt);
                    }

                    if (dt.Rows.Find(text) != null) return;

                    DataRow row = dt.NewRow();
                    row["text"] = text;
                    row["translation"] = translation;
                    dt.Rows.Add(row);

                    _dsLocalLang.WriteXml(AppDomain.CurrentDomain.BaseDirectory + "localLanguage.xml", XmlWriteMode.WriteSchema);

                }
            }
            catch { }
        }
        public static void saveTransData(string[] text, string toLang, string[] translation)
        {
            try
            {
                lock (_dsLocalLang)
                {
                    DataTable dt = null;
                    if (_dsLocalLang.Tables.Contains(toLang))
                        dt = _dsLocalLang.Tables[toLang];
                    else
                    {
                        dt = new DataTable();
                        dt.TableName = toLang;
                        dt.Columns.Add("text", typeof(string));
                        dt.Columns.Add("translation", typeof(string));
                        dt.PrimaryKey = new DataColumn[] { dt.Columns["text"] };
                        _dsLocalLang.Tables.Add(dt);
                    }

                    bool modify = false;
                    for (int i = 0; i < text.Length; i++)
                    {
                        if (dt.Rows.Find(text[i]) != null) continue;
                        modify = true;
                        DataRow row = dt.NewRow();
                        row["text"] = text[i];
                        row["translation"] = translation[i];
                        dt.Rows.Add(row);
                    }
                    if (modify)
                        _dsLocalLang.WriteXml(AppDomain.CurrentDomain.BaseDirectory + "localLanguage.xml", XmlWriteMode.WriteSchema);

                }
            }
            catch { }
        }
        public static string getTransData(string text, string toLang)
        {
            try
            {
                DataTable dt = null;
                if (_dsLocalLang.Tables.Contains(toLang))
                    dt = _dsLocalLang.Tables[toLang];
                else
                    return null;

                DataRow row = dt.Rows.Find(text);
                if (row == null)
                    return null;
                else
                    return row["translation"].ToString();
            }
            catch { }
            return null;
        }
    }
}
