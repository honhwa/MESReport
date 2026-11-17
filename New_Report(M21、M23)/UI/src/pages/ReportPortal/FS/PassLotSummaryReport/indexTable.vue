<template>
    <div class="PassLotSummaryReport">
        <a-card>
            <DynamicTable row-key="id" :header-title="PageSetting.TopTitle" :columns="PageSetting.TopColumns" sticky
            showFavorite
            :style="{ display: 'inline-block' }"

                :key="PageSetting.TopColumns" bordered size="small" :data-source="PageSetting.TopList"
                :pagination="false" :scroll="{ y: 350 }" export-file-name="导出" :row-class-name="getRowClassNmae">
                <template #export-button>
                    <a-button type="success"> 导出 </a-button>
                </template>
            </DynamicTable>
            <DynamicTable row-key="id" :header-title="PageSetting.BottomTitle" :columns="PageSetting.BottomColumns" sticky
                :key="PageSetting.BottomColumns" bordered size="small" :data-source="PageSetting.BottomList"
                :pagination="false" :scroll="{ y: 350 }" export-file-name="导出" :row-class-name="getRowClassNmae">
                <template #export-button>
                    <a-button type="success"> 导出 </a-button>
                </template>
            </DynamicTable>
        </a-card>
    </div>
</template>


<script lang="tsx" setup>
import { AddPassLotSummaryReport } from '@/store/module/PassLotSummaryReport';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, ref } from '@vue/reactivity';
// import { cloneDeep } from 'lodash-es';
import { GetCommon } from '@/api/pages/common';
import { onMounted } from '@vue/runtime-core';
import { watch } from "vue";
import { message } from 'ant-design-vue';
import type { TableColumn } from '@/components/core/dynamic-table';
import router from '@/router';

import { PassLotShiftDataSet_SHIP } from './LCM1/PassLotShiftDataSet_SHIP'
import { PassLotShiftDataSet } from './LCM1/PassLotShiftDataSet'
import { getRowClassNmae } from '@/pages/ReportPortal/utils/common'
import { PassLotShiftDataSet_Finish_Un } from './LCM1/PassLotShiftDataSet_Finish-Un'
import { PassLotShiftDataSet_ABCD } from './LCM1/PassLotShiftDataSet_ABCD'
import { PassLotShiftDataSet_Finish } from './LCM1/PassLotShiftDataSet_Finish'
import { PassLotShiftDataSet_Date } from './LCM1/PassLotShiftDataSet_Date'



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
let usePassLotSummaryreport = AddPassLotSummaryReport();
let fromParams = usePassLotSummaryreport.ALLFormParams;


watch(
    () => usePassLotSummaryreport.ALLFormParams,
    async () => {
        fromParams = usePassLotSummaryreport.ALLFormParams;
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
    apiName: "PassLotShiftDetail",
    FactoryType: "BEOL1",
    Owner: fromParams["Owner"],
    SQLCond: "",
    SQLCond2: "",
    qty: 0,
    s_per: "",
    SQLPeriod: ""
}
function TableCustomCell(Label: string[], Prop: string[], ClickColumn: string[] = [], ToOtherpage: Boolean = false) {
    return Prop.map((item: string, index: number) => {
        return {
            title: ClickColumn.includes(item) ? `${Label[index]}*` : Label[index],//可点击的栏位显示*号
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
                                PageSetting.BottomTitle = `Pass Lot Detail of row ${rowindex}, col ${index}`;
                                let tempSQLCond = "";
                                fromParams["Groups"].forEach(groupitem => {
                                    let tempgroupitem = groupitem
                                    if (groupitem == "EQPT_ID") {
                                        tempgroupitem = "CR_EQPT_ID"
                                    }
                                    tempSQLCond += ` and ${tempgroupitem} = '${record[groupitem].replace(/\s*/g, "")}'  `
                                })
                                bottomParams.SQLCond = tempSQLCond + `  and OWNER_CATE in ('P', 'X')`;
                                bottomParams.SQLCond2 = bottomParams.SQLCond;
                                /* if(isNaN(record[item])){
                                    record[item] = 0;
                                } */
                                bottomParams.qty = isNaN(record[item]) ? 0 : Number(record[item]);
                                bottomParams.s_per = record["MFGDATE"];
                                let type = "";
                                if (column['__originColumn__']["title"].indexOf("(日)") != -1 || column['__originColumn__']["title"].indexOf("(早)") != -1) {
                                    let temp = "";
                                    if (column['__originColumn__']["title"].indexOf("movein") != -1) {
                                        temp = "LOGON_STAMP";
                                        type = "LOGN";

                                    } else {
                                        temp = "LOGOF_STAMP";
                                        type = "LOGOF";

                                    }
                                    bottomParams.SQLPeriod = `Where ${temp} Between  '${record["MFGDATE"]} 07:30:00.000000' and  '${record["MFGDATE"]} 19:29:59.000000' and instr(PFCD, 'SF817') = 0 `;
                                } else if (column['__originColumn__']["title"].indexOf("(夜)") != -1 || column['__originColumn__']["title"].indexOf("(晚)") != -1) {
                                    let temp = "";
                                    if (column['__originColumn__']["title"].indexOf("movein") != -1) {
                                        temp = "LOGON_STAMP";
                                        type = "LOGN";
                                    } else {
                                        temp = "LOGOF_STAMP"
                                        type = "LOGOF";
                                    }
                                    bottomParams.SQLPeriod = `Where ${temp} Between  '${record["MFGDATE"]} 19:30:00.000000' and  '${record["MFGDATE"]} 07:29:59.000000' and instr(PFCD, 'SF817') = 0 `;

                                }
                                switch (fromParams["Rep"]) {
                                    case "DAY":
                                        break;
                                    case "Date":
                                        break;
                                    case "ABCD":
                                        bottomParams.qty = 0;
                                        // bottomParams.s_per = "";
                                        bottomParams.SQLCond2 = ""
                                        break;
                                    default:
                                        break;
                                }

                                // 请求下表格数据
                                await getbuttomtable(bottomParams, type);
                                // usePassLotSummaryreport.AddFirstTableParams({ ...record, ...column['__originColumn__'] })

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
    switch (fromParams["ProdTypes"]) {
        case "PE":
            tmpProdTypes.value = "Prod+E→P+Eng"
            break;
        case "PX":
            tmpProdTypes.value = "Prod+E→P"
            break;
        case "EXP":
            tmpProdTypes.value = "E→P"
            break;
        default:
            break;
    }
    switch (fromParams["Rep"]) {
        case "DAY":
            PageSetting.TopTitle = "LCD Pass Lot Summary of " + tmpProdTypes.value + ` (From ${fromParams["date_f"]}  To  ${fromParams["date_t"]})`
            let { ToptableLabels: DAYToptableLabels, ToptableProps: DAYToptableProps, middleProps } = await PassLotShiftDataSet(PageSetting, fromParams);
            PageSetting.TopColumns = TableCustomCell(DAYToptableLabels, DAYToptableProps, middleProps);
            break;
        case "Date":
            PageSetting.TopTitle = "Pass Lot Summary of " + tmpProdTypes.value + ` (From ${fromParams["date_f"]}  To  ${fromParams["date_t"]})`
            let { ToptableLabels: DateToptableLabels, ToptableProps: DateToptableProps, middleProps: DatemiddleProps } = await PassLotShiftDataSet_Date(PageSetting, fromParams);
            PageSetting.TopColumns = TableCustomCell(DateToptableLabels, DateToptableProps, DatemiddleProps);
            break;
        case "ABCD":
            PageSetting.TopTitle = "LCD5 Pass Lot Summary of " + tmpProdTypes.value + ` (From ${fromParams["date_f"]}  To  ${fromParams["date_t"]})`
            let { ToptableLabels: ABCDToptableLabels, ToptableProps: ABCDToptableProps, middleProps: ABCDmiddleProps } = await PassLotShiftDataSet_ABCD(PageSetting, fromParams);
            PageSetting.TopColumns = TableCustomCell(ABCDToptableLabels, ABCDToptableProps, ABCDmiddleProps);

            break;
        case "Finish":
            PageSetting.TopTitle = "LCD Finish Summary by Date of " + tmpProdTypes.value + ` (From ${fromParams["date_f"]}  To  ${fromParams["date_t"]})`
            let { ToptableLabels: FinishToptableLabels, ToptableProps: FinishToptableProps ,bottomLabels,bottomProps,bottomDate} = await PassLotShiftDataSet_Finish(PageSetting, fromParams);
            
            PageSetting.TopColumns = TableCustomCell(FinishToptableLabels, FinishToptableProps);
            PageSetting.BottomTitle = PageSetting.TopTitle;
            PageSetting.BottomColumns = TableCustomCell(bottomLabels,bottomProps);
            PageSetting.BottomList = bottomDate;
            break;
        case "Finish-UnFinish":
            PageSetting.TopTitle = "LCD Finish Summary by Date of " + tmpProdTypes.value + ` (From ${fromParams["date_f"]}  To  ${fromParams["date_t"]})`
            let { ToptableLabels, ToptableProps } = await PassLotShiftDataSet_Finish_Un(PageSetting, fromParams);
            PageSetting.TopColumns = TableCustomCell(ToptableLabels, ToptableProps);
            break;
        case "SHIP":
            PageSetting.TopTitle = "LCD Finish Summary by Date of" + tmpProdTypes.value + ` (From ${fromParams["date_f"]}  To  ${fromParams["date_t"]})`;
            let { ToptableLabels: SHIPToptableLabels, ToptableProps: SHIPToptableProps } = await PassLotShiftDataSet_SHIP(PageSetting, fromParams);
            PageSetting.TopColumns = TableCustomCell(SHIPToptableLabels, SHIPToptableProps);
            break;
        default:
            break;

    }



}
// 表格2数据请求以及处理
async function getbuttomtable(bottomParams: Object, item: string) {

    let BottomLabels = ["CST ID", "Lofon Slot No", "Logof Slot No", "Panel ID", "clm_cate", "Operation", "Group ID", "PID", "Panel Cnt", "Owner", "Priority", "QTap LotID", "Move_in Time", "In_Opr ID", "Move_out Time", "Out_Opr ID", "Grade", "Grade Group", "OX Info", "Work_Order"];
    let AllandmoveinBottomLabels = ["CST ID", "Lofon Slot No", "Logof Slot No", "Panel ID", "clm_cate", "Operation", "Group ID", "PID", "Panel Cnt", "Owner", "Priority", "QTap LotID", "Move_in Time", "In_Opr ID", "Move_out Time", "Out_Opr ID", `${item} Grade`, "Grade Group", "OX Info", "Work_Order"]
    let AllandmoveinBottomProps = ["LOGOF_CASET_ID", "LOGON_SLOT_NO", "LOGOF_SLOT_NO", "SHEET_ID", "CLM_CATE", "PROC_ID", "GROUP_ID", "PID", "LOGOF_PANEL_CNT", "OWNER_ID", "PRIORITY", "QTAP_LOTID", "LOGON_STAMP", "LOGON_USER", "LOGOF_STAMP", "LOGOF_USER", "GRADE", "GRADE_GROUP", "PNL_GRADE", "WORK_ORDER"];
    let BottomProps: any[] = [];
    BottomProps = AllandmoveinBottomProps;
    BottomLabels = AllandmoveinBottomLabels;

    PageSetting.BottomList = [];
    const bottomBaseColumns: TableColumn[] = TableCustomCell(BottomLabels, BottomProps)
    PageSetting.BottomColumns = bottomBaseColumns;
    const { Date: { Data: { Table: Data } } } = await GetCommon(bottomParams);
    PageSetting.BottomList = Data;


}

onMounted(
    async () => {
        await handlefinaldata();
    }
)

</script>


