using Microsoft.Extensions.Caching.Memory;
using System.Data;

namespace InnSqlAgent_Ext
{
    public static class QueryCache
    {
        static MemoryCache memDataSet = new MemoryCache(new MemoryCacheOptions() { SizeLimit = 100000 });//SizeLimit到達時會無法加入緩存(不會報錯)，過期的或舊緩存會被移除，下一次就可以加成功
        [ThreadStatic]
        static string? _db;
        [ThreadStatic]
        static string? _sql;
        [ThreadStatic]
        static string? _value;

        public static void putCache(DataSet ds)
        {
            var op = new MemoryCacheEntryOptions();

            if (_sql?.IndexOf("mes_fab", StringComparison.OrdinalIgnoreCase) != -1)
                op.SetAbsoluteExpiration(TimeSpan.FromSeconds(100));
            else
                op.SetAbsoluteExpiration(TimeSpan.FromSeconds(10));

            op.Size = ds.Tables[0].Rows.Count;//指定此條緩存佔用的大小

            memDataSet.Set(key, ds, op);
        }

        public static DataSet getCache(string db, string sql, params object[] dbParm)
        {
            _db = db;
            _sql = sql;
            _value = "";
            foreach (object o in dbParm)
                _value += o.ToString();

            memDataSet.TryGetValue<DataSet>(key, out DataSet ds);//過期的緩存會在此時被移除(同時超過MemoryCacheOptions.ExpirationScanFrequency時(默認1分鐘))

            return ds;
        }
        static string key
        {
            get
            {
                return "db=" + _db + ",sql=" + _sql + ",value=" + _value;
            }
        }
    }
}
