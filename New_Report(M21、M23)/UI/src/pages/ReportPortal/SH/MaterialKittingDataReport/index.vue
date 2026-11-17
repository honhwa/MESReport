<template>
    <div class="MaterialKittingData">
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
                    <div class="SHScrollForm">
                        <SchemaForm @submit="confirm"> </SchemaForm>
                    </div>
                </a-col>
                <a-col :span="PageParams.TableSpan">                  
                    <div class="ScrollTable" ref="MaterialKittingData">
                        <DynamicTable sticky
                            showFavorite
                            EditExplain
                            row-key="id"
                            :header-title="PageSetting.HeadTitle"
                            :columns="PageSetting.Columns"
                            :key="PageSetting.Columns"
                            bordered
                            size="small" :PageParams="PageParams"
                            :data-source="PageSetting.List"
                            :data-all="PageSetting.moreData"
                            :pagination="false"
                            export-file-name="导出"
                            :scroll="{ x: PageSetting.x,}"
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
// import { schemas } from './form-schema';
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, unref, ref, onMounted, watch, computed, Ref } from 'vue';
import { GetCommon } from '@/api/pages/common';
import useRemoteSelect from '@/pages/ReportPortal/utils/SHuseRemoteSelect';
import { useLoading } from '@/components/feat/Loading';
import {cloneDeep} from 'lodash-es';
import { getPovitData, PovitParams } from '@/utils/helper/povitHelper'
import { showHidden, getRowClassName,getSumData,mergeTableRow,getdiffdate, } from '@/pages/ReportPortal/utils/SHcommon'
import { AddCommonPageParams } from '@/store/module/commonpage';
import { Console } from 'console';
import { message } from 'ant-design-vue';
import {PageParams} from '@/components/core/dynamic-table/src/dynamic-table'

// import {useRoute,useRouter} from 'vue-router'
// import { useInfiniteScroll, useVirtualList } from '@vueuse/core'

// let route  = useRoute();
// route.fullPath
let useCommonParams = AddCommonPageParams();
let bigfab=useCommonParams.Fab.slice(0,useCommonParams.Fab.length-1)
const MaterialKittingData = ref<HTMLElement | null>(null)
//vue3的mixin
const {  InfiniteScroll, } = useRemoteSelect()
function getImport(site: any) {
    //let { optionsdata } = require('@/pages/ReportPortal/Static/SHCommon-Form-schema');
    const { schemas } = require(`@/pages/ReportPortal/SH/MaterialKittingDataReport/${bigfab}/form-schema`);
    const { baseColumns,SpanColumns } = require(`@/pages/ReportPortal/SH/MaterialKittingDataReport/${bigfab}/Table`);
    return { schemas, baseColumns, SpanColumns };
}

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});

//let isShowExplain = ref<boolean>(false);


let { schemas, baseColumns, SpanColumns} = getImport(useCommonParams.Fab);

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
PageSetting.HeadTitle=`${useCommonParams.Fab} Material Kitting Data Report`
//PageSetting.Columns=[...baseColumns,...SpanColumns];
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
    let baseParams = {name: "Date",apiName: "MaterialKitDataSet",FactoryType:useCommonParams.Fab,G_FAC:useCommonParams.G_FAC_COM,}

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
        if (i == 'PanelId'||i == 'BLId'||i == 'PCBA'||i == 'CB') values[i] = dynamicFormRef.formModel[i].split('\n');
        else {
            values[i] = dynamicFormRef.formModel[i];
        }
    }

    if(values.CK_UseDate!="checked"){
        if (bigfab==="BL" ){
        if ((values.PanelId && values.PanelId.length > 0)||(values.PCBA && values.PCBA.length > 0)||(values.CB && values.CB.length > 0)  )
         {
             if(values.PanelId.length>1000||values.PCBA.length>1000||values.CB.length>1000)
             {
                closeFullLoading();
                return message.error('Panel ID Or 料號 or 批號输入超過 “最大限制筆數:1000” ');
             }
           
        } else {
            closeFullLoading();
            return message.error('请输入Panel ID Or 料號 or 批號！');
        } 
    }
    if (bigfab==="LCM" ){

        if ((values.PanelId && values.PanelId.length > 0)||(values.PCBA && values.PCBA.length > 0)||(values.CB && values.CB.length > 0) ||(values.BLId && values.BLId.length > 0) )
         {
             if(values.PanelId.length>1000||values.PCBA.length>1000||values.CB.length>1000||values.BLId.length>1000)
             {
                closeFullLoading();
                return message.error('Panel ID Or 料號 or 批號输入超過 “最大限制筆數:1000” ');
             }
           
        } else {
            closeFullLoading();
            return message.error('请输入Panel ID Or 料號 or 批號！');
        } 

    }

    }
     
   
   
   //獲取數據綁定table
    dynamicFormRef.validate().then(async () => {

       try{
          const { Date: { Data: { Table: Data },SQL1 } }  = await GetCommon({ ...baseParams, ...values });

          PageSetting.SQLHidden ="MaterialKittingDataReport-SQLHidden:"+ SQL1;

            if (Data.length == 0 ) {
                closeFullLoading();
                return message.warn("无数据！")
            }

            if (Data.length > 100000 ) {
                closeFullLoading();
                return message.warn("數據量超過10萬筆，請增加查詢條件減少數據量！")
            }

            PageSetting.Columns=baseColumns;
            let pushdata = cloneDeep(Data); 

                //第一次合併
            let combineCol1: any = {
            data: pushdata,
            mergeColNames: ['PNL_ID','TRANS_DATE','PROD_NBR'],
            //firstMergeColNames: ['FLOOR'],
            firstMerge: ['PNL_ID']
            };
            /**进行表格合并 */
            mergeTableRow(combineCol1);

            //第二次合併
            let combineCol2: any = {
            data: pushdata,
            mergeColNames: ['TRANS_DATE','PROD_NBR'],
            //firstMergeColNames: ['WORK_CTR', 'SIZE1'],
            firstMerge: 'TRANS_DATE'
            };

            mergeTableRow(combineCol2);

            //第三次合併
            let combineCol3: any = {
            data: pushdata,
            mergeColNames: ['PROD_NBR'],
            //firstMergeColNames: ['WORK_CTR', 'SIZE1'],
            firstMerge: 'PROD_NBR'
            };

            let Mergedata = mergeTableRow(combineCol3);

            PageSetting.moreData=Mergedata;

            if (PageSetting.moreData.length > 100) {
                PageSetting.List = PageSetting.moreData.slice(0, 40)
                // isLongData.value = true
            }//预先加载50条数据
            else {
                PageSetting.List = PageSetting.moreData
                // isLongData.value = false
            }
            if (PageSetting.Columns.length > 8 ){
                PageSetting.x = 150 * PageSetting.Columns.length;
            }
          
            //加載防護罩結束
            closeFullLoading();

       }catch(error: any){ 
           closeFullLoading();
           return Promise.reject(error); 
        }

    
    });
}



InfiniteScroll(MaterialKittingData, PageSetting)

//載入api 方法
onMounted(async () => {
    //optionsdata.OwnerName = await Getcommon(params) 
    // if (bigfab==="LCM" ){
    //     await mounted(optionsdata,useCommonParams.Fab)
    //     await mountedLCMowner(optionsdata, useCommonParams.Fab); 
    //     await mountedLCMBigEquipment(optionsdata, useCommonParams.Fab); 
    // }
    // if (bigfab==="BL" ){
    //     await mounted(optionsdata,useCommonParams.Fab)    
    //     await mountedLCMBigEquipment(optionsdata, useCommonParams.Fab); 
       
    // }
    
})

</script>

<style lang="less" scoped>
.btn-rows {
    button {
        margin-right: 12px;
    }
}


.MaterialKittingData {
    height: 700px;
    overflow-y: auto;
}

.swapper {
    display: flex;
    flex-direction: column;
}
</style>
