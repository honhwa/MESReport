<template>
    <div class="MTLConsume">
        <a-card>
            <a-row>
                <a-col>
                    <DynamicTable row-key="id" :header-title="PageSetting.title" :columns="PageSetting.TopColumns"
                        :key="PageSetting.TopColumns" bordered size="small" 
                        :row-class-name="getRowClassNmae"
                        :data-source="PageSetting.TopList"
                        :pagination="false" :scroll="{  y: 500 }" export-file-name="导出">
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
import { reactive} from '@vue/reactivity';
import { GetCommon } from '@/api/pages/common';
import { onMounted, watch } from '@vue/runtime-core';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { MTLConsumeWithOut } from '@/store/module/MTLConsume';
import { getRowClassNmae } from '@/pages/ReportPortal/utils/common'


const UseParamsAdd = AddCommonPageParams()
let FactoryType: any = UseParamsAdd.Fab;

const UseWoOrder = MTLConsumeWithOut();
let ALLFormParams = UseWoOrder.ALLFormParams;
watch(
    () => UseWoOrder.ALLFormParams,
    async () => {
        ALLFormParams = UseWoOrder.ALLFormParams;
        await getTableData();
    },
    {
        deep: true,
    }

)

function getImport(site: any) {
    const { detailColumsn } = require(`@/pages/ReportPortal/${site}/MTLConsume/LCM1/table`);
    return { detailColumsn };
}
const { detailColumsn } = getImport('FS');

// 頁面數據
let PageSetting = reactive({
    TableSpan: 20,
    showmenu: true,
    TopList: [] as any,
    TopColumns: [] as any,
    Message: {},
    title: "LCMA 材料耗用報表"


});
const [DynamicTable] = useTable();

async function getTableData() {
    PageSetting.TopList = [];
    PageSetting.TopColumns = [];
    const { Date: { Data0: { Table: Data }, Message: sqlmessage } } = await GetCommon(ALLFormParams);
    if(Data.length!=0){
        let tempobj = {};
        Object.keys(Data[0]).forEach(item=>{
            if(item == "TRANS_DATE"){
            tempobj[item] = "合計"
            }else if(["LOT_NUMBER","MAT_QTY_OK"].includes(item)){
                tempobj[item] = Data.length
            }else{
                tempobj[item] = ""
            }
        })
        Data.push(tempobj);
    }
    
    PageSetting.TopList = Data;
    PageSetting.TopColumns = detailColumsn;

}
onMounted(
    async () => {
        await getTableData();
    }
)

</script>

<style lang="less" scoped>
.btn-rows {
    button {
        margin-right: 12px;
    }
}

.MTLConsume :deep(.sub-total) {
    background-color: #ffb468 !important;
}

.MTLConsume :deep(.total) {
    background-color: #ff80c0 !important;
}

.swapper {
    display: flex;
    flex-direction: column;
}
</style>
