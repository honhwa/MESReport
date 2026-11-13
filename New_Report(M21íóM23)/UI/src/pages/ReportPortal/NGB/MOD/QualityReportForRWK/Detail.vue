<template>
    <div>
        <a-card class="ScrollTable">
          
           <div v-bind="containerProps" style="height: 430px;" >
            <DynamicTable row-key="id" v-bind="wrapperProps" :header-title="PageSetting.FirstTitle" :columns="PageSetting.FirstColumns"
                :key="PageSetting.FirstColumns" bordered size="small" :data-source="PageSetting.FirstList"
                sticky :data-all="PageSetting.moreData" :explain="false"
                :OwnerFoldParams="false"
                :pagination="false" :export-file-name="PageSetting.FirstTitle" :scroll="{ x: PageSetting.x,y:400}"
                :row-class-name="getRowClassNmae">
                <template #export-button>
                    <a-button type="success"> 导出 </a-button>
                </template>
            </DynamicTable>
            <div hidden>{{ PageSetting.SQLHidden}}</div>
           </div>
            <!-- <DynamicTable row-key="id" :header-title="PageSetting.SecondTitle" :columns="PageSetting.SecondColumns"
            sticky
                :key="PageSetting.SecondColumns" bordered size="small" :data-source="PageSetting.SecondList"
                :explain="false"
                :OwnerFoldParams="false"
                :pagination="false" :export-file-name="PageSetting.SecondTitle" :scroll="{ x: 600, y: PageSetting.y1 }"
                :row-class-name="getRowClassNmae">
                <template #export-button>
                    <a-button type="success"> 导出 </a-button>
                </template>
            </DynamicTable> -->
            <DynamicTable row-key="id" :header-title="PageSetting.ThirdTitle" :columns="PageSetting.ThirdColumns"
            sticky  :explain="false"
                :OwnerFoldParams="false"
                :key="PageSetting.ThirdColumns" bordered size="small" :data-source="PageSetting.ThirdList"
                v-if ="!['BLU1','BLU2'].includes(FactoryType)"
                :pagination="false" :export-file-name="PageSetting.ThirdTitle" :scroll="{ y: 400 }"
                :row-class-name="getRowClassNmae">
                <template #export-button>
                    <a-button type="success"> 导出 </a-button>
                </template>
            </DynamicTable>
            <div hidden>{{ PageSetting.SQLHidden2}}</div>
        </a-card>
    </div>
</template>


<script lang="ts" setup>
import { useTable } from '@/components/core/dynamic-table';
import { reactive, ref, watchEffect, watch, computed  } from 'vue';
import { GetCommon } from '@/api/pages/common';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { getRowClassNmae, getfinalsum, Inserttotalrow, dclick } from '@/pages/ReportPortal/utils/common';
import { cloneDeep } from 'lodash';
import { message } from 'ant-design-vue';
import { useLoading } from '@/components/feat/Loading';
import { ReportCommon } from '@/store/module/ReportCommon';
import { useVirtualList } from '@vueuse/core'


function getImport() {    
    const { getDetail } = require(`@/pages/ReportPortal/NGB/MOD/QualityReportForRWK/${bigfab}/RenderFunction`);
    const { detail1_1, } = require(`@/pages/ReportPortal/NGB/MOD/QualityReportForRWK/${bigfab}/table`);
    return {getDetail,detail1_1 };
}

let useCommonParams = AddCommonPageParams();
let useReportCommon = ReportCommon();
let bigfab=useCommonParams.Fab.slice(0,useCommonParams.Fab.length-1);
let ReportNameParams=`QualityReportForRWK-${useCommonParams.Fab}`;

let {getDetail,detail1_1} = getImport();
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
    ThirdList: [] as any,
    FirstColumns: [] as any,
    SecondColumns: [] as any,
    ThirdColumns: [] as any,
    FirstTitle: `${useCommonParams.Fab} Quality Report`,
    // SecondTitle: `${useCommonParams.Fab} Defect Reason Code Qty`,
    ThirdTitle: `${useCommonParams.Fab} Quality Report`,
    x: '' as any,
    y1: '' as any,
    SQLHidden:'',
    SQLHidden2:'',
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
        PageSetting.FirstList = []
        list.value.forEach(item => {            
            if (item.index < PageSetting.moreData.length) {
                pushData.push(item.data)
            }
        })
        
        PageSetting.FirstList = pushData
    })
watchEffect(() => {
    //一阶表格监控
    //一阶表格监控
    tableParams = useReportCommon[ReportNameParams].FirstTableParams;
    formParams= useReportCommon[ReportNameParams].FormParams;     
    getDetail(tableParams,APIMessage,formParams,PageSetting,detail1_1); 
});
</script>



    