<template>
    <div class="OperationPerformance">
        <a-card>
            <DynamicTable row-key="id" :header-title="PageSetting.TopTitle" :columns="PageSetting.Columns"
                :key="PageSetting.Columns" 
                sticky
                :data-all="PageSetting.List"
                :flexwidth="1"
                :style="{ 'margin-bottom': '10px' }"
                bordered  :data-source="PageSetting.List" :pagination="false"
                 export-file-name="导出" :row-class-name="getRowClassNmae">
                <template #export-button>
                    <a-button type="success"> 导出 </a-button>
                </template>
            </DynamicTable>
            <DynamicTable row-key="id" :header-title="PageSetting.BottomTitle" :columns="PageSetting.SecondColumns"
                :key="PageSetting.SecondColumns" bordered  :data-source="PageSetting.SecondList"
                sticky
                :flexwidth="1"
                :data-all="PageSetting.SecondList"
                :style="{ 'margin-bottom': '10px' }"
                :pagination="false"  export-file-name="导出" :row-class-name="getRowClassNmae">
                <template #export-button>
                    <a-button type="success"> 导出 </a-button>
                </template>
            </DynamicTable>
            <DynamicTable row-key="id" :header-title="PageSetting.ThirdTitle" :columns="PageSetting.ThirdColumns"
                :key="PageSetting.ThirdColumns" bordered  :data-source="PageSetting.ThirdList"
                sticky
                :flexwidth="1"
                :data-all="PageSetting.ThirdList"
                :pagination="false"  export-file-name="导出" :row-class-name="getRowClassNmae">
                <template #export-button>
                    <a-button type="success"> 导出 </a-button>
                </template>
            </DynamicTable>
        </a-card>
    </div>
</template>

<script lang="ts" setup>
import { useTable } from '@/components/core/dynamic-table';
import { watch, reactive, ref, onMounted, watchEffect } from 'vue';
import { AddOperationParams } from '@/store/module/operationDeatil';
import { getRowClassNmae } from '@/pages/ReportPortal/utils/common'
import { message } from 'ant-design-vue';
import { AddCommonPageParams } from '@/store/module/commonpage';
const UseParamsAdd = AddCommonPageParams()
let FactoryType: any = UseParamsAdd.Fab;
// console.log("detail页面的FactoryType",FactoryType);

let importurl = ref("LCM");
switch (FactoryType) {
    case "LCM1":
    case "LCM2":
    case "SET1":
    case "SET2":
        importurl.value = "LCM";
        break;
    case "BLU1":
    case "BLU2":
        importurl.value = "BLU";
        break;
    default:
        break;
}
console.log("detail的site是",importurl.value);

function getImport(site: any) {
    const {  getDetailStart, getFirstStart, getThirddetailstart  } = require(`@/pages/ReportPortal/FS/OperationPerformance/${site}/RenderFunction`);
    return {  getDetailStart, getFirstStart, getThirddetailstart};
}
const {  getDetailStart, getFirstStart, getThirddetailstart} = getImport(importurl.value);
let useOperation = AddOperationParams();
let tableParams = useOperation.FirstTableParams;
let PageSetting = reactive({
    List: [] as any,
    Columns: [] as any,
    SecondList: [] as any,
    SecondColumns: [] as any,
    TopTitle: '',
    BottomTitle: '',
    ThirdList: [] as any,
    ThirdColumns: [] as any,
    ThirdTitle: ''
});
const [DynamicTable] = useTable();

watchEffect(() => {
    //一阶表格监控
    PageSetting['List'] = [];
    PageSetting['Columns'] = [];
    PageSetting['SecondList'] = [];
    PageSetting['ThirdList'] = [];
    PageSetting['ThirdColumns'] = [];
    PageSetting['BottomTitle'] = "";
    PageSetting['ThirdTitle'] = "";
    PageSetting['TopTitle'] = "";
    tableParams = useOperation.FirstTableParams;
    getFirstStart(tableParams, PageSetting);
});
watch(
    //二阶表格监控
    () => useOperation.SecondTableParams,
    (newValue) => {
        // console.log(newValue);
        
        PageSetting['SecondList'] = [];
        PageSetting['BottomTitle'] = "";
        PageSetting['ThirdList'] = [];
        PageSetting['ThirdTitle'] = "";
        PageSetting['ThirdColumns'] = [];
        getDetailStart(tableParams, PageSetting);
    },
    {
        deep: true
    },
);
watch(// 三阶表格监控
    () => useOperation.ThirdTableParams,
    () => {
        PageSetting['ThirdList'] = [];
        PageSetting['ThirdTitle'] = "";
        PageSetting['ThirdColumns'] = [];
        getThirddetailstart(tableParams, PageSetting);
    },
    {
        deep: true
    },)
</script>

<style lang="less" scoped>
:deep(.ant-table) {
    height: 30vh !important;
}

:deep(.ant-table-container) {
    height: 30vh !important;
}

:deep(.ant-table-wrapper) {
    height: 30vh !important;
    // overflow: auto;
}

:deep(.ant-table-body) {
    height: 30vh !important;
    overflow: auto;
}
</style>
