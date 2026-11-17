<template>
    <div>
        <a-card  >
           <div class="ScrollTable">
            <DynamicTable row-key="id"  :header-title="PageSetting.FirstTitle" :columns="PageSetting.FirstColumns"
                :key="PageSetting.FirstColumns" bordered size="small" :data-source="PageSetting.FirstList"
                sticky  :explain="false" 
                :OwnerFoldParams="false" 
                :pagination="false" :export-file-name="PageSetting.FirstTitle" :scroll="{y: PageSetting.y}"
                :row-class-name="getRowClassName">
                <template #export-button>
                    <a-button type="success"> 导出 </a-button>
                </template>
            </DynamicTable>
         
            <DynamicTable row-key="id" :header-title="PageSetting.SecondTitle" :columns="PageSetting.SecondColumns"
            sticky :flexwidth="-5" :explain="false"
                :OwnerFoldParams="false"
                :key="PageSetting.SecondColumns" bordered size="small" :data-source="PageSetting.SecondList"
                :pagination="false" :export-file-name="PageSetting.SecondTitle" :scroll="{ y: 400 }"
                :row-class-name="getRowClassName">
                <template #export-button>
                    <a-button type="success"> 导出 </a-button>
                </template>
            </DynamicTable>
        </div>            
            <div hidden>{{ PageSetting.SQLHidden}}</div>
            <div hidden>{{ PageSetting.SQLHidden2}}</div>
        </a-card>
    </div>
</template>


<script lang="ts" setup>
import { useTable } from '@/components/core/dynamic-table';
import { reactive, ref, watchEffect, watch, computed  } from 'vue';
import { GetCommon } from '@/api/pages/common';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { getRowClassName, getfinalsum, Inserttotalrow, dclick } from '@/pages/ReportPortal/utils/SHcommon';
import { cloneDeep } from 'lodash';
import { message } from 'ant-design-vue';
import { useLoading } from '@/components/feat/Loading';
import { ReportCommon } from '@/store/module/ReportCommon';
import { useVirtualList } from '@vueuse/core'


function getImport() {    
    const { getDetail,getDetail2} = require(`@/pages/ReportPortal/SH/Top5DefectCode/${bigfab}/RenderFunction`);
    //const { Top5DetailColumns, } = require(`@/pages/ReportPortal/SH/Top5DefectCode/${bigfab}/table`);
    return {getDetail,getDetail2};
}

let useCommonParams = AddCommonPageParams();
let useReportCommon = ReportCommon();
let bigfab=useCommonParams.Fab.slice(0,useCommonParams.Fab.length-1);
let ReportNameParams=`Top5DefectCode-${useCommonParams.Fab}`;


let {getDetail,getDetail2} = getImport();
const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});

let tableParams = useReportCommon[ReportNameParams].FirstTableParams;
let formParams= useReportCommon[ReportNameParams].FormParams;
let APIMessage=JSON.parse(useReportCommon[ReportNameParams].APIMessage);

let PageSetting = reactive({
    moreData: [] as any,
    FirstList: [] as any,
    SecondList: [] as any,
    
    FirstColumns: [] as any,
    SecondColumns: [] as any,
   
    FirstTitle: `${useCommonParams.Fab} Top5DefectCode Report`,
    SecondTitle: `${useCommonParams.Fab} Top5DefectCode Report`,
    SQLHidden:'',
    SQLHidden2:'',
    y: '' as any,
    x: '' as any,
});
const [DynamicTable] = useTable();
let onMoreDataLoad = computed(() => {
    if (PageSetting.moreData.length > 0) return PageSetting.moreData
    else return []
})
// const { list, containerProps, wrapperProps } = useVirtualList(
//     onMoreDataLoad,
//     {
//         itemHeight: 23,
//     },
// )
watch(
    //二阶表格监控
    () => useReportCommon[ReportNameParams].SecondTableParams,
    (newValue) => {
        //console.log("1234",useReportCommon[ReportNameParams].SecondTableParams)
        getDetail2(newValue);
    },
    {
        deep: true
    }
);
watchEffect(() => {
    //一阶表格监控
    //一阶表格监控   
    tableParams = useReportCommon[ReportNameParams].FirstTableParams;
    formParams= useReportCommon[ReportNameParams].FormParams; 
    getDetail(tableParams,APIMessage,formParams,PageSetting); 
});
</script>



    