<template>
    <div class="QualityReportForDownGrade">
        <a-card>
            <a-row>
                <a-col :span="24 - PageParams.TableSpan" Source-show="PageSetting.showmenu">
                    <div class="SHScrollForm">
                        <SchemaForm @submit="confirm"> </SchemaForm>
                    </div>
                </a-col>
                <a-col :span="PageParams.TableSpan" class="ScrollTable">                    
                        <DynamicTable sticky showFavorite :PageParams="PageParams" id="QualityReportForDownGrade"
                        :header-title="PageSetting.Title" :columns="PageSetting.Columns"
                            :key="PageSetting.Columns" bordered size="small"
                            :scroll="{ x: PageSetting.x, y: PageSetting.y, }" :data-source="PageSetting.List"
                             :pagination="false" export-file-name="导出"
                            :row-class-name="getRowClassName_Sel">
                            <template #export-button>
                                <a-button type="success"> 导出 </a-button>
                            </template>
                        </DynamicTable>                    
                    <V3chart width="100%" height="400px" :options="PageSetting.EchartOptions"
                        v-show="PageSetting.isShowTable"></V3chart>
                    <div v-show="PageSetting.isShowTable">
                        <DynamicTable row-key="id" :header-title="PageSetting.SecondTitle"
                           :flexwidth='-5'
                            :columns="PageSetting.SecondColumns" sticky :explain="false" :key="PageSetting.SecondColumns"
                            bordered size="small" :data-source="PageSetting.SecondList" :pagination="false"
                            :export-file-name="PageSetting.SecondTitle" :scroll="{ y: 400 }">
                            <template #export-button>
                                <a-button type="success"> 导出 </a-button>
                            </template>
                        </DynamicTable>
                    </div>
                    <div hidden>{{ PageSetting.SQLHidden }}</div>
                </a-col>
            </a-row>
        </a-card>
    </div>
</template>


<script lang="tsx" setup>
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, onMounted, ref } from 'vue';
import { cloneDeep } from 'lodash-es';
import useRemoteSelect from '@/pages/ReportPortal/utils/SHuseRemoteSelect';
import { getrowsum, Inserttotalrow, sortArray } from '@/pages/ReportPortal/utils/SHcommon';
import { message } from 'ant-design-vue';
import type { TableColumn } from '@/components/core/dynamic-table';
import { GetCommon } from '@/api/pages/common';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { useRouter, useRoute } from 'vue-router';
import V3chart from "@/components/charts/V3chart.vue";
import { Console } from 'console';
import { useLoading } from '@/components/feat/Loading';
import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table'
import { ReportCommon } from '@/store/module/ReportCommon';
import { PovitParams, getPovitData } from "@/utils/reportcommon/provit";


const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});
const useCommonParams = AddCommonPageParams();
let useReportCommon = ReportCommon();
let bigfab = useCommonParams.Fab.slice(0, useCommonParams.Fab.length - 1);
const QualityTable = ref<HTMLElement | null>(null);


const router = useRouter();
let ReportItem = reactive({
    // 首頁後端返回的sql以及sql參數
    APIMessage: '',
    // 左側請求參數
    FormParams: {},
    // 二級頁面所需參數
    FirstTableParams: {},
    // 其餘頁面所需參數
    SecondTableParams: reactive({}),
    ThirdTableParams: {},
    ThirdTableClickParams: {},
    //廠別
    Fab: '',
    G_FAC_COM: '',
});
let ReportNameParams = `QualityReport-${useCommonParams.Fab}`;
useReportCommon.AddObject(ReportNameParams, ReportItem);



// 頁面數據
let PageSetting = reactive({
    List: [] as any,
    Columns: [] as any,
    Message: {},
    x: '' as any,
    y: '' as any,
    Title: "",
    moreData: [] as any,
    SQLHidden: '',
    APIColumns: [] as any,
    EchartOptions: {},
    SecondList: [] as any,
    SecondColumns: [] as any,
    isShowTable: true,
    SecondTitle: "",
});

PageSetting.y = 250
let Message: any;
let TopTableParams: any;



function getImport(site: any) {
    //let { optionsdata } = require('@/pages/ReportPortal/Static/SHCommon-Form-schema');
    const { schemas, GrpOptions } = require(`@/pages/ReportPortal/SH/QualityReportForDownGrade/${bigfab}/form-schema`);
    return { schemas,  GrpOptions };
}
let { schemas,  GrpOptions } = getImport(useCommonParams.Fab);
const [DynamicTable] = useTable();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 130,
    schemas:schemas(useCommonParams.Fab,reactive({})),
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
});
const {  InfiniteScroll,  } = useRemoteSelect()



async function confirm(values) {
    openFullLoading();
    PageSetting.List = [];
    PageSetting.Columns = [];
    PageSetting.SecondList = [];
    PageSetting.SecondColumns = [];

    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        }
        if (["WorkOrderLike", "Operator"].includes(key)) {
            values[key] = values[key].split(',')
        }
    });


    let ApiNamePara = '';
    if (values.ShowMode == "small_prod") {
        ApiNamePara = "DGQualityDataSet_prodnbr"
    } else if (values.ShowMode == "operators") {
        ApiNamePara = "DGQualityDataSet_operators"
    } else {
        ApiNamePara = "DGQualityDataSet"
    }

    TopTableParams = { name: "Date", apiName: ApiNamePara, FactoryType: useCommonParams.Fab, G_FAC: useCommonParams.G_FAC_COM, };

    TopTableParams = { ...TopTableParams, ...values }

    try {

        //顯示方式：選擇站點
        if (values.ShowMode == "work_ctr") {
            PageSetting.isShowTable = true
            const { Date: {
                Data: { Table: Data },//total
                Data_2: { Table: Data_2 },//dfct table1
                Data_3: { Table: Data_3 },//chart
                Data_4: { Table: Data_4 },//dfct table2
                TOTAL_SQL,
                SQL01,
                SQL02,
                SQL03,
                FromDate,
                ToDate,
                ShowMode,
                sql_LcmGrade,
                StrSql_w1,

            } } = await GetCommon(TopTableParams);

            PageSetting.SQLHidden = ApiNamePara + "-SQLHidden SQL01:" + SQL01 + ";<END> SQL02:" + SQL02 + ";<END> SQL03:" + SQL03 + ";<END> TOTAL_SQL:" + TOTAL_SQL

            if (Data.length == 0 && Data_2.length == 0) {
                closeFullLoading();
                return message.warn("无数据！")
            }
            console.log("SQL01",SQL01)
             console.log("SQL02",SQL02)
            //console.log("SQL03",SQL03)
            // console.log("TOTAL_SQL",TOTAL_SQL)

            Message = {
                FromDate: FromDate,
                ToDate: ToDate,
                ShowMode: ShowMode,
                sql_LcmGrade:sql_LcmGrade,
                StrSql_w1:StrSql_w1,
            };

            let Totaldata: any[] = []
            let SecondTotaldata: any[] = []
            Data.forEach(item => {
                item['WORK_NAME'] = item["ACCT_DATE"]
                item['WORK_CTR'] = item["ACCT_DATE"]
                item['DOWN_GRADE'] = item["DG_QTY"]
                item['GOOD_GRADE'] = item["TOTAL"] - item["DG_QTY"]
                if (item["ACCT_DATE"] == "TOTAL") {
                    item['WORK_CTR'] = "1300"
                    item['WORK_NAME'] = "C INSPECTION"
                    Totaldata.push(item)
                } else {
                    SecondTotaldata.push(item)
                }
            })

            let Defctdata: any[] = []
            let SecondDefctdata: any[] = []
            Data_2.forEach(item => {
                item['WORK_CTR'] = "1300"
                Defctdata.push(item)
            })

            Data_4.forEach(item => {
                item['WORK_CTR'] = item['TIMES']
                SecondDefctdata.push(item)
            })



            let resultDate = DataHandle(Totaldata, Defctdata)
            let SecondresultDate = DataHandle(SecondTotaldata, SecondDefctdata,false)

            //console.log("resultDate[1]", resultDate,)

            PageSetting.Columns = resultDate["baseColumns"];
            PageSetting.List = resultDate["finalData"];

            PageSetting.SecondColumns = SecondresultDate["baseColumns"];
            PageSetting.SecondList = SecondresultDate["finalData"];

            PageSetting.y = null;
            PageSetting.y = 250
            PageSetting.Title = `${useCommonParams.Fab} DownGrade Report`;
            PageSetting.SecondTitle = `${useCommonParams.Fab} DownGrade Report`;

            /////*******************chart*******************/////
            // x轴数据
            let xData: number[] = [];
            sortArray(Data_3, "TIMES", 'asc').forEach(item => {
                if (!xData.includes(item["TIMES"])) {
                    xData.push(item["TIMES"])
                }
            })


            // legent DFCT_CODE 数据
            let legendData: string[] = [];
            Data_2.forEach(item => {
                legendData.push(item["DFCT_CODE"]);
            });

            // 折线图数据
            let lineData: Object[] = [];

            for (let d = 0; d< legendData.length; d++) {                
                let RateData: Object[] = []; 
                 SecondresultDate["finalData"].forEach(item => {
                    if (item["DFCT_CODE"]==legendData[d]){
                        for (let h = 0; h< xData.length; h++) {
                            if(item[`${xData[h]}RATE`]){
                                RateData.push(item[`${xData[h]}RATE`].replace("%",""))
                            }else{
                                RateData.push("null")
                            }

                        }

                   }
                })

                lineData.push({
                        name: legendData[d],
                        type: "line",
                        data: RateData,
                        tooltip: {
                            valueFormatter: function (value) {
                                return value + ' %';
                            }
                        },
                        // emphasis: {//折线图的高亮状态。
                        //     focus: "series",//聚焦当前高亮的数据所在的系列的所有图形。
                        // },
                        // //背景色
                        // // showBackground: true,
                        // backgroundStyle: {
                        //     color: "rgba(180, 180, 180, 0.2)",
                        // }
                    })

            }
            
                        
            //GRADE legend
            let GradelegendData: string[] = [];
            // 堆叠柱状图数据
            let BarData: any[] = [];
            //down grade數據處理 
             let ListDowngrade=['A','B','C','D','E','W','O']
             for (let i = 0; i < ListDowngrade.length; i++){
                GradelegendData.push(ListDowngrade[i])
                //累計某個等級數量               
                let Downgrade: Object[] = [];
                 for (let j = 0; j< xData.length; j++) {
                    let countqty: number = 0;
                    Data_3.forEach(item => {  
                        if(['A','B'].includes(ListDowngrade[i])){
                            if (item["DOWNGRADE"].slice(0,1)==ListDowngrade[i] && item["TIMES"] == xData[j]) {
                            countqty = countqty + item[ListDowngrade[i]]
                          }
                        }else if(ListDowngrade[i]=='C'){
                            if ((item["DOWNGRADE"].slice(0,1)==ListDowngrade[i]||(item["DOWNGRADE"].includes("C")&&!['A','B'].includes(item["DOWNGRADE"].slice(0,1)))) && item["TIMES"] == xData[j]) {
                            countqty = countqty + item[ListDowngrade[i]]
                            }
                        }
                        else{
                            if (item["DOWNGRADE"].slice(0,1)==ListDowngrade[i]&&!item["DOWNGRADE"].includes("C")&& item["TIMES"] == xData[j]) {
                            countqty = countqty + item[ListDowngrade[i]]
                          } 
                        } 
                    })
                    Downgrade.push(countqty)
                  }
                  
                   //添加條形數據集
                   BarData.push(
                    {
                        name: ListDowngrade[i],
                        type: "bar",
                        barWidth: "30",
                        data: Downgrade,
                        yAxisIndex: 1,
                        stack: 'Ad',
                        tooltip: {                           
                            valueFormatter: function (value) {
                                return value + ' pcs';
                            }
                        },
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

             console.log("BarData",BarData)
             
            //所有legend 數據
            let TotallegendData=[...GradelegendData,...legendData]
           
          //https://echarts.apache.org/examples/zh/editor.html?c=mix-line-bar
           
            PageSetting.EchartOptions = {
                title: [
                   {
                        text: "DownGrade Chart",
                        left: "center"
                    }

                ],
                tooltip: {
                    //trigger: 'axis',
                    axisPointer: {
                        type: 'cross',
                        crossStyle: {
                            color: '#999'
                        }
                    }
                },
                grid: {
                    x: 0,
                    y: 0,
                    x2: 0,
                    y2: 0,
                     top: "10%",
                     left: "5%",
                     right: "12%",
                     bottom: "10%",
                    containLabel: true,
                },
                toolbox: {
                    feature: {
                        dataView: { show: true, readOnly: false },
                        magicType: { show: true, type: ['line', 'bar'] },
                        restore: { show: true },
                        saveAsImage: { show: true }
                    }
                },
                legend: {
                    data: TotallegendData,
                    type: 'scroll',
                    orient: 'vertical',
                    right: "2%",
                    top: "10%",
                    
                },
                xAxis: [
                    {
                        type: 'category',
                        data: xData,
                        axisPointer: {
                            type: 'shadow'
                        }
                    }
                ],
                yAxis: [
                    {
                        type: 'value',
                        name: 'Rate',
                        min: 0,
                        //max: 250,
                        //interval: 50,
                        axisLabel: {
                            formatter: '{value} %'
                        }
                    },
                    {
                        type: 'value',
                        name: 'DownGrade',
                        min: 0,
                        //max: 25,
                        // interval: 5,
                        axisLabel: {
                            formatter: '{value} pcs'
                        }
                    }
                ],
                series: [
                    // 堆叠柱状图
                    ...BarData,
                    // 折线图
                    ...lineData,
                ]


            }
           
        } else {
            //顯示方式：小機種或者人員別
            PageSetting.isShowTable = false;
            const { Date: {
                Data: { Table: Data },//total
                Data_2: { Table: Data_2 },//dfct table1
                SQL01,
                SQL02,
                FromDate,
                ToDate,
                ShowMode,
                sql_LcmGrade,
                StrSql_w1,

            } } = await GetCommon(TopTableParams);

            PageSetting.SQLHidden = ApiNamePara + "-SQLHidden SQL01:" + SQL01 + ";<END> SQL02:" + SQL02

            if (Data.length == 0 && Data_2.length == 0) {
                closeFullLoading();
                return message.warn("无数据！")
            }
            // console.log("SQL01", SQL01)
            // console.log("SQL02", SQL02)


            Message = {
                FromDate: FromDate,
                ToDate: ToDate,
                ShowMode: ShowMode,
                sql_LcmGrade:sql_LcmGrade,
                StrSql_w1:StrSql_w1,
            };

            let Totaldata: any[] = []
            Data.forEach(item => {
                if (values.ShowMode == "small_prod") {
                    item['WORK_NAME'] = item["PROD_NBR"]
                    item['WORK_CTR'] = item["PROD_NBR"]
                } else {
                    item['WORK_NAME'] = item["OPERATORS"]
                    item['WORK_CTR'] = item["OPERATORS"]
                }

                Totaldata.push(item)
            })

            let Defctdata: any[] = []
            let SecondDefctdata: any[] = []
            Data_2.forEach(item => {
                if (values.ShowMode == "small_prod") {
                    item['WORK_CTR'] = item["PROD_NBR"]
                } else {
                    item['WORK_CTR'] = item["OPERATORS"]
                }
                Defctdata.push(item)
            })


            let resultDate = DataHandle(Totaldata, Defctdata)

           // console.log("resultDate[1]", resultDate,)

            PageSetting.Columns = resultDate["baseColumns"];
            PageSetting.List = resultDate["finalData"];

            PageSetting.x = null;
            if (PageSetting.Columns.length > 7) {
                PageSetting.x = 90 * PageSetting.Columns.length;
            }

            PageSetting.y = null;
            PageSetting.Title = `${useCommonParams.Fab} DownGrade Report`;

        }

        closeFullLoading();
    } catch (error: any) {
        //message.error(error);
        closeFullLoading();
    }

}



function DataHandle(Data, Data_2,isNegative: boolean = true) {
    let baseColumns: any[] = [
        {
            title: 'process name',
            align: 'left',
            dataIndex: `DFCT_CODE`,
            key: `DFCT_CODE`,
            className: "v-center",
            fixed: 'left',
        },
        {
            title: 'process name',
            align: 'left',
            dataIndex: `ERRC_DESCR`,
            key: `ERRC_DESCR`,
            className: "v-center",
            fixed: 'left',
        },
    ];

    // PROD_NBR记录
    let prodobj = {
        "DFCT_CODE": "PROD_NBR",
        "ERRC_DESCR": "PROD_NBR",
    };

    // inspection qty记录
    let inspectionobj = {
        "DFCT_CODE": "INSPECTION QTY",
        "ERRC_DESCR": "INSPECTION QTY",
    };
    let good = {
        "DFCT_CODE": "GOOD",
        "ERRC_DESCR": "GOOD",
    };
    let ng = {
        "DFCT_CODE": "DownGrade",
        "ERRC_DESCR": "DownGrade",
    };
    let yieldrate = {
        "DFCT_CODE": "Down Grade Rate",
        "ERRC_DESCR": "Down Grade Rate",
    };
    let yieldloss = {
        "DFCT_CODE": "YIELD LOSS",
        "ERRC_DESCR": "YIELD LOSS",
    };

    let defectitem = {
        "DFCT_CODE": "defect ITEM",
        "ERRC_DESCR": "defect ITEM",
    };

    let middleProps: any[] = [];
    let middlerateProps: any[] = [];
    let finalData: object[] = [];


    Data.forEach(item => {
        middleProps.push(item["WORK_CTR"]);
        middlerateProps.push(`${item["WORK_CTR"]}RATE`);

        baseColumns.push(
            {
                title: item["WORK_NAME"],
                align: 'center',
                dataIndex: `${item["WORK_CTR"]}`,
                key: `${item["WORK_CTR"]}QTY`,
            },
            {
                title: item["WORK_NAME"],
                align: 'center',
                dataIndex: `${item["WORK_CTR"]}RATE`,
                key: `${item["WORK_CTR"]}RATE`,
            }
        );

        // prodobj[`${item["WORK_CTR"]}`] = item["PROD_NBR"];
        // prodobj[`${item["WORK_CTR"]}RATE`] = item["PROD_NBR"];

        // if(values.ShowMode=="equip_nbr"||values.ShowMode=="1300to1600"){
        //     prodobj[`${item["WORK_CTR"]}`] = item["PROD_NBR"];
        //     prodobj[`${item["WORK_CTR"]}RATE`] = item["PROD_NBR"];
        // }

        inspectionobj[`${item["WORK_CTR"]}`] = item["TOTAL"];
        inspectionobj[`${item["WORK_CTR"]}RATE`] = item["TOTAL"];
        ng[`${item["WORK_CTR"]}`] = item["DOWN_GRADE"];
        ng[`${item["WORK_CTR"]}RATE`] = item["DOWN_GRADE"];

        good[`${item["WORK_CTR"]}`] = item["GOOD_GRADE"];
        good[`${item["WORK_CTR"]}RATE`] = item["GOOD_GRADE"];
        yieldrate[`${item["WORK_CTR"]}`] = (((item["GOOD_GRADE"] / item["TOTAL"]) * 100).toFixed(3)) + "%";
        yieldrate[`${item["WORK_CTR"]}RATE`] = (((item["GOOD_GRADE"] / item["TOTAL"]) * 100).toFixed(3)) + "%";

        yieldloss[`${item["WORK_CTR"]}`] = ((item["DOWN_GRADE"] / item["TOTAL"]) * 100).toFixed(3) + "%";
        yieldloss[`${item["WORK_CTR"]}RATE`] = ((item["DOWN_GRADE"] / item["TOTAL"]) * 100).toFixed(3) + "%";

        defectitem[`${item["WORK_CTR"]}`] = "QTY";
        defectitem[`${item["WORK_CTR"]}RATE`] = "%";

    });

    finalData.push(inspectionobj, good, ng, yieldrate, yieldloss, defectitem);

    // // Data_2添加属性
    // Data_2.forEach(item => {
    //     item[`${item["WORK_CTR"]}-SECOND_QTY`] = item["SECOND_QTY"]
    // })

    // Data_2處理,行列轉換
    // 表格数据
    let config: PovitParams = {
        Data: Data_2,
        povitColumn: {
            "WORK_CTR": 'QTY'
        },
        changeColumn: middleProps,
        repeatColumn: ["DFCT_CODE", "ERRC_DESCR"]
    }
    let povitData = getPovitData(config);


    let tempmiddleProps = [...middleProps, ...middlerateProps];
    for (let i = 0; i < tempmiddleProps.length; i++) {
        tempmiddleProps[i] = String(tempmiddleProps[i])
    }

    // 计算defect rate 第7行開始
    povitData.forEach(item => {
        let totalqty = 0;
        middleProps.forEach((titem, index) => {
            if (item[titem] != null) {
                item[middlerateProps[index]] = Number(((item[titem] / finalData[2][titem] * finalData[4][titem].replace("%", ""))).toFixed(3)) + "%";
            }
        })

    })

    finalData.push(...povitData);


    let ClickColumn = [...tempmiddleProps];
    // 合并表头        
    baseColumns.forEach((item, index) => {
        if ((index % 2) === 0) {
            item["colSpan"] = 2
        } else {
            item["colSpan"] = 0
        }

        item["customCell"] = (record: Object, rowIndex: number, column: Object) => {
            let obj = {};

            // 双击功能功能
            let stylefunc = () => {
                let tempStyle = {};
                if (ClickColumn.includes(item["dataIndex"])) {
                    tempStyle = { cursor: 'pointer' }
                } else {
                    tempStyle = {};
                }
                return { ...tempStyle };
            }
            obj["onDblclick"] = async () => {
                if (ClickColumn.includes(item["dataIndex"])&& isNegative==true) {
                    ReportItem.APIMessage = JSON.stringify(Message);
                    ReportItem.FirstTableParams = { ...record, ...column['__originColumn__'] };
                    ReportItem.FormParams = TopTableParams;
                    router.push({ name: `QualityReportForDownGradeDetail-${useCommonParams.Fab}` });
                    //handclick(record, rowIndex, column, item, index);
                } else {
                    message.warning('此栏位没有二阶数据');
                }
            },
                obj["style"] = stylefunc()

            if (rowIndex < 5) {
                if (column["key"].includes("QTY") && !column["key"].includes("TOTAL")) {
                    return {
                        colSpan: 2,// 跨几列合并，这里现在是跨两列
                        ...obj
                    }
                } else if (column["key"].includes("RATE") && !column["key"].includes("TOTAL")) {
                    return {
                        colSpan: 0,
                        ...obj
                    }
                }
                else if (column["key"].includes("DFCT_CODE")) {
                    return {
                        colSpan: 2,
                        ...obj
                    }
                } else if (column["key"].includes("ERRC_DESCR")) {
                    return {
                        colSpan: 0,
                        ...obj
                    }
                } else {
                    if (rowIndex == 2) {
                        if (column["key"].includes("TOTAL")) {
                            return {
                                ...obj
                            }

                        }
                    }
                }
            } else if (rowIndex == 5) {
                if (column["key"].includes("DFCT_CODE")) {
                    return {
                        colSpan: 2,
                        ...obj
                    }
                } else if (column["key"].includes("ERRC_DESCR")) {
                    return {
                        colSpan: 0,
                        ...obj
                    }
                }
            } else if (rowIndex > 5) {
                return {
                    ...obj
                }
            }

        }
    })
   // console.log("finalData ---111 ", finalData)
    return { finalData, baseColumns }
}


function getRowClassName_Sel(record: Object) {
    if (record && Object.keys(record).length != 0) {
        for (const [key, value] of Object.entries(record)) {
            if (typeof (value) == "string" && value.includes("defect ITEM")) {
                return 'v-center';
            } else if (typeof (value) == "string" && value.includes("INSPECTION QTY")) {
                return 'Purple';
            } else if (typeof (value) == "string" && ["GOOD", "Down Grade Rate"].includes(value)) {
                return 'GrayBlue';
            } else if (typeof (value) == "string" && ["DownGrade", "YIELD LOSS"].includes(value)) {
                return 'YellowGray';
            }

        }
    }
}


//实现无限滚动或者说懒加载
//InfiniteScroll(QualityTable, PageSetting);
onMounted(async () => {
    // if (bigfab === "LCM") {
    //     await mounted(optionsdata, useCommonParams.Fab)
    //     await mountedSourceFab(optionsdata, useCommonParams.Fab);
    //     await mountedOper(optionsdata, useCommonParams.Fab);
    //     await mountedWoTypeBySap(optionsdata, useCommonParams.Fab);
    //     await mountedLCMowner(optionsdata, useCommonParams.Fab);
    //     await mountedLCDowner(optionsdata, useCommonParams.Fab);
    //     await mountedLCMBigEquipment(optionsdata, useCommonParams.Fab);
    //     await mountedLCMDefectCode(optionsdata, useCommonParams.Fab);
    //     await mountedLCDGrade(optionsdata, useCommonParams.Fab);
    //     await mountedModuleData(optionsdata, useCommonParams.Fab);
    // } else {

    // }
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
</style>
