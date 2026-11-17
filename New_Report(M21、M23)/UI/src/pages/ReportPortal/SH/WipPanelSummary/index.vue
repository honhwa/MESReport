<template>
    <div class="WipPanelSummary">
        <a-card>
            <!-- <a-row>
                <span class="menu-fold" @click="showHidden(PageSetting)">
                    <component :is="!PageSetting.showmenu ? MenuUnfoldOutlined : MenuFoldOutlined" />
                </span>
            </a-row> -->
            <a-row>
                <a-col :span="24 - PageParams.TableSpan" class="SHScrollForm" v-show="PageSetting.showmenu">
                    <SchemaForm @submit="confirm"> </SchemaForm>
                </a-col>
                <a-col :span="PageParams.TableSpan">
                    <div class="ScrollTable" ref="WipPanelSummary">
                        <DynamicTable showFavorite sticky
                            row-key="WipPanelSummary"
                            :header-title="PageSetting.Title"
                            :columns="PageSetting.Columns"
                            :key="PageSetting.Columns"
                            bordered
                            size="small"
                            :data-source="PageSetting.List"
                            :data-all="PageSetting.moreData"
                             :pagination="false"
                            :PageParams="PageParams"
                            :row-class-name="getRowClassName" 
                            :scroll="{ x: PageSetting.x,}"                       
                            export-file-name="导出"
                        >
                           <!-- <template #toolbar>
                                <a-button type="primary" @click="isShowExplain = !isShowExplain">{{isShowExplain?'显示表格':'显示说明文件'}}</a-button>
                            </template> -->
                            <template #export-button>
                                <a-button type="success"> 导出 </a-button>
                            </template>
                        </DynamicTable>
                    </div>
                </a-col>
            </a-row>
             <div hidden>{{ PageSetting.SQLHidden }}</div>
        </a-card>
    </div>
</template>

<script lang="ts" setup>
import { MenuFoldOutlined, MenuUnfoldOutlined } from '@ant-design/icons-vue';
import { showHidden } from '@/pages/ReportPortal/utils/common';
// import { schemas } from './form-schema';
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, onMounted, ref } from 'vue';
import { cloneDeep } from 'lodash-es';
import useRemoteSelect from '@/pages/ReportPortal/utils/SHuseRemoteSelect';
import {getSumData} from '@/pages/ReportPortal/utils/SHcommon';
import { message } from 'ant-design-vue';
import type { TableColumn } from '@/components/core/dynamic-table';
import { GetCommon } from '@/api/pages/common';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { useRouter, useRoute } from 'vue-router';
import { Console } from 'console';
import { useLoading } from '@/components/feat/Loading';
import {PageParams} from '@/components/core/dynamic-table/src/dynamic-table'
import { ReportCommon } from '@/store/module/ReportCommon';
import { useInfiniteScroll, useVirtualList } from '@vueuse/core'

//let isShowExplain = ref<boolean>(false);
const router = useRouter();
const route = useRoute();
const { mounted,getCommonFormParams,mountedLCMowner,mountedModuleData,mountedOper,mountedHoldCode,InfiniteScroll } = useRemoteSelect();
const useCommonParams = AddCommonPageParams();
let useReportCommon = ReportCommon();
let bigfab=useCommonParams.Fab.slice(0,useCommonParams.Fab.length-1);

const WipPanelSummary = ref<HTMLElement | null>(null);
function getImport(site: any) {
    //let { optionsdata } = require('@/pages/ReportPortal/Static/SHCommon-Form-schema');
    const { schemas, GrpOptions } = require(`@/pages/ReportPortal/SH/WipPanelSummary/${bigfab}/form-schema`);
    
    return { schemas,  GrpOptions };
}
let { schemas,  GrpOptions } = getImport(useCommonParams.Fab);

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
let ReportNameParams = `WipPanelSummary-${useCommonParams.Fab}`;
useReportCommon.AddObject(ReportNameParams,ReportItem);


let PageSetting = reactive({
    List: [] as any,
    TableSpan: 20,
    showmenu: true,
    Columns: [] as any,
    moreData: [] as any,
    Title:'',
    SQLHidden:'',
    APIColumns: [] as any,
    x: '' as any,
});

const [DynamicTable] = useTable();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 130,
    schemas:schemas(useCommonParams.Fab,reactive({})),
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
});
const customCellRender = (record: Object, rowIndex: number, column: Object) => {
    return {
        ondblclick: () => {
            ReportItem.FirstTableParams={ ...record, ...column['__originColumn__'] };
            router.push({ name: `WipPanelSummaryDetail-${useCommonParams.Fab}`});
        },
        style: {
            cursor: 'pointer'
        }
    };
};


async function confirm(values) {
    openFullLoading();
    let dynamicColumns: TableColumn[] = [];

    //apiName傳參數 for 各個類型廠
    let ApiNamePara='';
    if (bigfab==="LCM"){
        ApiNamePara="WipPanelSumDataSet";
    }else if(bigfab==="BL"){
       ApiNamePara="WipPanelSumDataSet"; 
    }

    let baseParams = {name: "Date",
        apiName: ApiNamePara,
        FactoryType: useCommonParams.Fab,
        G_FAC: useCommonParams.G_FAC_COM,
    };

    if (values.Prod_nbr && values.Prod_nbr.length > 1000 )
    {
         closeFullLoading();
         return message.error('料號输入超過 “最大限制筆數:1000” '); 
    }
    
    if (values.Colname && values.Colname.length > 0) {
        let i=0;
        values.Colname.forEach((item) => {
            let itemsplit = item.split('.')[1] ?? item.split('.')[0];
            let Data = GrpOptions.filter((Prop) => Prop.value == item);            
            if(Data[0].label!="ALL"){
              {
                i++;
                let WidthPars=50;
                if(itemsplit.toUpperCase()=="PROD_NBR"||itemsplit.toUpperCase()=="WO_NBR"||itemsplit.toUpperCase()=="LCD_PROD_NBR")
                {
                   WidthPars=110;
                }else if(itemsplit.toUpperCase()=="DUE_DT"){
                   WidthPars=80;
                }

                else {
                   WidthPars=80;
                }

               if(i==1){
                dynamicColumns.push({
                    title: Data[0].label,
                    dataIndex: itemsplit.toUpperCase(),
                    align: 'center',
                    width:WidthPars,
                    customCell: customCellRender,
                    fixed:"left",
                    className:"v-center",
                });

               }else{
                  dynamicColumns.push({
                    title: Data[0].label,
                    dataIndex: itemsplit.toUpperCase(),
                    align: 'center',
                    width:WidthPars,
                    customCell: customCellRender
                });
               }
                
              }

           }
            
        });
    } else {
         closeFullLoading();
         return message.error('请输入显示栏位');
    } 
    
    let tempCol=dynamicColumns[0]["dataIndex"];
    let obj = {}
    obj[tempCol] = '合計'
    if (bigfab==="BL" ){
    PageSetting.Columns = [
        ...dynamicColumns,
       
        {
            title: 'SUB_Qty',
            align: 'center',
            dataIndex: 'SUB',
            width:60,
            customCell: customCellRender
            // customRender: ({ record }) => {
            //     return <span>{getAllSumData(record)}</span>;
            // }
        },
    
    ];}
    if (bigfab==="LCM" ){
    PageSetting.Columns = [
        ...dynamicColumns,
       
        {
            title: 'SUB_Qty',
            align: 'center',
            dataIndex: 'SUB',
            width:60,
            customCell: customCellRender
            // customRender: ({ record }) => {
            //     return <span>{getAllSumData(record)}</span>;
            // }
        },
        {
            title: 'Max Stay Hrs',
            align: 'center',
            dataIndex: 'MAX_START_HOURS',
            width:60,
            customCell: customCellRender,
            
            // customRender: ({ record }) => {
            //     return <span>{getAllSumData(record)}</span>;
            // }
        },
         {
            title: 'Start Over Hours',
            align: 'center',
            dataIndex: 'START_OVER_HOURS',
            width:60,
            customCell: customCellRender
            // customRender: ({ record }) => {
            //     return <span>{getAllSumData(record)}</span>;
            // }
        },
    
    ];}
    //獲取query頁面的條件值
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

    //獲取數據綁定table
    dynamicFormRef.validate().then(async () => {
      try{
          const { Date: { Data: { Table: Data },                    
                    sql_Floor,
                    sql_HoldCode,
                    sql_Operation,
                    sql_Prod_nbr,
                    sql_IsHold,
                    sql_Wo_nbr,
                    sql_StartHour,
                    sql_ToHour,
                    cuttime,
                    SQL01,
                    Colname } }  = await GetCommon({ ...baseParams, ...values });

          //console.log("WipPanelSumDataSet-SQLHidden:",SQL01)
          PageSetting.SQLHidden=ApiNamePara+"-SQLHidden:"+SQL01;

          let Message = {                
                sql_Floor: sql_Floor,
                sql_HoldCode: sql_HoldCode,
                sql_Operation: sql_Operation,
                sql_Prod_nbr: sql_Prod_nbr,
                sql_IsHold: sql_IsHold,
                sql_Wo_nbr:sql_Wo_nbr,
                sql_StartHour:sql_StartHour,
                sql_ToHour:sql_ToHour,
                Colname:Colname,
            };
          
          
          PageSetting.Title=`${useCommonParams.Fab} Wip Panel Summary ( 更新時間:${cuttime.slice(0,19)})`
          PageSetting.moreData=[];

          if (Data.length != 0) {
            let pushdata = cloneDeep(Data);
            let SUM_SUB = getSumData(Data.length, 0, Data, ["SUB"]);

            pushdata.splice(Data.length+1, 0, Object.assign(SUM_SUB, obj));            

            PageSetting.moreData = pushdata;
            //PageSetting.List = PageSetting.moreData

            PageSetting.x = null;
            if (PageSetting.Columns.length > 7) {
                PageSetting.x = 100 * PageSetting.Columns.length;
            }
            
            if (PageSetting.moreData.length > 1000 ) {
                PageSetting.List = PageSetting.moreData.slice(0, 100)
            
                // isLongData.value = true
            }//预先加载50条数据
            else {
                PageSetting.List = PageSetting.moreData
                // isLongData.value = false
            }
         }else{

                 message.warning("無數據！")
         }

          closeFullLoading();
          ReportItem.APIMessage = JSON.stringify(Message);  
          
          ReportItem.FormParams={ ...baseParams,...values }

      }catch(error: any){ 
           closeFullLoading();
           return Promise.reject(error); 
        }
        
    })

    // //獲取數據綁定table
    // dynamicFormRef.validate().then(async () => {

    //    try{
    //       const { Date: { Data: { Table: Data },sql } }  = await GetCommon({ ...baseParams, ...values });

    //       PageSetting.SQLHidden = sql
    //       console.log("DailyMove-SQLHidden:",sql)

    //         //循環查詢data，獲取table Colums 數據
    //         let ColumsData: any = [];
    //         let WidthPars=50;
    //         let titlePars="";//label
    //         let classNamePars="v-center";
    //         let middleProps: string[] = [];//API提供轉置后的時間處理（例如：A2022_11_30）

    //         for (let item in Data[0]) {  
    //             // if(item=="WORK_CTR"){
    //             //     WidthPars=70;
    //             //     titlePars="Operation" ;
    //             // }else if(item=="SIZE1"){
    //             //     WidthPars=40;
    //             //     titlePars="Size";
    //             // }else if(item=="LCM_OWNER"){
    //             //     WidthPars=50;
    //             //     titlePars="Owner" ;               
    //             // }else if(item=="PROD_NBR"){
    //             //     WidthPars=105;
    //             //     titlePars="Product";                
    //             // }else if(item=="WO_NBR"){
    //             //     WidthPars=100;
    //             //     titlePars="Work Oder";                
    //             // } else if(item=="CUST_GRP"){
    //             //     WidthPars=70;
    //             //     titlePars="Customer" ;               
    //             // }else if(item=="SO_NBR"){
    //             //     WidthPars=70;
    //             //     titlePars="需求單號" ;              
    //             // }else {
    //             //     WidthPars=70;
    //             //     if(item.slice(0,1)=="A"){
    //             //     titlePars=item.slice(1).replaceAll('_', '-');
    //             //     middleProps.unshift(item);

    //             //     }else{
    //             //     titlePars=item;
    //             //     } 
                    
    //             //     classNamePars="pro_auto-center";

    //             //}

    //             ColumsData.push({ title: item, align: 'center', dataIndex: item, className: classNamePars, width:WidthPars,
    //             // customCell: (record: Object, rowIndex: number, column: Object) => { 
    //             //     if(item=="WORK_CTR"||item=="SIZE1"||item=="LCM_OWNER"){
    //             //         let text = record[item]
    //             //         let obj = {
    //             //             text,
    //             //             style: {}
    //             //         }
    //             //         obj['rowSpan'] = record?.[`${item}-span`]?.['rowspan'] || 0
    //             //         return obj

    //             //     }else if(item=="CUST_GRP"){
    //             //         let text = record['CUST_GRP']
    //             //         let obj = {
    //             //             text,
    //             //             style: {}
    //             //         }
    //             //         if (text == '小计' || text == '合计') obj['colSpan'] = 3
    //             //         return obj
    //             //     }               
                        
    //             //     }
                
    //             });
    //         }

    //         //為加總提供要加總 計算的Columns
    //         PageSetting.APIColumns = [];
    //         PageSetting.APIColumns = middleProps;
    //         //console.log("PageSetting.APIColumns",PageSetting.APIColumns)

    //         PageSetting.Columns = [
    //             ...ColumsData,
    //             // {
    //             //     title: '总计',
    //             //     align: 'center',
    //             //     dataIndex: 'Total',
    //             //     className:'sum-center',
    //             //     width:60
    //             //     //className:'v-center'
    //             //     // customRender: ({ record }) => {
    //             //     //     return <span>{getAllSumData(record)}</span>;
    //             //     // }
    //             // }
    //         ]; //vue3或者adv无法监测push,shift等方法

            
    //         PageSetting.moreData = getTableData(Data)//进行表格合并

    //         if (PageSetting.moreData.length > 100) {
    //             PageSetting.List = PageSetting.moreData.slice(0, 50)
    //             // isLongData.value = true
    //         }//预先加载50条数据
    //         else {
    //             PageSetting.List = PageSetting.moreData
    //             // isLongData.value = false
    //         }
        


    //         // let config: PovitParams = {
    //         //     Data,
    //         //     povitColumn: {
    //         //         PD: 'QTY'
    //         //     },
    //         //     changeColumn: PageSetting.APIColumns,
    //         //     repeatColumn: repeatData
    //         // }

    //         // let povitData = getPovitData(unref(config))//进行类行列转置的动作
    //         // PageSetting.moreData = getTableData(povitData)//进行表格合并
    //         // if (PageSetting.moreData.length > 100) {
    //         //     PageSetting.List = PageSetting.moreData.slice(0, 40)
    //         //     // isLongData.value = true
    //         // }//预先加载40条数据
    //         // else {
    //         //     PageSetting.List = PageSetting.moreData
    //         //     // isLongData.value = false
    //         // }
    //         //加載防護罩結束
    //         closeFullLoading();

    //    }catch(error: any){ 
    //        closeFullLoading();
    //        return Promise.reject(error); 
    //     }

    
    // });


    
}
//实现无限滚动或者说懒加载
InfiniteScroll(WipPanelSummary, PageSetting);
onMounted(async () => {
    // if (bigfab==="LCM" ){
    //     await mounted(optionsdata,useCommonParams.Fab)    
    //     await mountedModuleData(optionsdata, useCommonParams.Fab);
    //     await mountedOper(optionsdata, useCommonParams.Fab);
    //     await mountedHoldCode(optionsdata, useCommonParams.Fab);
    // }
    // if (bigfab==="BL" ){
    //     await mounted(optionsdata,useCommonParams.Fab) 
    //     await mountedOper(optionsdata, useCommonParams.Fab);
    //     //await mountedHoldCode(optionsdata, useCommonParams.Fab);
    //    // await mountedModuleData(optionsdata, useCommonParams.Fab);
    // }
   
    // else{
        
    // }
});
</script>

