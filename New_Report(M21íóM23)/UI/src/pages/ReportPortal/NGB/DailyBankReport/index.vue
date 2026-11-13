<template>
    <div class="DailyBank">
        <a-card>
            <!-- <a-row>
                <span class="menu-fold" @click="showHidden(PageSetting)">
                    <component :is="!PageSetting.showmenu ? MenuUnfoldOutlined : MenuFoldOutlined" />
                </span>
            </a-row> -->
            <a-row>
                <a-col
                    :span="24 - PageParams.TableSpan"
                    Source-show="PageSetting.showmenu"
                >
                    <div class="NGBScrollForm">
                        <SchemaForm @submit="confirm"> </SchemaForm>
                    </div>
                </a-col>
                <a-col :span="PageParams.TableSpan">                  
                    <div class="ScrollTable" ref="DailyBankTable">
                        <DynamicTable
                            showFavorite sticky
                            EditExplain
                            row-key="id"
                            :header-title="PageSetting.HeadTitle"
                            :columns="PageSetting.Columns"
                            :key="PageSetting.Columns"
                            bordered
                            size="small"
                            :data-source="PageSetting.List"
                            :data-all="PageSetting.moreData"
                            :pagination="false"
                            :scroll="{ x: PageSetting.x,}"
                            :PageParams="PageParams"
                            export-file-name="导出"
                            :row-class-name="getRowClassName"                  
                        >
                            
                           <!-- <template #toolbar>
                                <a-button type="primary" @click="isShowExplain = !isShowExplain">{{isShowExplain?'显示表格':'显示说明文件'}}</a-button>
                            </template>   -->
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
import { getPovitData, PovitParams } from '@/utils/helper/povitHelper'
import { showHidden, getRowClassName,getSumData,mergeTableRow,getdiffdate, } from '@/pages/ReportPortal/utils/NGBcommon'
import { AddCommonPageParams } from '@/store/module/commonpage';
import { Console } from 'console';
import { message } from 'ant-design-vue';


let useCommonParams = AddCommonPageParams();
let bigfab=useCommonParams.Fab.slice(0,useCommonParams.Fab.length-1)
const DailyBankTable = ref<HTMLElement | null>(null)
//vue3的mixin
const {  InfiniteScroll } = useRemoteSelect()
function getImport(site: any) {
    //let { optionsdata } = require('@/pages/ReportPortal/Static/NGBCommon-Form-schema');
    const { schemas } = require(`@/pages/ReportPortal/NGB/DailyBankReport/${bigfab}/form-schema`);
    return { schemas };
}

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});

let isShowExplain = ref<boolean>(false);


let { schemas} = getImport(useCommonParams.Fab);

let PageSetting = reactive({
    List:[] as any,
    TableSpan: 20,
    showmenu: true,
    APIColumns: [] as any,
    Columns: [] as any,
    moreData: [] as any,
    SQLHidden:'',
    HeadTitle:'',
    x: '' as any,
});
// let isLongData = ref(true)
PageSetting.HeadTitle=`${useCommonParams.Fab} Daily Bank Report`
const [DynamicTable] = useTable();

const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 130,
    schemas:schemas(useCommonParams.Fab,reactive({})),
    actionColOptions: {
        span: 18
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
    
    //基礎query參數條件值
    let baseParams = {name: "Date",apiName: "DailyBankDataSet",FactoryType:useCommonParams.Fab,G_FAC:useCommonParams.G_FAC_COM,building:"",}

   //獲取query頁面的條件值
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
            values[i] = dynamicFormRef.formModel[i];
        }
    }
    
    //console.log(dynamicFormRef.formMode
   
   //獲取數據綁定table
    dynamicFormRef.validate().then(async () => {

       try{
          const { Date: { Data: { Table: Data },sql } }  = await GetCommon({ ...baseParams, ...values });

          PageSetting.SQLHidden = "DailyBankDataSet-SQLHidden:"+sql
         

          if (Data.length != 0) { 

                //循環查詢data，獲取table Colums 數據
                let ColumsData: any = [];
                let WidthPars=50;
                let titlePars="";//label
                let classNamePars="v-center";//API提供轉置后的時間處理（例如：A2022_11_30）
                let fixedPara="left";

                for (let item in Data[0]) { 
                   if(item=="PROD_NBR"){
                        WidthPars=110;
                        titlePars="LCM機種";                
                    }else if(item=="B1_PROD_NBR"){
                        WidthPars=110;
                        titlePars="BL 料號";                
                    } else if(item=="ATTRS"){
                        WidthPars=60;
                        titlePars="屬性" ;               
                    }else {
                        WidthPars=70;
                        if(item.slice(0,1)=="A"){
                        titlePars=item.slice(1).replaceAll('_', '-');
                        fixedPara=""
                        }else{
                        titlePars=item;
                        fixedPara="left"
                        } 
                        classNamePars="";

                    }

                    if(item!="PROD_NBR"){
                      ColumsData.push({ title: titlePars, align: 'center', dataIndex: item, className: classNamePars, width:WidthPars, fixed:fixedPara, 
                      });
                    }
                    
                }

                PageSetting.Columns = [               
                    {
                        title: 'LCM機種',
                        align: 'center',
                        dataIndex: 'PROD_NBR',  
                        className:'v-center',
                        width:110,
                        fixed:"left",
                        customCell: (record: Object, rowindex: number, column: Object) => { 
                            let text = record['PROD_NBR']
                                            let obj = {
                                                text,
                                                style: {},
                                            
                                        }
                                            obj['rowSpan'] = record?.[`${'PROD_NBR'}-span`]?.['rowspan'] || 0
                                            return obj
                        }                  
                    },
                   ...ColumsData,
                    
                ];
                       
                PageSetting.moreData=getTableData(Data);
                

                  PageSetting.x = null;
                if (PageSetting.Columns.length > 7) {
                    PageSetting.x = 110 * PageSetting.Columns.length;
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
           
            //加載防護罩結束
            closeFullLoading();

       }catch(error: any){ 
           closeFullLoading();
           return Promise.reject(error); 
        }

    
    });
}

function getTableData(Data: any[]) {

    /**表格合并所需参数
     * @data 数据源
     * @mergeColNames 需要合并的列
     * @firstMergeColNames 受到影响的列
     * @firstMerge 以哪列为基础进行合并
     */
    let combineCol1: any = {
                    data: Data,
                    mergeColNames: ['PROD_NBR'],
                    firstMerge: 'PROD_NBR'
                };
    /**进行表格合并 */
    let Mergedata = mergeTableRow(combineCol1);    
    return Mergedata;
}

InfiniteScroll(DailyBankTable, PageSetting)

//載入api 方法
onMounted(async () => {
    //optionsdata.OwnerName = await Getcommon(params) 
    // await mounted(optionsdata,useCommonParams.Fab)
    // await mountedLCMowner(optionsdata, useCommonParams.Fab);        
    
})

</script>
<!-- <script lang="tsx">
export default{
    name:'daylimove'
}
</script> -->
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
