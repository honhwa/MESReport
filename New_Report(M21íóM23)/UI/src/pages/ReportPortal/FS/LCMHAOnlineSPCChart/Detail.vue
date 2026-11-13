<script lang="ts" setup>
import { useTable } from '@/components/core/dynamic-table';
import {PageParams} from '@/components/core/dynamic-table/src/dynamic-table'
import { reactive, ref, watchEffect } from 'vue';
import { GetCommon } from '@/api/pages/common';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { ReportCommonWithOut } from '@/store/module/ReportCommon';
import { getRowClassNmae, getfinalsum, Inserttotalrow, dclick } from '@/pages/ReportPortal/utils/common';
import { cloneDeep } from 'lodash';
import { message } from 'ant-design-vue';
import V3chart from "./LCM1/V3chart.vue";
const UseParamsAdd = ReportCommonWithOut();
let FactoryType: any = UseParamsAdd.SubFab;
let importurl = ref("BLU");
const [DynamicTable] = useTable();
switch (FactoryType) {
    case "LCM1":
        importurl.value = FactoryType;
        break;
    case "LCM2":
        importurl.value = FactoryType; 
        break;
    case "BLU1":
    case "BLU2":
        importurl.value = "BLU";
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
    } = require(`@/pages/ReportPortal/FS/LCMHAOnlineSPCChart/${site}/table`);
    return { detail1_1, detail1_2, detail1_3, datakey, ChildProps };
}
let { detail1_1, detail1_2, detail1_3, datakey, ChildProps } = getImport(importurl.value);

let useCommonParams = AddCommonPageParams();
let TableParams = useCommonParams.FirstTableParams;
let PageSetting = reactive({
    moreData: [] as any,
    FirstList: [] as any,
    SecondList: [] as any,
    ThirdList: [] as any,
    FirstColumns: [] as any,
    SecondColumns: [] as any,
    ThirdColumns: [] as any,
    EchartOptions1: {},
    EchartOptions: {},
    FirstTitle: 'LCMA Quality Report ',
    SecondTitle: 'LCMA Defect Reason Code Qty ',
    ThirdTitle: 'LCMA Quality Report '
});
async function thirdtablehandle(record, rowindex, column, ritem, index) {
    PageSetting.ThirdList = [];
    let thirdparams = cloneDeep(TableParams);
    thirdparams["entity_no"] = "'" + record["ENTITY_NO"] + "'";
    thirdparams["ReportType"] = "DRAW_EDC_CHART";
    let { Date: { Data0: { Table: Data } } } = await GetCommon(thirdparams);
    if (Data.length == 0) {
        return message.warn("無數據！")
    }
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
}
async function GetDetailData(TableParams: any) {

    PageSetting.FirstColumns = [];
    PageSetting.SecondColumns = [];
    let { Data: { Data0: { Table: Data }, title, title2 } } = await GetCommon(TableParams);
    let xData: number[] = [];
    let lineData_UCL: number[] = [];
    let lineData_LSPEC: number[] = [];
    let lineData_USPEC: number[] = [];
    let lineData_CL: number[] = [];
    let lineData_dtavg: number[] = [];
    let lineData_LCL: number[] = [];
    let lineData2_UCL: number[] = [];
    let lineData2_dtrm: number[] = [];
    let chartitle
    Data.forEach(item => {
      chartitle = item['CHARTID']
    })
    Data.forEach(item => {
      xData.push(item["CLDATE"])
      lineData_UCL.push(item["UCL1"])
      lineData_LSPEC.push(item["LSPEC"])
      lineData_USPEC.push(item["USPEC"])
      lineData_CL.push(item["CL1"])
      lineData_dtavg.push((item["DTMAX"]+item["DTMIN"])/2)
      lineData_LCL.push(item["LCL1"])
      lineData2_UCL.push(item["UCL2"])
      lineData2_dtrm.push(item["RM"])
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
      title.forEach((item, index) => {
        if(item == title2[index]){
          temp.push({
                  title: item,
                  dataIndex: temp1[index],
                  key: item,
                  resizable: true,
                  width: 100
          });
          
        }
        else{
            if(title[index-1] == item){
              children1.push({
                title: title2[index-1],
                dataIndex: temp1[index-1],
                key: title2[index-1],
                resizable: true,
                width: 100
              });
            }
            else if(title[index-1] != "Prod ID"){
              children1.push({
                title: title2[index-1],
                dataIndex: temp1[index-1],
                key: title2[index-1],
                resizable: true,
                width: 100
              });
              temp.push({
              title: title[index-1],
              children: children1
              });
              children1 = [];
              temp13 = true;
            }
            if(index == title.length-1){
            children1.push({
                title: title2[index],
                dataIndex: temp1[index],
                key: title2[index],
                resizable: true,
                width: 100
              });
              temp.push({
              title: item,
              children: children1
              });
              children1 = [];
              temp13 = true;
          }
          }
      });
      PageSetting.ThirdList = Data;
      PageSetting.ThirdColumns = temp;
    let legendData: string[] = [];
    let legendData2: string[] = [];
      legendData.push("UCL")
      legendData.push("LSPEC")
      legendData.push("USPEC")
      legendData.push("dtavg")
      legendData.push("LCL")
      legendData.push("CL")
      legendData2.push("UCL")
      legendData2.push("dtrm")
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
            left: "0%",
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
          }
        ],
    }
}
function callbackFun(value: any) {
    console.log("点击了", value);

}
watchEffect(() => {
    //一阶表格监控
    TableParams = useCommonParams.FirstTableParams;
    GetDetailData(TableParams);
});
</script>

<!-- <V3chart width="50%" height="50%" :options="PageSetting.EchartOptions" isClick
                        @callbackFun="callbackFun"></V3chart> -->

<template>
  <div>
    <a-card>
      <V3chart  height="60vh" width = "100%" :options="PageSetting.EchartOptions" isClick
                        @callbackFun="callbackFun"></V3chart>
      <V3chart  height="60vh" width = "100%" :options="PageSetting.EchartOptions1" isClick
      @callbackFun="callbackFun"></V3chart>
        <DynamicTable row-key="id" :header-title="PageSetting.ThirdTitle" :columns="PageSetting.ThirdColumns"
        sticky
            :key="PageSetting.ThirdColumns" bordered size="small" :data-source="PageSetting.ThirdList"
            v-if ="!['BLU1','BLU2'].includes(FactoryType)"
            :pagination="false" :export-file-name="PageSetting.ThirdTitle" :scroll="{ x: 3000, y: 500 }"
            :row-class-name="getRowClassNmae">
            <template #export-button>
                <a-button type="success"> 导出 </a-button>
            </template>
        </DynamicTable>
    </a-card>
  </div>
</template>