<template>
    <div class="ScrollTable">
    <div >
        <a-card>            
            <DynamicTable
                row-key="id" sticky
                :header-title="PageSetting.TopTitle"
                :columns="PageSetting.Columns"
                :key="PageSetting.List"
                :row-class-name="getRowClassName"
                bordered
                size="small"
                :style="{ display: 'table',width:PageSetting.Columns.length > 8 ? '100% !important' : '70% !important'}"
                :data-source="PageSetting.List"
                :pagination="false" :explain="false"
                :scroll="{y:250}"
                :OwnerParams="false"
                export-file-name="导出"
            >
                <template #export-button>
                    <a-button type="success"> 导出 </a-button>
                </template>
            </DynamicTable>
            <div hidden>{{ PageSetting.SQLHidden }}</div>
        </a-card>
        <a-card >
            <DynamicTable sticky
                row-key="id"
                :header-title="PageSetting.BottomTitle"
                :columns="PageSetting.BottomColumns"
                :key="PageSetting.BottomList"
                bordered
                size="small"
                :data-source="PageSetting.BottomList"
                :pagination="{
                                pageSizeOptions:['10','50','100','200'],
                                pageSize:10                            
                            }"
                :OwnerFoldParams="false"
                :scroll="{ x: PageSetting.x,y:450}"
                export-file-name="导出"
                :explain="false"
            >
                <template #export-button>
                    <a-button type="success"> 导出 </a-button>
                </template>
            </DynamicTable>
           
        </a-card>
    </div>

    <div hidden>{{ PageSetting.BottomSQLHidden }}</div>
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

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});
const UseParamsAdd = AddCommonPageParams();
function getImport(site: any) {
    const { DownSecondColumns, DownThridColumns,DownSecond2Columns,DownOperatorSecondColumns,DownThrid2Columns} = require(`@/pages/ReportPortal/SH/DownGrade/${bigfab}/Table`);
    return { DownSecondColumns, DownThridColumns,DownSecond2Columns,DownOperatorSecondColumns,DownThrid2Columns};
}
let useCommonParams = AddCommonPageParams();
let useReportCommon = ReportCommon();
let ReportNameParams=`DownGrade-${useCommonParams.Fab}`;
let bigfab=useCommonParams.Fab.slice(0,useCommonParams.Fab.length-1)

let tableParams = useReportCommon[ReportNameParams].FirstTableParams;
let formParams= useReportCommon[ReportNameParams].FormParams;
let APIMessage=JSON.parse(useReportCommon[ReportNameParams].APIMessage);

let { DownSecondColumns, DownThridColumns,DownSecond2Columns,DownOperatorSecondColumns,DownThrid2Columns } = getImport("SH");

console.log("useReportCommon",)

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
    x: '' as any,
});
const [DynamicTable] = useTable();

function GetDetailData(tableParams){   
   
    PageSetting.List=[];
    PageSetting.Columns=[];
    PageSetting.BottomList=[];
    PageSetting.BottomColumns=[];
    //加載防護罩開始
    openFullLoading();

    if(tableParams['col']+1===17||tableParams['col']+1===19){      
       GetDetail_DownGrade(tableParams);
    }
    
   if(tableParams['col']+1>=1 && tableParams['col']+1<=16){    
       GetDetail_Grade(tableParams);
    }
};

async function GetDetail_DownGrade(tableParams) { 
    
    formParams= useReportCommon[ReportNameParams].FormParams;
    APIMessage=JSON.parse(useReportCommon[ReportNameParams].APIMessage);
    let Col0Para="";

    if(formParams['ShowMode']==="Defect_Code"){
       Col0Para=tableParams['DFCT_CODE']
    }else if(formParams['ShowMode']==="Operator"){
       Col0Para=tableParams['OPERATORS']
    }else if(formParams['ShowMode']==="Prod_nbr"){
       Col0Para=tableParams['PROD_NBR']
    }

     const ParamsData: any = {
        name: "Date",
        apiName: "TDownGrade_DownGradeQty",
        FactoryType: formParams["FactoryType"],
        G_FAC: formParams["G_FAC"],
        StrSql_w:APIMessage["StrSql_w"],
        StrSql_w1:APIMessage["StrSql_w1"],
        lstr_ShowMode:APIMessage["lstr_ShowMode"],
        lstr_Operation:APIMessage["lstr_Operation"],

        col0:Col0Para?? '',
        col2:tableParams["DOWNGRADE"],
        col3:tableParams['col']+1,
     };

     PageSetting.TopTitle = `${formParams["FactoryType"]} Down Grade Detail`;

    try{
        const { Date: { Data: { Table: Data },
        SQL01,
        StrSql_w1,
        StrSql_w,
        lstr_ShowMode,
        lstr_Operation
         } } = await GetCommon(ParamsData);
        //console.log("TDownGrade_DownGradeQty-SQLSecondDetail:",SQL01) 
        PageSetting.SQLHidden="TDownGrade_DownGradeQty-SQLSecondDetail:"+SQL01;

        SendMessages = { 
        StrSql_w1:StrSql_w1,
        StrSql_w:StrSql_w,
        lstr_ShowMode:lstr_ShowMode,
        lstr_Operation:lstr_Operation,
        };

        let ColumnsData = {
            Defect_Code: [{ title: 'Product', dataIndex: 'PROD_NBR', align: 'center',
                  customCell: (record: Object, rowindex: number, column: Object) => { 
                    let text = record['PROD_NBR']
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
                 }, ...DownSecond2Columns],
            Operator: [{ title: '工號', dataIndex: 'OPERATOR', align: 'center',
                  customCell: (record: Object, rowindex: number, column: Object) => { 
                    let text = record['OPERATOR']
                                    let obj = {
                                        text,
                                        style: {},
                                        onDblclick: () => {
                                        message.warning('此栏位没有二阶数据')
                                    }
                                 }
                                    obj['rowSpan'] = record?.[`${'OPERATOR'}-span`]?.['rowspan'] || 0
                                    return obj
                      } 
                    }, ...DownSecond2Columns],
            Prod_nbr: [{ title: 'Product', dataIndex: 'PROD_NBR', align: 'center',
                  customCell: (record: Object, rowindex: number, column: Object) => { 
                    let text = record['PROD_NBR']
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
               }, ...DownSecond2Columns],
        };

       
        PageSetting.Columns = ColumnsData[formParams['ShowMode']];
            
            if(formParams['ShowMode']==="Defect_Code"){
              let { insertIndexList, insertrowList } = Inserttotalrow(Data, ["PROD_NBR"], 
                ['QTY',], "PROD_NBR");               

            }else{

               let SUM_Sub = getSumData(Data.length, 0, Data, ["QTY"]);
               Data.splice(Data.length, 0, Object.assign(SUM_Sub, { ITEM: '小計' }));
            }
            

            let temp_mergeColNames: any
            if(formParams['ShowMode']==="Operator"){
               temp_mergeColNames= ['OPERATOR'];
            }else{
               temp_mergeColNames= ['PROD_NBR']; 
            }
                  
            let combineCol1: any = {
            data: Data,
            mergeColNames: temp_mergeColNames,
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

async function GetDetail_Grade(tableParams) { 
    
    formParams= useReportCommon[ReportNameParams].FormParams;
    APIMessage=JSON.parse(useReportCommon[ReportNameParams].APIMessage);
    
    let Col0Para="";

    if(formParams['ShowMode']==="Defect_Code"){
       Col0Para=tableParams['DFCT_CODE']
    }else if(formParams['ShowMode']==="Operator"){
       Col0Para=tableParams['OPERATORS']
    }else if(formParams['ShowMode']==="Prod_nbr"){
       Col0Para=tableParams['PROD_NBR']
    }
     const ParamsData: any = {
        name: "Date",
        apiName: "TDownGrade_GradeQty",
        FactoryType: formParams["FactoryType"],
        G_FAC: formParams["G_FAC"],
        StrSql_w:APIMessage["StrSql_w"],
        StrSql_w1:APIMessage["StrSql_w1"],
        lstr_ShowMode:APIMessage["lstr_ShowMode"],
        lstr_Operation:APIMessage["lstr_Operation"],
        sql_WoType:APIMessage["sql_WoType"],

        col0:Col0Para?? '',
        col2:tableParams["DOWNGRADE"],
        col1:tableParams["title"].replace('*', ''),
     };

     PageSetting.TopTitle = `${formParams["FactoryType"]} Down Grade Detail`;

    try{
        const { Date: { Data: { Table: Data },
        SQL01,
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
            Defect_Code: [{ title: 'Product', dataIndex: 'PROD_NBR', align: 'center',
                  customCell: (record: Object, rowindex: number, column: Object) => { 
                    let text = record['PROD_NBR']
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
            Operator: [{ title: 'Product', dataIndex: 'PROD_NBR', align: 'center',
                  customCell: (record: Object, rowindex: number, column: Object) => { 
                    let text = record['PROD_NBR']
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
                    }, ...DownOperatorSecondColumns],
            Prod_nbr: [{ title: 'Product', dataIndex: 'PROD_NBR', align: 'center',
                  customCell: (record: Object, rowindex: number, column: Object) => { 
                    let text = record['PROD_NBR']
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
        Data.splice(Data.length, 0, Object.assign(SUM_Sub, { PROD_NBR: '總計' }));
         
                  
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
    if(tableParams['col']+1===17||tableParams['col']+1===19){
       GetSecond_DownGrade(tableParams,SecondTableParams);
    }
    
   if(tableParams['col']+1>=1 && tableParams['col']+1<=16){      
       GetSecond_Grade(tableParams,SecondTableParams);
    }
};


async function GetSecond_DownGrade(tableParams: any, SecondTableParams: any) {
   let temp_Col2_0="";
   if(formParams['ShowMode']==="Operator"){
     temp_Col2_0=SecondTableParams["OPERATOR"]||'';

   }else{
     temp_Col2_0=SecondTableParams["PROD_NBR"]||'';
   }
   
    formParams= useReportCommon[ReportNameParams].FormParams;
    APIMessage=JSON.parse(useReportCommon[ReportNameParams].APIMessage);
    
     const ParamsData: any = {
        name: "Date",
        apiName: "TDownGrade_DownGradeQtyDetail",
        FactoryType: formParams["FactoryType"],
        G_FAC: formParams["G_FAC"],
        StrSql_w1:APIMessage["StrSql_w1"],
        StrSql_w:APIMessage["StrSql_w"],
        lstr_ShowMode:APIMessage["lstr_ShowMode"],
        lstr_Operation:APIMessage["lstr_Operation"],

        Col2_0:temp_Col2_0,
        Item:SecondTableParams["ITEM"]|| '',
        Defect:SecondTableParams["DEFECT_CODE"]|| '',
        Grade:SecondTableParams["LCM_GRADE"]|| '',
     };

     if(temp_Col2_0!="合計"){
        PageSetting.BottomTitle = `${formParams["FactoryType"]} Down Grade Detail`;
        try{
            const { Date: { Data: { Table: Data },SQL01 } } = await GetCommon(ParamsData);
            //console.log("TDownGrade_DownGradeQtyDetail-SQLThirdDetail:",SQL01) 
            PageSetting.BottomSQLHidden="TDownGrade_DownGradeQtyDetail-SQLThirdDetail:"+SQL01;

            PageSetting.BottomList=Data;
            PageSetting.BottomColumns=DownThrid2Columns;
            PageSetting.x = null;
            if (PageSetting.BottomColumns.length > 7) {
                PageSetting.x = 120 * PageSetting.BottomColumns.length;
            }    

            //加載防護罩結束
            closeFullLoading();

        } catch(error: any){ 
            //加載防護罩結束
            closeFullLoading();
            return Promise.reject(error); 
        }
     }else{
        //加載防護罩結束
         closeFullLoading();
        message.warning('此栏位没有二阶数据')
        return;
     }
     

  
}


async function GetSecond_Grade(tableParams: any, SecondTableParams: any) {

    formParams= useReportCommon[ReportNameParams].FormParams;
    APIMessage=JSON.parse(useReportCommon[ReportNameParams].APIMessage);
    
     const ParamsData: any = {
        name: "Date",
        apiName: "TDownGrade_GradeQtyDetail",
        FactoryType: formParams["FactoryType"],
        G_FAC: formParams["G_FAC"],
        StrSql_w1:SendMessages["StrSql_w1"],
        lstr_ShowMode:APIMessage["lstr_ShowMode"],
        lstr_Operation:APIMessage["lstr_Operation"],
        sql_DownGrade:SendMessages["sql_DownGrade"],
        col0:SendMessages["col0"],
        col1:SendMessages["col1"],

        Product:SecondTableParams["PROD_NBR"]|| '',
        Defect:SecondTableParams["DFCT_CODE"]|| '',
        Grade:SecondTableParams["LCM_GRADE"]|| '',

        // col0:tableParams['OPERATORS'] ?? tableParams['PROD_NBR'] ?? tableParams['DFCT_CODE'] ?? '',
        // col2:tableParams["DOWNGRADE"],
        // col1:tableParams["title"].replace('*', ''),
     };

     PageSetting.BottomTitle = `${formParams["FactoryType"]} Down Grade Detail`;
    try{
        const { Date: { Data: { Table: Data },SQL01 } } = await GetCommon(ParamsData);
        //console.log("TDownGrade_GradeQtyDetail-SQLThirdDetail:",SQL01) 
        PageSetting.BottomSQLHidden="TDownGrade_GradeQtyDetail-SQLThirdDetail:"+SQL01;

        // let SUM_Sub = getSumData(Data.length, 0, Data, ["QTY"]);
        // Data.splice(Data.length, 0, Object.assign(SUM_Sub, { PROD_NBR: '小計' }));

        let ColumnsData = [{ title: 'Product', dataIndex: 'PROD_NBR', align: 'center',width:60,
                  customCell: (record: Object, rowindex: number, column: Object) => { 
                    let text = record['PROD_NBR']
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
                    },...DownThridColumns  
        ]

        
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
        PageSetting.x = null;
        if (PageSetting.BottomColumns.length > 7) {
            PageSetting.x = 120 * PageSetting.BottomColumns.length;
        } 
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
    console.log("tableParams",tableParams)
    GetDetailData(tableParams);
});
watch(
    //二阶表格监控
    () => useReportCommon[ReportNameParams].SecondTableParams,
    (newValue) => {
        PageSetting['BottomColumns'] = [];
        PageSetting['BottomTitle'] = "";
        //tableParams = useCommonParams.FirstTableParams;        
        GetSecondTable(tableParams, newValue);
    },
    {
        deep: true
    }
);

</script>


