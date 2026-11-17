<template>
    <div class="PanelWipStayNew">
        <a-card>
            <!-- <a-row>
                <span class="menu-fold" @click="showHidden(PageSetting)">
                    <component :is="!PageSetting.showmenu ? MenuUnfoldOutlined : MenuFoldOutlined" />
                </span>
            </a-row> -->
            <a-row>
                <a-col :span="24 - PageParams.TableSpan" class="NGBScrollForm" v-show="PageSetting.showmenu">
                    <SchemaForm @submit="confirm"> </SchemaForm>
                </a-col>
                <a-col :span="PageParams.TableSpan">
                    <div class="ScrollTable" ref="PanelWipStayNewTable">
                        <!-- <div v-bind="containerProps" style="height: 80vh"> -->
                        <!-- v-bind="wrapperProps" -->
                        <DynamicTable
                            sticky
                            showFavorite
                            row-key="id"
                            :header-title="PageSetting.Title"
                            :columns="PageSetting.Columns"
                            :key="PageSetting.Columns"
                            bordered
                            size="small"
                            :PageParams="PageParams"
                            :pagination="false"
                            :data-source="PageSetting.List"
                            :scroll="{ x: PageSetting.x,}"
                            export-file-name="导出"
                            :data-all="PageSetting.moreData"
                            :row-class-name="getRowClassName_temp"
                        >
                            <!-- <template #toolbar>
                                <a-button type="primary" @click="isShowExplain = !isShowExplain">{{isShowExplain?'显示表格':'显示说明文件'}}</a-button>
                            </template>  -->
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


<script lang="tsx" setup>
import { MenuFoldOutlined, MenuUnfoldOutlined } from '@ant-design/icons-vue';
// import { schemas } from './form-schema';
import {useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, unref, ref, onMounted, watch, computed, Ref } from 'vue';
import { GetCommon } from '@/api/pages/common';
import { cloneDeep } from 'lodash-es';
import useRemoteSelect from '@/pages/ReportPortal/utils/NGBuseRemoteSelect';
import { showHidden, getSumData ,handfloatprops,mergeTableRow} from '@/pages/ReportPortal/utils/NGBcommon';
import { message } from 'ant-design-vue';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { useLoading } from '@/components/feat/Loading';
import {PageParams} from '@/components/core/dynamic-table/src/dynamic-table'
import { ReportCommon } from '@/store/module/ReportCommon';
//import { useVirtualList } from '@vueuse/core'

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});

//let isShowExplain = ref<boolean>(false);
const useCommonParams = AddCommonPageParams();
let useReportCommon = ReportCommon();
let bigfab=useCommonParams.Fab.slice(0,useCommonParams.Fab.length-1)
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
let ReportNameParams = `PanelWipStayNew-${useCommonParams.Fab}`;
useReportCommon.AddObject(ReportNameParams,ReportItem);

// console.log(ReportNameParams,"dddddlllll")
// console.log(useCommonParams.G_FAC_COM,"dddddlllll")
// console.log(useCommonParams.Fab,"ddddd22222")
let fab = "LCM";
if(useCommonParams.Fab == "LCM1" || useCommonParams.Fab == "LCM3" )
{
    fab = useCommonParams.Fab;
}


 const PanelWipStayNewTable = ref<HTMLElement | null>(null)
function getImport(site: any) {
   // const { optionsdata } = require('@/pages/ReportPortal/Static/NGBCommon-Form-schema');
    const { schemas } = require(`@/pages/ReportPortal/NGB/MOD/PanelWipStayNew/${fab}/form-schema`);
    const { PanelWipColumns, TableColumnItem, PanelWipLabel,PanelWipProps } = require(`@/pages/ReportPortal/NGB/MOD/PanelWipStayNew/${bigfab}/Table`);

    return { schemas, PanelWipColumns, TableColumnItem,  PanelWipLabel,PanelWipProps};
}

let { schemas, PanelWipColumns,  PanelWipLabel,PanelWipProps } = getImport(useCommonParams.Fab);
let { InfiniteScroll, } = useRemoteSelect();

let PageSetting = reactive({
    List: [] as any,
    TableSpan: 20,
    showmenu: true,
    APIColumns: [] as any,
    Columns: [] as any,
    Title:"",
    moreData: [] as any,
    SQLHidden:'',
    x: '' as any,
});


const [DynamicTable] = useTable();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 130,
    schemas:schemas(        
    useCommonParams.Fab,reactive({})),
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
});


async function confirm(values) {
       

    //table 初始化
    PageSetting.List=[];
    PageSetting.Columns=[];
    openFullLoading();

if(values.FAC=="LCM2")
{
    useCommonParams.Fab="LCM2"
    useCommonParams.G_FAC_COM="B"
} 
if(values.FAC=="LCM1")
{
    useCommonParams.Fab="LCM1"
    useCommonParams.G_FAC_COM="C"

} 
if(values.FAC=="LCM4")
{
    useCommonParams.Fab="LCM4"
    useCommonParams.G_FAC_COM="D"
} 
if(values.FAC=="LCM3")
{
    useCommonParams.Fab="LCM3"
    useCommonParams.G_FAC_COM="E"

} 

closeFullLoading();
     //apiName傳參數 for 各個類型廠
    let ApiNamePara='';
    if (bigfab==="LCM"){
        ApiNamePara="PanelWipStayNewDataSet_MOD";
    }
    let baseParams = { name: "Date",apiName: ApiNamePara, FactoryType: useCommonParams.Fab, G_FAC:useCommonParams.G_FAC_COM, };
    let ifprod="NO";
  
    /**values为所设定的所有参数,将values里为undefined的值赋为空*/
    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        }
    });

    if (values.Prod_nbr && values.Prod_nbr.length > 1000 )
    {
         closeFullLoading();
         return message.error('料號输入超過 “最大限制筆數:1000” '); 
    }

    for (let i in dynamicFormRef?.formModel) {
        if (i == 'Wo_nbr') values[i] = dynamicFormRef.formModel[i].split('\n');
        else {
             if(i == 'Check_type'){
               ifprod=values[i];
             }
              values[i] = dynamicFormRef.formModel[i];
        }
    }

    if(ifprod=="NO"){
        PageSetting.Columns = [
            {
                title: '站點',
                align: 'center',
                dataIndex: 'WORK_CTR',  
                className:'v-center',
                width:25,  
                fixed:'left',                 
            },
            ...PanelWipColumns,
        ];

    }else{

       PageSetting.Columns = [
             {
                title: '站點',
                align: 'center',
                dataIndex: 'WORK_CTR',  
                className:'v-center',
                width:25, 
                fixed:'left',
                customCell: (record: Object, rowindex: number, column: Object) => { 
                    let text = record['WORK_CTR']
                                    let obj = {
                                        text,
                                        style: {},
                                        onDblclick: () => {
                                        message.warning('此栏位没有二阶数据')
                                    }
                                 }
                                    obj['rowSpan'] = record?.[`${'WORK_CTR'}-span`]?.['rowspan'] || 0
                                    return obj
                 }                  
            },
             {
                title: 'Prod Nbr',
                align: 'center',
                dataIndex: 'PROD_NBR', 
                className:'v-center',
                width:35, 
                fixed:'left',              
            },
            ...PanelWipColumns,
        ];

    }

  
    dynamicFormRef.validate().then(async () => {
      try{
          const { Date: { Data: { Table: Data },
                    cuttime,
                    SQL01,
                    sql_Where } }  = await GetCommon({ ...baseParams, ...values });
          
          //console.log("WipPanelSumDataSet-SQLHidden:",SQL01);

          PageSetting.SQLHidden=ApiNamePara+"-SQLHidden:"+SQL01;
          
          let Message = {                
                sql_Where: sql_Where,
            };


           PageSetting.Title=`${useCommonParams.Fab} Panel Wip Stay  ( 更新時間:${cuttime.slice(0,19)})`
          /**将API回传参数存进共用的vuex中*/
            ReportItem.APIMessage = JSON.stringify(Message);  
            ReportItem.FormParams={ ...baseParams,...values }
            /**将表单参数存进共用的vuex中*/
            
          
            if (Data.length != 0) { 
                
                if(ifprod!="NO"){
                        let temp_date=getSubData(Data);
                        
                        let combineCol1: any = {
                        data: temp_date,
                        mergeColNames: ['WORK_CTR','PROD_NBR'],
                        //firstMergeColNames: ['FLOOR'],
                        firstMerge: ['WORK_CTR']
                        };

                        /**进行表格合并 */
                        PageSetting.moreData=mergeTableRow(combineCol1)
                                       

                }else
                {
                    /**获取小计或总计的值*/
                   PageSetting.moreData=getSubData(Data) 
                }
                //PageSetting.List = PageSetting.moreData

                PageSetting.x = null;
                if (PageSetting.Columns.length > 7) {
                    PageSetting.x = 100 * PageSetting.Columns.length;
                }

                if (PageSetting.moreData.length > 1000) {
                    PageSetting.List = PageSetting.moreData.slice(0, 100)
                    // isLongData.value = true
                }//预先加载100条数据
                else {
                    PageSetting.List = PageSetting.moreData
                    // isLongData.value = false
                }
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




function getSubData(Data: any[]) {
    let pushdata = cloneDeep(Data);    
    let BANK = pushdata.filter((item) => parseInt(item.WORK_CTR) <= 1000);
    let count = BANK.length;
    let BANK_Sub = getSumData(BANK.length, 0, Data, PanelWipProps);
    let WIPSUM_Sub = getSumData(Data.length, BANK.length, Data, PanelWipProps);
    //let ALLSUM_Sub = getSumData(Data.length, 0, Data, PanelWipProps);
    pushdata.splice(count, 0, Object.assign(BANK_Sub, { WORK_CTR: 'BANK_SUM' }));
    pushdata.splice(pushdata.length, 0, Object.assign(WIPSUM_Sub, { WORK_CTR: 'WIP_SUM' }));
    //pushdata.splice(pushdata.length, 0, Object.assign(ALLSUM_Sub, { WORK_CTR: 'SUM' }));
    let temppushdata = cloneDeep(pushdata);
    let WIP_RATE=temppushdata.filter((item, i, arr) => {
      return item.WORK_CTR === "WIP_SUM"
    });

    let WIP_RATE_SUM = cloneDeep(WIP_RATE);
    
    WIP_RATE.forEach((item) => {
       for (let i = 1; i <= 13; i++){
         let colname="QTY"+i
         item[colname] = handfloatprops(item, colname, 'TOTAL_QTY');
       }
       item["TOTAL_QTY"]= handfloatprops(item, 'TOTAL_QTY', 'TOTAL_QTY');
       item["QTY15"]= "";
       item["WORK_CTR"]="當站WIP比率"
    });

   WIP_RATE_SUM.forEach((item) => {
       let SUMQTY=0;
       for (let i = 1; i <= 13; i++){        
         let colname="QTY"+i
         SUMQTY=SUMQTY+item[colname];
         item[colname] = SUMQTY/item["TOTAL_QTY"]== Infinity
              ? 0 :((SUMQTY/item["TOTAL_QTY"])* 100).toFixed(2)+"%";
       }
       item["TOTAL_QTY"]= "";
       item["QTY15"]= "";
       item["WORK_CTR"]="累計WIP比率"
    });

    
    pushdata.splice(pushdata.length, 0,WIP_RATE[0] );
    pushdata.splice(pushdata.length, 0,WIP_RATE_SUM[0] );
    return pushdata;
}
// let onMoreDataLoad = computed(() => {
//     if (PageSetting.moreData.length > 0) return PageSetting.moreData
//     else return []
// })
// const { list, containerProps, wrapperProps } = useVirtualList(
//     onMoreDataLoad,
//     {
//         itemHeight: 5,
//     },
// )
// watch(() => list.value,
//     () => {  
          
//         let pushData: any[] = []
//         PageSetting.List = []
//         list.value.forEach(item => {
            
//             if (item.index < PageSetting.moreData.length) {
//                 pushData.push(item.data)
//             }
//         })
        
//         PageSetting.List = pushData
//     })

InfiniteScroll(PanelWipStayNewTable, PageSetting)
onMounted(async () => {
    // if (bigfab==="LCM"){
    //     await mounted(optionsdata,useCommonParams.Fab)
    //     await mountedLCMowner(optionsdata, useCommonParams.Fab); 
    //     await mountedModuleData(optionsdata, useCommonParams.Fab);
    //     await mountedOper(optionsdata, useCommonParams.Fab);
    // }else{
        
    // }       
});


function getRowClassName_temp(record: Object) {
    if (record && Object.keys(record).length != 0) {
        for (const [key, value] of Object.entries(record)) {
            if (['小计', 'BANK_SUM', 'WIP_SUM','小計','合 計'].includes(value)) {
                return 'Yellow';
            } else if (['總計', 'SUM', '當站WIP比率', '累計WIP比率', 'Sum', '合计'].includes(value)) {
                return 'Green';
            }
        }
    }
}


</script>

<style lang="less" scoped>
.PanelWipStayNew :deep(.v-center) {
    background-color: #f0f0f0;
}

.PanelWipStayNew :deep(.Yellow) {
    background-color: #80ff80;
}

.PanelWipStayNew :deep(.YellowGreen) {
    background-color: #f39230;
}
</style>
