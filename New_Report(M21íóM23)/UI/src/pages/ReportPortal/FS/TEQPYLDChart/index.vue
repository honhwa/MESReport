<template>
    <div class="EquipmentUtilization">
        <a-card>
            <a-row>
                <a-col :span="24 - PageParams.TableSpan" class="ScrollTable" v-show="PageParams.ShowMenu">
                    <SchemaForm  class="ScrollForm" @submit="confirm"> </SchemaForm>
                </a-col>
                <a-col :span="PageParams.TableSpan">
                    <a-button type="primary" @click="changeView(1)">{{ PageSetting.buttonText }}</a-button>

                    <a-button type="primary" @click="changeView2">{{ PageSetting.buttonText2 }}</a-button>
                    <a-button type="primary" @click="changeView(2)">{{ PageSetting.buttonText3 }}</a-button>

                    <!-- v-if="!PageSetting.isShowTable"></V3chart> -->
                    <V3chart
                        width="100vh"
                        height="85vh"
                        :options="PageSetting.EchartOptions"
                        isClick
                        v-show="PageSetting.TopList.length != 0 && PageSetting.button1"
                    ></V3chart>
                    <div class="table" v-show="PageSetting.TopList.length != 0 && PageSetting.button2">
                        <DynamicTable
                            row-key="id"
                            header-title="LCMA Cycle Time "
                            :columns="PageSetting.TopColumns"
                            showFavorite
                            sticky
                            :style="{ display: 'inline-block' }"
                            :PageParams="PageParams"
                            :key="PageSetting.TopColumns"
                            bordered
                            size="small"
                            :data-source="PageSetting.TopList"
                            :pagination="false"
                            :scroll="{ y: 350 }"
                            export-file-name="导出"
                            :row-class-name="getRowClassNmae"
                        >
                            <template #export-button>
                                <a-button type="success"> 导出 </a-button>
                            </template>
                        </DynamicTable>
                    </div>
                    <V3chart
                        width="100vh"
                        height="85vh"
                        :options="PageSetting.EchartOptions2"
                        isClick
                        v-show="PageSetting.TopList.length != 0 && PageSetting.button3"
                    ></V3chart>
                </a-col>
            </a-row>
        </a-card>
    </div>
</template>
<script lang="tsx" setup>
import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table';
import { useTable } from '@/components/core/dynamic-table';
import { reactive } from '@vue/reactivity';
import { useForm } from '@/components/core/schema-form';
import { GetCommon } from '@/api/pages/common';
import { onMounted } from '@vue/runtime-core';
import { message } from 'ant-design-vue';
import { getfinalsum, Inserttotalrow, getrowsum, showHidden, getdiffdate } from '@/pages/ReportPortal/utils/common';
import { cloneDeep } from 'lodash';
import V3chart from '@/components/charts/V3chart.vue';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { getPovitData, PovitParams } from '@/utils/reportcommon/provit';
const UseParamsAdd = AddCommonPageParams();
let FactoryType: any = UseParamsAdd.Fab;

function getImport(site: any) {
    const { schemas, optionsdata } = require(`@/pages/ReportPortal/FS/TEQPYLDChart/${site}/form-schema`);
    const { baseColumns } = require(`@/pages/ReportPortal/FS/TEQPYLDChart/${site}/table`);
    return { schemas, optionsdata, baseColumns };
}
const { schemas, optionsdata, baseColumns } = getImport(FactoryType);
let usechart = 'chart';
// 頁面數據
let PageSetting = reactive({
    TopList: [] as any,
    TopColumns: [] as any,
    EchartOptions: {},
    EchartOptions2: {},
    Message: {},
    isShowTable: false,
    buttonText: '圖1',
    buttonText2: '表格',
    buttonText3: '圖2',
    button1: true,
    button2: false,
    button3: false
});
const [DynamicTable] = useTable();
const { mounted, mountedowner, mountedOper, mountedEquip } = useRemoteSelect();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas:schemas(FactoryType),
    actionColOptions: {
        span: 24
    },
    fieldMapToTime: [['fieldTime', ['vDate_s', 'vDate_e1'], 'YYYY-MM-DD']],
    layout: 'vertical'
});

function changeView(type: number) {
    if (type == 1) {
        PageSetting.button1 = true;
         PageSetting.button2 = false;
    PageSetting.button3 = false;
         usechart = 'chart';

    } else {
         PageSetting.button1 = false;
           PageSetting.button2 = false;
        PageSetting.button3 = true;
    usechart = 'chart2';
    // console.log("輸出echart圖的數據",PageSetting.EchartOptions.legend  = []);
    // console.log("輸出echart圖的數據",PageSetting.EchartOptions.xAxis  = []);
    // console.log("輸出echart圖的數據",PageSetting.EchartOptions.ser  = []);

    }
   
   
   
}
function changeView2() {
    //第二個圖表

    PageSetting.button1 = false;
    PageSetting.button2 = true;
    PageSetting.button3 = false;
   
}
let TopTableParams = reactive({
    name: 'Date',
    apiName: 'TEQPYLDChartDataSet',
    FactoryType: UseParamsAdd.Fab,
    G_FAC: 'A',
    FromDate: '',
    ToDate: '',
    Shift: '',
    Interval: '',
    Floor: '',
    Equipment: []
});
function getRowClassNmae(record: Object) {
    if (record && Object.keys(record).length != 0) {
        for (const [key, value] of Object.entries(record)) {
            if (typeof value == 'string' && value.includes('Sum')) {
                return 'Green';
            }
        }
    }
}
async function confirm() {
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
        message.warning('請選擇機台,否則在X軸 無法show 全部機台');
    } else {
        const {
            Date: {
                Data: { Table: Data }, //表格数据
                Data1: { Table: Data1 }, //图表数据
                Data2: { Table: Data2 } //图表2数据
            }
        } = await GetCommon(TopTableParams);
        let middleProps: string[] = [];
        let middleColumns: Object[] = [];
        // 获取中间表头属性
        let tempDate = getdiffdate(TopTableParams['FromDate'], TopTableParams['ToDate']);
        switch (TopTableParams['Interval']) {
            case 'shift':
                tempDate.forEach((item) => {
                    middleProps.push(item + 'A');
                    middleProps.push(item + 'B');
                    middleColumns.push({
                        title: item + 'A',
                        align: 'center',
                        dataIndex: item + 'A',
                        key: item + 'A'
                    });
                    middleColumns.push({
                        title: item + 'B',
                        align: 'center',
                        dataIndex: item + 'B',
                        key: item + 'B'
                    });
                });
                break;
            case 'day':
                tempDate.forEach((item) => {
                    middleProps.push(item);
                    middleColumns.push({
                        title: item,
                        align: 'center',
                        dataIndex: item,
                        key: item
                    });
                });
                break;
            case 'week':
                Data1.forEach((item) => {
                    middleProps.push(item['TRANS_DATE']);
                });
                middleProps.sort((a, b) => {
                    return Number(a) - Number(b);
                });
                middleProps.forEach((item) => {
                    middleColumns.push({
                        title: item,
                        align: 'center',
                        dataIndex: item,
                        key: item
                    });
                });
                break;
            case 'month':
                Data1.forEach((item) => {
                    middleProps.push(item['TRANS_DATE']);
                });
                middleProps.sort((a, b) => {
                    return new Date(a).getTime() - new Date(b).getTime();
                });
                middleProps.forEach((item) => {
                    middleColumns.push({
                        title: item,
                        align: 'center',
                        dataIndex: item,
                        key: item
                    });
                });

                break;
            case 'equipment':
            case 'equipshift':
                Data1.forEach((item) => {
                    middleProps.push(item['TRANS_DATE']);
                    middleColumns.push({
                        title: item['TRANS_DATE'],
                        align: 'center',
                        dataIndex: item['TRANS_DATE'],
                        key: item['TRANS_DATE']
                    });
                });
                break;
            default:
                break;
        }
        // 表格数据
        let config: PovitParams = {
            Data: Data,
            povitColumn: {
                TRANS_DATE: 'SUMTIME'
            },
            changeColumn: middleProps,
            repeatColumn: ['CODE', 'C_DESC', 'ENTITY_STATUS', 'EQ_STATUS']
        };

        console.log('表格數據', config);

        let povitData = getPovitData(config);
        // 柱圖數據
        // let povitData_chart = getPovitData(config_chart);
        PageSetting.TopColumns = [
            ...baseColumns,
            ...middleColumns,
            {
                title: '總計',
                align: 'center',
                dataIndex: 'Sum',
                key: 'Sum'
            },
            {
                title: '百分比',
                align: 'center',
                dataIndex: 'Rate',
                key: 'Rate'
            }
        ];
        // 数据多出一条为undefiend的数据，后面再找原因
        povitData = povitData.filter((item) => {
            return typeof item != 'undefined';
        });
        let { insertrowList, insertIndexList } = Inserttotalrow(
            povitData,
            ['TYPE'],
            middleProps,
            'C_DESC',
            'Sum',
            false,
            false
        );
        // 数组头部添加新的值
        console.log('povitData測試fanish1', povitData);
        insertIndexList.unshift(0);
        // 数组头部移除最后一位数字
        insertIndexList.pop();
        // 插入之后的位置
        let afterinsert: number[] = [];
        for (let i = 0; i < insertIndexList.length; i++) {
            povitData.splice(insertIndexList[i] + i, 0, insertrowList[i]);
            afterinsert.push(insertIndexList[i] + i);
        }
        console.log('測試insertrowList', insertrowList);
        console.log('測試insertIndexList', insertIndexList);
        console.log('測試afterinsert', afterinsert);
        afterinsert.forEach((item, index) => {
            middleProps.forEach((titem) => {
                // povitData[item][titem] = povitData[item][titem].toFixed(2);
            });
        });
        let flag = false;
        povitData.forEach(function (item, index) {
            if (item) {
                //判定sum行位置
                afterinsert.includes(index) ? (flag = true) : (flag = false);
                if (flag == true) {
                    // && povitData[index+1]["TYPE"]=="A"
                    console.log('輸出itemindexTYPE', povitData[index]['TYPE']);

                    switch (povitData[index + 1]['TYPE']) {
                        case 'A':
                            povitData[index]['CODE'] = `A`;
                            povitData[index]['C_DESC'] = `PM Sum`;
                            povitData[index]['TYPE'] = `top`;
                            break;
                        case 'B':
                            povitData[index]['CODE'] = `B`;
                            povitData[index]['C_DESC'] = `Down Sum`;
                            povitData[index]['TYPE'] = `top`;
                            break;
                        case 'C':
                            povitData[index]['CODE'] = `C`;
                            povitData[index]['C_DESC'] = `Idle Sum`;
                            povitData[index]['TYPE'] = `top`;
                            break;

                        default:
                            break;
                    }
                }
            }
        });

        console.log('插入測試fanish1', povitData);
        // 添加总计列
        povitData.push(getrowsum(povitData, middleProps, 'Sum'));

        console.log('插入測試fanish2', povitData);
        povitData.forEach((item) => {
            // 保留两位小数
            if (item) {
                for (let i in item) {
                    if ([...middleProps, 'Sum'].includes(i) && typeof item[i] != 'undefined') {
                        item[i] = item[i].toFixed(2);
                    }
                }
            }
        });

        // 所有的总计
        let totalsum = 0.0;
        afterinsert.forEach((item) => {
            totalsum += Number(povitData[item]['Sum']);
        });
        let testsum = 0;
        insertrowList.forEach((item) => {
            testsum += Number(item['2022-12-08B']);
        });
        // 数据多出一条为undefiend的数据，后面再找原因
        povitData = povitData.filter((item) => {
            return typeof item != 'undefined';
        });
        // 计算百分比
        povitData.forEach((item) => {
            if (item['Sum']) {
                item['Rate'] = ((Number(item['Sum']) / totalsum) * 100).toFixed(2) + '%';
            }
        });

        // 最後四行數據處理
        let uptimerate = { TYPE: 'UPTIME', CODE: '', C_DESC: 'Up Time Rate', Rate: 'Up Time Rate', Sum: 0 };
        let productnum = { TYPE: 'EQPROTECT', CODE: '', C_DESC: '設備保養率', Rate: '設備保養率', Sum: 0 };
        let performancerate = { TYPE: 'EQDOWN', CODE: '', C_DESC: '設備當機率', Rate: '設備當機率', Sum: 0 };
        let comprehensivecroprate = { TYPE: 'EQRELEX', CODE: '', C_DESC: '設備空閒率', Rate: '設備空閒率', Sum: 0 };
        Data2.forEach((item) => {
            //productnum["Sum"] += item["SUMTIME"];
            productnum[item['TRANS_DATE']] = ((item['A'] / item['SUMTIME']) * 100).toFixed(2) + '%';
            uptimerate[item['TRANS_DATE']] = ((1 - item['B'] / item['SUMTIME']) * 100).toFixed(2) + '%';
            performancerate[item['TRANS_DATE']] = ((item['B'] / item['SUMTIME']) * 100).toFixed(2) + '%';
            comprehensivecroprate[item['TRANS_DATE']] = ((item['C'] / item['SUMTIME']) * 100).toFixed(2) + '%';
            productnum['Sum'] += (item['A'] / item['SUMTIME']) * 100;
            performancerate['Sum'] += (item['B'] / item['SUMTIME']) * 100;
            comprehensivecroprate['Sum'] += (item['C'] / item['SUMTIME']) * 100;
        });
        productnum['Sum'] = (productnum['Sum'] / Data2.length).toFixed(2) + '%';
        performancerate['Sum'] = (performancerate['Sum'] / Data2.length).toFixed(2) + '%';
        comprehensivecroprate['Sum'] = (comprehensivecroprate['Sum'] / Data2.length).toFixed(2) + '%';
        povitData.push(uptimerate, productnum, performancerate, comprehensivecroprate);

        console.log('測試最後四行數據', povitData);
        //  堆叠柱图
        let legendData: string[] = [];
        // 获取插入的四行
        let Irow: object[] = [];
        afterinsert.forEach((item) => {
            if (!legendData.includes(povitData[item]['C_DESC'])) {
                legendData.push(povitData[item]['C_DESC']);
            }

            Irow.push(cloneDeep(povitData[item]));
        });

        povitData = povitData.filter((item) => {
            //過濾數據
            return item.TYPE && item.TYPE != null;
        });
        let Irowsum = getfinalsum(Irow, middleProps, 'CODE');
        let Robj = {};
        Irow.forEach((item) => {
            // 每行除以最后一行
            middleProps.forEach((mitem) => {
                item[mitem] = (item[mitem] / Irowsum[mitem]) * 100;
                if (!Robj[item['C_DESC']]) {
                    Robj[item['C_DESC']] = [item[mitem]];
                } else {
                    Robj[item['C_DESC']].push(item[mitem]);
                }
            });
        });
        console.log('輸出柱状图的leggeng', legendData);
        let BarData: Object[] = [];
        let BarData2: Object[] = [];
        console.log(Robj);

        console.log('輸出堆叠圖所需的數據1', Data1);
        console.log('輸出堆叠圖所需的數據2', Data2);

        /*  for (let i in Robj) {
             BarData.push({
                 name: i,
                 type: "bar",
                 barWidth: "15%",
                 data: Robj[i],
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
         } */
        let barData = {
            A_sum: Data1.map((item) => {
                return ((item.A / (item.A + item.B + item.C)) * 100).toFixed(1);
            }),
            B_sum: Data1.map((item) => {
                return ((item.B / (item.A + item.B + item.C)) * 100).toFixed(1);
            }),
            C_sum: Data1.map((item) => {
                return ((item.C / (item.A + item.B + item.C)) * 100).toFixed(1);
            })
        };
        let barData2 = {
            Up_Time_Rate: Data2.map((item) => {
                return ((1 - item.B / item.SUMTIME) * 100).toFixed(1);
            }),
            設備保養率: Data2.map((item) => {
                return ((item.A / item.SUMTIME) * 100).toFixed(1);
            }),
            設備當機率: Data2.map((item) => {
                return ((item.B / item.SUMTIME) * 100).toFixed(1);
            }),
            設備空閒率: Data2.map((item) => {
                return ((item.C / item.SUMTIME) * 100).toFixed(1);
            })
        };
        for (let i in barData) {
            BarData.push({
                name: i,
                type: 'bar',
                barWidth: '15%',
                data: barData[i],
                stack: 'CyleTime(Hour)',
                emphasis: {
                    //折线图的高亮状态。
                    focus: 'series' //聚焦当前高亮的数据所在的系列的所有图形。
                },
                //背景色
                // showBackground: true,
                backgroundStyle: {
                    color: 'rgba(180, 180, 180, 0.2)'
                }
            });
        }
        for (let i in barData2) {
            BarData2.push({
                name: i,
                type: 'bar',
                barWidth: '15%',
                data: barData2[i],
                stack: 'CyleTime(Hour)',
                emphasis: {
                    //折线图的高亮状态。
                    focus: 'series' //聚焦当前高亮的数据所在的系列的所有图形。
                },
                //背景色
                // showBackground: true,
                backgroundStyle: {
                    color: 'rgba(180, 180, 180, 0.2)'
                }
            });
        }
        PageSetting.TopList = povitData;
        let mylegend =reactive( ['A_sum', 'B_sum', , 'C_sum']); //柱圖
        let myxAxis = Data1.map((item) => {
            //x軸
            return item.TRANS_DATE;
        });
        let myxAxis2 = Data2.map((item) => {
                //x軸
                return item.TRANS_DATE;
            });
        let myBarData = BarData; //值
        if (usechart != 'chart') {
            mylegend = reactive(['Up_Time_Rate', '設備保養率', , '設備當機率', '設備空閒率']); //柱圖
            myxAxis = Data2.map((item) => {
                //x軸
                return item.TRANS_DATE;
            });
            myBarData = BarData2; //值
        }
        console.log('圖表顯示barData', barData);
        console.log('圖表顯示myBarData', myBarData);
        console.log('圖表顯示', usechart);

        PageSetting.EchartOptions = {
            title: [
                {
                    text: 'LCMA 測試設備良率推移圖',
                    left: 'center'
                }
            ],
            grid: {
                x: 0,
                y: 0,
                x2: 0,
                y2: 0,
                top: '15%',
                // top: "5%",
                left: '3%',
                right: '3%',
                bottom: '20%',
                // bottom: "10%",
                // height:"450px",
                containLabel: true
            },
            legend: {
                // 柱状图的leggeng
                data: mylegend, //["A_sum", "B_sum", , "C_sum"],

                // bottom: "bottom",
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
                trigger: 'item',
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
                    interval: 1,
                    axisLabel: {
                        color: 'rgba(36, 173, 254, 1)',
                        fontSize: '12'
                    },
                    axisLine: {
                        show: true
                    },
                    data: myxAxis
                    // Data1.map(item => {
                    //     return item.TRANS_DATE;
                    // }),
                }
            ],
            yAxis: [
                {
                    type: 'value',
                    name: 'Percent(%)',
                    splitLine: false,
                    axisLine: {
                        show: true
                    }
                }
            ],
            series: [...myBarData]
        };
        PageSetting.EchartOptions2 = {
            title: [
                {
                    text: 'LCMA 測試設備良率推移圖',
                    left: 'center'
                }
            ],
            grid: {
                x: 0,
                y: 0,
                x2: 0,
                y2: 0,
                top: '15%',
                // top: "5%",
                left: '3%',
                right: '3%',
                bottom: '20%',
                // bottom: "10%",
                // height:"450px",
                containLabel: true
            },
            legend: {
                // 柱状图的leggeng
                data: ['Up_Time_Rate', '設備保養率', , '設備當機率', '設備空閒率'], //["A_sum", "B_sum", , "C_sum"],

                // bottom: "bottom",
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
                trigger: 'item',
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
                    interval: 1,
                    axisLabel: {
                        color: 'rgba(36, 173, 254, 1)',
                        fontSize: '12'
                    },
                    axisLine: {
                        show: true
                    },
                    data: myxAxis2
                    // Data1.map(item => {
                    //     return item.TRANS_DATE;
                    // }),
                }
            ],
            yAxis: [
                {
                    type: 'value',
                    name: 'Percent(%)',
                    splitLine: false,
                    axisLine: {
                        show: true
                    }
                }
            ],
            series: [...BarData2]
        };
    }
}
onMounted(async () => {
   /*  await mountedowner(optionsdata, FactoryType);
    await mountedOper(optionsdata, FactoryType);
    await mounted(optionsdata, FactoryType);
    await mountedEquip(optionsdata, FactoryType, 0); */
});
</script>

<style lang="less" scoped>
.btn-rows {
    button {
        margin-right: 12px;
    }
}

.swapper {
    display: flex;
    flex-direction: column;
}

// .EquipmentUtilization :deep(.test) {
//     background-color: #80ff80 !important;
// }


</style>
