<template>
    <div >
        <a-card>
            <a-row>
                <a-col :span="24 - PageParams.TableSpan" Source-show="PageSetting.showmenu">
                    <div class="SHScrollForm">
                     <SchemaForm @submit="confirm"> </SchemaForm>
                </div>
                </a-col>
                <a-col :span="PageParams.TableSpan">
                    <div  class="ScrollTable" ref="TotalYieldForINT">
                        <DynamicTable sticky
                            showFavorite
                            :PageParams="PageParams"
                            id="ExpQualityQuery"
                            export-type="Table"
                            :header-title="PageSetting.Title"
                            :columns="PageSetting.Columns"
                            :key="PageSetting.Columns"
                            bordered
                            size="small"
                             :scroll="{ x: PageSetting.x,}"
                            :data-source="PageSetting.List"
                            :data-all="PageSetting.moreData"
                            :pagination="false"
                            export-file-name="导出"
                            :row-class-name="getRowClassName_Sel"
                            
                        >
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
import {getrowsum, Inserttotalrow} from '@/pages/ReportPortal/utils/SHcommon';
import { message } from 'ant-design-vue';
import type { TableColumn } from '@/components/core/dynamic-table';
import { GetCommon } from '@/api/pages/common';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { useRouter, useRoute } from 'vue-router';
import { Console } from 'console';
import { useLoading } from '@/components/feat/Loading';
import {PageParams} from '@/components/core/dynamic-table/src/dynamic-table'
import { ReportCommon } from '@/store/module/ReportCommon';
import { PovitParams ,getPovitData} from "@/utils/reportcommon/provit";


const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});
const useCommonParams = AddCommonPageParams();
let useReportCommon = ReportCommon();
let bigfab=useCommonParams.Fab.slice(0,useCommonParams.Fab.length-1);


const router = useRouter();
let ReportItem = reactive({
    // 首頁後端返回的sql以及sql參數
        APIMessage:'',
        // 左側請求參數
        FormParams: {},
        // 二級頁面所需參數
        FirstTableParams: {},
        // 其餘頁面所需參數
        SecondTableParams:reactive({}),
        ThirdTableParams:{},
        ThirdTableClickParams:{},
        //廠別
        Fab:'',
        G_FAC_COM:'',
});
let ReportNameParams = `TotalYieldForINT-${useCommonParams.Fab}`;
useReportCommon.AddObject(ReportNameParams,ReportItem);



// 頁面數據
let PageSetting = reactive({
    List: [] as any,
    Columns: [] as any,
    Message: {},
    Title: "",
    moreData: [] as any,
    x: '' as any,
    SQLHidden:'',
    APIColumns: [] as any,
});


function getImport(site: any) {
    const { schemas, GrpOptions } = require(`@/pages/ReportPortal/SH/TotalYieldForINT/${bigfab}/form-schema`);
    return { schemas,  GrpOptions };
}
let { schemas, GrpOptions } = getImport(useCommonParams.Fab);
const [DynamicTable] = useTable();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 130,
    schemas:schemas(useCommonParams.Fab,reactive({})),
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
});


async function confirm(values) {
    openFullLoading();
    PageSetting.List = [];
    PageSetting.Columns = [];

    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        }
        if (["WorkOrderLike", "Operator"].includes(key)) {
            values[key] = values[key].split(',')
        }
    });

    
    
    let TopTableParams =  { name: "Date",apiName: "ExpQualityDataSet", FactoryType: useCommonParams.Fab, G_FAC:useCommonParams.G_FAC_COM, };

    TopTableParams = { ...TopTableParams, ...values }
   
    try {
            const { Date: {
                Data: { Table: Data },//前6行的组成,表头的组成
                Data_2: { Table: Data_2 },//DFCT
                SQL01,
                SQL03,
                sql_dfct_where,
                sql_dggrade_where,
               

            } } = await GetCommon(TopTableParams);

            PageSetting.SQLHidden = "-SQLHidden SQL01:"+SQL01+";<END> SQL02:"+SQL03

            if (Data.length == 0 && Data_2.length == 0) {
                closeFullLoading();
                return message.warn("无数据！")
            }

             let Message = {                
                sql_dfct_where: sql_dfct_where,
                sql_dggrade_where: sql_dggrade_where,
            };

            let baseColumns: any[] = [
                {
                    title: 'process name',
                    align: 'left',
                    dataIndex: `DFCT_CODE`,
                    key: `DFCT_CODE`,
                    className: "v-center", 
                    fixed:'left',
                },
                {
                    title: 'process name',
                    align: 'left',
                    dataIndex: `ERRC_DESCR`,
                    key: `ERRC_DESCR`,
                    className: "v-center",
                    fixed:'left',
                },
            ];
          let finalData: object[] = [];

        //    // PROD_NBR记录
        //     let prodobj = {
        //         "DFCT_CODE": "PROD_NBR",
        //         "ERRC_DESCR": "PROD_NBR",
        //         "TOTAL YIELD QTY": "+PD",
        //         "TOTAL YIELD RATE": "-PD",
        //     };

           // inspection qty记录
            let inspectionobj = {
                "DFCT_CODE": "INSPECTION QTY",
                "ERRC_DESCR": "INSPECTION QTY",
                "TOTAL YIELD QTY": "+PD",
                "TOTAL YIELD RATE": "-PD",
            };
            let good = {
                "DFCT_CODE": "GOOD",
                "ERRC_DESCR": "GOOD",
                "TOTAL YIELD QTY": 0,
                "TOTAL YIELD RATE": 0,
            };
            let ng = {
                "DFCT_CODE": "NG",
                "ERRC_DESCR": "NG",
                "TOTAL YIELD QTY": 0,
                "TOTAL YIELD RATE": 0,
            };
            let yieldrate = {
                "DFCT_CODE": "YIELD Rate",
                "ERRC_DESCR": "YIELD Rate",
                "TOTAL YIELD QTY": 1,
                "TOTAL YIELD RATE": 1,
            };
            let yieldloss = {
                "DFCT_CODE": "YIELD LOSS",
                "ERRC_DESCR": "YIELD LOSS",
                "TOTAL YIELD QTY": 1,
                "TOTAL YIELD RATE": 1,
            };
            let defectitem = {
                "DFCT_CODE": "defect ITEM",
                "ERRC_DESCR": "defect ITEM",
                "TOTAL YIELD QTY": "QTY",
                "TOTAL YIELD RATE": "%",
            };
            
            //let totalyieldsum = 0;
            let middleProps: any[] = [];
            let middlerateProps: any[] = [];
            let middlesecondqtyprops: any[] = [];
            Data.forEach(item => {
                middleProps.push(item["WORK_CTR"]);
                middlerateProps.push(`${item["WORK_CTR"]}RATE`);
                middlesecondqtyprops.push(`${item["WORK_CTR"]}-SECOND_QTY`);
                
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
                //totalyieldsum += item["QTY"];

                // if(values.ShowMode=="equip_nbr"||values.ShowMode=="1300to1600"){
                //     prodobj[`${item["WORK_CTR"]}`] = item["PROD_NBR"];
                //     prodobj[`${item["WORK_CTR"]}RATE`] = item["PROD_NBR"];
                // }

                inspectionobj[`${item["WORK_CTR"]}`] = item["QTY"];
                inspectionobj[`${item["WORK_CTR"]}RATE`] = item["QTY"];
                ng[`${item["WORK_CTR"]}`] = item["RWK_QTY"];
                ng[`${item["WORK_CTR"]}RATE`] = item["REJECT_QTY"];

                good[`${item["WORK_CTR"]}`] = item["QTY"] - item["RWK_QTY"];
                good[`${item["WORK_CTR"]}RATE`] = item["QTY"]- item["REJECT_QTY"];
                yieldrate[`${item["WORK_CTR"]}`] = ((((item["QTY"] - item["RWK_QTY"]) / item["QTY"]) * 100).toFixed(3));
                yieldrate[`${item["WORK_CTR"]}RATE`] = ((((item["QTY"] - item["RWK_QTY"]) / item["QTY"]) * 100).toFixed(3));  //((((item["QTY"] - item["REJECT_QTY"]) / item["QTY"]) * 100).toFixed(3));
                yieldloss[`${item["WORK_CTR"]}`] = ((item["RWK_QTY"] / item["QTY"]) * 100).toFixed(3);
                yieldloss[`${item["WORK_CTR"]}RATE`] = ((item["REJECT_QTY"] / item["QTY"]) * 100).toFixed(3);
                defectitem[`${item["WORK_CTR"]}`] = "QTY";
                defectitem[`${item["WORK_CTR"]}RATE`] = "%";
                good["TOTAL YIELD QTY"] += good[`${item["WORK_CTR"]}`];
                good["TOTAL YIELD RATE"] += good[`${item["WORK_CTR"]}`]; //good[`${item["WORK_CTR"]}RATE`];
                ng["TOTAL YIELD QTY"] += ng[`${item["WORK_CTR"]}`];
                ng["TOTAL YIELD RATE"] +=ng[`${item["WORK_CTR"]}`]; //ng[`${item["WORK_CTR"]}RATE`];
        });

        middleProps.forEach(item => {
            yieldrate["TOTAL YIELD QTY"] *= Number(yieldrate[item]) / 100;
        })

        middlerateProps.forEach(item => {
            yieldrate["TOTAL YIELD RATE"] *= Number(yieldrate[item]) / 100;
        })

        yieldrate["TOTAL YIELD QTY"] = Number(yieldrate["TOTAL YIELD QTY"].toFixed(5)) * 100;
        yieldrate["TOTAL YIELD QTY"] = Number(yieldrate["TOTAL YIELD QTY"].toFixed(3));

        yieldrate["TOTAL YIELD RATE"] = Number(yieldrate["TOTAL YIELD RATE"].toFixed(5)) * 100;
        yieldrate["TOTAL YIELD RATE"] = Number(yieldrate["TOTAL YIELD RATE"].toFixed(3));

        yieldloss["TOTAL YIELD QTY"] = Number((100 - yieldrate["TOTAL YIELD QTY"]).toFixed(3));
        yieldloss["TOTAL YIELD RATE"] = Number((100 - yieldrate["TOTAL YIELD RATE"]).toFixed(3));
        // 统一添加百分号
        [...middleProps, ...middlerateProps, "TOTAL YIELD QTY", "TOTAL YIELD RATE"].forEach(item => {
            yieldrate[item] = yieldrate[item] + "%";
            yieldloss[item] = yieldloss[item] + "%";
        })

        finalData.push(inspectionobj, good, ng, yieldrate, yieldloss, defectitem);  

        // if(values.ShowMode=="equip_nbr"||values.ShowMode=="1300to1600"){
        //     finalData.push(prodobj,inspectionobj, good, ng, yieldrate, yieldloss, defectitem); 
        // }else{
        //     finalData.push(inspectionobj, good, ng, yieldrate, yieldloss, defectitem);  
        // }       
        // Data_2添加属性
        Data_2.forEach(item => {
            item[`${item["WORK_CTR"]}-SECOND_QTY`] = item["SECOND_QTY"]
        })

        // Data_2處理,行列轉換
        // 表格数据
        let config: PovitParams = {
            Data: Data_2,
            povitColumn: {
                "WORK_CTR": 'MAIN_QTY'
            },
            changeColumn: middleProps,
            repeatColumn: ["DFCT_CODE", "ERRC_DESCR"]
        }
        let povitData = getPovitData(config);
       
        // // 計算屬於什麼品質
        povitData.forEach(item => {
            item["condition"] = item["DFCT_CODE"].substr(2, 1)
        })       
        let tempmiddleProps = [...middleProps, ...middlerateProps];
        for (let i = 0; i < tempmiddleProps.length; i++) {
            tempmiddleProps[i] = String(tempmiddleProps[i])
        }
       
        //列计算
        getrowsum(povitData, middleProps, "TOTAL YIELD QTY");
        let { insertrowList, insertIndexList } = Inserttotalrow(povitData, ["condition"], [...tempmiddleProps, ...middlesecondqtyprops, "TOTAL YIELD QTY"], "DFCT_CODE", "總計",  false, false);
       
        // 数组头部添加新的值
        insertIndexList.unshift(0);
         // 数组头部移除最后一位数字
         insertIndexList.pop();
        // 插入之后的位置
        let afterinsert: number[] = [];
        for (let i = 0; i < insertIndexList.length; i++) {
            povitData.splice(insertIndexList[i] + i, 0, insertrowList[i]);
            afterinsert.push(insertIndexList[i] + i)
        }        

        afterinsert.forEach(item => {
            povitData[item]["DFCT_CODE"] = `${povitData[item + 1]["condition"]}品`;
            povitData[item]["ERRC_DESCR"] = `${povitData[item + 1]["condition"]}品`;
        })

        povitData.forEach(item => {
            // 當前行倒數第二列的括號計算方式
            let totalqty = 0;
            middleProps.forEach((titem, index) => {
                if (item[titem] != null) {
                    // 计算从第6行开始的百分号那一列的计算
                //    if(values.ShowMode=="equip_nbr"||values.ShowMode=="1300to1600"){ 
                //       item[middlerateProps[index]] = Number(((item[titem] / finalData[1][titem]) * 100).toFixed(3));
                //     }else{
                //        item[middlerateProps[index]] = Number(((item[titem] / finalData[0][titem]) * 100).toFixed(3)); 
                //     }
                    item[middlerateProps[index]] = Number(((item[titem] / finalData[0][titem]) * 100).toFixed(3)); 

                    // 添加括号（）
                    if (item[`${titem}-SECOND_QTY`] != undefined) {
                        item[titem] = `${item[titem]}(${item[`${titem}-SECOND_QTY`]})`;
                        totalqty += Number(`${item[`${titem}-SECOND_QTY`]}`);
                    } else {
                        item[titem] =  `${item[titem]}(0)`;
                    }

                }
            })

             // 最後一列的計算公式
            item["TOTAL YIELD RATE"] = (item["TOTAL YIELD QTY"] / ng["TOTAL YIELD QTY"]) * (Number(String(yieldloss["TOTAL YIELD QTY"]).replace('%', '')));
            item["TOTAL YIELD RATE"] = item["TOTAL YIELD RATE"].toFixed(3);
            item["TOTAL YIELD QTY"] = `${item["TOTAL YIELD QTY"]}(${totalqty})`
         })
        
         finalData.push(...povitData);
         //console.log("finalData",finalData)
       
        baseColumns.push(
            {
                title: "TOTAL YIELD",
                align: 'center',
                dataIndex: `TOTAL YIELD QTY`,
                key: `TOTAL YIELD QTY`,
            },
            {
                title: "TOTAL YIELD",
                align: 'center',
                dataIndex: `TOTAL YIELD RATE`,
                key: `TOTAL YIELD RATE`,
            }
        )

        
        let ClickColumn = [...tempmiddleProps, "TOTAL YIELD QTY", "TOTAL YIELD RATE"];
        // 合并表头        
        baseColumns.forEach((item, index) => {
            if ((index % 2) === 0) {
                item["colSpan"] = 2
            } else {
                item["colSpan"] = 0
            }
            // item["title"] = ClickColumn.includes(item["dataIndex"]) ? `${item["title"]}*` : item["title"];//可点击的栏位显示*号
            item["customCell"] = (record: Object, rowIndex: number, column: Object) => {                
                let obj = {};
                // console.log("record",record)
                // console.log("column",column)

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
                    if (ClickColumn.includes(item["dataIndex"])) {  
                          ReportItem.APIMessage = JSON.stringify(Message);  
                          ReportItem.FirstTableParams={ ...record, ...column['__originColumn__'] };
                          ReportItem.FormParams=TopTableParams;
                          router.push({ name: `ExpQualityQueryDetail-${useCommonParams.Fab}` });                                                 
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
                    }else{
                           if(rowIndex ==2){
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
       
        PageSetting.Columns = baseColumns;
        PageSetting.moreData = finalData;
        //PageSetting.List =finalData
        
        
        console.log("最終的數據", finalData);
        if (PageSetting.moreData.length > 100) {
            //预先加载40条数据
            PageSetting.List = PageSetting.moreData.slice(0, 30);
        }
        else {
            PageSetting.List = PageSetting.moreData;
        }

        PageSetting.x = null;
        if (PageSetting.Columns.length > 7) {
            PageSetting.x = 90 * PageSetting.Columns.length;
        }
        closeFullLoading();
    } catch (error) {
        closeFullLoading();
    }

}



function getRowClassName_Sel(record: Object) {
    if (record && Object.keys(record).length != 0) {
        for (const [key, value] of Object.entries(record)) {
            if (typeof (value) == "string" && value.includes("品")) {
                return 'Green';
            }else if(typeof (value) == "string" && value.includes("defect ITEM")) {
                return 'v-center';
            }else if(typeof (value) == "string" && value.includes("INSPECTION QTY")) {
                return 'Purple';
            }else if(typeof (value) == "string" && ["GOOD","YIELD Rate"].includes(value)) {
                return 'GrayBlue';
            }else if(typeof (value) == "string" && ["NG","YIELD LOSS"].includes(value)) {
                return 'YellowGray';
            }
           
        }
    }
}



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
