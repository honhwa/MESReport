<template>
    <div>
        <a-card>
                <div v-bind="containerProps" style="height: 80vh">
                    <DynamicTable sticky v-bind="wrapperProps" row-key="id" :header-title="PageSetting.Title" :columns="PageSetting.Columns"
                        :key="PageSetting.Columns" bordered size="small" 
                        :row-class-name="getRowClassName"  :data-all="PageSetting.moreData"
                        :data-source="PageSetting.List" :OwnerParams="false" :explain="false"                        
                        :pagination="false"  export-file-name="导出">
                        <template #export-button>
                            <a-button type="success"> 导出 </a-button>
                        </template>
                    </DynamicTable> 
                </div>                   
               
                <div hidden>{{ PageSetting.SQLHidden }}</div>
           
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
let ReportNameParams=`MfgOperatorNew-${useCommonParams.Fab}`;
let bigfab=useCommonParams.Fab.slice(0,useCommonParams.Fab.length-1)

let tableParams = useReportCommon[ReportNameParams].FirstTableParams;
let formParams= useReportCommon[ReportNameParams].FormParams;
let APIMessage=JSON.parse(useReportCommon[ReportNameParams].APIMessage);

function getImport() {
    const DetailColumns = require(`@/pages/ReportPortal/SH/MOD/MfgOperatorNew/${bigfab}/Table`);
    return DetailColumns
   
}
let DetailColumns = getImport();

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});

let PageSetting = reactive({
    List: [] as any,
    Columns: [] as any,
    Title: '',
    moreData: [],
    SQLHidden:'',
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

watchEffect(async() => {
    //一阶表格监控
    PageSetting['List'] = [];
    PageSetting['Columns'] = [];
    PageSetting['Title'] = "";
    PageSetting['SQLHidden'] = "";
    tableParams = useReportCommon[ReportNameParams].FirstTableParams;
    openFullLoading();      
    getTableData(tableParams, formParams,PageSetting);  
    
});

onMounted(
    async () => {
        await getTableData(tableParams, formParams,PageSetting);
    }
)

async function getTableData(tableParams, formParams,PageSetting) {
    
    const ParamsData: any = {
        name: "Date",
        apiName: "MFGOperatorDetail_MOD",
        FactoryType: formParams["FactoryType"],
        G_FAC: formParams["G_FAC"],
        FromDate:formParams["FromDate"],
        ToDate:formParams["ToDate"],
        QueryType:formParams["QueryType"],

        sql_Floor:APIMessage["sql_Floor"],
        sql_Shift:APIMessage["sql_Shift"],
        sql_Product:APIMessage["sql_Product"],
        sql_Operation:APIMessage["sql_Operation"],
        sql_LcmOwner:APIMessage["sql_LcmOwner"], 
        
        col0:tableParams['OPERATORS'] ?? tableParams['EQUIP_NBR'] ?? '',
        col1Name:tableParams["title"],
   };

   PageSetting.Title = `${formParams["FactoryType"]} MFGOperator Detail`;

    try{
        const { Date: { Data: { Table: Data },SQL01,} } = await GetCommon(ParamsData);

        //console.log("MFGOperatorDetail-SQLSecondDetail:",SQL01) 
        PageSetting.SQLHidden="MFGOperatorNewDetail-SQLSecondDetail:"+SQL01;
       
        PageSetting.Columns = DetailColumns.MfgOperatorNewDetailColumns;
        //PageSetting.List = Data;
          PageSetting.moreData = Data;  
        if (Data.length != 0) {       
        PageSetting.moreData = Data;
        PageSetting.List =PageSetting.moreData;
        PageSetting.x = null;
        if (PageSetting.Columns.length > 7) {
            PageSetting.x = 110 * PageSetting.Columns.length;
        }


       }else{

             console.log("無數據！")
     }
    closeFullLoading();

             //加載防護罩結束
            closeFullLoading();
       
    }catch(error: any){ 
         //加載防護罩結束
        closeFullLoading();
        return Promise.reject(error); 
     }

}

</script>

<style lang="less" scoped>
.btn-rows {
    button {
        margin-right: 12px;
    }
}
.swapper {
    display: flex;
    flex-direction: column;
}
</style>
