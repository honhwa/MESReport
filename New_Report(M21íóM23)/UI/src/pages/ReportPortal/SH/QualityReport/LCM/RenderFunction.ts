// import { message } from 'ant-design-vue';
import { cloneDeep } from 'lodash-es';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { Console } from 'console';
import { useLoading } from '@/components/feat/Loading';
import { getfinalsum, Inserttotalrow, dclick,mergeTableRow } from '@/pages/ReportPortal/utils/SHcommon';
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
        detail1_3,
        datakey, ChildProps
    } = require(`@/pages/ReportPortal/SH/QualityReport/${bigfab}/table`);    
    return { detail1_2, detail1_3, datakey, ChildProps };
}

let {detail1_2, detail1_3, datakey, ChildProps} = getImport();


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

    if (formParams["ShowMode"]=="1300to1600"){
        ParamsData = {
            name: "Date",
            apiName:"Quality1300to1600Detail1",
            FactoryType: formParams["FactoryType"],
            G_FAC: formParams["G_FAC"],

            FromDate:APIMessage["FromDate"],
            ToDate:APIMessage["ToDate"],
            ShowMode:APIMessage["ShowMode"],
            col0:tableParams["title"].replace('*', ''),
            row0:tableParams['DFCT_CODE'],
        
            StrSql_w:APIMessage['StrSql_w'],
            StrSql_w1:APIMessage["StrSql_w1"],
            StrSql_w3:APIMessage["StrSql_w3"],
            guolv:APIMessage["guolv"],
        };
    }else{
        ParamsData = {
            name: "Date",
            apiName:"QualityDetail1",
            FactoryType: formParams["FactoryType"],
            G_FAC: formParams["G_FAC"],

            FromDate:APIMessage["FromDate"],
            ToDate:APIMessage["ToDate"],
            ShowMode:APIMessage["ShowMode"],
            col0:tableParams["title"].replace('*', ''),
            row0:tableParams['DFCT_CODE'],
        
            StrSql_w:APIMessage['StrSql_w'],
            StrSql_w1:APIMessage["StrSql_w1"],
            StrSql_wf:APIMessage["StrSql_wf"],
            rework:APIMessage["rework"],
            guolv:APIMessage["guolv"],
            stroper:APIMessage["stroper"],
            FAST_RWK_FLG_type:APIMessage["FAST_RWK_FLG_type"],
        };

    }

    let { Date: { Data: DataA, Data_2: DataB,
        SQL01,
        SQL03,
        FromDate,
        ToDate,
        ShowMode ,    
    } } = await  GetCommon(ParamsData);
    let Data0 = DataA["Table"];
    let Data1 = DataB?.Table;
    PageSetting.SQLHidden = "QualityDetail1-SQLHidden SQL01:"+SQL01+";<END> SQL02:"+SQL03

    if (Data0.length == 0) {
        //加載防護罩結束
        closeFullLoading();
        return message.warn("無數據！")
    }
    let ClickColumn = [ "ENTITY_NO", "TRANS_DATE1", "TRANS_DATE2","EQUIP_NBR", "OPERATOR", "OPERATORS","OUTPUT_FG", "WORK_CTR", "TO_WC", "DFCT_CODE", "PATTERN_CODE","PREV_DFCT_CODE", "STIMES_TTL", "ITEM_NBR", "OPERATOR_1600", "PASS_QTY"]
   
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
    Data0.forEach((item) => {
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

    if (Data1 == null || Data1.length == 0) {
        //加載防護罩結束
        closeFullLoading();
        return message.warn("表格2無數據！")
    }
    // Data1.forEach(item => {
    //     item["Rate"] = ((item["QTY"] / item["TOTAL"]) * 100).toFixed(2);
    // })
    PageSetting.SecondColumns = detail1_2;
    let tempsum2 = getfinalsum(Data1, ["QTY"], "DFCT_CODE");
    // Inserttotalrow(Data1, ["DFCT_CODE"], ["QTY"], "DFCT_CODE");
    Data1.push(tempsum2);
    PageSetting.SecondList = Data1;
    PageSetting.y1 = null;
    PageSetting.y1 = 10 * (Data1.length+6);
    if(PageSetting.y1>350){
        PageSetting.y1=350;
    }
    //加載防護罩結束
    closeFullLoading();

}


async function thirdtablehandle(record, rowindex, column, ritem, index) {
    
    //console.log("formParams",formParams)
    const ParamsData2: any = {
        name: "Date",
        apiName: "QualityDetail2",
        FactoryType: NewFactoryType,
        G_FAC: NewG_FAC,
        entity_no:record["ENTITY_NO"],
        trans_date1:record["TRANS_DATE1"],
        trans_date2:record["TRANS_DATE2"],
       };

    const { Date: { Data: { Table: Data },SQL01 } } = await  GetCommon(ParamsData2);
    NewPageSetting.SQLHidden2 = "QualityDetail3-SQLHidden SQL01:"+SQL01

    if (Data.length == 0) {
        return message.warn("無數據！")
    }
    NewPageSetting.ThirdColumns = detail1_3;
    NewPageSetting.ThirdList=Data;
    closeFullLoading();

    // let finalData: object[] = [];
    // let obj = {};
    // let obj2 = { "WORK_CTR": 0 };
    // // 清除不存在于datakey = [1050, 1100, 1200, 1300, 1400, 1500, 1600, 1700, 1900];
    // Data = Data.filter(item => {
    //     return datakey.includes(item["WORK_CTR"])
    // })

    // for (let i = 0; i < Data.length; i++) {
    //     if (Data[i]["PROD_NBR"].trim() != '') {
    //         ChildProps.forEach(item => {
    //             Data[i][`${Data[i]["WORK_CTR"]}-${item}`] = Data[i][item]
    //         });
    //         // 如果和并到最后发现，obj的workctr比下一个大或者等于，说明走了一轮
    //         if (obj2["WORK_CTR"] >= Data[i]["WORK_CTR"]) {
    //             finalData.push(obj2);
    //             obj2 = Data[i];
    //         } else {
    //             obj2 = Object.assign(obj2, Data[i]);
    //         }
    //     }
    // }
    // finalData.push(obj2);
    // let tempobj = {};
    // finalData.forEach(item => {
    //     let work = item["WORK_CTR"];
    //     if (!tempobj[work]) {
    //         tempobj[work] = 0;
    //     } tempobj[work]++;

    // })
    // let arr: number[] = [];
    // // 数组排序
    // for (let i in tempobj) {
    //     arr.push(tempobj[i])
    // }
    // arr.sort((a, b) => {
    //     return a - b
    // })
    // let maxcount = 0;
    // if (arr[arr.length - 1] > 10) {
    //     maxcount = arr[arr.length - 2]
    // } if (arr.length == 2 && arr[0] == 1 && arr[1] == 1) {
    //     maxcount = finalData.length;
    // } if (arr.length == 3 && arr[0] == 1 && arr[1] == 1 && arr[2] == 1) {
    //     maxcount = 2;
    // } else {
    //     maxcount = finalData.length;
    // }
    // PageSetting.ThirdColumns = detail1_3;
    // PageSetting.ThirdList = finalData.slice(0, maxcount);
}


// async function GetThirdtable() {
//     PageSetting.ThirdList = [];
//     const ParamsData2: any = {
//         name: "Date",
//         apiName: "QualityDetail2",
//         FactoryType: useCommonParams.Fab,
//         G_FAC: useCommonParams.G_FAC_COM,
//         entity_no:record["ENTITY_NO"],
//        };

//     let { Date: { Table: Data,
//         SQL01,    
//     } } = await  GetCommon(ParamsData2);

//     if (Data.length == 0) {
//         return message.warn("無數據！")
//     }
//     PageSetting.ThirdColumns = detail1_3;
//     PageSetting.ThirdList=Data;

//     // let finalData: object[] = [];
//     // let obj = {};
//     // let obj2 = { "WORK_CTR": 0 };
//     // // 清除不存在于datakey = [1050, 1100, 1200, 1300, 1400, 1500, 1600, 1700, 1900];
//     // Data = Data.filter(item => {
//     //     return datakey.includes(item["WORK_CTR"])
//     // })

//     // for (let i = 0; i < Data.length; i++) {
//     //     if (Data[i]["PROD_NBR"].trim() != '') {
//     //         ChildProps.forEach(item => {
//     //             Data[i][`${Data[i]["WORK_CTR"]}-${item}`] = Data[i][item]
//     //         });
//     //         // 如果和并到最后发现，obj的workctr比下一个大或者等于，说明走了一轮
//     //         if (obj2["WORK_CTR"] >= Data[i]["WORK_CTR"]) {
//     //             finalData.push(obj2);
//     //             obj2 = Data[i];
//     //         } else {
//     //             obj2 = Object.assign(obj2, Data[i]);
//     //         }
//     //     }
//     // }
//     // finalData.push(obj2);
//     // let tempobj = {};
//     // finalData.forEach(item => {
//     //     let work = item["WORK_CTR"];
//     //     if (!tempobj[work]) {
//     //         tempobj[work] = 0;
//     //     } tempobj[work]++;

//     // })
//     // let arr: number[] = [];
//     // // 数组排序
//     // for (let i in tempobj) {
//     //     arr.push(tempobj[i])
//     // }
//     // arr.sort((a, b) => {
//     //     return a - b
//     // })
//     // let maxcount = 0;
//     // if (arr[arr.length - 1] > 10) {
//     //     maxcount = arr[arr.length - 2]
//     // } if (arr.length == 2 && arr[0] == 1 && arr[1] == 1) {
//     //     maxcount = finalData.length;
//     // } if (arr.length == 3 && arr[0] == 1 && arr[1] == 1 && arr[2] == 1) {
//     //     maxcount = 2;
//     // } else {
//     //     maxcount = finalData.length;
//     // }
//     // PageSetting.ThirdColumns = detail1_3;
//     // PageSetting.ThirdList = finalData.slice(0, maxcount);
// }


