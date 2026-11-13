<template>
    
        <a-card>          
            <a-row>
                <a-col :span="24 - PageParams.TableSpan" class="SHScrollForm" v-show="PageSetting.showmenu">
                    <SchemaForm @submit="confirm"> </SchemaForm>
                </a-col>
                <a-col :span="PageParams.TableSpan" class="ScrollTable">                    
                    <DynamicTable sticky
                        showFavorite
                        row-key="id"
                        header-title="LCD廠panel於LCM廠投產分佈狀況 Summary1"
                        :columns="PageSetting.TopColumns"
                        :key="PageSetting.TopColumns"
                        bordered 
                        id="LCDPanelLCMDistribution1"
                        export-type="Table"
                        size="small"
                        :data-source="PageSetting.TopList"
                        :pagination="false"
                        :PageParams="PageParams"
                        :scroll="{ y: PageSetting.y1 }"
                        export-file-name="LCDPanelLCMDistribution1"
                    >
                      
                        <template #export-button>
                            <a-button type="success"> 导出 </a-button>
                        </template>
                    </DynamicTable>
                    <DynamicTable sticky
                        row-key="id"
                        header-title="LCD廠panel於LCM廠投產分佈狀況 Summary2"
                        :columns="PageSetting.BottomColumns"
                        :key="PageSetting.BottomColumns"
                        bordered
                        size="small" 
                        :data-source="PageSetting.BottomList"
                        :pagination="false" 
                        :explain="false"
                        id="LCDPanelLCMDistribution2"
                        export-type="Table"
                        :OwnerFoldParams="false"
                        :scroll="{ y:  PageSetting.y2 }"  v-show="RightBottomPageShow"
                        export-file-name="LCDPanelLCMDistribution2"  >
                        <template #export-button>
                            <a-button type="success"> 导出 </a-button>
                        </template>
                    </DynamicTable>
                </a-col>
                <div hidden>{{ PageSetting.SQLHidden }}</div>
            </a-row>

        </a-card>
   
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
import { mergeTableRow,getSumData } from '@/pages/ReportPortal/utils/SHcommon'
import { AddCommonPageParams,AddCommonPageParamsWithOut } from '@/store/module/commonpage';
import useRemoteSelect from '@/pages/ReportPortal/utils/SHuseRemoteSelect';
import { AnyNaptrRecord } from 'dns';
import router from '@/router';
import { useLoading } from '@/components/feat/Loading';
import { cloneDeep } from 'lodash-es';
import {PageParams} from '@/components/core/dynamic-table/src/dynamic-table'
import { ReportCommon } from '@/store/module/ReportCommon';

const useCommonParams = AddCommonPageParams();
let useReportCommon = ReportCommon();
let bigfab=useCommonParams.Fab.slice(0,useCommonParams.Fab.length-1)
const RightBottomPageShow = ref(false);


function getImport(site: any) {
    //const { optionsdata } = require('@/pages/ReportPortal/Static/SHCommon-Form-schema');
    const { FirstSumLabel,FirstSum2Label } = require(`@/pages/ReportPortal/SH/LCDPanelLCMDistribution/${bigfab}/Table`);
    const { schemas,  getSchemaData } = require(`@/pages/ReportPortal/SH/LCDPanelLCMDistribution/${bigfab}/form-schema`);
    return { schemas,  getSchemaData,FirstSumLabel,FirstSum2Label };
}
const { schemas,  getSchemaData,FirstSumLabel,FirstSum2Label} = getImport(useCommonParams.Fab);
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
let ReportNameParams = `LCDPanelLCMDistribution-${useCommonParams.Fab}`;
useReportCommon.AddObject(ReportNameParams,ReportItem);


// 頁面數據
let PageSetting = reactive({
    TopList: [] as any,
    BottomList: [] as any,
    TableSpan: 20,
    showmenu: true,
    TopColumns: [] as any,
    BottomColumns: [] as any,
    HeadTitle:'',
    SQLHidden:'',
    y1: '' as any,
    y2: '' as any,

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



PageSetting.HeadTitle=`${useCommonParams.Fab} LCD廠panel於LCM廠投產分佈狀況`

async function confirm(values) {
    openFullLoading();
      //table 初始化
    PageSetting.TopList=[];
    PageSetting.BottomList=[];

    PageSetting.TopColumns=[];
    PageSetting.BottomColumns=[];
    let ApiName="";

    if(values.QueryType=="MIX"){
        ApiName="MixRateAlarmDataSet"
    }else{
        ApiName="LCDSTPNDataSet"
    }

    let baseParams = {name: "Date",apiName: ApiName,FactoryType:useCommonParams.Fab,G_FAC:useCommonParams.G_FAC_COM,}
        
    Object.keys(values).forEach((key) => {      
        if (!values[key]) {
            values[key] = '';
        }
       
    });

   

   for (let i in dynamicFormRef?.formModel) {
       values[i] = dynamicFormRef.formModel[i];
    }

    dynamicFormRef.validate().then(async () => {

        try{

            if(values.QueryType=="MIX"){

                PageSetting.y2=PageSetting.y1=350

                RightBottomPageShow.value=true
                    const { Date: { 
                    Data: { Table: Data},//SUM1
                    Data_3: { Table: Data_3 },//SUM2
                    SQL01,
                    SQL02,
                    FromDate,
                    ToDate,
                    lstr_AbnormalCode,
                    sql_Date,
                    sql_Shift,
                    sql_Floor,
                    sql_LcdProduct,
                    sql_Prod_nbr,
                    sql_LcmOwner,
                
                } } = await GetCommon({...baseParams,...values});
                PageSetting.SQLHidden=ApiName+"-SQLHidden:"+SQL01+";"+SQL02;
                if (Data.length == 0 && Data_3.length == 0) {
                closeFullLoading();
                return message.warn("无数据！")
               }

                let Message = {
                    FromDate:FromDate,
                    ToDate:ToDate,
                    lstr_AbnormalCode:lstr_AbnormalCode,
                    sql_Date:sql_Date,
                    sql_Shift:sql_Shift,
                    sql_Floor:sql_Floor,
                    sql_LcdProduct:sql_LcdProduct,
                    sql_Prod_nbr:sql_Prod_nbr,
                    sql_LcmOwner:sql_LcmOwner,
                };
                ReportItem.APIMessage = JSON.stringify(Message); 
                ReportItem.FormParams={ ...baseParams,...values }

                PageSetting.TopColumns=FirstSumLabel;
                PageSetting.BottomColumns=FirstSum2Label;

                PageSetting.TopList=Data;
                PageSetting.BottomList=Data_3;
            }else{
                PageSetting.y1=600
                PageSetting.y2=0

                RightBottomPageShow.value=false
                const { Date: { 
                    Data: { Table: Data},
                    SQL01,                   
                
                } } = await GetCommon({...baseParams,...values});
                PageSetting.SQLHidden=ApiName+"-SQLHidden:"+SQL01;
                if (Data.length == 0) {
                closeFullLoading();
                return message.warn("无数据！")
               }

               let ColumsData: any = [];
                let WidthPars=80;
                let classNamePars="";
                let titlePars="";

                for (let item in Data[0]) { 
                    if(item.slice(0,1)=="A"){
                        titlePars=item.slice(1).replaceAll('_', '-');
                        WidthPars=80;
                    }else{
                        titlePars=item;
                        if (item=="LCM_PROD"||item=="LCD_PROD"||item=="LCD_ABN_LCD") {
                            WidthPars=120;  
                        }else{
                            WidthPars=85; 
                        }
                    } 
                    ColumsData.push({ title: titlePars, align: 'center', dataIndex: item, className: classNamePars, width:WidthPars});
                }
            
                PageSetting.TopColumns=ColumsData;
                PageSetting.TopList = Data;
            }
            
            
          
            closeFullLoading();


        }catch(error: any){ 
           closeFullLoading();
           return Promise.reject(error); 
        }
        
    });
   
}




// //实现无限滚动或者说懒加载
// InfiniteScroll(StartBankStatusWip, PageSetting);
onMounted(
    async () => {
        // await mountedLCDowner(optionsdata, useCommonParams.Fab);
        // await mountedLCDGrade(optionsdata, useCommonParams.Fab);        
    }
)

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
