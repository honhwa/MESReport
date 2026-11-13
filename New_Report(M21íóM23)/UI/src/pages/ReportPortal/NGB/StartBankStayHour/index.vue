<template>
    <div>
        <a-card>
            <!-- <a-row>
                <span class="menu-fold" @click="showHidden">
                    <component :is="!PageSetting.showmenu ? MenuUnfoldOutlined : getRowClassNameMenuFoldOutlined" />
                </span>
            </a-row> -->
            <a-row>
                <a-col :span="24 - PageParams.TableSpan" class="NGBScrollForm" v-show="PageSetting.showmenu">
                    <SchemaForm @submit="confirm"> </SchemaForm>                  
                   
                </a-col>
                <a-col :span="PageParams.TableSpan">
                  <div class="ScrollTable" ref="StartBankStayHour">
                    <DynamicTable showFavorite row-key="id" :header-title="PageSetting.HeadTitle" sticky
                        :columns="PageSetting.Columns" :key="PageSetting.Columns" bordered size="small"
                        :data-source="PageSetting.List" :pagination="false" 
                        :data-all="PageSetting.moreData"   :PageParams="PageParams"
                        export-file-name="导出">

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
import { useTable } from '@/components/core/dynamic-table';
import { reactive, ref } from '@vue/reactivity';
import { useForm } from '@/components/core/schema-form';
// import { cloneDeep } from 'lodash-es';
import { GetCommon } from '@/api/pages/common';
import { onMounted } from '@vue/runtime-core';
import { message } from 'ant-design-vue';
import type { TableColumn } from '@/components/core/dynamic-table';
import { getrowsum} from "@/utils/reportcommon/Calculation";
import { mergeTableRow,getSumData } from '@/pages/ReportPortal/utils/NGBcommon'
import { AddCommonPageParams,AddCommonPageParamsWithOut } from '@/store/module/commonpage';
import useRemoteSelect from '@/pages/ReportPortal/utils/NGBuseRemoteSelect';
import { AnyNaptrRecord } from 'dns';
import router from '@/router';
import { useLoading } from '@/components/feat/Loading';
import { cloneDeep } from 'lodash-es';
import {PageParams} from '@/components/core/dynamic-table/src/dynamic-table'
import { ReportCommon } from '@/store/module/ReportCommon';

const useCommonParams = AddCommonPageParams();
let useReportCommon = ReportCommon();
let bigfab=useCommonParams.Fab.slice(0,useCommonParams.Fab.length-1)
const {InfiniteScroll} = useRemoteSelect()

const StartBankStayHour = ref<HTMLElement | null>(null);
function getImport(site: any) {
    const { FirstLabel,} = require(`@/pages/ReportPortal/NGB/StartBankStayHour/${bigfab}/Table`);
    const { schemas, } = require(`@/pages/ReportPortal/NGB/StartBankStayHour/${bigfab}/form-schema`);
    return { schemas, FirstLabel };
}
const { schemas,FirstLabel} = getImport(useCommonParams.Fab);
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
let ReportNameParams = `StartBankStayHour-${useCommonParams.Fab}`;
useReportCommon.AddObject(ReportNameParams,ReportItem);


// 頁面數據
let PageSetting = reactive({
    TableSpan: 20,
    showmenu: true,
    List: [] as any,
    Columns: [] as any,
    Message: {},
    moreData: [] as any,
    HeadTitle:'',
    SQLHidden:'',
    APIColumns: [] as any,
});

const [DynamicTable] = useTable();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 130,
    schemas:schemas(useCommonParams.Fab,reactive({})),
    actionColOptions: {
        span: 24
    },
    layout:"vertical"
});

PageSetting.HeadTitle=`${useCommonParams.Fab} Start Bank Stay Hour `

async function confirm(values) {
    openFullLoading();
      //table 初始化
    PageSetting.List=[];
    PageSetting.Columns=[];

    let baseParams = {name: "Date",apiName: "StartBankDataset",FactoryType:useCommonParams.Fab,G_FAC:useCommonParams.G_FAC_COM,}
        
    Object.keys(values).forEach((key) => { 
        if (!values[key]) {
            values[key] = '';
        }
    });
   

   for (let i in dynamicFormRef?.formModel) {
       values[i] = dynamicFormRef.formModel[i];
    }

    if(!values.StartHour){
        closeFullLoading();
        return message.error('Start Hours沒有輸入！'); 
    }


    //增加查詢參數 OverHours 數據等於 參數StartHour
    values['OverHours']=values['StartHour'] 
    dynamicFormRef.validate().then(async () => {
        try{

            const { Date: { Data: { Table: Data},SQL01 } } = await GetCommon({...baseParams,...values});
            
            //console.log("StartBankStatusDataSet-SQLHidden:",SQL01)

             PageSetting.SQLHidden="StartBankStayHour-SQLHidden:"+SQL01;
             ReportItem.FormParams={ ...baseParams,...values }

            if (Data.length != 0) {  

                if(values['AbnormalCode']=="Y"){
                    PageSetting.Columns=[
                        ...FirstLabel,
                        {
                            title: "Abnormal Code",
                            dataIndex: "LCD_ABN_LCD",
                            width:60,
                        },
                    ]

                }else{
                    PageSetting.Columns = FirstLabel;
                }
                
                let combineCol1: any = {
                    data: Data,
                    mergeColNames: ['SIZE1'],
                    //firstMergeColNames: ['FLOOR'],
                    firstMerge: ['SIZE1']
                };

                /**进行表格合并 */
                PageSetting.moreData=mergeTableRow(combineCol1)

                if (PageSetting.moreData.length > 2000) {
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


        }catch(error: any){ 
           closeFullLoading();
           return Promise.reject(error); 
        }
        
    });
   
}
//实现无限滚动或者说懒加载
InfiniteScroll(StartBankStayHour, PageSetting);


</script>

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
.newbutton {
    float: right;
    margin-left: 7px;
}
</style>
