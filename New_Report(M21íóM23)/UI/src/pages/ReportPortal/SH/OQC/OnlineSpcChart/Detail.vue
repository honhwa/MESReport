<template>
    <div>
        <a-card>
            <V3chart  height="60vh" width = "100%" :options="PageSetting.EchartOptions" isClick
                          @callbackFun="callbackFun"></V3chart>
            <div :v-show="PageSetting.showValue">
                <V3chart  height="60vh" width = "100%" :options="PageSetting.EchartOptions1" isClick
                @callbackFun="callbackFun"></V3chart>
           </div>
            <DynamicTable sticky v-bind="wrapperProps" row-key="id" :header-title="PageSetting.Title"
                :columns="PageSetting.Columns" :key="PageSetting.Columns" 
                    bordered size="small"
                :data-all="PageSetting.moreData" :explain="false" :OwnerParams="false"
                :data-source="PageSetting.List" :pagination="false" export-file-name="导出" 
                :scroll="{ x: 3000, y: 500 }">
                <template #export-button>
                    <a-button type="success"> 导出 </a-button>
                </template>
            </DynamicTable>
           <div hidden>{{ PageSetting.SQLHidden }}</div>

        </a-card>
    </div>
</template>

<script lang="ts" setup>
import { useTable } from '@/components/core/dynamic-table';
import { reactive, watchEffect, watch, computed,onMounted, } from 'vue';
import { GetCommon } from '@/api/pages/common';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { useLoading } from '@/components/feat/Loading';
import { message } from 'ant-design-vue';
import { Console } from 'console';
import { mergeTableRow,getSumData,getRowClassName,Inserttotalrow} from '@/pages/ReportPortal/utils/SHcommon'
import { ReportCommon } from '@/store/module/ReportCommon';
// import { useVirtualList } from '@vueuse/core';
import V3chart from "./LCM/V3chart.vue";

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});
const UseParamsAdd = AddCommonPageParams();
function getImport(site: any) {
    const { OnlineSpcChartSecondColumns} = require(`@/pages/ReportPortal/SH/OQC/OnlineSpcChart/LCM/Table`);
    return { OnlineSpcChartSecondColumns};
}
let useCommonParams = AddCommonPageParams();
let useReportCommon = ReportCommon();

let ReportNameParams=`OnlineSpcChart-${useCommonParams.Fab}`;


let tableParams = useReportCommon[ReportNameParams].FirstTableParams;
let formParams= useReportCommon[ReportNameParams].FormParams;
let APIMessage=JSON.parse(useReportCommon[ReportNameParams].APIMessage);

let { OnlineSpcChartSecondColumns } = getImport("SH");

let PageSetting = reactive({
    moreData: [] as any,
    List: [] as any,
    Columns: [] as any,
    TopTitle: '',
    BottomTitle: '',
    BottomList: [] as any,
    BottomColumns: [] as any,
    SQLHidden:'',
    BottomSQLHidden:'',
    x: '' as any,
    EchartOptions1: {},
    EchartOptions: {},
    showValue: false
});
const [DynamicTable] = useTable();

function GetDetailData(tableParams){   
   
    PageSetting.List=[];
    PageSetting.Columns=[];
    PageSetting.BottomList=[];
    PageSetting.BottomColumns=[];
    //加載防護罩開始
    openFullLoading();

    GetDetail_OnlineSpcChart(tableParams);
};

async function GetDetail_OnlineSpcChart(tableParams) { 
    

    formParams= useReportCommon[ReportNameParams].FormParams;
    APIMessage=JSON.parse(useReportCommon[ReportNameParams].APIMessage);

     const ParamsData: any = {
        name: "Date",
        apiName: "OnlineSpcChartDetail",
        FactoryType: formParams["FactoryType"],
        G_FAC: formParams["G_FAC"],
        FromDate:APIMessage["FromDate"],
        ToDate:APIMessage["ToDate"],
        stropID:APIMessage["stropID"],
        colONCHID:tableParams['ONCHID'],
        colONCHTYPE:tableParams["ONCHTYPE"],
     };

     PageSetting.TopTitle = `${formParams["FactoryType"]} Down Grade Detail`;

    try{
        const { Date: { Data: { Table: Data }, SQL01 } } = await GetCommon(ParamsData);

        let xData: number[] = [];
    let lineData_UCL: number[] = [];
    let lineData_LSPEC: number[] = [];
    let lineData_USPEC: number[] = [];
    let lineData_CL: number[] = [];
    let lineData_dtavg: number[] = [];
    let lineData_LCL: number[] = [];
    let lineData2_UCL: number[] = [];
    let lineData2_dtrm: number[] = [];
    let lineData2_CL: number[] = [];
    let lineData2_ICL: number[] = [];
    let chartitle
    Data.forEach(item => {
      chartitle = item['ONCHID']
    })
    Data.forEach(item => {
      xData.push(item["CLDATE"])
      lineData_UCL.push(item["UCL1"])
      lineData_LSPEC.push(item["LSPEC"])
      lineData_USPEC.push(item["USPEC"])
      lineData_CL.push(item["CL1"])
      lineData_dtavg.push(item["DTAVG"])
      lineData_LCL.push(item["LCL1"])
      lineData2_UCL.push(item["UCL2"])
      lineData2_dtrm.push(item["DTRNG"])
      lineData2_CL.push(item["CL2"])
      lineData2_ICL.push(item["LCL2"])
    })
    let temp1: Object[] = [];
      Object.keys(Data[0]).forEach((item) => {
          temp1.push(
              item
          );
      });
    let temp: Object[] = [];
      let children1: Object[] = [];
      let temp13 = true;
     
  
    let legendData: string[] = [];
    let legendData2: string[] = [];
      legendData.push("dtavg")
      legendData.push("USPEC")
      legendData.push("LSPEC")
      legendData.push("UCL")
      legendData.push("CL")
      legendData.push("LCL")

      legendData2.push("dtrm")
      legendData2.push("UCL")
      legendData2.push("CL")
      legendData2.push("ICL")
    PageSetting.EchartOptions = {
        // backgroundColor: "#061740",
        title: [
            {
                text: chartitle+'(x)',
                left: "center"
            }

        ],
        grid: {
            x: 0,
            y: 0,
            top: "15%",
            left: "10%",
            right: "0%",
            bottom: "30%",
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
                minorSplitLine: true,
                axisTick: {
                    show: false,
                },
                axisLabel: {
                    color: "rgba(36, 173, 254, 1)",
                    fontSize: "12",
                    interval:0, //坐标刻度之间的显示间隔，默认就可以了（默认是不重叠）
               rotate:38   //调整数值改变倾斜的幅度（范围-90到90）
                },
                axisLine: {
                    show: false,
                },
                data: xData,
            },
        ],
        yAxis: [
            {
                type: 'value',
                align: 'right',
                // name: 'Movement',
                // splitLine: false,
                // axisLine: {
                //     show: true,
                // },
                /* min: 0,
                max: 25,
                interval: 1,
                axisLabel: {
                    formatter: '{value} °C'
                } */
            },
        ],
        series: [
            // 折线图
          {
            data: lineData_CL,
            type: 'line',
            name: 'CL'
          },

          {
            data: lineData_UCL,
            type: 'line',
            name: 'UCL'
          },
          {
            data: lineData_LCL,
            type: 'line',
            name: 'LCL'
          },
          {
            data: lineData_dtavg,
            type: 'line',
            name: 'dtavg'
          },
          {
            data: lineData_USPEC,
            type: 'line',
            name: 'USPEC'
          },
          {
            data: lineData_LSPEC,
            type: 'line',
            name: 'LSPEC'
          },
        ],
    }
    if (chartitle.includes("BA") || chartitle.includes("OC")|| chartitle.includes("PS")|| chartitle.includes("BS")|| chartitle.includes("AP")|| chartitle.includes("PP")|| chartitle.includes("BW")) {
        PageSetting.showValue = true   
        PageSetting.EchartOptions1 = {
            // backgroundColor: "#061740",
            title: [
                {
                    text: chartitle+'(RM)',
                    left: "center"
                }

            ],
            grid: {
                x: 0,
                y: 0,
                top: "15%",
                left: "0%",
                right: "0%",
                bottom: "30%",
                containLabel: true,
            },
            legend: {
                // 柱状图的leggeng
                data: legendData2,
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
                    minorSplitLine: true,
                    axisTick: {
                        show: false,
                    },
                    interval: 1,
                    axisLabel: {
                        color: "rgba(36, 173, 254, 1)",
                        fontSize: "12",
                    },
                    axisLine: {
                        show: false,
                    },
                    data: xData,
                },
            ],
            yAxis: [
                {
                    type: 'value',
                    align: 'right',
                    // name: 'Movement',
                    // splitLine: false,
                    // axisLine: {
                    //     show: true,
                    // },
                    /* min: 0,
                    max: 25,
                    interval: 1,
                    axisLabel: {
                        formatter: '{value} °C'
                    } */
                },
            ],
            series: [
                // 折线图
            {
                data: lineData2_UCL,
                type: 'line',
                name: 'UCL'
            },

            {
                data: lineData2_dtrm,
                type: 'line',
                name: 'dtrm'
            },
            {
                data: lineData2_CL,
                type: 'line',
                name: 'CL'
            },
            {
                data: lineData2_ICL,
                type: 'line',
                name: 'ICL'
            }
            ],
        }
    }
    
    function callbackFun(value: any) {
    console.log("点击了", value);

    }


        PageSetting.SQLHidden="OnlineSpcChartDetail-SQLSecondDetail:"+SQL01;

       
        PageSetting.Columns = [...OnlineSpcChartSecondColumns];

        /**进行表格合并 */
        PageSetting.moreData=Data;                
        PageSetting.List=PageSetting.moreData;
       
    }catch(error: any){ 
         //加載防護罩結束
        closeFullLoading();
        return Promise.reject(error); 
     }
    
}


watchEffect(() => {
    //一阶表格监控
    PageSetting['List'] = [];
    PageSetting['Columns'] = [];
    PageSetting['TopTitle'] = "";
    tableParams = useReportCommon[ReportNameParams].FirstTableParams;
    GetDetailData(tableParams);
});

</script>


