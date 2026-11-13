<template>
    <div class="WOOrderStatusReport">
        <a-card>
            <a-row>
                <a-col>
                    <DynamicTable row-key="id" :header-title="PageSetting.title" :columns="PageSetting.TopColumns"
                        :key="PageSetting.TopColumns" bordered size="small" :row-class-name="getRowClassNmae" sticky
                        :data-source="PageSetting.TopList" :pagination="false" :scroll="{ x: 3000, y: 500 }"
                        export-file-name="Export">
                        <template #export-button>
                            <a-button type="success"> Export </a-button>
                        </template>
                    </DynamicTable>
                </a-col>
            </a-row>
        </a-card>
    </div>
</template>


<script lang="tsx" setup>
import { useTable } from '@/components/core/dynamic-table';
import { reactive, watchEffect, watch, computed } from 'vue';
//import { reactive, ref } from '@vue/reactivity';
import { useForm } from '@/components/core/schema-form';
import { GetCommon } from '@/api/pages/common';
//import { inject, onMounted, watch } from '@vue/runtime-core';
import { message } from 'ant-design-vue';
import type { TableColumn } from '@/components/core/dynamic-table';
import { cloneDeep } from 'lodash';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { ReportCommon } from '@/store/module/ReportCommon';
import dayjs from "dayjs";
import { getRowClassNmae } from '@/pages/ReportPortal/utils/common'
import { useVirtualList } from '@vueuse/core';
import { useLoading } from '@/components/feat/Loading';

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});

const UseParamsAdd = AddCommonPageParams()
let FactoryType: any = UseParamsAdd.Fab;

let useReportCommon = ReportCommon();

let ReportNameParams = `WOOrderStatusReportDetail_${UseParamsAdd.Fab}`;
let tableParams = useReportCommon[ReportNameParams].FirstTableParams;
let APIMessage = useReportCommon[ReportNameParams].APIMessage;
let formParams = useReportCommon[ReportNameParams].FormParams;	

// watch(
//     () => formParams,
//     async () => {
//         formParams = useReportCommon[ReportNameParams].FormParams;	
//         await getTableData(formParams);
//     },
//     {
//         deep: true,
//     }

// )

function getImport(site: any) {
    const { detailColumns } = require(`@/pages/ReportPortal/TN/WOOrderStatusReport/${site}/table`);
    return { detailColumns };
}
const { detailColumns } = getImport("MOD");

// 頁面數據
let PageSetting = reactive({
    TableSpan: 20,
    showmenu: true,
    TopList: [] as any,
    TopColumns: [] as any,
    Message: {},
    title: "",
    List: [] as any,
    moreData: [],
});
const [DynamicTable] = useTable();

async function getTableData(formParams, tableParams, APIMessage) {
    PageSetting.TopList = [];
    PageSetting.TopColumns = [];
    PageSetting.title = UseParamsAdd.Fab + " 未結案工單 Report"
    openFullLoading()

    const ParamsData: any = {
    name: "Date",
    apiName: "TN_WOOrderStatusDataSetDetail",
    FactoryType: UseParamsAdd.Fab,
    Sqlwhere:APIMessage["sql_Where"],
    WoNbr:tableParams["WO_NBR"],
    DrillCol:tableParams['dataIndex'],
    
   };
   debugger
    const { Date: { Data: { Table: Data },SQL01 } } = await GetCommon(ParamsData); 
    switch (FactoryType) {
      
        default:
            Data.forEach(item => {
                if (item["TRANS_DATE"]) {
                    item["stayhrs"] = (dayjs(dayjs(new Date()).format("YYYY-MM-DD HH:mm:ss")).diff(dayjs(item["TRANS_DATE"].substr(0, 19).replace(/\./g, ":")), 'm') / 60).toFixed(2);
                } else {
                    item["stayhrs"] = 0;
                    item["TRANS_DATE"] = ""
                }
                if (item["ISSUE_DATE"]) {
                    item["starthrs"] = (dayjs(dayjs(new Date()).format("YYYY-MM-DD HH:mm:ss")).diff(dayjs(item["ISSUE_DATE"].substr(0, 19).replace(/\./g, ":")), 'm') / 60).toFixed(2);
                } else {
                    item["starthrs"] = 0;
                    item["ISSUE_DATE"] = ""
                }
            });
            let obj = {};
            Object.keys(Data[0]).forEach(item => {
                if (item == 'PROD_NBR') {
                    obj[item] = "SUM";
                } else if (item == 'VER') {
                    obj[item] = Data.length;
                } else {
                    obj[item] = "";
                }
            })
            if (Data.length != 0) {
                Data.push(obj)
            }
            PageSetting.TopList = Data;
            PageSetting.TopColumns = detailColumns;
            break;
    }

    closeFullLoading();


}
// onMounted(
//     async () => {
//         await getTableData(formParams);
//     }
// )
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

watch(
    () => list.value,
    () => {
        let pushData: any[] = [];
        PageSetting.List = [];
        list.value.forEach((item) => {
            if (item.index < PageSetting.moreData.length) {
                pushData.push(item.data);
            }
        });
        PageSetting.List = pushData;
    }
);
watchEffect(() => {
    //一阶表格监控
    tableParams = useReportCommon[ReportNameParams].FirstTableParams;
    formParams = useReportCommon[ReportNameParams].FormParams;	
    APIMessage= useReportCommon[ReportNameParams].APIMessage;

    getTableData(formParams, tableParams, APIMessage);
});

</script>

<style lang="less" scoped>
.btn-rows {
    button {
        margin-right: 12px;
    }
}

.WOOrderStatusReport :deep(.sub-total) {
    background-color: #ffb468 !important;
}

.WOOrderStatusReport :deep(.total) {
    background-color: #ff80c0 !important;
}

.swapper {
    display: flex;
    flex-direction: column;
}
</style>
