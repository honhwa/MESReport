<template>
    <div class="WOOrderStatusReport">
        <a-card>
            <a-row>
                <a-col>
                    <DynamicTable row-key="id" :header-title="PageSetting.title" :columns="PageSetting.TopColumns"
                        :key="PageSetting.TopColumns" bordered :row-class-name="getRowClassNmae" sticky
                        :data-source="PageSetting.TopList" :pagination="false" :data-all="PageSetting.TopList"
                        :flexwidth="1" export-file-name="导出">
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
import { reactive, ref } from '@vue/reactivity';
import { useForm } from '@/components/core/schema-form';
import { GetCommon } from '@/api/pages/common';
import { inject, onMounted, watch, watchEffect } from '@vue/runtime-core';
import { message } from 'ant-design-vue';
import type { TableColumn } from '@/components/core/dynamic-table';
import { cloneDeep } from 'lodash';
import { ReportCommonWithOut } from '@/store/module/ReportCommon';
import { WoOrderStatusReportWithOut } from '@/store/module/WoOrderStatusReport';
import dayjs from "dayjs";
import { getRowClassNmae } from '@/pages/ReportPortal/utils/common'


const ReportCommon = ReportCommonWithOut()
let FactoryType: any = ReportCommon.Fab;

let ALLFormParams = ReportCommon["WOOrderStatusReport" + FactoryType]["FirstTableParams"];
watchEffect(async () => {
    ALLFormParams = ReportCommon["WOOrderStatusReport" + FactoryType]["FirstTableParams"];
    await getTableData();
})




function getImport(site: any) {
    const { detailColumns } = require(`@/pages/ReportPortal/FS/WOOrderStatusReport/${site}/table`);
    return { detailColumns };
}
const { detailColumns } = getImport(FactoryType);

// 頁面數據
let PageSetting = reactive({
    TableSpan: 20,
    showmenu: true,
    TopList: [] as any,
    TopColumns: [] as any,
    Message: {},
    title: ""


});
const [DynamicTable] = useTable();

async function getTableData() {
    PageSetting.TopList = [];
    PageSetting.TopColumns = [];
    switch (ALLFormParams["QueryRange"]) {
        case "":
            PageSetting.title = "LCMA 大於60天未結案工單 Report"
            break;
        case "BEOL1":
            PageSetting.title = "LCD 未結案工單 Report"
            break;
        case "BEOL2":
            PageSetting.title = "LCDB 未結案工單 Report"
            break;
        case "SET2":
            PageSetting.title = "LCM2_TV 未結案工單 Report"
            break;
        case "SET1":
            PageSetting.title = "LCMA_TV 未結案工單 Report"
            break;
        default:
            PageSetting.title = "LCMA 未結案工單 Report"
            break;
    }
    const { Date: { Data0: { Table: Data }, Message: sqlmessage } } = await GetCommon(ALLFormParams);
    switch (FactoryType) {
        case "BEOL1":
        case "BEOL2":
            let tempobj = { WORK_ORDER_ID: ALLFormParams["row0"] };
            // 屬性
            let props: number[] = [];
            let columns: object[] = [
                {
                    title: "Work Order ID",
                    align: 'center',
                    dataIndex: "WORK_ORDER_ID",
                    key: "WORK_ORDER_ID",
                }
            ];
            Data.forEach(item => {
                item[item["WORK_CTR"]] = item["QTY"];
                tempobj = { ...tempobj, ...item };
                if (!props.includes(item["WORK_CTR"])) {
                    columns.push({
                        title: item["WORK_CTR"],
                        align: 'center',
                        dataIndex: item["WORK_CTR"],
                        key: item["WORK_CTR"],
                    })
                }
            })
            PageSetting.TopList = [tempobj];
            PageSetting.TopColumns = columns;
            break;
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
