<template>
    <div class="OutputByGradeandDownGradeRatio">
        <a-row>
            <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                <SchemaForm @submit="confirm"> </SchemaForm>
            </a-col>
            <a-col :span="PageParams.TableSpan">
                <div class="table">
                    <DynamicTable row-key="id" header-title="LCMA Output By Grade" showFavorite :flexwidth="8"
                        :columns="PageSetting.TopColumns" sticky :PageParams="PageParams" :key="PageSetting.TopColumns"
                        bordered :data-source="PageSetting.TopList" :data-all="PageSetting.TopList" :pagination="false"
                        export-file-name="导出" :row-class-name="getRowClassNmae">
                        <template #export-button>
                            <a-button type="success"> 导出 </a-button>
                        </template>
                    </DynamicTable>
                </div>

            </a-col>
        </a-row>
    </div>
</template>


<script lang="tsx" setup>
import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table'
import { useTable } from '@/components/core/dynamic-table';
import { reactive } from '@vue/reactivity';
import { useForm } from '@/components/core/schema-form';
import { GetCommon } from '@/api/pages/common';
import { getrowsum, sortArray, TableCustomCell1, getRowClassNmae, getfinalsum } from "@/pages/ReportPortal/utils/common";
import { cloneDeep } from 'lodash';
import { useLoading } from '@/components/feat/Loading';
import { getPovitData, PovitParams } from '@/utils/reportcommon/provit';
import router from '@/router';
import { message } from 'ant-design-vue';

import { ReportCommonWithOut } from "@/store/module/ReportCommon";
import { type } from 'os';
const ReportCommonParams = ReportCommonWithOut();

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});
let FactoryType: string = ReportCommonParams.Fab;

function getImport(site: string) {
    const { schemas } = require(`@/pages/ReportPortal/FS/OutputByGradeandDownGradeRatio/${site}/form-schema`);
    const { baseColumns } = require(`@/pages/ReportPortal/FS/OutputByGradeandDownGradeRatio/${site}/table`);
    return { schemas, baseColumns };
}
const { schemas, baseColumns } = getImport(FactoryType);

// 頁面數據
let PageSetting = reactive({
    TopList: [] as any,
    TopColumns: [] as any,
    Message: {},
});
const [DynamicTable] = useTable();


const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas: schemas(FactoryType),
    actionColOptions: {
        span: 24
    },
    fieldMapToTime: [['fieldTime', ['vDate_s', 'vDate_e1'], 'YYYY-MM-DD']],
    layout: "vertical"
});

let TopTableParams = reactive({
    name: "Date",
    apiName: "OutputByGradeDownGrade",
    ReportType: "OutputByGradeDataSet",
    FactoryType: FactoryType,
    G_FAC: "A",
    FromDate: "",
    ToDate: "",
    Shift: "",
    Interval: "",
    Floor: "",
    QueryMethod: "",
    ProductType: "",
    LCMProductType: "",
    WorkOrderType: [],
    WorkOrderInformation: [],
    BigProduct: "ALL",
    Product: [],
    LCMOwner: [],
    LCDGrade: [],
    LCMGrade: [],
    PalletID: [],
});

function Inserttotalrow(dataSource: any[], standard: any[], calcuList: any[], sumprops: string, sumname: string = "合計") {
    let temp: any = cloneDeep(dataSource[0]);
    let totalsum: any[] = [];
    let totalindex: any[] = [];
    let temp2: any = cloneDeep(dataSource[0]);
    let totalsum2: any[] = [];
    let totalindex2: any[] = [];
    let regArr: any[] = [];
    let DownGrade: Object[] = [];
    dataSource.forEach(item => {
        if (item["DOWNGRADE"].includes(item["GRADE"])) {
            DownGrade.push(item)
        }
    })


    for (let i = 0; i < dataSource.length - 1; i++) {
        standard.forEach(item => {
            regArr.push(dataSource[i][item] == dataSource[i + 1][item])
        })
        if (regArr.includes(false)) {
            regArr = [];
            Object.keys(temp).forEach((item) => {
                if (!calcuList.includes(item)) {
                    temp[item] = '';
                }
                if (item == sumprops) {
                    temp[item] = sumname;
                }
            });

            totalindex.push(i + 1);
            totalsum.push(temp);
            temp = cloneDeep(dataSource[i + 1]);
            continue;
        } else {
            for (let j = 0; j < calcuList.length; j++) {
                if (isNaN(temp[calcuList[j]])) {
                    temp[calcuList[j]] = 0;
                } if (!isNaN(dataSource[i + 1][calcuList[j]])) {
                    temp[calcuList[j]] += dataSource[i + 1][calcuList[j]];
                }
            }
        }
    }

    // 由於comtimunu的原因需要兩個循環
    for (let i = 0; i < dataSource.length - 1; i++) {
        // 計算product id截取前5位相同的相加
        if (dataSource[i]["PROD_NBR"].slice(0, 5) === dataSource[i + 1]["PROD_NBR"].slice(0, 5)) {
            for (let j = 0; j < calcuList.length; j++) {
                if (isNaN(temp2[calcuList[j]])) {
                    temp2[calcuList[j]] = 0;
                } if (!isNaN(dataSource[i + 1][calcuList[j]])) {
                    temp2[calcuList[j]] += dataSource[i + 1][calcuList[j]];
                }
            }
        } else {
            Object.keys(temp2).forEach((item) => {
                if (!calcuList.includes(item) && item != "PROD_NBR") {
                    temp2[item] = '';
                }
                if (item == "PROD_NBR") {
                    temp2[item] = `${temp2["PROD_NBR"].slice(0, 5)} Sub Sum`;
                }
                if (item == sumprops) {
                    temp[item] = sumname;
                }
            });

            totalindex2.push(i + 1);
            totalsum2.push(temp2);
            temp2 = cloneDeep(dataSource[i + 1]);
            continue;
        }
    }
    Object.keys(temp).forEach((item) => {
        if (!calcuList.includes(item)) {
            temp[item] = '';
        }

        if (item == "PROD_NBR") {
            temp[item] = temp[item];
        }
        if (item == sumprops) {
            temp[item] = sumname;
        }
    });
    totalsum.push(temp);
    totalindex.push(dataSource.length);
    Object.keys(temp2).forEach((item) => {
        if (!calcuList.includes(item) && item != "PROD_NBR") {
            temp2[item] = '';
        }
        if (item == "PROD_NBR") {
            temp2[item] = `${temp2["PROD_NBR"].slice(0, 5)} Sub Sum`;
        }
    });
    totalsum2.push(temp2);
    totalindex2.push(dataSource.length);
    // 保存插入了多少行
    let temptotalindex = cloneDeep(totalindex2);
    for (let i = 0; i < totalindex.length; i++) {
        // 直接保存插入後的索引
        dataSource.splice(totalindex[i] + i, 0, totalsum[i]);
        for (let j = 0; j < totalindex2.length; j++) {

            if (totalindex2[j] == totalindex[i]) {
                temptotalindex[j] = totalindex[i] + i + 1;
            }
        }
    }
    for (let i = 0; i < temptotalindex.length; i++) {
        dataSource.splice(temptotalindex[i] + i, 0, totalsum2[i]);
    }



    // 最后四行计算
    // 第一行
    // 第二行
    // 第三行
    // 第四行
    /*  console.log("輸出calcuList",calcuList);
     console.log("輸出DownGrade",DownGrade);
     console.log("輸出totalsum",totalsum); */

    let DownGradeQty = getfinalsum(DownGrade, calcuList, "PROD_NBR", "DownGrade Qty(A)");
    let TotalQty = getfinalsum(totalsum, calcuList, "PROD_NBR", "Total Qty(B)");
    let DownGradeQtyRate = { PROD_NBR: "DownGrade率%[A/B*100%]" };
    let TotalQtyRate = { PROD_NBR: "可出貨率[(1-A/B)100%]" };
    console.log("輸出DownGradeQty", DownGradeQty);
    console.log("輸出TotalQty", TotalQty);
    console.log("輸出calcuList", calcuList);

    calcuList.forEach(item => {
        if (DownGradeQty && DownGradeQty.hasOwnProperty(item)) {
            DownGradeQtyRate[item] = ((DownGradeQty[item] / TotalQty[item]) * 100).toFixed(2);
        }else{
            DownGradeQtyRate[item] = 0;
        }

        TotalQtyRate[item] = 100 - DownGradeQtyRate[item];
        DownGradeQtyRate[item] = DownGradeQtyRate[item] + "%";
        TotalQtyRate[item] = TotalQtyRate[item] + "%";
    });
    Object.keys(dataSource[0]).forEach(item=>{
        if(typeof DownGradeQty == "undefined"){
            DownGradeQty = {PROD_NBR:"DownGrade Qty(A)"}
        }
        if(!DownGradeQty.hasOwnProperty(item)){
            DownGradeQty[item] = ""
        }
        if(typeof TotalQty == "undefined"){
            TotalQty = {PROD_NBR:"Total Qty(B)"}
        }
        if(!TotalQty.hasOwnProperty(item)){
            TotalQty[item] = ""
        }
        if(!DownGradeQtyRate.hasOwnProperty(item)){
            DownGradeQtyRate[item] = ""
        }
        if(!TotalQtyRate.hasOwnProperty(item)){
            TotalQtyRate[item] = ""
        }
    })
    dataSource.push(DownGradeQty, TotalQty, DownGradeQtyRate, TotalQtyRate);
    return {
        insertrowList: totalsum,
        insertIndexList: totalindex
    };
}
async function confirm() {
    openFullLoading();
    try {
        //給ParamsData賦值 
        let titems = Object.keys(TopTableParams);
        let tempdynamicFormRef = cloneDeep(dynamicFormRef.formModel)
        for (let i in tempdynamicFormRef) {
            if (titems.includes(i)) {
                if (i == "PalletID") {
                    TopTableParams[i] = tempdynamicFormRef[i].split(/[\r\n\t]/).filter(item => item)
                }
                TopTableParams[i] = tempdynamicFormRef[i];
            }
        }
        const { Date: { Data0: { Table: Data0 }, Data1: { Table: Data1 } } } = await GetCommon(TopTableParams);
        if (Data1.length == 0) {
            PageSetting.TopList = [];
            PageSetting.TopColumns = [];
            closeFullLoading();
            return message.warn("無數據!");
        }
        let middleProps: string[] = [];
        let middColumns: object[] = [];
        Data1.forEach(item => {
            middleProps.push(item["ACCT_DATE"]);
            middColumns.push({
                title: item["ACCT_DATE"],
                align: 'center',
                dataIndex: item["ACCT_DATE"],
            })
        });
        // 排序
        let sortdata = sortArray(Data0, "PROD_NBR", "asc");
        // 表格数据
        let config: PovitParams = {
            Data: sortdata,
            povitColumn: {
                "ACCT_DATE": 'QTY'
            },
            changeColumn: middleProps,
            repeatColumn: ["WO_NBR", "PROD_NBR", "CUSTOMER", "ACTUAL_QTY", "GRADE"]
        }
        let povitData = getPovitData(config)
        getrowsum(povitData, middleProps, "SUM");
        Inserttotalrow(povitData, ["PROD_NBR"], [...middleProps, "SUM"], "PROD_NBR", "Sub Total");
        let tempbaseColumns = cloneDeep(baseColumns);
        let tempColumns = [...tempbaseColumns, ...middColumns, {
            title: 'SUM',
            align: 'center',
            dataIndex: 'SUM',
        }];
        PageSetting.TopList = povitData;
        TableCustomCell1(tempColumns, [...middleProps, "SUM"], ["WO_NBR", "PROD_NBR", "CUSTOMER"], PageSetting.TopList, [], clickfun);
        PageSetting.TopColumns = tempColumns;
        closeFullLoading();
    } catch (error) {
        closeFullLoading();
        console.log(error);
        message.error((error as Error).message)
    }

}
async function clickfun(record, rowindex, column, item, index) {
    let detailParams = cloneDeep(TopTableParams);
    detailParams["col0"] = item["dataIndex"];
    detailParams["row0"] = record["WO_NBR"];
    detailParams["row1"] = record["PROD_NBR"];
    detailParams["row2"] = record["GRADE"];
    detailParams["row3"] = "";
    detailParams["ReportType"] = "OutputByGradeDetail1";
    if (record["PROD_NBR"].includes("Sub Total")) {
        detailParams["row1"] = PageSetting.TopList[rowindex - 1]["PROD_NBR"];
    } else if (record["PROD_NBR"].includes("Sub Sum")) {
        detailParams["row1"] = record["PROD_NBR"].substring(0, 5);
    }
    // console.log("输出detailParams",detailParams);

    ReportCommonParams.AddFirstTableParams("OutputByGradeandDownGradeRatio" + FactoryType, detailParams);
    router.push({ name: `OutputByGradeDetail` + FactoryType });
}


</script>

<style lang="less" scoped></style>
