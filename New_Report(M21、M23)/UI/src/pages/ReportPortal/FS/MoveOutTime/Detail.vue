<script lang="ts" setup>
import { useTable } from '@/components/core/dynamic-table';
import { reactive, watchEffect, watch, computed } from 'vue';
import { GetCommon } from '@/api/pages/common';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { useVirtualList } from '@vueuse/core';

const UseParamsAdd = AddCommonPageParams();

function getImport(site: any) {
    const { MoveOutTimeDetailColumns } = require(`@/pages/ReportPortal/FS/MoveOutTime/${site}/Table`);
    return { MoveOutTimeDetailColumns };
}
let { MoveOutTimeDetailColumns } = getImport(UseParamsAdd.Fab);

let useCommonParams = AddCommonPageParams();
let TableParams = useCommonParams.FirstTableParams;
let PageSetting = reactive({
    moreData: [],
    List: [] as any,
    Columns: MoveOutTimeDetailColumns,
    TopTitle: ''
    // BottomTitle:''
});
const [DynamicTable] = useTable();

async function GetDetailData(TableParams: any) {
    // let APIParams: any = useCommonParams.APIMessage
    let formParams = useCommonParams.FormParams;

    let { api01 } = await GetCommon({
        ...formParams,
        ReportType: 'MoveOutTimeDataSetDetail',
        FromDate:TableParams['TRANS_DATE1'],
        ToDate:TableParams['TRANS_DATE1'],
        Equipment:TableParams.EQUIP_NBR,
        Product:TableParams.PROD_NBR
    });
    let pushdata = api01['Data0']['Table'];
    PageSetting.moreData = pushdata;
    // console.log(pushdata);
    PageSetting.moreData.forEach((item: object) => {
        item['TRANS_DATE1'] = item['TRANS_DATE'].split(' ')[0];
        item['Time'] = item['TRANS_DATE'].split(' ')[1];
    });
    switch(UseParamsAdd.Fab){
        case "LCM1":
            PageSetting.TopTitle = 'LCMA Move Out Time Detail ';
            break;
        case "LCM2":
            PageSetting.TopTitle = 'LCMB Move Out Time Detail ';
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
            <div v-bind="containerProps" style="height: 600px">
                <DynamicTable
                    v-bind="wrapperProps"
                    sticky
                    row-key="id"
                    :header-title="PageSetting.TopTitle"
                    :columns="PageSetting.Columns"
                    :key="PageSetting.TopTitle"
                    bordered
                    size="small"
                    :data-source="PageSetting.List"
                    :pagination="false"
                    export-file-name="导出"
                >
                    <template #export-button>
                        <a-button type="success"> 导出 </a-button>
                    </template>
                </DynamicTable>
            </div>
        </a-card>
    </div>
</template>
