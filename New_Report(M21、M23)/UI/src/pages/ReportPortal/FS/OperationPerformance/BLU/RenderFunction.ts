import { AddOperationParamsWithOut } from '@/store/module/operationDeatil';
import { GetCommon } from '@/api/pages/common';
import { cloneDeep } from 'lodash-es';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { ref } from 'vue';
import { getfinalsum, Inserttotalrow, TableCustomCell0 } from '@/pages/ReportPortal/utils/common';
import { message } from 'ant-design-vue';
/* let FactoryType: string = "";
export function initFactoryType(pFactoryType:string){
    FactoryType =  pFactoryType
} */
function addStyle(Columns: any[]) {
    Columns.forEach((item, index) => {
        if (index == 0) {
            item['className'] = 'v-center';
        }
    });
}
let useOperation = AddOperationParamsWithOut();
let secondTableParams = {};
let [fromParams, APIMessage, firstTableParams, thirdTableParams] = [{}, {}, {}, {}];
const UseParamsAdd = AddCommonPageParams();
let FactoryType: any = UseParamsAdd.Fab;
console.log('renderFuntion页面的FactoryType', FactoryType);

let importurl = ref('LCM');
switch (FactoryType) {
    case 'LCM1':
    case 'LCM2':
        importurl.value = 'LCM';
        break;
    case 'BLU1':
    case 'BLU2':
        importurl.value = 'BLU';
        break;
    default:
        break;
}
console.log(importurl.value);

function getImport(site: any) {
    const OperationColumsData = require(`@/pages/ReportPortal/FS/OperationPerformance/${site}/Table`);
    return OperationColumsData;
}
let OperationColumsData = getImport(importurl.value);

function firstclick(record, rowindex, column, item, index) {
    console.log('-----------------');
    console.log('此时record', record);
    console.log('此时rowindex', rowindex);
    console.log('此时column', column);
    console.log('此时item', item);
    console.log('此时index', index);
    console.log('-----------------');
    if (item == 'PASS_QTY' && record?.PROD_NBR == '合計') {
        return;
    } else if (item == 'QTY' && record?.WORK_CTR == 'Total') {
        return;
    } else {
        useOperation.AddSecondTableParams({ record, rowindex, column, item, index });
    }
}
// 首页跳转到第一层页面,tableParams:vuex的参数,PageSetting:页面的配置
export async function getFirstStart(tableParams: object, PageSetting: object) {
    fromParams = useOperation.FormParams;
    APIMessage = useOperation.APIMessage;
    firstTableParams = useOperation.FirstTableParams;
    PageSetting['SecondColumns'] = []; //初始值设置为空
    PageSetting['SecondList'] = [];
    console.log('vuex一阶表格数据：', tableParams);
    console.log('输出页面配置', PageSetting);
    if (tableParams['record']['WORK_CTR'] >= 1000 && tableParams['record']['WORK_CTR'] <= 2000) {
        switch (tableParams['item']['dataIndex']) {
            case 'PASS_QTY':
                const {
                    Date: {
                        Data: { Table: Data }
                    }
                } = await GetCommon({
                    apiName: 'OperationPerformancePassQty',
                    name: 'Date',
                    FactoryType: FactoryType,
                    StrSql_w: APIMessage['StrSql_w'],
                    StrSql_w4: APIMessage['StrSql_w4'],
                    Sqlbu2: APIMessage['SQLbu2'],
                    sql_m: APIMessage['sql_m'],
                    row1: tableParams['record']['WORK_CTR'],
                    G_FAC: 'A'
                });
                Data.forEach((item) => {
                    item['GOOD_PASS'] = item['PASS_QTY'] - item['RWK_QTY'];
                });
                let Toppassqty = OperationColumsData.Toppassqty;
                Data.push(getfinalsum(Data, Toppassqty['CalcuList'], 'PROD_NBR', '合計'));
                PageSetting['List'] = Data;
                PageSetting['Columns'] = TableCustomCell0(
                    Toppassqty['Label'],
                    Toppassqty['Props'],
                    Toppassqty['ClickColumn'],
                    firstclick,
                    Toppassqty['MergetList'],
                    PageSetting['List']
                );
                addStyle(PageSetting['Columns']);
                PageSetting['TopTitle'] = Toppassqty['Title'];
                console.log('走到這個流程');
                console.log('输出页面配置', PageSetting);

                break;
            case 'TOTALRWK_QTY':
                const {
                    Date: {
                        Data: { Table: RWkData }
                    }
                } = await GetCommon({
                    apiName: 'OperationPerformanceReworkQty',
                    name: 'Date',
                    FactoryType: FactoryType,
                    G_FAC: 'A',
                    StrSql_w: APIMessage['StrSql_w'],
                    Sqlbu2: APIMessage['SQLbu2'],
                    sql_m: APIMessage['sql_m'],
                    row1: tableParams['record']['WORK_CTR']
                });
                let rwkqty = OperationColumsData.BottomTobeScrap;
                RWkData.push(getfinalsum(RWkData, rwkqty['CalcuList'], 'CODE', '合計'));
                PageSetting['List'] = RWkData;
                let rwkqtyLabel = cloneDeep(rwkqty['Label']);
                rwkqtyLabel[0] = 'Defect Code';
                PageSetting['Columns'] = TableCustomCell0(
                    rwkqtyLabel,
                    rwkqty['Props'],
                    rwkqty['ClickColumn'],
                    firstclick,
                    rwkqty['MergetList'],
                    PageSetting['List']
                );
                addStyle(PageSetting['Columns']);
                PageSetting['TopTitle'] = rwkqty['Title'];
                break;
            case 'WIP_QTY':
                const {
                    Date: {
                        Data: { Table: WipData }
                    }
                } = await GetCommon({
                    apiName: 'WipQty',
                    name: 'Date',
                    FactoryType: FactoryType,
                    G_FAC: 'A',
                    StrSql_w: APIMessage['StrSql_w'],
                    StrSql_w1: APIMessage['StrSql_w1'],
                    StrSql_w2: APIMessage['StrSql_w2'],
                    Sqlbu1: APIMessage['SQLbu1'],
                    owner_code: '',
                    vdate_e: fromParams['DateTo'],
                    row1: tableParams['record']['WORK_CTR'],
                    col0: 'WIP'
                });
                let TopWip = OperationColumsData.TopWip;
                WipData.push(getfinalsum(WipData, TopWip['CalcuList'], 'PROD_NBR', '合計'));
                PageSetting['List'] = WipData;
                PageSetting['Columns'] = TableCustomCell0(
                    TopWip['WipQtyLabel'],
                    TopWip['WipQtyProp'],
                    TopWip['ClickColumn'],
                    firstclick,
                    [],
                    []
                );
                addStyle(PageSetting['Columns']);
                PageSetting['TopTitle'] = TopWip['Title'];
                break;

            default:
                break;
        }
    } else {
        switch (tableParams['item']['dataIndex']) {
            case 'IN_QTY':
            case 'PASS_QTY':
            case 'PASS2_QTY':
            case 'RATE':
                const {
                    Date: {
                        Data: { Table: Data }
                    }
                } = await GetCommon({
                    name: 'Date',
                    apiName: 'Rework',
                    FactoryType: FactoryType,
                    G_FAC: 'A',
                    Sqlbu1: '',
                    Sqlbu2: '',
                    StrSql_w: '',
                    StrSql_w1: APIMessage['StrSql_w1'],
                    StrSql_w2: '',
                    col0: tableParams['item']['title'].replace(/\*/g, ''),
                    col1: '',
                    owner_code: '',
                    row0: tableParams['record']['WORK_CTR'],
                    row1: tableParams['record']['WORK_CTR'],
                    sql_m: APIMessage['sql_m'],
                    sql_m1: APIMessage['sql_m1'],
                    vdate_e: fromParams['DateTo']
                });
                let BottomIn = OperationColumsData.BottomIn;
                Data.push(getfinalsum(Data, BottomIn['CalcuList'], 'WORK_CTR', 'Total'));
                PageSetting['List'] = Data;
                PageSetting['TopTitle'] = BottomIn['Title'];
                PageSetting['Columns'] = TableCustomCell0(
                    BottomIn['Label'],
                    BottomIn['Props'],
                    BottomIn['ClickColumn'],
                    firstclick,
                    BottomIn['MergetList'],
                    PageSetting['List']
                );
                addStyle(PageSetting['Columns']);
                break;
            case 'TOBESCRAP_QTY':
                const {
                    Date: {
                        Data: { Table: ToData }
                    }
                } = await GetCommon({
                    name: 'Date',
                    apiName: 'OperationPerformanceReworkQty',
                    FactoryType: FactoryType,
                    G_FAC: 'A',
                    Sqlbu2: APIMessage['SQLbu2'],
                    StrSql_w: APIMessage['StrSql_w'],
                    row1: tableParams['record']['WORK_CTR'],
                    sql_m: APIMessage['sql_m']
                });
                if (ToData.length != 0) {
                    let BottomTobeScrap = OperationColumsData.BottomTobeScrap;
                    ToData.push(getfinalsum(ToData, BottomTobeScrap['CalcuList'], 'COUNT'));
                    PageSetting['List'] = ToData;
                    PageSetting['TopTitle'] = BottomTobeScrap['Title'];
                    PageSetting['Columns'] = TableCustomCell0(
                        BottomTobeScrap['Label'],
                        BottomTobeScrap['Props'],
                        BottomTobeScrap['ClickColumn'],
                        firstclick,
                        [],
                        []
                    );
                    addStyle(PageSetting['Columns']);
                } else {
                    message.warning('無數據！');
                }

                break;
            // 点击了也是没有数据的
            //  case "WIP_QTY":
            //     const { Date: { Data: { Table: WipData } } } = await GetCommon(
            //         {
            //             name: "Date",
            //             apiName: "OperationPerformanceDetail",
            //             FactoryType: FactoryType,
            //             G_FAC: 'A',
            //             Sqlbu1: APIMessage["SQLbu1"],
            //             Sqlbu2: APIMessage["SQLbu2"],
            //             StrSql_w: APIMessage['StrSql_w'],
            //             StrSql_w1: APIMessage['StrSql_w1'],
            //             StrSql_w2: APIMessage['StrSql_w2'],
            //             col0: tableParams["item"]["title"].replace(/\*/g, ''),
            //             col1: "",
            //             row0: "",
            //             owner_code: "",
            //             row1: tableParams["record"]["WORK_CTR"],
            //             row2: secondTableParams['PROD_NBR'] || '',
            //             row3: secondTableParams['VER'] || '',
            //             sql_m: APIMessage['sql_m'],
            //             sql_m1: APIMessage['sql_m1'],
            //             vdate_e: fromParams['DateTo']
            //         }
            //     )
            //     if (WipData.length != 0) {
            //         let wipqty = OperationColumsData.Wipsecond;
            //         let wiptotalsum = getfinalsum(WipData, wipqty["CalcuList"], "PROD_NBR");
            //         Inserttotalrow(WipData, wipqty["insertrow"]["condition"], wipqty["CalcuList"], "PROD_NBR");
            //         WipData.push(wiptotalsum);
            //         PageSetting["List"] = WipData;
            //         PageSetting["TopTitle"] = wipqty["Title"];
            //         PageSetting["Columns"] = TableCustomCell0(wipqty["Label"], wipqty["Props"], [], null, wipqty["MergetList"], WipData, []);
            //     } else {
            //         message.warning("無數據！")
            //     }
            //     break;
            default:
                break;
        }
    }
}
//第二层点击
export async function getDetailStart(tableParams: any, PageSetting: any) {
    // 获取二层点击参数
    secondTableParams = useOperation.SecondTableParams;
    // 获取一层点击参数
    firstTableParams = useOperation.FirstTableParams;
    // 获取首页左侧参数
    fromParams = useOperation.FormParams;
    console.log('参数来的一层的tableparams:', tableParams);
    console.log('secondTableParams', secondTableParams);
    console.log('一层参数firstTableParams：', firstTableParams);
    if (tableParams['record']['WORK_CTR'] >= 1000 && tableParams['record']['WORK_CTR'] <= 2000) {
        switch (tableParams['item']['dataIndex']) {
            // 上表格
            case 'PASS_QTY':
                const {
                    Date: {
                        Data: { Table: Data }
                    }
                } = await GetCommon({
                    apiName: 'OperPerfDetail8',
                    name: 'Date',
                    FactoryType: FactoryType,
                    G_FAC: 'A',
                    StrSql_w4: '',
                    Sqlbu2: '',
                    sql_m: APIMessage['sql_m'],
                    sql_m1: '',
                    sql_owner: '',
                    row0: secondTableParams['record']['PROD_NBR'],
                    row1: tableParams['record']['WORK_CTR'],
                    row2: secondTableParams['record']['EQUIP_NBR'] || '',
                    row3: secondTableParams['record']['SHIFT_ID'] || ''
                });
                let qty = OperationColumsData.Toppassqtysecond;
                let totalsum = getfinalsum(Data, qty['CalcuList'], 'ENTITY_NO');
                Inserttotalrow(Data, qty['insertrow']['condition'], qty['CalcuList'], 'ENTITY_NO');
                Data.push(totalsum);
                PageSetting['SecondList'] = Data;
                PageSetting['BottomTitle'] = qty['Title'];
                PageSetting['SecondColumns'] = TableCustomCell0(
                    qty['Label'],
                    qty['Props'],
                    [],
                    null,
                    qty['MergetList'],
                    Data,
                    []
                );
                addStyle(PageSetting['Columns']);
                break;
            case 'TOTALRWK_QTY':
                const {
                    Date: {
                        Data: { Table: tqyData }
                    }
                } = await GetCommon({
                    apiName: 'OperationPerformanceDetail',
                    name: 'Date',
                    FactoryType: FactoryType,
                    G_FAC: 'A',
                    StrSql_w: APIMessage['StrSql_w'],
                    StrSql_w1: APIMessage['StrSql_w1'],
                    StrSql_w2: APIMessage['StrSql_w2'],
                    Sqlbu1: APIMessage['SQLbu1'],
                    Sqlbu2: APIMessage['SQLbu2'],
                    sql_m: APIMessage['sql_m'],
                    sql_m1: APIMessage['sql_m1'],
                    owner_code: fromParams['LcmOwner'],
                    vdate_e: fromParams['DateTo'],
                    col0: 'Defect Code',
                    col1: tableParams['record']['WORK_CTR'],
                    row0: '',
                    row1: tableParams['record']['WORK_CTR'],
                    row2: secondTableParams['record']['CODE'],
                    row3: '',
                    row4: ''
                });
                let tqty = OperationColumsData.Toppassqtysecond;
                let ttotalsum = getfinalsum(tqyData, tqty['CalcuList'], 'ENTITY_NO');
                Inserttotalrow(tqyData, tqty['insertrow']['condition'], tqty['CalcuList'], 'ENTITY_NO');
                tqyData.push(ttotalsum);
                PageSetting['SecondList'] = tqyData;
                PageSetting['BottomTitle'] = tqty['Title'];
                PageSetting['SecondColumns'] = TableCustomCell0(
                    tqty['Label'],
                    tqty['Props'],
                    [],
                    null,
                    tqty['MergetList'],
                    tqyData,
                    []
                );
                addStyle(PageSetting['Columns']);
                break;
            case 'WIP_QTY':
                const {
                    Date: {
                        Data: { Table: wipData }
                    }
                } = await GetCommon({
                    apiName: 'OperationPerformanceDetail',
                    name: 'Date',
                    FactoryType: FactoryType,
                    G_FAC: 'A',
                    StrSql_w: APIMessage['StrSql_w'],
                    StrSql_w1: APIMessage['StrSql_w1'],
                    StrSql_w2: APIMessage['StrSql_w2'],
                    Sqlbu1: APIMessage['SQLbu1'],
                    Sqlbu2: APIMessage['SQLbu2'],
                    sql_m: APIMessage['sql_m'],
                    sql_m1: APIMessage['sql_m1'],
                    owner_code: fromParams['LcmOwner'],
                    vdate_e: fromParams['DateTo'],
                    col0: 'Wip',
                    col1: tableParams['record']['WORK_CTR'],
                    row0: '',
                    row1: tableParams['record']['WORK_CTR'],
                    row2: secondTableParams['record']['PROD_NBR'],
                    row3: secondTableParams['record']['VER'] || '',
                    row4: ''
                });
                let wipqty = OperationColumsData.Wipsecond;
                let wiptotalsum = getfinalsum(wipData, wipqty['CalcuList'], 'PROD_NBR');
                Inserttotalrow(wipData, wipqty['insertrow']['condition'], wipqty['CalcuList'], 'PROD_NBR');
                wipData.push(wiptotalsum);
                PageSetting['SecondList'] = wipData;
                PageSetting['BottomTitle'] = wipqty['Title'];
                PageSetting['SecondColumns'] = TableCustomCell0(
                    wipqty['Label'],
                    wipqty['Props'],
                    [],
                    null,
                    wipqty['MergetList'],
                    wipData,
                    []
                );
                addStyle(PageSetting['Columns']);
                break;
            default:
                break;
        }
    } else {
        switch (tableParams['item']['dataIndex']) {
            // 下表格
            case 'IN_QTY':
            case 'PASS_QTY':
            case 'PASS2_QTY':
            case 'RATE':
                const {
                    Date: {
                        Data: { Table: BData }
                    }
                } = await GetCommon({
                    apiName: 'OperationPerformanceDetail',
                    name: 'Date',
                    FactoryType: FactoryType,
                    G_FAC: 'A',
                    StrSql_w: APIMessage['StrSql_w'],
                    StrSql_w1: APIMessage['StrSql_w1'],
                    StrSql_w2: APIMessage['StrSql_w2'],
                    Sqlbu1: APIMessage['SQLbu1'],
                    Sqlbu2: APIMessage['SQLbu2'],
                    sql_m: APIMessage['sql_m'],
                    sql_m1: APIMessage['sql_m1'],
                    owner_code: fromParams['LcmOwner'],
                    vdate_e: fromParams['DateTo'],
                    col0: tableParams['item']['title'].replace(/\*/g, ''),
                    col1: tableParams['record']['WORK_CTR'],
                    row0: secondTableParams['record']['WORK_CTR'],
                    row1: tableParams['record']['WORK_CTR'],
                    row2: '',
                    row3: '',
                    row4: ''
                });
                let BottomIn = OperationColumsData.BottomInsecond;
                let Btotalsum = getfinalsum(BData, BottomIn['CalcuList'], 'PROD_NBR');
                Inserttotalrow(BData, BottomIn['insertrow']['condition'], BottomIn['CalcuList'], 'PROD_NBR');
                BData.push(Btotalsum);
                PageSetting['SecondList'] = BData;
                PageSetting['BottomTitle'] = BottomIn['Title'];
                PageSetting['SecondColumns'] = TableCustomCell0(
                    BottomIn['Label'],
                    BottomIn['Props'],
                    [],
                    null,
                    BottomIn['MergetList'],
                    BData,
                    []
                );
                addStyle(PageSetting['Columns']);
                break;
            default:
                break;
        }
    }
}
/* // 第三层点击
export function getThirddetailstart(tableParams: any, PageSetting: any) {
    console.log("执行了");

    // 获取二层点击参数
    secondTableParams = useOperation.SecondTableParams;
    // 获取一层点击参数
    firstTableParams = useOperation.FirstTableParams;
    // 获取首页左侧参数
    fromParams = useOperation.FormParams;
    //  获取第三层参数
    thirdTableParams = useOperation.ThirdTableParams;

    console.log("二层点击的列是", secondTableParams['dataIndex']);
    console.log("点击的参数", secondTableParams);
    console.log("输出一层的参数", firstTableParams);
    console.log("输出tableParams", tableParams);
    console.log("输出PageSetting", PageSetting);
    console.log("输出thirdTableClickParams", thirdTableParams);
    GetQTYThird(tableParams['dataIndex'], tableParams['WORK_CTR'], PageSetting)




} */
