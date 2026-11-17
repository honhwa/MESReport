<template>
    <div class="StartBankStatusWip">
        <a-card>
            <!-- <a-row>
                <span class="menu-fold" @click="showHidden">
                    <component :is="!PageSetting.showmenu ? MenuUnfoldOutlined : getRowClassNameMenuFoldOutlined" />
                </span>
            </a-row> -->
            <a-row>
                <a-col :span="24 - PageParams.TableSpan" class="NGBScrollForm" v-show="PageSetting.showmenu">
                    <SchemaForm @submit="confirm"> </SchemaForm>                  
                    <a-button type="primary" size="small" class="newbutton" @click="confirm_Qtime($event, 'NoOverQtime')">未超Qtime</a-button>                    
                    <a-button type="primary" size="small" class="newbutton" @click="confirm_Qtime($event, 'OverQtime')">超Qtime</a-button>
                     
                </a-col>
                <a-col :span="PageParams.TableSpan">
                  <div class="ScrollTable" ref="StartBankStatusWip">
                    <DynamicTable showFavorite row-key="id" :header-title="PageSetting.HeadTitle" sticky
                        :columns="PageSetting.Columns" :key="PageSetting.Columns" bordered size="small"
                        :data-source="PageSetting.List" :pagination="false" 
                         :scroll="{ x: PageSetting.x,}"
                        :data-all="PageSetting.moreData"   :PageParams="PageParams"
                        export-file-name="导出" :row-class-name="getRowClassName">

                        <!-- <template #toolbar>
                                <a-button type="primary" @click="isShowExplain = !isShowExplain">{{isShowExplain?'显示表格':'显示说明文件'}}</a-button>
                            </template> -->
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
const {mountedLCDowner,mountedLCDGrade,InfiniteScroll} = useRemoteSelect()

const StartBankStatusWip = ref<HTMLElement | null>(null);
//let isShowExplain = ref<boolean>(false);
function getImport(site: any) {
    //const { optionsdata } = require('@/pages/ReportPortal/Static/NGBCommon-Form-schema');
    const { schemas,  getSchemaData } = require(`@/pages/ReportPortal/NGB/StartBankStatusWip/${bigfab}/form-schema`);
    return { schemas,  getSchemaData };
}
const { schemas,  getSchemaData } = getImport(useCommonParams.Fab);
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
let ReportNameParams = `StartBankStatusWip-${useCommonParams.Fab}`;
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
    x: '' as any,
});

const [DynamicTable] = useTable();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 130,
    schemas:schemas(useCommonParams.Fab,reactive({})),
    actionColOptions: {
        span: 24
    },
    //fieldMapToTime: [['fieldTime', ['vDate_s', 'vDate_e1'], 'YYYY-MM-DD']],
    layout:"vertical"
});

// function showHidden() {
//     PageSetting.showmenu = !PageSetting.showmenu;
//     if (PageSetting.showmenu) PageSetting.TableSpan = 20;
//     else PageSetting.TableSpan = 24;
// }

PageSetting.HeadTitle=`${useCommonParams.Fab} Start Bank Status Wip`

async function confirm(values) {
    openFullLoading();
      //table 初始化
    PageSetting.List=[];
    PageSetting.Columns=[];

    let baseParams = {name: "Date",apiName: "StartBankStatusDataSet",FactoryType:useCommonParams.Fab,G_FAC:useCommonParams.G_FAC_COM,QueryType:"Query",}
        
    Object.keys(values).forEach((key) => {      
        if (!values[key]) {
            values[key] = '';
        }
       
    });

   

   for (let i in dynamicFormRef?.formModel) {
        if (i == 'Lcd_Prod_nbr') values[i] = dynamicFormRef.formModel[i].split('\n');
        else {
            values[i] = dynamicFormRef.formModel[i];
        }
    }

          
    ReportItem.FormParams={ ...baseParams,...values }

    dynamicFormRef.validate().then(async () => {

        try{

            const { Date: { Data: { Table: Data},SQL01 } } = await GetCommon({...baseParams,...values});
            
            //console.log("StartBankStatusDataSet-SQLHidden:",SQL01)

             PageSetting.SQLHidden="StartBankStatusDataSet-SQLHidden:"+SQL01;

            if (Data.length != 0) {
                let ColumsData: any = [];
                let WidthPars=50;
                let titlePars="";//label
                let classNamePars="v-center";
                let middleProps: string[] = [];//API提供轉置后的數據

                for (let item in Data[0]) {  
                    if(item=="GRADE"){
                        WidthPars=50;
                        titlePars="Grade";
                    }else if(item=="LCD_PROD"){
                        WidthPars=110;
                        titlePars="Prod(PFCD)" ;               
                    }else if(item=="FLOOR"){
                        WidthPars=50;
                        titlePars="Floor";
                    }else if(item=="ABN_CODE"){
                        WidthPars=50;
                        titlePars="ABN Code";
                    }
                    else {
                        WidthPars=60;
                        titlePars=item.replaceAll('_', ' ')+""+"*";
                        middleProps.unshift(item); 
                    }
                    
                    ColumsData.push({ title: titlePars, align: 'center', dataIndex: item, className: classNamePars, width:WidthPars,
                        customCell: (record: Object, rowindex: number, column: Object) => {
                        
                        if (middleProps.includes(item)) {  
                            return {           
                                onDblclick: () => {  
                                // 是否跳轉到二級頁面                       
                                    console.log("跳转到第二层");

                                    ReportItem.FirstTableParams={ ...record, ...column['__originColumn__'] };
                                    router.push({ name: `StartBankStatusWipDetail-${useCommonParams.Fab}`});
                                    
                                
                                }, style: { cursor: 'pointer'}
                            }
                            } else {
                                
                                    if(item=="LCD_PROD"||item=="FLOOR"||item=="GRADE"){
                                        let text = record[item]
                                        let obj = {
                                            text,
                                            style: {},
                                            onDblclick: () => {
                                            message.warning('此栏位没有二阶数据')
                                        }
                                    }
                                        obj['rowSpan'] = record?.[`${item}-span`]?.['rowspan'] || 0
                                        return obj

                                    }else{                               
                                            return {
                                            onDblclick: () => {
                                            message.warning('此栏位没有二阶数据')
                                            }, style: { }
                                        }

                                    }

                                
                            }

                        }
                    
                    });
                }

                PageSetting.Columns = [
                    ...ColumsData,
                    {
                        title: '總計',
                        align: 'center',
                        dataIndex: 'Sum',
                        className:'',
                        width:50
                    }
                ];

                // 添加行总计
                getrowsum(Data, middleProps, "Sum");

            //添加列總計
                let pushdata = cloneDeep(Data); 
                let Sum_Column=middleProps;
                Sum_Column.unshift("Sum")
                let SUM_Sub = getSumData(Data.length, 0, Data, Sum_Column);
                //console.log("SUM_Sub",SUM_Sub)
                pushdata.splice(pushdata.length, 0, Object.assign(SUM_Sub, { GRADE: '合計' }));



                //第一次合併
                let combineCol1: any = {
                data: pushdata,
                mergeColNames: ['LCD_PROD','FLOOR','GRADE'],
                //firstMergeColNames: ['FLOOR'],
                firstMerge: ['LCD_PROD']
                };
                /**进行表格合并 */
                mergeTableRow(combineCol1);

            //第二次合併
                let combineCol2: any = {
                data: pushdata,
                mergeColNames: ['FLOOR','GRADE'],
                //firstMergeColNames: ['WORK_CTR', 'SIZE1'],
                firstMerge: 'FLOOR'
                };

                mergeTableRow(combineCol2);

                //第三次合併
                let combineCol3: any = {
                data: pushdata,
                mergeColNames: ['GRADE'],
                //firstMergeColNames: ['WORK_CTR', 'SIZE1'],
                firstMerge: 'GRADE'
                };

                PageSetting.moreData=[];

                let Mergedata = mergeTableRow(combineCol3);

                PageSetting.moreData = Mergedata;
                if (PageSetting.moreData.length > 1000) {
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

async function confirm_Qtime(values,QueryTypePars) {
    openFullLoading();
    let baseParams = {name: "Date",apiName: "StartBankQtimeDataSet",FactoryType:useCommonParams.Fab,G_FAC:useCommonParams.G_FAC_COM,QueryType:QueryTypePars,}
        
    Object.keys(values).forEach((key) => {      
        if (!values[key]) {
            values[key] = '';
        }
    });



   for (let i in dynamicFormRef?.formModel) {
        if (i == 'Lcd_Prod_nbr') values[i] = dynamicFormRef.formModel[i].split('\n');
        else {
            values[i] = dynamicFormRef.formModel[i];
        }
    }

    dynamicFormRef.validate().then(async () => {
         try{

            const { Date: { Data: { Table: Data},SQL01 } } = await GetCommon({...baseParams,...values});

            PageSetting.SQLHidden = SQL01
            //console.log(QueryTypePars+"-StartBankQtimeDataSet-SQLHidden:",SQL01)

            PageSetting.SQLHidden=QueryTypePars+"-StartBankQtimeDataSet-SQLHidden:"+SQL01;

            //useCommonParams.AddFormParams({...baseParams,...values});

            let ColumsData: any = [];
            let WidthPars=50;
            let titlePars="";//label
            let classNamePars="v-center";        

            for (let item in Data[0]) {  
                if(item=="GRADE_GRP"){
                    WidthPars=70;
                    titlePars="Grade_Group";
                    classNamePars="";    
                }else if(item=="LCD_PROD_NBR"){
                    WidthPars=120;
                    titlePars="LCD Product ID" ;  
                    classNamePars="v-center";                        
                }else if(item=="QTIME"){
                    WidthPars=120;
                    titlePars="QTIME";
                    classNamePars="";
                }else if(item=="CST_ID"){
                    WidthPars=100;
                    titlePars="Casset ID";
                    classNamePars="";
                }else if(item=="QTY"){
                    WidthPars=70;
                    titlePars="Panel Qty";
                    classNamePars="";
                }

                ColumsData.push({ title: titlePars, align: 'center', dataIndex: item, className: classNamePars, width:WidthPars});
                
            }

            PageSetting.moreData=[];
            PageSetting.Columns=ColumsData;
            PageSetting.moreData = Data;
            if (PageSetting.moreData.length > 1000) {
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

            closeFullLoading();

            }catch(error: any){ 
            closeFullLoading();
            return Promise.reject(error); 
        }
    });


}


//实现无限滚动或者说懒加载
InfiniteScroll(StartBankStatusWip, PageSetting);
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


.StartBankStatusWip :deep(.v-center) {
    background-color: #f0f0f0;
}

.StartBankStatusWip :deep(.Yellow) {
    background-color: #80ff80;
}

.StartBankStatusWip :deep(.YellowGreen) {
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
