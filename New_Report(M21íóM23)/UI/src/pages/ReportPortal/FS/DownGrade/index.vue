<script lang="ts" setup>
import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table'
import { getRowClassNmae, mergeTableRow } from '@/pages/ReportPortal/utils/common';
// import { schemas } from './form-schema';
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, onMounted, ref, unref } from 'vue';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import { GetCommon } from '@/api/pages/common';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { useLoading } from '@/components/feat/Loading';
import { GetLCMProductSizeData, GetLcdParamsData } from '@/pages/ReportPortal/utils/useFormParams';

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});

// import { useInfiniteScroll, useVirtualList } from '@vueuse/core'
const { mounted, InfiniteScroll, mountedLCDGrade, mountedLcmProdType } = useRemoteSelect();
const UseParamsAdd = AddCommonPageParams();
const DownGrade = ref<HTMLElement | null>(null);
function getImport(site: any) {
    // let { optionsdata } = require('@/pages/ReportPortal/FS/PanelWipStayChart/LCM1/form-schema');
    const { schemas, optionsdata2,optionsdata } = require(`@/pages/ReportPortal/FS/DownGrade/${site}/form-schema`);
    const {
        DownGradeColumns: DownGradeColumnsLCM1,
        DownGradeProdColumns: DownGradeProdColumnsLCM1,
        DownGradeColumnsLCM2
    } = require(`@/pages/ReportPortal/FS/DownGrade/${site}/Table`);
    return { schemas, optionsdata, DownGradeColumnsLCM1, DownGradeProdColumnsLCM1, DownGradeColumnsLCM2, optionsdata2 };
}
let { schemas, optionsdata, DownGradeColumnsLCM1, DownGradeProdColumnsLCM1, DownGradeColumnsLCM2, optionsdata2 } = getImport(UseParamsAdd.Fab);
let FactoryType: string = UseParamsAdd.Fab;
let DownGradeColumns;
let DownGradeProdColumns;
switch (FactoryType) {
    case "LCM1":
        DownGradeColumns = DownGradeColumnsLCM1;
        DownGradeProdColumns = DownGradeProdColumnsLCM1;
        break;
    case "LCM2":
        DownGradeColumns = DownGradeColumnsLCM2;
        DownGradeProdColumns = DownGradeProdColumnsLCM1;
    default:
        break;
}
let PageSetting = reactive<BasePageParams.BasePageSetting>({
    List: [],
    APIColumns: [],
    Columns: [],
    moreData: [],
    x:null,
});
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
    PageSetting.Columns = [];
    // let dynamicColumns: TableColumn[] = []
    let baseParams = {
        apiName: 'DownGrade',
        FactoryType,
        G_FAC: UseParamsAdd.G_FAC_COM,
        ReportType: values['ShowMode'] == 'Operator1' ? 'DownGradeDeta_prod' : ''
    };
    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        }
    });
    try {
        let { api01 } = await GetCommon({ ...values, ...baseParams });
        // // let children: any[] = [];
        PageSetting.Title = 'LCMA Down Grade Summary';
        Handledata(api01['Data0']['Table'], values['ShowMode']);
        UseParamsAdd.AddFormParams({ ...baseParams, ...values });
        UseParamsAdd.AddAPIResultMessage(api01['Message']);
        closeFullLoading();
    } catch {
        closeFullLoading();
    }
}
//实现无限滚动或者说懒加载
InfiniteScroll(DownGrade, PageSetting);
onMounted(async () => {
    //获取Size,bigproduct,prodcut数据
    optionsdata = await mounted(optionsdata, UseParamsAdd.Fab);
    await mountedLCDGrade(optionsdata2, UseParamsAdd.Fab);
    await mountedLcmProdType(optionsdata2, UseParamsAdd.Fab)

    await GetLcdParamsData(optionsdata);
    optionsdata.LCMProductType = await GetLCMProductSizeData('LCMProductType');
    optionsdata.LCMProductSize = await GetLCMProductSizeData('LCMProductSize_TBSR');
    optionsdata2["OperationOption"] = optionsdata2[`OperationOption` + UseParamsAdd.Fab]
});
function Handledata(Data: any[], ShowMode: string) {
    let ColumnsData = {
        Operator: [{ title: '人員工號', dataIndex: '人員工號', align: 'center',fixed:'left' }, ...DownGradeColumns],
        equip_nbr: [{ title: '站别', dataIndex: '站别', align: 'center',fixed:'left' }, ...DownGradeColumns],
        Prod_nbr: [{ title: 'Product', dataIndex: 'Product', align: 'center',fixed:'left' }, ...DownGradeColumns],
        Operator1: DownGradeProdColumns
    };
    if (ShowMode !== 'Operator1') PageSetting.moreData = Data;
    else {
        let combineCol2 = {
            data: Data,
            mergeColNames: ['Product'],
            firstMergeColNames: ['Product'],
            firstMerge: 'Product'
        };
        PageSetting.moreData = mergeTableRow(combineCol2);
    }
    PageSetting.Columns = ColumnsData[ShowMode];

    if (PageSetting.moreData.length > 40) PageSetting.List = PageSetting.moreData.slice(0, 40);
    else PageSetting.List = PageSetting.moreData;
    PageSetting.x = 2300
}
</script>

<template>
    <div>
        <a-row>
            <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                <SchemaForm @submit="confirm" :showTextPosition="'center'"> </SchemaForm>
            </a-col>
            <a-col :span="PageParams.TableSpan">
                <div ref="DownGrade">
                    <DynamicTable
                        showFavorite
                        :PageParams="PageParams"
                        row-key="DownGrade"
                        :header-title="PageSetting.Title"
                        sticky
                        :columns="PageSetting.Columns"
                        :key="PageSetting.Columns"
                        bordered
                        size="small"
                        :data-source="PageSetting.moreData"
                        :export-file-name="PageSetting.Title"
                        :data-all="PageSetting.moreData"
                        :row-class-name="getRowClassNmae"
                        :pagination="false"
                        :flexwidth="1"
                        :id="PageSetting.Title"
                        export-type="Table"
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
