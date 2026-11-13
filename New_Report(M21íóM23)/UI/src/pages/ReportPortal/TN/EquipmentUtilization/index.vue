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
                <V3chart 
                    width="100%"
                    height="100%"
                    :options="PageSetting.EchartOptions"
                    isClick
                    v-if="!PageSetting.isShowTable"
                ></V3chart>
                <div class="ScrollTable" v-else>
                    <DynamicTable
                        row-key="id"
                        :header-title="PageSetting.Title"
                        :columns="PageSetting.TopColumns"
                        showFavorite
                        sticky
                        :key="PageSetting.TopColumns"
                        bordered
                        size="small"
                        :data-source="PageSetting.TopList"
                        :data-all="PageSetting.TopList"
                        :pagination="false"
                        :export-file-name="PageSetting.Title"
                        :row-class-name="getRowClassNmae"
                        :scroll="{x:1500, y:700}"
                        :PageParams="PageParams"
                    >
                        <template #export-button>
                            <a-button type="success"> Export </a-button>
                        </template>
                    </DynamicTable>
                </div>
            </a-col>
        </a-row>
    </div>
</template>
<script lang="tsx" setup>
import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table';
import { useTable } from '@/components/core/dynamic-table';
import { reactive } from 'vue';
import { useForm } from '@/components/core/schema-form';
import { GetCommon } from '@/api/pages/common';
import { onMounted } from '@vue/runtime-core';
import { message } from 'ant-design-vue';
import { getfinalsum, Inserttotalrow, getrowsum, showHidden, getdiffdate } from '@/pages/ReportPortal/utils/common';
import { cloneDeep } from 'lodash';
import V3chart from '@/components/charts/V3chart.vue';
import useRemoteSelect from '@/pages/ReportPortal/utils/TNuseRemoteSelect';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { getPovitData, PovitParams } from '@/utils/reportcommon/provit';
import { useLoading } from '@/components/feat/Loading';
//import {optionsdata} from '@/pages/ReportPortal/Static/TNCommon-Form-schema';

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});

const UseParamsAdd = AddCommonPageParams();
let FactoryType: any = UseParamsAdd.Fab;
function getImport(site: any) {
    //let { optionsdata } = require('@/pages/ReportPortal/Static/TNCommon-Form-schema');
    const { schemas} = require(`@/pages/ReportPortal/TN/EquipmentUtilization/MOD/form-schema`);
    const { baseColumns } = require(`@/pages/ReportPortal/TN/EquipmentUtilization/MOD/table`);
    return { schemas, baseColumns };
}
const { schemas, baseColumns } = getImport('TN');
// 頁面數據
let PageSetting = reactive({
    TopList: [] as any,
    TopColumns: [] as any,
    EchartOptions: {},
    Message: {},
    isShowTable: false,
    buttonText: '切换表格',
    Title:''
});

const [DynamicTable] = useTable();
const { mounted, mountedEQP } = useRemoteSelect();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas: schemas(FactoryType),
    actionColOptions: {
        span: 24
    },
    fieldMapToTime: [['fieldTime', ['vDate_s', 'vDate_e1'], 'YYYY-MM-DD']],
    layout: 'vertical'
});

function changeView() {
    PageSetting.isShowTable = !PageSetting.isShowTable;
    if (PageSetting.isShowTable) {
        PageSetting.buttonText = '切换圖表';
    } else {
        PageSetting.buttonText = '切换表格';
    }
}

PageSetting.Title = `${UseParamsAdd.Fab} Equipment Utilization`;   

let TopTableParams = reactive({
    name: 'Date',
    apiName: 'TN_EquipmentUtilizationDataSet',
    FactoryType: FactoryType,
    G_FAC: 'A',
    vDate_s: '',
    vDate_e1: '',
    Shift: '',
    Interval: '',
    Floor: '',
    EQPID: []
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
async function confirm(values) {
    openFullLoading();
    PageSetting.TopList = [];
    PageSetting.TopColumns = [];
    //給ParamsData賦值
    let values1 = Object.keys(TopTableParams);
    for (let i in cloneDeep(dynamicFormRef.formModel)) {
        if (values1.includes(i)) {
            TopTableParams[i] = cloneDeep(dynamicFormRef.formModel[i]);
        }
    }
    if (['equipshift', 'equipment'].includes(TopTableParams['Interval']) && TopTableParams['EQPID'].length == 0) {
        message.warning('請選擇機台,否則在X軸 無法show 全部機台');
        closeFullLoading();
    } else {
        try {
            const {
                Date: {
                    Data: { Table: Data }, //表格數據
                    Data1: { Table: Data1 }, //圖數據
                    Data2: { Table: Data2 }, //生產片數
                    Data3: { Table: Data3 } //Up Time Rate / 性能稼動率 / 總合稼動率 數據
                }
            } = await GetCommon({ ...TopTableParams, ...values });
            let middleProps: string[] = [];
            let middleColumns: Object[] = [];
            // 获取中间表头属性
            let tempDate = getdiffdate(TopTableParams['vDate_s'], TopTableParams['vDate_e1']);
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
            let povitData = getPovitData(config);
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

            let clonData: object[] = [];
            clonData.push(cloneDeep(povitData));

            let { insertrowList, insertIndexList } = Inserttotalrow(
                povitData,
                ['RPT_STATUS'],
                middleProps,
                'C_DESC',
                'Sum',
                false,
                false
            );
            debugger
            // 数组头部添加新的值
            insertIndexList.unshift(0);
            // 数组头部移除最后一位数字
            insertIndexList.pop();
            // 插入之后的位置
            let afterinsert: number[] = [];
            let eqpStatus: string[] = [];
            for (let i = 0; i < insertIndexList.length; i++) {
                povitData.splice(insertIndexList[i] + i, 0, insertrowList[i]);
                afterinsert.push(insertIndexList[i] + i);
                eqpStatus.push(clonData[0][insertIndexList[i]]['RPT_STATUS']);
            }
            debugger
            afterinsert.forEach((item, index) => {
                middleProps.forEach((titem) => {
                    if (povitData[item][titem]) 
                    {
                        povitData[item][titem] = povitData[item][titem].toFixed(2);
                    }
                });
                switch (eqpStatus[index]) {
                    case "DOWN":
                        povitData[item]['CODE'] = `DOWN`;
                        povitData[item]['C_DESC'] = `Down Sum`;
                        break;
                    case "IDEL":
                        povitData[item]['CODE'] = `IDLE`;
                        povitData[item]['C_DESC'] = `Idle Sum`;
                        break;
                    case "RUN":
                        povitData[item]['CODE'] = `RUN`;
                        povitData[item]['C_DESC'] = `Run Sum`;
                        break;
                    case "OTHER":
                        povitData[item]['CODE'] = `OTHER`;
                        povitData[item]['C_DESC'] = `Other Sum`;
                        break;
                    default:
                        break;
                }
            });
            // 添加总计列
            povitData.push(getrowsum(povitData, middleProps, 'Sum'));
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
            let productnum = { CODE: '', C_DESC: '生產片數', Rate: '生產片數', Sum: 0 };
            let uptimerate = { CODE: '', C_DESC: 'Up Time Rate', Rate: 'Up Time Rate', Sum: 0 };
            let performancerate = { CODE: '', C_DESC: '性能稼動率', Rate: '性能稼動率', Sum: 0 };
            let comprehensivecroprate = { CODE: '', C_DESC: '綜合稼動率', Rate: '綜合稼動率', Sum: 0 };
            Data2.forEach((item) => {
                productnum['Sum'] += item['SUMTIME'];
                productnum[item['TRANS_DATE']] = item['SUMTIME'];
            });
            
            Data3.forEach(item => {
                switch (item["C_DESC"]) {
                    case 'Up Time Rate':
                        uptimerate["C_DESC"] = item["C_DESC"]
                        uptimerate["Sum"] = item["SUMTIME"]
                        uptimerate[item['TRANS_DATE']] = item['SUMTIME'];
                        uptimerate["Rate"] = item["C_DESC"]+"%"
                    case '性能稼動率':
                        performancerate["C_DESC"] = item["C_DESC"]
                        performancerate["Sum"] = item["SUMTIME"]
                        performancerate[item['TRANS_DATE']] = item['SUMTIME'];
                        performancerate["Rate"] = item["C_DESC"]+"%"
                    case '綜合稼動率':
                        comprehensivecroprate["C_DESC"] = item["C_DESC"]
                        comprehensivecroprate["Sum"] = item["SUMTIME"]
                        comprehensivecroprate[item['TRANS_DATE']] = item['SUMTIME'];
                        comprehensivecroprate["Rate"] = item["C_DESC"]+"%"    
                }
            })
            povitData.push(productnum, uptimerate, performancerate, comprehensivecroprate);
            

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
            let BarData: Object[] = [];
            for (let i in Robj) {
                BarData.push({
                    name: i,
                    type: 'bar',
                    barWidth: '15%',
                    data: Robj[i],
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
            PageSetting.EchartOptions = {
                title: [
                    {
                        text: UseParamsAdd.Fab + ' Equipment UtilizationChart',
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
                    data: legendData,
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
                        data: middleProps
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
                series: [...BarData]
            };
            closeFullLoading();
        } catch (error) {
            console.log(error);
            message.error('數據處理失敗!');
            closeFullLoading();
        }
    }
}
onMounted(async () => {
    //await mounted(optionsdata,UseParamsAdd.Fab);    
    //await mountedEQP(optionsdata,UseParamsAdd.Fab, "Equipment Utilization");
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

.EquipmentUtilization :deep(.test) {
    background-color: #80ff80 !important;
}
</style>
