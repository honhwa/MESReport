<template>
    <div>
        <a-card>
            <DynamicTable
                row-key="id"
                :header-title="PageSetting.TopTitle"
                :columns="PageSetting.Columns"
                :key="PageSetting.List"
                :row-class-name="getRowClassName"
                bordered
                size="small"
                :data-source="PageSetting.List"
                :pagination="false" :explain="false"
                :scroll="{y:350}"
                sticky
                :OwnerParams="false"
                export-file-name="Export"
            >
                <template #export-button>
                    <a-button type="success"> Export </a-button>
                </template>
            </DynamicTable>
            <div hidden>{{ PageSetting.SQLHidden }}</div>
        </a-card>
        <a-card>
            <DynamicTable
                row-key="id"
                :header-title="PageSetting.BottomTitle"
                :columns="PageSetting.BottomColumns"
                :key="PageSetting.BottomList"
                bordered
                size="small"
                :data-source="PageSetting.BottomList"
                :pagination="false"
                :OwnerFoldParams="false"
                :scroll="{x:1500}"
                sticky
                export-file-name="Export"
                :explain="false"
            >
                <template #export-button>
                    <a-button type="success"> Export </a-button>
                </template>
            </DynamicTable>
            <div hidden>{{ PageSetting.BottomSQLHidden }}</div>
        </a-card>
    </div>
</template>


<script lang="ts" setup>
import { useTable } from '@/components/core/dynamic-table';
import { reactive, watchEffect, watch, computed } from 'vue';
import { GetCommon } from '@/api/pages/common';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { ReportCommon } from '@/store/module/ReportCommon';
import { useLoading } from '@/components/feat/Loading';
import { message } from 'ant-design-vue';
import { Console } from 'console';
import { mergeTableRow,getSumData,getRowClassName,Inserttotalrow} from '@/pages/ReportPortal/utils/TNcommon'
// import { useVirtualList } from '@vueuse/core';

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});
const UseParamsAdd = AddCommonPageParams();
function getImport(site: any) {
    const { DownSecondColumns, DownThridColumns,DownSecond2Columns,DownOperatorSecondColumns,DownThrid2Columns} = require(`@/pages/ReportPortal/TN/DownGrade/MOD/Table`);
    return { DownSecondColumns, DownThridColumns,DownSecond2Columns,DownOperatorSecondColumns,DownThrid2Columns};
}
let { DownSecondColumns, DownThridColumns,DownSecond2Columns,DownOperatorSecondColumns,DownThrid2Columns } = getImport(UseParamsAdd.Fab);

let useCommonParams = AddCommonPageParams();
let useReportCommon = ReportCommon();

let ReportNameParams = `DownGradeDetail_${useCommonParams.Fab}`;
let tableParams = useReportCommon[ReportNameParams].FirstTableParams;
let APIMessage = JSON.parse(useReportCommon[ReportNameParams].APIMessage);
let formParams = useReportCommon[ReportNameParams].FormParams;
let SendMessages:any;

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

});
const [DynamicTable] = useTable();

function GetDetailData(tableParams){   
   
    PageSetting.List=[];
    PageSetting.Columns=[];
    PageSetting.BottomList=[];
    PageSetting.BottomColumns=[];
    //加載防護罩開始
    openFullLoading();

    GetDetail_Grade(tableParams);

//     if(tableParams['col']+1===17||tableParams['col']+1===19){      
//        GetDetail_DownGrade(tableParams);
//     }
    
//    if(tableParams['col']+1>=1 && tableParams['col']+1<=16){    
//        GetDetail_Grade(tableParams);
//     }
};

// async function GetDetail_DownGrade(tableParams) { 
        
//     APIMessage=JSON.parse(useCommonParams.APIMessage); 
//     formParams = useCommonParams.FormParams;
//      const ParamsData: any = {
//         name: "Date",
//         apiName: "TDownGrade_DownGradeQty",
//         FactoryType: useCommonParams.Fab,
//         G_FAC: useCommonParams.G_FAC_COM,
//         StrSql_w:APIMessage["StrSql_w"],
//         StrSql_w1:APIMessage["StrSql_w1"],
//         lstr_ShowMode:APIMessage["lstr_ShowMode"],
//         lstr_Operation:APIMessage["lstr_Operation"],

//         col0:tableParams['OPERATORS'] ?? tableParams['PROD_NBR'] ?? tableParams['DFCT_CODE'] ?? '',
//         col2:tableParams["DOWNGRADE"],
//         col3:tableParams['col']+1,
//      };

//      PageSetting.TopTitle = `${useCommonParams.Fab} Down Grade Detail`;

//     try{
//         const { Date: { Data: { Table: Data },
//         SQL01,
//         StrSql_w1,
//         StrSql_w,
//         lstr_ShowMode,
//         lstr_Operation
//          } } = await GetCommon(ParamsData);
//         //console.log("TDownGrade_DownGradeQty-SQLSecondDetail:",SQL01) 
//         PageSetting.SQLHidden="TDownGrade_DownGradeQty-SQLSecondDetail:"+SQL01;

//         SendMessages = { 
//         StrSql_w1:StrSql_w1,
//         StrSql_w:StrSql_w,
//         lstr_ShowMode:lstr_ShowMode,
//         lstr_Operation:lstr_Operation,
//         };

//         let ColumnsData = {
//             Defect_Code: [{ title: 'Product', dataIndex: 'PROD_NBR', align: 'center',
//                   customCell: (record: Object, rowindex: number, column: Object) => { 
//                     let text = record['PROD_NBR']
//                                     let obj = {
//                                         text,
//                                         style: {},
//                                         onDblclick: () => {
//                                         message.warning('此栏位没有二阶数据')
//                                     }
//                                  }
//                                     obj['rowSpan'] = record?.[`${'PROD_NBR'}-span`]?.['rowspan'] || 0
//                                     return obj
//                       }     
//                  }, ...DownSecond2Columns],
//             Operator: [{ title: '工號', dataIndex: 'OPERATOR', align: 'center',
//                   customCell: (record: Object, rowindex: number, column: Object) => { 
//                     let text = record['OPERATOR']
//                                     let obj = {
//                                         text,
//                                         style: {},
//                                         onDblclick: () => {
//                                         message.warning('此栏位没有二阶数据')
//                                     }
//                                  }
//                                     obj['rowSpan'] = record?.[`${'OPERATOR'}-span`]?.['rowspan'] || 0
//                                     return obj
//                       } 
//                     }, ...DownSecond2Columns],
//             Prod_nbr: [{ title: 'Product', dataIndex: 'PROD_NBR', align: 'center',
//                   customCell: (record: Object, rowindex: number, column: Object) => { 
//                     let text = record['PROD_NBR']
//                                     let obj = {
//                                         text,
//                                         style: {},
//                                         onDblclick: () => {
//                                         message.warning('此栏位没有二阶数据')
//                                     }
//                                  }
//                                     obj['rowSpan'] = record?.[`${'PROD_NBR'}-span`]?.['rowspan'] || 0
//                                     return obj
//                       }
//                }, ...DownSecond2Columns],
//         };

       
//         PageSetting.Columns = ColumnsData[formParams['ShowMode']];
            
//             if(formParams['ShowMode']==="Defect_Code"){

//               let { insertIndexList, insertrowList } = Inserttotalrow(Data, ["PROD_NBR"], 
//                 ['QTY',], "PROD_NBR");

//             }else{

//                let SUM_Sub = getSumData(Data.length, 0, Data, ["QTY"]);
//                Data.splice(Data.length, 0, Object.assign(SUM_Sub, { ITEM: '小計' }));
//             }
            

//             let temp_mergeColNames: any
//             if(formParams['ShowMode']==="Operator"){
//                temp_mergeColNames= ['OPERATOR'];
//             }else{
//                temp_mergeColNames= ['PROD_NBR']; 
//             }
                  
//             let combineCol1: any = {
//             data: Data,
//             mergeColNames: temp_mergeColNames,
//             //firstMergeColNames: ['FLOOR'],
//             firstMerge: ColumnsData[formParams['ShowMode']][0]['dataIndex']
//             };

//             /**进行表格合并 */
//             PageSetting.moreData=mergeTableRow(combineCol1);                
//             PageSetting.List=PageSetting.moreData;

//              //加載防護罩結束
//             closeFullLoading();
       
//     }catch(error: any){ 
//          //加載防護罩結束
//         closeFullLoading();
//         return Promise.reject(error); 
//      }
    
// }

async function GetDetail_Grade(tableParams) { 
    //加載防護罩開始
    openFullLoading();
    APIMessage=JSON.parse(useReportCommon[ReportNameParams].APIMessage); 
    formParams = useReportCommon[ReportNameParams].FormParams;
    debugger
     const ParamsData: any = {
        name: "Date",
        apiName: "TN_DownGradeDataSetDetail",
        FactoryType: useCommonParams.Fab,
        G_FAC: useCommonParams.G_FAC_COM,
        Sqlwhere:APIMessage["SQLFilter"],
        ITEM:tableParams['OPERATOR'] ?? tableParams['PROD_NBR'] ?? '',
        TableTitle:tableParams["title"].replace('*', ''),
        ShowMode:formParams["ShowMode"],
     };

     PageSetting.TopTitle = `${useCommonParams.Fab} Down Grade Detail`;

    try{
        const { Date: { Data: { Table: Data },
        SQL01,
        SQLFilter,
        sql_DownGrade,
        StrSql_w1,
        lstr_ShowMode,
        lstr_Operation,
        col0,
        col1
       } } = await GetCommon(ParamsData);
        //console.log("TDownGrade_GradeQty-SQLSecondDetail:",SQL01) 
         PageSetting.SQLHidden="TDownGrade_GradeQty-SQLSecondDetail:"+SQL01;

        SendMessages = {  
        sql_DownGrade: sql_DownGrade,
        StrSql_w1:StrSql_w1,
        lstr_ShowMode:lstr_ShowMode,
        lstr_Operation:lstr_Operation,
        col0:col0,
        col1:col1, 
        };


        let ColumnsData = {
            Operator: [{ title: '工號', dataIndex: 'SHOW_MODE', align: 'center',
                  customCell: (record: Object, rowindex: number, column: Object) => { 
                    let text = record['SHOW_MODE']
                                    let obj = {
                                        text,
                                        style: {},
                                        onDblclick: () => {
                                        message.warning('此栏位没有二阶数据')
                                    }
                                 }
                                    obj['rowSpan'] = record?.[`${'PROD_NBR'}-span`]?.['rowspan'] || 0
                                    return obj
                      } 
                    }, ...DownSecondColumns],
            Prod_nbr: [{ title: 'Product', dataIndex: 'SHOW_MODE', align: 'center',
                  customCell: (record: Object, rowindex: number, column: Object) => { 
                    let text = record['SHOW_MODE']
                                    let obj = {
                                        text,
                                        style: {},
                                        onDblclick: () => {
                                        message.warning('此栏位没有二阶数据')
                                    }
                                 }
                                    obj['rowSpan'] = record?.[`${'PROD_NBR'}-span`]?.['rowspan'] || 0
                                    return obj
                      }
               }, ...DownSecondColumns],
        };

       
        PageSetting.Columns = ColumnsData[formParams['ShowMode']];
        let SUM_Sub = getSumData(Data.length, 0, Data, ["QTY"]);
        Data.splice(Data.length, 0, Object.assign(SUM_Sub, { SHOW_MODE: '總計' }));
         
                  
        let combineCol1: any = {
        data: Data,
        mergeColNames: ['PROD_NBR'],
        //firstMergeColNames: ['FLOOR'],
        firstMerge: ColumnsData[formParams['ShowMode']][0]['dataIndex']
        };

        /**进行表格合并 */
        PageSetting.moreData=mergeTableRow(combineCol1);                
        PageSetting.List=PageSetting.moreData;
         //加載防護罩結束
        closeFullLoading();
       
    }catch(error: any){ 
         //加載防護罩結束
        closeFullLoading();
        return Promise.reject(error); 
     }
    
}


function GetSecondTable(tableParams,SecondTableParams){ 

     //加載防護罩開始
    openFullLoading();
    GetSecond_Grade(tableParams,SecondTableParams);
//     if(tableParams['col']+1===17||tableParams['col']+1===19){
//        GetSecond_DownGrade(tableParams,SecondTableParams);
//     }
    
//    if(tableParams['col']+1>=1 && tableParams['col']+1<=16){      
//        GetSecond_Grade(tableParams,SecondTableParams);
//     }
};


// async function GetSecond_DownGrade(tableParams: any, SecondTableParams: any) {
//    let temp_Col2_0="";
//    if(DownGradeformParams['ShowMode']==="Operator"){
//      temp_Col2_0=SecondTableParams["OPERATOR"]||'';

//    }else{
//      temp_Col2_0=SecondTableParams["PROD_NBR"]||'';
//    }
   
//    DownGradeAPIMessage=JSON.parse(useCommonParams.DownGradeAPIMessage); 
//    DownGradeformParams = useCommonParams.DownGradeFormParams;
//      const ParamsData: any = {
//         name: "Date",
//         apiName: "TDownGrade_DownGradeQtyDetail",
//         FactoryType: useCommonParams.Fab,
//         G_FAC: useCommonParams.G_FAC_COM,
//         StrSql_w1:DownGradeAPIMessage["StrSql_w1"],
//         StrSql_w:DownGradeAPIMessage["StrSql_w"],
//         lstr_ShowMode:DownGradeAPIMessage["lstr_ShowMode"],
//         lstr_Operation:DownGradeAPIMessage["lstr_Operation"],

//         Col2_0:temp_Col2_0,
//         Item:SecondTableParams["ITEM"]|| '',
//         Defect:SecondTableParams["DEFECT_CODE"]|| '',
//         Grade:SecondTableParams["LCM_GRADE"]|| '',
//      };

//      PageSetting.BottomTitle = `${useCommonParams.Fab} Down Grade Detail`;
//     try{
//         const { Date: { Data: { Table: Data },SQL01 } } = await GetCommon(ParamsData);
//         //console.log("TDownGrade_DownGradeQtyDetail-SQLThirdDetail:",SQL01) 
//          PageSetting.BottomSQLHidden="TDownGrade_DownGradeQtyDetail-SQLThirdDetail:"+SQL01;

//         PageSetting.BottomList=Data;
//         PageSetting.BottomColumns=DownThrid2Columns;
//          //加載防護罩結束
//          closeFullLoading();

//     } catch(error: any){ 
//          //加載防護罩結束
//          closeFullLoading();
//         return Promise.reject(error); 
//      }

  
// }


async function GetSecond_Grade(tableParams: any, SecondTableParams: any) {

    APIMessage=JSON.parse(useReportCommon[ReportNameParams].APIMessage); 
    formParams = useReportCommon[ReportNameParams].FormParams;
     const ParamsData: any = {
        name: "Date",
        apiName: "TN_DownGradeDataSetDetail2",
        FactoryType: useCommonParams.Fab,
        G_FAC: useCommonParams.G_FAC_COM,
        Sqlwhere:APIMessage["SQLFilter"],
        ITEM:tableParams['OPERATOR'] ?? tableParams['PROD_NBR'] ?? '',
        TableTitle:tableParams["title"].replace('*', ''),
        ShowMode:formParams["ShowMode"],
        Defect:SecondTableParams["DEFECT_CODE"]|| '',
        Grade:SecondTableParams["LCM_GRADE"]|| '',

        // col0:tableParams['OPERATORS'] ?? tableParams['PROD_NBR'] ?? tableParams['DFCT_CODE'] ?? '',
        // col2:tableParams["DOWNGRADE"],
        // col1:tableParams["title"].replace('*', ''),
     };

     PageSetting.BottomTitle = `${useCommonParams.Fab} Down Grade Detail`;
    try{
        const { Date: { Data: { Table: Data },SQL01 } } = await GetCommon(ParamsData);
        //console.log("TDownGrade_GradeQtyDetail-SQLThirdDetail:",SQL01) 
        PageSetting.BottomSQLHidden="TDownGrade_GradeQtyDetail-SQLThirdDetail:"+SQL01;

        // let SUM_Sub = getSumData(Data.length, 0, Data, ["QTY"]);
        // Data.splice(Data.length, 0, Object.assign(SUM_Sub, { PROD_NBR: '小計' }));

        let ColumnsData = [...DownThridColumns  ]

        
        let combineCol1: any = {
        data: Data,
        mergeColNames: ['PROD_NBR'],
        //firstMergeColNames: ['FLOOR'],
        firstMerge: ['PROD_NBR']
        };

        /**进行表格合并 */
        PageSetting.moreData=mergeTableRow(combineCol1);  
        PageSetting.BottomList=PageSetting.moreData;
        PageSetting.BottomColumns=ColumnsData;
         //加載防護罩結束
        closeFullLoading();

    } catch(error: any){ 
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
watch(
    //二阶表格监控
    () => useReportCommon[ReportNameParams].SecondTableParams,
    (newValue) => {
        PageSetting['BottomColumns'] = [];
        PageSetting['BottomTitle'] = "";
        //tableParams = useCommonParams.FirstTableParams;
        debugger
        GetSecondTable(tableParams, newValue);
    },
    {
        deep: true
    }
);
</script>


