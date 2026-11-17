import { AddCommonPageParamsWithOut } from '@/store/module/commonpage';

import {
    GetCommon

} from '@/api/pages/common';
// import { message } from 'ant-design-vue';
import { cloneDeep } from 'lodash-es';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { Console } from 'console';
import { useLoading } from '@/components/feat/Loading';
import { ref} from 'vue';
import { message } from 'ant-design-vue';
import { ReportCommon } from '@/store/module/ReportCommon';

// import { Object } from 'lodash';
const RightTopPageShow= ref(false);
const RightBottomPageShow = ref(false);


// let useOperation = AddCommonPageParamsWithOut();
const useCommonParams = AddCommonPageParams();
let useReportCommon = ReportCommon();
//let ReportNameParams=`OperationPerformance-${useCommonParams.Fab}`;
// let tableParams = useOperation.FirstTableParams;
let secondTableParams = {};
let [formParams, APIMessage, firstTableParams, thirdTableParams] = [{}, {}, {}, {}]
const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});

function getImport() {
    const OperationColumsData = require(`@/pages/ReportPortal/SH/OperationPerformance/BL/Table`);
    return OperationColumsData
    // return { OperDetail3Columns, WipColumns,WipDetailCoulmns };
}
let OperationColumsData = getImport();
// 插入行合计,dataSource数据源，standard根据属性合计，calcuList要合计的列，是否是负数isNegative,当前点击的列CilckKey，合计属性sumprops
function Inserttotalrow(dataSource: any[], standard: string, calcuList: any[], CilckKey: string, sumprops: string, isNegative: boolean = false) {
        let temp: any = cloneDeep(dataSource[0]);
        let totalsum: any[] = [];
        let totalindex: any[] = [];
        if (
            ['TOTALRWK_QTY'].includes(CilckKey)
        ) {
            let temparr = JSON.parse(JSON.stringify(dataSource));
            for (let i = 0; i < temparr.length; i++) {
                if (Number(temparr[i]["STIMES_TTL"]) == 0) {
                    temparr[i]["PASS_QTY"] = -Number(temparr[i]["PASS_QTY"]);
                }
            }
            dataSource = temparr;
            temp = cloneDeep(dataSource[0]);

        }
        for (let i = 0; i < dataSource.length - 1; i++) {
            if (dataSource[i][standard] == dataSource[i + 1][standard]) {
                for (let j = 0; j < calcuList.length; j++) {
                    if (isNaN(dataSource[i][calcuList[j]]) || typeof dataSource[i][calcuList[j]] != 'number') {
                        dataSource[i][calcuList[j]] = Number(dataSource[i][calcuList[j]]);
                    } else if (isNaN(dataSource[i + 1][calcuList[j]]) || typeof dataSource[i + 1][calcuList[j]] != 'number') {
                        dataSource[i + 1][calcuList[j]] = Number(dataSource[i + 1][calcuList[j]]);
                    }
                    temp[calcuList[j]] = Number(temp[calcuList[j]]);
                    temp[calcuList[j]] += Number(dataSource[i + 1][calcuList[j]]);
                }
            } else {
                Object.keys(temp).forEach((item) => {
                    if (!calcuList.includes(item)) {
                        temp[item] = '';
                    }
                    if (item == sumprops) {
                        temp[item] = '合計';
                    }
                    // 合計需要是負數的處理
                    // 不包含在數組裏面的就是整數
                    if (isNegative && calcuList.includes(item)) {
                        temp[item] = -Math.ceil(temp[item]);
                    }
                });

                totalindex.push(i + 1);
                totalsum.push(temp);
                temp = cloneDeep(dataSource[i + 1]);
                continue;
            }
        }
        Object.keys(temp).forEach((item) => {
            if (!calcuList.includes(item)) {
                temp[item] = '';
            }

            if (item == sumprops) {
                temp[item] = '合計';
            }
            // 合計需要是負數的處理
            if (isNegative && calcuList.includes(item)) {
                temp[item] = -Math.ceil(temp[item]);
            }
        });
        totalsum.push(temp);
        totalindex.push(dataSource.length);
        return {
            insertrowList: totalsum,
            insertIndexList: totalindex
        };
    
}

// 添加最后一行的总计
// dataSource数据源，calcuList要计算的属性数组,屬性值propList
function getfinalsum(dataSource: any[], calcuList: any[], CilckKey: string, sumprops: string, isNegative: boolean = false) {
    if (Array.isArray(dataSource) && dataSource.length != 0) {
        const tempObj = cloneDeep(dataSource[0]);
        calcuList.forEach((el) => {
            for (let i = 1; i < dataSource.length; i++) {
                if (
                    ['TOTALRWK_QTY'].includes(CilckKey)
                ) {
                    tempObj[el] = Number(tempObj[el]);
                    if (Number(tempObj['STIMES_TTL']) == 0) {
                        tempObj[el] = -Number(tempObj[el]);
                    }
                    tempObj[el] +=
                        Number(dataSource[i]['STIMES_TTL']) == 0 ? -Number(dataSource[i][el]) : Number(dataSource[i][el]);
                } else {
                    tempObj[el] = Number(tempObj[el]);
                    tempObj[el] += Number(dataSource[i][el]);
                }
            }
        });
        // 不包含在属性数组中的属性设置为空
        Object.keys(tempObj).forEach((el,) => {
            if (!calcuList.includes(el)) {
                tempObj[el] = null;
            }
            if (el == sumprops) {
                tempObj[el] = '合計';
            }
            // 總計需要是負數的處理
            // 不包含在數組裏面的就是整數
            if (isNegative && calcuList.includes(el)) {
                tempObj[el] = -tempObj[el];
            }
        });
        return tempObj;
    }
}

///////////////----------------上表格----------------------------//////////////////
////點擊pass qty 上表格 跳二階頁面
async function getPassQtyFirstData(CilckKey: string, WORK_CTR: string, PageSetting: any) {   
    // 上表格的OUt
    const ParamsData: any = {
        name: "Date",
        apiName: "OperationPerformancePassQty",
        FactoryType: formParams["FactoryType"],
        G_FAC: formParams["G_FAC"],
        FromDate:formParams["FromDate"],
        ToDate:formParams["ToDate"],
        sql_Shift:APIMessage["sql_Shift"],
        sql_IncludeBL:APIMessage["sql_IncludeBL"],
        sql_Floor:APIMessage["sql_Floor"],
        sql_WorkOrder:APIMessage["sql_WorkOrder"],
        sql_Product:APIMessage["sql_Product"],
        sql_LcmOwner:APIMessage["sql_LcmOwner"],
        sql_LcdOwner:APIMessage["sql_LcdOwner"],        
        row1: WORK_CTR
    };

    try{

        const { Date: { Data: { Table: Data },SQL01 } } = await GetCommon(ParamsData);

        //console.log("OperationPerformancePassQty-SQLFirstDetail:",SQL01)
        PageSetting.SQLHidden="OperationPerformancePassQty-SQLFirstDetail:"+SQL01;


        PageSetting['TopTitle'] = `${formParams["FactoryType"]} Operation Performance Pass Qty`

        if (Data.length != 0) { 
            PageSetting['Columns'] = OperationColumsData.PassOperDetailColumns;
            PageSetting['List'] = Data.map((item) => {
                item['GOOD_PASS'] = item['PASS_QTY'] - item['RWK_QTY'];
                return item;
            });
            if (CilckKey == "PASS_QTY") {
                // 要計算的列
                let calcuList = ["PASS_QTY", "GOOD_PASS", "RWK_QTY"];

                // 添加總計     
                PageSetting['List'].push(getfinalsum(PageSetting['List'], calcuList, CilckKey, 'PROD_NBR'));
            }
       }else{
        message.warning("無數據！")
      }
        closeFullLoading();

    }catch(error: any){ 
           closeFullLoading();
           return Promise.reject(error); 
        }
    
}
////點擊pass qty上表格二階頁面 跳三階頁面/////////
async function getPassQtyDeatil(WORK_CTR: string, PageSetting: any, secondTableParams: any) { 
    
    //console.log("secondTableParams['col']",secondTableParams['col']);
    const ParamsData: any = {
        name: "Date",
        apiName: "OperationPerformancePassQtyDetail",
        FactoryType: formParams["FactoryType"],
        G_FAC: formParams["G_FAC"],
        FromDate:formParams["FromDate"],
        ToDate:formParams["ToDate"],

        row1: WORK_CTR,
        col1: secondTableParams['col']|| '',
        Eqp_Nbr: secondTableParams['EQUIP_NBR'] || '',
        Prod_Nbr: secondTableParams['PROD_NBR'] || '',
        Shift: secondTableParams['SHIFT_ID'] || '',
        Ver: secondTableParams['VER'] || '',
        In_Qty:secondTableParams['PASS_QTY'] || '',
        Good_Qty:secondTableParams['GOOD_PASS'] || '',

        sql_TransType:APIMessage['sql_TransType'],
        sql_Shift:APIMessage["sql_Shift"],
        sql_IncludeBL:APIMessage["sql_IncludeBL"],
        sql_Floor:APIMessage["sql_Floor"],
        sql_WorkOrder:APIMessage["sql_WorkOrder"],
        sql_Product:APIMessage["sql_Product"],
        sql_LcmOwner:APIMessage["sql_LcmOwner"],
        sql_LcdOwner:APIMessage["sql_LcdOwner"],  
    };
    
    try{
        const { Date: { Data: { Table: Data },SQL01 } } = await GetCommon(ParamsData);
        //console.log("OperationPerformancePassQtyDetail-SQLSecondDetail:",SQL01)
        PageSetting.BottomSQLHidden="OperationPerformancePassQtyDetail-SQLSecondDetail:"+SQL01;

        PageSetting['BottomTitle'] = `${formParams["FactoryType"]} Operation Performance Detail`
        if (Data.length>0){
            let ColumsData: any = [];
            let WidthPars=50;
            for (let item in Data[0]) {  
                if(item=="TRANS_DATE"){
                    WidthPars=90 
                }else if(item=="PNL_ID"||item=="PROD_NBR"||item=="WO_NBR"||item=="COMMENTS"){
                    WidthPars=70
                }else{
                    if(item.length<=6){
                        WidthPars=35; 
                    }else{
                        WidthPars=50; 
                    }
                    
                }
                ColumsData.push({ title: item, align: 'center', dataIndex: item,  width:WidthPars });
            }

            PageSetting['SecondColumns'] = ColumsData;            
            PageSetting['SecondList'] = Data;

       
            // let totalsum = getfinalsum(PageSetting['SecondList'], ["PASS_QTY"], secondTableParams['dataIndex'], 'PROD_NBR');
            // let { insertrowList, insertIndexList } = Inserttotalrow(PageSetting['SecondList'], "PROD_NBR", ["PASS_QTY"], secondTableParams['dataIndex'], "PROD_NBR");
            // // 添加合计行
            // let temp = cloneDeep(PageSetting['SecondList']);
            // for (let i = 0; i < insertIndexList.length; i++) {
            //     temp.splice(insertIndexList[i] + i, 0, insertrowList[i]);
            // }
            // PageSetting['SecondList'] = temp;
            // // 添加總計     
            // PageSetting['SecondList'].push(totalsum);
           
            PageSetting.x = null;
            if (PageSetting.SecondColumns.length > 7) {
                PageSetting.x = 120 * PageSetting.SecondColumns.length;
            }

        }else{
            message.warning("無數據！")
        }
   
        closeFullLoading()
    }catch(error: any){ 
        closeFullLoading();
        return Promise.reject(error); 
     }
}

////點擊to Rwk  上表格 跳二階頁面 （與work_ctr>=2000 的 下表格 ToBeScrap 跳二階頁面公用API）
async function getToReworkFirstData(CilckKey: string, WORK_CTR: number, PageSetting: any) {
    const ParamsData: any = {
        name: "Date",
        apiName: "ToRwkQty",
        FactoryType: formParams["FactoryType"],
        G_FAC: formParams["G_FAC"],
        FromDate:formParams["FromDate"],
        ToDate:formParams["ToDate"],
        sql_Shift:APIMessage["sql_Shift"],
        sql_IncludeBL:APIMessage["sql_IncludeBL"],
        sql_Floor:APIMessage["sql_Floor"],
        sql_WorkOrder:APIMessage["sql_WorkOrder"],
        sql_Product:APIMessage["sql_Product"],
        sql_LcmOwner:APIMessage["sql_LcmOwner"],
        sql_LcdOwner:APIMessage["sql_LcdOwner"],        
        row1: WORK_CTR
    };

    try{
        const { Date: { Data: { Table: Data },SQL01 } } = await GetCommon(ParamsData);
        //console.log("ToRwkQty-SQLFirstDetail:",SQL01)

        PageSetting.SQLHidden="ToRwkQty-SQLFirstDetail:"+SQL01;


        if (CilckKey == "TOTALRWK_QTY") {        
            PageSetting['TopTitle'] = `${formParams["FactoryType"]} OPeration Performance Rework Q'ty`;       

        } else if (CilckKey == "TOBESCRAP_QTY") {
            PageSetting['TopTitle'] = `${formParams["FactoryType"]} OPeration Performance Scrap Q'ty`; 
        }

        if (Data.length != 0) {
            PageSetting['List'] = Data;
            //console.log("输出PageSetting", PageSetting);

            // 要計算的列
            let calcuList = ['COUNT'];
            PageSetting['Columns'] = OperationColumsData.ToReworkColumns;
            
            // 添加總計     
            PageSetting['List'].push(getfinalsum(PageSetting['List'], calcuList, CilckKey, 'CODE'));
        }else{
            message.warning("無數據！")
        }
        closeFullLoading();
    }catch(error: any){ 
        closeFullLoading();
        return Promise.reject(error); 
     }
}
////點擊to Rwk上表格二階頁面 跳三階頁面 （與work_ctr>=2000 的 下表格 ToBeScrap 跳二階頁面公用API）/////////
async function getToReworkQtyDetail(CilckKey: string, WORK_CTR: number, PageSetting: any,secondTableParams: any) {

    const ParamsData: any = {
        name: "Date",
        apiName: "ToRwkQtyDetail",
        FactoryType: formParams["FactoryType"],
        G_FAC: formParams["G_FAC"],
        FromDate:formParams["FromDate"],
        ToDate:formParams["ToDate"],
        sql_Shift:APIMessage["sql_Shift"],
        sql_IncludeBL:APIMessage["sql_IncludeBL"],
        sql_Floor:APIMessage["sql_Floor"],
        sql_WorkOrder:APIMessage["sql_WorkOrder"],
        sql_Product:APIMessage["sql_Product"],
        sql_LcmOwner:APIMessage["sql_LcmOwner"],
        sql_LcdOwner:APIMessage["sql_LcdOwner"], 
        row1: WORK_CTR,
        code: secondTableParams['CODE']=="總計" ? "合計": secondTableParams['CODE']|| '',
    };
    
    try{
        const { Date: { Data: { Table: Data },SQL01 } } = await GetCommon(ParamsData);
        //console.log("ToRwkQtyDetail-SQLSecondDetail:",SQL01) 

        PageSetting.BottomSQLHidden="ToRwkQtyDetail-SQLSecondDetail:"+SQL01;

        if (Data.length>0){
            if (WORK_CTR >= 1000 && WORK_CTR < 2000) {

                if (["TOTALRWK_QTY"].includes(CilckKey)) {
                    PageSetting['SecondColumns'] = OperationColumsData.ToReworkDetailColumns;
                    PageSetting['BottomTitle'] = `${formParams["FactoryType"]} Operation Performance Detail`
                    PageSetting['SecondList'] = Data;

                    let totalsum = getfinalsum(PageSetting['SecondList'], ["PASS_QTY"], CilckKey, 'PROD_NBR');
                    let { insertrowList, insertIndexList } = Inserttotalrow(PageSetting['SecondList'], "PROD_NBR", ["PASS_QTY"], CilckKey, "PROD_NBR");
                    // 添加合计行
                    let temp = cloneDeep(PageSetting['SecondList']);
                    for (let i = 0; i < insertIndexList.length; i++) {
                        temp.splice(insertIndexList[i] + i, 0, insertrowList[i]);
                    }
                    PageSetting['SecondList'] = temp;
                    // 添加總計     
                    PageSetting['SecondList'].push(totalsum);
                
                }
                }else if (WORK_CTR >= 2000 && WORK_CTR < 3600) {

                    if (["TOBESCRAP_QTY"].includes(CilckKey)) {
                        let ColumsData: any = [];
                        let WidthPars=50;
                        for (let item in Data[0]) {
                            if(item=="TRANS_DATE"){
                                WidthPars=70 
                            }else if(item=="PNL_ID"||item=="PROD_NBR"||item=="WO_NBR"||item=="ENTITY_NO"){
                                WidthPars=70
                            }else{
                                if(item.length<=6){
                                    WidthPars=35; 
                                }else{
                                    WidthPars=50; 
                                }
                                
                            }
                            ColumsData.push({ title: item, align: 'center', dataIndex: item,width:WidthPars });
                        }
                        PageSetting['SecondColumns'] = ColumsData;
                        PageSetting['BottomTitle'] = `${formParams["FactoryType"]} OPeration Performance Scrap Q'ty`; 
                        PageSetting['SecondList'] = Data;
                    
                        let totalsum = getfinalsum(PageSetting['SecondList'], ["PASS_QTY"], CilckKey, 'PROD_NBR');
                        let { insertrowList, insertIndexList } = Inserttotalrow(PageSetting['SecondList'], "PROD_NBR", ["PASS_QTY"], CilckKey, "PROD_NBR");
                        // 添加合计行
                        let temp = cloneDeep(PageSetting['SecondList']);
                        for (let i = 0; i < insertIndexList.length; i++) {
                            temp.splice(insertIndexList[i] + i, 0, insertrowList[i]);
                        }
                        PageSetting['SecondList'] = temp;
                        // 添加總計     
                        PageSetting['SecondList'].push(totalsum);                        
                    }                   
            }
            PageSetting.x = null;
            if (PageSetting.SecondColumns.length > 7) {
                PageSetting.x = 120 * PageSetting.SecondColumns.length;
            }

        }else{
            message.warning("無數據！")
        }
        closeFullLoading();
        }catch(error: any){ 
            closeFullLoading();
            return Promise.reject(error); 
         }
}


////點擊ToBeMoveIn  上表格 跳二階頁面
/*async function getToBeMoveInFirstQty(CilckKey: string, WORK_CTR: string, PageSetting: any) {
   
    let ParamsData: any = {
        name: "Date",
        apiName: "ToBeMoveIn",
        FactoryType: formParams["FactoryType"],
        G_FAC: formParams["G_FAC"],
        FromDate:formParams["FromDate"],
        ToDate:formParams["ToDate"],
        sql_Shift:APIMessage["sql_Shift"],
        sql_IncludeBL:APIMessage["sql_IncludeBL"],
        sql_Floor:APIMessage["sql_Floor"],
        sql_WorkOrder:APIMessage["sql_WorkOrder"],
        sql_Product:APIMessage["sql_Product"],
        sql_LcmOwner:APIMessage["sql_LcmOwner"],
        sql_LcdOwner:APIMessage["sql_LcdOwner"],        
        row1: WORK_CTR
    };

    
    try{
        const { Date: { Data: { Table: Data },SQL01 } } = await GetCommon(ParamsData);
        //console.log("ToBeMoveIn-SQLFirstDetail:",SQL01) 
        PageSetting.SQLHidden="ToBeMoveIn-SQLFirstDetail:"+SQL01;

        PageSetting['TopTitle'] = `${formParams["FactoryType"]} Wip Qty`

        if (Data.length != 0) {
            let ColumsData: any = [];
            for (let item in Data[0]) {
                ColumsData.push({ title: item, align: 'center', dataIndex: item });
            }
            
            PageSetting['Columns'] = OperationColumsData.ToBeMoveInColumns;
            PageSetting['List'] = Data;
            // if (["RWK_WIP_QTY", "WIP_QTY"].includes(CilckKey!)) {
                // 要計算的列
                let calcuList = ['COUNT'];
                // 添加總計     
                PageSetting['List'].push(getfinalsum(PageSetting['List'], calcuList, CilckKey, 'PROD_NBR'));

            // }
         }else{
          message.warning("無數據！")
        }
        closeFullLoading();
    }catch(error: any){ 
        closeFullLoading();
        return Promise.reject(error); 
     }
}
////點擊ToBeMoveIn 上表格 跳三階頁面
async function getToBeMoveInDetail(CilckKey: string, WORK_CTR: number, PageSetting: any, secondTableParams:any) {

    const ParamsData: any = {
        name: "Date",
        apiName: "ToBeMoveInDetail",
        FactoryType: formParams["FactoryType"],
        G_FAC: formParams["G_FAC"],
        FromDate:formParams["FromDate"],
        ToDate:formParams["ToDate"],
        sql_Shift:APIMessage["sql_Shift"],
        sql_IncludeBL:APIMessage["sql_IncludeBL"],
        sql_Floor:APIMessage["sql_Floor"],
        sql_WorkOrder:APIMessage["sql_WorkOrder"],
        sql_Product:APIMessage["sql_Product"],
        sql_LcmOwner:APIMessage["sql_LcmOwner"],
        sql_LcdOwner:APIMessage["sql_LcdOwner"], 
        row1: WORK_CTR,
        Prod_Nbr: secondTableParams['PROD_NBR']=="總計" ? "合計": secondTableParams['PROD_NBR']|| '',
        Ver: secondTableParams['VER'] || '',       
    };
    if (WORK_CTR >= 1000 && WORK_CTR < 2000) {
    
        try{
            const { Date: { Data: { Table: Data },SQL01 } } = await GetCommon(ParamsData);
            //console.log("ToBeMoveInDetail-SQLSecondDetail:",SQL01)  
            
            PageSetting.BottomSQLHidden="ToBeMoveInDetail-SQLSecondDetail:"+SQL01;
          
            PageSetting['SecondColumns'] = OperationColumsData.AllToBeMoveInColumns;
            PageSetting['BottomTitle'] = `${formParams["FactoryType"]} Operation Performance Detail`
            if (Data.length>0){
                 PageSetting['SecondList'] = Data;
                let totalsum = getfinalsum(PageSetting['SecondList'], ["QTY"], CilckKey, 'PROD_NBR');
                let { insertrowList, insertIndexList } = Inserttotalrow(PageSetting['SecondList'], "PROD_NBR", ["QTY"], CilckKey, "PROD_NBR");
                // 添加合计行
                let temp = cloneDeep(PageSetting['SecondList']);
                for (let i = 0; i < insertIndexList.length; i++) {
                    temp.splice(insertIndexList[i] + i, 0, insertrowList[i]);
                }
                PageSetting['SecondList'] = temp;
                // 添加總計     
                PageSetting['SecondList'].push(totalsum);
                PageSetting.x = null;
                if (PageSetting.SecondColumns.length > 7) {
                    PageSetting.x = 120 * PageSetting.SecondColumns.length;
                }

            }else{
                message.warning("無數據！")
            }
            closeFullLoading();   

           }catch(error: any){ 
            closeFullLoading();
            return Promise.reject(error); 
          }
          
    }    
} */

////點擊WIP,HOLD  上表格 跳二階頁面
async function getWipFirstQty(CilckKey: string, WORK_CTR: number, PageSetting: any, ColumnName: string) {
    if(ColumnName=="WIP Hold"){
        ColumnName="hold"
    }
    
    const ParamsData: any = {
        name: "Date",
        apiName: "WipQty",
        FactoryType: formParams["FactoryType"],
        G_FAC: formParams["G_FAC"],
        FromDate:formParams["FromDate"],
        ToDate:formParams["ToDate"],
        sql_Shift:APIMessage["sql_Shift"],
        sql_IncludeBL:APIMessage["sql_IncludeBL"],
        sql_Floor:APIMessage["sql_Floor"],
        sql_WorkOrder:APIMessage["sql_WorkOrder"],
        sql_Product:APIMessage["sql_Product"],
        sql_LcmOwner:APIMessage["sql_LcmOwner"],
        sql_LcdOwner:APIMessage["sql_LcdOwner"],        
        row1: WORK_CTR,
        col0: ColumnName,
    };
    
    try{
        const { Date: { Data: { Table: Data },SQL01 } } = await GetCommon(ParamsData);
        //console.log("WipQty-SQLFirstDetail:",SQL01) 
        PageSetting.SQLHidden="WipQty-SQLFirstDetail:"+SQL01;

        if (Data.length != 0) {
            PageSetting['Columns'] = OperationColumsData.WipColumns;
            PageSetting['List'] = Data;
            let calcuList = ['COUNT'];
            // 添加總計     
            PageSetting['List'].push(getfinalsum(PageSetting['List'], calcuList, CilckKey, 'PROD_NBR'));
        }else{
            message.warning("無數據！")
        }

        closeFullLoading();
    }catch(error: any){ 
        closeFullLoading();
        return Promise.reject(error); 
     }
}
////點擊WIP,HOLD （rwk wip, rwk hold） 上下表格 跳三階頁面
async function getWipQtyDetail(CilckKey: string, WORK_CTR: number, PageSetting: any,secondTableParams:any, ColumnName: string) {

    if(ColumnName=="WIP Hold"){
        ColumnName="hold"
    }
    const ParamsData: any = {
        name: "Date",
        apiName: "WipQtyDetail",
        FactoryType: formParams["FactoryType"],
        G_FAC: formParams["G_FAC"],
        FromDate:formParams["FromDate"],
        ToDate:formParams["ToDate"],
        sql_Shift:APIMessage["sql_Shift"],
        sql_IncludeBL:APIMessage["sql_IncludeBL"],
        sql_Floor:APIMessage["sql_Floor"],
        sql_WorkOrder:APIMessage["sql_WorkOrder"],
        sql_Product:APIMessage["sql_Product"],
        sql_LcmOwner:APIMessage["sql_LcmOwner"],
        sql_LcdOwner:APIMessage["sql_LcdOwner"],        
        row1: WORK_CTR,
        col0: ColumnName,
        Prod_Nbr: secondTableParams['PROD_NBR']=="總計" ? "合計": secondTableParams['PROD_NBR']|| '',
        Ver: secondTableParams['VER'] || '', 
      //  Cart_Flag: secondTableParams['CART_FLAG'] || '',   
    };

    try{
        const { Date: { Data: { Table: Data },SQL01 } } = await GetCommon(ParamsData);
        //console.log("WipQtyDetail-SQLSecondDetail:",SQL01) 

        PageSetting.BottomSQLHidden="WipQtyDetail-SQLSecondDetail:"+SQL01;
       
        if (Data.length != 0) {
            if (WORK_CTR >= 1000 && WORK_CTR < 2000) {
                PageSetting['SecondColumns'] = OperationColumsData.AllWipColumns;
                PageSetting['BottomTitle'] = `${formParams["FactoryType"]} Operation Performance Detail`
                PageSetting['SecondList'] = Data;

                if (Data.length>0){
                    let totalsum = getfinalsum(PageSetting['SecondList'], ["QTY"], CilckKey, 'PROD_NBR');
                    let { insertrowList, insertIndexList } = Inserttotalrow(PageSetting['SecondList'], "PROD_NBR", ["QTY"], CilckKey, "PROD_NBR");
                    // 添加合计行
                    let temp = cloneDeep(PageSetting['SecondList']);
                    for (let i = 0; i < insertIndexList.length; i++) {
                        temp.splice(insertIndexList[i] + i, 0, insertrowList[i]);
                    }
                    PageSetting['SecondList'] = temp;
                    // 添加總計     
                    PageSetting['SecondList'].push(totalsum);
                    PageSetting.x = null;
                    if (PageSetting.SecondColumns.length > 7) {
                        PageSetting.x = 100 * PageSetting.SecondColumns.length;
                    }
                }
                
            }else if (WORK_CTR >= 2000 && WORK_CTR < 3600) {

                PageSetting['Columns'] = OperationColumsData.AllWipColumns;
                PageSetting['Title'] = `${formParams["FactoryType"]} Operation Performance Detail`
                PageSetting['List'] = Data;
            
                if (Data.length>0){
                    let totalsum = getfinalsum(PageSetting['List'], ["QTY"], CilckKey, 'PROD_NBR');
                    let { insertrowList, insertIndexList } = Inserttotalrow(PageSetting['List'], "PROD_NBR", ["QTY"], CilckKey, "PROD_NBR");
                    // 添加合计行
                    let temp = cloneDeep(PageSetting['List']);
                    for (let i = 0; i < insertIndexList.length; i++) {
                        temp.splice(insertIndexList[i] + i, 0, insertrowList[i]);
                    }
                    PageSetting['List'] = temp;
                    // 添加總計     
                    PageSetting['List'].push(totalsum);
                    PageSetting.x2 = null;
                    if (PageSetting.Columns.length > 7) {
                        PageSetting.x2 = 100 * PageSetting.Columns.length;
                    }
                    PageSetting.pagination={
                        pageSizeOptions:['20','50','100','200'],
                        pageSize:20 
                    
                    }
                }
            }
        }else{
            message.warning("無數據！")
        }

        closeFullLoading();

    }catch(error: any){ 
        closeFullLoading();
        return Promise.reject(error); 
     }
}


///////////////----------------下表格----------------------------//////////////////
////點擊 rework in，rework Out，rework Out(1)以上 下表格跳二階頁面/////////
async function getRwkIn2000FirstData(CilckKey: string, WORK_CTR: number, PageSetting: any,firstTableParams: any) {
    const ParamsData: any = {
        name: "Date",
        apiName: "ReworkInOut",
        FactoryType: formParams["FactoryType"],
        G_FAC: formParams["G_FAC"],
        FromDate:formParams["FromDate"],
        ToDate:formParams["ToDate"],
        sql_Shift:APIMessage["sql_Shift"],
        sql_IncludeBL:APIMessage["sql_IncludeBL"],
        sql_Floor:APIMessage["sql_Floor"],
        sql_WorkOrder:APIMessage["sql_WorkOrder"],
        sql_Product:APIMessage["sql_Product"],
        sql_LcmOwner:APIMessage["sql_LcmOwner"],
        sql_LcdOwner:APIMessage["sql_LcdOwner"],        
        row1: WORK_CTR,
        col1: firstTableParams['col']|| '',
    };

    try{
        const { Date: { Data: { Table: Data },SQL01 } } = await GetCommon(ParamsData);
        //console.log("ReworkInOut-SQLFirstDetail:",SQL01) 

        PageSetting.SQLHidden="ReworkInOut-SQLFirstDetail:"+SQL01;

        if (Data.length != 0) {
            PageSetting['List'] = Data;
            PageSetting['Columns'] = OperationColumsData.RwkIn2000Columns;
            PageSetting['TopTitle'] = `${formParams["FactoryType"]} Rework`;
            
            // 要計算的列
            let calcuList = ['QTY'];
            // 添加總計     
            PageSetting['List'].push(getfinalsum(PageSetting['List'], calcuList, CilckKey, 'WORK_CTR'));
       }else{
        message.warning("無數據！")
       }  
        
        closeFullLoading();
    }catch(error: any){ 
        closeFullLoading();
        return Promise.reject(error); 
     }
}
////點擊 rework in，rework Out，rework Out(1)以上 下表格跳三階頁面/////////
async function getRwkIn2000DetailData(CilckKey: string, WORK_CTR: number, PageSetting: any,firstTableParams: any,secondTableParams:any) {
    const ParamsData: any = {
        name: "Date",
        apiName: "ReworkInOutDetail",
        FactoryType: formParams["FactoryType"],
        G_FAC: formParams["G_FAC"],
        FromDate:formParams["FromDate"],
        ToDate:formParams["ToDate"],
        sql_Shift:APIMessage["sql_Shift"],
        sql_IncludeBL:APIMessage["sql_IncludeBL"],
        sql_Floor:APIMessage["sql_Floor"],
        sql_WorkOrder:APIMessage["sql_WorkOrder"],
        sql_Product:APIMessage["sql_Product"],
        sql_LcmOwner:APIMessage["sql_LcmOwner"],
        sql_LcdOwner:APIMessage["sql_LcdOwner"],        
        row1: WORK_CTR,
        col1: firstTableParams['col']|| '',
        Operation:secondTableParams['WORK_CTR']|| '',
    };

    try{
        const { Date: { Data: { Table: Data },SQL01 } } = await GetCommon(ParamsData);
        //console.log("ReworkInOutDetail-SQLSecondDetail:",SQL01) 

        PageSetting.BottomSQLHidden="ReworkInOutDetail-SQLSecondDetail:"+SQL01;

        PageSetting['SecondColumns'] = OperationColumsData.Rwkin2000DetailCoulmns;
        PageSetting['BottomTitle'] = `${formParams["FactoryType"]} Rework Performance Detail`;

        if (Data.length>0){
            PageSetting['SecondList'] = Data;
            // 添加合计行
            let totalsum = getfinalsum(PageSetting['SecondList'], ["QTY"], secondTableParams['dataIndex'], 'PROD_NBR');
            let { insertrowList, insertIndexList } = Inserttotalrow(PageSetting['SecondList'], "PROD_NBR", ["QTY"], secondTableParams['dataIndex'], "PROD_NBR");
        
            let temp = cloneDeep(PageSetting['SecondList']);
            for (let i = 0; i < insertIndexList.length; i++) {
                temp.splice(insertIndexList[i] + i, 0, insertrowList[i]);
            }
            PageSetting['SecondList'] = temp;
            // 添加總計     
            PageSetting['SecondList'].push(totalsum);
            PageSetting.x = null;
            if (PageSetting.SecondColumns.length > 7) {
                PageSetting.x = 120 * PageSetting.SecondColumns.length;
            }
        }else{
            message.warning("無數據！")
        }
        closeFullLoading();
    }catch(error: any){ 
        closeFullLoading();
        return Promise.reject(error); 
     }
}




// 首页跳转到第一层页面
export function getFirstStart(tableParams: any, PageSetting: any,FormParams:any,APIMessageParams:any) {
    openFullLoading();   
    RightTopPageShow.value=true; 
    RightBottomPageShow.value=false;    
    console.log("第一层页面");
    formParams = FormParams; 
    

    firstTableParams= tableParams;
    APIMessage=JSON.parse(APIMessageParams);

    PageSetting['SecondColumns'] = []//初始值设置为空
    PageSetting['SecondList'] = []
    // secondTableParams = useOperation.SecondTableParams;
    if (tableParams['WORK_CTR'] >= 2000 && tableParams['WORK_CTR'] < 3600) {
        switch (tableParams['dataIndex']) {
            // label:rework in，rework Out，rework Out(1)以上
            case 'IN_QTY':
            case 'PASS_QTY':
            case 'PASS2_QTY':     
                getRwkIn2000FirstData(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting,tableParams);
                break;  
            case 'TOBESCRAP_QTY': 
                 getToReworkFirstData(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting);
            break;  
              //wip，hold
            case 'HODE_QTY':
            case 'WIP_QTY':
                getWipQtyDetail(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting,secondTableParams,tableParams["title"].replace('*', '')) 
                    break;             
            default:
                //console.log("tableParams['dataIndex']",tableParams['dataIndex']);
                break;

        }
    } else if (tableParams['WORK_CTR'] >= 1000 && tableParams['WORK_CTR'] < 2000) {
        switch (tableParams['dataIndex']) {
            // label为PASS_QTY
            case 'PASS_QTY':
                getPassQtyFirstData(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting);
                break;
            // label为TOBEMOVEIN
            //case 'TOBEMOVEIN_QTY':
             //   getToBeMoveInFirstQty(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting);
            //    break;
            //wip，hold
            case 'HODE_QTY':
            case 'WIP_QTY':
                getWipFirstQty(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting,tableParams["title"].replace('*', '')) 
                break;            
            // label为out to rwk
            case 'TOTALRWK_QTY':
                getToReworkFirstData(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting);
                break;           
            default:
                //console.log(tableParams['dataIndex']);
                break;
        }
        
    }
    

}
//第二层点击
export function getDetailStart(tableParams: any, PageSetting: any,FormParams: any,secondTableParams: any) {
    openFullLoading();
    RightBottomPageShow.value=true;     
    // // 获取二层点击参数
    // secondTableParams = useOperation.SecondTableParams;
    // // 获取一层点击参数
    // firstTableParams = useOperation.FirstTableParams;
    // 获取首页左侧参数
    firstTableParams=tableParams;
    formParams = FormParams;      
    console.log("二层点击的列是x", secondTableParams['dataIndex']);
   // console.log("点击的参数", secondTableParams);
    //console.log("输出一层的参数x", firstTableParams);
    //console.log(secondTableParams['dataIndex']);
    
    switch (secondTableParams['dataIndex']) {
        /////-----上表格跳转到二级页面--------/////
        case 'PASS_QTY':
        case 'GOOD_PASS':
        case 'RWK_QTY':
            getPassQtyDeatil(tableParams['WORK_CTR'], PageSetting, secondTableParams)           
            break;
        case 'COUNT': 
           if (firstTableParams['dataIndex']=="TOTALRWK_QTY"||firstTableParams['dataIndex']=="TOBESCRAP_QTY"){
             getToReworkQtyDetail(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting, secondTableParams);
           }
           //if (firstTableParams['dataIndex']=="TOBEMOVEIN_QTY"){
            //getToBeMoveInDetail(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting, secondTableParams);
         // }
          if (firstTableParams['dataIndex']=="WIP_QTY"||firstTableParams['dataIndex']=="HODE_QTY"){
            getWipQtyDetail(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting, secondTableParams,firstTableParams['title'].replace('*', ''));
          }
          break; 

        /////-----下表格跳转到二级页面--------///// 
        case 'QTY':
            getRwkIn2000DetailData(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting,tableParams,secondTableParams);
            break;
        default:
            break;
    }
}






// 第三层点击
// export function getThirddetailstart(tableParams: any, PageSetting: any) {   
//     openFullLoading();
//     // 获取二层点击参数
//     secondTableParams = useOperation.SecondTableParams;
//     // 获取一层点击参数
//     firstTableParams = useOperation.FirstTableParams;
//     // 获取首页左侧参数
//     fromParams = useOperation.FormParams;
//     //  获取第三层参数
//     thirdTableParams = useOperation.ThirdTableParams;

//     console.log("二层点击的列是", secondTableParams['dataIndex']);
//     console.log("点击的参数", secondTableParams);
//     console.log("输出一层的参数", firstTableParams);
//     console.log("输出tableParams", tableParams);
//     console.log("输出PageSetting", PageSetting);
//     console.log("输出thirdTableClickParams", thirdTableParams);
//     GetQTYThird(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting)

// }

export{RightTopPageShow,RightBottomPageShow}