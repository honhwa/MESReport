using System.ComponentModel;

namespace Innolux.EFCore.Model.OperationPerformance
{
    public class OperationPerformance
    {
    }

    public class Rework_RES//名字+RES：对应每一个API的接收的数据，名字：对应每一个API的传回的数据
    {
        /// <summary>
        /// 对应operation
        /// </summary>
        /// 
        [DefaultValue("")]
        public string? col1 { get; set; }

        [DefaultValue("Out rwk(1)以上")]
        public string? col0 { get; set; }



        [DefaultValue("2403")]
        public string? row0 { get; set; }
        [DefaultValue("2403")]
        public string? row1 { get; set; }



        [DefaultValue("and  main_wc > 1000")]
        public string? StrSql_w { get; set; }

        [DefaultValue("AND to_wc > 0")]
        public string? StrSql_w1 { get; set; }

        [DefaultValue("and  work_ctr > 1000")]
        public string? StrSql_w2 { get; set; }

        [DefaultValue("")]
        public string? Sqlbu1 { get; set; }

        [DefaultValue("")]
        public string? Sqlbu2 { get; set; }


        [DefaultValue("")]
        public string? vdate_e { get; set; }

        [DefaultValue("AND acct_date >= '2022-09-14' AND acct_date <= '2022-09-14'")]
        public string? sql_m { get; set; }

        [DefaultValue("and trans_date >= '2022-09-14 08:00:00.000000' and trans_date <= '2022-09-15 08:00:00.000000'")]
        public string? sql_m1 { get; set; }

        [DefaultValue("")]
        public string? owner_code { get; set; }

        [DefaultValue("A")]
        public string? G_FAC { get; set; }

    }
    public class Rework
    {
        public string WORK_CTR { get; set; }
        public string QTY { get; set; }

    }
    public class ReworkSet
    {
        public string SQL01 { get; set; }
        public string G_FAC { get; set; }
        public string StrSql_w2 { get; set; }
        public string StrSql_w1 { get; set; }
        public string StrSql_w { get; set; }
        public string col0 { get; set; }
        public string col1 { get; set; }
        public string row0 { get; set; }
        public string row1 { get; set; }
        public string sql_p { get; set; }
        public string sql_m { get; set; }
        public string sql_m1 { get; set; }
        public string sqlbu1 { get; set; }
        public string sqlbu2 { get; set; }
        public string vdate_s { get; set; }
        public string vdate_e { get; set; }
        public string owner_code { get; set; }

    }
    public class OperationPerformanceDetail_RES
    {
        /// <summary>
        /// 对应operation
        /// </summary>
        /// 
        [DefaultValue("")]
        public string? col1 { get; set; }

        [DefaultValue("WIP")]
        public string? col0 { get; set; }

        [DefaultValue("")]
        public string? StrSql_w { get; set; }

        [DefaultValue("")]
        public string? StrSql_w1 { get; set; }

        [DefaultValue("")]
        public string? StrSql_w2 { get; set; }

        [DefaultValue("")]
        public string? Sqlbu1 { get; set; }

        [DefaultValue("")]
        public string? Sqlbu2 { get; set; }

        [DefaultValue("")]
        public string? owner_code { get; set; }

        [DefaultValue("")]
        public string? vdate_e { get; set; }

        [DefaultValue("")]
        public string? sql_m { get; set; }

        [DefaultValue("")]
        public string? sql_m1 { get; set; }

        [DefaultValue("2401")]
        public string? row1 { get; set; }

        [DefaultValue("")]
        public string? row0 { get; set; }
        [DefaultValue("")]
        public string? row2 { get; set; }
        [DefaultValue("")]
        public string? row3 { get; set; }

        [DefaultValue("A")]
        public string? G_FAC { get; set; }

    }
    public class OperationPerformanceDetail
    {
        public string OPER { get; set; }
        public string PROD_NBR { get; set; }
        public string ENTITY_NO { get; set; }
        public string ACCT_DATE { get; set; }
        public string TRANS_TYPE { get; set; }
        public string TRANS_DATE { get; set; }
        public string EQUIP_NBR { get; set; }
        public string LAST_TRANS_DT { get; set; }
        public string PNL_ID { get; set; }
        public string OPERATOR { get; set; }
        public string LAST_EQP_NBR { get; set; }
        public string OUTPUT_FG { get; set; }
        public string WORK_CTR { get; set; }
        public string TO_WC { get; set; }
        public string DFCT_CODE { get; set; }
        public string PRE_DFCTCODE { get; set; }
        public string STIMES_TTL { get; set; }
        public string ITEM_NBR { get; set; }
        public string QTY { get; set; }
        public string PASS_QTY { get; set; }
    }
    public class OperationPerformanceDetailSet
    {
        public string SQL01 { get; set; }
        public string G_FAC { get; set; }
        public string col0 { get; set; }
        public string col1 { get; set; }
        public string trans_date { get; set; }
        public string prod_nbr { get; set; }
        public string Operator { get; set; }
        public string work_ctr { get; set; }
        public string to_wc { get; set; }
        public string stimes_ttl { get; set; }
        public string equip_nbr { get; set; }
        public string acct_date { get; set; }
        public string Sqlbu2 { get; set; }
        //public string StrTit1 { get; set; }
    }
    public class OperationPerformancePassQty_RES
    {
        /// <summary>
        /// 对应operation
        /// </summary>
        [DefaultValue("1300")]
        public string? row1 { get; set; }

        [DefaultValue("AND main_wc > 1000")]
        public string? StrSql_w { get; set; }

        [DefaultValue("")]
        public string? StrSql_w4 { get; set; }

        [DefaultValue("")]
        public string? Sqlbu2 { get; set; }

        [DefaultValue("\tAND acct_date >= '2022-09-01'\r\n\tAND acct_date <= '2022-09-01'")]
        public string? sql_m { get; set; }

        [DefaultValue("A")]
        public string? G_FAC { get; set; }

    }
    public class OperationPerformancePassQty
    {
        public string PROD_NBR { get; set; }
        public string VER { get; set; }
        public string EQUIP_NBR { get; set; }
        public string SHIFT_ID { get; set; }
        public string PASS_QTY { get; set; }
        public string RWK_QTY { get; set; }
    }
    public class OperationPerformancePassQtySet
    {
        public string SQL01 { get; set; }
        public string G_FAC { get; set; }
        public string StrSql_w { get; set; }
        public string StrSql_w4 { get; set; }
        public string col0 { get; set; }
        public string col1 { get; set; }
        public string row0 { get; set; }
        public string row2 { get; set; }
        public string row3 { get; set; }
        public string row1 { get; set; }
        public string sql_m { get; set; }
        public string sqlbu2 { get; set; }
        public string vdate_s { get; set; }
        public string vdate_e { get; set; }
    }
    public class OperationPerformanceReworkQty_RES
    {
        /// <summary>
        /// 对应operation
        ///   "row1": 1350,
        /// </summary>
        [DefaultValue("1350")]
        public string? row1 { get; set; }

        [DefaultValue("AND main_wc > 1000")]
        public string? StrSql_w { get; set; }

        [DefaultValue(" ")]
        public string? Sqlbu2 { get; set; }

        [DefaultValue("AND acct_date >= '2022-09-02' AND acct_date <= '2022-09-02'")]
        public string? sql_m { get; set; }

        [DefaultValue("A")]
        public string? G_FAC { get; set; }

    }
    public class OperationPerformanceReworkQty
    {

        public string CODE { get; set; }
        public string CODE_DESC { get; set; }
        public string COUNT { get; set; }
    }
    public class OperationPerformanceReworkQtySet
    {

        public string SQL01 { get; set; }
        public string G_FAC { get; set; }
        public string StrSql_w2 { get; set; }
        public string StrSql_w1 { get; set; }
        public string StrSql_w { get; set; }
        public string col0 { get; set; }
        public string col1 { get; set; }
        public string row2 { get; set; }
        public string row0 { get; set; }
        public string row1 { get; set; }
        public string sql_m { get; set; }
        public string sqlbu1 { get; set; }
        public string sqlbu2 { get; set; }
    }
    public class WipQty_RES
    {
        /// <summary>
        /// </summary>
        /// 
        [DefaultValue("1350")]
        public string? row1 { get; set; }

        [DefaultValue("WIP")]
        public string? col0 { get; set; }

        [DefaultValue("")]
        public string? StrSql_w { get; set; }

        [DefaultValue("")]
        public string? StrSql_w1 { get; set; }

        [DefaultValue("AND work_ctr > 1000")]
        public string? StrSql_w2 { get; set; }

        [DefaultValue("")]
        public string? Sqlbu1 { get; set; }

        [DefaultValue("2022-09-02")]
        public string? vdate_e { get; set; }

        [DefaultValue("")]
        public string? owner_code { get; set; }

        [DefaultValue("A")]
        public string? G_FAC { get; set; }

    }
    public class WipQty
    {

        public string PROD_NBR { get; set; }
        public string VER { get; set; }
        public string COUNT { get; set; }
    }
    public class WipQtySet
    {

        public string SQL01 { get; set; }
        public string G_FAC { get; set; }
        public string StrSql_w { get; set; }
        public string StrSql_w1 { get; set; }
        public string StrSql_w2 { get; set; }
        public string col0 { get; set; }
        public string col1 { get; set; }
        public string row2 { get; set; }
        public string row3 { get; set; }
        public string row0 { get; set; }
        public string row1 { get; set; }
        public string sqlbu1 { get; set; }
        public string owner_code { get; set; }
    }
    public class PanelWipStayChart_RES
    {
        /// <summary>
        /// </summary>
        [DefaultValue("2022-09-02")]
        public string? acct_date { get; set; }

        [DefaultValue("")]
        public string? trans_date { get; set; }

        [DefaultValue("")]
        public string? prod_nbr { get; set; }

        [DefaultValue("")]
        public string? Operator { get; set; }

        [DefaultValue("2401")]
        public string? work_ctr { get; set; }

        [DefaultValue("2110")]
        public string? to_wc { get; set; }

        [DefaultValue("")]
        public string? stimes_ttl { get; set; }

        [DefaultValue("")]
        public string? equip_nbr { get; set; }

        [DefaultValue("")]
        public string? col0 { get; set; }

        [DefaultValue("")]
        public string? col1 { get; set; }

        [DefaultValue("")]
        public string? Sqlbu2 { get; set; }

        [DefaultValue("A")]
        public string? G_FAC { get; set; }

    }
    public class PanelWipStayChart
    {
        public string PNL_ID { get; set; }
        public string TRANS_DATE { get; set; }
        public string OPERATOR { get; set; }
        public string WORK_CTR { get; set; }
        public string TO_WC { get; set; }
        public string STIMES_TTL { get; set; }
        public string MAT_TYPE { get; set; }
        public string MAT_ID { get; set; }
        public string LOT_NUMBER { get; set; }
        public string DFCT_CODE { get; set; }
    }
    public class OperationPerformanceDetail9_RES
    {
        /// <summary>
        /// </summary>
        [DefaultValue("2V320BJ8Q230S")]
        public string? prod_no { get; set; }

        [DefaultValue("AOPS03")]
        public string? equip_no { get; set; }

        [DefaultValue("A")]
        public string? shift_no { get; set; }

        [DefaultValue("1100")]
        public string? row1 { get; set; }

        [DefaultValue("")]
        public string? StrSql_w4 { get; set; }

        [DefaultValue("")]
        public string? Sqlbu2 { get; set; }

        [DefaultValue("2022-09-02")]
        public string? vdate_e { get; set; }

        [DefaultValue("AND acct_date >= '2022-09-05' AND acct_date <= '2022-09-05'")]
        public string? sql_m { get; set; }

        [DefaultValue("")]
        public string? sql_m1 { get; set; }

        [DefaultValue("A")]
        public string? G_FAC { get; set; }

    }
    public class OperationPerformanceDetail9
    {
        public string PROD_NBR { get; set; }
        public string TIME { get; set; }
        public string PASS_QTY { get; set; }

    }
    public class OperationPerformanceDetail9Set
    {
        public string SQL01 { get; set; }
        public string StrSql_w { get; set; }
        public string prod_nbr { get; set; }
        public string dfct_code { get; set; }

    }
    public class WipQty10_RES
    {
        /// <summary>
        /// </summary>
        [DefaultValue("1465")]
        public string? row1 { get; set; }

        [DefaultValue("")]
        public string? col0 { get; set; }

        [DefaultValue("")]
        public string? StrSql_w { get; set; }

        [DefaultValue("")]
        public string? StrSql_w1 { get; set; }

        [DefaultValue("AND work_ctr > 1000")]
        public string? StrSql_w2 { get; set; }

        [DefaultValue("")]
        public string? Sqlbu1 { get; set; }

        [DefaultValue("2022-09-02")]
        public string? vdate_e { get; set; }

        [DefaultValue("")]
        public string? owner_code { get; set; }

        [DefaultValue("A")]
        public string? G_FAC { get; set; }

    }
    public class WipQty10
    {

        public string PROD_NBR { get; set; }
        public string VER { get; set; }
        public string COUNT { get; set; }
    }
    public class WipQty10Set
    {
        public string SQL01 { get; set; }
        public string G_FAC { get; set; }
        public string StrSql_w { get; set; }
        public string StrSql_w1 { get; set; }
        public string StrSql_w2 { get; set; }
        public string col0 { get; set; }
        public string col1 { get; set; }
        public string row2 { get; set; }
        public string row3 { get; set; }
        public string row0 { get; set; }
        public string row1 { get; set; }
        public string sqlbu1 { get; set; }
        public string owner_code { get; set; }

    }
    public class OperationPerformancePassQtyZ_RES
    {
        /// <summary>
        /// </summary>
        /// 
        [DefaultValue("1300")]
        public string? row0 { get; set; }

        [DefaultValue("后→前")]
        public string? col0 { get; set; }

        [DefaultValue("")]
        public string? TO_ERP_OPER { get; set; }

        [DefaultValue("")]
        public string? shift { get; set; }

        [DefaultValue("")]
        public string? floor { get; set; }

        [DefaultValue("")]
        public string? vDate_s { get; set; }

        [DefaultValue("")]
        public string? vDate_e1 { get; set; }

        [DefaultValue("")]
        public string? StrSql_ww { get; set; }

        [DefaultValue("")]
        public string? Sqlbu2 { get; set; }

        [DefaultValue("AND trans_date >= '2022-09-05 08:00:00.000000' AND trans_date <= '2022-09-06 08:00:00.000000'")]
        public string? sql_m1 { get; set; }

        [DefaultValue("AND oper > 1000")]
        public string? sql_p { get; set; }

        [DefaultValue("A")]
        public string? G_FAC { get; set; }

    }
    public class OperationPerformancePassQtyZ
    {
        public string PROD_NBR { get; set; }
        public string FR_ERP_OPER { get; set; }
        public string TOM { get; set; }

    }
    public class OperationPerformancePassQtyZSet
    {
        public string SQL01 { get; set; }
        public string G_FAC { get; set; }
        public string StrSql_ww { get; set; }
        public string col1 { get; set; }
        public string col2 { get; set; }
        public string row0 { get; set; }
        public string rows { get; set; }
        public string Row1 { get; set; }
        public string sql_p { get; set; }
        public string sql_m1 { get; set; }
        public string sqlbu2 { get; set; }
        public string vdate_s { get; set; }
        public string vdate_e1 { get; set; }
        public string Shift { get; set; }
        public string Floor { get; set; }
        public string TO_ERP_OPER { get; set; }


    }
    public class OperationPerformancePassQtyZ1_RES
    {
        /// <summary>
        /// </summary>
        /// 
        [DefaultValue("2V400HJ9E1H0S")]
        public string? prod_nbr { get; set; }

        [DefaultValue("1800")]
        public string? from_oper { get; set; }

        [DefaultValue("")]
        public string? to_oper { get; set; }

        [DefaultValue("后站點→前站點")]
        public string? condition { get; set; }

        [DefaultValue("")]
        public string? Shift { get; set; }

        [DefaultValue("")]
        public string? Floor { get; set; }

        [DefaultValue("")]
        public string? StrSql_ww { get; set; }

        [DefaultValue("")]
        public string? Sqlbu2 { get; set; }

        [DefaultValue("AND trans_date >= '2022-09-05 08:00:00.000000' AND trans_date <= '2022-09-06 08:00:00.000000'")]
        public string? sql_m1 { get; set; }

        [DefaultValue("AND trans_date >= '2022-09-05 08:00:00.000000' AND trans_date <= '2022-09-06 08:00:00.000000'")]
        public string? sql_m2 { get; set; }

        [DefaultValue("AND oper > 1000")]
        public string? sql_p { get; set; }

        [DefaultValue("A")]
        public string? G_FAC { get; set; }

    }
    public class OperationPerformancePassQtyZ1
    {
        public string FAC_ID { get; set; }
        public string TRANS_NBR { get; set; }
        public string TRANS_DATE { get; set; }
        public string PROD_NBR { get; set; }
        public string VER { get; set; }
        public string PNL_ID { get; set; }
        public string OPERATORS { get; set; }
        public string TRANS_TYPE { get; set; }
        public string OPER { get; set; }
        public string COMMENTS { get; set; }
        public string MAT_TYPE { get; set; }
        public string MAT_ID { get; set; }
        public string LOT_NUMBER { get; set; }
        public string FR_ERP_OPER { get; set; }
        public string TO_ERP_OPER { get; set; }
        public string WO_NBR { get; set; }
        public string EQUIP_NBR { get; set; }
    }
    public class OperationPerformancePassQtyZ1Set
    {
        public string SQL01 { get; set; }
        public string G_FAC { get; set; }
        public string Sqlbu2 { get; set; }
        public string Sqlbu1 { get; set; }
        public string StrSql_ww { get; set; }
        public string col0 { get; set; }
        public string col1 { get; set; }
        public string row0 { get; set; }
        public string top_oper { get; set; }
        public string Shift { get; set; }
        public string Floor { get; set; }
        public string sql_m1 { get; set; }
        public string sql_m2 { get; set; }
        public string vdate_s { get; set; }
        public string vdate_e { get; set; }

    }
}