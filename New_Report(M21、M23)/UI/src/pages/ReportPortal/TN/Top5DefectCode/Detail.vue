<template>
    <div>
        <a-card>
            <div v-bind="containerProps" style="height: 600px">
                <DynamicTable
                    v-bind="wrapperProps"
                    row-key="id"
                    :header-title="PageSetting.TopTitle"
                    :columns="PageSetting.Columns"
                    :key="PageSetting.Columns"
                    bordered
                    size="small"
                    :data-source="PageSetting.List"
                    :pagination="false"
                    export-file-name="Export"
                >
                    <template #export-button>
                        <a-button type="success"> Export </a-button>
                    </template>
                </DynamicTable>
            </div>
        </a-card>
    </div>
</template>

<script lang="ts" setup>
import { useTable } from '@/components/core/dynamic-table';
import { reactive, watchEffect, watch, computed } from 'vue';
import { GetCommon } from '@/api/pages/common';
import { ReportCommon } from '@/store/module/ReportCommon';
import { useVirtualList } from '@vueuse/core';

function getImport(site: any) {
    const { detailColumns } = require(`@/pages/ReportPortal/TN/Top5DefectCode/${site}/Table`);
    return { detailColumns };
}
let { detailColumns } = getImport('MOD');

let useCommonParams = ReportCommon();
let TableParams = useCommonParams['Top5DefectCode' + useCommonParams.Fab]['FirstTableParams'];

let PageSetting = reactive({
    moreData: [],
    List: [] as any,
    Columns: detailColumns,
    TopTitle: 'Top5 Defect Code'
});
const [DynamicTable] = useTable();

async function GetDetailData(TableParams: any) {
    let formParams = useCommonParams['Top5DefectCode' + useCommonParams.Fab]['FormParams'];
    let baseParams = {
        apiName: 'TN_Top5DefectCodeDataSetDetail',
        FactoryType: useCommonParams.Fab
    };
    let DetailParams:any = {};
    let work_ctr = TableParams.dataIndex?.replace('-DFCT_CODE','');
    if(!TableParams[work_ctr].match(/[0-9]*\.[0-9]{3}\%\([0-9]*\)/)){
        DetailParams.ErrDescr = TableParams[work_ctr];
    }
    if(isNaN(TableParams.PROD)){
        DetailParams.AssmEqp = TableParams.PROD;
    }
    DetailParams.OperName = TableParams.title;

debugger
    let { api01 } = await GetCommon({ ...baseParams, ...formParams, ...DetailParams });
    let pushdata = api01['Data']['Table'];
    PageSetting.moreData = pushdata;
}

let onMoreDataLoad = computed(() => {
    if (PageSetting.moreData.length > 0) return PageSetting.moreData;
    else return [];
});
const { list, containerProps, wrapperProps } = useVirtualList(onMoreDataLoad, {
    itemHeight: 22
});
watch(
    () => list.value,
    () => {
        let pushData: any[] = [];
        PageSetting.List = [];
        list.value.forEach((item) => {
            if (item.index < PageSetting.moreData.length) {
                pushData.push(item.data);
            }
        });
        PageSetting.List = pushData;
    }
);
watchEffect(() => {
    //一阶表格监控
    TableParams = useCommonParams['Top5DefectCode' + useCommonParams.Fab]['FirstTableParams'];

    GetDetailData(TableParams);
});
</script>
