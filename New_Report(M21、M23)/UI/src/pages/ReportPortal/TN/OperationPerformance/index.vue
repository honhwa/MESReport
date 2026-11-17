<template>
    <div class="OperationPerformance">
        <Loading
            :loading="compState.loading"
            :absolute="compState.absolute"
            :theme="compState.theme"
            :background="compState.background"
            :tip="compState.tip"
        />
        <a-card>
            <a-row>
                <span class="menu-fold" @click="showHidden">
                    <component :is="!PageParams.ShowMenu ? MenuUnfoldOutlined : MenuFoldOutlined" />
                </span>
            </a-row>
            <a-row>
                <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                    <SchemaForm @submit="confirm"> </SchemaForm>
                </a-col>
                <a-col :span="PageParams.TableSpan" class="ScrollTable">
                    <DynamicTable
                        showFavorite
                        row-key="id"
                        header-title="Operation Performance"
                        :columns="PageSetting.TopColumns"
                        :key="PageSetting.TopColumns"
                        bordered
                        size="small"
                        :data-source="PageSetting.TopList"
                        :pagination="false"
                        :scroll="{ y: 350 }"
                        export-file-name="Export"
                        :row-class-name="getRowClassName"
                    >
                        <template #export-button>
                            <a-button type="success"> Export </a-button>
                        </template>
                    </DynamicTable>
                    <DynamicTable
                        row-key="id"
                        header-title="Operation Performance Rework"
                        :columns="PageSetting.BottomColumns"
                        :key="PageSetting.BottomColumns"
                        bordered
                        size="small"
                        :data-source="PageSetting.BottomList"
                        :pagination="false"
                        :scroll="{y: 350 }"
                        export-file-name="Export"
                        :row-class-name="getRowClassName"
                    >
                        <template #export-button>
                            <a-button type="success"> Export </a-button>
                        </template>
                    </DynamicTable>
                </a-col>
            </a-row>
        </a-card>
    </div>
</template>

<script lang="ts" setup>
import { MenuFoldOutlined, MenuUnfoldOutlined } from '@ant-design/icons-vue';import {PageParams} from '@/components/core/dynamic-table/src/dynamic-table'
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive } from '@vue/reactivity';
import { inject, onMounted } from '@vue/runtime-core';
import { GetOperactionPerforamce } from '@/api/pages/operationPerformance';
import { getRowClassName } from '@/pages/ReportPortal/utils/TNcommon';
import useRemoteSelect from '@/pages/ReportPortal/utils/TNuseRemoteSelect';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { ReportCommon } from '@/store/module/ReportCommon';
import { Loading, useLoading } from '@/components/feat/Loading';
//import {optionsdata} from '@/pages/ReportPortal/Static/TNCommon-Form-schema';

// defineOptions({
//   name: 'DemosFormDynamicForm',
// });

const UseFormParamsAdd = AddCommonPageParams();
const useReportCommon = ReportCommon();
let ReportParamsItem = reactive({
    APIMessage:'',
    FormParams: {},
    FirstTableParams: {},
    SecondTableParams:reactive({}),
    ThirdTableParams:{},
    ThirdTableClickParams:{},
    Fab:'',
    G_FAC_COM:'',
});
let ReportNameParams = `OperationPerformanceDetail_${UseFormParamsAdd.Fab}`;

const compState = reactive({
    absolute: true,
    loading: false,
    theme: 'dark',
    background: 'rgba(111,111,111,.7)',
    tip: '加载中...'
});

const [openFullLoading, closeFullLoading] = useLoading({
        tip: '數據加載中...',
      });

function getImport(site: any) {
    const {
        schemas,
        getSizeData
    } = require(`@/pages/ReportPortal/TN/OperationPerformance/${site}/form-schema`);
    const {
        topBaseColumns,
        bottomBaseColumns,
        topTableProps,
        bottomTableProps
    } = require(`@/pages/ReportPortal/TN/OperationPerformance/${site}/Table`);
    return { schemas, topBaseColumns, bottomBaseColumns, getSizeData, topTableProps, bottomTableProps };
}
const { schemas, topBaseColumns, bottomBaseColumns, getSizeData, topTableProps, bottomTableProps } =
    getImport("MOD");
const { mounted, InfiniteScroll} = useRemoteSelect();

let PageSetting = reactive({
    TopList: [] as any,
    BottomList: [] as any,
    TableSpan: 20,
    showmenu: true,
    TopColumns: [] as any,
    BottomColumns: [] as any
});
PageSetting.TopColumns = topBaseColumns;
PageSetting.BottomColumns = bottomBaseColumns;
let FactoryType: any = UseFormParamsAdd.Fab;
const [DynamicTable] = useTable();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas: schemas(FactoryType),
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
    // fieldMapToTime: [['fieldTime', ['vDate_s', 'vDate_e1'], 'YYYY-MM-DD']]
});
// 点击提交
// PageSetting.columns = [...baseColumns];

function confirm() {
    openFullLoading();
    // console.log('dynamicForm.value', dynamicFormRef.value?.formModel);
    const ParamsData: any = {
        name: 'Date',
        apiName: 'TN_OperationPerformance',
        FactoryType: UseFormParamsAdd.Fab,
        FacId: 'A',
        Building: 'A',
        DateFrom: '',
        DateTo: '',
        Shift: 'ALL',
        Floor: 'ALL',
        WorkOrder: '',
        LcmProductType: 'ALL',
        Size: 'ALL',
        BigProduct: 'ALL',
        LcmOwner: [],
        LcdGrade: [],
        Product: [],
        ProdId: []
    };
    for (let i in dynamicFormRef.formModel) {
        if (i == 'wo_num') ParamsData[i] = dynamicFormRef.formModel[i].split(',');
        else ParamsData[i] = dynamicFormRef.formModel[i];
    }

    dynamicFormRef.validate().then(async () => {
        const {
            Date: {
                Data: { Table: Data },
                SQL01,
                SQLbu1,
                SQLbu2,
                StrSql_w,
                StrSql_w1,
                StrSql_w2,
                StrSql_w2e,
                StrSql_w3,
                StrSql_w4,
                StrSql_we,
                StrSql_ww,
                sql_m,
                sql_m1,
                sql_m2
            }
        } = await GetOperactionPerforamce(ParamsData);
        /* if (!Success) {
            return message.error(Message);
        } */
        let Message = {
            SQL01: SQL01,
            SQLbu1: SQLbu1,
            SQLbu2: SQLbu2,
            StrSql_w: StrSql_w,
            StrSql_w1: StrSql_w1,
            StrSql_w2: StrSql_w2,
            StrSql_w2e: StrSql_w2e,
            StrSql_w3: StrSql_w3,
            StrSql_w4: StrSql_w4,
            StrSql_we: StrSql_we,
            StrSql_ww: StrSql_ww,
            sql_m: sql_m,
            sql_m1: sql_m1,
            sql_m2: sql_m2,
        };
        //UseFormParamsAdd.AddAPIResultMessage(JSON.stringify(Message));
        UseFormParamsAdd.AddFormParams(ParamsData);

        ReportParamsItem.APIMessage = JSON.stringify(Message);
        ReportParamsItem.FormParams = ParamsData;
        useReportCommon.AddObject(ReportNameParams , ReportParamsItem)

        let TopList: any = [];
        let BottomList: any = [];
        let RwkSumList: any = [];
        Data.forEach((item) => {
            if ((item.WORK_CTR >= 2000 && item.WORK_CTR < 3000) || item.WORK_CTR == 0) {
                // item['TD'] = handprops(item, 'WIP_QTY', 'PASS_QTY');
                // item['TR'] = handprops(item, 'PASS_QTY', 'IN_QTY');
                // item['PASS2_QTYRate'] = handfloatprops(item, 'PASS2_QTY', 'PASS_QTY');
                BottomList.push(item);
            } else if ((item.WORK_CTR > 1000 && item.WORK_CTR < 2000) || item.WORK_CTR >= 6000) {
                // item['TD'] = handprops(item, 'WIP_QTY', 'PASS_QTY');
                // item['TR'] = handprops(item, 'PASS_QTY', 'IN_QTY');
                // item['FirstYield'] = handfloatprops(item, 'PASS1_QTY', 'PASS_QTY');
                // item['TotalYield'] =
                //     (item.PASS_QTY - item.TOTALRWK_QTY) / item.PASS_QTY == Infinity
                //         ? 0
                //         : (((item.PASS_QTY - item.TOTALRWK_QTY) / item.PASS_QTY) * 100).toFixed(2);
                // item['TDR'] = handprops(item, 'RWK_WIP_QTY', 'RWK_PASS_QTY');
                // item['TRR'] = handprops(item, 'RWK_PASS_QTY', 'RWK_IN_QTY');
                // item['RepairRate'] = handfloatprops(item, 'RWK_TORWK_QTY', 'RWK_PASS_QTY');
                TopList.push(item);
            }
        });
        // 合计行
        //handleData(TopList, topTableProps);
        //handleData(BottomList, bottomTableProps);
        // 最后一行的计算
        // TopList.forEach((item) => {
        //     if (Number(item.WORK_CTR) == 1100) {
        //         TopList[TopList.length - 1].IN_QTY = item.IN_QTY;
        //     }
        //     if (Number(item.WORK_CTR) == 1910) {
        //         TopList[TopList.length - 1].PASS_QTY = item.PASS_QTY;
        //     }
        // });
        //TopList[TopList.length - 1].TD = handprops(TopList[TopList.length - 1], 'WIP_QTY', 'PASS_QTY');
        //TopList[TopList.length - 1].TR = handprops(TopList[TopList.length - 1], 'PASS_QTY', 'IN_QTY');
        //TopList[TopList.length - 1]['WORK_CTR'] = 'SUM';
        // BottomList.forEach((item) => {
        //     debugger
        //     if (Number(item.WORK_CTR) == 2100) {
        //         BottomList[BottomList.length - 1].IN_QTY = item.PASS_QTY;
        //     }
        //     if (Number(item.WORK_CTR) == 2490) {
        //         BottomList[BottomList.length - 1].PASS_QTY =
        //             parseFloat(item.PASS_QTY * 100) / 100 +
        //             parseFloat(BottomList[BottomList.length - 1].TOBESCRAP_QTY * 100) / 100;
        //     }
        // });
        //BottomList[BottomList.length - 1].TD = handprops(BottomList[BottomList.length - 1], 'WIP_QTY', 'PASS_QTY');
        //BottomList[BottomList.length - 1].TR = handprops(BottomList[BottomList.length - 1], 'PASS_QTY', 'IN_QTY');
        // TopList[TopList.length - 1].TotalYield =
        //     parseFloat(TopList[TopList.length - 1].PASS_QTY * 100) /
        //     100 /
        //     (parseFloat(BottomList[BottomList.length - 1].TOBESCRAP_QTY * 100) / 100 +
        //         parseFloat(TopList[TopList.length - 1].PASS_QTY * 100) / 100);
        // TopList[TopList.length - 1].TotalYield = TopList[TopList.length - 1].TotalYield.toFixed(4) * 100;
        BottomList[BottomList.length - 1]['WORK_CTR'] = 'RWK';
        PageSetting.TopList = TopList;
        PageSetting.BottomList = BottomList;
        closeFullLoading();
    });
    
}

function showHidden() {
    PageParams.ShowMenu = !PageParams.ShowMenu;
    if (PageParams.ShowMenu) PageParams.TableSpan = 20;
    else PageParams.TableSpan = 24;
}
// 属性处理
function handprops(item, prop1, prop2) {
    item[prop1] = Number(item[prop1]);
    item[prop2] = Number(item[prop2]);
    return item[prop1] / item[prop2] == Infinity || item[prop1] == 0 || isNaN(item[prop1] / item[prop2])
        ? 0
        : (item[prop1] / item[prop2]).toFixed(2);
}
// float属性处理
function handfloatprops(item, prop1, prop2) {
    item[prop1] = Number(item[prop1]);
    item[prop2] = Number(item[prop2]);
    return item[prop1] / item[prop2] == Infinity || item[prop1] == 0 || isNaN(item[prop1] / item[prop2])
        ? 0
        : ((item[prop1] / item[prop2]) * 100).toFixed(2);
}
// 总计行
function handleData(dataSource: any[], colAttr: any[]) {
    let keysArr = colAttr;
    const tempObj = JSON.parse(JSON.stringify(dataSource[0]));
    keysArr.forEach((el) => {
        for (let i = 1; i < dataSource.length; i++) {
            if (el == 'WORK_CTR') {
                continue;
            } else if (el == 'YIELD1') {
                if (isNaN(dataSource[i][el])) {
                    dataSource[i][el] = 0.0;
                } else if (isNaN(tempObj[el])) {
                    tempObj[el] = 0.0;
                }
                tempObj[el] = parseFloat(tempObj[el] * 100) / 100;
                tempObj[el] *= parseFloat(dataSource[i][el] * 100) / 100;
                tempObj[el] = tempObj[el].toFixed(2);
                continue;
            } else if (
                [
                    'TDR',
                    'TRR',
                    'RWK_PASS_QTY',
                    'RWK_IN_QTY',
                    'PASS2_QTYRate',
                    'S1_WIP_QTY',
                    'S2_WIP_QTY',
                    'S3_WIP_QTY',
                    'RepairRate'
                ].includes(el)
            ) {
                tempObj[el] = '';
                continue;
            } else {
                if (isNaN(dataSource[i][el])) {
                    dataSource[i][el] = 0.0;
                } else if (isNaN(tempObj[el])) {
                    tempObj[el] = 0.0;
                }
                tempObj[el] = Number(tempObj[el]);
                tempObj[el] += Number(dataSource[i][el]);
            }
        }
        /* if (index == 0) {
            tempObj[el] = '总计';
        } */
    });
    dataSource.push(tempObj);
}
/* function getTotalData(data: any[], columns: any[]) {
    //合計
    let obj = {};
    columns.map((m: Object) => {
        if (m['dataIndex'] != 'WORK_CTR') {
            let sum = 0;
            data.map((n) => {
                sum += isNaN(n[m['dataIndex']]) ? 0 : parseInt(n[m['dataIndex']]);
            });
            obj[m['dataIndex']] = sum;
        }
    });
    return obj;
} */

onMounted(async () => {
    //await mounted(optionsdata, UseFormParamsAdd.Fab);
});
</script>
<!-- <script lang="ts">
export default{
    name:'OperationPerformance'
}
</script> -->

<style lang="less" scoped>
.btn-rows {
    button {
        margin-right: 12px;
    }
}

.OperationPerformance :deep(.v-center) {
    background-color: #f0f0f0;
}

.OperationPerformance :deep(.Yellow) {
    background-color: #80ff80;
}

.OperationPerformance :deep(.YellowGreen) {
    background-color: #f39230;
}

// .baseModel {
//     overflow-y: auto;
//     overflow-x: hidden;
//      max-width: 200px;
//     height: 85vh;
// }
</style>
