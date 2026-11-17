<template>
    <div class="StartBankStatusWip">
        <a-card>
            <a-row>
                <a-col :span="24 - PageSetting.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                    <SchemaForm @submit="confirm"> </SchemaForm>                    
                    
                </a-col>
                <a-col :span="PageSetting.TableSpan">
                  <div class="TNScrollForm" ref="StartBankStatusWip">
                    <DynamicTable 
                        showFavorite
                        row-key="id" :header-title=PageSetting.HeadTitle
                        sticky
                        :columns="PageSetting.Columns" :key="PageSetting.Columns" bordered size="small"
                        :data-source="PageSetting.List" :pagination="false" 
                        :data-all="PageSetting.moreData" :explain="isShowExplain"
                        :row-class-name="getRowClassNmae"
                        :PageParams="PageParams"
                        export-file-name="Export" >
                        
                        <template #export-button>
                            <a-button type="success"> Export </a-button>
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
import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table'
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
import { mergeTableRow,getSumData,getRowClassNmae } from '@/pages/ReportPortal/utils/common'
import { AddCommonPageParams } from '@/store/module/commonpage';
import { ReportCommonWithOut } from '@/store/module/ReportCommon';
import useRemoteSelect from '@/pages/ReportPortal/utils/TNuseRemoteSelect';
import { AnyNaptrRecord } from 'dns';
import router from '@/router';
import { useLoading } from '@/components/feat/Loading';
import { cloneDeep } from 'lodash-es';
import { SetColumnsStyle } from '@/pages/ReportPortal/utils/common';
//import {optionsdata} from '@/pages/ReportPortal/Static/TNCommon-Form-schema';

const useCommonParams = AddCommonPageParams();
const useReportCommon = ReportCommonWithOut();
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
let ReportNameParams = `StartBankStatusDetail_${useCommonParams.Fab}`;
const {InfiniteScroll,mounted} = useRemoteSelect()

const StartBankStatusWip = ref<HTMLElement | null>(null);
let isShowExplain = ref<boolean>(false);
function getImport(site: any) {
    //const { optionsdata } = require('@/pages/ReportPortal/Static/TNCommon-Form-schema');
    const { schemas,  getSchemaData } = require(`@/pages/ReportPortal/${site}/StartBankStatusWip/MOD/form-schema`);
    return { schemas, getSchemaData };
}
const { schemas, getSchemaData } = getImport('TN');
const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});


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
let FactoryType: any = useCommonParams.Fab;
const [DynamicTable] = useTable();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas: schemas(FactoryType),
    actionColOptions: {
        span: 24
    },
    //fieldMapToTime: [['fieldTime', ['vDate_s', 'vDate_e1'], 'YYYY-MM-DD']],
    layout:"vertical"
});

function showHidden() {
    PageSetting.showmenu = !PageSetting.showmenu;
    if (PageSetting.showmenu) PageSetting.TableSpan = 20;
    else PageSetting.TableSpan = 24;
}

PageSetting.HeadTitle=`${useCommonParams.Fab} Start Bank Status Wip`

async function confirm(values) {
    openFullLoading();
    let baseParams = {name: "Date",apiName: "TN_StartBankStatusWip",FactoryType:useCommonParams.Fab,G_FAC:useCommonParams.G_FAC_COM,QueryType:"Query",}
        
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

    //useCommonParams.AddStartBankStatusFormParams({...baseParams,...values});
    

    dynamicFormRef.validate().then(async () => {

        try{

            const { Date: { Data: { Table: Data},SQL01 } } = await GetCommon({...baseParams,...values});

            PageSetting.SQLHidden = SQL01
            console.log("StartBankStatusDataSet-SQLHidden:",SQL01)
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
                    }else if(item=="CST_TYPE"){
                        WidthPars=110;
                        titlePars="CST Type" ;               
                    }else if(item=="ABN_CODE"){
                        WidthPars=110;
                        titlePars="ABN Code" ;               
                    }else if(item=="SUB"){
                        WidthPars=50;
                        titlePars="Size";
                    }
                    else {
                        WidthPars=60;
                        titlePars=item.replaceAll('_', ' ')+""+"*";
                        middleProps.unshift(item);                
                        classNamePars="pro_auto-center";
                    }
                    
                    ColumsData.push({ title: titlePars, align: 'center', dataIndex: item, className: classNamePars, width:WidthPars,
                        customCell: (record: Object, rowindex: number, column: Object) => {
                            
                        if (middleProps.includes(item)) {  
                            // if(column["dataIndex"] == "PROD 1000")
                            // {
                            //     if(record["PROD 1000"] < 10)
                            //     {
                            //         return {style: { 'background-color' : 'rgb(0,255,0)' }}
                            //     }
                            //     else if(record["PROD 1000"] > 10 && record["PROD 1000"] < 100)
                            //     {
                            //         return {style: { 'background-color' : 'rgb(255,255,0)' }}
                            //     }
                            //     else
                            //     {
                            //         return {style: { 'background-color' : 'rgb(255,150,150)' }}
                            //     }
                            // }
                            return {           
                                onDblclick: () => {             
                                //const UseParamsAdd = AddCommonPageParamsWithOut()  
                                // 是否跳轉到二級頁面                       
                                    console.log("跳转到第二层");
                                    //UseParamsAdd.AddStartBankStatusFirstTableParams({ ...record, ...column['__originColumn__'] })
                                    ReportParamsItem.FormParams = { ...baseParams, ...values };
                                    ReportParamsItem.FirstTableParams = { ...record, ...column['__originColumn__'] };
                                    useReportCommon.AddObject(ReportNameParams , ReportParamsItem)
                                    //useReportCommon.AddFirstTableParams(`StartBankStatusDetail_${useCommonParams.Fab}`,{ ...record, ...column['__originColumn__'] })
                                              
                                    router.push({ name: `StartBankStatusWipDetail_${useCommonParams.Fab}`});
                                    
                                
                                }, style: { cursor: 'pointer'}
                            }
                            } else {
                                
                                    if(item=="LCD_PROD"||item=="SUB"||item=="GRADE"){
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
                                            }, style: {}
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
                        className:'pro_auto-center',
                        width:50,
                        // customCell: (record: Object, rowindex: number, column: Object) => {
                        //     let style = SetColumnsStyle([700,900,1000,2000],['black','green','grey','red','yellow'],record['Sum']);
                        //     return style??{}
                        //         // if(record["Sum"] < 10)
                        //         // {
                        //         //     let AA = "'background-color' : 'rgb(0,255,0)'"
                        //         //     return {style: { 'background-color' : 'rgb(0,255,0)' }}
                        //         // }
                        //         // else if(record["Sum"] > 10 && record["Sum"] < 100)
                        //         // {
                        //         //     return {style: { 'background-color' : 'rgb(255,255,0)' }}
                        //         // }
                        //         // else
                        //         // {
                        //         //     return {style: { 'background-color' : 'rgb(255,150,150)' }}
                        //         // }
                        //     }

                    }
                ];

                // 添加行总计
                getrowsum(Data, middleProps, "Sum");

            //添加列總計
                let pushdata = cloneDeep(Data);    
                // let SUM_Sub = getSumData(Data.length, 0, Data, ColumsData);
                // pushdata.splice(pushdata.length, 0, Object.assign(SUM_Sub, { GRADE: '合計' }));



                //第一次合併
                let combineCol1: any = {
                data: pushdata,
                mergeColNames: ['SUB','GRADE','LCD_PROD','CST_TYPE','ABN_CODE'],
                //firstMergeColNames: ['FLOOR'],
                firstMerge: ['SUB']
                };
                /**进行表格合并 */
                mergeTableRow(combineCol1);

                //第二次合併
                let combineCol2: any = {
                data: pushdata,
                mergeColNames: ['GRADE','LCD_PROD','CST_TYPE','ABN_CODE'],
                //firstMergeColNames: ['WORK_CTR', 'SIZE1'],
                firstMerge: 'GRADE'
                };

                mergeTableRow(combineCol2);

                //第三次合併
                let combineCol3: any = {
                data: pushdata,
                mergeColNames: ['LCD_PROD','CST_TYPE','ABN_CODE'],
                //firstMergeColNames: ['WORK_CTR', 'SIZE1'],
                firstMerge: 'LCD_PROD'
                };

                PageSetting.moreData=[];

                let Mergedata = mergeTableRow(combineCol3);

                PageSetting.moreData = Mergedata;
                if (PageSetting.moreData.length > 100) {
                    PageSetting.List = PageSetting.moreData.slice(0, 50)
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
InfiniteScroll(StartBankStatusWip, PageSetting);
onMounted(
    async () => {
        //await mountedLCDowner(optionsdata, useCommonParams.Fab);
        //await mountedLCDGrade(optionsdata, useCommonParams.Fab); 
        //await mounted(optionsdata, useCommonParams.Fab);       
    }
)

</script>

<style lang="less" scoped>
.btn-rows {
    button {
        margin-right: 12px;
    }
}
:deep(.pro_auto-center){
    border: 1px solid black;
}

.StartBankStatusWip :deep(.v-center) {
    background-color: #f0f0f0;
    border: 1px solid black;
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
