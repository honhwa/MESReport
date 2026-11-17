<template>
    <div class="Report">
        <a-card>
            <a-row>
                <a-col :span="24 - PageParams.TableSpan" class="ScrollForm">
                    <SchemaForm @submit="confirm"> </SchemaForm>
                </a-col>
                <a-col :span="PageParams.TableSpan">
                    <a-button type="primary" @click="changeView(1)" class="a-button">
                        显示图表</a-button>
                    <a-button type="primary" @click="changeView(2)" class="a-button">
                        显示表格</a-button>
                    <V3chart width="100%" height="100%" :options="PageSetting.EchartOptions" v-if="PageSetting.isShowTable">
                    </V3chart>
                    <div v-else>
                        <DynamicTable row-key="report" :header-title="PageSetting.Title" :columns="PageSetting.Columns"
                            showFavorite sticky :key="PageSetting.Columns" bordered :row-class-name="getRowClassNmae"
                            :style="{ 'margin-bottom': '20px' }" :data-source="PageSetting.moreData"
                            :data-all="PageSetting.moreData" :pagination="false" :flexwidth="1"
                            :export-file-name="PageSetting.Title">
                            <template #export-button>
                                <a-button type="success"> 导出 </a-button>
                            </template>
                        </DynamicTable>
                        <DynamicTable row-key="report" :header-title="PageSetting.BottomTitle"
                            :columns="PageSetting.BottomColumns" sticky :key="PageSetting.BottomColumns" bordered
                            :row-class-name="getRowClassNmae" :data-source="PageSetting.BottomList"
                            :data-all="PageSetting.BottomList" :pagination="false" :flexwidth="1"
                            :export-file-name="PageSetting.BottomTitle">
                            <template #export-button>
                                <a-button type="success"> 导出 </a-button>
                            </template>
                        </DynamicTable>
                    </div>
                </a-col>
            </a-row>
        </a-card>
    </div>
</template>

<script lang="tsx" setup>
import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table';
import { useLoading } from '@/components/feat/Loading';
import { GetCommon } from '@/api/pages/common';
import { cloneDeep } from 'lodash';
import { onMounted } from '@vue/runtime-core';
import { reactive, ref } from '@vue/reactivity';
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import { ReportCommon } from '@/store/module/ReportCommon';
import V3chart from '@/components/charts/V3chart.vue';
import { message } from 'ant-design-vue';
import { getindex, mergeCells, Inserttotalrow, getfinalsum, mergerobjList, sortArray } from '@/pages/ReportPortal/utils/common';
import dayjs from 'dayjs';
import { useRouter } from 'vue-router';
const router = useRouter();
console.log("輸出router", router);

const UseParamsAdd = ReportCommon();
let FactoryType: string = UseParamsAdd.Fab;
const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});
const report = ref<HTMLElement | null>(null);
function getImport(site: string) {
    const { schemas } = require(`@/pages/ReportPortal/FS/OQCQualityReport/${site}/form-schema`);
    return { schemas };
}
let { schemas } = getImport(FactoryType);

let PageSetting = reactive<BasePageParams.BasePageSetting>({
    List: [] as any,
    Columns: [] as any,
    Title: '',
    moreData: [],
    SQLHidden: '',
    APIColumns: [] as any,
    EchartOptions: {},
    isShowTable: true,
    StrSql_w: "",
    StrSql_R: "",
    values: {},
    BottomColumns: [] as any,
    BottomList: [] as any,
    BottomTitle: ""
});
switch (FactoryType) {
    case 'SET1':
    case 'SET1A':
        PageSetting.Title = 'LCMA_TV OQC 品質推移圖';
        PageSetting.BottomTitle = "LCMA OQC 品質推移圖 Detail"
        break;
    case "LCM1":
    case "LCM1A":
    case "LCM2":
        PageSetting.Title = "LCMA OQC 品質推移圖"
        PageSetting.BottomTitle = "LCMA OQC 品質推移圖 Detail"
    default:
        break;
}
const [DynamicTable] = useTable();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas: schemas(FactoryType),
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
});
function changeView(type: number) {
    if (type == 1) {
        PageSetting.isShowTable = true;
    } else {
        PageSetting.isShowTable = false;

    }
}
function getRowClassNmae(record: Object) {
    if (record && Object.keys(record).length != 0) {
        for (const [key, value] of Object.entries(record)) {
            if (key == "Date" && ['品位(F)', "電氣(C)", "外觀(D)", ''].includes(value)) {
                return 'YellowBold';
            }
        }
    }
}
async function confirm(values) {
    openFullLoading();
    PageSetting.BottomList = [];
    let baseParams = {
        apiName: 'OQCQuality_ADataSet',
        FactoryType: FactoryType,
        G_FAC: UseParamsAdd.G_FAC_COM,
        prodsize: ""
    };
    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        }
        if (['FromTime', 'ToTime'].includes(key)) {
            values[key] = dayjs(values[key]).format('HH.mm');
        }
    });

    try {
        let {
            api01: {
                chartData: { Table: chartData },
                tableData: { Table: tableData }, //柱狀圖數據
                xlist,
                Message: { StrSql_w, StrSql_R }//x軸數據
            }
        } = await GetCommon({ ...baseParams, ...values });

        if (xlist.length == 0) {
            PageSetting.moreData = [];
            closeFullLoading();
            return message.warn('无数据!');
        }
        PageSetting.StrSql_w = StrSql_w;
        PageSetting.StrSql_R = StrSql_R;
        PageSetting.values = values;
        // 填充為0時候的數據對象，用來做合并f
        let tempobj = {};
        xlist.forEach((item) => {
            tempobj[item] = 0;
        });
        let barobj = {};
        console.log("未處理之前的數據", tableData);

        // 保存上一個的tableDatad的item
        /* tableData = tableData.filter(item => {
            return item.OQC_GROUP != null
        }) */
        // 如果是null，設置為品位(F)
        tableData.forEach(item => {
            if (item.OQC_GROUP == null) {
                item.OQC_GROUP = "品位(F)"
            }
        })
        console.log("處理之后的數據", tableData);

        let temptabledataitem = {};
        tableData.forEach((item) => {
            if (item['OQC_GROUP']) {
                // 存在這個對象
                if (barobj[item['OQC_GROUP']]) {
                    if (temptabledataitem['ACCT_DATE'] == item['ACCT_DATE'] && temptabledataitem['OQC_GROUP'] == item['OQC_GROUP']) {
                        barobj[item['OQC_GROUP']].push({
                            [item['ACCT_DATE']]: item['QTY'] + temptabledataitem['QTY']
                        });
                    } else {
                        barobj[item['OQC_GROUP']].push({
                            [item['ACCT_DATE']]: item['QTY']
                        });
                    }
                } else {
                    // 不存在則新建
                    barobj[item['OQC_GROUP']] = [];
                    barobj[item['OQC_GROUP']].push({
                        [item['ACCT_DATE']]: item['QTY']
                    });
                }
                temptabledataitem = item;
            }
        });
        // console.log("輸出處理中的barobj", barobj);

        let finalbarobj = {};
        // 求差集，并將差集設置為0
        for (const key in barobj) {
            finalbarobj[key] = Object.values(Object.assign(cloneDeep(tempobj), ...barobj[key]));
        }
        let legentdata = ['品位(F)', '外觀(D)', '電氣(C)'];
        // 計算差集
        const collectionD = legentdata.filter((c) => !Object.keys(finalbarobj).some((d) => d === c));
        // 缺少其中一個就填補
        collectionD.forEach((item) => {
            finalbarobj[item] = xlist.map((item) => 0);
        });
        // 折綫圖數據
        let lineobj = {};
        let finallineobj = {};
        let linelist = chartData.map((item) => {
            return {
                [item['ACCT_DATE']]: item['OUT_QTY_1']
            };
        });
        // console.log("輸出linelist", linelist);

        // 柱狀圖最終數據
        let BarData: any[] = [];
        for (let i in finalbarobj) {
            BarData.push({
                name: i,
                type: 'bar',
                barWidth: '15%',
                data: finalbarobj[i],
                stack: '數量',
                emphasis: {
                    //折线图的高亮状态。
                    focus: 'series' //聚焦当前高亮的数据所在的系列的所有图形。
                },
                //背景色
                // showBackground: true,
                /*  backgroundStyle: {
                     color: 'rgba(180, 180, 180, 0.2)'
                 }, */
                itemStyle: function style(i: string) {
                    let color;
                    // color: ['綠色#0bf10b', '藍色#0d8add', '紅色#ee0606']
                    switch (i) {
                        case "電氣(C)":
                            color = "#ee0606";
                            break;
                        case "外觀(D)":
                            color = "#0d8add";
                            break;
                        case "品位(F)":
                            color = "#0bf10b";
                            break;
                        default:
                            break;
                    }
                    return {//自定义颜色
                        normal: { color: color },
                    }
                }(i),
            });
            finallineobj[`(${getindex(i)})Failure Rate`] = finalbarobj[i].map((item, index) => {
                if (item) {
                    return Number((1 + (item - 1) + (item / linelist[index][xlist[index]]) * 100).toFixed(2))
                } else {
                    return 0
                }
            })
        }
        /*  console.log("BarData", BarData);
         console.log("輸出最終的柱圖數據finalbarobj", finalbarobj); */


        let lineData: any[] = [];
        for (let i in finallineobj) {
            lineData.push(
                {
                    name: i,
                    type: 'line',
                    data: finallineobj[i],
                    itemStyle: function style(i: string) {
                        console.log("輸出i", i);
                        let color;
                        switch (i) {
                            case "(C)Failure Rate":
                                color = "#ee0606";
                                break;
                            case "(D)Failure Rate":
                                color = "#0d8add";
                                break;
                            case "(F)Failure Rate":
                                color = "#0bf10b";
                                break;
                            default:
                                break;
                        }
                        return {//自定义颜色
                            normal: {
                                color: color,//折线点的颜色
                                lineStyle: {
                                    color: color//折线的颜色
                                }
                            },
                        }
                    }(i),
                }
            )
        }
        /*  console.log("輸出lineData", lineData);
         console.log("输出finallineobj", finallineobj); */
        PageSetting.EchartOptions = {
            title: [
                {
                    text: 'OQC 品質推移圖',
                    left: 'center'
                }
            ],
            grid: {
                x: 0,
                y: 0,
                x2: 0,
                y2: 0,
                top: '15%',
                left: '3%',
                right: '3%',
                bottom: '20%',
                containLabel: true
            },
            legend: {
                // 柱状图的leggeng
                data: legentdata.concat(Object.keys(finallineobj)),
                bottom: '16%',
                type: 'scroll',
                padding: [10, 0, 0, 0],
                textStyle: {
                    color: 'rgba(36, 173, 254, 1)',
                    fontSize: '1rem'
                },
                //图例标记的图形高度
                itemHeight: 5,
                //图例标记的图形宽度
                itemWidth: 10
            },
            // 提示
            tooltip: {
                trigger: 'axis',
                axisPointer: {
                    type: 'shadow'
                }
            },
            // x轴显示内容
            xAxis: [
                {
                    type: 'category',
                    axisTick: {
                        show: false
                    },
                    splitLine: {
                        // 坐標軸虛綫
                        show: true,
                        lineStyle: {
                            type: 'dashed'
                        }
                    },
                    interval: 1,
                    axisLabel: {
                        color: 'rgba(36, 173, 254, 1)',
                        fontSize: '12'
                    },
                    axisLine: {
                        show: true
                    },
                    data: xlist
                }
            ],
            yAxis: [
                {
                    type: 'value',
                    name: '數量',
                    splitLine: {
                        // 坐標軸虛綫
                        show: true,
                        lineStyle: {
                            type: 'dashed'
                        }
                    },
                    axisLine: {
                        show: true
                    }
                }
            ],
            series: [
                // 堆叠柱状图
                ...BarData,
                // 折线图
                ...lineData

            ],

        };
        // console.log("最终的echart", PageSetting.EchartOptions);
        // 表格数据处理
        // 表头合成
        let leftcolumns = [{
            title: '日期(by day)',
            align: 'center',
            dataIndex: 'Date',
            fixed: 'left',
            className: 'v-center'
        },
        {
            title: '日期(by day)',
            align: 'center',
            dataIndex: 'Date Rate',
            fixed: 'left',
            className: 'v-center'
        }]
        let centercolumns: any[] = [];
        xlist.forEach(item => {
            centercolumns.push({
                title: item,
                align: 'center',
                dataIndex: item,
            })
            centercolumns.push({
                title: item,
                align: 'center',
                dataIndex: `${item} Rate`,
            })
        })
        let rightcolumns = [{
            title: "TOTAL",
            align: 'center',
            dataIndex: "TOTAL",
        },
        {
            title: "TOTAL",
            align: 'center',
            dataIndex: "TOTAL Rate",
        }]
        // PageSetting.Columns = leftcolumns.concat(centercolumns).concat(rightcolumns);
        PageSetting.Columns = [...leftcolumns, ...centercolumns, ...rightcolumns];
        // 表頭合并
        PageSetting.Columns.forEach((item, index) => {
            if (index % 2 === 0) {
                item['colSpan'] = 2;
            } else {
                item['colSpan'] = 0;
            }
            item['customCell'] = (record: Object, rowIndex: number, column: Object) => {
                let colSpan;
                // 合并前三行的數據
                if (rowIndex < 3) {
                    // 除了第一第二列不合并
                    if (index >= 2) {
                        if (index % 2 === 0) {
                            colSpan = 2;
                        } else {
                            colSpan = 0;

                        }
                    }
                }
                if (rowIndex == 9 || rowIndex == 10) {
                    if (index == 0) {
                        colSpan = 2;
                    } else if (index == 1) {
                        colSpan = 0;
                    }
                }
                let rowSpan;
                if (["Date"].includes(item["dataIndex"])) {
                    rowSpan = mergeCells(record[item["dataIndex"]], item["dataIndex"], rowIndex, PageSetting.moreData)
                }
                let onDblclick;
                let style;
                if (rowIndex > 10) {
                    style = { cursor: 'pointer' };
                    onDblclick = async () => {
                        /*  console.log("輸出點擊的record", record);
                         console.log("輸出點擊的rowIndex", rowIndex);
                         console.log("輸出點擊的column", column); */
                        let defcode;
                        if (record["Date"] == "外觀(D)") {
                            defcode = "D外觀"
                        } else if (record["Date"] == "品位(F)") {
                            defcode = "F品位"
                        } else if (record["Date"] == "電氣(C)") {
                            defcode = "C電氣"
                        } else {
                            defcode = record["Date"]
                        }
                        let defdate;
                        if (column["dataIndex"] == "TOTAL Rate") {
                            defdate = "TOTAL"
                        } else {
                            defdate = column["dataIndex"];
                        }
                        let BottomParams = {
                            apiName: 'OQCQuality_ADetail',
                            FactoryType: FactoryType,
                            G_FAC: UseParamsAdd.G_FAC_COM,
                            FromDate: PageSetting.values["FromDate"],
                            ToDate: PageSetting.values["ToDate"],
                            FromTime: PageSetting.values["FromTime"],
                            ToTime: PageSetting.values["ToTime"],
                            defcode,
                            defdate,
                            // StrSql_w和StrSql_R看起來是沒用的，舊版的f12一直都是空的
                            StrSql_R: PageSetting.StrSql_R,
                            StrSql_w: PageSetting.StrSql_w,
                            operation: PageSetting.values["operation"]
                        }
                        try {
                            openFullLoading();
                            let { api01: { Data: { Table: Data } } } = await GetCommon(BottomParams);
                            if (Data.length) {
                                PageSetting.BottomColumns = [];
                                PageSetting.BottomColumns = Object.keys(Data[0]).map(item => {
                                    return {
                                        title: item,
                                        align: 'center',
                                        dataIndex: item,
                                    }
                                })
                                if (["LCM2", "LCM1"].includes(FactoryType)) {
                                    PageSetting.BottomColumns.forEach(item => {
                                        item['customCell'] = (record: Object, rowindex: number, column: Object) => {
                                            let style = {};
                                            let onDblclick;
                                            if (["Panel ID"].includes(item["dataIndex"])) {
                                                style = { cursor: 'pointer' };
                                                onDblclick = async () => {
                                                    console.log("record", record["Panel ID"]);
                                                    UseParamsAdd.AddFirstTableParams("OQCQualityReport" + FactoryType, { pnl_id: record["Panel ID"] });
                                                    router.push({ name: "OQCQualityReportDetail" + FactoryType })
                                                };
                                                // 跳轉到二階頁面
                                            } else {
                                                style = {};
                                                onDblclick = () => {
                                                    message.warning('此栏位没有二阶数据');
                                                }
                                            }
                                            return {
                                                onDblclick,
                                                style,
                                            }

                                        }
                                    })
                                }
                                PageSetting.BottomList = Data;
                                closeFullLoading();
                            } else {
                                PageSetting.BottomList = [];
                                closeFullLoading();
                                return message.warn("無數據!");
                            }
                            // console.log("輸出Data", Data);
                        } catch (error) {
                            console.log(error);
                            closeFullLoading();
                        }
                    }
                } else {
                    onDblclick = () => {
                        message.warning('此栏位没有二阶数据');
                    }
                }
                return { rowSpan, colSpan, onDblclick, style }
            }
        }
        )
        // 前6行处理
        let Sampleqty1, Sampleqty2, Sampleqty3, Passrate1, Passrate2, Passrate3 = {};
        Sampleqty1 = {
            Date: "Sample Qty",
            'Date Rate': "電氣(c)",
        };
        Sampleqty2 = {
            Date: "Sample Qty",
            'Date Rate': "品位(f)",
        };
        Sampleqty3 = {
            Date: "Sample Qty",
            'Date Rate': "外觀(d)",
        };
        Passrate1 = {
            Date: "Pass Rate",
            'Date Rate': "電氣(c)",
        }
        Passrate2 = {
            Date: "Pass Rate",
            'Date Rate': "品位(f)",
        }
        Passrate3 = {
            Date: "Pass Rate",
            'Date Rate': "外觀(d)",
        }
        chartData.forEach((item) => {
            Sampleqty1[item['ACCT_DATE']] = item['OUT_QTY_1'];
            Sampleqty1[`${item['ACCT_DATE']} Rate`] = item['OUT_QTY_1'];
            Sampleqty2[item['ACCT_DATE']] = item['OUT_QTY_1'];
            Sampleqty2[`${item['ACCT_DATE']} Rate`] = item['OUT_QTY_1'];
            Sampleqty3[item['ACCT_DATE']] = item['OUT_QTY_1'];
            Sampleqty3[`${item['ACCT_DATE']} Rate`] = item['OUT_QTY_1'];
            Passrate1[item['ACCT_DATE']] = item['OUT_QTY_1'];
            Passrate1[`${item['ACCT_DATE']} Rate`] = item['OUT_QTY_1'];
            Passrate2[item['ACCT_DATE']] = item['OUT_QTY_1'];
            Passrate2[`${item['ACCT_DATE']} Rate`] = item['OUT_QTY_1'];
            Passrate3[item['ACCT_DATE']] = item['OUT_QTY_1'];
            Passrate3[`${item['ACCT_DATE']} Rate`] = item['OUT_QTY_1'];
        });
        const dataSource = [Sampleqty1, Sampleqty2, Sampleqty3];
        //  linelist的總計
        let linetotal;
        dataSource.forEach((item) => {
            item["TOTAL"] = 0;
            for (let i = 0; i < xlist.length; i++) {
                if (!isNaN(item[xlist[i]])) {
                    item[xlist[i]] = Number(item[xlist[i]]);
                    item["TOTAL"] += item[xlist[i]];
                }
            }
            item["TOTAL Rate"] = item["TOTAL"];
            linetotal = item["TOTAL"];

        });
        // 不良項目處理
        let badproject = {
            Date: "不良項目",
            'Date Rate': "不良項目",
        };
        // Passrate三行處理
        xlist.forEach((item, index) => {
            Passrate1[`${item} Rate`] = ((Passrate1[item] - finalbarobj["電氣(C)"][index]) / Passrate1[item] * 100).toFixed(2) + "%";
            Passrate1[item] = Passrate1[item] - finalbarobj["電氣(C)"][index];
            Passrate2[`${item} Rate`] = ((Passrate2[item] - finalbarobj["品位(F)"][index]) / Passrate2[item] * 100).toFixed(2) + "%";
            Passrate2[item] = Passrate2[item] - finalbarobj["品位(F)"][index];
            Passrate3[`${item} Rate`] = ((Passrate3[item] - finalbarobj["外觀(D)"][index]) / Passrate3[item] * 100).toFixed(2) + "%";
            Passrate3[item] = Passrate3[item] - finalbarobj["外觀(D)"][index];
            badproject[item] = "不良數";
            badproject[`${item} Rate`] = "%"
        })
        const dataSource2 = [Passrate1, Passrate2, Passrate3];
        dataSource2.forEach((item, index) => {
            item["TOTAL"] = 0;
            for (let i = 0; i < xlist.length; i++) {
                if (!isNaN(item[xlist[i]])) {
                    item[xlist[i]] = Number(item[xlist[i]]);
                    item["TOTAL"] += item[xlist[i]];
                }
            }
            item["TOTAL Rate"] = (item["TOTAL"] / dataSource[index]["TOTAL"] * 100).toFixed(2) + "%";
        });
        PageSetting.moreData = [];
        PageSetting.moreData.push(Sampleqty1, Sampleqty2, Sampleqty3, Passrate1, Passrate2, Passrate3);
        // Failure Rate 和不良項目一樣，只是failure rate保存小數乘以1000
        let FailureRate = {}
        // finalbarobj,linelist ,xlist
        for (const key in finalbarobj) {
            FailureRate[key] = {};
            FailureRate[key]["Date"] = "Failure Rate";
            FailureRate[key]["Date Rate"] = key;
            xlist.forEach((item, index) => {
                FailureRate[key][item] = finalbarobj[key][index]
                FailureRate[key][`${item} Rate`] = Number((((finalbarobj[key][index]) / (linelist[index][item])) * 1000000).toFixed(2))
            })
            // console.log("輸出", FailureRate[key]);

            // 計算total
            // for (const rowkey in FailureRate[key]) {
            FailureRate[key]["TOTAL"] = 0;
            for (let i = 0; i < xlist.length; i++) {
                if (!isNaN(FailureRate[key][xlist[i]])) {
                    FailureRate[key][xlist[i]] = Number(FailureRate[key][xlist[i]]);
                    FailureRate[key]["TOTAL"] += FailureRate[key][xlist[i]];
                }
            }
            let firsttotal = dataSource.find(ditem => FailureRate[key]["Date Rate"].substring(0, 2).includes(ditem["Date Rate"].substring(0, 2)))
            FailureRate[key]["TOTAL Rate"] = ((FailureRate[key]["TOTAL"] / firsttotal["TOTAL"] * 1000000).toFixed(2));
            // }
            /*  [FailureRate[key]].forEach((item, index) => {
                 item["TOTAL"] = 0;
                 for (let i = 0; i < xlist.length; i++) {
                     if (!isNaN(item[xlist[i]])) {
                         item[xlist[i]] = Number(item[xlist[i]]);
                         item["TOTAL"] += item[xlist[i]];
                     }
                 }
                 let firsttotal = dataSource.find(ditem => FailureRate[key]["Date Rate"].substring(0, 2).includes(ditem["Date Rate"].substring(0, 2)))
                 item["TOTAL Rate"] = ((item["TOTAL"] / firsttotal["TOTAL"] * 1000000).toFixed(2));
             }); */
            PageSetting.moreData.push(FailureRate[key]);
        }
        // console.log("輸出FailureRate", FailureRate);

        let dataSource3: any = [];
        for (const key in FailureRate) {
            dataSource3.push(FailureRate[key])
        }
        let calculist: any[] = [];
        xlist.forEach(item => {
            calculist.push(item, `${item} Rate`);
        })

        let temp = getfinalsum(dataSource3, [...calculist, "TOTAL", "TOTAL Rate"], "Date", "Total Failure rate");
        temp["Date Rate"] = "Total Failure rate";
        temp["TOTAL Rate"] = Number(temp["TOTAL Rate"].toFixed(2))
        console.log("输出calculist",calculist);
        console.log("輸出temp的值", temp);
        calculist.forEach(item=>{
            if(item.includes("Rate")){
                temp[item] = Number(temp[item].toFixed(2))
            }
        })
        PageSetting.moreData.push(temp, badproject);



        // 不良項目的之後的處理
        // 使用對象存儲,讓順序在一起
        // linelist的總計
        let badprojectobj = {};
        tableData.forEach(item => {
            if (badprojectobj[item["OQC_GROUP"]]) {
                badprojectobj[item["OQC_GROUP"]].push({
                    Date: item["ERR_CODE"],
                    'Date Rate': item["ERRC_DESCR"],
                    [item["ACCT_DATE"]]: item["QTY"],
                    [`${item["ACCT_DATE"]} Rate`]: linelist.find(titem => { return titem[item["ACCT_DATE"]] }) == undefined ? null : Number(((item["QTY"] / (linelist.find(titem => { return titem[item["ACCT_DATE"]] })[item["ACCT_DATE"]])) * 100).toFixed(2)),
                    ...item
                })
            } else {
                badprojectobj[item["OQC_GROUP"]] = [];
                badprojectobj[item["OQC_GROUP"]].push({
                    Date: item["ERR_CODE"],
                    'Date Rate': item["ERRC_DESCR"],
                    [item["ACCT_DATE"]]: item["QTY"],
                    [`${item["ACCT_DATE"]} Rate`]: linelist.find(titem => { return titem[item["ACCT_DATE"]] }) == undefined ? null : Number(((item["QTY"] / (linelist.find(titem => { return titem[item["ACCT_DATE"]] })[item["ACCT_DATE"]])) * 100).toFixed(2)),
                    ...item
                })
            }
        })
        // console.log("輸出badprojectobj", badprojectobj);
        // 相同項合并
        for (const key in badprojectobj) {
            // 排序
            badprojectobj[key] = sortArray(badprojectobj[key], "Date");
            badprojectobj[key] = mergerobjList(badprojectobj[key], ["Date", "Date Rate"]);
        }
        for (const key in badprojectobj) {
            badprojectobj[key].forEach((item) => {
                item["TOTAL"] = 0;
                for (let i = 0; i < xlist.length; i++) {
                    if (!isNaN(item[xlist[i]])) {
                        item[xlist[i]] = Number(item[xlist[i]]);
                        item["TOTAL"] += item[xlist[i]];
                    }
                }
                item["TOTAL Rate"] = ((item["TOTAL"] / linetotal) * 100).toFixed(2) + "%";
            });
            let { insertrowList, insertIndexList } = Inserttotalrow(badprojectobj[key], [], [...calculist, "TOTAL", "TOTAL Rate"], "Date", "", false, false);
            insertrowList[0]["Date"] = key == "null" ? "" : key;
            insertrowList[0]["Date Rate"] = key == "null" ? "" : key;
            insertrowList[0]["TOTAL Rate"] = ((insertrowList[0]["TOTAL"] / linetotal) * 100).toFixed(2) + "%";
            badprojectobj[key].splice(0, 0, insertrowList[0]);
            PageSetting.moreData.push(...badprojectobj[key])
        }
        // 添加百分號
        PageSetting.moreData.forEach((item, index) => {
            if (index >= 11) {
                Object.keys(item).forEach(kitem => {
                    if (kitem.includes("Rate") && !["TOTAL Rate", "Date Rate"].includes(kitem)) {
                        item[kitem] = item[kitem].toFixed(2) + "%"
                    }
                })
            }
        })
        // console.log("輸出最終的數據", PageSetting.moreData);
        // 清除為0或者0(0)的數據
        let keyarr = Object.keys(PageSetting.moreData[0]);
        PageSetting.moreData.forEach((item, index) => {
            keyarr.forEach((kitem) => {
                if ([0,].includes(item[kitem])) {
                    item[kitem] = '';
                }
            });
        });
        closeFullLoading();
    } catch (error) {
        console.log('輸出error', error);
        closeFullLoading();
    }
}

onMounted(async () => { });
</script>

<style lang="less" scoped>
.a-button {
    margin-right: 10px;
}

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
