import { AddCommonPageParamsWithOut } from '@/store/module/commonpage';
import { ReportCommonWithOut } from '@/store/module/ReportCommon';
import {
    GetOperactionPerforamce,

} from '@/api/pages/operationPerformance';
// import { message } from 'ant-design-vue';
import { cloneDeep } from 'lodash-es';
import { useLoading } from '@/components/feat/Loading';
// import { Object } from 'lodash';

let useOperation = AddCommonPageParamsWithOut();
let useReportCommon = ReportCommonWithOut();


// let tableParams = useOperation.FirstTableParams;
let secondTableParams = {};
let [fromParams, APIMessage, firstTableParams, thirdTableParams] = [{}, {}, {}, {}]
const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});


function getImport(site: any) {
    const OperationColumsData = require(`@/pages/ReportPortal/TN/OperationPerformance/${site}/Table`);
    return OperationColumsData
    // return { OperDetail3Columns, WipColumns,WipDetailCoulmns };
}
let OperationColumsData = getImport('MOD');


// 插入行合计,dataSource数据源，standard根据属性合计，calcuList要合计的列，是否是负数isNegative,当前点击的列CilckKey，合计属性sumprops
function Inserttotalrow(dataSource: any[], standard: string, calcuList: any[], CilckKey: string, sumprops: string, isNegative: boolean = false) {
    debugger
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
                        temp[item] = '小計';
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
                temp[item] = '小計';
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
                if (['RWKOUT_QTY'].includes(CilckKey)) 
                {
                    tempObj[el] = Number(tempObj[el]);
                    if (Number(tempObj['STIMES_TTL']) == 0) {
                        tempObj[el] = -Number(tempObj[el]);
                    }
                    tempObj[el] +=
                        Number(dataSource[i]['STIMES_TTL']) == 0 ? -Number(dataSource[i][el]) : Number(dataSource[i][el]);
                } 
                else {
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
                tempObj[el] = '總計';
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
async function getPassQtyFirstData(CilckKey: string, WORK_CTR: string, PageSetting: any) {
    debugger
    // 上表格的OUt
    const ParamsData: any = {
        name: "Date",
        apiName: "TN_OperationPerformanceDetail3",
        FactoryType: useOperation.Fab,
        G_FAC: 'A',
        StrSql_ww: APIMessage['StrSql_ww'],
        StrSql_w4: APIMessage['StrSql_w4'],
        col0: '',
        col1: '',
        row0: '',
        row1: WORK_CTR,
        row2: '',
        row3: '',
        sql_m: APIMessage['sql_m'],
        sql_m2: APIMessage['sql_m2'],
        sqlbu2: APIMessage['Sqlbu2'],
        vdate_e: '',
        vdate_s: ''
    };

    const { Date: { Data: { Table: Data } } } = await GetOperactionPerforamce(ParamsData);

    /*  if (!Success) {
        return message.error(Message);
    } */
    // let OperationColumsData = getImport('MOD');
    PageSetting['TopTitle'] = useOperation.Fab + ' Operation Performance Pass Qty'
    PageSetting['Columns'] = OperationColumsData.OperDetail3Columns;
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
    closeFullLoading();
}

async function getRwkOutQtyFirstData(CilckKey: string, WORK_CTR: string, PageSetting: any, Col0: string) {
    debugger
    // 上表格的OUt
    const ParamsData: any = {
        name: "Date",
        apiName: "TN_OperationPerformanceDetail1",
        FactoryType: useOperation.Fab,
        G_FAC: 'A',
        StrSql_ww: APIMessage['StrSql_ww'],
        StrSql_w4: APIMessage['StrSql_w4'],
        col0: Col0,
        col1: '',
        row0: '',
        row1: WORK_CTR,
        row2: '',
        row3: '',
        sql_m: APIMessage['sql_m'],
        sqlbu2: APIMessage['Sqlbu2'],
        vdate_e: '',
        vdate_s: ''
    };

    const { Date: { Data: { Table: Data } } } = await GetOperactionPerforamce(ParamsData);

    /*  if (!Success) {
        return message.error(Message);
    } */
    // let OperationColumsData = getImport('MOD');
    PageSetting['TopTitle'] = useOperation.Fab + ' Rework'
    PageSetting['Columns'] = OperationColumsData.OperDetail1Columns;
    PageSetting['List'] = Data.map((item) => {
        return item;
    });
    if (CilckKey == "RWKOUT_QTY" || CilckKey == "PASS_QTY" || CilckKey == "PASS2_QTY" || CilckKey == "RWK_IN") {
        // 要計算的列
        let calcuList = ["RWKOUT_QTY", "IN20_QTY"];

        // 添加總計     
        PageSetting['List'].push(getfinalsum(PageSetting['List'], calcuList, CilckKey, 'WORK_CTR'));
    }
    closeFullLoading();
}

async function getRwkFirstQty(CilckKey: string, WORK_CTR: string, PageSetting: any, Col0: string) {
    console.log(WORK_CTR);
    console.log(CilckKey);

    let ParamsData: any = {
        name: "Date",
        apiName: "TN_OperationPerformanceDetail4",
        FactoryType: useOperation.Fab,
        G_FAC: '',
        Sqlbu2: APIMessage['Sqlbu2'],
        StrSql_ww: APIMessage['StrSql_ww'],
        sql_m: APIMessage['sql_m'],
        owner_code: '',
        row1: WORK_CTR,
        col0: Col0
    };
    const { Date: { Data: { Table: Data } } } = await GetOperactionPerforamce(ParamsData);
    /*  if (!Success) {
         return message.error(Message);
     } */
    let ColumsData: any = [];
    for (let item in Data[0]) {
        ColumsData.push({ title: item, align: 'center', dataIndex: item });
    }

    if(Col0 == "Top_To_Rwk")
    {
        PageSetting['TopTitle'] = useOperation.Fab + ' Operation Performance Rework Qty'
    }
    else if (Col0 == "Bottom_To_Be_Scrap")
    {
        PageSetting['TopTitle'] = useOperation.Fab + ' Operation Performance Scrap Qty'
    }

    PageSetting['Columns'] = OperationColumsData.OperDetail4Columns;
    PageSetting['List'] = Data;
    if (["TOTALRWK_QTY","TOBESCRAP_QTY"].includes(CilckKey!)) {
        // 要計算的列
        let calcuList = ['QTY'];
        // 添加總計     
        PageSetting['List'].push(getfinalsum(PageSetting['List'], calcuList, CilckKey, 'CODE'));

    }
    closeFullLoading();
}

async function getWipQtyDetail(CilckKey: string, WORK_CTR: number, PageSetting: any, ColumnName: string) {

    const ParamsData: any = {
        name: "Date",
        apiName: "TN_OperationPerformanceDetail2",
        FactoryType: useOperation.Fab,
        G_FAC: 'A',
        Sqlbu1: "",
        Sqlbu2: APIMessage['Sqlbu2'],
        StrSql_ww: APIMessage['StrSql_ww'],
        StrSql_w1: APIMessage['StrSql_w1'],
        StrSql_w2: APIMessage['StrSql_w2'],
        col0: ColumnName,
        col1: "",
        row0: "",
        owner_code: "",
        row1: WORK_CTR,
        row2: "",
        row3: "",
        row5: "",
        sql_m: APIMessage['sql_m'],
        sql_m1: APIMessage['sql_m1'],
        vdate_e: fromParams['DateTo']
    };
    // if(secondTableParams)
    if (WORK_CTR >= 1000 && WORK_CTR < 2000 || WORK_CTR > 3000) {
        if (["RWKOUT_QTY"].includes(CilckKey)) {
            ParamsData["row0"] = "";
            ParamsData["col0"] = "Rework Out Detail";
            ParamsData["row2"] = secondTableParams['WORK_CTR'];
            ParamsData["row3"] = '';
            if (secondTableParams["WORK_CTR"] == "總計") {
                ParamsData["row2"] = "小計";
            }

        } 
        // else if (["WIP_QTY"].includes(CilckKey)) {
        //     ParamsData["row2"] = secondTableParams['PROD_NBR'] == "總計" ? "小計" : secondTableParams['PROD_NBR']

        // }

    } else if (WORK_CTR >= 2000 && WORK_CTR < 3000) {
        
    }

    const { Date: { Data: { Table: Data } } } = await GetOperactionPerforamce(ParamsData);
    /*  if (!Success) {
         return message.error(Message);
     } */
    if (WORK_CTR > 2000 && WORK_CTR < 3000) {
        debugger
        PageSetting['Columns'] = OperationColumsData.RwkWipDetailColumns;
        PageSetting['BottomTitle'] = useOperation.Fab + "Operation Performance Detail"
        PageSetting['List'] = Data;
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
    } else {
        if (["RWKOUT_QTY"].includes(CilckKey)) {
            debugger
            PageSetting['SecondColumns'] = OperationColumsData.OutTorwkColumns;
            PageSetting['BottomTitle'] = useOperation.Fab + "Operation Performance Detail"
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
        }


    }

    closeFullLoading();
}

async function getWipFirstQty(CilckKey: string, WORK_CTR: string, PageSetting: any, ColumnName: string = 'WIP_QTY') {
    console.log(WORK_CTR);
    console.log(CilckKey);

    let ParamsData: any = {
        name: "Date",
        apiName: "TN_OperationPerformanceDetail5",
        FactoryType: useOperation.Fab,
        G_FAC: '',
        Sqlbu2: APIMessage['Sqlbu2'],
        StrSql_ww: APIMessage['StrSql_ww'],
        sql_m1: APIMessage['sql_m1'],
        owner_code: '',
        row1: WORK_CTR,
        vdate_e: fromParams['DateTo']
    };
    const { Date: { Data: { Table: Data } } } = await GetOperactionPerforamce(ParamsData);
    /*  if (!Success) {
         return message.error(Message);
     } */
    let ColumsData: any = [];
    for (let item in Data[0]) {
        ColumsData.push({ title: item, align: 'center', dataIndex: item });
    }
    PageSetting['TopTitle'] = useOperation.Fab + 'Wip Qty'
    PageSetting['Columns'] = OperationColumsData.WipColumns;
    PageSetting['List'] = Data;
    if (["RWK_WIP_QTY", "WIP_QTY"].includes(CilckKey!)) {
        // 要計算的列
        let calcuList = ['COUNT'];
        // 添加總計     
        PageSetting['List'].push(getfinalsum(PageSetting['List'], calcuList, CilckKey, 'PROD_NBR'));

    }
    closeFullLoading();
}

async function getWipHoldFirstQty(CilckKey: string, WORK_CTR: string, PageSetting: any) {
    
    console.log(CilckKey);

    let ParamsData: any = {
        name: "Date",
        apiName: "TN_OperationPerformanceDetail9",
        FactoryType: useOperation.Fab,
        G_FAC: '',
        Sqlbu2: APIMessage['Sqlbu2'],
        StrSql_ww: APIMessage['StrSql_ww'],
        sql_m1: APIMessage['sql_m1'],
        owner_code: '',
        row1: WORK_CTR,
        col0: CilckKey
    };
    const { Date: { Data: { Table: Data } } } = await GetOperactionPerforamce(ParamsData);
    /*  if (!Success) {
         return message.error(Message);
     } */
    let ColumsData: any = [];
    for (let item in Data[0]) {
        ColumsData.push({ title: item, align: 'center', dataIndex: item });
    }
    PageSetting['TopTitle'] = useOperation.Fab + ' Wip Qty'
    PageSetting['Columns'] = OperationColumsData.OperDetail9Columns;
    PageSetting['List'] = Data;
    closeFullLoading();
}

async function getINQtyFirstData(CilckKey: string, WORK_CTR: string, PageSetting: any) {
    debugger
    // 上表格的OUt
    const ParamsData: any = {
        name: "Date",
        apiName: "TN_OperationPerformanceDetailIN1",
        FactoryType: useOperation.Fab,
        G_FAC: 'A',
        StrSql_ww: APIMessage['StrSql_ww'],
        StrSql_w4: '',
        row1: WORK_CTR,
        sql_m: APIMessage['sql_m'],
        sqlbu2: APIMessage['Sqlbu2']
    };

    const { Date: { Data: { Table: Data } } } = await GetOperactionPerforamce(ParamsData);

    /*  if (!Success) {
        return message.error(Message);
    } */
    // let OperationColumsData = getImport('MOD');
    PageSetting['TopTitle'] = useOperation.Fab + ' Operation Performance Pass Qty'
    PageSetting['Columns'] = OperationColumsData.OperDetailINColumns;
    PageSetting['List'] = Data.map((item) => {
        return item;
    });
    if (CilckKey == "IN_QTY") {
        // 要計算的列
        let calcuList = ["IN_QTY"];

        // 添加總計     
        PageSetting['List'].push(getfinalsum(PageSetting['List'], calcuList, CilckKey, 'PROD_NBR'));
    }
    closeFullLoading();
}

async function getPassQty2ndDetail(CilckKey: string, WORK_CTR: number, PageSetting: any, tableDetailParams: any) {
debugger
    const ParamsData: any = {
        name: "Date",
        apiName: "TN_OperationPerformanceDetail8",
        FactoryType: useOperation.Fab,
        G_FAC: 'A',
        Sqlbu1: "",
        Sqlbu2: APIMessage['Sqlbu2'],
        StrSql_ww: APIMessage['StrSql_ww'],
        StrSql_w1: APIMessage['StrSql_w1'],
        StrSql_w2: APIMessage['StrSql_w2'],
        col0: "",
        col1: "",
        row0: "",
        owner_code: "",
        row1: WORK_CTR,
        row2: tableDetailParams['PROD_NBR'],
        row3: tableDetailParams['EQUIP_NBR'],
        row4: tableDetailParams['SHIFT_ID'],
        sql_m: APIMessage['sql_m'],
        sql_m1: APIMessage['sql_m1'],
        sql_m2: APIMessage['sql_m2']
    };
    // if(secondTableParams)
    if ((WORK_CTR >= 1000 && WORK_CTR < 2000) || WORK_CTR > 3000) {
        if (["RWKOUT_QTY"].includes(CilckKey)) {
            ParamsData["row0"] = "";
            ParamsData["col0"] = "Rework Out Detail";
            ParamsData["row2"] = secondTableParams['WORK_CTR'];
            ParamsData["row3"] = '';
            if (secondTableParams["WORK_CTR"] == "總計") {
                ParamsData["row2"] = "小計";
            }

        } else if (["WIP_QTY"].includes(CilckKey)) {
            ParamsData["row2"] = secondTableParams['PROD_NBR'] == "總計" ? "小計" : secondTableParams['PROD_NBR']

        } 

    } else if (WORK_CTR >= 2000 && WORK_CTR < 3000) {
      
    }

    const { Date: { Data: { Table: Data } } } = await GetOperactionPerforamce(ParamsData);
    /*  if (!Success) {
         return message.error(Message);
     } */
    if (WORK_CTR > 2000 && WORK_CTR < 3000) {
      
    } else {
        if (["PASS_QTY"].includes(CilckKey)) {
            debugger
            PageSetting['SecondColumns'] = OperationColumsData.PassQty2ndColumns;
            PageSetting['BottomTitle'] = useOperation.Fab + "Operation Performance Detail "
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
    closeFullLoading();

}

async function getQty2ndDetail(CilckKey: string, WORK_CTR: number, PageSetting: any, tableDetailParams: any, Cilck2ndKey: string) {
        const ParamsData: any = {
            name: "Date",
            apiName: "TN_OperationPerformanceDetail2",
            FactoryType: useOperation.Fab,
            G_FAC: 'A',
            Sqlbu1: "",
            Sqlbu2: APIMessage['Sqlbu2'],
            StrSql_ww: APIMessage['StrSql_ww'],
            StrSql_w1: APIMessage['StrSql_w1'],
            StrSql_w2: APIMessage['StrSql_w2'],
            col0: "",
            col1: "",
            row0: "",
            owner_code: "",
            row1: WORK_CTR,
            row2: "",
            row3: "",
            row4: "",
            row5: "",
            row6: "",
            sql_m: APIMessage['sql_m'],
            sql_m1: APIMessage['sql_m1'],
            sql_m2: APIMessage['sql_m2']
        };
        // if(secondTableParams)
        if ((WORK_CTR >= 1000 && WORK_CTR < 2000) || WORK_CTR > 3000) {
            if (["TOTALRWK_QTY"].includes(CilckKey)) {
                ParamsData["col0"] = "Top To Rework 2nd Detail";
                ParamsData["row2"] = tableDetailParams['CODE'];
            } 
            else if (["WIP_QTY"].includes(CilckKey)) {
                ParamsData["col0"] = "Top WIP 2nd Detail";
                ParamsData["row3"] = tableDetailParams['PROD_NBR'];
                ParamsData["row4"] = tableDetailParams['VER'];
            } 
    
        } 
        else if (WORK_CTR >= 2000 && WORK_CTR < 3000) 
        {
            debugger
            if (["RWK_IN"].includes(CilckKey) && ["RWKOUT_QTY"].includes(Cilck2ndKey)) {
                ParamsData["row0"] = "";
                ParamsData["col0"] = "Bottom Rework IN Qty 2nd Detail";
                ParamsData["row5"] = tableDetailParams['WORK_CTR'];
            }
            else if (["RWK_IN"].includes(CilckKey) && ["IN20_QTY"].includes(Cilck2ndKey)) {
                ParamsData["row0"] = "";
                ParamsData["col0"] = "Bottom Rework IN MVIN 2nd Detail";
                ParamsData["row5"] = tableDetailParams['WORK_CTR'];
            }
            else if (["PASS_QTY"].includes(CilckKey) && ["RWKOUT_QTY"].includes(Cilck2ndKey)) {
                ParamsData["row0"] = "";
                ParamsData["col0"] = "Bottom Rework Out Qty 2nd Detail";
                ParamsData["row5"] = tableDetailParams['WORK_CTR'];
            }
            else if (["PASS_QTY"].includes(CilckKey) && ["IN20_QTY"].includes(Cilck2ndKey)) {
                ParamsData["row0"] = "";
                ParamsData["col0"] = "Bottom Rework Out MVIN 2nd Detail";
                ParamsData["row5"] = tableDetailParams['WORK_CTR'];
            }
            else if (["PASS2_QTY"].includes(CilckKey) && ["RWKOUT_QTY"].includes(Cilck2ndKey)) {
                ParamsData["row0"] = "";
                ParamsData["col0"] = "Bottom Rework Out Up1 Qty 2nd Detail";
                ParamsData["row5"] = tableDetailParams['WORK_CTR'];
            }
            else if (["PASS2_QTY"].includes(CilckKey) && ["IN20_QTY"].includes(Cilck2ndKey)) {
                ParamsData["row0"] = "";
                ParamsData["col0"] = "Bottom Rework Out Up1 MVIN 2nd Detail";
                ParamsData["row5"] = tableDetailParams['WORK_CTR'];
            }
            else if (["TOBESCRAP_QTY"].includes(CilckKey)) {
                ParamsData["row0"] = "";
                ParamsData["col0"] = "Bottom To Be Scrap 2nd Detail";
                ParamsData["row6"] = tableDetailParams['CODE'];
            }
        }
    
        const { Date: { Data: { Table: Data } } } = await GetOperactionPerforamce(ParamsData);
        /*  if (!Success) {
             return message.error(Message);
         } */
        if (WORK_CTR > 2000 && WORK_CTR < 3000) {
            if (["RWK_IN", "PASS_QTY", "PASS2_QTY"].includes(CilckKey)) {
                debugger
                PageSetting['SecondColumns'] = OperationColumsData.OutTorwkColumns;
                PageSetting['BottomTitle'] = useOperation.Fab + "Operation Performance Detail"
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
            }
            else if (["TOBESCRAP_QTY"].includes(CilckKey)) {
                debugger
                PageSetting['SecondColumns'] = OperationColumsData.Scrap2ndColumns;
                PageSetting['BottomTitle'] = useOperation.Fab + "Operation Performance Detail"
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
        } else {
            if (["TOTALRWK_QTY"].includes(CilckKey)) {
                debugger
                PageSetting['SecondColumns'] = OperationColumsData.ToRwk2ndColumns;
                PageSetting['BottomTitle'] = useOperation.Fab + "Operation Performance Detail "
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
            else if (["WIP_QTY"].includes(CilckKey)) {
                debugger
                PageSetting['SecondColumns'] = OperationColumsData.WIP2ndColumns;
                PageSetting['BottomTitle'] = useOperation.Fab + "Operation Performance Detail "
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
            }
    
    
        }
    
        closeFullLoading();
    }

async function getIN2ndDetail(CilckKey: string, WORK_CTR: number, PageSetting: any, tableDetailParams: any) {
    debugger
        const ParamsData: any = {
            name: "Date",
            apiName: "TN_OperationPerformanceDetailIN2",
            FactoryType: useOperation.Fab,
            G_FAC: 'A',
            Sqlbu1: "",
            Sqlbu2: APIMessage['Sqlbu2'],
            StrSql_ww: APIMessage['StrSql_ww'],
            StrSql_w1: APIMessage['StrSql_w1'],
            StrSql_w2: APIMessage['StrSql_w2'],
            col0: "",
            col1: "",
            owner_code: "",
            row0: WORK_CTR,
            row1: "",
            row2: "",
            row3: "",
            row4: "",
            sql_m: APIMessage['sql_m'],
            sql_m1: APIMessage['sql_m1'],
            sql_m2: APIMessage['sql_m2']
        };
        // if(secondTableParams)
        if (WORK_CTR >= 1000 && WORK_CTR < 2000 || WORK_CTR > 3000) {
            if (["IN_QTY"].includes(CilckKey)) {
                ParamsData["row1"] = tableDetailParams['PROD_NBR'];
                ParamsData["row2"] = tableDetailParams['VER'];
                ParamsData["row3"] = tableDetailParams['EQUIP_NBR'];
                ParamsData["row4"] = tableDetailParams['SHIFT_ID'];
            }  
    
        } else if (WORK_CTR >= 2000 && WORK_CTR < 3000) {
            
        }
    
        const { Date: { Data: { Table: Data } } } = await GetOperactionPerforamce(ParamsData);

        if (WORK_CTR > 2000 && WORK_CTR < 3000) {
            
        } else {
            if (["IN_QTY"].includes(CilckKey)) {
                debugger
                PageSetting['SecondColumns'] = OperationColumsData.IN2ndColumns;
                PageSetting['BottomTitle'] = useOperation.Fab + "Operation Performance Detail "
                PageSetting['SecondList'] = Data;
                let totalsum = getfinalsum(PageSetting['SecondList'], ["QTY"], CilckKey, 'PROD_NBR');
                let { insertrowList, insertIndexList } = Inserttotalrow(PageSetting['SecondList'], "PROD_NBR", ["QTY"], CilckKey, "PROD_NBR");
             
                // 添加總計     
                PageSetting['SecondList'].push(totalsum);
            }
        }
    
        closeFullLoading();
    }    

// 首页跳转到第一层页面
export function getFirstStart(tableParams: any, PageSetting: any) {
    openFullLoading(); 
    //fromParams = useOperation.FormParams;
    //APIMessage = JSON.parse(useOperation.APIMessage);
    //firstTableParams = useOperation.FirstTableParams
debugger
    let ReportNameParams = `OperationPerformanceDetail_${useOperation.Fab}`;
    fromParams = useReportCommon[ReportNameParams].FormParams;
    APIMessage = JSON.parse(useReportCommon[ReportNameParams].APIMessage);
    firstTableParams = useReportCommon[ReportNameParams].FirstTableParams;
debugger
    PageSetting['SecondColumns'] = []//初始值设置为空
    PageSetting['SecondList'] = []
    // secondTableParams = useOperation.SecondTableParams;
    debugger
    if (tableParams['WORK_CTR'] >= 2000 && tableParams['WORK_CTR'] < 3000) {
        switch (tableParams['dataIndex']) {
            // label:in
            case 'RWK_IN':
                getRwkOutQtyFirstData(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting, 'Bottom_Rwk_In');
                break;  
            // label为 rework out
            case 'PASS_QTY':
                getRwkOutQtyFirstData(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting, 'Bottom_Rwk_Out');
                break; 
            // label为 rework out
            case 'PASS2_QTY':
                getRwkOutQtyFirstData(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting, 'Bottom_Rwk_Out2');
                break; 
            // label为to be scrap
            case 'TOBESCRAP_QTY':
                getRwkFirstQty(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting, 'Bottom_To_Be_Scrap');
                break;   
            //label为wip
            case 'WIP_QTY':
                getWipQtyDetail(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting, 'Bottom_WIP');
                break;     
            // label为wip Hold
            case 'HOLD_QTY':
                getWipHoldFirstQty(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting);
                break;
            // label为wip Non-Hold
            case 'NOHOLD_QTY':
                getWipHoldFirstQty(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting);
                break;     

            default:
                console.log(tableParams['dataIndex']);
                break;

        }
    } else if ((tableParams['WORK_CTR'] >= 1000 && tableParams['WORK_CTR'] < 2000) || tableParams['WORK_CTR'] > 3000) {
        debugger
        switch (tableParams['dataIndex']) {
            // label为out
            case 'PASS_QTY':
                getPassQtyFirstData(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting);
                break;
            // label为to Rwk
            case 'TOTALRWK_QTY':
                getRwkFirstQty(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting, 'Top_To_Rwk');
                break;    
            // label为 rework out
            case 'RWKOUT_QTY':
                getRwkOutQtyFirstData(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting, 'Top_Rwk_Out');
                break;    
            // label为wip
            case 'WIP_QTY':
                getWipFirstQty(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting);
                break;
            // label为wip Hold
            case 'HOLD_QTY':
                getWipHoldFirstQty(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting);
                break;
            // label为wip Non-Hold
            case 'NOHOLD_QTY':
                getWipHoldFirstQty(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting);
                break;    
            // label为IN
            case 'IN_QTY':
                getINQtyFirstData(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting);
                break; 
            default:
                console.log(tableParams['dataIndex']);
                break;

        }
    }
    closeFullLoading();

}
//第二层点击
export function getDetailStart(tableParams: any, PageSetting: any, tableDetailParams: any) {
    openFullLoading(); 
    let ReportNameParams = `OperationPerformanceDetail_${useOperation.Fab}`;
    // 获取二层点击参数
    //secondTableParams = useOperation.SecondTableParams;
    secondTableParams = useReportCommon[ReportNameParams].SecondTableParams;
    // 获取一层点击参数
    //firstTableParams = useOperation.FirstTableParams;
    firstTableParams = useReportCommon[ReportNameParams].FirstTableParams;
    // 获取首页左侧参数
    //fromParams = useOperation.FormParams;
    fromParams = useReportCommon[ReportNameParams].FormParams;
    console.log("二层点击的列是", secondTableParams['dataIndex']);
    console.log("点击的参数", secondTableParams);
    console.log("输出一层的参数", firstTableParams['dataIndex']);
    console.log("输出一层的title", firstTableParams['title']);
    //console.log("record", record);
    // 上表格的out，跳转到二级页面，点击投产数
    debugger
    if ((tableParams['WORK_CTR'] >= 1000 && tableParams['WORK_CTR'] < 2000) || tableParams['WORK_CTR'] > 3000) 
    {
        debugger
        switch (tableParams['dataIndex']) {
            case 'PASS_QTY':
                //firstclick;
                getPassQty2ndDetail(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting, tableDetailParams)
                break;
            case 'TOTALRWK_QTY':
                //firstclick;
                getQty2ndDetail(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting, tableDetailParams, tableDetailParams['dataIndex'])
                break;
            case "RWKOUT_QTY":
                getWipQtyDetail(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting, '');
                break;  
            case 'WIP_QTY':
                //firstclick;
                getQty2ndDetail(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting, tableDetailParams, tableDetailParams['dataIndex'])
                break;  
            case 'IN_QTY':
                //firstclick;
                getIN2ndDetail(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting, tableDetailParams)
                break;  
            case 'TOBESCRAP_QTY':
                //firstclick;
                getQty2ndDetail(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting, tableDetailParams, tableDetailParams['dataIndex'])
                break;    

            default:
                break;   
        }
    }
    else if (tableParams['WORK_CTR'] >= 2000 && tableParams['WORK_CTR'] < 3000) 
    {
        switch (tableParams['dataIndex']) {
            case 'RWK_IN':
                //firstclick;
                getQty2ndDetail(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting, tableDetailParams, tableDetailParams['dataIndex'])
                break;        
            case 'PASS_QTY':
                //firstclick;
                getQty2ndDetail(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting, tableDetailParams, tableDetailParams['dataIndex'])
                break;      
            case 'PASS2_QTY':
                //firstclick;
                getQty2ndDetail(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting, tableDetailParams, tableDetailParams['dataIndex'])
                break; 
            case 'TOBESCRAP_QTY':
                //firstclick;
                getQty2ndDetail(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting, tableDetailParams, tableDetailParams['dataIndex'])
                break;             
                
            default:
                break;
        }
    }
    closeFullLoading();
}
// 第三层点击
export function getThirddetailstart(tableParams: any, PageSetting: any) {
    console.log("执行了");
    openFullLoading(); 
    let ReportNameParams = `OperationPerformanceDetail_${useOperation.Fab}`;
    // 获取二层点击参数
    //secondTableParams = useOperation.SecondTableParams;
    secondTableParams = useReportCommon[ReportNameParams].SecondTableParams;
    // 获取一层点击参数
    //firstTableParams = useOperation.FirstTableParams;
    firstTableParams = useReportCommon[ReportNameParams].FirstTableParams;
    // 获取首页左侧参数
    //fromParams = useOperation.FormParams;
    fromParams = useReportCommon[ReportNameParams].FormParams;
    //  获取第三层参数
    //thirdTableParams = useOperation.ThirdTableParams;
    thirdTableParams = useReportCommon[ReportNameParams].ThirdTableParams;

    console.log("二层点击的列是", secondTableParams['dataIndex']);
    console.log("点击的参数", secondTableParams);
    console.log("输出一层的参数", firstTableParams);
    console.log("输出tableParams", tableParams);
    console.log("输出PageSetting", PageSetting);
    console.log("输出thirdTableClickParams", thirdTableParams);
    //GetQTYThird(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting)




}