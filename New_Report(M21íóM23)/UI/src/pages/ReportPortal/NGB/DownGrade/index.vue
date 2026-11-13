<template>
    <div class="DownGrade">
        <a-card>
            <!-- <a-row>
                <span class="menu-fold" @click="showHidden(PageSetting)">
                    <component :is="!PageSetting.showmenu ? MenuUnfoldOutlined : MenuFoldOutlined" />
                </span>
            </a-row> -->
            <a-row>
                <a-col :span="24 - PageParams.TableSpan" class="NGBScrollForm" v-show="PageSetting.showmenu">
                    <SchemaForm @submit="confirm" :showTextPosition="'center'"> </SchemaForm>
                </a-col>
                <a-col :span="PageParams.TableSpan">
                    <div class="ScrollTable" ref="DownGrade">
                        <DynamicTable sticky
                          showFavorite
                          :PageParams="PageParams"
                            row-key="DownGrade"
                            :header-title="PageSetting.Title"
                            :columns="PageSetting.Columns"
                            :key="PageSetting.Columns"
                            bordered
                            size="small"
                            :data-source="PageSetting.moreData"
                            export-file-name="导出"
                            :row-class-name="getRowClassName_Sel"
                            
                            :scroll="{ x: PageSetting.x,}"   
                            :pagination="false"
                        >
                            <!-- <template #toolbar>
                                <a-button type="primary" @click="isShowExplain = !isShowExplain">{{
                                    isShowExplain ? '显示表格' : '显示说明文件'
                                }}</a-button>
                            </template> -->
                            <template #export-button>
                                <a-button type="success"> 导出 </a-button>
                                <!-- <span>{{ $t('layout.multipleTab.reload') }}</span> -->
                            </template>
                        </DynamicTable>
                    </div>
                </a-col>
                <div hidden>{{ PageSetting.SQLHidden }}</div>
            </a-row>
            
        </a-card>
    </div>
</template>

<script lang="ts" setup>
import {PageParams} from '@/components/core/dynamic-table/src/dynamic-table'
import { MenuFoldOutlined, MenuUnfoldOutlined } from '@ant-design/icons-vue';
import { showHidden, getRowClassName, mergeTableRow,Inserttotalrow,getSumData } from '@/pages/ReportPortal/utils/NGBcommon';
// import { schemas } from './form-schema';
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, onMounted, ref, unref } from 'vue';
import useRemoteSelect from '@/pages/ReportPortal/utils/NGBuseRemoteSelect';
import { GetCommon } from '@/api/pages/common';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { useLoading } from '@/components/feat/Loading';
import { message } from 'ant-design-vue';
import {cloneDeep} from 'lodash-es';
import { AnyARecord } from 'dns';
import { ReportCommon } from '@/store/module/ReportCommon';
import { useRouter } from 'vue-router';

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});
//let isShowExplain = ref<boolean>(false);
const { InfiniteScroll, } = useRemoteSelect();
const useCommonParams = AddCommonPageParams();
let useReportCommon = ReportCommon();
let bigfab=useCommonParams.Fab.slice(0,useCommonParams.Fab.length-1)


const DownGrade = ref<HTMLElement | null>(null);
function getImport(site: any) {
    
    //const { optionsdata } = require('@/pages/ReportPortal/Static/NGBCommon-Form-schema');
    const { schemas } = require(`@/pages/ReportPortal/NGB/DownGrade/${bigfab}/form-schema`);
    const { DownGradeColumns, DownGradeDfctColumns } = require(`@/pages/ReportPortal/NGB/DownGrade/${bigfab}/Table`);
    return { schemas,  DownGradeColumns, DownGradeDfctColumns };
}
let { schemas,  DownGradeColumns, DownGradeDfctColumns } = getImport(useCommonParams.Fab);


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
let ReportNameParams = `DownGrade-${useCommonParams.Fab}`;
useReportCommon.AddObject(ReportNameParams,ReportItem);

let PageSetting = reactive({
    List: [],
    TableSpan: 20,
    showmenu: true,
    APIColumns: [],
    Columns: [] as any,
    moreData: [],
    SQLHidden:"",
    Title:"",
     x: '' as any,
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

async function confirm(values) {
       //table 初始化
    PageSetting.List=[];
    PageSetting.Columns=[];
    //加載防護罩開始
    openFullLoading();
    let baseParams = {
        name: "Date",
        apiName: 'TDownGrade_DataSet',
        FactoryType:useCommonParams.Fab,
        G_FAC: useCommonParams.G_FAC_COM,
    };
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

    //獲取數據綁定table
    dynamicFormRef.validate().then(async () => {

       try{
          const { Date: { Data: { Table: Data },
                 cuttime,
                 SQL01,
                 StrSql_w,
                 StrSql_w1,
                 lstr_ShowMode,
                 lstr_Operation,
                 sql_WoType,
          
          } }  = await GetCommon({ ...baseParams, ...values });

          PageSetting.SQLHidden = "TDownGrade_DataSet-SQLHidden:"+SQL01
          //console.log("TDownGrade_DataSet-SQLHidden:",SQL01)  
          PageSetting.Title = `${useCommonParams.Fab} Down Grade Summary`;     

          if (Data.length != 0) {             

               let Message = {                
                StrSql_w: StrSql_w,
                StrSql_w1: StrSql_w1,
                lstr_ShowMode: lstr_ShowMode,
                lstr_Operation: lstr_Operation,
                sql_WoType: sql_WoType,
               };
               
              
               if(values['ShowMode']=="Defect_Code"){
                   PageSetting.Columns=[
                        {
                            title: "Dfct Code",
                            align: 'left',
                            dataIndex: "DFCT_CODE",
                            className:'v-center',
                            width:30,
                            fixed:"left",
                        },
                        
                        ...DownGradeDfctColumns,
                    ]

               }else if(values['ShowMode']=="Operator"){

                    PageSetting.Columns=[
                        {
                            title: "人員工號",
                            align: 'center',
                            dataIndex: "OPERATORS",
                            className:'v-center',
                            width:40,
                            fixed:"left",
                        },
                        ...DownGradeColumns,
                    ]
               }else if(values['ShowMode']=="Prod_nbr"){

                    PageSetting.Columns=[
                            {
                                title: "Product",
                                align: 'center',
                                dataIndex: "PROD_NBR",
                                className:'v-center',
                                width:60,
                                fixed:"left",
                            },
                            ...DownGradeColumns,
                    ]

               }
               
              
              if(values['ShowMode']=="Prod_nbr"){
                
                let { insertIndexList, insertrowList } = Inserttotalrow_Sel(Data, ["PROD_NBR"], 
                ['TOTAL', 'A', 'B', 'C', 'C2', 'C3', 'C4', 'C5', 'C6', 'D', 'E', 'W','K','G','L', 'S','DOWN','RWK',], "PROD_NBR");
                                
              }

              Data.forEach((item) => {                   
        
               if(values['ShowMode']=="Prod_nbr"){
                    if (item.PROD_NBR.indexOf("subtotal")===-1) {
                        item['DOWN_GRADE_RATE']=item.DOWN/item.TOTAL== Infinity
                                    ? 0:(item.DOWN/item.TOTAL* 100).toFixed(2)+"%";
                    
                        }else{
                            item['DOWN_GRADE_RATE']=item.DOWN/(item.TOTAL-item.L)== Infinity
                                    ? 0:(item.DOWN/(item.TOTAL-item.L)* 100).toFixed(2)+"%";
                        }

                        item['REPAIR_RATE']=item.RWK/item.TOTAL== Infinity
                                    ? 0:(item.RWK/item.TOTAL* 100).toFixed(2)+"%"; 

                        item['OK_RATE']=(item.TOTAL-item.DOWN)/item.TOTAL== Infinity
                                    ? 0:((item.TOTAL-item.DOWN)/item.TOTAL* 100).toFixed(2)+"%"; 
                }else{

                        item['REPAIR_RATE']=item.RWK/item.TOTAL== Infinity
                                    ? 0:(item.RWK/item.TOTAL* 100).toFixed(2)+"%"; 

                        item['OK_RATE']=(item.TOTAL-item.DOWN)/item.TOTAL== Infinity
                                    ? 0:((item.TOTAL-item.DOWN)/item.TOTAL* 100).toFixed(2)+"%"; 
                        
                        item['DOWN_GRADE_RATE']=item.DOWN/item.TOTAL== Infinity
                                    ? 0:(item.DOWN/item.TOTAL* 100).toFixed(2)+"%";

                }



                       
                });

               PageSetting.moreData=Data;
               //PageSetting.List = PageSetting.moreData
               if (PageSetting.moreData.length > 1000 ) {
                PageSetting.List = PageSetting.moreData.slice(0, 100)
            
                // isLongData.value = true
                }//预先加载50条数据
                else {
                    PageSetting.List = PageSetting.moreData
                    // isLongData.value = false
                }

                PageSetting.x = null;
                if (PageSetting.Columns.length > 7) {
                    PageSetting.x = 100 * PageSetting.Columns.length;
                }

               ReportItem.APIMessage = JSON.stringify(Message); 
               ReportItem.FormParams={ ...baseParams,...values }

               //console.log("useReportCommon",useReportCommon)
                
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
//实现无限滚动或者说懒加载
InfiniteScroll(DownGrade, PageSetting);


// 插入行合计,dataSource数据源，standard:相同的屬性才合計，calcuList要合计的列，是否是负数isNegative,合计属性sumprops
function Inserttotalrow_Sel(dataSource: any[], standard: any[], calcuList: any[], sumprops: string, isNegative: boolean = false) {
    let temp: any = cloneDeep(dataSource[0]);
    let totalsum: any[] = [];
    let totalindex: any[] = [];
    let regArr: any[] = [];
    let now_big_prod="";
    let next_big_prod="";
    
    for (let i = 0; i < dataSource.length - 1; i++) {        
        standard.forEach(item => {
            regArr.push(dataSource[i][item].slice(0,8) == dataSource[i + 1][item].slice(0,8),dataSource[i][item].slice(0,8),dataSource[i + 1][item].slice(0,8))            
            
        })
       
        //debugger;
        if (regArr.includes(false)) {
            now_big_prod=regArr[1];
            next_big_prod=regArr[2];
            regArr = [];
            Object.keys(temp).forEach((item) => {
                if (!calcuList.includes(item)) {
                    temp[item] = '';
                }
                if (item == sumprops) {
                    temp[item] = now_big_prod+'-subtotal';
                }
                // 合計需要是負數的處理
                // 不包含在數組裏面的就是整數
                if (isNegative && calcuList.includes(item)) {
                    temp[item] = -Math.ceil(temp[item]);
                }
            });
           
            totalindex.push(i + 1);
            totalsum.push(temp);            
            temp = cloneDeep(dataSource[i + 1]);
            continue;
        } else {
            for (let j = 0; j < calcuList.length; j++) {
                if(isNaN(temp[calcuList[j]])){
                    temp[calcuList[j]] = 0;
                }if(!isNaN(dataSource[i + 1][calcuList[j]])){
                    temp[calcuList[j]] += dataSource[i + 1][calcuList[j]];
                }
            }
        }
       
    }
    
   
   next_big_prod=dataSource[dataSource.length-1][sumprops].slice(0,8)
   
   //console.log("next_big_prod",next_big_prod)

    //最後一批資料的加總
    Object.keys(temp).forEach((item) => {
        if (!calcuList.includes(item)) {
            temp[item] = '';
        }

        if (item == sumprops) {
            temp[item] = next_big_prod+'-subtotal';
        }
        // 合計需要是負數的處理
        if (isNegative && calcuList.includes(item)) {
            temp[item] = -Math.ceil(temp[item]);
        }
    });

    
     totalsum.push(temp);
     totalindex.push(dataSource.length);
     

    for (let i = 0; i < totalindex.length; i++) {
        dataSource.splice(totalindex[i] + i, 0, totalsum[i]);
    }

    let TotalSub = getSumData(totalindex.length, 0, totalsum,calcuList); 
    dataSource.splice(dataSource.length + 1, 0, Object.assign(TotalSub, { PROD_NBR: 'total' }));  

    return {
        insertrowList: totalsum,
        insertIndexList: totalindex
    };
}

function getRowClassName_Sel(record: Object) {
    if (record && Object.keys(record).length != 0) {
        for (const [key, value] of Object.entries(record)) {
            if(key=="PROD_NBR"){
               
                if (['total'].includes(value)) {
                    return 'Yellow';
                }
                if (value.indexOf("subtotal")!==-1 ) {
                   return 'Green'; 
                }
            }
           
        }
    }
}

</script>


