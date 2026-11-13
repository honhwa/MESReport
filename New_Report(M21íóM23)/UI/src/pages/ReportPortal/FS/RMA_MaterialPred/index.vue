<template>
    <div class="RMA_MaterialPred">
        <a-row>
            <a-col :span="24 - PageParams.TableSpan" class="baseModel" v-show="PageParams.ShowMenu">
                <!-- <SchemaForm1 @submit="confirm1" v-if="showTable"  class="ScrollForm"> </SchemaForm1> -->
                <!-- <SchemaForm @submit="confirm" v-if="showSelect" :showTextPosition="'center'"> </SchemaForm> -->
                <!-- <div v-if="showSelect">
                    <a-button type="primary" size="small " @click="SendQuery(2)">進入資料</a-button>
                    </div> -->
                <SchemaForm class="ScrollForm" @submit="confirm" v-if="showSelect" :showTextPosition="'center'">
                </SchemaForm>
               
            </a-col>
            <a-col :span="PageParams.TableSpan">
                <a-button type="primary" @click="changeView">{{ PageSetting.buttonText }}</a-button>
                <a-button type="primary" @click="changeView2">{{ PageSetting.buttonText2 }}</a-button>
                <a-button type="primary" @click="changeView3">{{ PageSetting.buttonText3 }}</a-button>
                <V3chart
                    width="100vh"
                    height="85vh"
                    :options="PageSetting.EchartOptions"
                    isClick
                    v-show="PageSetting.button1"
                ></V3chart>
                <div  ref="report" v-show="PageSetting.button2">
                    <DynamicTable
                        row-key="RMA_MaterialPred"
                        header-title="Export RMA_MaterialPred Data"
                        :columns="PageSetting.Columns"
                        showFavorite
                        :PageParams="PageParams"
                        :key="PageSetting.TopColumns"
                        bordered
                        size="small"
                        :data-source="PageSetting.List"
                        :data-all="PageSetting.moreData"
                        :pagination="false"
                        :scroll="{ x: PageSetting.x, y: 750 }"
                        export-file-name="导出"
                        :row-class-name="getRowClassNmae"
                    >
                        <template #export-button>
                            <a-button type="success"> 导出 </a-button>
                        </template>
                    </DynamicTable>
                </div>
            </a-col>
            <UploadFile :visible="visible" @closemodal="closemodal" @uploadfile="uploadfile"></UploadFile>
        </a-row>
    </div>
</template>

<script lang="ts" setup>
import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table';
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, ref } from '@vue/reactivity';
import { onMounted } from '@vue/runtime-core';
import { GetCommon } from '@/api/pages/common';
import { ReportCommonWithOut } from '@/store/module/ReportCommon';
import { useLoading } from '@/components/feat/Loading';
import { getRowClassNmae } from '@/pages/ReportPortal/utils/common';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { cloneDeep } from 'lodash';
import V3chart from '@/components/charts/V3chart.vue';
import { useRouter } from 'vue-router';
import UploadFile from './Upload.vue';
import dayjs from 'dayjs';
import axios from 'axios';
const router = useRouter();
import { message, Popconfirm, TypographyLink } from 'ant-design-vue';

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});
const { InfiniteScroll } = useRemoteSelect();
const UseParamsAdd = AddCommonPageParams();
const ReportCommonParams = ReportCommonWithOut();
let FactoryType: string = ReportCommonParams.Fab;
const OQCInspectionDetailList = ref<HTMLElement | null>(null);
let showTable = ref(true);
let showSelect = ref(true);
//文件上傳彈窗visible
let visible = ref(false);
function getImport(site: any) {
    const { schemas2, schemas, optionsdata } = require(`@/pages/ReportPortal/FS/RMA_MaterialPred/RMA/form-schema`);
    const { baseColumns1, baseColumns2 } = require(`@/pages/ReportPortal/FS/RMA_MaterialPred/RMA/Table`);
    return { schemas2, schemas, optionsdata, baseColumns1, baseColumns2 };
}
let { schemas2, schemas, optionsdata, baseColumns1, baseColumns2 } = getImport(FactoryType);

let usechart = 'chart';
let PageSetting = reactive<BasePageParams.BasePageSetting>({
    List: [],
    TopList: [] as any,
    EchartOptions: {},
    APIColumns: [],
    buttonText: '圖',
    buttonText2: '表格',
    buttonText3: '導出',
    button1: false,
    button2: true,
    button3: false,
    Columns: baseColumns1,
    moreData: []
});
const [DynamicTable] = useTable();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas:schemas(),
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
});
function changeView2() {
    //第一個圖表
    PageSetting.button1 = false;
    PageSetting.button2 = true;
    PageSetting.Columns = baseColumns1;
}
function changeView3() {
    //第二個圖表
    PageSetting.button1 = false;
    PageSetting.button3 = true;
    PageSetting.Columns = baseColumns2;
}
function changeView() {
    PageSetting.button1 = true;
    PageSetting.button2 = false;
}
const editableData = reactive({});
let selectData;
async function confirm(values) {
    console.log('输出当前斷點', dayjs().format('MM'));
    openFullLoading();
    let baseParams = {
        apiName: 'MaterialPredctSET',
        FactoryType: UseParamsAdd.Fab
    };
    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        }
    });
    try {
        // const {
        //     Date: {
        //         Data: { Table: Data }, //表格数据
        //         Data1: { Table: Data1 }, //图表数据
        //     }
        // } = await GetCommon({ ...values, ...baseParams });
        let { api01 } = await GetCommon({ ...values, ...baseParams });

        let middleProps: string[] = [];
        let middleColumns: Object[] = [];

        console.log('输出当前baseParams', values);
        PageSetting.Title = 'Export PC Product Plan Set Data ';
        // if (values['data_typ'] == '2') {
        PageSetting.Columns = baseColumns1;
        if (PageSetting.button3 == true) {
            PageSetting.Columns = baseColumns2;
        }  

        PageSetting.moreData = api01['Data']['Table']; //表格數據
        let Data1 = api01['Data1']['Table'];

        // if (PageSetting.moreData.length > 100) PageSetting.List = PageSetting.moreData.slice(0, 100);
        // else PageSetting.List = PageSetting.moreData;
        PageSetting.List = PageSetting.moreData;
        // let barData = Object.values(Data1);
        if (Data1.length != 0) {
            let BarData: Object[] = [];
            let barData = {
                BL: Data1.map((item) => {
                    return item.BL;
                }),
                BEZEL: Data1.map((item) => {
                    return item.BEZEL;
                }),
                CB : Data1.map((item) => {
                    return item.CB ;
                }),
                COF : Data1.map((item) => {
                    return item.COF ;
                }),
                POL : Data1.map((item) => {
                    return item.POL ;
                }),
                PCBA: Data1.map((item) => {
                    return item.PCBA;
                })

            };
            console.log('標記1');
            Data1.forEach((item) => {
                //列
                if (!middleProps.includes(item['MAT_TYPE'])) {
                    middleProps.push(item['MAT_TYPE']);
                }
            });

            console.log('標記2', middleProps);
           

            console.log('標記3', barData);
            // let myxAxis = Data1.map((item) => {
            //     //x軸
            //     return item.TRANS_TYPE;
            // });

            let myxAxis: string[] = []; //柱圖
            Data1.forEach((item) => {
                if (!myxAxis.includes(item['TRANS_TYPE'])) {
                    myxAxis.push(item['TRANS_TYPE']);
                }
            });
            
            let mylegend = reactive(['BL','BEZEL','CB','COF','POL','PCBA']); //柱圖
            let color=0;
            var colorList = ['#00A3E0','#FFA100', '#ffc0cb', '#ca8622', '#BBFFAA','#749f83', '#ca8622'];
                                
            // Data1.forEach((item) => {
            for (let i in barData) {
                BarData.push({
                    name: i,
                    type: 'bar',
                    barWidth: '20%',
                    data: barData[i],
                    stack: 'CyleTime(Hour)',
                    label: 
                    {
                        normal: 
                        {
                            show: true,
                            position: 'inside',
                            textStyle: 
                            {
                                color: '#000',
                                fontSize: 12
                            }
                         
                        }
                    },
                    emphasis: {
                        //折线图的高亮状态。
                        focus: 'series' //聚焦当前高亮的数据所在的系列的所有图形。
                    },
                    //背景色
                    // showBackground: true,
                    backgroundStyle: {
                        color: 'rgba(180, 180, 180, 0.2)'
                    },
                    itemStyle: {
                        normal: {
                            //这里是颜色
                            // color: '#00A3E0'
                            color:  colorList[color]
                        }
                    }

                });
                
            console.log('color', color);
                color=color+1;
            }
            console.log('barData', barData);
            console.log('BarData', BarData);

            PageSetting.EchartOptions = {
                title: [
                    {
                        text: '物料預估/缺口圖',
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
                    data: mylegend,
                    label: { show: true },
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
                        name: 'Parts(片數)',
                        splitLine: false,
                        axisLine: {
                            show: true
                        }
                    }
                ],
                series: [...BarData]
            };
            closeFullLoading();
        }
        closeFullLoading();
    } catch (error) {
        console.log(error);
        closeFullLoading();
    }
}

const edit = (key: string) => {
    editableData[key] = cloneDeep(PageSetting.List.filter((item) => key === item['key'])[0]);
};
const save = (key: string) => {
    Object.assign(PageSetting.List.filter((item) => key === item['key'])[0], editableData[key]);
    message.success('修改成功！');
    delete editableData[key];
};
const cancel = (key: string) => {
    delete editableData[key];
};
// 文件上傳
function upload(values) {
    // 存儲vux
    // ReportCommonParams.AddFormParams(`ProductPlanDataImport${FactoryType}`,values)
    // 頁面跳轉
    // router.push({name:`ProductPlanDataImport${FactoryType}`})
    visible.value = true;
    selectData = values;
}
const closemodal = () => {
    visible.value = false;
};
const uploadfile = async (fileList, formData) => {
    console.log('输出当前上傳斷點', dayjs().format('MM'));
    let baseParams = {
        apiName: 'ParaUPLOAD',
        FactoryType: UseParamsAdd.Fab,
        // G_FAC: ReportCommonParams.G_FAC_COM,
        //ReportType: 'OQCInspFrequencyImport',
        action: 'Import'
    };
    let temp = { ...baseParams, ...selectData };
    Object.keys(temp).forEach((key) => {
        formData.append(key, temp[key]);
    });
    console.log('输出当前baseParams', selectData);
    console.log('输出当前上傳斷點formData', formData);
    let data;
    try {
        data = await axios.post(process.env.VUE_APP_BASE_API + 'SQLAgent/ApiWork', formData, {
            headers: { 'content-type': 'application/x-www-form-urlencoded' }
        });
        //  console.log("输出当前baseParams",key);
    } catch (error) {
        return message.error('上傳失敗!請檢查文件是否爲空/文件是否為excell!');
    }

    console.log(data);

    const result = data.data.result;

    if (result != '匯入Success') {
        return message.warn(result);
    }
    const Data = data.data.Data0.Table;
    PageSetting.moreData = Data;
    if (PageSetting.moreData.length > 100) {
        //预先加载40条数据
        PageSetting.List = PageSetting.moreData.slice(0, 40);
    } else {
        PageSetting.List = PageSetting.moreData;
    }
    message.success('文件上傳成功!');
};

//实现无限滚动或者说懒加载
InfiniteScroll(OQCInspectionDetailList, PageSetting);
onMounted(async () => {
    await GetProDuctData();
});

async function GetProDuctData() {
    function getMonths(start: string, end: string) {
        let startyear = Number(start.split('-')[0]);
        let startMonth = Number(start.split('-')[1]);
        let endyear = Number(end.split('-')[0]);
        let endMonth = Number(end.split('-')[1]);
        let result: string[] = [];
        let year = startyear;
        let month = startMonth;
        while (year < endyear || (year === endyear && month <= endMonth)) {
            if (month >= 10) {
                result.push(`${year}${month}`);
            } else {
                result.push(`${year}0${month}`);
            }
            month++;
            if (month > 12) {
                year++;
                month = 1;
            }
        }
        return result;
    }
    let start = String(dayjs().year() - 1) + `-${dayjs().add(6, 'months').format('MM')}`;
    let end = String(dayjs().year()) + `-${dayjs().add(6, 'months').format('MM')}`;
    optionsdata.TimeData = getMonths(start, end).map((item) => {
        return { label: item, value: item };
    });

    async function getschmaData(FactoryType: string, apiName: string, paramsname: string) {
        let {
            api01: {
                Data: { Table: k }
            }
        } = await GetCommon({
            G_FAC: 'A',
            FactoryType: FactoryType,
            apiName: apiName
        });
        return k.map((item) => {
            return { label: item[paramsname], value: item[paramsname] };
        });
    }
}
</script>
<style lang="less" scoped>
.btn-rows {
    button {
        margin-right: 12px;
    }
}

.OperationPerformance :deep(.v-center) {
    background-color: #f0f0f0;
}

.OperationPerformance :deep(.Yellow) {
    background-color: #80ff80;
}

.OperationPerformance :deep(.YellowGreen) {
    background-color: #f39230;
}
</style>
