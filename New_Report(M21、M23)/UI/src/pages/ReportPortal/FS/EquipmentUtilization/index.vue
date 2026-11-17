<template>
    <div class="EquipmentUtilization">
        <a-row>
            <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                <SchemaForm @submit="confirm"> </SchemaForm>
            </a-col>
            <a-col :span="PageParams.TableSpan">
                <a-button type="primary" @click="changeView" v-show="PageSetting.TopList.length != 0">{{
                    PageSetting.buttonText
                }}</a-button>
                <V3chart width="100%" height="100%" :options="PageSetting.EchartOptions" isClick
                    v-if="!PageSetting.isShowTable"></V3chart>
                <div class="table" v-else>
                    <DynamicTable row-key="id" header-title="LCMA Cycle Time " :columns="PageSetting.TopColumns"
                        showFavorite sticky :PageParams="PageParams" :key="PageSetting.TopColumns" bordered
                        :data-source="PageSetting.TopList" :data-all="PageSetting.TopList" :pagination="false"
                        export-file-name="LCMA Cycle Time " :row-class-name="getRowClassNmae" :flexwidth="1">
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
import {PageParams} from '@/components/core/dynamic-table/src/dynamic-table'
import { useTable } from '@/components/core/dynamic-table';
import { reactive } from '@vue/reactivity';
import { useForm } from '@/components/core/schema-form';
import { GetCommon } from '@/api/pages/common';
import { onMounted } from '@vue/runtime-core';
import { message } from 'ant-design-vue';
import { getfinalsum } from '@/pages/ReportPortal/utils/common';
import { cloneDeep } from 'lodash';
import V3chart from "@/components/charts/V3chart.vue";
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { useLoading } from '@/components/feat/Loading';

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});

const UseParamsAdd = AddCommonPageParams();
let FactoryType: any = UseParamsAdd.Fab;
function getImport(site: any) {
    const { schemas, optionsdata, } = require(`@/pages/ReportPortal/${site}/EquipmentUtilization/LCM1/form-schema`);
    const { baseColumns } = require(`@/pages/ReportPortal/${site}/EquipmentUtilization/LCM1/table`);
    return { schemas, optionsdata, baseColumns };
}
const { schemas, optionsdata, baseColumns, } = getImport('FS');
// 頁面數據
let PageSetting = reactive({
    TopList: [] as any,
    TopColumns: [] as any,
    EchartOptions: {},
    Message: {},
    isShowTable: false,
    buttonText: "切换表格",

});
const [DynamicTable] = useTable();
const { mounted, mountedowner, mountedOper, mountedEquip } = useRemoteSelect()
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas: schemas(FactoryType),
    actionColOptions: {
        span: 24
    },
    fieldMapToTime: [['fieldTime', ['vDate_s', 'vDate_e1'], 'YYYY-MM-DD']],
    layout: "vertical"
});

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
    apiName: "EquUtilizationDataSet",
    FactoryType: FactoryType,
    G_FAC: "A",
    FromDate: "",
    ToDate: "",
    Shift: "",
    Interval: "",
    Floor: "",
    Equipment: [],
});
function getRowClassNmae(record: Object) {
    if (record && Object.keys(record).length != 0) {
        for (const [key, value] of Object.entries(record)) {
            if (typeof (value) == "string" && value.includes("Sum")) {
                return 'Green';
            }
        }
    }
}
async function confirm() {
    openFullLoading();
    PageSetting.TopList = [];
    PageSetting.TopColumns = [];
    //給ParamsData賦值 
    let titems = Object.keys(TopTableParams);
    for (let i in cloneDeep(dynamicFormRef.formModel)) {
        if (titems.includes(i)) {
            TopTableParams[i] = cloneDeep(dynamicFormRef.formModel[i]);
        }
    }
    if (['equipshift', 'equipment'].includes(TopTableParams['Interval']) && TopTableParams['Equipment'].length == 0) {
        closeFullLoading();
        return message.warning('請選擇機台,否則在X軸 無法show 全部機台');
    } else {
        try {
            const {
                Date: {
                    Data: { Table: Data }, //表格数据
                    Data1: { Table: Data1 }, //用於計算最後四行
                    Data2: { Table: Data2 },//倒数第四行的生產台數计算
                    Date: { Table: Date },//時間軸
                }
            } = await GetCommon(TopTableParams);
            let middleProps: string[] = [];
            let middleColumns: Object[] = [];
            Date.forEach(item => {
                middleProps.push(item["TRANS_DATE"])
                middleColumns.push(
                    {
                        title: item["TRANS_DATE"],
                        align: 'center',
                        dataIndex: item["TRANS_DATE"],
                    }
                )
            })
            // 最後面的數據，多出來的合并邏輯，相同的code，如果是最大的就保留
            let finalData: Object[] = [];
            let code: string = "";
            let sumindex = {
                DOWN: {
                    index: 0,
                    SUM: 0,
                    CODE: "DOWN",
                    C_DESC: "Down Sum",
                    bar: [],
                },
                RUN: {
                    index: 0,
                    SUM: 0,
                    CODE: "RUN",
                    C_DESC: "Run Sum",
                    bar: [],
                },
                IDLE: {
                    index: 0,
                    SUM: 0,
                    CODE: "IDLE",
                    C_DESC: "Idle Sum",
                    bar: [],
                },
                Other: {
                    index: 0,
                    SUM: 0,
                    CODE: "Other",
                    C_DESC: "Othen Sum",
                    bar: [],
                }
            } as any;
            for (const key in sumindex) {
                middleProps.forEach(mitem => {
                    sumindex[key][mitem] = 0;
                })
            }
            // 最後面的數據，多出來的合并邏輯，相同的code，如果日期是最大的就保留
            Data.forEach((item, index) => {
                if (item.CODE == code) {
                    middleProps.forEach(ditem => {
                        if (item[ditem] >= finalData[finalData.length - 1][ditem]) {
                            finalData[finalData.length - 1][ditem] = item[ditem]
                        }
                    })
                    // 由于在上面重新筛选了，所以在后端计算sum没有了意义
                    finalData[finalData.length - 1]["SUM"] = 0;
                    middleProps.forEach(mitem => {
                        finalData[finalData.length - 1]["SUM"] += finalData[finalData.length - 1][mitem]
                    });
                    finalData[finalData.length - 1]["SUM"] = Number(finalData[finalData.length - 1]["SUM"].toFixed(2))
                } else {
                    finalData.push(item)
                }
                // 保存上一個code
                code = item.CODE;
            })
            finalData.forEach((item: any, index) => {
                switch (item.ENTITY_STATUS) {
                    case "DOWN":
                        sumindex.DOWN.index = index;
                        middleProps.forEach(mitem => {
                            sumindex.DOWN[mitem] += Number(Number(item[mitem]).toFixed(2))
                            sumindex.DOWN[mitem] = Number(Number(sumindex.DOWN[mitem]).toFixed(2))
                        })
                        break;
                    case "RUN":
                        sumindex.RUN.index = index
                        middleProps.forEach(mitem => {
                            sumindex.RUN[mitem] += Number(Number(item[mitem]).toFixed(2))
                            sumindex.RUN[mitem] = Number(Number(sumindex.RUN[mitem]).toFixed(2))

                        })
                        break;
                    case "IDLE":
                        sumindex.IDLE.index = index
                        middleProps.forEach(mitem => {
                            sumindex.IDLE[mitem] += Number(Number(item[mitem]).toFixed(2))
                            sumindex.IDLE[mitem] = Number(Number(sumindex.IDLE[mitem]).toFixed(2))

                        })
                        break;
                    case null:
                        sumindex.Other.index = index
                        middleProps.forEach(mitem => {
                            sumindex.Other[mitem] += Number(Number(item[mitem]).toFixed(2))
                            sumindex.Other[mitem] = Number(Number(sumindex.Other[mitem]).toFixed(2))
                        })
                        break;
                    default:
                        break;
                }
            })
            // 用于计算百分比，Sum/totalsum
            let totalsum = 0.0;
            for (const key in sumindex) {
                middleProps.forEach(mitem => {
                    sumindex[key]["SUM"] += sumindex[key][mitem];
                    sumindex[key]["SUM"] = Number(Number(sumindex[key]["SUM"]).toFixed(2))
                })

            }
            //遍历对象属性
            Object.keys(sumindex).forEach((key, index) => {
                finalData.splice(sumindex[key]["index"] + index + 1, 0, sumindex[key]);
                totalsum += sumindex[key]["SUM"];
            })
            // 计算百分比
            finalData.forEach(item => {
                item["Rate"] = ((Number(item["SUM"]) / Number(totalsum)) * 100).toFixed(2) + '%';
            })
            // 最後四行數據處理
            let productnum = { CODE: '', C_DESC: '生產台數', Rate: '生產台數', SUM: 0 };
            let uptimerate = {
                CODE: '',
                C_DESC: 'Up Time Rate',
                Rate: 'Up Time Rate',
                SUM: 0 as any,
                SUMTIME: 0,
                up: 0,
            };
            let performancerate = {
                CODE: '',
                C_DESC: '性能稼動率',
                Rate: '性能稼動率',
                SUM: 0 as any,
                RUN_TIME: 0,
                up: 0,
            };
            let comprehensivecroprate = {
                CODE: '',
                C_DESC: '綜合稼動率',
                Rate: '綜合稼動率',
                SUM: 0 as any,
                RUN_TIME: 0,
                SUMTIME: 0,
            };
            /* 
            计算公式：UP = Run + IDEL   Total = Run + IDEL + Down 
            生产台数：直接取值Date2的值
            up time rate:(up/total)
            性能稼动率：(run/up)
            综合稼动率:(run/total)
            */
            Data2.forEach((item) => {
                productnum['SUM'] += item['SUMTIME'];
                productnum[item['TRANS_DATE']] = item['SUMTIME'];
            });
            Data1.forEach((item, index) => {
                let up = item['RUN_TIME'] + item['IDLE_TIME'];
                uptimerate['up'] += up;
                uptimerate['SUMTIME'] += item['SUMTIME'];
                uptimerate[item['TRANS_DATE']] = ((up / item['SUMTIME']) * 100).toFixed(2) + '%';

                performancerate['up'] += up;
                performancerate['RUN_TIME'] += item['RUN_TIME'];
                performancerate[item['TRANS_DATE']] = ((item['RUN_TIME'] / up) * 100).toFixed(2) + '%';

                comprehensivecroprate['SUMTIME'] += item['SUMTIME'];
                comprehensivecroprate['RUN_TIME'] += item['RUN_TIME'];
                comprehensivecroprate[item['TRANS_DATE']] =
                    ((item['RUN_TIME'] / item['SUMTIME']) * 100).toFixed(2) + '%';
                if (index == Data1.length - 1) {
                    comprehensivecroprate['SUM'] =
                        ((comprehensivecroprate['RUN_TIME'] / comprehensivecroprate['SUMTIME']) * 100).toFixed(2) +
                        '%';
                    performancerate['SUM'] =
                        ((performancerate['RUN_TIME'] / performancerate['up']) * 100).toFixed(2) + '%';
                    uptimerate['SUM'] = ((uptimerate['up'] / uptimerate['SUMTIME']) * 100).toFixed(2) + '%';
                }
            });
            /* Data2.forEach((item) => {
                productnum['SUM'] += item['SUMTIME'];
                productnum[item['TRANS_DATE']] = item['SUMTIME'];
            });
            let Up = {} as any;
            let Total = {} as any;
            [...middleProps, "SUM"].forEach(item => {
                Up[item] = Number((sumindex["RUN"][item] + sumindex["IDLE"][item]).toFixed(2))
                Total[item] = Number((sumindex["RUN"][item] + sumindex["IDLE"][item] + sumindex["DOWN"][item]).toFixed(2));
                uptimerate[item] = ((Up[item] / Total[item]) * 100).toFixed(2) + "%";
                performancerate[item] = ((sumindex["RUN"][item] / Up[item]) * 100).toFixed(2) + "%";
                comprehensivecroprate[item] = ((sumindex["RUN"][item] / Total[item]) * 100).toFixed(2) + "%";
            }) */
            finalData.push(productnum, uptimerate, performancerate, comprehensivecroprate)
            PageSetting.TopList = finalData;
            PageSetting.TopColumns = [
                ...baseColumns,
                ...middleColumns,
                {
                    title: '總計',
                    align: 'center',
                    dataIndex: 'SUM',
                    key: 'SUM'
                },
                {
                    title: '百分比',
                    align: 'center',
                    dataIndex: 'Rate',
                    key: 'Rate'
                }
            ];
            console.log("输出sumindex", sumindex);
            // echart图的逻辑
            // total = down,idle,run,other的middleProps，sum相加（绿色行的四行的sum相加）
            // 每个柱状图 = 当前列 / total
            let Irowsum = getfinalsum(Object.values(sumindex), middleProps, 'CODE');
            let BarData: Object[] = [];
            Object.keys(sumindex).forEach((key, index) => {
                middleProps.forEach(mitem => {
                    sumindex[key].bar.push((sumindex[key][mitem] / Irowsum[mitem]) * 100);
                })
                BarData.push({
                    name: key,
                    type: 'bar',
                    barWidth: '15%',
                    data: sumindex[key].bar,
                    stack: 'CyleTime(Hour)',
                    emphasis: {
                        //折线图的高亮状态。
                        focus: 'series' //聚焦当前高亮的数据所在的系列的所有图形。
                    },
                    //背景色
                    // showBackground: true,
                    backgroundStyle: {
                        color: 'rgba(180, 180, 180, 0.2)'
                    },
                    itemStyle: function style(i: string) {
                        let color;
                        // color: ['綠色#0bf10b', '藍色#0d8add', '紅色#ee0606']
                        switch (i) {
                            case "DOWN":
                                color = "#cedb16";
                                break;
                            case "RUN":
                                color = "#0d98e9";
                                break;
                            case "OTHER":
                                color = "#e90d61";
                                break;
                            case "IDLE":
                                color = "#0de93d";
                                break;
                            default:
                                break;
                        }
                        return {//自定义颜色
                            normal: { color: color },
                        }
                    }(key),
                });
            })

            console.log("输出bar", sumindex);

            PageSetting.EchartOptions = {
                title: [
                    {
                        text: 'LCMA Cycle Time Chart',
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
                    data: Object.keys(sumindex),
                    // 设置与图容器的距离
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
                        data: middleProps
                    }
                ],
                yAxis: [
                    {
                        type: 'value',
                        name: 'Percent(%)',
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
                series: [...BarData]
            };
            console.log("输出PageSetting.EchartOptions", PageSetting.EchartOptions);

            closeFullLoading();
        } catch (error) {
            console.log(error);
            message.error('數據處理失敗!');
            closeFullLoading();
        }
    }
}
if (history.scrollRestoration) {
    history.scrollRestoration = 'auto';
}
onMounted(async () => {
    await mountedowner(optionsdata, FactoryType);
    await mountedOper(optionsdata, FactoryType);
    await mounted(optionsdata, FactoryType);
    await mountedEquip(optionsdata, FactoryType, 0);
});
</script>

