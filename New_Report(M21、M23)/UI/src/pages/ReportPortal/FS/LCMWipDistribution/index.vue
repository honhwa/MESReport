<template>
    <div class="LCMWipDistribution">
        <a-row>
            <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                <SchemaForm @submit="confirm"> </SchemaForm>
            </a-col>
            <a-col :span="PageParams.TableSpan">
                <a-button type="primary" @click="changeView" v-show="PageSetting.TopList.length != 0">{{
                    PageSetting.buttonText
                }}</a-button>
                <V3chart width="100%" height="100%" :options="PageSetting.EchartOptions" isClick @callbackFun="callbackFun"
                    v-if="!PageSetting.isShowTable"></V3chart>
                <template v-else>
                    <DynamicTable row-key="id" header-title="LCMA Wip Distribution " showFavorite sticky
                    :style="{ 'margin-bottom': '40px' }"
                        :PageParams="PageParams" :columns="PageSetting.TopColumns" :key="PageSetting.TopColumns" bordered
                        id="LCMWipDistribution" :data-source="PageSetting.TopList" :data-all="PageSetting.TopList"
                        export-type="Exceljs" :pagination="false" export-file-name="导出" :row-class-name="getRowClassNmae"
                        :flexwidth="1">
                        <template #export-button>
                            <a-button type="success"> 导出 </a-button>
                        </template>
                    </DynamicTable>
                    <DynamicTable row-key="id" :header-title="PageSetting.BottomTitle" sticky
                        :columns="PageSetting.BottomColumns" :key="PageSetting.BottomColumns" bordered
                        :data-source="PageSetting.BottomList" :data-all="PageSetting.BottomList" :pagination="false"
                        :flexwidth="1" export-file-name="导出" :row-class-name="getRowClassNmae">
                        <template #export-button>
                            <a-button type="success"> 导出 </a-button>
                        </template>
                    </DynamicTable>
                </template>
            </a-col>
        </a-row>
    </div>
</template>


<script lang="tsx" setup>
import {PageParams} from '@/components/core/dynamic-table/src/dynamic-table'
import { useTable } from '@/components/core/dynamic-table';
import { reactive } from '@vue/reactivity';
import { useForm } from '@/components/core/schema-form';
import { GetCommon } from '@/api/pages/common';
import {  onMounted } from '@vue/runtime-core';
import { message } from 'ant-design-vue';
import { getfinalsum, Inserttotalrow } from "@/utils/reportcommon/Calculation";
import { cloneDeep } from 'lodash';
import V3chart from '@/components/charts/V3chart.vue';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { getRowClassNmae } from "@/pages/ReportPortal/utils/common";

const UseParamsAdd = AddCommonPageParams();
let FactoryType: any = UseParamsAdd.Fab;
function getImport(site: any) {
    const { schemas, optionsdata } = require(`@/pages/ReportPortal/FS/LCMWipDistribution/${site}/form-schema`);
    const {
        baseColumns,
        WipDistDetail3Columns,
        WipDistDetail2Columns,
        WipDistDetail1Columns
    } = require(`@/pages/ReportPortal/FS/LCMWipDistribution/${site}/table`);
    return { schemas, optionsdata, baseColumns, WipDistDetail3Columns, WipDistDetail2Columns, WipDistDetail1Columns };
}
const { schemas, optionsdata, baseColumns, WipDistDetail3Columns, WipDistDetail2Columns, WipDistDetail1Columns } =
    getImport(FactoryType);

// 頁面數據
let PageSetting = reactive({
    TableSpan: 21,
    showmenu: true,
    TopList: [] as any,
    TopColumns: [] as any,
    EchartOptions: {},
    Message: {},
    isShowTable: false,
    buttonText: "切换表格",
    BottomList: [] as any,
    BottomColumns: [] as any,
    BottomTitle: "",

});
const [DynamicTable] = useTable();
const { mounted,  mountedowner, mountedOper,mountedLcmProdType } = useRemoteSelect()


const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas: schemas(),
    actionColOptions: {
        span: 24
    },
    fieldMapToTime: [['fieldTime', ['vDate_s', 'vDate_e1'], 'YYYY-MM-DD']],
    layout: "vertical"
});

function callbackFun(value: any) {
    console.log("点击了", value);

}
function changeView() {
    PageSetting.isShowTable = !PageSetting.isShowTable;
    if (PageSetting.isShowTable) {
        PageSetting.buttonText = "切换图表"
    } else {
        PageSetting.buttonText = "切换表格"

    }

}
let TopTableParams = reactive({
    name: "Date",
    apiName: "LCMWipDistribution",
    ReportType: "WipDistDataSet",
    FactoryType: FactoryType,
    FacID: "A",
    FromDate: "",
    ToDate: "",
    LcmOwner: [],
    Product: [],
    ProdType: "",
    Shift: "",
});
// 添加合并列功能
function TableCustomCell(Columns: Object[], ClickColumn: string[] = [], ToOtherpage: Boolean = false) {

    Columns.forEach(item => {
        if (item["children"]) {
            item["children"].forEach(citem => {
                if (ClickColumn.includes(citem.dataIndex)) {
                    citem['customCell'] = (record: Object, rowindex: number, column: Object) => {
                        return {
                            onDblclick: async () => {
                                // 新页面打开
                                if (ToOtherpage) {

                                } else {
                                    PageSetting['BottomTitle'] = `LCMA Wip Distribution -- ${column['__originColumn__'][
                                        'title'
                                    ]}`;
                                    PageSetting.BottomColumns = [];
                                    PageSetting.BottomList = [];
                                    let BottomTableParams = cloneDeep(TopTableParams);
                                    switch (column["dataIndex"]) {
                                        case "MOVE_QTY":
                                            BottomTableParams["ReportType"] = "WipDistDetail3";
                                            BottomTableParams["row0"] = record["PROD_NBR"];
                                            BottomTableParams["col1"] = record[citem["dataIndex"]];
                                            const { Date: { Data0: { Table: Data } } } = await GetCommon(BottomTableParams);
                                            if (Data.length != 0) {
                                                PageSetting.BottomColumns = WipDistDetail3Columns;
                                                PageSetting.BottomList = Data;
                                                PageSetting.BottomList.push(getfinalsum(PageSetting.BottomList, ["QTY"], "EQUIP_NBR"));
                                            } else {
                                                message.warning("无数据！")
                                            }
                                            break;
                                        case "RETURN_QTY":
                                            BottomTableParams["ReportType"] = "WipDistDetail2";
                                            BottomTableParams["row0"] = record["PROD_NBR"];
                                            BottomTableParams["col1"] = record[citem["dataIndex"]];
                                            const { Date: { Data0: { Table: detail2Data } } } = await GetCommon(BottomTableParams);
                                            if (detail2Data.length != 0) {
                                                PageSetting.BottomColumns = WipDistDetail2Columns;
                                                PageSetting.BottomList = detail2Data;
                                            } else {
                                                message.warning("无数据！")
                                            }

                                            break;
                                        case "ALL_WIP":
                                        case "ALL_HOLD_WIP":
                                            BottomTableParams["ReportType"] = "WipDistDetail1";
                                            BottomTableParams["row0"] = record["PROD_NBR"];
                                            BottomTableParams["col1"] = record[citem["dataIndex"]];
                                            const { Date: { Data0: { Table: detail1Data } } } = await GetCommon(BottomTableParams);
                                            if (detail1Data.length != 0) {
                                                PageSetting.BottomColumns = WipDistDetail1Columns;
                                                PageSetting.BottomList = detail1Data;
                                                let temp = cloneDeep(PageSetting.BottomList[0]);
                                                for (let i in temp) {
                                                    if (i == "WORK_CTR") {
                                                        temp[i] = "合計"
                                                    } else if (i == "HOLD_MARK") {
                                                        temp[i] = PageSetting.BottomList.length
                                                    } else {
                                                        temp[i] = '';
                                                    }
                                                }
                                                PageSetting.BottomList.push(temp);
                                            } else {
                                                message.warning("无数据！")
                                            }

                                            break;
                                        case "RWK_WIP":
                                        default:
                                            break;
                                    }
                                }
                            },
                            style: { cursor: 'pointer' },

                        }
                    }
                } else {
                    return {
                        onDblclick: () => {
                            message.warning('此栏位没有二阶数据')
                        },
                        style: {},
                    }

                }
            })
        }
    })
}
async function confirm() {
    PageSetting.BottomList = [];
    PageSetting.TopList = [];
    PageSetting.TopColumns = [];
    //給ParamsData賦值 
    let tempkeys = Object.keys(TopTableParams);
    for (let i in cloneDeep(dynamicFormRef.formModel)) {
        if (tempkeys.includes(i)) {
            TopTableParams[i] = cloneDeep(dynamicFormRef.formModel[i]);
        }
    }
    const { Date: {
        Data0: { Table: Data0 },//图标数据
        Data2: { Table: Data2 },//堆叠图数据
        Data3: { Table: Data3 },//折线图数据
        Data4: { Table: Data4 },//legent
        Data5: { Table: Data5 },//x轴数据
        Message: sqlmessage } } = await GetCommon(TopTableParams);
    // 表格数据

    // 计算Yield
    Data0.forEach(item => {
        if (item["SHIP_QTY"] > 0) {
            item["pp"] = (item["SHIP_QTY"] == 0 ? 0 : (item["SHIP_QTY"] * 100) / item["SHIP_QTY"]) + item["TOBESCRAP_QTY"];
            item["tempnbr"] = item["PROD_NBR"].slice(0, 5)
        } else {
            item['pp'] = 0;
        }
        if (item['SHIP_TOTALQTY'] > 0) {
            item['pp1'] =
                Number(item['SHIP_TOTALQTY']) == 0
                    ? 0
                    : (
                        ((Number(item['SHIP_TOTALQTY']) - Number(item['TOBESCRAP_TOTALQTY'])) * 100) /
                        Number(item['SHIP_TOTALQTY'])
                    ).toFixed(2);

            item["pp2"] = Number((((item["SHIP_G_TOTALQTY"] * 100) / item["SHIP_TOTALQTY"])).toFixed(2));

            item["tempnbr"] = item["PROD_NBR"].slice(0, 5)
        } else {
            item["pp1"] = 0;
            item["pp2"] = 0;
            item["tempnbr"] = item["PROD_NBR"].slice(0, 5)
        }
    })
    let sub_total = getfinalsum(Data0, [
        "SBANK_G",
        'SBANK_D',
        'START_QTY',
        "MOVE_QTY",
        "RETURN_QTY",
        "ALL_WIP",
        "ALL_HOLD_WIP",
        "RWK_WIP",
        "TORWK_QTY",
        "TORRWK_QTY",
        "TOBESCRAP_QTY",
        "SCRAP_WIP",
        "SCRAP_QTY",
        "SHIP_QTY",
        "SHIP_G_QTY",
        "SHIP_D_QTY",
        "pp",
        "START_TOTALQTY",
        "TOBESCRAP_TOTALQTY",
        "SHIP_TOTALQTY",
        "pp1",
        "SHIP_G_TOTALQTY",
        "SHIP_D_TOTALQTY",
        "pp2"
    ], "PROD_NBR");
    console.log("Data0", Data0);

    let { insertIndexList, insertrowList } = Inserttotalrow(Data0, ["tempnbr"], [
        "SBANK_G",
        'SBANK_D',
        'START_QTY',
        "MOVE_QTY",
        "RETURN_QTY",
        "ALL_WIP",
        "ALL_HOLD_WIP",
        "RWK_WIP",
        "TORWK_QTY",
        "TORRWK_QTY",
        "TOBESCRAP_QTY",
        "SCRAP_WIP",
        "SCRAP_QTY",
        "SHIP_QTY",
        "SHIP_G_QTY",
        "SHIP_D_QTY",
        "pp",
        "START_TOTALQTY",
        "TOBESCRAP_TOTALQTY",
        "SHIP_TOTALQTY",
        "pp1",
        "SHIP_G_TOTALQTY",
        "SHIP_D_TOTALQTY",
        "pp2"
    ], "PROD_NBR");
    // 合计行的最后一列，是G Grade*100  / Shipping
    insertrowList.forEach(item => {
        // if(Number(item["SHIP_QTY"])>0){
        item['pp'] =
            Number(item['SHIP_QTY']) == 0
                ? 0
                : (
                    ((Number(item['SHIP_QTY']) - Number(item['TOBESCRAP_QTY'])) * 100) /
                    Number(item['SHIP_QTY'])
                ).toFixed(2);
        item['pp1'] =
            Number(item['SHIP_TOTALQTY']) == 0
                ? 0
                : (
                    ((Number(item['SHIP_TOTALQTY']) - Number(item['TOBESCRAP_TOTALQTY'])) * 100) /
                    Number(item['SHIP_TOTALQTY'])
                ).toFixed(2);
        item['pp2'] =
            Number(item['SHIP_TOTALQTY']) == 0
                ? 0
                : ((Number(item['SHIP_G_TOTALQTY']) * 100) / Number(item['SHIP_TOTALQTY'])).toFixed(2);
        // }
    })
    for (let i = 0; i < insertIndexList.length; i++) {
        Data0.splice(insertIndexList[i] + i, 0, insertrowList[i]);
    }
    sub_total['pp'] =
        Number(sub_total['SHIP_QTY']) == 0
            ? 0
            : (
                ((Number(sub_total['SHIP_QTY']) - Number(sub_total['TOBESCRAP_QTY'])) * 100) /
                Number(sub_total['SHIP_QTY'])
            ).toFixed(2);
    sub_total['pp1'] =
        Number(sub_total['SHIP_TOTALQTY']) == 0
            ? 0
            : (
                ((Number(sub_total['SHIP_TOTALQTY']) - Number(sub_total['TOBESCRAP_TOTALQTY'])) * 100) /
                Number(sub_total['SHIP_TOTALQTY'])
            ).toFixed(2);
    sub_total['pp2'] =
        Number(sub_total['SHIP_TOTALQTY']) == 0
            ? 0
            : ((Number(sub_total['SHIP_G_TOTALQTY']) * 100) / Number(sub_total['SHIP_TOTALQTY'])).toFixed(2);
    sub_total['PROD_NBR'] = 'sub-total';
    Data0.push(sub_total);
    let total = cloneDeep(sub_total);
    total["PROD_NBR"] = "total";
    Data0.push(total);

    PageSetting.TopList = Data0;

    PageSetting.TopList = Data0;
    TableCustomCell(baseColumns, ["MOVE_QTY", "RETURN_QTY", "ALL_WIP", "ALL_HOLD_WIP", "RWK_WIP"]);
    PageSetting.TopColumns = baseColumns;
    console.log('輸出PageSetting.TopColumns', PageSetting.TopColumns);
    // DynamicColumnWidth(PageSetting.TopColumns, PageSetting.TopList,0);

    // x轴数据
    let xData: number[] = [];
    Data5.forEach(item => {
        xData.push(item["WORK_CTR"])
    })

    // 折线图数据
    let lineData: number[] = [];

    Data3.forEach(item => {
        if (xData.includes(item["WORK_CTR"])) {
            lineData.push(item["MOVEQTY"])
        }
    })
    // legent数据
    let legendData: string[] = [];
    Data4.forEach(item => {
        legendData.push(item["PROD_NBR"]);
    });
    legendData.push("Movement");
    // 堆叠柱状图数据
    // 排序
    // 如果没有x轴的数据，则需要添加
    let obj = {};
    for (let i = 0; i < Data2.length; i++) {
        // PROD_NBR :2v5
        let value = Data2[i].PROD_NBR
        //如果obj中不存在这个PROD_NBR，则向obj中添加PROD_NBR属性和一个数组
        if (!obj[value]) {
            obj[value] = {},
                obj[value]["Table"] = [Data2[i]];
            obj[value]["WORK_CTR"] = [Data2[i]["WORK_CTR"]];

        } else {
            obj[value]["Table"].push(Data2[i]);
            obj[value]["WORK_CTR"].push(Data2[i]["WORK_CTR"]);
        }
    }

    for (let i in obj) {
        // 去除arry1中不存在的值
        let arr = xData.filter(x => obj[i]["WORK_CTR"].every(y => y !== x));
        for (let index = 0; index < arr.length; index++) {
            obj[i]["Table"].push({
                WORK_CTR: arr[index],
                PROD_NBR: i,
                WIPQTY: 0
            })
        }
    }

    // 取出table
    let finalTable: any[] = [];
    for (let i in obj) {
        finalTable = [...finalTable, ...obj[i]["Table"]]
    }
    // 排序
    finalTable = finalTable.sort((a, b) => {
        return a.WORK_CTR - b.WORK_CTR;
    })
    let fianlobj: object = {}
    // 保存legent
    let arr2 = [];
    for (let i = 0; i < finalTable.length; i++) {
        let value = finalTable[i].PROD_NBR
        if (!arr2.includes(finalTable[i].PROD_NBR)) {
            arr2.push(finalTable[i].PROD_NBR);
        }
        //如果obj中不存在这个PROD_NBR，则向obj中添加PROD_NBR属性和一个数组
        if (!fianlobj[value]) {
            fianlobj[value] = [finalTable[i]["WIPQTY"]]
        } else {
            fianlobj[value].push(finalTable[i]["WIPQTY"])
        }
    }
    let BarData: any[] = [];
    console.log("最總輸出", fianlobj);

    for (let i in fianlobj) {
        BarData.push(
            {
                name: i,
                type: "bar",
                barWidth: "80%",
                data: fianlobj[i],
                stack: "WIP",
                emphasis: {//折线图的高亮状态。
                    focus: "series",//聚焦当前高亮的数据所在的系列的所有图形。
                },
                //背景色
                // showBackground: true,
                backgroundStyle: {
                    color: "rgba(180, 180, 180, 0.2)",
                }
            }
        )


    }
    PageSetting.EchartOptions = {
        // backgroundColor: "#061740",
        title: [
            {
                text: "LCMA Wip Distribution Chart",
                left: "center"
            }

        ],
        grid: {
            x: 0,
            y: 0,
            x2: 0,
            y2: 0,
            top: "15%",
            left: "3%",
            right: "3%",
            bottom: "20%",
            containLabel: true,
        },
        legend: {
            // 柱状图的leggeng
            data: legendData,
            bottom: "16%",
            type: "scroll",
            padding: [10, 0, 0, 0],
            textStyle: {
                color: "rgba(36, 173, 254, 1)",
                fontSize: "1rem",
            },
            //图例标记的图形高度
            itemHeight: 5,
            //图例标记的图形宽度
            itemWidth: 10,

        },
        // 提示
        tooltip: {
            trigger: "item",
            axisPointer: {
                type: "shadow",
            }
        },
        // x轴显示内容
        xAxis: [
            {
                type: "category",
                axisTick: {
                    show: false,
                },
                interval: 1,
                axisLabel: {
                    color: "rgba(36, 173, 254, 1)",
                    fontSize: "12",
                },
                axisLine: {
                    show: true,
                },
                data: xData,
            },
        ],
        yAxis: [
            {
                type: 'value',
                name: 'WIP',
                splitLine: false,
                axisLine: {
                    show: true,
                },
                /* min: 0,
                max: 250,
                interval: 50,
                axisLabel: {
                    formatter: '{value} ml'
                } */
            },
            {
                type: 'value',
                name: 'Movement',
                splitLine: false,
                axisLine: {
                    show: true,
                },
                /* min: 0,
                max: 25,
                interval: 1,
                axisLabel: {
                    formatter: '{value} °C'
                } */
            },


        ],
        series: [
            // 堆叠柱状图
            ...BarData,
            // 折线图
            {
                type: "line",
                stack: "Total",
                name: "Movement",
                emphasis: {
                    focus: "series",
                },
                yAxisIndex: 1,
                data: lineData,
                itemStyle: {
                    normal: {
                        color: "#FFBB00", //改变折线点的颜色
                        lineStyle: {
                            color: "#FFBB00", //改变折线颜色
                            type: "dashed",
                        },
                    },
                },
                symbol: "circle",
                symbolSize: 15,
            },
        ],
    }
}
onMounted(async () => {
    /* await mountedowner(optionsdata, FactoryType);
    await mountedOper(optionsdata, FactoryType);
    await mounted(optionsdata, FactoryType);
    await mountedLcmProdType(optionsdata, FactoryType); */
});
</script>

<style lang="less" scoped>
:deep(.ant-table) {
    height: 35vh !important;
}

:deep(.ant-table-container) {
    height: 35vh !important;
}

:deep(.ant-table-wrapper) {
    height: 35vh !important;
    // overflow: auto;
}

:deep(.ant-table-body) {
    height: 35vh !important;
    overflow: auto;
}

/* .tablewrapper {
    height: 50vh;
    overflow: scroll;
 
} */
</style>