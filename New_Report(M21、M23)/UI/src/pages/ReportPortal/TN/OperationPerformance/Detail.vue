<template>
    <div class="OperationPerformance">
        <Loading
            :loading="compState.loading"
            :absolute="compState.absolute"
            :theme="compState.theme"
            :background="compState.background"
            :tip="compState.tip"
        />
        <a-card>
            <DynamicTable sticky
                row-key="id"
                :header-title="PageSetting.TopTitle"
                :columns="PageSetting.Columns"
                :key="PageSetting.Columns"
                bordered
                size="small"
                :data-source="PageSetting.List"
                :pagination="false"
                :scroll="{ y: 350 }"
                export-file-name="Export"
                :row-class-name="getRowClassNmae"
            >
                <template #export-button> <a-button type="success"> Export </a-button> </template>
            </DynamicTable>
            <DynamicTable sticky
                row-key="id"
                :header-title="PageSetting.BottomTitle"
                :columns="PageSetting.SecondColumns"
                :key="PageSetting.SecondColumns"
                bordered
                size="small"
                :data-source="PageSetting.SecondList"
                :pagination="false"
                :scroll="{ y: 350 }"
                export-file-name="Export"
                :row-class-name="getRowClassNmae"
            >
                <template #export-button> <a-button type="success"> Export </a-button> </template>
            </DynamicTable>
        </a-card>
    </div>
</template>

<script lang="ts" setup>
import { useTable } from '@/components/core/dynamic-table';
import { watch, reactive, ref, onMounted, watchEffect } from 'vue';
import { getDetailStart, getFirstStart ,getThirddetailstart} from './RenderFunction';
import { AddOperationParams } from '@/store/module/operationDeatil';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { ReportCommon } from '@/store/module/ReportCommon';
import { getRowClassNmae } from '@/pages/ReportPortal/utils/common'
import { Loading, useLoading } from '@/components/feat/Loading';

import { message } from 'ant-design-vue';
let useOperation = AddOperationParams();
let useReportCommon = ReportCommon();
let useCommonParams = AddCommonPageParams();

let ReportNameParams = `OperationPerformanceDetail_${useCommonParams.Fab}`;
debugger
let tableParams = useReportCommon[ReportNameParams].FirstTableParams;
let APIMessage = JSON.parse(useReportCommon[ReportNameParams].APIMessage);
let formParams = useReportCommon[ReportNameParams].FormParams;
let tableDetailParams = useReportCommon[ReportNameParams].SecondTableParams;
//let tableParams = useOperation.FirstTableParams;
//let tableDetailParams = useOperation.SecondTableParams;


let PageSetting = reactive({
    List: [] as any,
    Columns: [] as any,
    SecondList: [] as any,
    SecondColumns: [] as any,
    TopTitle: '',
    BottomTitle: '',
    ThirdList: [] as any,
    ThirdColumns: [] as any,
    ThirdTitle:''
});

const compState = reactive({
    absolute: true,
    loading: false,
    theme: 'dark',
    background: 'rgba(111,111,111,.7)',
    tip: '加载中...'
});

const [openFullLoading, closeFullLoading] = useLoading({
        tip: '數據加載中...',
      });

// PageSetting.Columns = OperDetail3Columns;
const [DynamicTable] = useTable();

// 点击提交
// PageSetting.columns = [...baseColumns];


watchEffect(() => {
    openFullLoading()
    //一阶表格监控
    PageSetting['List'] = [];
    PageSetting['Columns'] = [];
    PageSetting['SecondList'] = [];
    PageSetting['ThirdList'] = [];
    PageSetting['ThirdColumns'] = [];
    PageSetting['BottomTitle'] = "";
    PageSetting['ThirdTitle'] = "";
    PageSetting['TopTitle'] = "";
    //tableParams = useOperation.FirstTableParams;
    tableParams = useReportCommon[ReportNameParams].FirstTableParams;
    getFirstStart(tableParams, PageSetting);
    closeFullLoading();
});
watch(
    //二阶表格监控
    () => useReportCommon[ReportNameParams].SecondTableParams,
    (newValue) => {
        //console.log("newValue",newValue);
        PageSetting['SecondList'] = [];
        PageSetting['BottomTitle'] = "";
        PageSetting['ThirdList'] = [];
        PageSetting['ThirdTitle'] = "";
        PageSetting['ThirdColumns'] = [];
        //tableDetailParams = useOperation.SecondTableParams;
        debugger
        tableParams = useReportCommon[ReportNameParams].FirstTableParams;
        tableDetailParams = useReportCommon[ReportNameParams].SecondTableParams;
        getDetailStart(tableParams, PageSetting, tableDetailParams);
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
        tableParams = useReportCommon[ReportNameParams].FirstTableParams;
        getThirddetailstart(tableParams, PageSetting);
    },
    {
        deep: true
    },)
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


.OperationPerformance :deep(.v-center) {
    background-color: #f0f0f0;
}

.OperationPerformance :deep(.Yellow) {
    background-color: #80ff80;
}

.OperationPerformance :deep(.YellowGreen) {
    background-color: #f39230;
}
// .baseModel {
//     overflow-y: auto;
//     overflow-x: hidden;
//      max-width: 200px;
//     height: 85vh;
// }
</style>
