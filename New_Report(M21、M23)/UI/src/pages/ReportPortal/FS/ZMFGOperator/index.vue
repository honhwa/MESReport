<template>
    <div class="Report">
        <a-card>
            <a-row>
                <a-col :span="24 - PageParams.TableSpan" class="ScrollForm">
                    <SchemaForm @submit="confirm"> </SchemaForm>
                </a-col>
                <a-col :span="PageParams.TableSpan">
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
                        :scroll="{y:350}"
                    >
                        <template #export-button>
                            <a-button type="success"> 导出 </a-button>
                        </template>
                    </DynamicTable>
                    <MixedChart
                        height="350px"
                        width="100%"
                        :x-data="PageSetting.ChartXData"
                        :seriesData="PageSetting.ChartData"
                        title="By 检验/判退图表"
                        :y-data="YDefine"
                        :rotate="-90"

                    />
                </a-col>
            </a-row>
        </a-card>
    </div>
</template>

<script lang="ts" setup>
import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table';
import { useLoading } from '@/components/feat/Loading';
import { GetCommon } from '@/api/pages/common';
import { onMounted } from '@vue/runtime-core';
import { reactive, ref } from '@vue/reactivity';
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { ReportCommon } from '@/store/module/ReportCommon';
import { message } from 'ant-design-vue';
import { getRowClassNmae } from '@/pages/ReportPortal/utils/common';
import MixedChart from '@/components/charts/MixedChart.vue';


const UseParamsAdd = ReportCommon();
let FactoryType: string = UseParamsAdd.Fab;
const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});
function getImport(site: string) {
    const { schemas } = require(`@/pages/ReportPortal/FS/ZMFGOperator/${site}/form-schema`);
    const { ZMFGOperatorColumns, ZMFGOperatorHColumns } = require(`@/pages/ReportPortal/FS/ZMFGOperator/${site}/Table`);
    return { schemas, ZMFGOperatorColumns, ZMFGOperatorHColumns };
}
let { schemas, ZMFGOperatorColumns, ZMFGOperatorHColumns } = getImport(FactoryType);

let PageSetting = reactive({
    List: [] as any,
    Columns: [] as any,
    Title: '',
    moreData: [],
    SQLHidden: '',
    ChartData: [] as any,
    ChartXData: [] as any
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
    labelWidth: 120,
    schemas,
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
});

async function confirm(values) {
    openFullLoading();
    let baseParams = {
        apiName: 'ZMFGOperator',
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
        let Data = api01['Data']['Table'];
        let Data1 = Data.filter(item=>item['QITEM']!='SUM')
        if (Data.length == 0) {
            PageSetting.moreData = [];
            closeFullLoading();
            return message.warn('无数据!');
        }
        let ChartData = Data1.map((item) => item.INSP_CNT);
        let LineData = Data1.map((item) => item.QPAN);
        PageSetting.ChartXData = Data1.map((item) => item.QITEM);

        PageSetting.moreData = Data;
        PageSetting.Columns = [...ZMFGOperatorHColumns[values['QType']], ...ZMFGOperatorColumns];
        PageSetting.ChartData = [
            { name: '检验数量', data: ChartData, type: 'bar', barWidth: 20, yAxisIndex: 0 },
            { name: '判退数量', data: LineData, type: 'line', yAxisIndex: 1 }
        ];
        UseParamsAdd.AddFormParams('ZMFGOperatorLCM1',values);
        UseParamsAdd.AddAPIResultMessage('ZMFGOperatorLCM1',api01['Message']);
        closeFullLoading();
    } catch (error) {
        console.log('error' + error);

        closeFullLoading();
    }
}
let YDefine = [
    {
        type: 'value',
        min: 0,
        name: '检验数量'
        // max: 1500
        // interval: 500
    },
    {
        type: 'value',
        min:0,
        name: '判退数量',
        // max: 250
        interval: 1
    }
];
onMounted(async () => {});
</script>

<style lang="less" scoped>
.Report {
    overflow: auto;
}
</style>
