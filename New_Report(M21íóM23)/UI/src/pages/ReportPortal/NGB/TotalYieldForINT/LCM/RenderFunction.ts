// import { message } from 'ant-design-vue';
import { cloneDeep } from 'lodash-es';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { Console } from 'console';
import { useLoading } from '@/components/feat/Loading';
import { getfinalsum, Inserttotalrow, dclick,mergeTableRow } from '@/pages/ReportPortal/utils/NGBcommon';
import { ref} from 'vue';
import { GetCommon } from '@/api/pages/common';
import { message } from 'ant-design-vue';



const useCommonParams = AddCommonPageParams();
let bigfab=useCommonParams.Fab.slice(0,useCommonParams.Fab.length-1);
let secondTableParams = {};
let [formParams, APIMessage] = [{}, {}]
let NewPageSetting: any;
let NewFactoryType="";
let NewG_FAC="";

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});

function getImport() {
    const {        
        detail1_2,
    } = require(`@/pages/ReportPortal/NGB/TotalYieldForINT/${bigfab}/table`);    
    return { detail1_2, };
}

let {detail1_2,} = getImport();


export function getDetail(tableParams,APIMessage,formParams,PageSetting,detail1_1){
    openFullLoading(); 
    NewPageSetting=PageSetting;
    NewFactoryType=formParams["FactoryType"];
    NewG_FAC=formParams["G_FAC"];
    GetFirstData(tableParams,APIMessage,formParams,PageSetting,detail1_1);
};


async function GetFirstData(tableParams,APIMessage,formParams,PageSetting,detail1_1) {
    PageSetting.FirstColumns = [];
    PageSetting.SecondColumns = [];
    PageSetting.ThirdList = [];
    let ParamsData: any
    
    ParamsData = {
        name: "Date",
        apiName:"ExpQualityDetail",
        FactoryType: formParams["FactoryType"],
        G_FAC: formParams["G_FAC"],

        sql_dfct_where:APIMessage["sql_dfct_where"],
        sql_dggrade_where:APIMessage["sql_dggrade_where"],
        sel_oper:tableParams["dataIndex"].replace("RATE",""),
        sel_dfctcode:tableParams["DFCT_CODE"],
    
    };
   
    //console.log("ParamsData",ParamsData)

    const { Date: {
        Data: { Table: Data },
        SQL01,    
    } } = await  GetCommon(ParamsData);

    console.log("TotalYieldForINTDetail1-SQLHidden SQL01:",SQL01)
   
    PageSetting.SQLHidden = "TotalYieldForINTDetail1-SQLHidden SQL01:"+SQL01

    if (Data.length == 0) {
        //加載防護罩結束
        closeFullLoading();
        return message.warn("無數據！")
    }
    let ClickColumn = [ "ENTITY_NO", "TRANS_DATE", "TRANS_TYPE","EQUIP_NBR", "OPERATOR", "LCM_GRADE","OUTPUT_FG", "WORK_CTR", "TO_WC", "DFCT_CODE",  "STIMES_TTL", "ITEM_NBR", "PASS_QTY"]
   
    dclick(detail1_1, ClickColumn, thirdtablehandle);
    PageSetting.FirstColumns = [
        {
            title: 'prod_nbr',
            align: 'center',
            dataIndex: `PROD_NBR`,
            key: `PROD_NBR`,
            className: "v-center",
            width:90,
            customCell: (record) => {
              let text = record['PROD_NBR']
              let obj = {
                text,
                style: {}
              }
              obj['rowSpan'] = record?.['PROD_NBR-span']?.['rowspan'] || 0
              return obj
            }
          },
        ...detail1_1,
       
    ]; 
    
    let TopList: any = [];
    let TopList2: any = [];
    Data.forEach((item) => {
        item['PASS_QTY']=1
        if (item.OUTPUT_FG != " "){
            item['PASS_QTY2']=item['PASS_QTY']*(-1);
        }else{
            item['PASS_QTY2']=item['PASS_QTY'];
        }
        TopList.push(item);
    })

    let tempsum = getfinalsum(TopList, ["PASS_QTY2"], "PROD_NBR");
    Inserttotalrow(TopList, ["PROD_NBR"], ["PASS_QTY2"], "PROD_NBR");
    TopList.push(tempsum);

    TopList.forEach((item) => {
        if (item.PROD_NBR == "合計"||item.PROD_NBR == "總計"){
            item['PASS_QTY']=item['PASS_QTY2']
        }else{
            item['PASS_QTY']=item['PASS_QTY'];
        }
        TopList2.push(item);
    })

    PageSetting.moreData = TopList2;


    let combineCol1: any = {
        data: TopList2,
        mergeColNames: ['PROD_NBR','PENTITY_NO'],
        //firstMergeColNames: ['FLOOR'],
        firstMerge: ['PROD_NBR']
        };

    /**进行表格合并 */
    PageSetting.moreData=mergeTableRow(combineCol1)

    PageSetting.x = null;
    if (PageSetting.FirstColumns.length > 7) {
        PageSetting.x = 120 * PageSetting.FirstColumns.length;
    }

    
    // PageSetting.SecondColumns = detail1_2;
    // let tempsum2 = getfinalsum(Data1, ["QTY"], "DFCT_CODE");
    // // Inserttotalrow(Data1, ["DFCT_CODE"], ["QTY"], "DFCT_CODE");
    // Data1.push(tempsum2);
    // PageSetting.SecondList = Data1;
    // PageSetting.y1 = null;
    // PageSetting.y1 = 10 * (Data1.length+6);
    // if(PageSetting.y1>350){
    //     PageSetting.y1=350;
    // }
    //加載防護罩結束
    closeFullLoading();

}


async function thirdtablehandle(record, rowindex, column, ritem, index) {
    
    console.log("formParams",formParams)
    const ParamsData2: any = {
        name: "Date",
        apiName: "QualityDetail2",
        FactoryType: NewFactoryType,
        G_FAC: NewG_FAC,
        entity_no:record["ENTITY_NO"],
       };

    const { Date: { Data: { Table: Data },SQL01 } } = await  GetCommon(ParamsData2);
    NewPageSetting.SQLHidden2 = "QualityDetail3-SQLHidden SQL01:"+SQL01

    if (Data.length == 0) {
        return message.warn("無數據！")
    }
    NewPageSetting.ThirdColumns = detail1_2;
    NewPageSetting.ThirdList=Data;

    
}





