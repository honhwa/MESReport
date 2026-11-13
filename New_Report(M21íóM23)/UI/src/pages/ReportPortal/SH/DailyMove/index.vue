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
                    Source-show="PageSetting.showmenu"
                >
                    <div class="SHScrollForm">
                        <SchemaForm @submit="confirm"> </SchemaForm>
                    </div>
                </a-col>
                <a-col :span="PageParams.TableSpan">                  
                    <div class="ScrollTable" ref="DaylimoveTable">
                        <DynamicTable sticky
                            showFavorite
                            :PageParams="PageParams"
                            row-key="id" 
                            :header-title="PageSetting.HeadTitle"
                            :columns="PageSetting.Columns"
                            :key="PageSetting.Columns"
                            bordered export-type="Exceljs"
                            :exceljsparams="PageSetting.exceljsparams"
                            size="small"
                             :scroll="{ x: PageSetting.x,}"
                            :data-source="PageSetting.List"
                            :data-all="PageSetting.moreData"
                            :pagination="false"
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
import useRemoteSelect from '@/pages/ReportPortal/utils/SHuseRemoteSelect';
import { useLoading } from '@/components/feat/Loading';
import {cloneDeep} from 'lodash-es';
import { getPovitData, PovitParams } from '@/utils/helper/povitHelper'
import { showHidden, getRowClassName,getSumData,mergeTableRow,getdiffdate, } from '@/pages/ReportPortal/utils/SHcommon'
import { AddCommonPageParams } from '@/store/module/commonpage';
// import { useUserStoreWithOut } from '@/store/module/user';
import { Console } from 'console';
import { message } from 'ant-design-vue';


let useCommonParams = AddCommonPageParams();
let bigfab=useCommonParams.Fab.slice(0,useCommonParams.Fab.length-1)

const DaylimoveTable = ref<HTMLElement | null>(null)
//vue3的mixin
const {  InfiniteScroll } = useRemoteSelect()
function getImport(site: any) {
   // let { optionsdata } = require('@/pages/ReportPortal/Static/SHCommon-Form-schema');
    const { schemas } = require(`@/pages/ReportPortal/SH/DailyMove/${bigfab}/form-schema`);
    const { baseColumns } = require(`@/pages/ReportPortal/SH/DailyMove/${bigfab}/Table`);
    return { schemas, baseColumns, };
}

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});

//let isShowExplain = ref<boolean>(false);
// 
//let repeatData = ['WORK_CTR', 'WO_NBR', 'LCM_OWNER', 'SIZE1', 'PROD_NBR', 'CUST_GRP','SO_NBR']

let { schemas, baseColumns,} = getImport(useCommonParams.Fab);

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
    exceljsparams: {
        Header: {}
    }

});
// let isLongData = ref(true)
PageSetting.HeadTitle=`${useCommonParams.Fab} Daily Move`
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
    let baseParams = {name: "Date",apiName: "DailyMove",FactoryType:useCommonParams.Fab,G_FAC:useCommonParams.G_FAC_COM,building:"",}

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
    
    if (values.Prod_nbr && values.Prod_nbr.length > 1000 )
    {
         closeFullLoading();
         return message.error('料號输入超過 “最大限制筆數:1000” '); 
    }
   
   //獲取數據綁定table
    dynamicFormRef.validate().then(async () => {

       try{
          const { Date: { Data: { Table: Data },sql } }  = await GetCommon({ ...baseParams, ...values });

          PageSetting.SQLHidden = "DailyMove-SQLHidden:"+sql
          //console.log("DailyMove-SQLHidden:",sql)
        
        // Data.map((item: any) => {
        //     if (!PageSetting.APIColumns.includes(item['PD'])) {
        //         PageSetting.APIColumns.push(item['PD']);
        //     }
        // });
        // let DateColumns: any = [];
        // PageSetting.APIColumns.map((item: any) => {
        //     DateColumns.push({
        //         title: item,
        //         align: 'center',
        //         dataIndex: item,
        //         className:'pro_auto-center'
        //     });
        // });
        // PageSetting.Columns = [
        //     ...baseColumns,
        //     ...DateColumns,
        //     {
        //         title: '总计',
        //         align: 'center',
        //         dataIndex: 'Total',
        //         className:'sum-center'
        //         //className:'v-center'
        //         // customRender: ({ record }) => {
        //         //     return <span>{getAllSumData(record)}</span>;
        //         // }
        //     }
        // ]; //vue3或者adv无法监测push,shift等方法

          if (Data.length != 0) { 

                //循環查詢data，獲取table Colums 數據
                let ColumsData: any = [];
                let WidthPars=50;
                let titlePars="";//label
                let classNamePars="v-center";
                let middleProps: string[] = [];//API提供轉置后的時間處理（例如：A2022_11_30）
                let fixedPara="left";
                

                for (let item in Data[0]) {  
                    if(item=="WORK_CTR"){
                        WidthPars=70;
                        titlePars="Operation" ;
                    }else if(item=="SIZE1"){
                        WidthPars=40;
                        titlePars="Size";
                    }else if(item=="LCM_OWNER"){
                        WidthPars=50;
                        titlePars="Owner" ;               
                    }else if(item=="PROD_NBR"){
                        WidthPars=105;
                        titlePars="Product";                
                    }else if(item=="WO_NBR"){
                        WidthPars=100;
                        titlePars="Work Oder";                
                    } else if(item=="CUST_GRP"){
                        WidthPars=70;
                        titlePars="Customer" ;               
                    }else if(item=="SO_NBR"){
                        WidthPars=70;
                        titlePars="需求單號" ;
                        fixedPara="";              
                    }else {
                        WidthPars=70;
                        if(item.slice(0,1)=="A"){
                        titlePars=item.slice(1).replaceAll('_', '-');
                        middleProps.unshift(item);
                         //console.log("middleProps",middleProps)
                         fixedPara="";

                        }else{
                        titlePars=item;
                        fixedPara="left"
                        } 
                        
                        classNamePars="";

                    }

                    ColumsData.push({ title: titlePars, align: 'center', dataIndex: item, className: classNamePars, width:WidthPars,fixed:fixedPara,
                    customCell: (record: Object, rowIndex: number, column: Object) => { 
                        if(item=="WORK_CTR"||item=="SIZE1"||item=="LCM_OWNER"){
                            let text = record[item]
                            let obj = {
                                text,
                                style: {}
                            }
                            obj['rowSpan'] = record?.[`${item}-span`]?.['rowspan'] || 0
                            return obj

                        }else if(item=="CUST_GRP"){
                            let text = record['CUST_GRP']
                            let obj = {
                                text,
                                style: {}
                            }
                            if (text == '小計' || text == '合計') obj['colSpan'] = 3
                            return obj
                        }               
                            
                        }
                    
                    });
                }

                //為加總提供要加總 計算的Columns
                PageSetting.APIColumns = [];
                PageSetting.APIColumns = middleProps;
                //console.log("PageSetting.APIColumns",PageSetting.APIColumns)

                PageSetting.Columns = [
                    ...ColumsData,
                    {
                        title: '總計',
                        align: 'center',
                        dataIndex: 'Total',
                        className:'sum-center',
                        width:60
                        //className:'v-center'
                        // customRender: ({ record }) => {
                        //     return <span>{getAllSumData(record)}</span>;
                        // }
                    }
                ]; //vue3或者adv无法监测push,shift等方法

                
                PageSetting.moreData = getTableData(Data)//进行表格合并

                if (PageSetting.moreData.length > 200) {
                    PageSetting.List = PageSetting.moreData.slice(0, 100)
                    // isLongData.value = true
                }//预先加载50条数据
                else {
                    PageSetting.List = PageSetting.moreData
                    // isLongData.value = false
                }

                 PageSetting.x = null;
                if (PageSetting.Columns.length > 7) {
                    PageSetting.x = 110 * PageSetting.Columns.length;
                }

                  // exceljs的表頭
                  let header = {
                    key: [] as any,
                    title: [] as any
                };

                PageSetting.Columns.forEach(item => {
                    if (item["children"]) {
                        item["children"].forEach(citem => {
                            header.key.push(citem["dataIndex"]);
                            header.title.push(citem["title"]);
                        })
                    } else {
                        header.key.push(item["dataIndex"]);
                        header.title.push(item["title"]);
                    }
               })

                PageSetting.exceljsparams.Header = header
          }else{

              message.warning("無數據！")

          }


            // let config: PovitParams = {
            //     Data,
            //     povitColumn: {
            //         PD: 'QTY'
            //     },
            //     changeColumn: PageSetting.APIColumns,
            //     repeatColumn: repeatData
            // }

            // let povitData = getPovitData(unref(config))//进行类行列转置的动作
            // PageSetting.moreData = getTableData(povitData)//进行表格合并
            // if (PageSetting.moreData.length > 100) {
            //     PageSetting.List = PageSetting.moreData.slice(0, 40)
            //     // isLongData.value = true
            // }//预先加载40条数据
            // else {
            //     PageSetting.List = PageSetting.moreData
            //     // isLongData.value = false
            // }
            //加載防護罩結束
            closeFullLoading();

       }catch(error: any){ 
           closeFullLoading();
           return Promise.reject(error); 
        }

    
    });
}


/**
 * 进行数据处理,将表格进行行列转置并进行表格合并
 * @Data 表格数据源
 */
function getTableData(Data: any[]) {

    /**表格合并所需参数
     * @data 数据源
     * @mergeColNames 需要合并的列
     * @firstMergeColNames 受到影响的列
     * @firstMerge 以哪列为基础进行合并
     */
    let combineCol1: any = {
        data: Data,
        mergeColNames: ['WORK_CTR', 'SIZE1', 'LCM_OWNER'],
        firstMergeColNames: ['WORK_CTR', 'SIZE1'],
        firstMerge: 'WORK_CTR'
    };
    /**进行表格合并 */
    let Mergedata = mergeTableRow(combineCol1);
  
    let { pushdata, AllSum } = getMergeData(Mergedata, 'SIZE1');

    pushdata.push(Object.assign({ CUST_GRP: '合计' }, getTotalData(AllSum)));
    pushdata.forEach(item =>
        item['Total'] = getAllSumData(item)//在渲染的实际数组中加入总计数据
    );

    let combineCol2 = {
        //二次合并
        data: pushdata,
        mergeColNames: ['WORK_CTR'],
        firstMergeColNames: ['WORK_CTR'],
        firstMerge: 'WORK_CTR'
    };
    return mergeTableRow(combineCol2);
}
function getAllSumData(row: any) {
    let sum = 0;
    PageSetting.APIColumns.map((item: any) => {
        row[item] = isNaN(row[item]) ? 0 : row[item];
        sum += row[item];
    });
    return sum;
}

function getMergeData(data: any[], params: string) {
    let pushdata = cloneDeep(data);
    let count = 0; //加总的数组的终点
    let count1 = 0; //加总的数组的起点
    let count2 = 0; //误差调整,要计算之前已经插入的小计行
    let AllSum: any = []; //合計
    for (let i in data) {
        //console.log("data",data)
        count += data[i][params + '-span']['rowspan']; //要加总几条数据
        if (data[i][params + '-span']['rowspan'] > 0) {
            const Sub = getSumData(count, count1, data,PageSetting.APIColumns);
            AllSum.push(Sub);            
            pushdata.splice(count + count2, 0, Object.assign({ WORK_CTR: data[i]['WORK_CTR'], CUST_GRP: '小計' }, Sub));
            count2 += 1; //记录插入过几条数据
            count1 += data[i][params + '-span']['rowspan']; //已加总的条数作为下一次加总的起点
        }
    }

    return { pushdata, AllSum };
}

function getTotalData(data: any[]) {
    //合計
    let obj = {};
    PageSetting.APIColumns.map((item: string) => {
        let sum = 0;
        data.map((n) => {
            sum += n[item];
        });
        obj[item] = sum;
    });
    return obj;
}
InfiniteScroll(DaylimoveTable, PageSetting)

//載入api 方法
onMounted(async () => {
    //optionsdata.OwnerName = await Getcommon(params) 
    // await mounted(optionsdata,useCommonParams.Fab)
    // await mountedLCMowner(optionsdata, useCommonParams.Fab);        
    // await mountedOper(optionsdata, useCommonParams.Fab);  
    // await mountedCustomer(optionsdata, useCommonParams.Fab);      
        // console.log("ppppppp",optionsdata.OwnerName)

    // optionsdata.OwnerName = await  getCommonFormParams('LCMOWNER',{
    //     name: "Date",
    //     apiName: "LcmOwner",
    //     FactoryType:useCommonParams.Fab,
    //     G_FAC: useCommonParams.G_FAC_COM,
    //     //G_FAC:'C'
    // })

    // optionsdata.OperationLabel = await  getCommonFormParams('OPER',{
    //     name: "Date",
    //     apiName: "Operation",
    //     FactoryType:useCommonParams.Fab,
    //     G_FAC: useCommonParams.G_FAC_COM
    // })

    // optionsdata.CustomDate = await  getCommonFormParams('CUSTOMER',{
    //     name: "Date",
    //     apiName: "Customer",
    //     FactoryType:useCommonParams.Fab,
    //     G_FAC: useCommonParams.G_FAC_COM
    // })
//console.log(optionsdata.OperationLabel);
    
    
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

.DayliMove :deep(.Source-center) {
    background-color: #f0f0f0;
}


.DayliMove :deep(.Yellow) {
    background-color: #80ff80;
}
:deep(.sum-center){
    border: 1px solid black;
}

.DaylimoveTable {
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
