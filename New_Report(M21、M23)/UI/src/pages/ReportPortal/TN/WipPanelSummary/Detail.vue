<template>
    <div>
        <a-card>
           
                <DynamicTable row-key="id" :header-title="PageSetting.Title"
                    :columns="PageSetting.Columns" :key="PageSetting.Columns" bordered size="small"
                    :scroll="{ x: 2000, y:500 }" :data-all="PageSetting.moreData"
                    sticky
                    :data-source="PageSetting.List" :pagination="false" export-file-name="Export">
                    <template #export-button>
                        <a-button type="success"> Export </a-button>
                    </template>
                </DynamicTable>
           
        </a-card>
    </div>
</template>



<script lang="ts" setup>
import { useTable } from '@/components/core/dynamic-table';
import { reactive, watchEffect, watch, computed } from 'vue';
import { GetCommon } from '@/api/pages/common';
import { useVirtualList } from '@vueuse/core';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { ReportCommon } from '@/store/module/ReportCommon';
import { useLoading } from '@/components/feat/Loading';
import { Console } from 'console';
import { message } from 'ant-design-vue';
import { getRowClassName } from '@/pages/ReportPortal/utils/TNcommon'
//import {getWipPanelSummaryDetail} from './MOD/RenderFunction';

function getImport() {
    const WipPanSumDetailCol = require(`@/pages/ReportPortal/TN/WipPanelSummary/MOD/Table`);
    return WipPanSumDetailCol
    // return { OperDetail3Columns, WipColumns,WipDetailCoulmns };
}
let WipPanSumDetailCol = getImport();
let useCommonParams = AddCommonPageParams();
let useReportCommon = ReportCommon();

let ReportNameParams = `WipPanelSummaryDetail_${useCommonParams.Fab}`;
let tableParams = useReportCommon[ReportNameParams].FirstTableParams;
let APIMessage = JSON.parse(useReportCommon[ReportNameParams].APIMessage);
let formParams = useReportCommon[ReportNameParams].FormParams;	

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});

// let formParams = 
let PageSetting = reactive({
    moreData: [],
    List: [] as any,
    Columns: [] as any,
    Title: '',
    // BottomTitle:''
});
const [DynamicTable] = useTable();


function getWipPanelSummaryDetail(tableParams,formParams,PageSetting, APIMessage){      
    
    getData(tableParams,formParams,PageSetting, APIMessage);
    

};

async function getData(tableParams,formParams,PageSetting, APIMessage){      
    openFullLoading()
    const ParamsData: any = {
    name: "Date",
    apiName: "TN_WipPanelSumDataSetDetail",
    FactoryType: useCommonParams.Fab,
    G_FAC: useCommonParams.G_FAC_COM,
    OverHours:formParams["StartHour"],
    SelectColumn:formParams["Colname"],
    Sqlwhere:APIMessage["sql_Where"],
    
    ProdNbr:tableParams["PROD_NBR"],
    Owner:tableParams["LCM_OWNER"],
    Operation:tableParams["WORK_CTR"],
    HoldFlag:tableParams["HOLD_FLAG"],
    HoldCode:tableParams["HOLD_CODE"],
    Size:tableParams["PANEL_SIZE"],
    WoNbr:tableParams["WO_NBR"],
    LcdProduct:tableParams["LCD_PROD_NBR"],
    LcdGrade:tableParams["LCD_GRADE"],
    MvinFlag:tableParams["MVIN_FLAG"],
    LcmGrade:tableParams["LCM_GRADE"],
    QTime:tableParams["MIN_QRS_DT"],
    ERP:tableParams["ERP_ORDER_TYPE"],
    
   };

  
  try{
       const { Date: { Data: { Table: Data },SQL01 } } = await GetCommon(ParamsData); 
       console.log("WipPanelSumDetail-SQLHidden:",SQL01)
       //if (Data.length != 0) {
        let ColumsData: any = [];
        let WidthPars=50;
        let classNamePars="pro_auto-center";
        

            PageSetting.Columns = WipPanSumDetailCol.WipPanSumDetailCol;
            PageSetting.moreData = Data;

            if (PageSetting.moreData.length > 100) PageSetting.List = PageSetting.moreData.slice(0, 40);
            else PageSetting.List = PageSetting.moreData;
        //}else{

        //         message.warning("無數據！")
        // }
        
        closeFullLoading();
    }catch(error: any){ 
        closeFullLoading();
        return Promise.reject(error); 
    }
}


let onMoreDataLoad = computed(() => {
    if (PageSetting.moreData.length > 0) return PageSetting.moreData
    else return []
})
const { list, containerProps, wrapperProps } = useVirtualList(
    onMoreDataLoad,
    {
        itemHeight: 100,
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
watchEffect(async() => {
     //一阶表格监控
    PageSetting['List'] = [];
    PageSetting['Columns'] = [];
    PageSetting['Title'] = "";
    tableParams = useReportCommon[ReportNameParams].FirstTableParams;
    APIMessage= useReportCommon[ReportNameParams].APIMessage;
    formParams = useReportCommon[ReportNameParams].FormParams;
    getWipPanelSummaryDetail(tableParams, formParams,PageSetting, APIMessage); 
    
});

 


</script>


