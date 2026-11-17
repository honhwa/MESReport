<template>
    <div class="StartBankStatusWip">
        <a-card>
            <!-- <div v-bind="containerProps" style="height: 600px"> -->
                <DynamicTable v-bind="wrapperProps" row-key="id" :header-title="PageSetting.Title"
                    :columns="PageSetting.Columns" :key="PageSetting.List" bordered size="small"
                    :data-source="PageSetting.List" :pagination="false" export-file-name="Export"
                   :data-all="PageSetting.moreData" :scroll="{x:2500, y:500}"
                    >
                    <template #export-button>
                        <a-button type="success"> Export </a-button>
                    </template>
                </DynamicTable>
            <!-- </div> -->
            
        </a-card>
    </div>
</template>

<script lang="ts" setup>
import { useTable } from '@/components/core/dynamic-table';
import { watch, reactive, onMounted, watchEffect ,computed} from 'vue';
import { AddCommonPageParams} from '@/store/module/commonpage';
import { ReportCommon } from '@/store/module/ReportCommon';
import { GetCommon } from '@/api/pages/common';
import { useLoading } from '@/components/feat/Loading';
import { useVirtualList } from '@vueuse/core';
//import { getStartBankStatusWipDetail} from './MOD/RenderFunction';


import { message } from 'ant-design-vue';
import { timestamp } from '@vueuse/shared';
let useCommonParams = AddCommonPageParams();
let useReportCommon = ReportCommon();

let ReportNameParams = `StartBankStatusDetail_${useCommonParams.Fab}`;
let tableParams = useReportCommon[ReportNameParams].FirstTableParams;
let formParams = useReportCommon[ReportNameParams].FormParams;		

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});

 
let PageSetting = reactive({
    List: [] as any,
    Columns: [] as any,
    Title: '',
    moreData: [],
});

const [DynamicTable] = useTable(); 
PageSetting.Title=`${useCommonParams.Fab} Start Bank Status Wip Detail`

function getStartBankStatusWipDetail(tableParams,formParams,PageSetting){      
    openFullLoading()
    getData(tableParams,formParams,PageSetting);

};

async function getData(tableParams,formParams,PageSetting){    
    const ParamsData: any = {
    name: "Date",
    apiName: "TN_StartBankStatusWipDetail",
    FactoryType: useCommonParams.Fab,
    G_FAC: useCommonParams.G_FAC_COM,
    ToDate:formParams["vDate_e1"],
    WorkCtr:tableParams["title"].replace('*', '').slice(5),
    Lcd_Prod_nbr:tableParams["LCD_PROD"],
    LcdGrade:tableParams["GRADE"],
    LcdOwner:tableParams["title"].trim().slice(0,4),
    CstType:tableParams["CST_TYPE"],
   };


  try{
       const { Date: { Data: { Table: Data },sql01 } } = await GetCommon(ParamsData); 
       console.log("StartBankStatusDetail-SQLHidden:",sql01)

        let ColumsData: any = [];
        let WidthPars=50;
        let classNamePars="pro_auto-center";

        for (let item in Data[0]) { 
            ColumsData.push({ title: item, align: 'center', dataIndex: item, className: classNamePars, width:WidthPars});
            
        }
    
        PageSetting.Columns=ColumsData;
        PageSetting.List = Data;
        PageSetting.moreData= Data;
        closeFullLoading();

    }catch(error: any){ 
        closeFullLoading();
        return Promise.reject(error); 
    }
}
 

let onMoreDataLoad = computed(() => {
    debugger
    console.log("PageSetting.moreData.length",PageSetting.moreData.length)
    if (PageSetting.moreData.length > 0) return PageSetting.moreData
    else return []
})
const { list, containerProps, wrapperProps } = useVirtualList(
    onMoreDataLoad,
    {
        itemHeight: 99,
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
        console.log("pushData",pushData)
        PageSetting.List = pushData
    })
// 点击提交
watchEffect(async() => {
    //一阶表格监控
    debugger
    PageSetting['List'] = [];
    PageSetting['Columns'] = [];
    PageSetting['Title'] = "";
    tableParams = useReportCommon[ReportNameParams].FirstTableParams;
    formParams= useReportCommon[ReportNameParams].FormParams;
    getStartBankStatusWipDetail(tableParams, formParams,PageSetting);    
});


   
     
        







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


.StartBankStatusWip :deep(.v-center) {
    background-color: #f0f0f0;
    border: 1px solid black;
}

</style>
