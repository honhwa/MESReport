<template>
    <div class="WorkOrderQuery">
        <a-card>
            <!--<a-row>
                <span class="menu-fold" @click="showHidden(PageSetting)">
                    <component :is="!PageParams.ShowMenu ? MenuUnfoldOutlined : MenuFoldOutlined" />
                </span>
            </a-row>-->
            <a-row>
                <a-col
                    :span="24 - PageParams.TableSpan"
                    Source-show="PageParams.ShowMenu"
                >
                    <div class="NGBScrollForm">
                        <SchemaForm @submit="confirm"> </SchemaForm>
                    </div>
                </a-col>
                <a-col :span="PageParams.TableSpan">                  
                    <div class="ScrollTable" ref="WorkOrderTable">
                        <DynamicTable
                            row-key="id" sticky
                            :header-title="PageSetting.Title"
                            :columns="PageSetting.Columns"
                            :key="PageSetting.Columns"
                            bordered
                            size="small"   :PageParams="PageParams"
                            :data-source="PageSetting.List"
                            :data-all="PageSetting.moreData"
                            :pagination="false"
                            :scroll="{ x: PageSetting.x,}"
                            export-file-name="导出"
                           
                        >
                            <template #export-button>
                                <a-button type="success"> 导出 </a-button>
                            </template>
                        </DynamicTable>
                    </div>
                </a-col>
                <div hidden>{{ PageSetting.SQLHidden }}</div>
            </a-row>
        </a-card>
    </div>
</template>


<script lang="tsx" setup>
import { MenuFoldOutlined, MenuUnfoldOutlined } from '@ant-design/icons-vue';
import {PageParams} from '@/components/core/dynamic-table/src/dynamic-table'
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, unref, ref, onMounted, watch, computed, Ref } from 'vue';
import { GetCommon } from '@/api/pages/common';
import useRemoteSelect from '@/pages/ReportPortal/utils/NGBuseRemoteSelect';
import { useLoading } from '@/components/feat/Loading';
import {cloneDeep} from 'lodash-es';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { Console } from 'console';
import { message } from 'ant-design-vue';
import type { TableColumn } from '@/components/core/dynamic-table';

let useCommonParams = AddCommonPageParams();
let bigfab=useCommonParams.Fab.slice(0,useCommonParams.Fab.length-1)
const WorkOrderTable = ref<HTMLElement | null>(null)
//vue3的mixin
const { mounted,getCommonFormParams,mountedLCMowner,InfiniteScroll, mountedOper,} = useRemoteSelect();

function getImport(site: any) {
    //let { optionsdata } = require('@/pages/ReportPortal/Static/NGBCommon-Form-schema');
    const { schemas ,GrpOptions} = require(`@/pages/ReportPortal/NGB/WorkOrderQuery/${bigfab}/form-schema`);
    return { schemas,   GrpOptions};
}

let { schemas,   GrpOptions} = getImport(useCommonParams.Fab);

let PageSetting = reactive({
    List:[] as any,
    TableSpan: 20,
    showmenu: true,
    APIColumns: [] as any,
    Columns: [] as any,
    moreData: [] as any,
    SQLHidden:'',
    Title:'',
    //scroll:{}as any,
     x: '' as any,
});


const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
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

    let dynamicColumns: TableColumn[] = [];
    let baseParams = {name: "Date",
        apiName: 'WoNbrDataSet',
        FactoryType: useCommonParams.Fab,
        G_FAC: useCommonParams.G_FAC_COM,
    };

    if (values.Prod_nbr && values.Prod_nbr.length > 1000 )
    {
         closeFullLoading();
         return message.error('料號输入超過 “最大限制筆數:1000” '); 
    }
    
    if (values.Colname && values.Colname.length > 0) {
        if (values.Colname.length > 8 ){
           PageSetting.x = 120 * values.Colname.length;
        }

        values.Colname.forEach((item) => {
            let itemsplit = item.split('.')[1] ?? item.split('.')[0];
            let Data = GrpOptions.filter((Prop) => Prop.value == item);
            //console.log("Data",Data)

            if(Data[0].label!="ALL"){
              {
                let WidthPars=50;
                if(itemsplit.toUpperCase()=="PROD_NBR"||itemsplit.toUpperCase()=="WO_NBR"||itemsplit.toUpperCase()=="LCD_OWNER")
                {
                   WidthPars=85;
                }else if(itemsplit.toUpperCase()=="LCD_PRODUCT_ID")
                {
                   WidthPars=150;
                }
                else if(itemsplit.toUpperCase()=="DUE_DT"||itemsplit.toUpperCase()=="RELEASE_DT"||itemsplit.toUpperCase()=="PLAN_ST_DT"
                ||itemsplit.toUpperCase()=="LCD_GRADE_GRP"||itemsplit.toUpperCase()=="GRADE_MAX_QTY"||itemsplit.toUpperCase()=="GRADE_ACTUAL_QTY"){
                   WidthPars=65;
                }else if(itemsplit.toUpperCase()=="VER"||itemsplit.toUpperCase()=="OPER"||itemsplit.toUpperCase()=="TYPES"||itemsplit.toUpperCase()=="FLOOR"){
                     WidthPars=40;
                }                
                else {
                   WidthPars=50;
                }

               if(Data[0].label!="DIFF"){

                  if(itemsplit.toUpperCase()!="WO_NBR"){
                     dynamicColumns.push({
                        title: Data[0].label,
                        dataIndex: itemsplit.toUpperCase(),
                        align: 'center',
                        width:WidthPars,
                        
                    });
                  }else{
                     dynamicColumns.push({
                        title: Data[0].label,
                        dataIndex: itemsplit.toUpperCase(),
                        align: 'center',
                        width:WidthPars,
                        fixed:"left",
                        className:"v-center",
                    });
                  }
                    

               }else{
                    dynamicColumns.push({
                        title: Data[0].label,
                        dataIndex: "DIFF",
                        align: 'center',
                        width:40,
                    });
               }
                
              }


           }
            
        });
    } else {
         closeFullLoading();
         return message.error('请输入显示栏位');
    } 

    PageSetting.Columns = dynamicColumns;
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
                    SQL01 } }  = await GetCommon({ ...baseParams, ...values });

          PageSetting.SQLHidden = SQL01
          //console.log("WoNbrDataSet-SQLHidden:",SQL01)
          PageSetting.SQLHidden="WoNbrDataSet-SQLHidden:"+SQL01;
          
          PageSetting.Title=`${useCommonParams.Fab} 工單查詢`
          PageSetting.moreData=[];

          if (Data.length != 0) {
            
            PageSetting.moreData = Data;
            
            if (PageSetting.moreData.length > 1000) {
                PageSetting.List = PageSetting.moreData.slice(0, 200)
            
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

InfiniteScroll(WorkOrderTable, PageSetting)

//載入api 方法
onMounted(async () => {
    // await mounted(optionsdata,useCommonParams.Fab)  
    // await mountedOper(optionsdata, useCommonParams.Fab);
    // await mountedLCMowner(optionsdata, useCommonParams.Fab); 
    
})

</script>

<style lang="less" scoped>
.btn-rows {
    button {
        margin-right: 12px;
    }
}

.WorkOrderTable {
    height: 700px;
    overflow-y: auto;
}

.swapper {
    display: flex;
    flex-direction: column;
}
</style>
