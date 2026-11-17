<script lang="ts" setup>
import { useTable } from '@/components/core/dynamic-table';
import { reactive, watchEffect, watch, computed } from 'vue';
import { GetCommon } from '@/api/pages/common';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { useVirtualList } from '@vueuse/core';

const UseParamsAdd = AddCommonPageParams();

function getImport(site: any) {
    const {
        LCDProps,
        LcdPanelAtLcmDetailColumns
    } = require(`@/pages/ReportPortal/FS/LCDPanelAtLCM/${site}/Table`);
    return { LCDProps, LcdPanelAtLcmDetailColumns };
}
let { LCDProps, LcdPanelAtLcmDetailColumns } = getImport(UseParamsAdd.Fab);

let useCommonParams = AddCommonPageParams();
let TableParams = useCommonParams.FirstTableParams;
// let formParams =
let PageSetting = reactive({
    moreData: [],
    List: [] as any,
    Columns: LcdPanelAtLcmDetailColumns,
    TopTitle: ''
    // BottomTitle:''
});
const [DynamicTable] = useTable();

async function GetDetailData(TableParams: any) {
    // let APIParams: any = useCommonParams.APIMessage
    let formParams = useCommonParams.FormParams;
    let IndexTableParams = {};
    LCDProps.forEach((item: string) => {
        IndexTableParams[item.toLowerCase()] = TableParams[item];
    });
    let Detailparams = { acct_date: TableParams['dataIndex'] };
    let { api01 } = await GetCommon({
        ...formParams,
        ReportType: 'LCDSTPNDetail',
        ...IndexTableParams,
        ...Detailparams
    });
    let pushdata = api01['Data0']['Table'];
    pushdata.push({ PROD_NBR: '合計', LCD_PROD_NBR: TableParams[TableParams.dataIndex] });
    PageSetting.moreData = pushdata;
    switch(UseParamsAdd.Fab){
        case "LCM1":
            PageSetting.TopTitle = 'LCMA STPN Panel Detail ';
            break;
        case "LCM2":
            PageSetting.TopTitle = 'LCMB STPN Panel Detail ';
            break;
    }
    
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
    TableParams = useCommonParams.FirstTableParams;

    GetDetailData(TableParams);
});
</script>

<template>
    <div>
        <a-card>
            <!-- <div v-bind="containerProps" style="height: 600px"> -->
            <div  style="height: 600px">
                <DynamicTable
                    v-bind="wrapperProps"
                    sticky
                    row-key="id"
                    :header-title="PageSetting.TopTitle"
                    :columns="PageSetting.Columns"
                    :key="PageSetting.TopTitle"
                    bordered
                    size="small"
                    :data-source="PageSetting.moreData"
                    :data-all="PageSetting.moreData"
                    :pagination="false"
                    export-file-name="导出"
                    :flexwidth="1"
                >
                    <template #export-button>
                        <a-button type="success"> 导出 </a-button>
                    </template>
                </DynamicTable>
            </div>
        </a-card>
    </div>
</template>
