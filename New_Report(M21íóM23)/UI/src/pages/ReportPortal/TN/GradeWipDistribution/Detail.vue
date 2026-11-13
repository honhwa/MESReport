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
    const { detailColumns } = require(`@/pages/ReportPortal/TN/GradeWipDistribution/${site}/Table`);
    return { detailColumns };
}
let { detailColumns } = getImport('MOD');

let useCommonParams = ReportCommon();
let TableParams = useCommonParams['GradeWipDistribution' + useCommonParams.Fab]['FirstTableParams'];

let PageSetting = reactive({
    moreData: [],
    List: [] as any,
    Columns: detailColumns,
    TopTitle: 'Grade Wip Distribution Detail'
});
const [DynamicTable] = useTable();

async function GetDetailData(TableParams: any) {
    let formParams = useCommonParams['GradeWipDistribution' + useCommonParams.Fab]['FirstTableParams'];
    let baseParams = {
        apiName: 'TN_GradeWipDistributionDetail',
        FactoryType: useCommonParams.Fab,
        Current_Date: formParams['Date']
    };
    let dataIndex: string = TableParams['dataIndex'];
    let holdFlag = dataIndex.split('').reverse()[0];
    let work_ctr = '';
    if (holdFlag == 'Y' || holdFlag == 'N') {
        work_ctr = dataIndex.replace(/_[NY]/, '');
        work_ctr = work_ctr == '小計' ? '' : work_ctr;
    } else {
        holdFlag = '';
    }
    let ClickParams = {
        work_ctr: work_ctr,
        prod_nbr: TableParams['PROD_NBR'],
        owner_code: TableParams['LCM_GRADE'],
        hold_flag: holdFlag
    };
    let { api01 } = await GetCommon({ ...baseParams, ...ClickParams });
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
    TableParams = useCommonParams['GradeWipDistribution' + useCommonParams.Fab]['FirstTableParams'];

    GetDetailData(TableParams);
});
</script>
