<template>
    <div class="WipPanelSummary">
        <a-card>
            <!-- <a-row>
                <span class="menu-fold" @click="showHidden">
                    <component :is="!PageSetting.showmenu ? MenuUnfoldOutlined : MenuFoldOutlined" />
                </span>
            </a-row> -->
            <a-row>
                <a-col :span="24 - PageSetting.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                    <SchemaForm @submit="confirm"> </SchemaForm>
                </a-col>
                <a-col :span="PageSetting.TableSpan">
                    <div class="TNScrollForm" ref="WipPanelSummary">
                        <DynamicTable
                            showFavorite
                            row-key="WipPanelSummary"
                            :header-title=PageSetting.HeadTitle
                            :columns="PageSetting.Columns"
                            :key="PageSetting.Columns"
                            bordered
                            size="small"
                            :data-source="PageSetting.List"
                            :data-all="PageSetting.moreData"
                            :pagination="false" 
                            :PageParams="PageParams"
                            :row-class-name="getRowClassName"   
                            :scroll="{x:2500}"
                            sticky                     
                            export-file-name="Export"
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

<script lang="ts" setup>
import { MenuFoldOutlined, MenuUnfoldOutlined } from '@ant-design/icons-vue';
import { showHidden } from '@/pages/ReportPortal/utils/common';
// import { schemas } from './form-schema';
import {PageParams} from '@/components/core/dynamic-table/src/dynamic-table'
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, onMounted, ref } from 'vue';
import { cloneDeep } from 'lodash-es';
import useRemoteSelect from '@/pages/ReportPortal/utils/TNuseRemoteSelect';
import {getSumData, getRowClassName} from '@/pages/ReportPortal/utils/TNcommon';
import { message } from 'ant-design-vue';
import type { TableColumn } from '@/components/core/dynamic-table';
import { GetCommon } from '@/api/pages/common';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { ReportCommonWithOut } from '@/store/module/ReportCommon';
import { useRouter, useRoute } from 'vue-router';
import { Console } from 'console';
import { useLoading } from '@/components/feat/Loading';
//import {optionsdata} from '@/pages/ReportPortal/Static/TNCommon-Form-schema';
// import { useInfiniteScroll, useVirtualList } from '@vueuse/core'

let isShowExplain = ref<boolean>(false);
const router = useRouter();
const route = useRoute();
const { mounted,getCommonFormParams,InfiniteScroll } = useRemoteSelect();
const useCommonParams = AddCommonPageParams();
const useReportCommon = ReportCommonWithOut();
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
let ReportNameParams = `WipPanelSummaryDetail_${useCommonParams.Fab}`;

let fab = "MOD";
if(useCommonParams.Fab == "MOD1" || useCommonParams.Fab == "MOD3" || useCommonParams.Fab == "N9")
{
    fab = useCommonParams.Fab;
}

const WipPanelSummary = ref<HTMLElement | null>(null);
function getImport(site: any, fab: any) {
    //let { optionsdata } = require('@/pages/ReportPortal/Static/TNCommon-Form-schema');
    const { schemas, GrpOptions } = require(`@/pages/ReportPortal/${site}/WipPanelSummary/${fab}/form-schema`);
    return { schemas, GrpOptions };
}
let { schemas, GrpOptions } = getImport("TN",fab);
debugger
const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});

let PageSetting = reactive({
    List: [] as any,
    TableSpan: 20,
    showmenu: true,
    Columns: [] as any,
    moreData: [] as any,
    Title:'',
    SQLHidden:'',
    APIColumns: [] as any,
    HeadTitle:'',
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
const customCellRender = (record: Object, rowIndex: number, column: Object) => {
    return {
        ondblclick: () => {
            //useCommonParams.AddWipPanelSummaryFirstTableParams({ ...record, ...column['__originColumn__'] });
            useReportCommon.AddFirstTableParams(`WipPanelSummaryDetail_${useCommonParams.Fab}`,{ ...record, ...column['__originColumn__'] })
            router.push({ name: `WipPanelSummaryDetail_${useCommonParams.Fab}`});
        },
        style: {
            cursor: 'pointer'
        }
    };
};


async function confirm(values) {
    openFullLoading();
    let dynamicColumns: TableColumn[] = [];
    let baseParams = {name: "Date",
        apiName: 'TN_WipPanelSumDataSet',
        FactoryType: useCommonParams.Fab,
        G_FAC: useCommonParams.G_FAC_COM,
    };
    
    if (values.Colname && values.Colname.length > 0) {
        values.Colname.forEach((item) => {
            let itemsplit = item.split('.')[1] ?? item.split('.')[0];
            let Data = GrpOptions.filter((Prop) => Prop.value == item);
            if(Data[0].label!="ALL"){
              {
                let WidthPars=50;
                if(itemsplit.toUpperCase()=="PROD_NBR"||itemsplit.toUpperCase()=="WO_NBR"||itemsplit.toUpperCase()=="LCD_PROD_NBR")
                {
                   WidthPars=100;
                }else if(itemsplit.toUpperCase()=="DUE_DT"){
                   WidthPars=80;
                }
                else {
                   WidthPars=45;
                }

                dynamicColumns.push({
                    title: Data[0].label,
                    dataIndex: itemsplit.toUpperCase(),
                    align: 'center',
                    width:WidthPars,
                    customCell: customCellRender
                });
              }

           }
            
        });
    } else {
         closeFullLoading();
         return message.error('請輸入顯示欄位');
    } 
    
    let tempCol=dynamicColumns[0]["dataIndex"];
    let obj = {}
    obj[tempCol] = '合計'

    //console.log("dynamicColumns",dynamicColumns[0]["dataIndex"])
    //console.log("dynamicColumns",dynamicColumns)
    PageSetting.Columns = [
        ...dynamicColumns,
        {
            title: 'SUB Qty',
            align: 'center',
            dataIndex: 'SUB',
            width:60,
            customCell: customCellRender
            // customRender: ({ record }) => {
            //     return <span>{getAllSumData(record)}</span>;
            // }
        }
    ];
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
                    SQL01,
                    SQLFilter 
                } }  = await GetCommon({ ...baseParams, ...values });

          PageSetting.SQLHidden = SQL01
          console.log("WipPanelSumDataSet-SQLHidden:",SQL01)
          let Message = {                
                SQLFilter: SQLFilter,
            };
          
          
          PageSetting.Title=`${useCommonParams.Fab} Wip Panel Summary`
          PageSetting.moreData=[];

          if (Data.length != 0) {
            let pushdata = cloneDeep(Data);
            let SUM_SUB = getSumData(Data.length, 0, Data, ["SUB"]);

            pushdata.splice(Data.length+1, 0, Object.assign(SUM_SUB, obj));
            console.log("tempObj",tempCol)
            

            PageSetting.moreData = pushdata;
            
            if (PageSetting.moreData.length > 100) {
                PageSetting.List = PageSetting.moreData.slice(0, 20)
            
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

          ReportParamsItem.APIMessage = JSON.stringify(Message);
          ReportParamsItem.FormParams = { ...baseParams, ...values };
          useReportCommon.AddObject(ReportNameParams , ReportParamsItem)
        //   useCommonParams.AddWipPanelSummaryAPIResultMessage(JSON.stringify(Message));
        //   useCommonParams.AddWipPanelSummaryFormParams({ ...baseParams, ...values });

      }catch(error: any){ 
           closeFullLoading();
           return Promise.reject(error); 
        }
        
    })

}
//实现无限滚动或者说懒加载
InfiniteScroll(WipPanelSummary, PageSetting);

onMounted(async () => {
    //await mounted(optionsdata,useCommonParams.Fab)   
});
</script>

<style lang="less" scoped>
.btn-rows {
    button {
        margin-right: 12px;
    }
}
:deep(.pro_auto-center){
    border: 1px solid black;
}

.WipPanelSummary :deep(.v-center) {
    background-color: #f0f0f0;
    border: 1px solid black;
}

.WipPanelSummary :deep(.Yellow) {
    background-color: #80ff80;
}

.WipPanelSummary :deep(.YellowGreen) {
    background-color: #f39230;
}


.swapper {
    display: flex;
    flex-direction: column;
}
.newbutton {
    float: right;
    margin-left: 7px;
}
</style>

