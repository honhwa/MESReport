<template>
    <div class="OperationPerformanceNew">
        <a-card> 
          <div class="ScrollTable"  >         
            <DynamicTable  sticky        
                row-key="id"
                :header-title="PageSetting.TopTitle"
                :columns="PageSetting.Columns"
                :key="PageSetting.Columns"
                bordered
                size="small"
                :PageParams="PageParams"
                :data-source="PageSetting.List"
                :data-all="PageSetting.moreData"
                :pagination="PageSetting.pagination"
                :style="{ display: 'table',width:PageSetting.Columns.length > 8 ? '100% !important' : '70% !important'}"
                :OwnerParams="false"
                :scroll="{x: PageSetting.x2, y: 350 }"
                :explain="false"
                export-file-name="导出"
                :row-class-name="getRowClassName" v-show="RightTopPageShow"
            >
            <template #export-button> <a-button type="success"> 导出 </a-button> </template>
            </DynamicTable>
          
          <!-- </div> -->
         
            <DynamicTable sticky
                row-key="id"
                :header-title="PageSetting.BottomTitle"
                :columns="PageSetting.SecondColumns"
                :key="PageSetting.SecondColumns"
                bordered
                size="small"
                :data-source="PageSetting.SecondList"                          
                :explain="false"
                :OwnerFoldParams="false"
                :scroll="{ x: PageSetting.x,y: 350 }"
                :pagination="{
                                pageSizeOptions:['20','50','100','200'],
                                pageSize:20                            
                            }"
                export-file-name="导出"
                :row-class-name="getRowClassName" v-show="RightBottomPageShow"
            >
                <template #export-button> <a-button type="success"> 导出 </a-button> </template>
            </DynamicTable>
        </div>
            <div hidden>{{ PageSetting.SQLHidden }}</div>
            <div hidden>{{ PageSetting.BottomSQLHidden }}</div>
            <!-- <DynamicTable
                row-key="id"
                :header-title="PageSetting.ThirdTitle"
                :columns="PageSetting.ThirdColumns"
                :key="PageSetting.ThirdColumns"
                bordered
                size="small"
                :data-source="PageSetting.ThirdList"
                :pagination="false"
                :scroll="{y: 350 }"
                export-file-name="导出"
                :row-class-name="getRowClassNmae"
            >
                <template #export-button> <a-button type="success"> 导出 </a-button> </template>
            </DynamicTable> -->
        </a-card>
    </div>
</template>

<script lang="ts" setup>
import { useTable } from '@/components/core/dynamic-table';
import { watch, reactive, onMounted, watchEffect, ref, } from 'vue';
//import { getDetailStart, getFirstStart ,getThirddetailstart} from './RenderFunction';
//import { getDetailStart, getFirstStart,RightTopPageShow,RightBottomPageShow} from './LCM/RenderFunction';
import { AddOperationParams } from '@/store/module/operationDeatil';
import { getRowClassName } from '@/pages/ReportPortal/utils/NGBcommon'
import {PageParams} from '@/components/core/dynamic-table/src/dynamic-table'
import { ReportCommon } from '@/store/module/ReportCommon';
import { AddCommonPageParams } from '@/store/module/commonpage';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';


import { message } from 'ant-design-vue';
let useCommonParams = AddCommonPageParams();
let useReportCommon = ReportCommon();
let ReportNameParams=`OperationPerformanceNew-${useCommonParams.Fab}`;
let bigfab=useCommonParams.Fab.slice(0,useCommonParams.Fab.length-1)

//const OperationPerformanceDetailTable= ref<HTMLElement | null>(null);
const { InfiniteScroll } = useRemoteSelect();

let tableParams =useReportCommon[ReportNameParams].FirstTableParams;
let formParams =useReportCommon[ReportNameParams].FormParams;
let APIMessage=useReportCommon[ReportNameParams].APIMessage;
function getImport(site: any) {
    const { getDetailStart, getFirstStart,RightTopPageShow,RightBottomPageShow} = require(`@/pages/ReportPortal/NGB/MOD/OperationPerformanceNew/${bigfab}/RenderFunction`);
    return {getDetailStart, getFirstStart,RightTopPageShow,RightBottomPageShow};
}

let { getDetailStart, getFirstStart,RightTopPageShow,RightBottomPageShow} = getImport(useCommonParams.Fab);

 
let PageSetting = reactive({
    List: [] as any,
    Columns: [] as any,
    SecondList: [] as any,
    SecondColumns: [] as any,
    TopTitle: '',
    BottomTitle: '',
    ThirdList: [] as any,
    ThirdColumns: [] as any,
    BottomSQLHidden:'',
    SQLHidden:'',
    x: '' as any,
    x2: '' as any,
    pagination:false,
});
// PageSetting.Columns = OperDetail3Columns;
const [DynamicTable] = useTable();

// 点击提交
// PageSetting.columns = [...baseColumns];

watchEffect(() => {
    //一阶表格监控
    PageSetting['List'] = [];
    PageSetting['Columns'] = [];
    PageSetting['SecondList'] = [];
    // PageSetting['ThirdList'] = [];
    // PageSetting['ThirdColumns'] = [];
    PageSetting['BottomTitle'] = "";
    // PageSetting['ThirdTitle'] = "";
    PageSetting['TopTitle'] = "";
    PageSetting['SQLHidden'] = "";
    PageSetting['BottomSQLHidden'] = "";
    tableParams = useReportCommon[ReportNameParams].FirstTableParams;
    formParams =useReportCommon[ReportNameParams].FormParams;
    APIMessage=useReportCommon[ReportNameParams].APIMessage;
    
    PageSetting.pagination=false;   
    getFirstStart(tableParams, PageSetting,formParams,APIMessage);
});

watch(
    //二阶表格监控
    () => useReportCommon[ReportNameParams].SecondTableParams,
    (newValue) => {
        PageSetting['SecondList'] = [];
        PageSetting['BottomTitle'] = "";
        PageSetting['BottomSQLHidden'] = "";
        // PageSetting['ThirdList'] = [];
        // PageSetting['ThirdTitle'] = "";
        // PageSetting['ThirdColumns'] = []; 
        getDetailStart(tableParams, PageSetting,formParams,useReportCommon[ReportNameParams].SecondTableParams);
    },
    {
        deep: true
    },

);
//InfiniteScroll(OperationPerformanceDetailTable, PageSetting.List);
// watch(// 三阶表格监控
//     () => useOperation.ThirdTableParams,
//     () => {
//         PageSetting['ThirdList'] = [];
//         PageSetting['ThirdTitle'] = "";
//         PageSetting['ThirdColumns'] = [];        
//         getThirddetailstart(tableParams, PageSetting);
        
//     },
//     {
//         deep: true
//     },)
</script>
<!-- <script lang="ts">
export default{
    name:'OperationPerformanceDetail'
}
</script> -->
<style lang="less" scoped>
.btn-rows {
    button {
        margin-right: 12px;
    }
}

.OperationPerformanceNew :deep(.v-center) {
    background-color: #f0f0f0;
}
// .baseModel {
//     overflow-y: auto;
//     overflow-x: hidden;
//      max-width: 200px;
//     height: 85vh;
// }
</style>
