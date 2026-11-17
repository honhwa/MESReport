<template>
    <div class="Report">
        <a-row>
            <a-col :span="24 - PageParams.TableSpan" class="ScrollForm">
                <SchemaForm @submit="confirm"> 
                </SchemaForm>
                <a-button type="success" @click="ShowTable=!ShowTable" class="submitButton">{{ShowTable?'显 示 图 表':'显 示 表 格'}}</a-button>

            </a-col>
            <a-col :span="PageParams.TableSpan">

                <div ref="report" v-if="ShowTable">
                    <DynamicTable
                        row-key="report"
                        :header-title="PageSetting.Title"
                        :columns="PageSetting.Columns"
                        showFavorite
                        sticky
                        :key="PageSetting.Columns"
                        bordered
                        :row-class-name="getRowClassNmae"
                        :data-source="PageSetting.moreData"
                        :data-all="PageSetting.moreData"
                        :pagination="false"
                        :export-file-name="PageSetting.Title"
                    >
                        <template #export-button>
                            <a-button type="success"> 导出 </a-button>
                        </template>
                    </DynamicTable>
                </div>
                <MixedChart
                    v-else
                    height="80%"
                    width="90%"
                    :x-data="PageSetting.ChartXData"
                    :seriesData="PageSetting.ChartData"
                />
            </a-col>
        </a-row>
    </div>
</template>

<script lang="ts" setup>
import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table';
import { useLoading } from '@/components/feat/Loading';
import { GetCommon } from '@/api/pages/common';
import { uniqBy } from 'lodash-es';
import { onMounted } from '@vue/runtime-core';
import { reactive, ref } from '@vue/reactivity';
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { ReportCommon } from '@/store/module/ReportCommon';
import { getRowClassNmae } from '@/pages/ReportPortal/utils/common';
import MixedChart from '@/components/charts/MixedChart.vue';

const UseParamsAdd = ReportCommon();
const ShowTable = ref(true);
let FactoryType: string = UseParamsAdd.Fab;
const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});
const report = ref<HTMLElement | null>(null);
function getImport(site: string) {
    const { schemas, optionsdata } = require(`@/pages/ReportPortal/FS/EventTop10Pareto/${site}/form-schema`);
    const { baseColumns } = require(`@/pages/ReportPortal/FS/EventTop10Pareto/${site}/Table`);
    return { schemas, optionsdata, baseColumns };
}
let { schemas, optionsdata, baseColumns } = getImport('LCM1');

let PageSetting = reactive<BasePageParams.BasePageSetting>({
    List: [] as any,
    Columns: [] as any,
    Title: '',
    moreData: [],
    SQLHidden: '',
    APIColumns: [] as any,
    ChartData: [],
    ChartXData: []
});
switch (FactoryType) {
    case '':
        PageSetting.Title = '';
        break;
    default:
        break;
}
const [DynamicTable] = useTable();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 150,
    schemas:schemas(FactoryType),
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
});

async function confirm(values) {
    openFullLoading();
    let baseParams = {
        apiName: 'EventTop10ParetoDataSet',
        FactoryType: FactoryType,
        G_FAC: UseParamsAdd.G_FAC_COM
    };
    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        }
    });

    try {
        let { api01 } = await GetCommon({ ...baseParams, ...values });
        console.log(api01);
        
        let TableData = api01['Data2']['Table'];
        let CountData = uniqBy(api01['Data3']['Table'], 'NEXT_CODE');
        let ShowTableData: any[] = [],
            ChartData: any[] = [],
            XData: any[] = [],
            LineData: any[] = [];
        CountData.forEach((item1, index) => {
            ShowTableData.push({
                event: item1['C_DESC'],
                NO: index + 1,
                SUBALLTIME: item1['SUBTIME'],
                SUBCOUNT: item1['COUNT'],
                event1: '小計'
            });
            TableData.forEach((item) => {
                if (item['NEXT_CODE'] == item1['NEXT_CODE']) {
                    ShowTableData.push(item);
                }
            });
            ChartData.push(values['ShowType']=='count'?item1['COUNT']:item1['SUBTIME']);
            XData.push(item1['C_DESC']);
        });
        let LineStartData = 0
        ChartData.forEach(item=>{
            LineStartData=(item*1000+LineStartData*1000)/1000
            LineData.push(LineStartData)
        })
        // LineData.push(ChartData[0]);
        // ChartData.reduce((prev, cur) => {
        //     LineData.push(prev + cur);
        //     return prev + cur;
        // });

        PageSetting.moreData = ShowTableData;
        PageSetting.ChartData = [
            { name: '数量', data: ChartData, type: 'bar', barWidth: 40 },
            { name: '累计次数', data: LineData, type: 'line' }
        ];
console.log(PageSetting.ChartData);


        PageSetting.ChartXData = XData;
        PageSetting.Columns = baseColumns;
        closeFullLoading();
    } catch (error) {
        closeFullLoading();
    }
}

onMounted(async () => {});
</script>

<style lang="less" scoped></style>
