<template>
    <div class="CycleTime">
        <a-card>
            <a-row>
                <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                    <SchemaForm @submit="confirm"> </SchemaForm>
                </a-col>
                <a-col :span="PageParams.TableSpan">
                    <a-button type="primary" @click="changeView" v-show="PageSetting.TopList.length != 0">{{
                            PageSetting.buttonText
                    }}</a-button>
                    <V3chart width="100%" height="100%" :options="PageSetting.EchartOptions" isClick
                        @callbackFun="callbackFun" v-if="!PageSetting.isShowTable"></V3chart>
                    <!-- <span>Date:{{ dynamicFormRef.formModel["vDate_s"] }}~{{ dynamicFormRef.formModel["vDate_e1"] }}
                        Owner:{{ dynamicFormRef.formModel["owner_code"] }}</span> -->
                    <div v-else>
                        <DynamicTable row-key="id" header-title="LCMA Cycle Time " :columns="PageSetting.TopColumns"
                            showFavorite :PageParams="PageParams" sticky :key="PageSetting.TopColumns" bordered
                            :flexwidth="1" :data-source="PageSetting.TopList" :data-all="PageSetting.TopList"
                            :pagination="false" export-file-name="导出" :row-class-name="getRowClassName">
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
import {PageParams} from '@/components/core/dynamic-table/src/dynamic-table'
import { useTable } from '@/components/core/dynamic-table';
import { reactive} from '@vue/reactivity';
import { useForm } from '@/components/core/schema-form';
// import { cloneDeep } from 'lodash-es';
import { GetCommon } from '@/api/pages/common';
import {  onMounted } from '@vue/runtime-core';
import { cloneDeep } from 'lodash';
import V3chart from "@/components/charts/V3chart.vue";
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { getPovitData, PovitParams } from '@/utils/reportcommon/provit';



const UseParamsAdd = AddCommonPageParams()
let FactoryType: any = UseParamsAdd.Fab;

function getImport(site: any) {
    const { schemas, optionsdata, } = require(`@/pages/ReportPortal/${site}/CycleTime/LCM1/form-schema`);
    const { baseColumns } = require(`@/pages/ReportPortal/${site}/CycleTime/LCM1/table`);
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
const { mounted, InfiniteScroll, mountedowner, mountedOper, mountedEquip } = useRemoteSelect()


const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas:schemas(FactoryType),
    actionColOptions: {
        span: 24
    },
    fieldMapToTime: [['fieldTime', ['vDate_s', 'vDate_e1'], 'YYYY-MM-DD']],
    layout: "vertical"
});

function callbackFun(value: any) {
    console.log('点击了', value);
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
    apiName: "CycleTimeDataSet",
    // ReportType: "CycleTimeDataSet",
    FactoryType: FactoryType,
    G_FAC: "A",
    FromDate: "",
    ToDate: "",
    Interval: "",
    LcmOwner: [],
    ProdType: "",
    Product: [],
    Equipment: ["ALL"],
    Operation: [],
    building: "",
    Floor: "",
    Shift: "",
});
function getRowClassName(record: Object) {
    if (record && Object.keys(record).length != 0) {
        for (const [key, value] of Object.entries(record)) {
            if (
                typeof value == 'string' &&
                value.includes("time")
            ) {
                return 'GreenBold';
            }
        }
    }

}

async function confirm() {
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
        Data1: { Table: Data1 },//图标数据
        Data2: { Table: Data2 },//堆叠图数据
    } } = await GetCommon(TopTableParams)
    let middleProps: string[] = [];
    let middleColumns: Object[] = [];
    Data2.forEach(item => {
        middleProps.push(item["TRANS_DATE1"]);
        middleColumns.push({
            title: item["TRANS_DATE1"],
            align: 'center',
            dataIndex: item["TRANS_DATE1"],
            // className: "v-center",
            key: item["TRANS_DATE1"],
        })
    });
    PageSetting.TopColumns = [...baseColumns, ...middleColumns]
    Data1.forEach(item => {
        item["QTY"] = item["USETIME"] == 0 ? 0 : (item["USETIME"] / item["QTY"]).toFixed(2)
    })
    // 表格数据
    let config: PovitParams = {
        Data: Data1,
        povitColumn: {
            "TRANS_DATE1": 'QTY'
        },
        changeColumn: middleProps,
        repeatColumn: ["WORK_CTR"]
    }
    let povitData = getPovitData(config)

    let BarData: Object[] = [];

    for (let i = 0; i < povitData.length; i++) {
        BarData.push({
            name: povitData[i]["WORK_CTR"],
            type: "bar",
            barWidth: "15%",
            data: middleProps.map(item => povitData[i][item]),
            stack: "CyleTime(Hour)",
            emphasis: {//折线图的高亮状态。
                focus: "series",//聚焦当前高亮的数据所在的系列的所有图形。
            },
            //背景色
            // showBackground: true,
            backgroundStyle: {
                color: "rgba(180, 180, 180, 0.2)",
            }
        })

    }




    // 切割表格数据，Operaction>=1100  && <=2000的小计

    // Operaction>=2000  && <=2000的小计
    let inlineobj = cloneDeep(povitData[0]);
    inlineobj["WORK_CTR"] = "Inline cycle time";
    let totalobj = {};
    for (let i = 1; i < povitData.length; i++) {
        if (povitData[i]["WORK_CTR"] > 1000 && povitData[i]["WORK_CTR"] < 2000) {
            middleProps.forEach(item => {
                if (isNaN(inlineobj[item])) {
                    inlineobj[item] = 0;
                }
                if (!isNaN(povitData[i][item])) {
                    inlineobj[item] = Number(inlineobj[item]);
                    inlineobj[item] += Number(povitData[i][item])
                    inlineobj[item] = inlineobj[item].toFixed(2)
                }
            })
        } else {
            if (!inlineobj['index']) {
                inlineobj['index'] = i;
                totalobj = cloneDeep(inlineobj);
            }
            middleProps.forEach(item => {
                if (!isNaN(povitData[i][item])) {
                    totalobj[item] = Number(totalobj[item]);
                    totalobj[item] += Number(povitData[i][item])
                    totalobj[item] = totalobj[item].toFixed(2)
                }
            })
        }
    }
    totalobj["WORK_CTR"] = "Total cycle time";
    if (!inlineobj['index']) {
        inlineobj["WORK_CTR"] = "Total cycle time";
        povitData.push(inlineobj);
    } else {
        povitData.splice(inlineobj.index, 0, inlineobj);
        povitData.push(totalobj);
    }

    console.log("表格处理的数据", povitData);



    PageSetting.TopList = povitData;


    let legentData: any[] = [];
    Data1.forEach(item => {
        if (!legentData.includes(String(item["WORK_CTR"]))) {
            legentData.push(String(item["WORK_CTR"]))
        }
    })
    console.log("输出legentData", legentData);
    console.log("输出X轴数据", middleProps);


    PageSetting.EchartOptions = {
        title: [
            {
                text: "LCMA Cycle Time Chart",
                left: "center"
            }

        ],
        grid: {
            x: 0,
            y: 0,
            x2: 0,
            y2: 0,
            top: "15%",
            // top: "5%",
            left: "3%",
            right: "3%",
            bottom: "20%",
            // bottom: "10%",
            // height:"450px",
            containLabel: true,
        },
        legend: {
            // 柱状图的leggeng
            data: legentData,
            // bottom: "bottom",
            // 设置与图容器的距离
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
                data: middleProps,
            },
        ],
        yAxis: [
            {
                type: 'value',
                name: 'CyleTime(Hour)',
                splitLine: false,
                axisLine: {
                    show: true,
                },

            },
        ],
        series: [
            ...BarData

        ],
    }
}
onMounted(async () => {
    /* await mountedowner(optionsdata, FactoryType);
    await mountedOper(optionsdata, FactoryType);
    await mounted(optionsdata, FactoryType);
    await mountedEquip(optionsdata, FactoryType, 0); */
});
</script>

<style lang="less" scoped></style>
