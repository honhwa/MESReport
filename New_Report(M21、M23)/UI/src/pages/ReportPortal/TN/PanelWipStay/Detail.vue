<template>
    <div>
        <a-card>
            <div v-bind="containerProps" >
                <DynamicTable v-bind="wrapperProps" row-key="id" :header-title="PageSetting.Title"
                    :columns="PageSetting.Columns" :key="PageSetting.Columns" bordered size="small"
                    :data-all="PageSetting.moreData" sticky
                    :scroll="{x:2500, y:500}"
                    :data-source="PageSetting.List" :pagination="false" export-file-name="Export">
                    <template #export-button>
                        <a-button type="success"> Export </a-button>
                    </template>
                </DynamicTable>
            </div>

        </a-card>
    </div>
</template>

<script lang="ts" setup>
import { useTable } from '@/components/core/dynamic-table';
import { reactive, watchEffect, watch, computed } from 'vue';
import { GetCommon } from '@/api/pages/common';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { ReportCommon } from '@/store/module/ReportCommon';
import { useVirtualList } from '@vueuse/core'
import { useLoading } from '@/components/feat/Loading';
import type { TableColumn } from '@/components/core/dynamic-table';
import { message } from 'ant-design-vue';
//import {getPanelWipStayDetail} from './MOD/RenderFunction';




function getImport(site: any) {
    const { PanelWipDetailColumns } = require(`@/pages/ReportPortal/TN/PanelWipStay/MOD/Table`);
    return { PanelWipDetailColumns };
}
let { PanelWipDetailColumns } = getImport('TN');

let useCommonParams = AddCommonPageParams();
let useReportCommon = ReportCommon();
debugger
let ReportNameParams = `PanelWipStayDetail_${useCommonParams.Fab}`;
let tableParams = useReportCommon[ReportNameParams].FirstTableParams;
let APIMessage = JSON.parse(useReportCommon[ReportNameParams].APIMessage);
let formParams = useReportCommon[ReportNameParams].FormParams;


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
});


const [DynamicTable] = useTable();

function getPanelWipStayDetail(tableParams){      
    openFullLoading()
    getData(tableParams);

};

async function getData(tableParams: any) {
     openFullLoading();
     const ParamsData: any = {
        name: "Date",
        apiName: 'TN_PanelWipStayDataSetDetail',
        FactoryType: useCommonParams.Fab,
        G_FAC: useCommonParams.G_FAC_COM,
        
        Sqlwhere:APIMessage["sql_Where"],
        Prod_nbr:tableParams["PROD_NBR"]|| '',
        Work_ctr:tableParams["WORK_CTR1"]|| '',
        Dates:tableParams["title"].replace('*', ''),
        Queryby:formParams["Queryby"],
    };

    try{
       const { Date: { Data: { Table: Data },SQL01 } } = await GetCommon(ParamsData); 
       console.log("PanelWipStayDetail-SQLHidden:",SQL01)

       //PageSetting.SQLHidden="PanelWipStayDetail-SQLHidden:"+SQL01;
        
        PageSetting.Title=`${useCommonParams.Fab} Wip Panel Detail`
        if (Data.length != 0) { 
            PageSetting.Columns = PanelWipDetailColumns;
            PageSetting.moreData = Data;
        }else{
             message.warning("無數據！")
        }
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
        PageSetting.List = pushData
    })
watchEffect(() => {
    //一阶表格监控
    tableParams = useReportCommon[ReportNameParams].FirstTableParams;
    formParams= useReportCommon[ReportNameParams].FormParams;
    getPanelWipStayDetail(tableParams); 
});

</script>


