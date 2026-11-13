<template>
    <div>
        <a-card>
            <!-- <div v-bind="containerProps" style="height: 600px"> -->
                <DynamicTable v-bind="wrapperProps" row-key="id" :header-title="PageSetting.Title" sticky
                    :columns="PageSetting.Columns"  :key="PageSetting.Columns" bordered size="small"
                    :data-source="PageSetting.List" 
                     :pagination="{
                                pageSizeOptions:['10','50','100','200'],
                                pageSize:10                            
                            }" 
                    export-file-name="导出"
                    :scroll="{ x: PageSetting.x,}"   :data-all="PageSetting.moreData"  :explain="false"
                    :OwnerParams="false"
                    :row-class-name="getRowClassName" 
                    >
                    <template #export-button>
                        <a-button type="success"> 导出 </a-button>
                    </template>
                </DynamicTable>
            <!-- </div> -->
            <div hidden>{{ PageSetting.SQLHidden }}</div>
        </a-card>
    </div>
</template>



<script lang="ts" setup>
import { useTable } from '@/components/core/dynamic-table';
import { reactive, watchEffect, watch, computed } from 'vue';
import { GetCommon } from '@/api/pages/common';
//import { useVirtualList } from '@vueuse/core';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { useLoading } from '@/components/feat/Loading';
import { Console } from 'console';

//import {getWipPanelSummaryDetail} from './LCM/RenderFunction';
import { ReportCommon } from '@/store/module/ReportCommon';

function getImport() {
    // const DetailColumsData = require(`@/pages/ReportPortal/SH/WipPanelSummary/${bigfab}/Table`);
    const {getWipPanelSummaryDetail} = require(`@/pages/ReportPortal/SH/MOD/WipPanelSummaryNew/${bigfab}/RenderFunction`);
    //return DetailColumsData
     return {getWipPanelSummaryDetail};
}

let useCommonParams = AddCommonPageParams();
let useReportCommon = ReportCommon();
let bigfab=useCommonParams.Fab.slice(0,useCommonParams.Fab.length-1);
let ReportNameParams=`WipPanelSummaryNew-${useCommonParams.Fab}`;

let {getWipPanelSummaryDetail} = getImport();

let tableParams = useReportCommon[ReportNameParams].FirstTableParams;
let formParams= useReportCommon[ReportNameParams].FormParams;
let APIMessage=JSON.parse(useReportCommon[ReportNameParams].APIMessage);
const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});

// let formParams = 
let PageSetting = reactive({
    moreData: [],
    List: [] as any,
    Columns: [] as any,
    Title: '',
    SQLHidden:'',
    x: '' as any,
});
const [DynamicTable] = useTable();

// let onMoreDataLoad = computed(() => {
//     if (PageSetting.moreData.length > 0) return PageSetting.moreData
//     else return []
// })
// const { list, containerProps, wrapperProps } = useVirtualList(
//     onMoreDataLoad,
//     {
//         itemHeight: 22,
//     },
// )
// watch(() => list.value,
//     () => {
       
//         let pushData: any[] = []
//         PageSetting.List = []
//         list.value.forEach(item => {
//             if (item.index < PageSetting.moreData.length) {
//                 pushData.push(item.data)
//             }
//         })
//         PageSetting.List = pushData
//     })
watchEffect(() => {
     //一阶表格监控
    PageSetting['List'] = [];
    PageSetting['Columns'] = [];
    PageSetting['Title'] = `${useCommonParams.Fab} Wip Panel Detail (Real-Time Refresh)`
    tableParams = useReportCommon[ReportNameParams].FirstTableParams; 
    getWipPanelSummaryDetail(tableParams, formParams,APIMessage,PageSetting); 
    
});

 


</script>


