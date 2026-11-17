<script lang="ts" setup>
import { useTable } from '@/components/core/dynamic-table';
import { reactive, ref, watchEffect } from 'vue';
import { GetCommon } from '@/api/pages/common';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { ReportCommon } from '@/store/module/ReportCommon';
import { getRowClassNmae, getfinalsum, Inserttotalrow, dclick } from '@/pages/ReportPortal/utils/common';
import { cloneDeep } from 'lodash';
import { message } from 'ant-design-vue';
const UseParamsAdd = AddCommonPageParams();
let useReportCommon = ReportCommon();
let FactoryType: any = UseParamsAdd.Fab;

function getImport(site: any) {
    const {
        detail1_1,
        detail1_2,
        detail1_3,
        datakey, ChildProps
    } = require(`@/pages/ReportPortal/TN/QualityReport/MOD/Table`);
    return { detail1_1, detail1_2, detail1_3, datakey, ChildProps };
}
let { detail1_1, detail1_2, detail1_3, datakey, ChildProps } = getImport("MOD");

let useCommonParams = AddCommonPageParams();
let ReportNameParams = `QualityReportDetail_${useCommonParams.Fab}`;
let TableParams = useReportCommon[ReportNameParams].FirstTableParams;
let APIMessage = JSON.parse(useReportCommon[ReportNameParams].APIMessage);
let formParams = useReportCommon[ReportNameParams].FormParams;	

let PageSetting = reactive({
    moreData: [] as any,
    FirstList: [] as any,
    SecondList: [] as any,
    ThirdList: [] as any,
    FirstColumns: [] as any,
    SecondColumns: [] as any,
    ThirdColumns: [] as any,
    FirstTitle: FactoryType + ' Quality Report ',
    SecondTitle: FactoryType + ' Defect Reason Code Qty ',
    ThirdTitle: FactoryType + ' Quality Report '
});
const [DynamicTable] = useTable();
async function thirdtablehandle(record, rowindex, column, ritem, index) {
    PageSetting.ThirdList = [];
    let thirdparams = cloneDeep(TableParams);
    thirdparams["FactoryType"] = useCommonParams.Fab;
    thirdparams["entity_no"] = "'" + record["ENTITY_NO"] + "'";
    thirdparams["apiName"] = "TN_QualityDataSetDetail2";
    let { Date: { Data0: { Table: Data } } } = await GetCommon(thirdparams);
    debugger
    if (Data.length == 0) {
        return message.warn("無數據！")
    }
    let finalData: object[] = [];
    let obj = {};
    let obj2 = { "WORK_CTR": 0 };
    // 清除不存在于datakey = [1050, 1100, 1200, 1300, 1400, 1500, 1600, 1700, 1900];
    Data = Data.filter(item => {
        return datakey.includes(item["WORK_CTR"])
    })

    for (let i = 0; i < Data.length; i++) {
        if (Data[i]["PROD_NBR"].trim() != '') {
            ChildProps.forEach(item => {
                Data[i][`${Data[i]["WORK_CTR"]}-${item}`] = Data[i][item]
            });
            // 如果和并到最后发现，obj的workctr比下一个大或者等于，说明走了一轮
            if (obj2["WORK_CTR"] >= Data[i]["WORK_CTR"]) {
                finalData.push(obj2);
                obj2 = Data[i];
            } else {
                obj2 = Object.assign(obj2, Data[i]);
            }
        }
    }
    finalData.push(obj2);
    let tempobj = {};
    finalData.forEach(item => {
        let work = item["WORK_CTR"];
        if (!tempobj[work]) {
            tempobj[work] = 0;
        } tempobj[work]++;

    })
    let arr: number[] = [];
    // 数组排序
    for (let i in tempobj) {
        arr.push(tempobj[i])
    }
    arr.sort((a, b) => {
        return a - b
    })
    let maxcount = 0;
    if (arr[arr.length - 1] > 10) {
        maxcount = arr[arr.length - 2]
    } if (arr.length == 2 && arr[0] == 1 && arr[1] == 1) {
        maxcount = finalData.length;
    } if (arr.length == 3 && arr[0] == 1 && arr[1] == 1 && arr[2] == 1) {
        maxcount = 2;
    } else {
        maxcount = finalData.length;
    }
    PageSetting.ThirdColumns = detail1_3;
    PageSetting.ThirdList = finalData.slice(0, maxcount);
}
async function GetDetailData(TableParams: any,  APIMessage: any, formParams: any) {
    PageSetting.FirstColumns = [];
    PageSetting.SecondColumns = [];
    debugger
    //let { Date: { Data0: DataA, Data1: DataB, Message } } = await GetCommon(TableParams);
    const ParamsData: any = {
        name: "Date",
        apiName: "TN_QualityDataSetDetail",
        FactoryType: useCommonParams.Fab,
        G_FAC: useCommonParams.G_FAC_COM,
        Sqlwhere:APIMessage["SQLFilter"],
        Sqlwhere_Defect:APIMessage["SQLFilter_Defect"],
        SelectItem:TableParams.defcond1,
        SelectOper:TableParams.opercond,
     };

     const { Date: {
            DataA: { Table: DataA },
            DataB: { Table: DataB },
            SQLFilter
        } } = await GetCommon(ParamsData);
debugger
    let Data0 = DataA;
    let Data1 = DataB;
    if (Data0.length == 0) {
        return message.warn("無數據！")
    }
    let ClickColumn = ["PROD_NBR", "ENTITY_NO", "TRANS_DATE", "TRANS_TYPE", "EQUIP_NBR", "OPERATOR", "OUTPUT_FG", "WORK_CTR", "TO_WC", "DFCT_CODE", "PRE_DFCTCODE", "WO_NBR", "PC_CUST", "STIMES_TTL", "ITEM_NBR", "PASS_QTY"]
    switch (UseParamsAdd.Fab) {
  
        default:
            dclick(detail1_1, ClickColumn, thirdtablehandle);
            break;
    }
    PageSetting.FirstColumns = detail1_1;
    let tempsum = getfinalsum(Data0, ["PASS_QTY"], "PROD_NBR");
    Inserttotalrow(Data0, ["PROD_NBR"], ["PASS_QTY"], "PROD_NBR");
    Data0.push(tempsum);
    PageSetting.FirstList = Data0;
    if (Data1 == null || Data1.length == 0) {
        return message.warn("表格2無數據！")
    }
    Data1.forEach(item => {
        item["Rate"] = ((item["QTY"] / item["TOTAL"]) * 100).toFixed(2);
    })
    PageSetting.SecondColumns = detail1_2;
    let tempsum2 = getfinalsum(Data1, ["QTY"], "DFCT_CODE");
    Inserttotalrow(Data1, ["DFCT_CODE"], ["QTY"], "DFCT_CODE");
    Data1.push(tempsum2);
    PageSetting.SecondList = Data1;

}
watchEffect(() => {
    //一阶表格监控
    //TableParams = useCommonParams.FirstTableParams;
    TableParams = useReportCommon[ReportNameParams].FirstTableParams;
    APIMessage = JSON.parse(useReportCommon[ReportNameParams].APIMessage);
    formParams = useReportCommon[ReportNameParams].FormParams;
    debugger
    GetDetailData(TableParams, APIMessage, formParams);
});
</script>


<template>
    <div>
        <a-card>
            <DynamicTable row-key="id" :header-title="PageSetting.FirstTitle" :columns="PageSetting.FirstColumns"
                :key="PageSetting.FirstColumns" bordered size="small" :data-source="PageSetting.FirstList"
                sticky
                :pagination="false" :export-file-name="PageSetting.FirstTitle" :scroll="{ x: 2500, y: 200 }"
                :row-class-name="getRowClassNmae">
                <template #export-button>
                    <a-button type="success"> Export </a-button>
                </template>
            </DynamicTable>
            <DynamicTable row-key="id" :header-title="PageSetting.SecondTitle" :columns="PageSetting.SecondColumns"
            sticky
                :key="PageSetting.SecondColumns" bordered size="small" :data-source="PageSetting.SecondList"
                :pagination="false" :export-file-name="PageSetting.SecondTitle" :scroll="{ x: 600, y: 200 }"
                :row-class-name="getRowClassNmae">
                <template #export-button>
                    <a-button type="success"> Export </a-button>
                </template>
            </DynamicTable>
            <DynamicTable row-key="id" :header-title="PageSetting.ThirdTitle" :columns="PageSetting.ThirdColumns"
            sticky
                :key="PageSetting.ThirdColumns" bordered size="small" :data-source="PageSetting.ThirdList"
        
                :pagination="false" :export-file-name="PageSetting.ThirdTitle" :scroll="{ x: 5000, y: 200 }"
                :row-class-name="getRowClassNmae">
                <template #export-button>
                    <a-button type="success"> Export </a-button>
                </template>
            </DynamicTable>
        </a-card>
    </div>
</template>