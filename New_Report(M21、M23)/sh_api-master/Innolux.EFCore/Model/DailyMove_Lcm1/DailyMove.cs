using System.ComponentModel;

namespace Innolux.EFCore.Model.DailyMove_Lcm1
{
    public class DailyMove
    {
    }

    /// <summary>
    /// 
    ///</summary>
    public class TbWeather36h
    {
        /// <summary>
        ///  城市
        ///</summary>
        public string CITY { get; set; }
        /// <summary>
        ///  週幾
        ///</summary>
        public string DATEWEEK { get; set; }
        /// <summary>
        ///  溫度
        ///</summary>
        public string TEMP { get; set; }
        /// <summary>
        ///  時間
        ///</summary>
        public string TIME { get; set; }
        /// <summary>
        ///  天氣
        ///</summary>
        public string WEATHER { get; set; }
    }


    public class PHARS
    {
        public string work_ctr { get; set; }
        public string wo_nbr { get; set; }
        public string size1 { get; set; }
        public string lcm_owner { get; set; }
        public string prod_nbr { get; set; }
        public string pd { get; set; }
        public string customer { get; set; }
        public decimal QTY { get; set; }
    }

    public class PHARS_REQ
    {
        /// <summary>
        /// 機種(product)
        /// </summary>
        /// <example>[""]</example>
        //[DefaultValue("")]
        public List<string> prod_nbr { get; set; }
        [DefaultValue("")]
        public string? Size { get; set; }
        /// <summary>
        /// big product
        /// </summary>
        [DefaultValue("ALL")]
        public string? Product { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        /// <example>2022-08-25</example>
        public string? vDate_s { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        /// <example>2022-08-25</example>
        public string? vDate_e1 { get; set; }
        /// <summary>
        /// owner
        /// </summary>
        /// <example>["ALL"]</example>
        //[DefaultValue("ALL")]
        public List<string> owner_code { get; set; }
        //[DefaultValue("")]
        /// <summary>
        /// opration
        /// </summary>
        /// <example>["ALL"]</example>
        //[DefaultValue("ALL")]
        public List<string> workGroup { get; set; }
        /// <summary>
        /// ALL或日或夜
        /// </summary>
        [DefaultValue("ALL")]
        public string? Shift { get; set; }
        /// <summary>
        /// worker_order like
        /// </summary>
        /// <example></example>
        [DefaultValue("")]
        public string? Wo_nbr { get; set; }
        [DefaultValue("")]
        public string? building { get; set; }
        [DefaultValue("")]
        public string? floor { get; set; }
        /// <summary>
        /// hour或day
        /// </summary>
        [DefaultValue("")]
        public string? intercond { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>A</example>
        //[DefaultValue("A")]
        public string? G_FAC { get; set; }
        /// <summary>
        /// work_order
        /// </summary>
        /// <example>[""]</example>
        public List<string> wo_num { get; set; }
    }

    public class SqlRes
    {
        public string sql_res { get; set; }
    }
}
