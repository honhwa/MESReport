<template>
    <div class="ChangeProductReport">
        <a-card>
            <a-row>
                <a-col>
                    <DynamicTable row-key="id" :header-title="PageSetting.title" :columns="PageSetting.TopColumns"
                        :key="PageSetting.TopColumns" bordered  
                        sticky
                        :row-class-name="getRowClassNmae"
                        :flexwidth="1"
                        :data-source="PageSetting.TopList"
                        :data-all="PageSetting.TopList"
                        :pagination="false"  export-file-name="导出">
                        <template #export-button>
                            <a-button type="success"> 导出 </a-button>
                        </template>
                    </DynamicTable>
                </a-col>
            </a-row>
        </a-card>
    </div>
</template>


<script lang="tsx" setup>
import { useTable } from '@/components/core/dynamic-table';
import { reactive } from '@vue/reactivity';
import { GetCommon } from '@/api/pages/common';
import {  onMounted, watch, watchEffect } from '@vue/runtime-core';
import { getRowClassNmae } from '@/pages/ReportPortal/utils/common'
import { ReportCommon } from '@/store/module/ReportCommon';
const UseParamsAdd = ReportCommon();
let FactoryType: string = UseParamsAdd.Fab;

let ALLFormParams = UseParamsAdd["ChangeProductReport" + FactoryType]["FirstTableParams"];

watchEffect(async() => {
    ALLFormParams = UseParamsAdd["ChangeProductReport" + FactoryType]["FirstTableParams"];
    await getTableData();
});


function getImport(site: any) {
    const { detailColumns } = require(`@/pages/ReportPortal/FS/ChangeProductReport/${site}/table`);
    return { detailColumns };
}
const { detailColumns } = getImport(FactoryType);

// 頁面數據
let PageSetting = reactive({
    TopList: [] as any,
    TopColumns: [] as any,
    Message: {},
    title: "LCMA Change Product Detail"


});
const [DynamicTable] = useTable();

async function getTableData() {
    PageSetting.TopList = [];
    PageSetting.TopColumns = [];
    const { Date: { Data0: { Table: Data }, Message: sqlmessage } } = await GetCommon(ALLFormParams);
    if(Data.length!=0){
        let tempobj = {};
        Object.keys(Data[0]).forEach(item=>{
            if(item == "ENTITY_NO"){
            tempobj[item] = "合計"
            }else if(["WORK_CTR"].includes(item)){
                tempobj[item] = Data.length
            }else{
                tempobj[item] = ""
            }
        })
        Data.push(tempobj);
    }
    
    PageSetting.TopList = Data;
    PageSetting.TopColumns = detailColumns;

}
onMounted(
    async () => {
        await getTableData();
    }
)

</script>

<style lang="less" scoped>



</style>
