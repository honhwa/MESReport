using System.ComponentModel;

namespace Innolux.EFCore.Model.OprationPerformance
{
    public class OperationPerformanceLcm1
    {
        public string WORK_CTR { get; set; }
        public string PASS_QTY { get; set; }
        public string TOTALRWK_QTY { get; set; }
        public string FIRSTRWK_QTY { get; set; }
        public string TOBESCRAP_QTY { get; set; }
        public string PASS1_QTY { get; set; }
        public string PASS2_QTY { get; set; }
        public string IN_QTY { get; set; }
        public string S1_WIP_QTY { get; set; }
        public string S2_WIP_QTY { get; set; }
        public string S3_WIP_QTY { get; set; }
        public string WIP_QTY { get; set; }
        public string RWK_WIP_QTY { get; set; }
        public string HOLD_WIP_QTY { get; set; }
        public string NHOLD_WIP_QTY { get; set; }
        public string RWK_OUTQTY { get; set; }
        public string RWK_IN_QTY { get; set; }
        public string RWK_PASS_QTY { get; set; }
        public string RWK_HOLDWIP_QTY { get; set; }
        public string RWK_NHOLDWIP_QTY { get; set; }
        public string RWK_TORWK_QTY { get; set; }
        public string MTO { get; set; }
        public string TOM { get; set; }
        public string RWK_TOMAIN_QTY { get; set; }
    }
    public class OperationPerformanceLcm1Req
    {
        [DefaultValue("A")]
        public string FacId { get; set; }
        [DefaultValue("A")]
        public string Building { get; set; }
        [DefaultValue("2022-09-01")]
        public string DateFrom { get; set; }
        [DefaultValue("2022-09-01")]
        public string DateTo { get; set; }
        [DefaultValue("ALL")]
        public string Shift { get; set; }
        [DefaultValue("ALL")]
        public string Floor { get; set; }
        [DefaultValue("")]
        public string WorkOrder { get; set; }
        [DefaultValue("ALL")]
        public string LcmProductType { get; set; }
        [DefaultValue("ALL")]
        public string? Size { get; set; }
        [DefaultValue("ALL")]
        public string BigProduct { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>[""]</example>
        public List<string> Product { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>[""]</example>
        public List<string> ProdId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>[""]</example>
        public List<string> LcmOwner { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>[""]</example>
        public List<string> LcdGrade { get; set; }
    }
    public class SqlSet
    {
        public string? SQL01 { get; set; }
        public string? sql_m { get; set; }
        public string? sql_m1 { get; set; }
        public string? StrSql_w { get; set; }
        public string? StrSql_we { get; set; }
        public string? StrSql_w1 { get; set; }
        public string? StrSql_w2 { get; set; }
        public string? StrSql_w2e { get; set; }
        public string? StrSql_w3 { get; set; }
        public string? StrSql_w4 { get; set; }
        public string? StrSql_ww { get; set; }
        public string? SQLbu1 { get; set; }
        public string? SQLbu2 { get; set; }


    }
}
