<script lang="ts" setup>
import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table';
import { getRowClassNmae, mergeTableRow } from '@/pages/ReportPortal/utils/common';
// import { schemas } from './form-schema';
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, onMounted, ref, unref } from 'vue';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import { GetCommon } from '@/api/pages/common';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { useLoading } from '@/components/feat/Loading';

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});

// import { useInfiniteScroll, useVirtualList } from '@vueuse/core'
const UseParamsAdd = AddCommonPageParams();
const OQCInspectionQuantityControl = ref<HTMLElement | null>(null);
function getImport(site: any) {
    // let { optionsdata } = require('@/pages/ReportPortal/FS/PanelWipStayChart/LCM1/form-schema');
    const { schemas } = require(`@/pages/ReportPortal/FS/OQCInspectionQuantityControl/${site}/form-schema`);
    const { OQCInspectionColumns } = require(`@/pages/ReportPortal/FS/OQCInspectionQuantityControl/${site}/Table.ts`);
    return { schemas, OQCInspectionColumns };
}
let { schemas,  OQCInspectionColumns } = getImport(UseParamsAdd.Fab);
let Fab2Data = getImport('LCM2');
let PageSetting = reactive<BasePageParams.BasePageSetting>({
    List: [],
    APIColumns: [],
    Columns: [],
    moreData: []
});
const [DynamicTable] = useTable();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas:schemas(UseParamsAdd.Fab),
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
});

async function confirm(values) {
    openFullLoading();
    PageSetting.Columns = [];
    // let dynamicColumns: TableColumn[] = []
    let baseParams = {
        apiName: 'OQCInspectionQuantityControl',
        FactoryType: UseParamsAdd.Fab,
        G_FAC: UseParamsAdd.G_FAC_COM,
        ReportType: ''
    };

    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        }
        if (key == 'InspectionSite' && values[key] != '') {
            values['InspectionSiteData'] = values[key].substring(1, values[key].length) ?? '';

            values[key] = values[key].substring(0, 1) ?? '';
        }
    });
    values['FromDate'] = values['CompleteFromDate'].split(' ')[0] + ' ';
    values['FromTime'] = values['CompleteFromDate'].split(' ')[1];
    values['ToDate'] = values['CompleteToDate'].split(' ')[0] + ' ';
    values['ToTime'] = values['CompleteToDate'].split(' ')[1];

    try {
        let { api01 } = await GetCommon({ ...values, ...baseParams });
        PageSetting.Title = 'OQC Inspection Quantity Control';
        Handledata(api01['Data0']['Table'],values['InspectionSite']);
        UseParamsAdd.AddFormParams({ ...baseParams, ...values });
        UseParamsAdd.AddAPIResultMessage(api01['Message']);
        if (UseParamsAdd.Fab == 'LCM1' && values['InspectionSite'] == 'B') {
            PageSetting.Columns = Fab2Data.OQCInspectionColumns;
        } else {
            PageSetting.Columns = OQCInspectionColumns;
        }
        closeFullLoading();
    } catch (error) {
        console.log(error);

        closeFullLoading();
    }
}
//实现无限滚动或者说懒加载
// onMounted(async () => {
//     //获取Size,bigproduct,prodcut数据
//     if (UseParamsAdd.Fab != 'BLU2') {
//         optionsdata = await mounted(optionsdata, UseParamsAdd.Fab);
//     } else {
//         // optionsdata =
//         await GetBLU2SizeData();
//     }
// });
function Handledata(Data: any[], params: any) {
    Data.forEach((item) => {
        item['PASS_QTY'] = isNaN(parseInt(item['PASS_QTY'])) ? 0 : parseInt(item['PASS_QTY']);
        item['INSPECT_QTY'] = isNaN(parseInt(item['INSPECT_QTY'])) ? 0 : parseInt(item['INSPECT_QTY']);
        item['NG_QTY'] = isNaN(parseInt(item['NG_QTY'])) ? 0 : parseInt(item['NG_QTY']);
        item['CHECK_CNT'] = isNaN(parseInt(item['CHECK_CNT'])) ? 0 : parseInt(item['CHECK_CNT']);
        if (item['INSP_FREQUENCY'] && item['INSP_FREQUENCY'] != '0' && item['INSP_FREQUENCY'] != '/') {
            let data = item['INSP_FREQUENCY'].replace('抽1', '');
            item['CHECK_CNT'] = Math.round(item['PASS_QTY'] / data + 0.499);
        }
        item['DIFF_CNT'] = item['INSPECT_QTY'] - item['CHECK_CNT'];
        item['DPPM'] = item['INSPECT_QTY'] == 0 ? 0 : Math.round((item['NG_QTY'] * 1000000) / item['INSPECT_QTY']);
    });
    let mergeColNames: string[] = [];
    if ((UseParamsAdd.Fab == 'LCM1' && params == 'B') || UseParamsAdd.Fab != 'LCM1') {
        mergeColNames = ['WORK_CTR', 'FLOOR'];
    } else {
        mergeColNames = ['WORK_CTR', 'LINE_NO'];
    }
    let combineCol1: any = {
        data: Data,
        mergeColNames,
        firstMergeColNames: ['WORK_CTR'],
        firstMerge: 'WORK_CTR'
    };
    /**进行表格合并 */
    let Mergedata = mergeTableRow(combineCol1);
    PageSetting.moreData = Mergedata;
}
// async function GetBLU2SizeData() {
//     let { api01 } = await GetCommon({
//         apiName: 'QCInspectionQuanConSize',
//         FactoryType: UseParamsAdd.Fab,
//         G_FAC: UseParamsAdd.G_FAC_COM
//     });
//     optionsdata['SizeData'] = api01['Data0']['Table'].map((item) => {
//         return { label: item['SIZE1'], value: item['SIZE1'] };
//     });
//     optionsdata['BigProductData1'] = optionsdata['BigProductData'] = api01['Data1']['Table'].map((item) => {
//         return { label: item['PART'], value: item['PART'] };
//     });
//     optionsdata['ProductData1'] = optionsdata['ProductData1'] = api01['Data2']['Table'].map((item) => {
//         return { label: item['SOPTION'], value: item['SOPTION'] };
//     });
// }
</script>

<template>
    <div class="OQCInspectionQuantityControl">
        <a-row>
            <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                <SchemaForm @submit="confirm"> </SchemaForm>
            </a-col>
            <a-col :span="PageParams.TableSpan">
                <div ref="OQCInspectionQuantityControl">
                    <DynamicTable
                        showFavorite
                        sticky
                        :PageParams="PageParams"
                        row-key="OQCInspectionQuantityControl"
                        :header-title="PageSetting.Title"
                        :columns="PageSetting.Columns"
                        :key="PageSetting.Columns"
                        bordered
                        size="small"
                        :data-source="PageSetting.moreData"
                        :data-all="PageSetting.moreData"
                        :pagination="false"
                        :export-file-name="PageSetting.Title"
                        :row-class-name="getRowClassNmae"
                        :flexwidth="1"
                    >
                        <template #export-button>
                            <a-button type="success"> 导出 </a-button>
                        </template>
                    </DynamicTable>
                </div>
            </a-col>
        </a-row>
    </div>
</template>
