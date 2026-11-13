<template>
    <div class="ChangeProductReport">
        <a-row>
            <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                <SchemaForm @submit="confirm"> </SchemaForm>
            </a-col>
            <a-col :span="PageParams.TableSpan">
                <div>
                    <DynamicTable row-key="ChangeProductReport" header-title="LCMA Change Product Report " showFavorite
                        export-type="Exceljs" :PageParams="PageParams" sticky :columns="PageSetting.TopColumns"
                        :key="PageSetting.TopColumns" bordered :flexwidth="1" :row-class-name="getRowClassNmae"
                        :data-source="PageSetting.TopList" :data-all="PageSetting.TopList" :pagination="false"
                        export-file-name="导出">
                        <template #export-button>
                            <a-button type="success"> 导出 </a-button>
                        </template>
                    </DynamicTable>
                </div>
            </a-col>
        </a-row>
    </div>
</template>


<script lang="tsx" setup>
import {PageParams} from '@/components/core/dynamic-table/src/dynamic-table'
import { useTable } from '@/components/core/dynamic-table';
import { reactive, ref } from '@vue/reactivity';
import { useForm } from '@/components/core/schema-form';
import { GetCommon } from '@/api/pages/common';
import { inject, onMounted } from '@vue/runtime-core';
import { message } from 'ant-design-vue';
import { cloneDeep } from 'lodash';
import { getPovitData, PovitParams } from "@/utils/reportcommon/provit";
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import {
    getRowClassNmae,
    dclick,
    getfinalsum,
    getrowsum,
    Inserttotalrow,
    antcustomCell,
    antdclick,
    antmegercell,
} from '@/pages/ReportPortal/utils/common';
import { ChangeProductReportWithOut } from '@/store/module/ChangeProductReport';
import router from '@/router';
import dayjs from 'dayjs';
import { ReportCommon } from '@/store/module/ReportCommon';
const UseWoOrder = ChangeProductReportWithOut();
const UseParamsAdd = ReportCommon();
let FactoryType: any = UseParamsAdd.Fab;
const ChangeProductReport = ref<HTMLElement | null>(null);

function getImport(site: any) {
    const {
        schemas,
        optionsdata,
    } = require(`@/pages/ReportPortal/FS/ChangeProductReport/${site}/form-schema`);
    const {
        middle,
        leftColumns,
    } = require(`@/pages/ReportPortal/FS/ChangeProductReport/${site}/table`);
    return { schemas, optionsdata, middle, leftColumns };
}
const { schemas, optionsdata, middle, leftColumns } = getImport(FactoryType);
// 頁面數據
let PageSetting = reactive({
    TopList: [] as any,
    TopColumns: [] as any,
    Message: {},
});
const [DynamicTable] = useTable();
const { mounted, InfiniteScroll, mountedLcmProdType, mountedOper } = useRemoteSelect()
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas: schemas(FactoryType),
    actionColOptions: {
        span: 24,
    },
    fieldMapToTime: [['fieldTime', ['vDate_s', 'vDate_e1'], 'YYYY-MM-DD']],
    layout: 'vertical',
});


let TopTableParams = reactive({
    name: "Date",
    apiName: "ChangeProduct",
    ReportType: "ChangeProdDataSet",
    FactoryType: FactoryType,
    G_FAC: "A",
    FromDate: "",
    FromTime: "",
    ToDate: "",
    ToTime: "",
    Shift: "",
    Floor: "",
    Interval: "",
    Owner: "",
    ConversionForm: "",
    Product: [],
    Operation: [],
    ShowFields: [] as string[],
    sqlcond: '',
});

async function confirm() {
    PageSetting.TopList = [];
    PageSetting.TopColumns = [];
    let tempkeys = Object.keys(TopTableParams);
    for (let i in cloneDeep(dynamicFormRef.formModel)) {
        if (tempkeys.includes(i)) {
            if (["FromTime", "ToTime"].includes(i)) {
                TopTableParams[i] = dayjs(dynamicFormRef.formModel[i]["$d"]).format("HH.mm")
            }
            else {
                TopTableParams[i] = cloneDeep(dynamicFormRef.formModel[i]);
            }
        }
    }
    if (TopTableParams["ShowFields"].length == 0) {
        message.warning("請輸入展示欄位")
    } else {
        const {
            Date: {
                Data0: { Table: TimeData },
                Data1: { Table: Data },
                Message: sqlmessage,
            },
        } = await GetCommon(TopTableParams);
        if (Data.length == 0) {
            return message.warning('無數據!');
        }
        // 中間的Props
        let middleColums: object[] = [];
        TopTableParams['ShowFields'].forEach((item) => {
            middle.forEach((mitem) => {
                if (item == mitem['value']) {
                    middleColums.push({
                        title: mitem['label'],
                        align: 'center',
                        dataIndex: item.toUpperCase(),
                    });
                }
            })
        })
        // 中間的時間獲取
        let timeArray: string[] = [];
        let timeColumns: object[] = [];
        TimeData.forEach(item => {
            timeArray.push(item["ACCT_DATE"]);
            timeColumns.push({
                title: item["ACCT_DATE"],
                align: 'center',
                dataIndex: item['ACCT_DATE'],
            });
        });
        

        let config: PovitParams = {
            Data: Data,
            povitColumn: {
                ACCT_DATE: 'QTY1',
            },
            changeColumn: timeArray,
            repeatColumn: ['P2E', ...TopTableParams['ShowFields']].map(item => {
                return item.toUpperCase();
            })
        };
        let tempColumns;
        if (FactoryType == 'LCM1') {
            tempColumns = [
                leftColumns,
                ...middleColums,
                ...timeColumns,
                {
                    title: '總計',
                    align: 'center',
                    dataIndex: 'Sum',
                },
            ];
        } else {
            tempColumns = [leftColumns, ...middleColums, ...timeColumns];
        }

        let PoviData = getPovitData(config);
        if (FactoryType == 'LCM1' || FactoryType == 'LCM2') {
            getrowsum(PoviData, timeArray, 'Sum');
            PoviData.push(getfinalsum(PoviData, [...timeArray, 'Sum'], 'P2E'));
        }

        PageSetting['TopList'] = PoviData;
        antcustomCell(tempColumns, ({ record, rowindex, column, item, index }) => {
            return {
                ...antmegercell({ record, rowindex, column }, ['P2E'], PageSetting['TopList']),
                ...antdclick(column, [...timeArray, 'Sum'], () => {
                    let detailparams = cloneDeep(TopTableParams);
                    detailparams['ReportType'] = 'ChangeProdDetail';
                    detailparams['ShowFields'].forEach((item) => {
                        if (item.includes('WORK_CTR')) {
                            detailparams['sqlcond'] =
                                detailparams['sqlcond'] + ` and work_ctr = ${record['WORK_CTR']}`;
                        } else if (item.includes('TRANS_DATE')) {
                            detailparams['sqlcond'] =
                                detailparams['sqlcond'] + ` and TRANS_DATE <= '${record['TRANS_DATE']}'`;
                        } else {
                            detailparams['sqlcond'] =
                                detailparams['sqlcond'] +
                                ` and nvl(trim(${item}),' ') like '${record[item.toUpperCase()]}%'`;
                        }
                    });
                    if (item['dataIndex'] == 'Sum') {
                        detailparams['acct_date'] = '總計';
                    } else {
                        detailparams['acct_date'] = item['dataIndex'];
                    }
                    detailparams['p2e2'] = record['P2E'] == null ? '' : record['P2E'];
                    UseWoOrder.AddALLFormParams(detailparams);
                    UseParamsAdd.AddFirstTableParams('ChangeProductReport' + FactoryType, detailparams);
                    router.push({ name: 'ChangeProductReportDetail' + FactoryType + 'PC' });
                }),
            };
        });
        PageSetting.TopColumns = tempColumns;
        console.log('輸出最終數據', PoviData);
        console.log('輸出最終數據', PageSetting.TopColumns);
    }
}
onMounted(async () => {
    /*  await mounted(optionsdata, FactoryType);
     await mountedOper(optionsdata, FactoryType); */
});
</script>

<style lang="less" scoped></style>
