<template>
    <div class="DayliMove">
        <a-card>
            <!-- <a-row>
                <span class="menu-fold" @click="showHidden(PageSetting)">
                    <component :is="!PageSetting.showmenu ? MenuUnfoldOutlined : MenuFoldOutlined" />
                </span>
            </a-row> -->
            <a-row>
                <a-col
                    :span="24 - PageParams.TableSpan"
                    class="SHScrollForm"
                    Source-show="PageSetting.showmenu"
                >
                    <div class="SHScrollForm">
                        <SchemaForm @submit="confirm"> </SchemaForm>
                    </div>
                </a-col>
                <a-col :span="PageParams.TableSpan">                  
                    <div class="ScrollTable" ref="SpcCpkPerformanceReport">

                        <DynamicTable
                            showFavorite
                            EditExplain
                            row-key="id"
                            :header-title="PageSetting.HeadTitle"
                            :columns="PageSetting.Columns"
                            :key="PageSetting.Columns"
                            bordered
                            size="small" :PageParams="PageParams"
                            :data-source="PageSetting.List"
                            :data-all="PageSetting.moreData"
                            :pagination="false"
                            export-file-name="导出"
                            :row-class-name="getRowClassName"
                            :scroll="{ x: 3000,y: 500 }"                            
                        >
                            
                           <!-- <template #toolbar>
                                <a-button type="primary" @click="isShowExplain = !isShowExplain">{{isShowExplain?'显示表格':'显示说明文件'}}</a-button>
                            </template>   -->
                            <template #export-button>
                                <a-button type="success"> 导出 </a-button>
                            </template>
                        </DynamicTable>
                        
                        <V3chart  height="60vh" width = "100%" :options="PageSetting.EchartOptions" isClick
                          @callbackFun="callbackFun"></V3chart>
                    </div>
                </a-col>
                <div hidden>{{ PageSetting.SQLHidden }}</div>
            </a-row>
        </a-card>
    </div>
</template>


<script lang="tsx" setup>
import { MenuFoldOutlined, MenuUnfoldOutlined } from '@ant-design/icons-vue';
// import { schemas } from './form-schema';
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, unref, ref, onMounted, watch, computed, Ref } from 'vue';
import { GetCommon } from '@/api/pages/common';
import useRemoteSelect from '@/pages/ReportPortal/utils/SHuseRemoteSelect';
import { useLoading } from '@/components/feat/Loading';
import {cloneDeep} from 'lodash-es';
import { getPovitData, PovitParams } from '@/utils/helper/povitHelper'
import { showHidden, getRowClassName,getSumData,mergeTableRow,getdiffdate, } from '@/pages/ReportPortal/utils/SHcommon'
import { AddCommonPageParams } from '@/store/module/commonpage';
import { Console } from 'console';
import { message } from 'ant-design-vue';
import {PageParams} from '@/components/core/dynamic-table/src/dynamic-table'
import { ReportCommon } from '@/store/module/ReportCommon';
import V3chart from "./LCM/V3chart.vue";
import { useRouter, useRoute } from 'vue-router';
// import { useInfiniteScroll, useVirtualList } from '@vueuse/core'

const router = useRouter();

// route.fullPath
let useCommonParams = AddCommonPageParams();
let useReportCommon = ReportCommon();
const SpcCpkPerformanceReport = ref<HTMLElement | null>(null)
//vue3的mixin
const { InfiniteScroll, } = useRemoteSelect()
function getImport(site: any) {
    //let { optionsdata } = require('@/pages/ReportPortal/Static/SHCommon-Form-schema');
    const { schemas } = require(`@/pages/ReportPortal/SH/OQC/SpcCpkPerformanceReport/LCM/form-schema`);
    return { schemas};
}

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});


let { schemas } = getImport(useCommonParams.Fab);

let ReportItem = reactive({
    // 首頁後端返回的sql以及sql參數
        APIMessage:'',
        // 左側請求參數
        FormParams: {},
        // 二級頁面所需參數
        FirstTableParams: {},
        // 其餘頁面所需參數
        SecondTableParams:{},
        ThirdTableParams:{},
        ThirdTableClickParams:{},
        //廠別
        Fab:'',
        G_FAC_COM:'',
});
let ReportNameParams = `SpcCpkPerformanceReport-${useCommonParams.Fab}`;
useReportCommon.AddObject(ReportNameParams,ReportItem);
let Message:any;

let PageSetting = reactive({
    List:[] as any,
    TableSpan: 20,
    showmenu: true,
    APIColumns: [] as any,
    Columns: [] as any,
    moreData: [] as any,
    SQLHidden:'',
    HeadTitle:'',
    EchartOptions: {},

});
// let isLongData = ref(true)
PageSetting.HeadTitle=`${useCommonParams.Fab} CpCpk Month Report`
const [DynamicTable] = useTable();

const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas:schemas(useCommonParams.Fab,reactive({})),
    actionColOptions: {
        span: 18
    },
    layout: 'vertical'
});

const customCellRender = (record: Object, rowIndex: number, column: Object) => {
    return {
        ondblclick: () => { 
             ReportItem.FirstTableParams={ ...record, ...column['__originColumn__'] };           
             //useReportCommon.AddFirstTableParams(`MfgOperator-${useCommonParams.Fab}`,{ ...record, ...column['__originColumn__'] })
           
            router.push({ name: `SpcCpkPerformanceDetail-${useCommonParams.Fab}`});
        },
        style: {
            cursor: 'pointer'
        }
    };
};

/**
 * 提交表单,验证通过后进行数据获取与处理
 * @dynamicFormRef 表单的所有参数与属性
 * @ParamsData API接收的参数
 */
function confirm(values) {

      //table 初始化
    PageSetting.List=[];
    PageSetting.Columns=[];
    //加載防護罩開始
    openFullLoading();
    
    //基礎query參數條件值
    let baseParams = {name: "Date",apiName: "CpkPerformanceDataSet",FactoryType:useCommonParams.Fab,G_FAC:useCommonParams.G_FAC_COM,building:"",}

   //獲取query頁面的條件值
    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        }
    });
    
    //console.log(dynamicFormRef.formMode
   
   //獲取數據綁定table
    dynamicFormRef.validate().then(async () => {

       try{
        const { Date: { Data: { Table: Data },
                Data01,
                sql,
                FromDate,
                ToDate,
                title, 
                sql_detail
                } }  = await GetCommon({ ...baseParams, ...values });

          PageSetting.SQLHidden = sql
          console.log("SpcCpkPerformanceReport-SQLHidden:",sql)
          //console.log("SpcCpkPerformanceReport-Data:",Data)
          //console.log("Data01:",Data01)
          if (Data.length != 0) {
            Message = {                
                sql:sql,
                FromDate: FromDate,
                ToDate: ToDate,
                sql_detail: sql_detail
            }; 
            let xData: number[] = [];
            let lineData_IQTY_0: number[] = [];
            let lineData_IQTY0_1: number[] = [];
            let lineData_IQTY1_1_33: number[] = [];
            let lineData_IQTY1_33_1_67: number[] = [];
            let lineData_IQTY1_67_2: number[] = [];
            let lineData_IQTY2_5: number[] = [];
            let ineData_IQTY5: number[] = [];
            let chartitle
            chartitle = title;
            Data.forEach(item => {
            xData.push(item["PROD"])
            lineData_IQTY_0.push(item["IQTY_0"])
            lineData_IQTY0_1.push(item["IQTY0_1"])
            lineData_IQTY1_1_33.push(item["IQTY1_1_33"])
            lineData_IQTY1_33_1_67.push(item["IQTY1_33_1_67"])
            lineData_IQTY1_67_2.push(item["IQTY1_67_2"])
            lineData_IQTY2_5.push(item["IQTY2_5"])
            ineData_IQTY5.push(item["IQTY5"])
            })
        
            let legendData: string[] = [];
            legendData.push("<0(%)")
            legendData.push("0~1.0(%)")
            legendData.push("1.0~1.33(%)")
            legendData.push("1.33~1.67(%)")
            legendData.push("1.67~2.0(%)")
            legendData.push("2.0~5.0(%)")
            legendData.push(">5.0(%)")

            PageSetting.EchartOptions = {
                // backgroundColor: "#061740",
                title: [
                    {
                        text: chartitle,
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
                    data: lineData_IQTY_0,
                    type: 'bar',
                    name: '<0(%)',
                    stack: 'CPK',
                    itemStyle: {
                        normal:{color:"RGB(0,0,255)"}
                    }
                },

                {
                    data: lineData_IQTY0_1,
                    type: 'bar',
                    name: '0~1.0(%)',
                    stack: 'CPK',
                    itemStyle: {
                        normal:{color:"RGB(153,204,0)"}
                    }
                },
                {
                    data: lineData_IQTY1_1_33,
                    type: 'bar',
                    name: '1.0~1.33(%)',
                    stack: 'CPK',
                    itemStyle: {
                        normal:{color:"RGB(0,255,255)"}
                    }
                },
                {
                    data: lineData_IQTY1_33_1_67,
                    type: 'bar',
                    name: '1.33~1.67(%)',
                    stack: 'CPK',
                    itemStyle: {
                        normal:{color:"RGB(255,255,0)"}
                    }
                },
                {
                    data: lineData_IQTY1_67_2,
                    type: 'bar',
                    name: '1.67~2.0(%)',
                    stack: 'CPK',
                    itemStyle: {
                        normal:{color:"RGB(255,153,204)"}
                    }
                },
                {
                    data: lineData_IQTY2_5,
                    type: 'bar',
                    name: '2.0~5.0(%)',
                    stack: 'CPK',
                    itemStyle: {
                        normal:{color:"RGB(225,102,0)"}
                    }
                },
                {
                    data: ineData_IQTY5,
                    type: 'bar',
                    name: '>5.0(%)',
                    stack: 'CPK',
                    itemStyle: {
                        normal:{color:"RGB(255,0,0)"}
                    }
                }
                ],
            }
            function callbackFun(value: any) {
                console.log("点击了", value);

                }
            //循環查詢data，獲取table Colums 數據
                let ColumsData: any = [];
                let WidthPars=50;
                let titlePars="";//label
                let classNamePars="v-center";//API提供轉置后的時間處理（例如：A2022_11_30）
                let fixedPara="left";
            for (let item in Data01[0]) { 
                if(item != "PROD"){
                    WidthPars=30;
                    titlePars=item;
                    classNamePars="";
                    ColumsData.push({ title: titlePars, align: 'center', dataIndex: item, className: classNamePars, width:WidthPars, fixed:fixedPara,
                        customCell: customCellRender, 
                        });
                    }  
                }

            PageSetting.Columns = [
                    {
                        title: 'PROD',
                        align: 'center',
                        dataIndex: 'PROD',
                        className:'sum-center',
                        width:60
                    },
                    ...ColumsData
                ];
            console.log("ColumsData", ColumsData);


            PageSetting.moreData = Data01;

            console.log("PageSetting.moreData", PageSetting.moreData);
            if (PageSetting.moreData.length > 100) 
               PageSetting.List = PageSetting.moreData.slice(0, 40);
            else 
               PageSetting.List = PageSetting.moreData;

            ReportItem.APIMessage = JSON.stringify(Message);  
            ReportItem.FormParams={ ...baseParams,...values };


          }else{

              message.warning("無數據！")

          }

            closeFullLoading();

       }catch(error: any){ 
           closeFullLoading();
           return Promise.reject(error); 
        }

    
    });
}


//实现无限滚动或者说懒加载
InfiniteScroll(SpcCpkPerformanceReport, PageSetting)

//載入api 方法
onMounted(async () => {
    //optionsdata.OwnerName = await Getcommon(params) 
    // await mountedEqptId(optionsdata, useCommonParams.Fab);
    // await mountedSpcProd(optionsdata, useCommonParams.Fab);  
    // await mountedDataGroup(optionsdata, useCommonParams.Fab);
    
})

</script>

<style lang="less" scoped>
.btn-rows {
    button {
        margin-right: 12px;
    }
}

.DayliMove :deep(.Source-center) {
    background-color: #f0f0f0;
}
:deep(.pro_auto-center){
    border: 1px solid black;
}

.DayliMove :deep(.Yellow) {
    background-color: #80ff80;
}
:deep(.sum-center){
    border: 1px solid black;
}

.SpcCpkPerformanceReport {
    height: 700px;
    overflow-y: auto;
}

.DayliMove :deep(.YellowGreen) {
    background-color: #f39230;
}
:deep(.v-center){
    border: 1px solid black;
}


.swapper {
    display: flex;
    flex-direction: column;
}
</style>
