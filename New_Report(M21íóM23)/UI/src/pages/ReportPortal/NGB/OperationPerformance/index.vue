<template>
    <div class="OperationPerformance">
        <a-card>
            <!-- <a-row>
                <span class="menu-fold" @click="showHidden">
                    <component :is="!PageSetting.showmenu ? MenuUnfoldOutlined : MenuFoldOutlined" />
                </span>
            </a-row> -->
            <a-row>
                <a-col :span="24 - PageParams.TableSpan" class="NGBScrollForm" v-show="PageSetting.showmenu">
                    <SchemaForm @submit="confirm"> </SchemaForm>
                </a-col>
                <!-- <a-col :span="PageSetting.TableSpan" class="ScrollTable" v-show="RightPageShow"> -->
                 <a-col :span="PageParams.TableSpan" class="ScrollTable">
                    <DynamicTable sticky
                        showFavorite
                        row-key="id"
                        :header-title="PageSetting.HeadTitle"
                        :columns="PageSetting.TopColumns"
                        :key="PageSetting.TopColumns"
                        bordered
                        size="small"
                        :data-source="PageSetting.TopList"
                        :data-all="PageSetting.moreData"
                        :pagination="false"
                        :PageParams="PageParams"
                        :scroll="{ y: 350 }"
                        export-file-name="导出"
                        :row-class-name="getRowClassName"
                    >
                        <!-- <template #toolbar>
                                <a-button type="primary" @click="isShowExplain = !isShowExplain">{{isShowExplain?'显示表格':'显示说明文件'}}</a-button>
                            </template>                          -->

                        <template #export-button>
                            <a-button type="success"> 导出 </a-button>
                        </template>
                    </DynamicTable>
                    <DynamicTable sticky
                        row-key="id"
                        header-title="Operation Performance Rework"
                        :columns="PageSetting.BottomColumns"
                        :key="PageSetting.BottomColumns"
                        bordered
                        size="small"
                        :data-source="PageSetting.BottomList"
                        :data-all="PageSetting.moreData"
                        :pagination="false"
                        :explain="false"
                        :OwnerFoldParams="false"
                        :scroll="{ y: 350 }"
                        export-file-name="导出"
                        :row-class-name="getRowClassName"   >
                        <template #export-button>
                            <a-button type="success"> 导出 </a-button>
                        </template>
                    </DynamicTable>
                </a-col>
                <div hidden>{{ PageSetting.SQLHidden }}</div>
            </a-row>
        </a-card>
    </div>
</template>

<script lang="ts" setup>
import { MenuFoldOutlined, MenuUnfoldOutlined } from '@ant-design/icons-vue';
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive,ref } from '@vue/reactivity';
import { inject, onMounted } from '@vue/runtime-core';
import { GetCommon } from '@/api/pages/common';
import { getRowClassName } from '@/pages/ReportPortal/utils/NGBcommon';
import useRemoteSelect from '@/pages/ReportPortal/utils/NGBuseRemoteSelect';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { useLoading } from '@/components/feat/Loading';
import { message } from 'ant-design-vue';
import {PageParams} from '@/components/core/dynamic-table/src/dynamic-table'
import { Console } from 'console';
import { ReportCommon } from '@/store/module/ReportCommon';

// defineOptions({
//   name: 'DemosFormDynamicForm',
// });

const useCommonParams = AddCommonPageParams();
let useReportCommon = ReportCommon();
let bigfab=useCommonParams.Fab.slice(0,useCommonParams.Fab.length-1)

function getImport(site: any) {
    const {schemas} = require(`@/pages/ReportPortal/NGB/OperationPerformance/${bigfab}/form-schema`);
    //let { optionsdata } = require('@/pages/ReportPortal/Static/NGBCommon-Form-schema');
    const {
        topBaseColumns,
        bottomBaseColumns,
        topTableProps,
        bottomTableProps
    } = require(`@/pages/ReportPortal/NGB/OperationPerformance/${bigfab}/Table`);
    return { schemas, topBaseColumns, bottomBaseColumns,  topTableProps, bottomTableProps };
}
let { schemas, topBaseColumns, bottomBaseColumns,   topTableProps, bottomTableProps } =
    getImport(useCommonParams.Fab);

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});



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
let ReportNameParams = `OperationPerformance-${useCommonParams.Fab}`;
useReportCommon.AddObject(ReportNameParams,ReportItem);


let PageSetting = reactive({
    TopList: [] as any,
    BottomList: [] as any,
    TableSpan: 20,
    showmenu: true,
    TopColumns: [] as any,
    BottomColumns: [] as any,
    HeadTitle:'',
    SQLHidden:'',

});

const [DynamicTable] = useTable();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 130,
    schemas:schemas(useCommonParams.Fab,reactive({})),
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
    // fieldMapToTime: [['fieldTime', ['vDate_s', 'vDate_e1'], 'YYYY-MM-DD']]
});
// 点击提交
// PageSetting.columns = [...baseColumns];

function confirm(values) {

     //table 初始化
    PageSetting.TopList=[];
    PageSetting.BottomList=[];
    PageSetting.TopColumns=[];
    PageSetting.BottomColumns=[];

    //apiName傳參數 for 各個類型廠
    let ApiNamePara='';
    if (bigfab==="LCM"){
        ApiNamePara="OperationPerformance";
    }else if(bigfab==="BL"){
       ApiNamePara="OperationPerformance"; 
    }


    //加載防護罩開始
    openFullLoading();   
    // RightPageShow.value=true;
    let baseParams = {name: "Date",apiName: ApiNamePara,FactoryType:useCommonParams.Fab,G_FAC:useCommonParams.G_FAC_COM,}
    
    Object.keys(values).forEach((key) => {      
        if (!values[key]) {
            values[key] = '';
        }
      
    });

   for (let i in dynamicFormRef?.formModel) {
        if (i == 'Wo_nbr') values[i] = dynamicFormRef.formModel[i].split('\n');
        else {
            values[i] = dynamicFormRef.formModel[i];
        }
    }
    
    if (values.Prod_nbr && values.Prod_nbr.length > 1000 )
    {
         closeFullLoading();
         return message.error('料號输入超過 “最大限制筆數:1000” '); 
    }

    dynamicFormRef.validate().then(async () => {

     try{
            const {
                Date: {
                    Data: { Table: Data},
                    sql_Shift,
                    sql_IncludeBL,
                    sql_Floor,
                    sql_WorkOrder,
                    sql_Product,
                    sql_LcmOwner,
                    sql_LcdOwner,
                    pass_cuttime,
                    wip_cuttime,
                    SQL01,
                }
            } = await GetCommon({...baseParams,...values});
            /* if (!Success) {
                return message.error(Message);
            } */
            let Message = {
                sql_Shift: sql_Shift,
                sql_IncludeBL: sql_IncludeBL,
                sql_Floor: sql_Floor,
                sql_WorkOrder: sql_WorkOrder,
                sql_Product: sql_Product,
                sql_LcmOwner: sql_LcmOwner,
                sql_LcdOwner: sql_LcdOwner,
                pass_cuttime:pass_cuttime,
                wip_cuttime:wip_cuttime,
                // StrSql_w3: StrSql_w3,
                // StrSql_w4: StrSql_w4,
                // StrSql_we: StrSql_we,
                // StrSql_ww: StrSql_ww,
                // sql_m: sql_m,
                // sql_m1: sql_m1
            };

            PageSetting.SQLHidden = ApiNamePara+"-SQLHidden:"+SQL01
           // console.log("OperationPerformance-SQLHidden:",SQL01)

            ReportItem.APIMessage = JSON.stringify(Message);
            ReportItem.FormParams={ ...baseParams,...values }
        
            PageSetting.HeadTitle=`${useCommonParams.Fab} Operation Performance( pass:${pass_cuttime.slice(0,16)}, wip:${wip_cuttime.slice(0,16)} )`

          if (Data.length != 0) { 
                let TopList: any = [];
                let BottomList: any = [];
                Data.forEach((item) => {
                     if (item.WORK_CTR.substring(0,2) == "MR" ) {
                        // item['TD'] = handprops(item, 'WIP_QTY', 'PASS_QTY');
                        // item['TR'] = handprops(item, 'PASS_QTY', 'IN_QTY');
                        item['PASS2_QTYRate'] = handfloatprops(item, 'PASS2_QTY', 'PASS_QTY');
                        BottomList.push(item);
                    }  else {
                        
                        item['RWKPass_Rate']=handfloatprops(item, 'TOTALRWK_QTY', 'PASS_QTY');
                        item['FirstYield'] = handfloatprops(item, 'PASS1_QTY', 'PASS_QTY');
                        item['TotalYield'] =
                                    (item.PASS_QTY - item.TOTALRWK_QTY) / item.PASS_QTY == Infinity
                                        ? 0
                                        : (((item.PASS_QTY - item.TOTALRWK_QTY) / item.PASS_QTY) * 100).toFixed(4);
                                    item['RepairRate'] = (item.PASS_QTY - item['PASS1_QTY']-item['TOTALRWK_QTY']) / item.PASS_QTY == Infinity
                                            ? 0
                                            : (((item.PASS_QTY - item['PASS1_QTY']-item['TOTALRWK_QTY']) / item.PASS_QTY) * 100).toFixed(4);
                            
                        TopList.push(item);
                    }
                });
                // // 合计行
                // handleData(TopList, topTableProps);
                // handleData(BottomList, bottomTableProps);
                //// 最后一行的计算
                // TopList.forEach((item) => {
                //     if (Number(item.WORK_CTR) == 1100) {
                //         TopList[TopList.length - 1].IN_QTY = item.IN_QTY;
                //     }
                //     if (Number(item.WORK_CTR) == 1910) {
                //         TopList[TopList.length - 1].PASS_QTY = item.PASS_QTY;
                //     }
                // });
                // TopList[TopList.length - 1].TD = handprops(TopList[TopList.length - 1], 'WIP_QTY', 'PASS_QTY');
                // TopList[TopList.length - 1].TR = handprops(TopList[TopList.length - 1], 'PASS_QTY', 'IN_QTY');
                // TopList[TopList.length - 1]['WORK_CTR'] = 'SUM';
                // BottomList.forEach((item) => {
                //     if (Number(item.WORK_CTR) == 2401) {
                //         BottomList[BottomList.length - 1].IN_QTY = item.IN_QTY;
                //     }
                //     if (Number(item.WORK_CTR) == 2490) {
                //         BottomList[BottomList.length - 1].PASS_QTY =
                //             parseFloat(item.PASS_QTY * 100) / 100 +
                //             parseFloat(BottomList[BottomList.length - 1].TOBESCRAP_QTY * 100) / 100;
                //     }
                // });
                // BottomList[BottomList.length - 1].TD = handprops(BottomList[BottomList.length - 1], 'WIP_QTY', 'PASS_QTY');
                // BottomList[BottomList.length - 1].TR = handprops(BottomList[BottomList.length - 1], 'PASS_QTY', 'IN_QTY');
                // TopList[TopList.length - 1].TotalYield =
                //     parseFloat(TopList[TopList.length - 1].PASS_QTY * 100) /
                //     100 /
                //     (parseFloat(BottomList[BottomList.length - 1].TOBESCRAP_QTY * 100) / 100 +
                //         parseFloat(TopList[TopList.length - 1].PASS_QTY * 100) / 100);
                // TopList[TopList.length - 1].TotalYield = TopList[TopList.length - 1].TotalYield.toFixed(4) * 100;
                // BottomList[BottomList.length - 1]['WORK_CTR'] = 'RWK';
                PageSetting.TopColumns = topBaseColumns;
                PageSetting.BottomColumns = bottomBaseColumns;
                
                PageSetting.TopList = TopList;
                PageSetting.BottomList = BottomList;
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

// function showHidden() {
//     PageSetting.showmenu = !PageSetting.showmenu;
//     if (PageSetting.showmenu) PageSetting.TableSpan = 20;
//     else PageSetting.TableSpan = 24;
// }
// 属性处理
function handprops(item, prop1, prop2) {
    item[prop1] = Number(item[prop1]);
    item[prop2] = Number(item[prop2]);
    return item[prop1] / item[prop2] == Infinity || item[prop1] == 0 || isNaN(item[prop1] / item[prop2])
        ? 0
        : (item[prop1] / item[prop2]).toFixed(4);
}
// float属性处理
function handfloatprops(item, prop1, prop2) {
    item[prop1] = Number(item[prop1]);
    item[prop2] = Number(item[prop2]);
    return item[prop1] / item[prop2] == Infinity || item[prop1] == 0 || isNaN(item[prop1] / item[prop2])
        ? 0
        : ((item[prop1] / item[prop2]) * 100).toFixed(4);
}
// 总计行
// function handleData(dataSource: any[], colAttr: any[]) {
//     let keysArr = colAttr;
//     const tempObj = JSON.parse(JSON.stringify(dataSource[0]));
//     keysArr.forEach((el) => {
//         for (let i = 1; i < dataSource.length; i++) {
//             if (el == 'FirstYield') {
//                 if (isNaN(dataSource[i][el])) {
//                     dataSource[i][el] = 0.0;
//                 } else if (isNaN(tempObj[el])) {
//                     tempObj[el] = 0.0;
//                 }
//                 tempObj[el] = parseFloat(tempObj[el] * 100) / 100;
//                 tempObj[el] *= parseFloat(dataSource[i][el] * 100) / 100;
//                 tempObj[el] = tempObj[el].toFixed(4);
//                 continue;
//             } else if (
//                 [
//                     'TDR',
//                     'TRR',
//                     'RWK_PASS_QTY',
//                     'RWK_IN_QTY',
//                     'PASS2_QTYRate',
//                     'S1_WIP_QTY',
//                     'S2_WIP_QTY',
//                     'S3_WIP_QTY',
//                     'RepairRate'
//                 ].includes(el)
//             ) {
//                 tempObj[el] = '';
//                 continue;
//             } else {
//                 if (isNaN(dataSource[i][el])) {
//                     dataSource[i][el] = 0.0;
//                 } else if (isNaN(tempObj[el])) {
//                     tempObj[el] = 0.0;
//                 }
//                 tempObj[el] = Number(tempObj[el]);
//                 tempObj[el] += Number(dataSource[i][el]);
//             }
//         }
//         /* if (index == 0) {
//             tempObj[el] = '总计';
//         } */
//     });
//     dataSource.push(tempObj);
// }
/* function getTotalData(data: any[], columns: any[]) {
    //合計
    let obj = {};
    columns.map((m: Object) => {
        if (m['dataIndex'] != 'WORK_CTR') {
            let sum = 0;
            data.map((n) => {
                sum += isNaN(n[m['dataIndex']]) ? 0 : parseInt(n[m['dataIndex']]);
            });
            obj[m['dataIndex']] = sum;
        }
    });
    return obj;
} */

onMounted(async () => {
    // if (bigfab==="LCM"){
    //     await mounted(optionsdata,useCommonParams.Fab)
    //     await mountedLCMowner(optionsdata, useCommonParams.Fab); 
    //     await mountedLCDowner(optionsdata, useCommonParams.Fab); 
    //     await mountedModuleData(optionsdata, useCommonParams.Fab);
    //  }else{
        
    //  }

    // optionsdata.LcdOwnerLabel = await  getCommonFormParams('LCDOWNER',{
    //     name: "Date",
    //     apiName: "LcdOwner",
    //     FactoryType:useCommonParams.Fab,
    //     G_FAC: useCommonParams.G_FAC_COM
    // })

    // optionsdata.ModuleData = await getCommonFormParams('PROD_TYPE',{
    //     name: "Date",
    //     apiName: "Module",
    //     FactoryType:useCommonParams.Fab,
    //     G_FAC: useCommonParams.G_FAC_COM
    // })
    
});
</script>
<!-- <script lang="ts">
export default{
    name:'OperationPerformance'
}
</script> -->

<style lang="less" scoped>
.btn-rows {
    button {
        margin-right: 12px;
    }
}



.OperationPerformance :deep(.Yellow) {
    background-color: #80ff80;
    
}

.OperationPerformance :deep(.YellowGreen) {
    background-color: #f39230;
   
}

// .baseModel {
//     overflow-y: auto;
//     overflow-x: hidden;
//      max-width: 200px;
//     height: 85vh;
// }
</style>
