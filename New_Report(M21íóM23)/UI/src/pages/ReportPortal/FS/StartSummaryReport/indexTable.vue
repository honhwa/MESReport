<template>
    <div class="PassLotSummaryReport">
        <a-card>
            <DynamicTable row-key="id" :header-title="PageSetting.TopTitle" :columns="PageSetting.TopColumns"
            showFavorite
                :key="PageSetting.TopColumns" bordered size="small" :data-source="PageSetting.TopList"
                :style="{ display: 'inline-block' }"
                sticky
                :pagination="false" :scroll="{ y: 350 }" export-file-name="导出" :row-class-name="getRowClassNmae">
                <template #export-button>
                    <a-button type="success"> 导出 </a-button>
                </template>
            </DynamicTable>
            <DynamicTable row-key="id" :header-title="PageSetting.BottomTitle" :columns="PageSetting.BottomColumns"
                :key="PageSetting.BottomColumns" bordered size="small" :data-source="PageSetting.BottomList" sticky
                :pagination="false" :scroll="{ y: 350 }" export-file-name="导出" :row-class-name="getRowClassNmae">
                <template #export-button>
                    <a-button type="success"> 导出 </a-button>
                </template>
            </DynamicTable>
        </a-card>
    </div>
</template>


<script lang="tsx" setup>
import { AddStartSummaryReportWithOut } from '@/store/module/StartSummaryReport';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, ref } from '@vue/reactivity';
// import { cloneDeep } from 'lodash-es';
import { GetCommon } from '@/api/pages/common';
import { onMounted } from '@vue/runtime-core';
import { watch } from "vue";
import { message } from 'ant-design-vue';
import type { TableColumn } from '@/components/core/dynamic-table';
import router from '@/router';

import { getRowClassNmae } from '@/pages/ReportPortal/utils/common'

import { AddCommonPageParams } from '@/store/module/commonpage';
// 導入table和form-schema的數據
const UseParamsAdd = AddCommonPageParams();
let FactoryType: any = UseParamsAdd.Fab;
function getImport(site: any) {
    const { StartLotDataSet } = require(`@/pages/ReportPortal/FS/StartSummaryReport/${site}/StartLotDataSet`);
    const { StartLotEQDataSet } = require(`@/pages/ReportPortal/FS/StartSummaryReport/${site}/StartLotEQDataSet`);
    const { StartLotFADataSet } = require(`@/pages/ReportPortal/FS/StartSummaryReport/${site}/StartLotFADataSet`);
    return { StartLotDataSet, StartLotEQDataSet, StartLotFADataSet };
}
const { StartLotDataSet, StartLotEQDataSet, StartLotFADataSet } = getImport(FactoryType);


// 頁面數據
let PageSetting = reactive({
    TopList: [] as any,
    TopTitle: "",
    BottomList: [] as any,
    showmenu: true,
    TopColumns: [] as any,
    BottomColumns: [] as any,
    BottomTitle: "",
    TimeData: [] as any,

});

const [DynamicTable] = useTable();




//從vuex獲取數據
let useStartSummaryReport = AddStartSummaryReportWithOut();
let fromParams = useStartSummaryReport.ALLFormParams;
console.log("fromParams里面存储的值是：", fromParams);


watch(
    () => useStartSummaryReport.ALLFormParams,
    async () => {
        fromParams = useStartSummaryReport.ALLFormParams;
        PageSetting.TopList = [];
        PageSetting.TopColumns = [];
        PageSetting.TimeData = [];
        PageSetting.TopTitle = "";
        PageSetting.BottomTitle = ""
        PageSetting.BottomColumns = [];
        PageSetting.BottomList = [];
        await handlefinaldata();
    },
    {
        deep: true
    }
)


let bottomParams = {
    name: "Date",
    apiName: "StartLotDetail",
    FactoryType: FactoryType,
    SQLPeriod: "",
    SQLCond: ""
}
function TableCustomCell(Label: string[], Prop: string[], ClickColumn: string[] = [], ToOtherpage: Boolean = false) {
    return Prop.map((item: string, index: number) => {
        return {
            title: ClickColumn.includes(item) ? `${Label[index]}` : Label[index],//可点击的栏位显示*号
            align: 'center' as any,
            dataIndex: item,
            //@ts-ignore
            customCell: (record: Object, rowindex: number, column: Object) => {
                if (ClickColumn.includes(item)) {
                    return {
                        onDblclick: async () => {
                            // 是否跳轉到二級頁面
                            if (ToOtherpage) {
                                // console.log("跳转到第二层");
                                // UseParamsAdd.AddFirstTableParams({ ...record, ...column['__originColumn__'] })
                                router.push({ name: 'OperationPerformanceDetail' });
                            } else {
                                switch (fromParams["QueryType"]) {
                                    case "DAY":
                                        if (!item.includes("Sum")) {
                                            PageSetting.BottomTitle = `Start Detail of  ${item}`;
                                            bottomParams.SQLCond = `and PFCD = '${record["PFCD"]}'and work_order = '${record['WORK_ORDER']}' and OWNER_ID = '${record["OWNER_ID"]}'  and OWNER_CATE in ('P', 'X') `
                                            bottomParams.SQLPeriod = `Where CLM_MFDT = '${item}'`
                                        } else {
                                            PageSetting.BottomTitle = `Start Detail of  ${fromParams["date_f"]} to ${fromParams["date_t"]}`;
                                            bottomParams.SQLCond = `and PFCD = '${record["PFCD"]}'and work_order = '${record['WORK_ORDER']}' and OWNER_ID = '${record["OWNER_ID"]}'  and OWNER_CATE in ('P', 'X') `
                                            bottomParams.SQLPeriod = `Where CLM_MFDT between '${fromParams["date_f"]}' and  '${fromParams["date_t"]}' and instr(PFCD, 'SF817') = 0 `;
                                        }
                                        break;
                                    case "EQ":
                                        PageSetting.BottomTitle = `Start Detail of  ${record["CLM_DATE"]} ${record["CLM_TIME"]}`;
                                        if (FactoryType == "BEOL1") {
                                            if (record["CATEGORY"] == "合計") {
                                                bottomParams.SQLCond = `Where CLM_MFDT between '${fromParams["date_f"]}' and '${fromParams["date_t"]}' and instr(PFCD, 'SF817') = 0  and OWNER_CATE in ('P', 'X')  and EQPT_ID = '${PageSetting["TopList"][rowindex - 1]["EQPT_ID"]}'`
                                            } else {
                                                bottomParams.SQLCond = `where clm_stamp = '${record["CLM_DATE"]} ${record["CLM_TIME"]}' and EQPT_ID = '${record['EQPT_ID']}' and GROUP_ID = '${record["PRE_GROUP_ID"]}' and PFCD = '${record["PFCD"]}' and OWNER_ID = '${record["OWNER_ID"]}'`
                                            }
                                        } else {
                                            if (record["CATEGORY"] == "合計") {
                                                bottomParams.SQLCond = `Where CLM_MFDT between '${fromParams["date_f"]}' and '${fromParams["date_t"]}'   and EQPT_ID = '${PageSetting["TopList"][rowindex - 1]["EQPT_ID"]}'`
                                            } else {
                                                bottomParams.SQLCond = ``
                                            }
                                        }
                                        bottomParams.SQLPeriod = ""
                                        break;
                                    case "FA":
                                        PageSetting.BottomTitle = `Start Detail of  ${record["CLM_MFDT"]}`;
                                        if (record["CATEGORY"] == "合計") {
                                            bottomParams.SQLCond = `Where CLM_MFDT between '${fromParams["date_f"]}' and '${fromParams["date_t"]}' and instr(PFCD, 'SF817') = 0  and SRC_SHOP = '${PageSetting["TopList"][rowindex - 1]["SRC_SHOP"]}' and owner_id in ('PROD', 'EXP')`

                                        }
                                        else {
                                            bottomParams.SQLCond = `where CLM_MFDT = '${record["CLM_MFDT"]}' and SRC_SHOP='${record["SRC_SHOP"]}'  and pfcd = '${record["PFCD"]}' and OWNER_ID = '${record["OWNER_ID"]}'`;

                                        }
                                        bottomParams.SQLPeriod = ""
                                        break;
                                    default:
                                        break;
                                }
                                // 请求下表格数据
                                if (bottomParams.SQLCond.length != 0) {
                                    await getbuttomtable(bottomParams);
                                }else{
                                    PageSetting.BottomList = [];
                                    message.warn("无数据！")
                                }
                            }
                        }, style: { cursor: 'pointer' }
                    }
                } else {
                    return {
                        onDblclick: () => {
                            message.warning('此栏位没有二阶数据')
                        }, style: {}
                    }
                }

            }
        }
    })

}
// 表格1最终数据处理
async function handlefinaldata() {
    // 設置title的值
    let tmpProdTypes = ref(fromParams["ProdTypes"]);
    console.log(tmpProdTypes.value);
    const ProdTypesoption = [
        {
            label: "All",
            value: "ALL"
        },
        {
            label: "Prod+E→P+Eng",
            value: "PE"
        },
        {
            label: "Prod+E→P",
            value: "PX"
        },
        {
            label: "Product",
            value: "Prod"
        },
        {
            label: "E→P",
            value: "EXP"
        },
        {
            label: "Engineer",
            value: "Eng"
        },
        {
            label: "Qlot",
            value: "Qlot"
        },
        {
            label: "Dummy",
            value: "Dumy"
        },
    ]
    ProdTypesoption.forEach(item => {
        if (item.value == tmpProdTypes.value) {
            tmpProdTypes.value = item.label;
        }
    })
    PageSetting.TopTitle = "Start Summary of  " + tmpProdTypes.value + ` (From ${fromParams["date_f"]}  To  ${fromParams["date_t"]})`

    switch (fromParams["QueryType"]) {
        case "DAY":
            console.log("执行Day");
            let { ToptableLabels: DAYToptableLabels, ToptableProps: DAYToptableProps, ClickProps } = await StartLotDataSet(PageSetting, fromParams);
            PageSetting.TopColumns = TableCustomCell(DAYToptableLabels, DAYToptableProps, ClickProps);
            break;
        case "EQ":
            console.log("执行EQ");

            let { ToptableLabels: DateToptableLabels, ToptableProps: DateToptableProps, EQcliCkProps } = await StartLotEQDataSet(PageSetting, fromParams);
            PageSetting.TopColumns = TableCustomCell(DateToptableLabels, DateToptableProps, EQcliCkProps);
            break;
        case "FA":
            console.log("执行FA");

            let { ToptableLabels: FAToptableLabels, ToptableProps: FAToptableProps } = await StartLotFADataSet(PageSetting, fromParams);
            PageSetting.TopColumns = TableCustomCell(FAToptableLabels, FAToptableProps, FAToptableProps);
            break;
        default:
            break;

    }



}
// 表格2数据请求以及处理
async function getbuttomtable(bottomParams: Object) {
    let BottomLabels = ["CST ID", "Trans Type", "PFCD", "Work_Order", "Group ID", "Panel ID", "Pnl Cnt", "Owner", "Operation", "EQP ID", "Trans Date", "Trans Time"];
    let BottomProps = ["CASET_ID", "CLM_CATE", "PFCD", "WORK_ORDER", "GROUP_ID", "SHEET_ID", "PANEL_CNT", "OWNER_ID", "PROC_ID", "EQPT_ID", "Trans_Date", "Trans_Time"]
    PageSetting.BottomList = [];
    const bottomBaseColumns: TableColumn[] = TableCustomCell(BottomLabels, BottomProps)
    PageSetting.BottomColumns = bottomBaseColumns;
    const { Date: { Data: { Table: Data } } } = await GetCommon(bottomParams);
    if (Data.length != 0) {
        PageSetting.BottomList = Data;
        PageSetting.BottomList.forEach(item => {
            item["Trans_Date"] = item["CLM_DTTM"].split(" ")[0];
            item["Trans_Time"] = item["CLM_DTTM"].split(" ")[1];
        })
    } else {
        message.warn("无数据！")
    }



}

onMounted(
    async () => {
        await handlefinaldata();
    }
)

</script>


