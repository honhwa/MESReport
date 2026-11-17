import { AddOperationParamsWithOut } from '@/store/module/operationDeatil';
import { GetCommon } from '@/api/pages/common';
import { cloneDeep } from 'lodash-es';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { ref } from 'vue';
import { message } from 'ant-design-vue';
let useOperation = AddOperationParamsWithOut();
let secondTableParams = {};
let [fromParams, APIMessage, firstTableParams, thirdTableParams] = [{}, {}, {}, {}]
const UseParamsAdd = AddCommonPageParams()
let FactoryType: any = UseParamsAdd.Fab;
let importurl = ref("LCM");
switch (FactoryType) {
    case "LCM1":
    case "LCM2":
    case "SET1":
    case "SET2":
        importurl.value = "LCM"
        break;
    case "BLU1":
    case "BLU2":
        importurl.value = "BLU"
        break;
    default:
        break;
}



function getImport(site: any) {
    const OperationColumsData = require(`@/pages/ReportPortal/FS/OperationPerformance/${site}/Table`);
    return OperationColumsData
}
let OperationColumsData = getImport(importurl.value);
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
    // 上表格的OUt
    const ParamsData: any = {
        name: "Date",
        apiName: "OperationPerformancePassQty",
        FactoryType: FactoryType,
        G_FAC: 'A',
        StrSql_w: APIMessage['StrSql_w'],
        StrSql_w4: APIMessage['StrSql_w4'],
        col0: '',
        col1: '',
        row0: '',
        row1: WORK_CTR,
        row2: '',
        row3: '',
        sql_m: APIMessage['sql_m'],
        sqlbu2: '',
        vdate_e: '',
        vdate_s: ''
    };

    const { Date: { Data: { Table: Data } } } = await GetCommon(ParamsData);
    if (Data.length != 0) {
        PageSetting['TopTitle'] = 'LCMA Operation Performance Pass Qty'
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
    } else {
        message.warn("无数据！")
    }
}
async function getWipFirstQty(CilckKey: string, WORK_CTR: string, PageSetting: any, ColumnName: string = 'WIP_QTY') {
    let ParamsData: any = {
        name: "Date",
        apiName: "WipQty",
        FactoryType: FactoryType,
        G_FAC: 'A',
        Sqlbu1: '',
        StrSql_w: APIMessage['StrSql_w'],
        StrSql_w1: APIMessage['StrSql_w1'],
        StrSql_w2: APIMessage['StrSql_w2'],
        col0: ColumnName.replace('*', ''),
        owner_code: '',
        row1: WORK_CTR,
        vdate_e: fromParams['DateTo']
    };
    const { Date: { Data: { Table: Data } } } = await GetCommon(ParamsData);
    if (Data.length != 0) {
        let ColumsData: any = [];
        for (let item in Data[0]) {
            ColumsData.push({ title: item, align: 'center', dataIndex: item });
        }
        PageSetting['TopTitle'] = 'LCMA Wip Qty'
        PageSetting['Columns'] = OperationColumsData.WipColumns;
        if (FactoryType == "LCM2") {
            PageSetting['Columns'] = OperationColumsData.LCM2WipColumns;
        }
        PageSetting['List'] = Data;
        if (["RWK_WIP_QTY", "WIP_QTY"].includes(CilckKey!)) {
            // 要計算的列
            let calcuList = ['COUNT'];
            // 添加總計     
            PageSetting['List'].push(getfinalsum(PageSetting['List'], calcuList, CilckKey, 'PROD_NBR'));

        }
    } else {
        message.warn("无数据！")
    }


}
async function getPassQtyDeatil(WORK_CTR: string, PageSetting: any, secondTableParams: any) {


    const ParamsData: any = {
        name: "Date",
        apiName: "OperationPerformanceDetail9",
        FactoryType: FactoryType,
        G_FAC: 'A',
        Sqlbu2: '',
        StrSql_w4: '',
        equip_no: secondTableParams['EQUIP_NBR'] || '',
        prod_no: secondTableParams['PROD_NBR'] || '',
        row1: WORK_CTR,
        shift_no: secondTableParams['SHIFT_ID'] || '',
        sql_m: APIMessage['sql_m'],
        sql_m1: '',
        vdate_e: fromParams['DateTo']
    };

    const { Date: { Data: { Table: Data } } } = await GetCommon(ParamsData);
    if (Data.length != 0) {
        let ColumsData: any = [];
        for (let item in Data[0]) {
            ColumsData.push({ title: item, align: 'center', dataIndex: item });
        }
        PageSetting['SecondColumns'] = ColumsData;
        PageSetting['BottomTitle'] = 'LCMA Operation Performance Detail'
        PageSetting['SecondList'] = Data;
        let totalsum = getfinalsum(PageSetting['SecondList'], ["PASS_QTY"], secondTableParams['dataIndex'], 'PROD_NBR');
        let { insertrowList, insertIndexList } = Inserttotalrow(PageSetting['SecondList'], "PROD_NBR", ["PASS_QTY"], secondTableParams['dataIndex'], "PROD_NBR");
        // 添加合计行
        let temp = cloneDeep(PageSetting['SecondList']);
        for (let i = 0; i < insertIndexList.length; i++) {
            temp.splice(insertIndexList[i] + i, 0, insertrowList[i]);
        }
        PageSetting['SecondList'] = temp;
        // 添加總計     
        PageSetting['SecondList'].push(totalsum);
    } else {
        message.warn("无数据！")
    }

}
async function getWipQtyDetail(CilckKey: string, WORK_CTR: number, PageSetting: any, ColumnName: string) {
    const ParamsData: any = {
        name: "Date",
        apiName: "OperationPerformanceDetail",
        FactoryType: FactoryType,
        G_FAC: 'A',
        Sqlbu1: "",
        Sqlbu2: '',
        StrSql_w: APIMessage['StrSql_w'],
        StrSql_w1: APIMessage['StrSql_w1'],
        StrSql_w2: APIMessage['StrSql_w2'],
        col0: ColumnName,
        col1: "",
        row0: "",
        owner_code: "",
        row1: WORK_CTR,
        row2: secondTableParams['PROD_NBR'] || '',
        row3: secondTableParams['VER'] || '',
        row4: secondTableParams["FLAG"] || "",
        sql_m: APIMessage['sql_m'],
        sql_m1: APIMessage['sql_m1'],
        vdate_e: fromParams['DateTo']
    };
    if (WORK_CTR >= 1000 && WORK_CTR < 2000 || WORK_CTR >= 8000 && WORK_CTR < 9000) {
        if (["TOTALRWK_QTY"].includes(CilckKey)) {
            ParamsData["row0"] = "";
            ParamsData["col0"] = "Defect Code";
            ParamsData["row2"] = secondTableParams['CODE'];
            ParamsData["row3"] = '';
            if (secondTableParams["CODE"] == "總計") {
                ParamsData["row2"] = "合計";
            }
            if(FactoryType.includes("SET")){
                ParamsData["col0"] = "Scrap Code"
            }
        } else if (["WIP_QTY"].includes(CilckKey)) {
            ParamsData["row2"] = secondTableParams['PROD_NBR'] == "總計" ? "合計" : secondTableParams['PROD_NBR']
        }
    } else if (WORK_CTR >= 2000 && WORK_CTR < 3000 || WORK_CTR >= 9000 && WORK_CTR < 10000) {
        ParamsData["col1"] = "";
        if (["IN_QTY"].includes(CilckKey)) {
            console.log("此時");
            // 原來
           /*  ParamsData["col1"] = WORK_CTR;
            ParamsData["row0"] = secondTableParams["WORK_CTR"] */
            ParamsData["col1"] = WORK_CTR;
            ParamsData["row0"] = WORK_CTR;
            ParamsData["row1"] = secondTableParams["WORK_CTR"];

            ParamsData["row2"] = "";
            ParamsData["row3"] = '';
           if(FactoryType.includes("SET")){
            if (secondTableParams["WORK_CTR"] == "總計") {
                ParamsData["row0"] = "合計";
            }
           }
        } else if (["TOBESCRAP_QTY"].includes(CilckKey)) {
            ParamsData["col0"] = "Scrap Code"
            ParamsData["row2"] = secondTableParams["CODE"];
            ParamsData["row3"] = '';
            if (secondTableParams["CODE"] == "總計") {
                ParamsData["row2"] = "合計";
            }
        } else if (["PASS2_QTY"].includes(CilckKey)) {
            ParamsData["row0"] = secondTableParams["WORK_CTR"]
            if (secondTableParams["WORK_CTR"] == "總計") {
                ParamsData["row0"] = WORK_CTR;
            }
        }
    }
    const { Date: { Data: { Table: Data } } } = await GetCommon(ParamsData);
    if (Data.length != 0) {
        if ((WORK_CTR > 2000 && FactoryType.includes("LCM")) || (WORK_CTR > 9000 && FactoryType.includes("SET"))) {
            if (["WIP_QTY", "NHOLD_WIP_QTY", "HOLD_WIP_QTY", "S1_WIP_QTY", "S2_WIP_QTY", "S3_WIP_QTY"].includes(CilckKey)) {
                PageSetting['Columns'] = OperationColumsData.WipDetailCoulmns;
                PageSetting['TopTitle'] = 'LCMA WIP  Detail(>2000)';
                PageSetting['List'] = Data;
                let isNegative = false;
                if (["NHOLD_WIP_QTY", "HOLD_WIP_QTY"].includes(CilckKey)) {
                    isNegative = true;
                }
                let totalsum = getfinalsum(PageSetting['List'], ["QTY"], CilckKey, 'PROD_NBR', isNegative);
                let { insertrowList, insertIndexList } = Inserttotalrow(PageSetting['List'], "PROD_NBR", ["QTY"], CilckKey, "PROD_NBR", isNegative);
                // 添加合计行
                let temp = cloneDeep(PageSetting['List']);
                for (let i = 0; i < insertIndexList.length; i++) {
                    temp.splice(insertIndexList[i] + i, 0, insertrowList[i]);
                }
                PageSetting['List'] = temp;
                // 添加總計     
                PageSetting['List'].push(totalsum);
                // 下面都是二级跳转
            } else if (["IN_QTY", "PASS2_QTY"].includes(CilckKey)) {
                PageSetting['SecondColumns'] = OperationColumsData.InqtyColumns;
                if (["PASS2_QTY"].includes(CilckKey)) {
                    PageSetting['SecondColumns'] = OperationColumsData.Inpass2secondColumns;
                }
                PageSetting['BottomTitle'] = "LCMA Operation Performance Pass Q'ty";
                PageSetting['SecondList'] = Data;

                let totalsum = getfinalsum(PageSetting['SecondList'], ["QTY"], CilckKey, 'OPER');
                let { insertrowList, insertIndexList } = Inserttotalrow(PageSetting['SecondList'], "PROD_NBR", ["QTY"], CilckKey, "OPER");
                // 添加合计行
                let temp = cloneDeep(PageSetting['SecondList']);
                for (let i = 0; i < insertIndexList.length; i++) {
                    temp.splice(insertIndexList[i] + i, 0, insertrowList[i]);
                }
                PageSetting['SecondList'] = temp;
                // 添加總計     
                PageSetting['SecondList'].push(totalsum);
                // 将数据存储到vuex
                if (CilckKey == "IN_QTY") {
                    useOperation.AddThirdTableClickParams(PageSetting['SecondList']);

                }
            } else if (["TOBESCRAP_QTY"].includes(CilckKey)) {
                PageSetting['SecondColumns'] = OperationColumsData.TobethildColumns;
                PageSetting['BottomTitle'] = "LCMA Operation Performance Pass Q'ty";
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
            if (["WIP_QTY"].includes(CilckKey)) {
                PageSetting['SecondColumns'] = OperationColumsData.WipDetailCoulmns;
                PageSetting['BottomTitle'] = "LCMA Operation Performance Detail";
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
            } else if (["TOTALRWK_QTY"].includes(CilckKey)) {
                PageSetting['SecondColumns'] = OperationColumsData.OutTorwkColumns;
                PageSetting['BottomTitle'] = "LCMA Operation Performance Detail"
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
            } else if (["RWK_WIP_QTY"].includes(CilckKey)) {
                PageSetting['SecondColumns'] = OperationColumsData.WipDetailCoulmns;
                PageSetting['BottomTitle'] = "LCMA Operation Performance Detail";
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
    } else {
        message.warn("无数据！")
    }
}
async function getWipQty10Data(CilckKey: string, WORK_CTR: number, PageSetting: any) {
    const ParamsData: any = {
        name: "Date",
        apiName: "WipQty10",
        FactoryType: FactoryType,
        G_FAC: "A",
        Sqlbu1: "",
        StrSql_w: "",
        StrSql_w1: "",
        StrSql_w2: APIMessage['StrSql_w2'],
        col0: "",
        owner_code: "",
        row1: WORK_CTR,
        vdate_e: fromParams['DateTo'],
    };

    const { Date: { Data: { Table: Data } } } = await GetCommon(ParamsData);
    PageSetting['TopTitle'] = 'LCMA Wip10 Qty'
    PageSetting['Columns'] = OperationColumsData.WipColumns;
    PageSetting['List'] = Data;
    if (Data.length != 0) {
        if (CilckKey == "RWK_OUTQTY") {
            // 要計算的列
            let calcuList = ['COUNT'];

            // 添加總計     
            PageSetting['List'].push(getfinalsum(PageSetting['List'], calcuList, CilckKey, 'PROD_NBR'));
        }
    } else {
        message.warn("无数据！")
    }

}
async function getReworkQtyData(CilckKey: string, WORK_CTR: number, PageSetting: any) {
    const ParamsData: any = {
        name: "Date",
        apiName: "OperationPerformanceReworkQty",
        FactoryType: FactoryType,
        G_FAC: "A",
        Sqlbu2: " ",
        StrSql_w: APIMessage['StrSql_w'],
        row1: WORK_CTR,
        sql_m: APIMessage['sql_m']
    };
    const { Date: { Data: { Table: Data } } } = await GetCommon(ParamsData);
    PageSetting['List'] = Data;
    if (Data.length != 0) {
        // 要計算的列
        let calcuList = ['COUNT'];
        if (CilckKey == "TOTALRWK_QTY") {
            PageSetting['Columns'] = OperationColumsData.ReworkColumns;
            PageSetting['TopTitle'] = "LCMA OPeration Performance Rework Q'ty";
            // 添加總計     
            PageSetting['List'].push(getfinalsum(PageSetting['List'], calcuList, CilckKey, 'CODE'));

        } else if (CilckKey == "TOBESCRAP_QTY") {
            PageSetting['Columns'] = OperationColumsData.ScrapColumns
            PageSetting['TopTitle'] = "LCMA OPeration Performance Scrap Q'ty";
            PageSetting['List'].push(getfinalsum(PageSetting['List'], calcuList, CilckKey, 'CODE'));
        }
    } else {
        message.warn("无数据！");
    }

}
async function getPassQtyTOMData(CilckKey: string, WORK_CTR: number, PageSetting: any, ColumnName: string = '后→前') {//API統一格式與參數后可將一階二階各整合成一個方法
    const ParamsData: any = {
        name: "Date",
        apiName: "OperationPerformancePassQtyZ",
        FactoryType: FactoryType,
        G_FAC: "A",
        Sqlbu2: "",
        StrSql_ww: APIMessage['StrSql_ww'],
        TO_ERP_OPER: "TO_ERP_OPER",
        // TO_ERP_OPER: "",
        col0: ColumnName,
        floor: fromParams['Floor'],
        row0: WORK_CTR,
        shift: fromParams['Shift'],
        sql_m1: APIMessage['sql_m1'],
        sql_p: "",
        vDate_e1: "",
        vDate_s: ""
    };
    const { Date: { Data: { Table: Data } } } = await GetCommon(ParamsData);
    /*  if (!Success) {
         return message.error(Message);
     } */
    if (Data.length != 0) {
        PageSetting['TopTitle'] = 'LCMA Rework Qty'
        if (ColumnName == '后→前') {
            PageSetting['Columns'] = OperationColumsData.TOMColumns;

        } else {
            // PageSetting['Columns'] = OperationColumsData.MTOColumns(Data);
            PageSetting['Columns'] = OperationColumsData.MTOColumns;

        }
        PageSetting['List'] = Data
        if (["MTO", "TOM".includes(CilckKey)]) {
            // 要計算的列
            let calcuList = ['TOM'];
            // 添加總計     
            PageSetting['List'].push(getfinalsum(PageSetting['List'], calcuList, CilckKey, 'PROD_NBR'));

        }
    } else {
        message.warn("无数据！");
    }

}
async function getPassQtyTOMDetailData(firstTableParams: Object, WORK_CTR: string, PageSetting: any) {
    // GetPassQtyTOMDetail
    const ParamsData: any = {
        name: "Date",
        apiName: "OperationPerformancePassQtyZ1",
        FactoryType: FactoryType,
        prod_nbr: secondTableParams['PROD_NBR'],
        from_oper: secondTableParams['FR_ERP_OPER'],
        // to_oper: '',
        to_oper:WORK_CTR,
        condition: secondTableParams["title"].replace("*", ''),
        Shift: '',
        Floor: '',
        StrSql_ww: '',
        Sqlbu2: '',
        sql_m1: APIMessage['sql_m1'],
        sql_m2: APIMessage['sql_m1'],
        sql_p: APIMessage['StrSql_ww'],
        G_FAC: 'A'
    };
    if (secondTableParams['PROD_NBR'] == "總計") {
        ParamsData["prod_nbr"] = "總計";
        ParamsData["to_oper"] = WORK_CTR;
        ParamsData["from_oper"] = "";

    }

    const { Date: { Data: { Table: Data } } } = await GetCommon(ParamsData);
    if (Data.length != 0) {
        PageSetting['SecondColumns'] = OperationColumsData.MTOTom;
        if(FactoryType.includes("SET")){
            PageSetting['SecondColumns'] = OperationColumsData.SetMTOTom;
        }
        PageSetting['BottomTitle'] = "LCMA Operation Performance Pass Q'ty";
        PageSetting['SecondList'] = Data;
    } else {
        message.warn("无数据！");
    }

}
async function GetReworkD2000Data(CilckKey: string, WORK_CTR: string, PageSetting: any, ColumnName: string = 'In') {
    const ParamsData: any = {
        name: "Date",
        apiName: "Rework",
        FactoryType: FactoryType,
        G_FAC: "A",
        Sqlbu1: "",
        Sqlbu2: "",
        StrSql_w: "",
        StrSql_w1: APIMessage['StrSql_w1'],
        StrSql_w2: "",
        col0: ColumnName,
        col1: "",
        owner_code: "",
        row0: WORK_CTR,
        row1: WORK_CTR,
        sql_m: APIMessage['sql_m'],
        sql_m1: APIMessage['sql_m1'],
        vdate_e: ""
    };
    const { Date: { Data: { Table: Data } } } = await GetCommon(ParamsData);
    if (Data.length != 0) {
        PageSetting['TopTitle'] = 'LCMA Rework'
        if (["IN_QTY", "PASS2_QTY"].includes(CilckKey)) {
            PageSetting['Columns'] = OperationColumsData.Inpass2Columns;
            PageSetting['List'] = Data;
            // 要計算的列
            let calcuList = ['QTY'];
            // 添加總計     
            PageSetting['List'].push(getfinalsum(PageSetting['List'], calcuList, CilckKey, 'WORK_CTR'));

        }
    } else {
        message.warn("无数据！");
    }

}

async function GetQTYThird(CilckKey: string, WORK_CTR: number, PageSetting: any) {
    const ParamsData: any = {
        name: "Date",
        apiName: "PanelWipStayChart",
        FactoryType: FactoryType,
        "acct_date": fromParams['DateTo'],
        "stimes_ttl": "",
        "G_FAC": "A",
        "col0": "",
        "col1": "",
        "to_wc": useOperation.ThirdTableParams["TO_WC"],
        "work_ctr": useOperation.ThirdTableParams["WORK_CTR"],
        "prod_nbr": useOperation.ThirdTableParams["PROD_NBR"],
        "Operator": "",
        "equip_nbr": "",
        "trans_date": ""

    };
    const { Date: { Data: { Table: Data } } } = await GetCommon(ParamsData);
    if (Data.length != 0) {
        if (["IN_QTY"].includes(CilckKey)) {
            PageSetting['ThirdTitle'] = "LCMA Panel Wip Stay Chart"
            PageSetting['ThirdColumns'] = OperationColumsData.ThirdColumns;
            PageSetting['ThirdList'] = Data;
        }
    } else {
        message.warn("无数据！")
    }

}
// 首页跳转到第一层页面
export function getFirstStart(tableParams: any, PageSetting: any) {
    fromParams = useOperation.FormParams;
    APIMessage = useOperation.APIMessage;
    firstTableParams = useOperation.FirstTableParams
    PageSetting['SecondColumns'] = []//初始值设置为空
    PageSetting['SecondList'] = []
    // secondTableParams = useOperation.SecondTableParams;
    if (tableParams['WORK_CTR'] >= 2000 && tableParams['WORK_CTR'] < 3000 || tableParams["WORK_CTR"] >= 9000 && tableParams["WORK_CTR"] < 10000) {
        switch (tableParams['dataIndex']) {
            // label:in
            case 'IN_QTY':
                GetReworkD2000Data(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting, 'In');
                break;
            //wip
            case 'WIP_QTY':
                getWipQtyDetail(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting, 'WIP');
                break;
            // hold
            case 'HOLD_WIP_QTY':
                getWipQtyDetail(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting, 'Hold');
                break;
            // non hold
            case 'NHOLD_WIP_QTY':
                getWipQtyDetail(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting, 'Non Hold');
                break;
            // to be scrap
            case 'TOBESCRAP_QTY':
                getReworkQtyData(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting);
                break;
            // out rwk 以上
            case 'PASS2_QTY':
                GetReworkD2000Data(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting, 'Out rwk(1)以上');
                break;
            // wip pcbi aken, cd ken
            case "S1_WIP_QTY":
            case "S2_WIP_QTY":
            case "S3_WIP_QTY":
                // getAllwip(tableParams['dataIndex'], tableParams["title"], tableParams['WORK_CTR'], PageSetting)
                getWipQtyDetail(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting, tableParams["title"].replace('*', ''));
                break;
            default:
                break;

        }
    } else if (tableParams['WORK_CTR'] >= 1000 && tableParams['WORK_CTR'] < 2000 || tableParams["WORK_CTR"] >= 8000 && tableParams["WORK_CTR"] < 9000) {
        switch (tableParams['dataIndex']) {
            // label为out
            case 'PASS_QTY':
                getPassQtyFirstData(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting);
                break;
            // label为wip
            case 'WIP_QTY':
                getWipFirstQty(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting);
                break;
            // label为in from rwk
            case 'RWK_OUTQTY':
                getWipQty10Data(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting);
                break;
            // label为out to rwk
            case 'TOTALRWK_QTY':
                getReworkQtyData(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting);
                break;
            // label为wip(R)
            case 'RWK_WIP_QTY':
                getWipFirstQty(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting, firstTableParams['title']);
                break;
            // label为后→前
            case 'TOM':
                getPassQtyTOMData(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting, '后→前');
                break;
            // label为前→后
            case 'MTO':
                getPassQtyTOMData(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting, '前→后');
                break;
            default:
                break;

        }
    }

}
//第二层点击
export function getDetailStart(tableParams: any, PageSetting: any) {
    // 获取二层点击参数
    secondTableParams = useOperation.SecondTableParams;
    // 获取一层点击参数
    firstTableParams = useOperation.FirstTableParams;
    // 获取首页左侧参数
    fromParams = useOperation.FormParams;
    // 上表格的out，跳转到二级页面，点击投产数
    switch (firstTableParams['dataIndex']) {
        case 'PASS_QTY':
            getPassQtyDeatil(tableParams['WORK_CTR'], PageSetting, secondTableParams)
            break;
        case 'WIP_QTY':
        case "TOTALRWK_QTY":
            getWipQtyDetail(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting, firstTableParams['title'].replace('*', ''));
            break;
        case 'RWK_WIP_QTY':
            getWipQtyDetail(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting, firstTableParams['title'].replace('*', ''));

            break;
        case 'MTO':
        case 'TOM':
            getPassQtyTOMDetailData(firstTableParams, tableParams['WORK_CTR'], PageSetting)
            break;
        // 下表格in_qty
        case "IN_QTY":
        case "PASS2_QTY":
            getWipQtyDetail(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting, firstTableParams['title'].replace('*', ''));
            break;
        case "TOBESCRAP_QTY":
            getWipQtyDetail(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting, firstTableParams['title'].replace('*', ''));

            break;
        default:
            break;

    }

}
// 第三层点击
export function getThirddetailstart(tableParams: any, PageSetting: any) {
    // 获取二层点击参数
    secondTableParams = useOperation.SecondTableParams;
    // 获取一层点击参数
    firstTableParams = useOperation.FirstTableParams;
    // 获取首页左侧参数
    fromParams = useOperation.FormParams;
    //  获取第三层参数
    thirdTableParams = useOperation.ThirdTableParams;
    GetQTYThird(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting)
}

