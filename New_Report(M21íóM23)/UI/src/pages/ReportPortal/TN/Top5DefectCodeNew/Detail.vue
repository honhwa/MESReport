<template>
    <div>
        <a-card>
            <DynamicTable
                sticky
                row-key="id"
                :header-title="PageSetting.TopTitle"
                :columns="PageSetting.Columns"
                :key="PageSetting.Columns"
                bordered
                size="small"
                :data-source="PageSetting.List"
                :pagination="false"
                :scroll="{ y: 100 }"
                export-file-name="Export"
            >
                <template #export-button> <a-button type="success"> Export </a-button> </template>
            </DynamicTable>
        <a-card>
        </a-card>
            <DynamicTable
                sticky
                row-key="id"
                :header-title="PageSetting.BottomTitle"
                :columns="PageSetting.SecondColumns"
                :key="PageSetting.SecondColumns"
                bordered
                size="small"
                :data-source="PageSetting.SecondList"
                :pagination="false"
                :scroll="{ y: 280 }"
                export-file-name="Export"
            >
                <template #export-button> <a-button type="success"> Export </a-button> </template>
            </DynamicTable>
        </a-card>
    </div>
</template>

<script lang="ts" setup>
import { useTable } from '@/components/core/dynamic-table';
import { reactive, watchEffect, watch, ref } from 'vue';
import { GetCommon } from '@/api/pages/common';
import { ReportCommon } from '@/store/module/ReportCommon';
import { useLoading } from '@/components/feat/Loading';

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});

function getImport(site: any) {
    const { detailColumns, SecondColumns } = require(`@/pages/ReportPortal/TN/Top5DefectCodeNew/${site}/Table`);
    return { detailColumns, SecondColumns };
}
let { detailColumns, SecondColumns } = getImport('MOD');

let useCommonParams = ReportCommon();
let formParams = useCommonParams['Top5DefectCodeNew' + useCommonParams.Fab].FormParams;
let tableParams = useCommonParams['Top5DefectCodeNew' + useCommonParams.Fab].FirstTableParams;
let tableDetailParams = useCommonParams['Top5DefectCodeNew' + useCommonParams.Fab].SecondTableParams;

let PageSetting = reactive({
    moreData: [],
    List: [] as any,
    APIColumns: [] as any,
    Columns: detailColumns,
    TopTitle: 'Detail',
    BottomTitle: '',
    SecondList: [] as any,
    SecondColumns: SecondColumns
});
const [DynamicTable] = useTable();

async function GetDetailData(formParams: any, TableParams: any) {
    let baseParams = {
        apiName: 'TN_Top5DefectCodeNewDetail',
        FactoryType: useCommonParams.Fab
    };
    let DetailParams: any = {};
    let SelDate = TableParams.dataIndex || '';
    DetailParams.SelDate = SelDate;
    DetailParams.ErrCode = TableParams[SelDate + '-REMARK'] || '';
    DetailParams.SEQ = TableParams[SelDate + '-SEQ'] || '0';
    DetailParams.Kpi = TableParams.KPI || '';

    openFullLoading();
    
    let { api01 } = await GetCommon({ ...baseParams, ...formParams, ...DetailParams });
    let pushdata = api01['Data']['Table'];
    pushdata.forEach((detail) => {
        detail.DEFT_RATE = Math.round(detail.DEFT_RATE * 1000) / 1000 + '%';
    });
    PageSetting.TopTitle = 'Detail of ' + DetailParams.Kpi + ' ; ' + DetailParams.SelDate + ' ; ' + DetailParams.ErrCode + ' ;';
    PageSetting.moreData = pushdata;
    PageSetting.List = PageSetting.moreData || [];
    
    closeFullLoading();
}

async function GetSecondDetailData(formParams, tableParams, tableDetailParams) {
    let baseParams = {
        apiName: 'TN_Top5DefectCodeNewDetail2',
        FactoryType: useCommonParams.Fab
    };
    let DetailParams: any = {};
    let SelDate = tableParams.dataIndex || '';
    DetailParams.SelDate = SelDate;
    DetailParams.ErrCode = tableParams[SelDate + '-REMARK'] || '';
    DetailParams.SEQ = tableParams[SelDate + '-SEQ'] || '0';
    DetailParams.Kpi = tableParams.KPI || '';
    DetailParams.Prod = tableDetailParams.PROD_NBR || '';

    openFullLoading();

    let { api01 } = await GetCommon({ ...baseParams, ...formParams, ...DetailParams });
    let pushdata = api01['Data']['Table'];
    PageSetting.BottomTitle = PageSetting.TopTitle + tableDetailParams.PROD_NBR;
    PageSetting.SecondList = pushdata || [];

    closeFullLoading();

}

watchEffect(() => {
    //一阶表格监控
    formParams = useCommonParams['Top5DefectCodeNew' + useCommonParams.Fab]['FormParams'];
    tableParams = useCommonParams['Top5DefectCodeNew' + useCommonParams.Fab]['FirstTableParams'];
    GetDetailData(formParams, tableParams);
});
watch(
    //二阶表格监控
    () => useCommonParams['Top5DefectCodeNew' + useCommonParams.Fab].SecondTableParams,
    () => {
        formParams = useCommonParams['Top5DefectCodeNew' + useCommonParams.Fab]['FormParams'];
        tableParams = useCommonParams['Top5DefectCodeNew' + useCommonParams.Fab].FirstTableParams;
        tableDetailParams = useCommonParams['Top5DefectCodeNew' + useCommonParams.Fab].SecondTableParams;
        GetSecondDetailData(formParams, tableParams, tableDetailParams);
    }
);


</script>
