<template>
    <div class="WOOrderStatusReport">
        <a-row>
            <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                <SchemaForm @submit="confirm"> </SchemaForm>
            </a-col>
            <a-col :span="PageParams.TableSpan">
                <DynamicTable row-key="WOOrderStatusReport" :header-title="PageSetting.Title" :PageParams="PageParams"
                    id="WOOrderStatusReport" export-type="Exceljs" :exceljsparams="PageSetting.exceljsparams"
                    :columns="PageSetting.Columns" showFavorite :key="PageSetting.Columns" bordered :flexwidth="1"
                    :row-class-name="getRowClassNmae" :data-source="PageSetting.moreData" :data-all="PageSetting.moreData"
                    :pagination="false" sticky export-file-name="导出">
                    <template #export-button>
                        <a-button type="success"> 导出 </a-button>
                    </template>
                </DynamicTable>
            </a-col>
        </a-row>
    </div>
</template>

<script lang="tsx" setup>
import { useLoading } from '@/components/feat/Loading';
import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table';
import { useTable } from '@/components/core/dynamic-table';
import { reactive } from '@vue/reactivity';
import { useForm } from '@/components/core/schema-form';
import { GetCommon } from '@/api/pages/common';
import { message } from 'ant-design-vue';
import { getfinalsum } from '@/utils/reportcommon/Calculation';
import { cloneDeep } from 'lodash';
import { ReportCommonWithOut } from '@/store/module/ReportCommon';
import { antcustomCell, antdclick } from '@/pages/ReportPortal/utils/common';
import { WoOrderStatusReportWithOut } from '@/store/module/WoOrderStatusReport';
import router from '@/router';

const UseWoOrder = WoOrderStatusReportWithOut();
const ReportCommon = ReportCommonWithOut();
let FactoryType: string = ReportCommon.Fab;

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});
// 頁面數據
let PageSetting = reactive<BasePageParams.BasePageSetting>({
    List: [] as any,
    Columns: [] as any,
    Title: '',
    moreData: [],
    SQLHidden: '',
    APIColumns: [] as any,
    exceljsparams: {}
});

switch (FactoryType) {
    case 'LCM1':
        PageSetting.Title = 'LCMA WO Order Status Report';
        break;
    case 'LCM2':
        PageSetting.Title = 'LCM MOD段工單狀態Report';
        break;
    case 'BLU1':
    case 'BLU2':
        PageSetting.Title = 'BLA WO Order Status Report';
        break;
    case 'SET1':
        PageSetting.Title = 'LCMA_TV WO Order Status Report';
        break;
    case 'SET2':
        PageSetting.Title = 'LCM2_TV WO Order Status Report';
        break;
    case 'BEOL1':
        PageSetting.Title = 'LCD WO Order Status Report';
        break;
    case 'BEOL2':
        PageSetting.Title = 'LCDB WO Order Status Report';
        break;
    default:
        break;
}
function getImport(site: any) {
    const { schemas } = require(`@/pages/ReportPortal/FS/WOOrderStatusReport/${site}/form-schema`);
    const {
        baseColumns,
        CalcuList,
        ClickColumn,
        exceljsparams
    } = require(`@/pages/ReportPortal/FS/WOOrderStatusReport/${site}/table`);
    return { schemas, baseColumns, CalcuList, ClickColumn, exceljsparams };
}
const { schemas, baseColumns, CalcuList, ClickColumn, exceljsparams } = getImport(FactoryType);
PageSetting.exceljsparams = exceljsparams;
const [DynamicTable] = useTable();

const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas: schemas(FactoryType),
    actionColOptions: {
        span: 24
    },
    fieldMapToTime: [['fieldTime', ['vDate_s', 'vDate_e1'], 'YYYY-MM-DD']],
    layout: 'vertical'
});

let TopTableParams = reactive({
    name: 'Date',
    apiName: 'WOOrderStatus',
    ReportType: 'WoUnClose60DataSet',
    FactoryType: FactoryType,
    G_FAC: 'A',
    QueryRange: '',
    WorkOrderType: [],
    Product: [],
    DEST_SHOP: [],
    isWoDueDay: '',
    FromDate_DueDay: '',
    ToDate_DueDay: '',
    isWoStartDay: '',
    FromDate_StartDay: '',
    ToDate_StartDay: '',
    // WorkOrder: [],
    WorkOrder: '',
    workOrderLike: [],
    LCMProductType: '',
    wo_status: '',
    mvin_rate: '',
    mvou_rate: ''
});
function getRowClassNmae(record: Object) {
    if (record && Object.keys(record).length != 0) {
        for (const [key, value] of Object.entries(record)) {
            if (typeof value == 'string' && ['小計', '合計'].includes(value)) {
                return 'Yellow';
            } else if (typeof value == 'string' && ['總計', 'SUM', 'Sum'].includes(value)) {
                return 'Green';
            }
        }
    }
}

async function confirm() {
    openFullLoading();
    let tempkeys = Object.keys(TopTableParams);
    for (let i in cloneDeep(dynamicFormRef.formModel)) {
        if (tempkeys.includes(i)) {
            if (i == 'WorkOrder') {
                TopTableParams[i] = cloneDeep(dynamicFormRef.formModel[i])
                    .split(/[\r\n\t]/)
                    .filter((item) => item);
            } else {
                TopTableParams[i] = cloneDeep(dynamicFormRef.formModel[i]);
            }
        }
    }
    try {
        const {
            Date: {
                Data0: { Table: Data },
                Message: sqlmessage
            }
        } = await GetCommon(TopTableParams);
        if (Data.length == 0) {
            closeFullLoading();
            PageSetting.List = [];
            PageSetting.Columns = [];
            return message.warn("無數據!");
        }
        let totalsum;
        let tempClolums = cloneDeep(baseColumns);
        if (['BEOL1', 'BEOL2'].includes(FactoryType)) {
            totalsum = getfinalsum(Data, CalcuList, 'WORK_ORDER_ID');
            antcustomCell(tempClolums, ({ record, rowindex, column, item, index }) => {
                return {
                    ...antdclick(column, ClickColumn, () => {
                        let detailparams = cloneDeep(TopTableParams);
                        detailparams['ReportType'] = 'WoUnClose60Detail';
                        UseWoOrder.AddALLFormParams({
                            ...detailparams,
                            vWoNbr: `(${record['WORK_ORDER_ID']})`,
                            colName0: item.key,
                            colName1: '',
                            row0: record['WORK_ORDER_ID']
                        });
                        router.push({ name: 'WOOrderStatusReportDetail' + FactoryType });
                        /* await GetCommon({
                            ...detailparams,
                            vWoNbr: `(${record["WORK_ORDER_ID"]})`,
                            colName0: item.key,
                            colName1: "",
                            row0: record["WORK_ORDER_ID"]
                        }) */
                    })
                }
            })
        } else {
            totalsum = getfinalsum(Data, CalcuList, 'WO_NBR');
            totalsum['RECY_SCRAP_SHIP'] =
                totalsum['SHIP_QTY'] == 0
                    ? 0
                    : (((totalsum['QTY1'] + totalsum['SCRAP_QTY']) / totalsum['SHIP_QTY']) * 100).toFixed(2);
            antcustomCell(tempClolums, ({ record, rowindex, column, item, index, paramsColumns }) => {
                return {
                    ...antdclick(column, ClickColumn, () => {
                        let detailparams = cloneDeep(TopTableParams);
                        detailparams['ReportType'] = 'WoUnClose60Detail';
                        UseWoOrder.AddALLFormParams({
                            ...detailparams,
                            vWoNbr: record['WO_NBR'],
                            colName0: paramsColumns['title'],
                            colName1: item['title']
                        });
                        ReportCommon.AddFirstTableParams("WOOrderStatusReport" + FactoryType, {
                            ...detailparams,
                            vWoNbr: record['WO_NBR'],
                            colName0: paramsColumns['title'],
                            colName1: item['title']
                        });
                        router.push({ name: 'WOOrderStatusReportDetail' + FactoryType });
                    }),
                }
            })
        }
        Data.push(totalsum);
        PageSetting.moreData = Data;
        PageSetting.moreData.forEach(item => {
            ['MVIN_RATE', 'MVOU_RATE', 'RECY_SCRAP_SHIP'].forEach(kitem => {
                if (item[kitem] || item[kitem] == 0) {
                    item[kitem] = item[kitem] + "%";
                }
            })
        })
        console.log("输出最终的数据", PageSetting.moreData);
        // PageSetting.moreData = PageSetting.moreData.slice(0,10);

        PageSetting.Columns = tempClolums;
        closeFullLoading();
    } catch (error) {
        closeFullLoading();
    }
}
</script>

<style lang="less" scoped></style>
