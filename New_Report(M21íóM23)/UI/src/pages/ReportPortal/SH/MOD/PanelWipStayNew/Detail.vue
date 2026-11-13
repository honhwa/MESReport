<template>
    <div>
        <a-card>
            <div v-bind="containerProps" style="height: 80vh">
                <DynamicTable sticky v-bind="wrapperProps" row-key="id" :header-title="PageSetting.Title"
                    :columns="PageSetting.Columns" :key="PageSetting.Columns" 
                     bordered size="small"
                     :scroll="{ x: PageSetting.x,}"
                    :data-all="PageSetting.moreData" :explain="false" :OwnerParams="false"
                    :data-source="PageSetting.List" :pagination="false" export-file-name="导出">
                    <template #export-button>
                        <a-button type="success"> 导出 </a-button>
                    </template>
                </DynamicTable>
            </div>
           <div hidden>{{ PageSetting.SQLHidden }}</div>

        </a-card>
    </div>
</template>

<script lang="ts" setup>
import { useTable } from '@/components/core/dynamic-table';
import { reactive, watchEffect, watch, computed } from 'vue';
import { GetCommon } from '@/api/pages/common';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { useVirtualList } from '@vueuse/core'
import { useLoading } from '@/components/feat/Loading';
import type { TableColumn } from '@/components/core/dynamic-table';
import { message } from 'ant-design-vue';
//import {getPanelWipStayDetail} from './LCM/RenderFunction';
import { ReportCommon } from '@/store/module/ReportCommon';


let useCommonParams = AddCommonPageParams();
let useReportCommon = ReportCommon();
let ReportNameParams=`PanelWipStayNew-${useCommonParams.Fab}`;
let bigfab=useCommonParams.Fab.slice(0,useCommonParams.Fab.length-1)

console.log(useCommonParams.Fab,"dffffffssss");
console.log(useCommonParams.G_FAC_COM,"dffffff");

function getImport(site: any) {
   
    const { PanelWipDetailColumns } = require(`@/pages/ReportPortal/SH/MOD/PanelWipStayNew/${bigfab}/Table`);
    const { getPanelWipStayNewDetail } = require(`@/pages/ReportPortal/SH/MOD/PanelWipStayNew/${bigfab}/RenderFunction`);
    return {getPanelWipStayNewDetail,PanelWipDetailColumns };
}



let tableParams = useReportCommon[ReportNameParams].FirstTableParams;
let APIMessage=JSON.parse(useReportCommon[ReportNameParams].APIMessage);
let formParams= useReportCommon[ReportNameParams].FormParams;

let { getPanelWipStayNewDetail,PanelWipDetailColumns } = getImport("SH");

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});

let PageSetting = reactive({
    List: [] as any,
    TableSpan: 20,
    showmenu: true,
    Columns: [] as any,
    moreData: [] as any,
    Title:'',
    SQLHidden:'',
    APIColumns: [] as any,
    x: '' as any,
});

const [DynamicTable] = useTable();

let onMoreDataLoad = computed(() => {
    if (PageSetting.moreData.length > 0) return PageSetting.moreData
    else return []
})
const { list, containerProps, wrapperProps } = useVirtualList(
    onMoreDataLoad,
    {
        itemHeight: 23,
    },
)
watch(() => list.value,
    () => {      
        let pushData: any[] = []
        PageSetting.List = []
        list.value.forEach(item => {
            
            if (item.index < PageSetting.moreData.length) {
                pushData.push(item.data)
            }
        })
        
        PageSetting.List = pushData
    })
watchEffect(() => {
    //一阶表格监控
    tableParams = useReportCommon[ReportNameParams].FirstTableParams;
    //console.log("2tableParams",tableParams)
    formParams= useReportCommon[ReportNameParams].FormParams;    
    getPanelWipStayNewDetail(tableParams,APIMessage,formParams,PageSetting,PanelWipDetailColumns); 
});

</script>


