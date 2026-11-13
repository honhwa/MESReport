<template>
    <div class="PanelWipStay">
        <a-card>
            <!-- <a-row>
                <span class="menu-fold" @click="showHidden(PageSetting)">
                    <component :is="!PageSetting.showmenu ? MenuUnfoldOutlined : MenuFoldOutlined" />
                </span>
            </a-row> -->
            <a-row>
                <a-col :span="24 - PageSetting.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                    <SchemaForm @submit="confirm"> </SchemaForm>
                </a-col>
                <a-col :span="PageSetting.TableSpan">
                  <div class="TNScrollForm" ref="PanelWipStayTable">
                    <DynamicTable
                        showFavorite
                        row-key="id"
                        :header-title=PageSetting.HeadTitle
                        :columns="PageSetting.Columns"
                        :key="PageSetting.Columns"
                        bordered
                        size="small"
                        :data-source="PageSetting.List"
                        :pagination="false"
                        export-file-name="Export"
                        :data-all="PageSetting.moreData"
                        :row-class-name="getRowClassName_temp"
                         :explain="isShowExplain"  
                        sticky
                        :PageParams="PageParams"
                    >
                
                        <template #export-button>
                            <a-button type="success"> Export </a-button>
                        </template>
                    </DynamicTable>
                  </div>
                </a-col>
            </a-row>
        </a-card>
    </div>
</template>


<script lang="tsx" setup>
import { MenuFoldOutlined, MenuUnfoldOutlined } from '@ant-design/icons-vue';
// import { schemas } from './form-schema';
import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table';
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, unref, ref, onMounted, watch, computed, Ref } from 'vue';
import { GetCommon } from '@/api/pages/common';
import { cloneDeep } from 'lodash-es';
import useRemoteSelect from '@/pages/ReportPortal/utils/TNuseRemoteSelect';
import { showHidden, getSumData ,handfloatprops,mergeTableRow} from '@/pages/ReportPortal/utils/TNcommon';
import { message } from 'ant-design-vue';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { ReportCommon } from '@/store/module/ReportCommon';
import { useLoading } from '@/components/feat/Loading';
//import {optionsdata} from '@/pages/ReportPortal/Static/TNCommon-Form-schema';

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});

let isShowExplain = ref<boolean>(false);
const useCommonParams = AddCommonPageParams();
const useReportCommon = ReportCommon();
let ReportParamsItem = reactive({
    APIMessage:'',
    FormParams: {},
    FirstTableParams: {},
    SecondTableParams:reactive({}),
    ThirdTableParams:{},
    ThirdTableClickParams:{},
    Fab:'',
    G_FAC_COM:'',
});
let ReportNameParams = `PanelWipStayDetail_${useCommonParams.Fab}`;

let fab = "MOD";
if(useCommonParams.Fab == "MOD1")
{
    fab = useCommonParams.Fab;
}

const PanelWipStayTable = ref<HTMLElement | null>(null)
function getImport(site: any) {

    //const { optionsdata } = require('@/pages/ReportPortal/Static/TNCommon-Form-schema');
    const { schemas } = require(`@/pages/ReportPortal/${site}/PanelWipStay/${fab}/form-schema`);
    const { PanelWipColumns, TableColumnItem, PanelWipLabel,PanelWipProps } = require(`@/pages/ReportPortal/${site}/PanelWipStay/MOD/Table`);

    return { schemas, PanelWipColumns, TableColumnItem, PanelWipLabel,PanelWipProps};
}

let { schemas, PanelWipColumns, PanelWipLabel,PanelWipProps } = getImport('TN');
let { mounted,getCommonFormParams,InfiniteScroll, } = useRemoteSelect();

let PageSetting = reactive({
    List: [] as any,
    TableSpan: 20,
    showmenu: true,
    APIColumns: [] as any,
    Columns: [] as any,
    HeadTitle:'',
    Title:"",
    moreData: [] as any,
});
let FactoryType: any = useCommonParams.Fab;
const [DynamicTable] = useTable();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas: schemas(FactoryType),
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
});

async function confirm(values) {
   debugger
      //table 初始化
    PageSetting.List=[];
    PageSetting.Columns=[];
    openFullLoading();
    let baseParams = { name: "Date",apiName: 'TN_PanelWipStayDataSet', FactoryType: useCommonParams.Fab, G_FAC:useCommonParams.G_FAC_COM, };
    let ifprod="NO";
    
    /**values为所设定的所有参数,将values里为undefined的值赋为空*/
    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        }
    });

    for (let i in dynamicFormRef?.formModel) {
        if (i == 'Wo_nbr') values[i] = dynamicFormRef.formModel[i].split('\n');
        else {
             if(i == 'ReportType'){
               ifprod=values[i];
             }
              values[i] = dynamicFormRef.formModel[i];
        }
    }

    if(ifprod=="ALL"){
        PageSetting.Columns = [
            {
                title: '站點',
                align: 'center',
                dataIndex: 'WORK_CTR',  
                className:'v-center',                   
            },
            ...PanelWipColumns,
        ];

    }else if(ifprod=="Oper"){

       PageSetting.Columns = [
             {
                title: '站點',
                align: 'center',
                dataIndex: 'WORK_CTR',  
                className:'v-center',
                customCell: (record: Object, rowindex: number, column: Object) => { 
                    let text = record['WORK_CTR']
                                    let obj = {
                                        text,
                                        style: {},
                                        onDblclick: () => {
                                        message.warning('此欄位沒有二階數據')
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
                className:'v-center'               
            },
            ...PanelWipColumns,
        ];

    }else{

    PageSetting.Columns = [
        {
            title: 'Product',
            align: 'center',
            dataIndex: 'PROD_NBR', 
            className:'v-center',
            customCell: (record: Object, rowindex: number, column: Object) => { 
                    let text = record['PROD_NBR']
                                    let obj = {
                                        text,
                                        style: {},
                                        onDblclick: () => {
                                        message.warning('此欄位沒有二階數據')
                                    }
                                 }
                                    obj['rowSpan'] = record?.[`${'PROD_NBR'}-span`]?.['rowspan'] || 0
                                    return obj
                 }                       
        },
        {
            title: '站點',
            align: 'center',
            dataIndex: 'WORK_CTR',  
            className:'v-center'               
        },
        ...PanelWipColumns,
    ];

    }

  
    dynamicFormRef.validate().then(async () => {
      try{
          const { Date: { Data: { Table: Data },
                    cuttime,
                    SQL01,
                    SQLFilter } }  = await GetCommon({ ...baseParams, ...values });
          
          console.log("WipPanelSumDataSet-SQLHidden:",SQL01);

          console.log("WipPanelSumDataSet-SQLHidden:",SQLFilter);
          
          let Message = {                
                sql_Where: SQLFilter,
            };


           PageSetting.Title=`${useCommonParams.Fab} Panel Wip Stay`
          /**将API回传参数存进共用的vuex中*/
            //useCommonParams.AddPanelWIPStayAPIResultMessage(JSON.stringify(Message));
            /**将表单参数存进共用的vuex中*/
            //useCommonParams.AddPanelWIPStayFormParams({ ...baseParams, ...values });  

            ReportParamsItem.APIMessage = JSON.stringify(Message);
            ReportParamsItem.FormParams = { ...baseParams, ...values };
            useReportCommon.AddObject(ReportNameParams , ReportParamsItem)

            if (Data.length != 0) { 
           
                if(ifprod=="Oper"){
                        let temp_date=Data;
                        
                        let combineCol1: any = {
                        data: temp_date,
                        mergeColNames: ['WORK_CTR','PROD_NBR'],
                        //firstMergeColNames: ['FLOOR'],
                        firstMerge: ['WORK_CTR']
                        };

                        /**进行表格合并 */
                        PageSetting.moreData=mergeTableRow(combineCol1);                

                }else if(ifprod=="Prod"){
                        let temp_date=Data;
                        
                        let combineCol1: any = {
                        data: temp_date,
                        mergeColNames: ['PROD_NBR','WORK_CTR'],
                        //firstMergeColNames: ['FLOOR'],
                        firstMerge: ['PROD_NBR']
                        };

                        /**进行表格合并 */
                        PageSetting.moreData=mergeTableRow(combineCol1);                

                }else
                {
                    /**获取小计或总计的值*/
                   PageSetting.moreData=Data; 
                }

                if (PageSetting.moreData.length > 100) {
                    PageSetting.List = PageSetting.moreData.slice(0, 40)
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
            
          }catch(error: any){ 
           closeFullLoading();
           return Promise.reject(error); 
        }
        


   })
}

function getSubData(Data: any[]) {
    let pushdata = cloneDeep(Data);    
    let BANK = pushdata.filter((item) => parseInt(item.WORK_CTR) <= 1000);
    let count = BANK.length;
    let BANK_Sub = getSumData(BANK.length, 0, Data, PanelWipProps);
    let WIPSUM_Sub = getSumData(Data.length, BANK.length, Data, PanelWipProps);
    let TOTALSUM_Sub = getSumData(Data.length, 0, Data, PanelWipProps);
    //let ALLSUM_Sub = getSumData(Data.length, 0, Data, PanelWipProps);
    pushdata.splice(count, 0, Object.assign(BANK_Sub, { WORK_CTR: 'BANK_SUM' }));

    pushdata.splice(pushdata.length, 0, Object.assign(WIPSUM_Sub, { WORK_CTR: 'WIP_SUM' }));
    pushdata.splice(pushdata.length, 0, Object.assign(TOTALSUM_Sub, { WORK_CTR: '總計' }));
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

InfiniteScroll(PanelWipStayTable, PageSetting)
onMounted(async () => {
    //await mounted(optionsdata,useCommonParams.Fab)
          
});


function getRowClassName_temp(record: Object) {
    if (record && Object.keys(record).length != 0) {
        for (const [key, value] of Object.entries(record)) {
            if (['小计', 'BANK_SUM', 'WIP_SUM','小計','合 計'].includes(value)) {
                return 'Yellow';
            } else if (['總計', 'SUM', '當站WIP比率', '累計WIP比率', 'Sum', '合计'].includes(value)) {
                return 'YellowGreen';
            }
        }
    }
}


</script>

<style lang="less" scoped>
:deep(.pro_auto-center){
    border: 1px solid black;
}

.PanelWipStay :deep(.v-center) {
    background-color: #f0f0f0;
    border: 1px solid black;
}

.PanelWipStay :deep(.Yellow) {
    background-color: #80ff80;
    border: 1px solid black;
}

.PanelWipStay :deep(.YellowGreen) {
    background-color: #f39230;
    border: 1px solid black;
}

</style>
