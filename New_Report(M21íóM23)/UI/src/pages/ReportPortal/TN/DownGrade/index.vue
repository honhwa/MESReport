<template>
    <div class="DownGrade">
        <a-card>
            <!-- <a-row>
                <span class="menu-fold" @click="showHidden(PageSetting)">
                    <component :is="!PageSetting.showmenu ? MenuUnfoldOutlined : MenuFoldOutlined" />
                </span>
            </a-row> -->
            <a-row>
                <a-col Source-show="PageParams.ShowMenu" :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                    <SchemaForm @submit="confirm" > </SchemaForm>
                </a-col>
                <a-col :span="PageParams.TableSpan">
                    <div ref="DownGrade" >
                        <DynamicTable
                          showFavorite
                          :PageParams="PageParams"
                            row-key="DownGrade"
                            :header-title="PageSetting.Title"
                            :columns="PageSetting.Columns"
                            :key="PageSetting.Columns"
                            bordered
                            size="small"
                            :data-source="PageSetting.moreData"
                            :data-all="PageSetting.moreData"
                            export-file-name="Export"
                            :row-class-name="getRowClassName_Sel"
                            :scroll="{x:1500}"
                            sticky
                            :pagination="false"
                            :flexwidth="1"
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
import { showHidden, getRowClassName, mergeTableRow,Inserttotalrow,getSumData } from '@/pages/ReportPortal/utils/TNcommon';
// import { schemas } from './form-schema';
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, onMounted, ref, unref } from 'vue';
import useRemoteSelect from '@/pages/ReportPortal/utils/TNuseRemoteSelect';
import { GetCommon } from '@/api/pages/common';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { ReportCommon } from '@/store/module/ReportCommon';
import { useLoading } from '@/components/feat/Loading';
import { message } from 'ant-design-vue';
import {cloneDeep} from 'lodash-es';
import {PageParams} from '@/components/core/dynamic-table/src/dynamic-table'
//import {optionsdata} from '@/pages/ReportPortal/Static/TNCommon-Form-schema';

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});
//let isShowExplain = ref<boolean>(false);
const { mounted,getCommonFormParams,InfiniteScroll } = useRemoteSelect();
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
let ReportNameParams = `DownGradeDetail_${useCommonParams.Fab}`;

let fab = "MOD";
if(useCommonParams.Fab == "MOD1")
{
    fab = useCommonParams.Fab;
}

// const DownGrade = ref<HTMLElement | null>(null);
function getImport(site: any) {
    
    //const { optionsdata } = require('@/pages/ReportPortal/Static/TNCommon-Form-schema');
    const { schemas } = require(`@/pages/ReportPortal/TN/DownGrade/${fab}/form-schema`);
    const { DownGradeColumns } = require(`@/pages/ReportPortal/TN/DownGrade/${fab}/Table`);
    return { schemas, DownGradeColumns };
}
let { schemas, DownGradeColumns } = getImport(fab);
let FactoryType: any = useCommonParams.Fab;
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
       //table 初始化
    PageSetting.List=[];
    PageSetting.Columns=[];
    //加載防護罩開始
    openFullLoading();
    let baseParams = {
        name: "Date",
        apiName: 'TN_DownGradeDataSet',
        FactoryType:useCommonParams.Fab,
        G_FAC: useCommonParams.G_FAC_COM,
    };
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
                 SQLFilter,
          
          } }  = await GetCommon({ ...baseParams, ...values });

          PageSetting.SQLHidden = "DownGrade_DataSet-SQLHidden:"+SQL01
          //console.log("TDownGrade_DataSet-SQLHidden:",SQL01)  
          PageSetting.Title = `${useCommonParams.Fab} Down Grade Summary`;     

          if (Data.length != 0) {             

               let Message = {                
                SQLFilter: SQLFilter
               };
              
               if(values['ShowMode']=="Operator"){

                    PageSetting.Columns=[
                        {
                            title: "人員工號",
                            align: 'center',
                            dataIndex: "OPERATOR",
                            className:'v-center',
                            width:50,
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
                                width:80,
                            },
                            ...DownGradeColumns,
                    ]

               }
               
              
              if(values['ShowMode']=="Prod_nbr"){
                
                let { insertIndexList, insertrowList } = Inserttotalrow_Sel(Data, ["PROD_NBR"], 
                ['TOTAL', 'A', 'B', 'C', 'C2', 'C3', 'C4', 'C5', 'C6', 'C7', 'D', 'E','K','G','W', 'R', 'H','DOWN','RWK',], "PROD_NBR");
                                
              }

              Data.forEach((item) => {                   
        
               if(values['ShowMode']=="Prod_nbr"){
                    if (item.PROD_NBR.indexOf("subtotal")===-1) {
                        item['DOWN_GRADE_RATE']=item.DOWN/item.TOTAL== Infinity
                                    ? 0:(item.DOWN/item.TOTAL* 100).toFixed(2)+"%";
                    
                        }else{
                            item['DOWN_GRADE_RATE']=item.DOWN/(item.TOTAL)== Infinity
                                    ? 0:(item.DOWN/(item.TOTAL)* 100).toFixed(2)+"%";
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
               PageSetting.List = PageSetting.moreData
               //useCommonParams.AddAPIResultMessage(JSON.stringify(Message));
               //useCommonParamsALL.AddAPIResultMessage(ReportNameParams, JSON.stringify(Message));
               //useReportCommon.AddObject(ReportNameParams , useCommonParams)

               //useCommonParams.AddFormParams({ ...baseParams, ...values });
               //useCommonParamsALL.AddFormParams(ReportNameParams, { ...baseParams, ...values });
               //useReportCommon.AddObject(ReportNameParams , useCommonParams)

               ReportParamsItem.APIMessage = JSON.stringify(Message);
               ReportParamsItem.FormParams = { ...baseParams, ...values };
               useReportCommon.AddObject(ReportNameParams , ReportParamsItem)
                
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
//InfiniteScroll(DownGrade, PageSetting);
onMounted(async () => {
    //await mounted(optionsdata,useCommonParams.Fab) 
});

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
            regArr.push(dataSource[i][item].slice(0,9) == dataSource[i + 1][item].slice(0,9),dataSource[i][item].slice(0,9),dataSource[i + 1][item].slice(0,9))            
        })
       
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


