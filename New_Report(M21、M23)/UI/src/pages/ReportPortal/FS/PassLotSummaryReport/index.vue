<template>
    <div class="PassLotSummaryReport">
        <a-card>
            <SchemaForm @submit="confirm"> </SchemaForm>
            <span>Update:{{UpdateTime}}</span>
            <div class="warning">
                <span>注意事項：</span>
                <ol>
                    <li v-for="item in WARNING">{{item}}</li>
                </ol>
            </div>
        </a-card>
    </div>
</template>


<script lang="tsx" setup>

import { AddPassLotSummaryReportWithOut } from '@/store/module/PassLotSummaryReport';

import router from '@/router';

// import { Alert, message } from 'ant-design-vue';
// import { schemas } from './form-schema';
import { useForm } from '@/components/core/schema-form';
// import { IconFont } from '@/components/basic/icon-font/index';
import { ref } from '@vue/reactivity';
import { onMounted } from '@vue/runtime-core';
import { cloneDeep } from 'lodash-es';
import { GetCommon } from '@/api/pages/common';

// 導入table和form-schema的數據
function getImport(site: any) {
    const { schemas, optionsdata, getSizeData, getUpdatetime } = require(`@/pages/ReportPortal/${site}/PassLotSummaryReport/LCM1/form-schema`);
    return { schemas, optionsdata, getSizeData, getUpdatetime };
}

const WARNING = ["選擇 Owner 選項時， Product Type 選項無效", "選擇 Summary by Date (Gls/Pnl Combine) 時，顯示 Panel 量。"
    , "選擇 Summary by 早晚 班時，顯示 Panel Move-out 量。", "選擇 Finish Summary 時，只計算 4600、4650及 4651 Move-out 量 (S, WS 及 Repair Grade 不列入計算)，Operation 及Group by 選項無效。"
    , "選擇 Finish-UnFinish Summary 時，數量為Finish 扣除 Unfinish的絕對Finish量。", "選擇 Ship Summary 時，數量為Ship站點的過賬量, Group By的PFCD和Grade為必選項。"]
const { schemas, optionsdata, getSizeData } = getImport('FS');
let UpdateTime = ref("");
// 給form-schema表單的下拉選擇框賦值
onMounted(async () => {
    optionsdata.ownerData = await getSizeData(GetCommon, 'CODE', {
        name: "Date",
        apiName: "Owner",
        FactoryType: "BEOL1",
    });
    optionsdata.operactionData = await getSizeData(GetCommon, 'PROC_ID', {
        name: "Date",
        apiName: "Operation",
        FactoryType: "BEOL1",
    });
    optionsdata.worderTypeData = await getSizeData(GetCommon, 'WORDER_TYPE', {
        name: "Date",
        apiName: "WorderType",
        FactoryType: "BEOL1",
    });
    const { Date: { Table: Data } } = await GetCommon({
        name: "Date",
        apiName: "Update",
        FactoryType: "BEOL1",
    });
    UpdateTime.value = Data[0]["T_STAMP"].substring(0, 19);


})


const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas,
    actionColOptions: {
        span: 24
    },
    fieldMapToTime: [['fieldTime', ['date_f', 'date_t'], 'YYYY-MM-DD']]
});



//定義vuex
let usePassLotSummaryreport = AddPassLotSummaryReportWithOut();
// 確認按鈕
function confirm() {
    dynamicFormRef.validate().then( () => {
        let ALLFormParams = cloneDeep(dynamicFormRef.formModel);
        // 存儲數據到vuex
        usePassLotSummaryreport.AddALLFormParams(ALLFormParams);
        // 跳轉到新頁面
        router.push({ name: 'PassLotSummaryReportDetail' });
    });
}
</script>
<style lang="less" scoped>
.btn-rows {
    button {
        margin-right: 12px;
    }
}
.PassLotSummaryReport :deep(.v-center) {
    background-color: #f0f0f0;
}


    .warning {
        border: #127312 1px solid;
        padding: 15px;
        background-color: rgb(163, 163, 46);
    }


.swapper {
    display: flex;
    flex-direction: column;
}

</style>
