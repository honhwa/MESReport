<template>
    <div>
        <a-card>            
            <DynamicTable
                row-key="id" sticky
                :header-title="PageSetting.TopTitle"
                :columns="PageSetting.Columns"
                :key="PageSetting.List"
                :row-class-name="getRowClassName"
                bordered
                size="small"
                :style="{ display: 'table',width:PageSetting.Columns.length > 8 ? '100% !important' : '70% !important'}"
                :data-source="PageSetting.List"
                :pagination="false" :explain="false"
                :scroll="{y:200}"
                :OwnerParams="false"
                export-file-name="导出"
            >
                <template #export-button>
                    <a-button type="success"> 导出 </a-button>
                </template>
            </DynamicTable>
            <div hidden>{{ PageSetting.SQLHidden }}</div>
        </a-card>
        <a-card >
            <DynamicTable sticky
                row-key="id"
                :header-title="PageSetting.BottomTitle"
                :columns="PageSetting.BottomColumns"
                :key="PageSetting.BottomList"
                bordered
                size="small"
                :data-source="PageSetting.BottomList"
                :pagination="{
                                pageSizeOptions:['50','100','200','1000'],
                                pageSize:50                            
                            }"
                :OwnerFoldParams="false"
                :scroll="{ x: PageSetting.x,y:450}"
                export-file-name="导出"
                :explain="false"
            >
                <template #export-button>
                    <a-button type="success"> 导出 </a-button>
                </template>
            </DynamicTable>
           
        </a-card>
    </div>
</template>


<script lang="tsx" setup>
import { useTable } from '@/components/core/dynamic-table';
import { watch, reactive, onMounted, watchEffect ,computed} from 'vue';
import { AddCommonPageParams} from '@/store/module/commonpage';
 import { GetCommon } from '@/api/pages/common';
import { useLoading } from '@/components/feat/Loading';
import { ReportCommon } from '@/store/module/ReportCommon';
import { useVirtualList } from '@vueuse/core'

let useCommonParams = AddCommonPageParams();
let useReportCommon = ReportCommon();
let ReportNameParams=`EquipmentUtilization-${useCommonParams.Fab}`;
let bigfab=useCommonParams.Fab.slice(0,useCommonParams.Fab.length-1)

let tableParams = useReportCommon[ReportNameParams].FirstTableParams;
let formParams= useReportCommon[ReportNameParams].FormParams;
let APIMessage=JSON.parse(useReportCommon[ReportNameParams].APIMessage);

function getImport() {    
    const { getDetail,getDetail2 } = require(`@/pages/ReportPortal/NGB/EquipmentUtilization/${bigfab}/RenderFunction`);
   // const { detail1_1, } = require(`@/pages/ReportPortal/NGB/DownGradeTop5DefectCode/${bigfab}/table`);
    return {getDetail,getDetail2 };
}
let {getDetail,getDetail2} = getImport();

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});

let PageSetting = reactive({
    moreData: [] as any,
    List: [] as any,
    Columns: [] as any,
    TopTitle: '',
    BottomTitle: '',
    BottomList: [] as any,
    BottomColumns: [] as any,
    SQLHidden:'',
    BottomSQLHidden:'',
    x: '' as any,
});

const [DynamicTable] = useTable(); 
let onMoreDataLoad = computed(() => {
    if (PageSetting.moreData.length > 0) return PageSetting.moreData
    else return []
})
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
