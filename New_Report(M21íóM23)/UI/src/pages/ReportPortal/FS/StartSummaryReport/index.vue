<template>
    <div class="StartSummaryReport">
        <a-card>
            <SchemaForm @submit="confirm"> </SchemaForm>
            <span>Update:{{ UpdateTime }}</span>
        </a-card>
        <div class="desciprtion" v-if="FactoryType == 'BEOL2'">
            <span>邏輯說明：</span>
            <ul>
                <li>1. 選Summary By Effective Move-in 查每日投入量，Summary By Move-in/Cancel Move-in 一樣的是查投入，show出cancel </li>
                <li>2. 切割站點的投入抓第一次move out量，偏貼不變，抓move in </li>
                <li>2. 切割站點的投入抓第一次move out量，偏貼不變，抓move in </li>
            </ul>
        </div>

    </div>
</template>


<script lang="tsx" setup>

import { AddStartSummaryReportWithOut } from '@/store/module/StartSummaryReport';

import router from '@/router';

import { useForm } from '@/components/core/schema-form';
import { ref } from '@vue/reactivity';
import { onMounted } from '@vue/runtime-core';
import { cloneDeep } from 'lodash-es';
import { GetCommon } from '@/api/pages/common';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import { AddCommonPageParams } from '@/store/module/commonpage';



// 導入table和form-schema的數據
const UseParamsAdd = AddCommonPageParams();
let FactoryType: any = UseParamsAdd.Fab;

function getImport(site: any) {
    const { schemas, optionsdata, getSizeData, getUpdatetime } = require(`@/pages/ReportPortal/FS/StartSummaryReport/${site}/form-schema`);
    return { schemas, optionsdata, getSizeData, getUpdatetime };
}

const { schemas, optionsdata, getSizeData } = getImport(FactoryType);
let UpdateTime = ref("");
const { InfiniteScroll, mountedowner, mountedOper } = useRemoteSelect()
// 給form-schema表單的下拉選擇框賦值
onMounted(async () => {
    if (FactoryType == "BEOL2") {
        await mountedowner(optionsdata, FactoryType);
        await mountedOper(optionsdata, FactoryType);
        optionsdata.worder_cateData = await getSizeData(GetCommon, 'WORDER_CATE', {
            name: "Date",
            apiName: "WorderType",
            FactoryType: FactoryType,
        });
    } else {
        optionsdata.worderTypeData = await getSizeData(GetCommon, 'WORDER_TYPE', {
            name: "Date",
            apiName: "WorderType",
            FactoryType: FactoryType,
        });
    }
    const { Date: { Table: Data } } = await GetCommon({
        name: "Date",
        apiName: "Update",
        FactoryType: FactoryType,
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
let useStartSummaryReport = AddStartSummaryReportWithOut();
// 確認按鈕
function confirm() {
    dynamicFormRef.validate().then(() => {
        let ALLFormParams = cloneDeep(dynamicFormRef.formModel);
        // 存儲數據到vuex
        useStartSummaryReport.AddALLFormParams(ALLFormParams);
        // 跳轉到新頁面
        router.push({ name: `StartSummaryReportDetail${FactoryType}`});
    });
}
</script>
<style scoped lang="less">
.desciprtion {
    background-color: #f0e68c;
    ul>{
        margin: 10px;
    }
}
</style>

