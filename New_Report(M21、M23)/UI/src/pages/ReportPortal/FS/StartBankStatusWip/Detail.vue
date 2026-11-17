<script lang="ts" setup>
import { useTable } from '@/components/core/dynamic-table';
import { reactive, watchEffect, watch, computed } from 'vue';
import { GetCommon } from '@/api/pages/common';
import { ReportCommonWithOut } from '@/store/module/ReportCommon';
import { useVirtualList } from '@vueuse/core';
import { message } from 'ant-design-vue';
import { getRowClassNmae } from '@/pages/ReportPortal/utils/common'



let useCommonParams = ReportCommonWithOut();
let FactoryType  = useCommonParams.Fab;
let TableParams = useCommonParams["StartBankStatus"+FactoryType]["FirstTableParams"];
let PageSetting = reactive({
    moreData: [],
    List: [] as any,
    Columns: [] as any,
    TopTitle: 'LCMA Start Bank Status Wip Detail'
});
const [DynamicTable] = useTable();

async function GetDetailData(TableParams: any) {
    PageSetting.Columns = TableParams.Columns;

    const { Date: { Data: { Table: Data } } } = await GetCommon(TableParams.apiParams);
    if(Data.length == 0){
        return message.warn("無數據！")
    }
    
  
    PageSetting.moreData = Data;
    
    
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
    TableParams = useCommonParams["StartBankStatus"+FactoryType]["FirstTableParams"];
    console.log("輸出二階TableParams",TableParams);
    
    GetDetailData(TableParams);
});
</script>

<template>
    <div>
        <a-card>
            <div v-bind="containerProps" style="height: 600px">
                <DynamicTable
                    v-bind="wrapperProps"
                    row-key="id"
                    :header-title="PageSetting.TopTitle"
                    :columns="PageSetting.Columns"
                    :key="PageSetting.TopTitle"
                    bordered
                    size="small"
                    :data-source="PageSetting.List"
                    :data-all="PageSetting.List"
                    :pagination="false"
                    export-file-name="导出"
                    :row-class-name="getRowClassNmae"
                    :flexwidth="1"
                    sticky
                >
                    <template #export-button>
                        <a-button type="success"> 导出 </a-button>
                    </template>
                </DynamicTable>
            </div>
        </a-card>
    </div>
</template>
