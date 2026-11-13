<template>
    <div>
        <a-card>
            <DynamicTable sticky v-bind="wrapperProps" row-key="id" :header-title="PageSetting.Title"
                :columns="PageSetting.Columns" :key="PageSetting.Columns" 
                    bordered size="small"
                :data-all="PageSetting.moreData" :explain="false" :OwnerParams="false"
                :data-source="PageSetting.List" :pagination="false" export-file-name="导出" 
                :scroll="{ x: 3000, y: 500 }">
                <template #export-button>
                    <a-button type="success"> 导出 </a-button>
                </template>
            </DynamicTable>
           <div hidden>{{ PageSetting.SQLHidden }}</div>

        </a-card>
    </div>
</template>

<script lang="ts" setup>
import { useTable } from '@/components/core/dynamic-table';
import { reactive, watchEffect, watch, computed,onMounted, } from 'vue';
import { GetCommon } from '@/api/pages/common';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { useLoading } from '@/components/feat/Loading';
import { message } from 'ant-design-vue';
import { Console } from 'console';
import { mergeTableRow,getSumData,getRowClassName,Inserttotalrow} from '@/pages/ReportPortal/utils/NGBcommon'
import { ReportCommon } from '@/store/module/ReportCommon';
import { useRouter, useRoute } from 'vue-router';

const router = useRouter();

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
        SecondTableParams:{},
        ThirdTableClickParams:{},
        //廠別
        Fab:'',
        G_FAC_COM:'',
});

let useCommonParams = AddCommonPageParams();
let useReportCommon = ReportCommon();

let ReportNameParams=`SpcCpkPerformanceReport-${useCommonParams.Fab}`;

let tableParams = useReportCommon[ReportNameParams].FirstTableParams;
let formParams= useReportCommon[ReportNameParams].FormParams;

let APIMessage=JSON.parse(useReportCommon[ReportNameParams].APIMessage);


let PageSetting = reactive({
    moreData: [] as any,
    List: [] as any,
    Columns: [] as any,
    TopTitle: '',
    BottomTitle: '',
    BottomList: [] as any,
    BottomColumns: [] as any,
    SQLHidden:'',
    BottomSQLHidden:'',
    x: '' as any,
    EchartOptions1: {},
    EchartOptions: {},
    heightValue: '0vh'
});

let Message:any;

const [DynamicTable] = useTable();

const customCellRender = (record: Object, rowIndex: number, column: Object) => {
    return {
        ondblclick: () => { 
            ReportItem.SecondTableParams={...record}; 
            useReportCommon.AddSecondTableParams(ReportNameParams, record);
             //useReportCommon.AddFirstTableParams(`MfgOperator-${useCommonParams.Fab}`,{ ...record, ...column['__originColumn__'] })
            router.push({ name: `SpcCpkPerformanceChart-${useCommonParams.Fab}`});
        },
        style: {
            cursor: 'pointer'
        }
    };
};


function GetDetailData(tableParams){   
   
    PageSetting.List=[];
    PageSetting.Columns=[];
    PageSetting.BottomList=[];
    PageSetting.BottomColumns=[];
    //加載防護罩開始
    openFullLoading();
    //基礎參數條件值
    GetDetail(tableParams);
};

async function GetDetail(tableParams) { 
    

    formParams= useReportCommon[ReportNameParams].FormParams;
    APIMessage=JSON.parse(useReportCommon[ReportNameParams].APIMessage);

     const ParamsData: any = {
        name: "Date",
        apiName: "CpkPerformanceDetail",
        FactoryType: formParams["FactoryType"],
        G_FAC: formParams["G_FAC"],
        FromDate:APIMessage["FromDate"],
        ToDate:APIMessage["ToDate"],
        sql_detail:APIMessage["sql_detail"]
     };

    try{
        const { Date: { Data: { Table: Data }, SQL01} } = await GetCommon(ParamsData);
         
        Message = {                
                sql:SQL01
            }; 
       

        PageSetting.SQLHidden="OnlineSpcChartDetail-SQLSecondDetail:"+SQL01;
        
        //循環查詢data，獲取table Colums 數據
        let ColumsData: any = [];
                let WidthPars=50;
                let titlePars="";//label
                let classNamePars="v-center";//API提供轉置后的時間處理（例如：A2022_11_30）
                let fixedPara="left";
            for (let item in Data[0]) { 
                WidthPars=30;
                titlePars=item;
                classNamePars="";
                ColumsData.push({ title: titlePars, align: 'center', dataIndex: item, className: classNamePars, width:WidthPars, fixed:fixedPara,
                       customCell: customCellRender, 
                      });  
                }

            PageSetting.Columns = ColumsData;

        /**进行表格合并 */
        PageSetting.moreData=Data;                
        PageSetting.List=PageSetting.moreData;

        ReportItem.APIMessage = JSON.stringify(Message);  
        ReportItem.FormParams={ ...ParamsData };
       
    }catch(error: any){ 
         //加載防護罩結束
        closeFullLoading();
        return Promise.reject(error); 
     }
    
}


watchEffect(() => {
    //一阶表格监控
    PageSetting['List'] = [];
    PageSetting['Columns'] = [];
    PageSetting['TopTitle'] = "";
    tableParams = useReportCommon[ReportNameParams].FirstTableParams;
    GetDetailData(tableParams);
});

</script>


