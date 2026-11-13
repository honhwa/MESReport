<script lang="ts" setup>
import { useTable } from '@/components/core/dynamic-table';
import { reactive, ref, watchEffect } from 'vue';
import { GetCommon } from '@/api/pages/common';
import { ReportCommonWithOut } from '@/store/module/ReportCommon';
import { getRowClassNmae, getfinalsum, Inserttotalrow, dclick } from '@/pages/ReportPortal/utils/common';
import { cloneDeep } from 'lodash';
import { message } from 'ant-design-vue';
import { useRouter } from 'vue-router';
const UseParamsAdd = ReportCommonWithOut();
let FactoryType: any = UseParamsAdd.Fab;
const router = useRouter();
let parentId = router.currentRoute.value.meta.parentId;
let SubFab: any = UseParamsAdd.SubFab;
console.log("parentId", parentId);
console.log("SubFab", SubFab);
console.log("UseParamsAdd", UseParamsAdd);

let attrname = ref("");
let importurl = ref("BLU");
let PageSetting = reactive({
    moreData: [] as any,
    FirstList: [] as any,
    SecondList: [] as any,
    ThirdList: [] as any,
    FirstColumns: [] as any,
    SecondColumns: [] as any,
    ThirdColumns: [] as any,
    FirstTitle: 'LCMA Quality Report ',
    SecondTitle: 'LCMA Defect Reason Code Qty ',
    ThirdTitle: 'LCMA Quality Report '
});
switch (FactoryType) {
    case "LCM1":
        importurl.value = FactoryType;
        attrname.value = "QualityReport" + FactoryType;
        break;
    case "LCM2":
        importurl.value = FactoryType;
        attrname.value = "QualityReport" + FactoryType;
        break;
    case "BLU1":
    case "BLU2":
        importurl.value = "BLU";
        attrname.value = "QualityReport" + FactoryType;
        break;
    case "SET1":
        attrname.value = parentId as string;
        if (SubFab == "INT") {
            PageSetting.FirstTitle = "LCMA_TV Quality Report";
            importurl.value = "SET1INT";
        } else {
            PageSetting.FirstTitle = "LCMA_TV Quality Report "
            importurl.value = "SET1";
        }
        break;
    case "SET2":
        importurl.value = "SET2";
        attrname.value = "QualityReport" + FactoryType;
        PageSetting.FirstTitle = "LCM2_TV Quality Report";
        break;
    default:
        break;
}
function getImport(site: any) {
    const {
        detail1_1,
        detail1_2,
        detail1_3,
        datakey, ChildProps
    } = require(`@/pages/ReportPortal/FS/QualityReport/${site}/table`);
    return { detail1_1, detail1_2, detail1_3, datakey, ChildProps };
}
let { detail1_1, detail1_2, detail1_3, datakey, ChildProps } = getImport(importurl.value);

let TableParams = UseParamsAdd[attrname.value].FirstTableParams;

const [DynamicTable] = useTable();
async function thirdtablehandle(record, rowindex, column, ritem, index) {
    PageSetting.ThirdList = [];
    let thirdparams = cloneDeep(TableParams);
    thirdparams["entity_no"] = "'" + record["ENTITY_NO"] + "'";
    thirdparams["ReportType"] = "QualityDetail2";
    let { Date: { Data0: { Table: Data } } } = await GetCommon(thirdparams);
    if (Data.length == 0) {
        return message.warn("無數據！")
    }
    console.log("三階表格數據", Data);

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
    console.log("最終處理的數據ThirdList", PageSetting.ThirdList);
    console.log("最終處理的數據ThirdColumns", PageSetting.ThirdColumns);

}
async function GetDetailData(TableParams: any) {
    PageSetting.FirstColumns = [];
    PageSetting.SecondColumns = [];
    let { Date: { Data0: DataA, Data1: DataB, Message } } = await GetCommon(TableParams);
    let Data0 = DataA["Table"];
    let Data1 = DataB?.Table;
    if (Data0.length == 0) {
        return message.warn("無數據！")
    }
    let ClickColumn = ["PROD_NBR", "ENTITY_NO", "TRANS_DATE", "TRANS_TYPE", "EQUIP_NBR", "OPERATOR", "OUTPUT_FG", "WORK_CTR", "TO_WC", "DFCT_CODE", "PRE_DFCTCODE", "STIMES_TTL", "ITEM_NBR", "PATTERN_NAME", "PASS_QTY"]
    switch (UseParamsAdd.Fab) {
        case "BLU1":
        case "BLU2":
            break;
        case "SET1":
            if (SubFab == "MFG") {
                dclick(detail1_1, ClickColumn, thirdtablehandle);
            }
            break;
        case "SET2":
            dclick(detail1_1, ClickColumn, thirdtablehandle);
            break;
        default:
            dclick(detail1_1, ClickColumn, thirdtablehandle);
            break;
    }
    PageSetting.FirstColumns = detail1_1;
    console.log("輸出PageSetting.FirstColumns", PageSetting.FirstColumns);

    let tempsum = getfinalsum(Data0, ["PASS_QTY"], "PROD_NBR");
    Inserttotalrow(Data0, ["PROD_NBR"], ["PASS_QTY"], "PROD_NBR");
    Data0.push(tempsum);
    PageSetting.FirstList = Data0;
    if (Data1 == null || Data1.length == 0) {
        // return message.warn("表格2無數據！")
        return;
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
    TableParams = UseParamsAdd[attrname.value].FirstTableParams;
    GetDetailData(TableParams);
});

let tableheght = ['BLU1', 'BLU2'].includes(FactoryType) ? "50vh" : "@tabheight"
</script>


<template>
    <div>
        <a-row>
            <a-col>
                <DynamicTable row-key="id" :header-title="PageSetting.FirstTitle" :columns="PageSetting.FirstColumns"
                    :key="PageSetting.FirstColumns" bordered :data-source="PageSetting.FirstList" sticky :flexwidth="1"
                    :data-all="PageSetting.FirstList" :pagination="false" :export-file-name="PageSetting.FirstTitle"
                    :row-class-name="getRowClassNmae">
                    <template #export-button>
                        <a-button type="success"> 导出 </a-button>
                    </template>
                </DynamicTable>
                <DynamicTable row-key="id" :header-title="PageSetting.SecondTitle" :columns="PageSetting.SecondColumns"
                    sticky :key="PageSetting.SecondColumns" bordered :data-source="PageSetting.SecondList" :flexwidth="1"
                    :data-all="PageSetting.SecondList" :pagination="false" :export-file-name="PageSetting.SecondTitle"
                    :row-class-name="getRowClassNmae">
                    <template #export-button>
                        <a-button type="success"> 导出 </a-button>
                    </template>
                </DynamicTable>
                <DynamicTable row-key="id" :header-title="PageSetting.ThirdTitle" :columns="PageSetting.ThirdColumns" sticky
                    :key="PageSetting.ThirdColumns" bordered :data-source="PageSetting.ThirdList"
                    :data-all="PageSetting.ThirdList" v-if="!['BLU1', 'BLU2'].includes(FactoryType)" :flexwidth="1"
                    :pagination="false" :export-file-name="PageSetting.ThirdTitle" :row-class-name="getRowClassNmae">
                    <template #export-button>
                        <a-button type="success"> 导出 </a-button>
                    </template>
                </DynamicTable>
            </a-col>
        </a-row>
    </div>
</template>

<style lang="less" scoped>
@tabheight: v-bind(tableheght);

:deep(.ant-table-wrapper) {
    height: @tabheight !important;
    overflow: auto !important;
}

:deep(.ant-table) {
    height: @tabheight !important;
}

:deep(.ant-table-container) {
    height: @tabheight !important;
}

:deep(.ant-table-wrapper) {
    height: @tabheight !important;
    // overflow: auto;
}

:deep(.ant-table-body) {
    height: @tabheight !important;
    overflow: auto;
}
</style>