<template>
    <div class="DayliMove">
        <a-card>
            <!-- <a-row>
                <span class="menu-fold" @click="showHidden(PageSetting)">
                    <component :is="!PageSetting.showmenu ? MenuUnfoldOutlined : MenuFoldOutlined" />
                </span>
            </a-row> -->
            <a-row>
                <a-col
                    :span="24 - PageParams.TableSpan"
                    class="NGBScrollForm"
                    Source-show="PageSetting.showmenu"
                >
                    <div class="NGBScrollForm">
                        <SchemaForm @submit="confirm"> </SchemaForm>
                    </div>
                </a-col>
                <a-col :span="PageParams.TableSpan">                  
                    <div class="ScrollTable" ref="OQCInspectionDetailList">
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
                            :row-class-name="getRowClassName"
                            :scroll="{ x: 3000,y: 500 }"                            
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
import useRemoteSelect from '@/pages/ReportPortal/utils/NGBuseRemoteSelect';
import { useLoading } from '@/components/feat/Loading';
import {cloneDeep} from 'lodash-es';
import { getPovitData, PovitParams } from '@/utils/helper/povitHelper'
import { showHidden, getRowClassName,getSumData,mergeTableRow,getdiffdate, } from '@/pages/ReportPortal/utils/NGBcommon'
import { AddCommonPageParams } from '@/store/module/commonpage';
import { Console } from 'console';
import { message } from 'ant-design-vue';
import {PageParams} from '@/components/core/dynamic-table/src/dynamic-table'

// import {useRoute,useRouter} from 'vue-router'
// import { useInfiniteScroll, useVirtualList } from '@vueuse/core'

// let route  = useRoute();
// route.fullPath
let useCommonParams = AddCommonPageParams();
const OQCInspectionDetailList = ref<HTMLElement | null>(null)
//vue3的mixin
const {  InfiniteScroll,} = useRemoteSelect()
function getImport(site: any) {
    //let { optionsdata } = require('@/pages/ReportPortal/Static/NGBCommon-Form-schema');
    const { schemas } = require(`@/pages/ReportPortal/NGB/OQC/OQCInspectionDetailList/LCM/form-schema`);
    const { OQCInspectionDetailListColumns } = require(`@/pages/ReportPortal/NGB/OQC/OQCInspectionDetailList/LCM/Table`);
    return { schemas, OQCInspectionDetailListColumns,  };
}

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});


let { schemas, OQCInspectionDetailListColumns, } = getImport(useCommonParams.Fab);

let PageSetting = reactive({
    List:[] as any,
    TableSpan: 20,
    showmenu: true,
    APIColumns: [] as any,
    Columns: [] as any,
    moreData: [] as any,
    SQLHidden:'',
    HeadTitle:'',

});
// let isLongData = ref(true)
PageSetting.HeadTitle=`${useCommonParams.Fab} (MD39) OQC Inspection Detail List`
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
    let baseParams = {name: "Date",apiName: "OQCInspListDataSet",FactoryType:useCommonParams.Fab,G_FAC:useCommonParams.G_FAC_COM,building:"",}

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

    //values['FromDate'] = values['CompleteFromDate'].split(' ')[0];
    //values['FromTime'] = values['CompleteFromDate'].split(' ')[1];
    //values['ToDate'] = values['CompleteToDate'].split(' ')[0];
    //values['ToTime'] = values['CompleteToDate'].split(' ')[1];

    for (let i in dynamicFormRef?.formModel) {
        if (i == 'Wo_nbr' || i == 'SN') values[i] = dynamicFormRef.formModel[i].split('\n');
        else {
            values[i] = dynamicFormRef.formModel[i];
        }
    }
    
    //console.log(dynamicFormRef.formMode
   
   //獲取數據綁定table
    dynamicFormRef.validate().then(async () => {

       try{
          const { Date: { Data: { Table: Data },sql } }  = await GetCommon({ ...baseParams, ...values });

          PageSetting.SQLHidden = sql
          //console.log("OQCInspectionDetailList-SQLHidden:",sql)
          //console.log("OQCInspectionDetailList-Data:",Data)
          if (Data.length != 0) {
            PageSetting.Columns = [...OQCInspectionDetailListColumns];
            PageSetting.moreData = Data;
            if (PageSetting.moreData.length > 100) 
               PageSetting.List = PageSetting.moreData.slice(0, 40);
            else 
               PageSetting.List = PageSetting.moreData;
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


//实现无限滚动或者说懒加载
InfiniteScroll(OQCInspectionDetailList, PageSetting)

//載入api 方法
onMounted(async () => {
    //optionsdata.OwnerName = await Getcommon(params) 
    // await mounted(optionsdata,useCommonParams.Fab)
    // await mountedLCMowner(optionsdata, useCommonParams.Fab);        
    // await mountedOper(optionsdata, useCommonParams.Fab);  
    // await mountedCustomer(optionsdata, useCommonParams.Fab);  
    // await mountedModuleData(optionsdata, useCommonParams.Fab);
    
})

</script>

<style lang="less" scoped>
.btn-rows {
    button {
        margin-right: 12px;
    }
}

.DayliMove :deep(.Source-center) {
    background-color: #f0f0f0;
}


.DayliMove :deep(.Yellow) {
    background-color: #80ff80;
}


.OQCInspectionDetailList {
    height: 700px;
    overflow-y: auto;
}

.DayliMove :deep(.YellowGreen) {
    background-color: #f39230;
}



.swapper {
    display: flex;
    flex-direction: column;
}
</style>
